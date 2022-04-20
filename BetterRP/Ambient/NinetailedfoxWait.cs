// -----------------------------------------------------------------------
// <copyright file="NinetailedfoxWait.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Mistaken.API;

namespace Mistaken.BetterRP.Ambients
{
    internal class NinetailedfoxWait : Ambient
    {
        public override int Id => 18;

        public override string Message => "PITCH_0.97 .g6 .g4 COMMAND TO NINETAILEDFOX . YOUR ONLY TASK NOW IS TO WAIT FOR BACKUP .g6 .g4";

        public override bool IsJammed => false;

        public override bool CanPlay()
        {
            if (base.CanPlay() == false)
                return false;
            return RealPlayers.List.Where(p => p.Role.Team == Team.MTF).Count() <= 2 && RealPlayers.List.Where(p => p.Role.Team == Team.SCP).Count() + RealPlayers.List.Where(p => p.Role.Team == Team.CHI).Count() > 10;
        }
    }
}
