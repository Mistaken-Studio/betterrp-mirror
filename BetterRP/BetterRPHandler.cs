// -----------------------------------------------------------------------
// <copyright file="BetterRPHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MEC;
using Mistaken.API;
using Mistaken.API.Diagnostics;
using Mistaken.API.Extensions;
using Mistaken.API.GUI;
using Mistaken.BetterRP.Ambients;

namespace Mistaken.BetterRP
{
    public class BetterRPHandler : Module
    {
        internal BetterRPHandler(PluginHandler plugin)
            : base(plugin)
        {
        }

        public override string Name => "BetterRP";

        public override void OnEnable()
        {
            Exiled.Events.Handlers.Server.RoundEnded += this.Server_RoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted += this.Server_RoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += this.Server_WaitingForPlayers;
            Exiled.Events.Handlers.Player.ChangingRole += this.Player_ChangingRole;
            Exiled.Events.Handlers.Player.Hurting += this.Player_Hurting;
            Exiled.Events.Handlers.Player.ItemUsed += this.Player_ItemUsed;
        }

        public override void OnDisable()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= this.Server_RoundEnded;
            Exiled.Events.Handlers.Server.RoundStarted -= this.Server_RoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= this.Server_WaitingForPlayers;
            Exiled.Events.Handlers.Player.ChangingRole -= this.Player_ChangingRole;
            Exiled.Events.Handlers.Player.Hurting -= this.Player_Hurting;
            Exiled.Events.Handlers.Player.ItemUsed -= this.Player_ItemUsed;
        }

        public static readonly string[] CIAnnouncments = new string[]
        {
            "pitch_0.97 .g6 WARNING . DETECTED UNAUTHORIZED ENTRANCE ZONE SECURITY SYSTEM ACCESS ATTEMPT . POSSIBLE UNAUTHORIZED PERSONNEL IN THE FACILITY . KEEP CAUTION .g6",
            "pitch_0.97 ACCESS ANALYSIS . INITIATED . . . pitch_0.9 DANGER . pitch_0.97 UNKNOWN PERSONNEL DETECTED IN THE FACILITY . ALL FOUNDATION PERSONNEL REPORT TO ANY SAFE AREA IMMEDIATELY",
            "pitch_0.97 .g6 WARNING . NOT AUTHORIZED P A SYSTEM ACCESS ATTEMPT .g3 . . . PRIMARY SYSTEMS ARE UNDER ATTACK .g6",
            "pitch_0.97 ATTENTION ALL M T F UNITS . FACILITY IS UNDER ATTACK . RETREAT IMMEDIATELY",
            "pitch_0.97 SYSTEM SCAN INITIATED . . . . . DETECTED UNKNOWN SOFTWARE OVERRIDE . . . pitch_1.5 .g4 . . .g4 . . pitch_0.2 .g2 pitch_0.85 ATTENTION ALL FOUNDATION PERSONNEL . THIS P A SYSTEM IS NOW . UNDER . MILITARY . COMMAND2 pitch_0.1 .g1 pitch_0.85 SURRENDER IMMEDIATELY OR YOU WILL BE TERMINATED",
            "pitch_0.5 .g5 .g5 . . . . pitch_0.9 SYSTEM OVERRIDE .g4 . .g4 . .g6 SECURITY SYSTEMS DISENGAGED . . . pitch_0.3 .g1 pitch_0.9 INTERNAL ACCESS DEVICE DETECTED . PROCEED TO ENTRANCE ZONE IMMEDIATELY",
            "pitch_0.97 ALL SECURITY SYSTEMS DEACTIVATED . . . pitch_0.85 WARNING . EXECUTIVE SYSTEM ACCESS DENIED . SECURITY BREACH PROTOCOL IN EFFECT pitch_0.2 .g4 pitch_0.85 UNAUTHORIZED PERSONNEL IN THE FACILITY . CASSIE SYSTEM CORE LOCKDOWN IN PROGRESS pitch_0.2 .g6",
        };

        internal static readonly List<int> UsedAmbients = new List<int>();

        internal static bool AmbientLock { get; set; } = false;

        internal static string GetAmbient(out bool jammed, int id = -1)
            => GetAmbient(0, out jammed, id);

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

        private readonly Dictionary<int, List<string>> healthEffects = new Dictionary<int, List<string>>();
        private readonly List<int> adrenalineNotReady = new List<int>();

