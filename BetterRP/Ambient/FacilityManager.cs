// -----------------------------------------------------------------------
// <copyright file="FacilityManager.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Exiled.API.Features;

namespace Mistaken.BetterRP.Ambients
{
    internal class FacilityManager : Ambient
    {
        public override int Id => 4;

        public override string Message => "ATTENTION PITCH_0.76 .G4 PITCH_0.97 TACTICAL TEAM OMEGA 1 . FACILITY MANAGER LOCATION UNKNOWN .G4 LOCATE HER AND EVACUATE TO GATE B PITCH_0.1 .G3";

        public override bool IsJammed => false;

        public override bool CanPlay()
        {
            var tor = base.CanPlay();
            if (!tor)
                return tor;
            AmbientHandler.AmbientLock = true;
            API.Diagnostics.Module.CallSafeDelayed(
                120,
                () =>
                {
                    Cassie.Message(".G5 PITCH_0.84 .G2 PITCH_0.98 FACILITY MANAGER FOUND DEAD IN INTERSECTION C 2 .G4 PITCH_0.95 TACTICAL TEAM OMEGA 1 REPORT TO SECURITY CHECKPOINT 5 JAM_020_3 IMMEDIATELY", false, false);
                    AmbientHandler.AmbientLock = false;
                },
                "Ambients.FacilityManager");
            return tor;
        }
    }
}
