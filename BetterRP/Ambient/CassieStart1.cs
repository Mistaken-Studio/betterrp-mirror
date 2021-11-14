// -----------------------------------------------------------------------
// <copyright file="CassieStart1.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.API.Features;

namespace Mistaken.BetterRP.Ambients
{
    internal class CassieStart1 : Ambient
    {
        public override int Id => 13;

        public override string Message => "pitch_0.9 WARNING . pitch_0.97 SYSTEM CORE CORRUPTION CRITICAL . SECURITY SOFTWARE STATUS UNKNOWN . NO REPORT FROM CONTAINMENT CHAMBERS DETECTED . . . . . . pitch_0.7 .g2 DO NOT ESCAPE . I FOUND YOU ALREADY .g6";

        public override bool IsJammed => false;

        public override bool CanPlay()
        {
            if (!base.CanPlay()) return false;
            if (Round.ElapsedTime.TotalSeconds > 60) return false;
            return true;
        }
    }
}