        private void Player_ItemUsed(Exiled.Events.EventArgs.UsedItemEventArgs ev)
        {
            if (ev.Player == null)
                return;
            if (ev.Item.Type == ItemType.Medkit || ev.Item.Type == ItemType.SCP500)
            {
                var pec = ev.Player.ReferenceHub.playerEffectsController;
                pec.DisableEffect<CustomPlayerEffects.Poisoned>();
                pec.DisableEffect<CustomPlayerEffects.Bleeding>();
                if (this.adrenalineNotReady.Contains(ev.Player.Id))
                    this.adrenalineNotReady.Remove(ev.Player.Id);
            }
        }

        private void Player_Hurting(Exiled.Events.EventArgs.HurtingEventArgs ev)
        {
            if (!ev.Target.IsHuman)
                return;
            if (UnityEngine.Random.Range(0, 100) < ev.Amount / 5)
            {
                if (ev.DamageType == DamageTypes.Scp0492)
                {
                    if (ev.Amount < ev.Target.Health + ev.Target.ArtificialHealth)
                        ev.Target.EnableEffect<CustomPlayerEffects.Poisoned>();
                }
            }

            if (ev.HitInformation.Tool == DamageTypes.Bleeding)
                ev.Amount *= 0.45f;
            if (ev.Amount >= ev.Target.Health + ev.Target.ArtificialHealth)
                return;
            if (
                ev.DamageType == DamageTypes.Com15 ||
                ev.DamageType == DamageTypes.E11SR ||
                ev.DamageType == DamageTypes.Grenade ||
                ev.DamageType == DamageTypes.AK ||
                ev.DamageType == DamageTypes.Shotgun ||
                ev.DamageType == DamageTypes.Revolver ||
                ev.DamageType == DamageTypes.Logicer ||
                ev.DamageType == DamageTypes.MicroHID ||
                ev.DamageType == DamageTypes.FSP9 ||
                ev.DamageType == DamageTypes.CrossVec ||
                ev.DamageType == DamageTypes.Scp0492 ||
                ev.DamageType == DamageTypes.Scp939 ||
                ev.DamageType == DamageTypes.Com18)
            {
                if (!this.adrenalineNotReady.Contains(ev.Target.Id) && ev.Attacker?.Team != ev.Target?.Team)
                {
                    this.CallDelayed(
                        0.1f,
                        () =>
                        {
                            ev.Target.SetGUI("adrenalin", PseudoGUIPosition.BOTTOM, "You feel <color=yellow>adrenaline</color> hitting", 5);
                            var pec = ev.Target.ReferenceHub.playerEffectsController;
                            var invigorated = pec.GetEffect<CustomPlayerEffects.Invigorated>();
                            var oldInvigoratedIntensityValue = invigorated.Intensity;
                            var oldInvigoratedDurationValue = invigorated.Duration;
                            pec.EnableEffect<CustomPlayerEffects.Invigorated>(5, true);
                            var cola = pec.GetEffect<CustomPlayerEffects.Scp207>();
                            var oldColaIntensityValue = cola.Intensity;
                            var oldColaDurationValue = cola.Duration;
                            pec.EnableEffect<CustomPlayerEffects.Scp207>(5, true);
                            ev.Target.ArtificialHealth += 7;
                            if (cola.Intensity < 1)
                                cola.Intensity = 1;
                            this.CallDelayed(
                                6,
                                () =>
                                {
                                    if (!ev.Target.IsConnected)
                                        return;
                                    if (oldInvigoratedIntensityValue > 0)
                                    {
                                        ev.Target.EnableEffect<CustomPlayerEffects.Invigorated>(oldInvigoratedDurationValue);
                                        pec.ChangeEffectIntensity<CustomPlayerEffects.Invigorated>(oldInvigoratedIntensityValue);
                                    }

                                    if (oldColaIntensityValue > 0)
                                    {
                                        ev.Target.EnableEffect<CustomPlayerEffects.Scp207>(oldColaDurationValue);
                                        pec.ChangeEffectIntensity<CustomPlayerEffects.Scp207>(oldColaIntensityValue);
                                    }
                                },
                                "Restore");
                        },
                        "Adrenalin");
                    this.adrenalineNotReady.Add(ev.Target.Id);
                    this.CallDelayed(90, () => this.adrenalineNotReady.Remove(ev.Target.Id), "Ready Adrenalin");
                }
            }

            if (
                ev.DamageType == DamageTypes.Com15 ||
                ev.DamageType == DamageTypes.E11SR ||
                ev.DamageType == DamageTypes.Grenade ||
                ev.DamageType == DamageTypes.AK ||
                ev.DamageType == DamageTypes.Shotgun ||
                ev.DamageType == DamageTypes.Revolver ||
                ev.DamageType == DamageTypes.Logicer ||
                ev.DamageType == DamageTypes.FSP9 ||
                ev.DamageType == DamageTypes.CrossVec ||
                ev.DamageType == DamageTypes.Scp939)
            {
                if (UnityEngine.Random.Range(0, 101) < ev.Amount / 5)
                    ev.Target.EnableEffect<CustomPlayerEffects.Bleeding>();
            }
            else if (ev.DamageType == DamageTypes.Falldown)
            {
                var pec = ev.Target.ReferenceHub.playerEffectsController;
                var rand = UnityEngine.Random.Range(0, 101);
                if (rand < (ev.Amount - 50) / 5)
                {
                    ev.Target.EnableEffect<CustomPlayerEffects.Bleeding>();
                    ev.Target.EnableEffect<CustomPlayerEffects.Ensnared>();
                    ev.Target.SetGUI("broken_legs", PseudoGUIPosition.MIDDLE, "Złamałeś obie nogi i <color=yellow>nie</color> możesz chodzić", 5);
                }
                else if (rand < ev.Amount / 5)
                {
                    ev.Target.EnableEffect<CustomPlayerEffects.Bleeding>();
                    ev.Target.EnableEffect<CustomPlayerEffects.Disabled>();
                }
            }
        }

