// -----------------------------------------------------------------------
// <copyright file="CassieIni.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.API.Features;

namespace Mistaken.BetterRP.Ambients
{
    internal class CassieIni : Ambient
    {
        public override int Id => 12;

        public override string Message => "PITCH_0.2 .G4 PITCH_2 . PITCH_0.2 .G4 PITCH_0.8 CASSIESYSTEM INITIATED PITCH_0.2 .G4 PITCH_2 . PITCH_0.2 .G4 PITCH_2 . PITCH_0.9 analysis ON GOING";

        public override bool IsJammed => false;

        public override bool CanPlay()
        {
            if (!base.CanPlay()) return false;
            if (Round.ElapsedTime.TotalSeconds > 60) return false;
            return true;
        }
    }
}
