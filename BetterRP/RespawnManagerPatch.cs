// -----------------------------------------------------------------------
// <copyright file="RespawnManagerPatch.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using HarmonyLib;
using Respawning;

namespace Mistaken.BetterRP
{
    [HarmonyPatch(typeof(RespawnEffectsController), "ServerExecuteEffects")]
    internal static class RespawnManagerPatch
    {
        public static bool Prefix(RespawnEffectsController.EffectType type, SpawnableTeamType team)
        {
            if ((PluginHandler.Instance.Config.DisableCIDrums || PluginHandler.Instance.Config.CIEntryMessage) && type == RespawnEffectsController.EffectType.UponRespawn && team == SpawnableTeamType.ChaosInsurgency)
            {
                if (PluginHandler.Instance.Config.CIEntryMessage)
                {
                    if (UnityEngine.Random.Range(1, 101) < 25)
                        Exiled.API.Features.Cassie.Message(BetterRPHandler.CIAnnouncments[UnityEngine.Random.Range(0, BetterRPHandler.CIAnnouncments.Length)]);
                }

                return false;
            }

            return true;
        }
    }
}
