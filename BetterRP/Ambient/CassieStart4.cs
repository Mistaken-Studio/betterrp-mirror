// -----------------------------------------------------------------------
// <copyright file="CassieStart4.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.API.Features;

namespace Mistaken.BetterRP.Ambients
{
    internal class CassieStart4 : Ambient
    {
        public override int Id => 16;

        public override string Message => "pitch_0.92 POWER LEVEL IS CRITICAL . EMERGENCY PROTOCOL ENGAGED";

        public override bool IsJammed => false;

        public override bool CanPlay()
        {
            if (!base.CanPlay()) return false;
            if (Round.ElapsedTime.TotalSeconds > 60) return false;
            return true;
        }
    }
}
