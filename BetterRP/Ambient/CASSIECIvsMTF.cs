// -----------------------------------------------------------------------
// <copyright file="CASSIECIvsMTF.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Exiled.API.Features;
using Mistaken.API;

namespace Mistaken.BetterRP.Ambients
{
    internal class CASSIECIvsMTF : Ambient
    {
        public override int Id => 11;

        public override string Message => "PITCH_0.2 .G4 PITCH_2 . PITCH_0.2 .G4 PITCH_0.98 ATTENTION . CASSIE JAM_020_5 SYSTEM PITCH_0.8 .G6 . NOW . UNDER . MILITARY . COMMAND . PITCH_0.2 .G4 PITCH_2 . PITCH_0.2 .G4 PITCH_0.9 ATTENTION . CHAOSINSURGENCY HASENTERED PITCH_0.2 .G3 PITCH_0.9 ALL CLASSD REPORT TO MILITARY FOR IMMEDIATE QUESTIONING PITCH_0.2 .G4 PITCH_2 . PITCH_0.2 .G4";

        public override bool IsJammed => false;

        public override bool CanPlay()
        {
            var tor = base.CanPlay();
            if (!tor) return false;
            if (RealPlayers.List.Where(p => p.Team == Team.CHI).Count() <= RealPlayers.List.Where(p => p.Team == Team.MTF).Count()) return false;
            BetterRPHandler.AmbientLock = true;
            API.Diagnostics.Module.CallSafeDelayed(
                120,
                () =>
                {
                    if (RealPlayers.List.Where(p => p.Team == Team.CHI).Count() < RealPlayers.List.Where(p => p.Team == Team.MTF).Count())
                        Cassie.Message("PITCH_0.2 .G4 PITCH_2 . PITCH_0.2 .G4 PITCH_0.8 ATTENTION . PITCH_0.7 .G6 PITCH_0.9 CASSIE JAM_018_5 SYSTEM .G6 . NOW . UNDER . FOUNDATION . COMMAND PITCH_0.1 .G3 PITCH_0.94 NEW OVERRIDE DETECTED .G6 ", false, false);
                    BetterRPHandler.AmbientLock = false;
                },
                "Ambients.CASSIECIvsMTF");
            return tor;
        }
    }
}
