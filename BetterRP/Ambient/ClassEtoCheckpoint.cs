// -----------------------------------------------------------------------
// <copyright file="ClassEtoCheckpoint.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Mistaken.BetterRP.Ambients
{
    internal class ClassEtoCheckpoint : Ambient
    {
        public override int Id => 1;

        public override string Message => "ATTENTION ALL CLASS E PERSONNEL IN SECTOR 7 .G2 PLEASE REPORT TO SECURITY CHECKPOINT GAMMA FOR JAM_024_3 QUESTIONING AND DECONTAMINATION";

        public override bool IsJammed => false;
    }
}
