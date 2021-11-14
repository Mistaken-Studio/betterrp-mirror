// -----------------------------------------------------------------------
// <copyright file="CassieStart3.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.API.Features;

namespace Mistaken.BetterRP.Ambients
{
    internal class CassieStart3 : Ambient
    {
        public override int Id => 15;

        public override string Message => "pitch_0.97 SHUTTING DOWN ALL SECONDARY SECURITY SYSTEMS . . pitch_0.6 .g2 .g4 . pitch_0.9 PROCEDURE SUCCESSFUL";

        public override bool IsJammed => false;

        public override bool CanPlay()
        {
            if (!base.CanPlay()) return false;
            if (Round.ElapsedTime.TotalSeconds > 60) return false;
            return true;
        }
    }
}
