// -----------------------------------------------------------------------
// <copyright file="AmbientHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MEC;
using Mistaken.API;
using Mistaken.API.Diagnostics;
using Mistaken.BetterRP.Ambients;

namespace Mistaken.BetterRP
{
    /// <inheritdoc/>
    public class AmbientHandler : Module
    {
        /// <inheritdoc/>
        public override string Name => "AmbientHandler";

        /// <inheritdoc/>
        public override void OnEnable()
        {
            Exiled.Events.Handlers.Server.RoundStarted += this.Server_RoundStarted;
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= this.Server_RoundStarted;
        }

        internal static readonly List<int> UsedAmbients = new List<int>();

        internal static bool AmbientLock { get; set; } = false;

        internal static string GetAmbient(out bool jammed, int id = -1)
            => GetAmbient(0, out jammed, id);

        internal AmbientHandler(PluginHandler plugin)
            : base(plugin)
        {
        }

        private const float DefaultChance = 10;

        private static readonly Ambient[] Ambients = new Ambient[]
        {
            new Spotted035(),
            new ClassEtoCheckpoint(),
            new MajorSciJuly(),
            new MajorSciDark(),
            new FacilityManager(),
            new Intruders(),
            new DoctorRJ(),
            new SCP008(),
            new SCP131A(),
            new SCP066(),
            new SCP538(),
            new CassieIni(),
            new CassieStart1(),
            new CassieStart2(),
            new CassieStart3(),
            new CassieStart4(),
            new CassieStart5(),
            new NinetailedfoxWait(),
            new NinetailedfoxTerminateChaos(),

            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
            new RandomAmbient(),
        };

        private static float chance = DefaultChance;

        private static string GetAmbient(int overflowId, out bool jammed, int id = -1)
        {
            jammed = true;
            if (overflowId > 100)
                return "CASSIE CRITICAL ERROR DETECTED";
            Ambient ambient = null;
            if (id == -1)
            {
                int random = UnityEngine.Random.Range(0, Ambients.Length);
                ambient = Ambients[random];
            }
            else
            {
                ambient = Ambients.First(item => item.Id == id);
                if (ambient == null) return GetAmbient(overflowId + 1, out jammed);
            }

            if (UsedAmbients.Contains(ambient.Id)) return GetAmbient(overflowId + 1, out jammed);
            if (!ambient.CanPlay()) return GetAmbient(overflowId + 1, out jammed);

            // else if (id == 100) return null;
            if (!ambient.IsReusable) UsedAmbients.Add(ambient.Id);
            jammed = ambient.IsJammed;
            return ambient.Message
                .Replace("$classd", RealPlayers.List.Where(p => p.Team == Team.CDP).Count().ToString())
                .Replace("$mtf", RealPlayers.List.Where(p => p.Team == Team.MTF).Count().ToString())
                .Replace("$ci", RealPlayers.List.Where(p => p.Team == Team.CHI).Count().ToString());
        }

        private void Server_RoundStarted()
        {
            this.RunCoroutine(this.DoAmbients(), "DoAmbients");
        }

        private IEnumerator<float> DoAmbients()
        {
            UsedAmbients.Clear();
            yield return Timing.WaitForSeconds(5);
            int rid = RoundPlus.RoundId;
            while (Round.IsStarted && rid == RoundPlus.RoundId)
            {
                if (UnityEngine.Random.Range(1, 101) <= chance && !AmbientLock)
                {
                    var msg = GetAmbient(out bool jammed);
                    if (msg != null)
                    {
                        while (Cassie.IsSpeaking)
                            yield return Timing.WaitForOneFrame;
                        if (jammed)
                            NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(msg, 0.1f, 0.07f);
                        else
                            Cassie.Message(msg, false, false);
                    }

                    chance -= 5;
                }
                else
                    chance = DefaultChance;

                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(120, 300));
            }
        }
    }
}
