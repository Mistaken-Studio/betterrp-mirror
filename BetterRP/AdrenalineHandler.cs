// -----------------------------------------------------------------------
// <copyright file="AdrenalineHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
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
            Exiled.Events.Handlers.Player.UsedItem += this.Player_UsedItem;
            Exiled.Events.Handlers.Server.WaitingForPlayers += this.Server_WaitingForPlayers;
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            Exiled.Events.Handlers.Player.Hurting -= this.Player_Hurting;
            Exiled.Events.Handlers.Player.UsedItem -= this.Player_UsedItem;
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

        private void Player_UsedItem(Exiled.Events.EventArgs.UsedItemEventArgs ev)
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

            if (ev.Target.WillDie((StandardDamageHandler)ev.Handler.Base))
                return;

            switch (ev.Handler.Type)
            {
                case DamageType.Firearm:
                case DamageType.MicroHid:
                case DamageType.Explosion:
                case DamageType.Scp939:
                case DamageType.Scp0492:
                    {
                        if (!this.adrenalineNotReady.Contains(ev.Target) && ev.Attacker?.Role.Team != ev.Target.Role.Team)
                            this.CallDelayed(0.1f, () => this.ActivateAdrenaline(ev.Target), "Adrenaline");
                        return;
                    }
            }
        }

        private void ActivateAdrenaline(Player player)
        {
            player.SetGUI("adrenaline", PseudoGUIPosition.BOTTOM, "You feel <color=yellow>adrenaline</color> hitting", 5);
            player.EnableEffect<CustomPlayerEffects.Invigorated>(15, true);
            var movementBoost = player.GetEffect(EffectType.MovementBoost);
            var oldMovementBoostIntensityValue = movementBoost.Intensity;
            var oldMovementBoostDurationValue = movementBoost.Duration;
            player.ChangeEffectIntensity(EffectType.MovementBoost, 10, 5);
            ((AhpStat)player.ReferenceHub.playerStats.StatModules[1])._activeProcesses.Add(new AhpStat.AhpProcess(10, 10, 1, 1, 5, false));
            this.CallDelayed(
                6,
                () =>
                {
                    if (!player.IsConnected)
                        return;

                    if (oldMovementBoostDurationValue > 0)
                        player.EnableEffect<CustomPlayerEffects.MovementBoost>(oldMovementBoostDurationValue, true);
                    if (oldMovementBoostIntensityValue > 0)
                        player.ChangeEffectIntensity<CustomPlayerEffects.MovementBoost>(oldMovementBoostIntensityValue);
                },
                "Restore");
            this.adrenalineNotReady.Add(player);
            this.CallDelayed(90, () => this.adrenalineNotReady.Remove(player), "Ready Adrenaline");
        }
    }
}