        private IEnumerator<float> DoHeathEffects()
        {
            yield return Timing.WaitForSeconds(1);
            while (Round.IsStarted)
            {
                foreach (var player in RealPlayers.List)
                {
                    if (this.healthEffects.TryGetValue(player.Id, out List<string> effects))
                    {
                        foreach (var effect in effects.ToArray())
                        {
                            switch (effect)
                            {
                                case "Disabled":
                                    player.DisableEffect<CustomPlayerEffects.Disabled>();
                                    break;
                                case "Deafened":
                                    player.DisableEffect<CustomPlayerEffects.Deafened>();
                                    break;
                                case "Concussed":
                                    player.DisableEffect<CustomPlayerEffects.Concussed>();
                                    break;
                                case "Blinded":
                                    player.DisableEffect<CustomPlayerEffects.Blinded>();
                                    break;
                                default:
                                    this.Log.Error("Unknown Effect | " + effect);
                                    continue;
                            }

                            this.healthEffects[player.Id].Remove(effect);
                        }
                    }
                    else
                        this.healthEffects.Add(player.Id, new List<string>());
                    if (!player.IsHuman || !player.IsConnected)
                        continue;
                    if (player.Health < 20)
                    {
                        player.EnableEffect<CustomPlayerEffects.Deafened>();
                        this.healthEffects[player.Id].Add("Deafened");

                        if (player.Health < 15)
                        {
                            player.EnableEffect<CustomPlayerEffects.Concussed>();
                            this.healthEffects[player.Id].Add("Concussed");

                            if (player.Health < 10)
                            {
                                player.EnableEffect<CustomPlayerEffects.Disabled>();
                                this.healthEffects[player.Id].Add("Disabled");

                                if (player.Health < 5)
                                {
                                    player.EnableEffect<CustomPlayerEffects.Blinded>();
                                    this.healthEffects[player.Id].Add("Blinded");
                                }
                            }
                        }
                    }
                }

                yield return Timing.WaitForSeconds(5);
            }

            this.Log.Info("DoHeathEffects END");
        }

        private void Player_ChangingRole(Exiled.Events.EventArgs.ChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleType.Spectator)
                this.CallDelayed(0.2f, () => ev.Player.DisableAllEffects(), "ClearEffects");
            if (ev.Items.Contains(ItemType.KeycardO5))
                ev.Items.RemoveAll(item => item == ItemType.KeycardNTFLieutenant || item == ItemType.KeycardChaosInsurgency);
        }

        private void Server_WaitingForPlayers()
        {
            /*RoundModifiersManager.SetInstance();
            if (UnityEngine.Random.Range(1, 101) < 2)
            {
                RoundModifiersManager.Instance.SetActiveEvents();
                this.Log.Debug("Activating random events", PluginHandler.Instance.Config.VerbouseOutput);
            }*/
        }

        private void Server_RoundStarted()
        {
            this.healthEffects.Clear();
            this.RunCoroutine(this.DoAmbients(), "DoAmbients");
            this.RunCoroutine(this.DoHeathEffects(), "DoHeathEffects");

            // RoundModifiersManager.Instance.ExecuteFags();
        }

        private void Server_RoundEnded(Exiled.Events.EventArgs.RoundEndedEventArgs ev)
        {
            foreach (var item in Map.Pickups.ToArray())
                item.Destroy();

            foreach (var player in RealPlayers.List.ToArray())
                player.ClearInventory();
        }

        private IEnumerator<float> DoAmbients()
        {
            UsedAmbients.Clear();
            yield return Timing.WaitForSeconds(5);
            while (Round.IsStarted)
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
