// -----------------------------------------------------------------------
// <copyright file="CassieStart5.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.API.Features;

namespace Mistaken.BetterRP.Ambients
{
    internal class CassieStart5 : Ambient
    {
        public override int Id => 17;

        public override string Message => "pitch_0.97 FACILITY ANALYSIS . INITIATED . . . PITCH_0.2 .G6 pitch_0.97 WARNING . ALL SECONDARY SECURITY SYSTEMS DISENGAGED . POSSIBLE CONTAINMENT BREACH IN PROGRESS . PRIMARY SYSTEMS POWER LEVEL IS CRITICAL . ENABLE ALL EMERGENCY GENERATORS IMMEDIATELY";

        public override bool IsJammed => false;

        public override bool CanPlay()
        {
            if (!base.CanPlay()) return false;
            if (Round.ElapsedTime.TotalSeconds > 60) return false;
            return true;
        }
    }
}
