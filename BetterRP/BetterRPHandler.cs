// -----------------------------------------------------------------------
// <copyright file="BetterRPHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Mistaken.API.Diagnostics;

namespace Mistaken.BetterRP
{
    /// <inheritdoc/>
    public class BetterRPHandler : Module
    {
        /// <summary>
        /// CI Announcments.
        /// </summary>
        public static readonly string[] CIAnnouncments = new string[]
        {
            "pitch_0.97 .g6 WARNING . DETECTED UNAUTHORIZED ENTRANCE ZONE SECURITY SYSTEM ACCESS ATTEMPT . POSSIBLE UNAUTHORIZED PERSONNEL IN THE FACILITY . KEEP CAUTION .g6",
            "pitch_0.97 ACCESS ANALYSIS . INITIATED . . . pitch_0.9 DANGER . pitch_0.97 UNKNOWN PERSONNEL DETECTED IN THE FACILITY . ALL FOUNDATION PERSONNEL REPORT TO ANY SAFE AREA IMMEDIATELY",
            "pitch_0.97 .g6 WARNING . NOT AUTHORIZED P A SYSTEM ACCESS ATTEMPT .g3 . . . PRIMARY SYSTEMS ARE UNDER ATTACK .g6",
            "pitch_0.97 ATTENTION ALL M T F UNITS . FACILITY IS UNDER ATTACK . RETREAT IMMEDIATELY",
            "pitch_0.97 SYSTEM SCAN INITIATED . . . . . DETECTED UNKNOWN SOFTWARE OVERRIDE . . . pitch_1.5 .g4 . . .g4 . . pitch_0.2 .g2 pitch_0.85 ATTENTION ALL FOUNDATION PERSONNEL . THIS P A SYSTEM IS NOW . UNDER . MILITARY . COMMAND2 pitch_0.1 .g1 pitch_0.85 SURRENDER IMMEDIATELY OR YOU WILL BE TERMINATED",
            "pitch_0.5 .g5 .g5 . . . . pitch_0.9 SYSTEM OVERRIDE .g4 . .g4 . .g6 SECURITY SYSTEMS DISENGAGED . . . pitch_0.3 .g1 pitch_0.9 INTERNAL ACCESS DEVICE DETECTED . PROCEED TO ENTRANCE ZONE IMMEDIATELY",
            "pitch_0.97 ALL SECURITY SYSTEMS DEACTIVATED . . . pitch_0.85 WARNING . EXECUTIVE SYSTEM ACCESS DENIED . SECURITY BREACH PROTOCOL IN EFFECT pitch_0.2 .g4 pitch_0.85 UNAUTHORIZED PERSONNEL IN THE FACILITY . CASSIE SYSTEM CORE LOCKDOWN IN PROGRESS pitch_0.2 .g6",
        };

        /// <inheritdoc/>
        public override string Name => "BetterRP";

        /// <inheritdoc/>
        public override void OnEnable()
        {
            Exiled.Events.Handlers.Player.ItemUsed += this.Player_ItemUsed;
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            Exiled.Events.Handlers.Player.ItemUsed -= this.Player_ItemUsed;
        }

        internal BetterRPHandler(PluginHandler plugin)
            : base(plugin)
        {
        }

        private void Player_ItemUsed(Exiled.Events.EventArgs.UsedItemEventArgs ev)
        {
            if (ev.Item.Type == ItemType.Medkit || ev.Item.Type == ItemType.SCP500)
            {
                ev.Player.DisableEffect(Exiled.API.Enums.EffectType.Bleeding);
                ev.Player.DisableEffect(Exiled.API.Enums.EffectType.Poisoned);
            }
        }
    }
}
