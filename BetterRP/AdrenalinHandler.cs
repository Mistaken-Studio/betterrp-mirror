// -----------------------------------------------------------------------
// <copyright file="AdrenalinHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MEC;
using Mistaken.API;
using Mistaken.API.Diagnostics;
using Mistaken.API.Extensions;
using Mistaken.API.GUI;

namespace Mistaken.BetterRP
{
    /// <inheritdoc/>
    public class AdrenalinHandler : Module
    {
        /// <inheritdoc/>
        public override string Name => "AdrenalinHandler";

        /// <inheritdoc/>
        public override void OnEnable()
        {
            Exiled.Events.Handlers.Player.Hurting += this.Player_Hurting;
            Exiled.Events.Handlers.Player.ItemUsed += this.Player_ItemUsed;
            Exiled.Events.Handlers.Server.WaitingForPlayers += Server_WaitingForPlayers;
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            Exiled.Events.Handlers.Player.Hurting -= this.Player_Hurting;
            Exiled.Events.Handlers.Player.ItemUsed -= this.Player_ItemUsed;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= Server_WaitingForPlayers;
        }

        internal AdrenalinHandler(PluginHandler plugin)
            : base(plugin)
        {
        }

        private readonly HashSet<Player> adrenalineNotReady = new HashSet<Player>();

        private void Server_WaitingForPlayers()
        {
            adrenalineNotReady.Clear();
        }

        private void Player_ItemUsed(Exiled.Events.EventArgs.UsedItemEventArgs ev)
        {
            if (ev.Player == null)
                return;
            if (ev.Item.Type == ItemType.Medkit || ev.Item.Type == ItemType.SCP500)
            {
                if (this.adrenalineNotReady.Contains(ev.Player))
                    this.adrenalineNotReady.Remove(ev.Player);
            }
        }

        private void Player_Hurting(Exiled.Events.EventArgs.HurtingEventArgs ev)
        {
            if (!ev.Target.IsHuman)
                return;

            if (ev.Amount >= ev.Target.Health + (ev.Target.ArtificialHealth * ev.Target.ReferenceHub.playerStats.ArtificialNormalRatio))
                return;

            if (
                ev.DamageType == DamageTypes.Com15 ||
                ev.DamageType == DamageTypes.E11SR ||
                ev.DamageType == DamageTypes.Grenade ||
                ev.DamageType == DamageTypes.AK ||
                ev.DamageType == DamageTypes.Shotgun ||
                ev.DamageType == DamageTypes.Revolver ||
                ev.DamageType == DamageTypes.Logicer ||
                ev.DamageType == DamageTypes.MicroHID ||
                ev.DamageType == DamageTypes.FSP9 ||
                ev.DamageType == DamageTypes.CrossVec ||
                ev.DamageType == DamageTypes.Scp0492 ||
                ev.DamageType == DamageTypes.Scp939 ||
                ev.DamageType == DamageTypes.Com18)
            {
                if (!this.adrenalineNotReady.Contains(ev.Target) && ev.Attacker?.Team != ev.Target.Team)
                    this.CallDelayed(0.1f, () => ActivateAdrenalin(ev.Target), "Adrenalin");
            }
        }

        private void ActivateAdrenalin(Player player)
        {
            player.SetGUI("adrenalin", PseudoGUIPosition.BOTTOM, "You feel <color=yellow>adrenaline</color> hitting", 5);
            player.EnableEffect<CustomPlayerEffects.Invigorated>(15, true);
            var cola = player.GetEffect(Exiled.API.Enums.EffectType.Scp207);
            var oldColaIntensityValue = cola.Intensity;
            var oldColaDurationValue = cola.Duration;
            player.EnableEffect<CustomPlayerEffects.Scp207>(5, true);
            player.ArtificialHealth += 7;
            this.CallDelayed(
                6,
                () =>
                {
                    if (!player.IsConnected)
                        return;

                    if (oldColaIntensityValue > 0)
                        player.ChangeEffectIntensity<CustomPlayerEffects.Scp207>(oldColaIntensityValue);
                },
                "Restore");
            this.adrenalineNotReady.Add(player);
            this.CallDelayed(90, () => this.adrenalineNotReady.Remove(player), "Ready Adrenalin");
        }
    }
}
