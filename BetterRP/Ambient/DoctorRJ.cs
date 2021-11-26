// -----------------------------------------------------------------------
// <copyright file="DoctorRJ.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.API.Features;

namespace Mistaken.BetterRP.Ambients
{
    internal class DoctorRJ : Ambient
    {
        public override int Id => 6;

        public override string Message => "ATTENTION . DOCTOR R J REPORT .G4 TO SECURITY CHECKPOINT THETA FOR IMMEDIATE JAM_020_2 QUESTIONING PITCH_0.1 .G3";

        public override bool IsJammed => false;

        public override bool CanPlay()
        {
            var tor = base.CanPlay();
            if (!tor) return tor;
            AmbientHandler.AmbientLock = true;
            API.Diagnostics.Module.CallSafeDelayed(
                120,
                () =>
                {
                    Cassie.Message("ATTENTION . SECURITY FORCE NATO_E 6 . SERPENTS HAND DETECTED . DOCTOR R J DESIGNATED FOR IMMEDIATE TERMINATION .G3 PITCH_0.1 .G3", false, false);
                    AmbientHandler.AmbientLock = false;
                },
                "Ambients.DoctorRJ");
            return tor;
        }
    }
}
