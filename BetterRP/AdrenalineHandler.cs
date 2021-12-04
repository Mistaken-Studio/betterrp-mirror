// -----------------------------------------------------------------------
// <copyright file="AdrenalineHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Mistaken.API.Diagnostics;
using Mistaken.API.Extensions;
using Mistaken.API.GUI;
using PlayerStatsSystem;

namespace Mistaken.BetterRP
{
    /// <inheritdoc/>
    public class AdrenalineHandler : Module
    {
        /// <inheritdoc/>
        public override string Name => "AdrenalinHandler";

        /// <inheritdoc/>
        public override void OnEnable()
        {
            Exiled.Events.Handlers.Player.Hurting += this.Player_Hurting;
            Exiled.Events.Handlers.Player.ItemUsed += this.Player_ItemUsed;
            Exiled.Events.Handlers.Server.WaitingForPlayers += this.Server_WaitingForPlayers;
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            Exiled.Events.Handlers.Player.Hurting -= this.Player_Hurting;
            Exiled.Events.Handlers.Player.ItemUsed -= this.Player_ItemUsed;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= this.Server_WaitingForPlayers;
        }

        internal AdrenalineHandler(PluginHandler plugin)
            : base(plugin)
        {
        }

        private readonly HashSet<Player> adrenalineNotReady = new HashSet<Player>();

        private void Server_WaitingForPlayers()
        {
            this.adrenalineNotReady.Clear();
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

            if (ev.Amount >= ev.Target.Health + (ev.Target.ArtificialHealth * ((AhpStat)ev.Target.ReferenceHub.playerStats.StatModules[1])._activeProcesses.LastOrDefault().Efficacy))
                return;

            switch (ev.Handler.Type)
            {
                case Exiled.API.Enums.DamageType.Firearm:
                case Exiled.API.Enums.DamageType.MicroHid:
                case Exiled.API.Enums.DamageType.Explosion:
                    {
                        if (!this.adrenalineNotReady.Contains(ev.Target) && ev.Attacker?.Team != ev.Target.Team)
                            this.CallDelayed(0.1f, () => this.ActivateAdrenaline(ev.Target), "Adrenaline");
                        return;
                    }

                case Exiled.API.Enums.DamageType.Scp:
                    {
                        switch (ev.Attacker.Role)
                        {
                            case RoleType.Scp93953:
                            case RoleType.Scp93989:
                            case RoleType.Scp0492:
                                {
                                    if (!this.adrenalineNotReady.Contains(ev.Target) && ev.Attacker?.Team != ev.Target.Team)
                                        this.CallDelayed(0.1f, () => this.ActivateAdrenaline(ev.Target), "Adrenaline");
                                    return;
                                }
                        }

                        return;
                    }
            }
        }

        private void ActivateAdrenaline(Player player)
        {
            player.SetGUI("adrenaline", PseudoGUIPosition.BOTTOM, "You feel <color=yellow>adrenaline</color> hitting", 5);
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
            this.CallDelayed(90, () => this.adrenalineNotReady.Remove(player), "Ready Adrenaline");
        }
    }
}
