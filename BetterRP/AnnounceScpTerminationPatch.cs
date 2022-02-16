// -----------------------------------------------------------------------
// <copyright file="AnnounceScpTerminationPatch.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Exiled.API.Features;
using HarmonyLib;
using Mistaken.API.Diagnostics;
using PlayerStatsSystem;
using Respawning;

namespace Mistaken.BetterRP
{
    [HarmonyPatch(typeof(NineTailedFoxAnnouncer), nameof(NineTailedFoxAnnouncer.AnnounceScpTermination))]
    internal static class AnnounceScpTerminationPatch
    {
        public static bool Prefix(ReferenceHub scp, DamageHandlerBase hit)
        {
            NineTailedFoxAnnouncer.singleton.scpListTimer = 0f;
            Role curRole = scp.characterClassManager.CurRole;
            if (curRole.team != 0 || curRole.roleId == RoleType.Scp0492)
                return false;

            string announcement = hit.CassieDeathAnnouncement.Announcement;
            if (string.IsNullOrEmpty(announcement))
                return false;

            var ev = new Exiled.Events.EventArgs.AnnouncingScpTerminationEventArgs(Player.Get(scp), hit);
            Exiled.Events.Handlers.Map.OnAnnouncingScpTermination(ev);

            if (!ev.IsAllowed)
                return false;

            announcement = ev.TerminationCause;

            Module.CallSafeDelayed(
                5,
                () =>
                {
                    foreach (NineTailedFoxAnnouncer.ScpDeath scpDeath in NineTailedFoxAnnouncer.scpDeaths)
                    {
                        if (!(scpDeath.announcement != announcement))
                        {
                            scpDeath.scpSubjects.Add(scp.characterClassManager.CurRole);
                            return;
                        }
                    }

                    NineTailedFoxAnnouncer.scpDeaths.Add(new NineTailedFoxAnnouncer.ScpDeath
                    {
                        scpSubjects = new List<Role>(new Role[1]
                        {
                            curRole,
                        }),
                        announcement = announcement,
                        subtitleParts = hit.CassieDeathAnnouncement.SubtitleParts,
                    });
                },
                "DelaySCPDeathMessage");

            return false;
        }
    }
}
