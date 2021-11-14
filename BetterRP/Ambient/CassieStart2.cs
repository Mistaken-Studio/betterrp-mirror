// -----------------------------------------------------------------------
// <copyright file="CassieStart2.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.API.Features;

namespace Mistaken.BetterRP.Ambients
{
    internal class CassieStart2 : Ambient
    {
        public override int Id => 14;

        public override string Message => "pitch_0.97 START SYSTEM SCAN . . . pitch_0.8 WARNING pitch_0.97 . DETECTED CRITICAL SYSTEM ERROR . ALL SECONDARY SECURITY SYSTEMS WILL SHUT DOWN IN T MINUS 1 MINUTE . POTENTIAL OF CONTAINMENT BREACH IS VERY HIGH . FIND SHELTER NOW pitch_0.6 .g6 .g4";

        public override bool IsJammed => false;

        public override bool CanPlay()
        {
            if (!base.CanPlay()) return false;
            if (Round.ElapsedTime.TotalSeconds > 60) return false;
            return true;
        }
    }
}
