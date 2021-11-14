// -----------------------------------------------------------------------
// <copyright file="Intruders.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Mistaken.BetterRP.Ambients
{
    internal class Intruders : Ambient
    {
        public override int Id => 5;

        public override string Message => "WARNING . INTRUDERS DETECTED IN SECTOR 2 . INTERSECTION A 8 .G4 TEAM NATO_H 1 NEUTRALIZE TARGETS IMMEDIATELY";

        public override bool IsJammed => false;
    }
}
