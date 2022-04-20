// -----------------------------------------------------------------------
// <copyright file="NinetailedfoxTerminateChaos.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Mistaken.API;

namespace Mistaken.BetterRP.Ambients
{
    internal class NinetailedfoxTerminateChaos : Ambient
    {
        public override int Id => 19;

        public override string Message => "ATTENTION ALL NINETAILEDFOX . NEW SECONDARY TASK . . STAND BY . . DETECTED CHAOSINSURGENCY IN FACILITY . ALL NINETAILEDFOX PRIMARY TASK NOW IS TO TERMINATE ALL CHAOSINSURGENCY";

        public override bool IsJammed => true;

        public override bool CanPlay()
        {
            if (base.CanPlay() == false)
                return false;
            return RealPlayers.List.Where(p => p.Role.Team == Team.MTF).Count() > 5 && RealPlayers.List.Where(p => p.Role.Team == Team.CHI).Count() > 10;
        }
    }
}
