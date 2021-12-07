// -----------------------------------------------------------------------
// <copyright file="RoundModifiersManager.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Interactables.Interobjects.DoorUtils;
using LightContainmentZoneDecontamination;
using MEC;
using Mirror;
using Mistaken.API;
using Mistaken.API.Diagnostics;
using UnityEngine;

namespace Mistaken.BetterRP.RoundModifiers
{
    public class RoundModifiersManager
    {
        public static RoundModifiersManager Instance { get; private set; }

        public static int RandomEventsLength => Enum.GetValues(typeof(RandomEvents)).Length - 1;

        public static void SetInstance()
            => Instance = new RoundModifiersManager();

        public static void Restart()
        {
            API.Utilities.Map.TeslaMode = API.Utilities.TeslaMode.ENABLED;
            RoundModifiersManager.Instance.ActiveEvents = RandomEvents.NONE;
        }

        [Flags]
        public enum RandomEvents : uint
        {
#pragma warning disable CS1591 // Brak komentarza XML dla widocznego publicznie typu lub składowej
            NONE = 0,
            GATE_A_PERNAMENT_CLOSE = 1,
            GATE_B_PERNAMENT_CLOSE = 2,
            GATE_A_ELEVATOR_LOCKDOWN = 4,
            GATE_B_ELEVATOR_LOCKDOWN = 8,
            LCZ_DECONTAMINATION_REAL_20_MINUTES = 16,
            LCZ_DECONTAMINATION_REAL_17_MINUTES = 32,
            LCZ_DECONTAMINATION_REAL_15_MINUTES = 64,
            LCZ_DECONTAMINATION_REAL_10_MINUTES = 128,
            LCZ_DECONTAMINATION_REAL_5_MINUTES = 256,
            LCZ_DECONTAMINATION_PAUSED = 512,
            SURFACE_GATE_LOCKDOWN = 1024,
            LIFT_A_LOCKDOWN = 2048,
            LIFT_B_LOCKDOWN = 4096,
            CHECKPOINT_A_PERNAMENT_OPEN = 8192,
            CHECKPOINT_B_PERNAMENT_OPEN = 16384,
            CHECKPOINT_A_PERNAMENT_CLOSE = 32768,
            CHECKPOINT_B_PERNAMENT_CLOSE = 65536,
            BLACKOUT_FOR_5_SECONDS_EVERY_1_MINUTE = 131072,
            BLACKOUT_FOR_5_SECONDS_EVERY_2_MINUTE = 262144,
            BLACKOUT_FOR_20_SECONDS_EVERY_5_MINUTE = 524288,
            BLACKOUT_FOR_1_SECONDS_EVERY_30_SECONDS = 1048576,
            CHECKPOINT_EZ_PERNAMENT_OPEN = 2097152,
            CASSIE_AUTO_SCAN_5_MINUTES = 4194304,
            CASSIE_AUTO_SCAN_10_MINUTES = 8388608,
            CASSIE_AUTO_SCAN_15_MINUTES = 16777216,
            ALL_DOORS_OPEN_AT_BEGINING = 33554432,
            GATE_A_PERNAMENT_OPEN = 67108864,
            GATE_B_PERNAMENT_OPEN = 134217728,
            TESLA_GATES_DISABLED = 268435456,
#pragma warning restore CS1591 // Brak komentarza XML dla widocznego publicznie typu lub składowej
        }

        public RandomEvents ActiveEvents { get; private set; }

        public bool SetActiveEvents(int events = -1)
        {
            if (events == -1)
                events = UnityEngine.Random.Range(1, 12);
            return this.SetActiveEvents(events, 0);
        }

        public void ExecuteFags()
        {
            foreach (var item in Map.Lifts)
                item.Network_locked = false;

            foreach (var item in Map.Doors)
            {
                item.IsOpen = false;
                item.Base.ActiveLocks = 0;
            }

            API.Utilities.Map.TeslaMode = API.Utilities.TeslaMode.ENABLED;
            API.Utilities.Map.Blackout.Enabled = false;

            if (this.ActiveEvents == 0)
            {
                Log.Debug("No Acitve Random Events");
                return;
            }

            List<string> locked = new List<string>();
            List<string> open = new List<string>();
            bool teslaOff = false;
            for (int i = 0; i < RandomEventsLength; i++)
            {
                RandomEvents re = (RandomEvents)Math.Pow(2, i);
                if (this.ActiveEvents.HasFlag(re))
                {
                    Log.Debug("Executing: " + re);
                    switch (re)
                    {
                        case RandomEvents.LCZ_DECONTAMINATION_PAUSED:
                            {
                                DecontaminationController.Singleton.disableDecontamination = true;
                                break;
                            }

                        case RandomEvents.LCZ_DECONTAMINATION_REAL_20_MINUTES:
                            {
                                DecontaminationController.Singleton.NetworkRoundStartTime = this.GetNewLCZTime(1200);
                                break;
                            }

                        case RandomEvents.LCZ_DECONTAMINATION_REAL_17_MINUTES:
                            {
                                DecontaminationController.Singleton.NetworkRoundStartTime = this.GetNewLCZTime(1020);
                                break;
                            }

                        case RandomEvents.LCZ_DECONTAMINATION_REAL_15_MINUTES:
                            {
                                DecontaminationController.Singleton.NetworkRoundStartTime = this.GetNewLCZTime(900);
                                break;
                            }

                        case RandomEvents.LCZ_DECONTAMINATION_REAL_10_MINUTES:
                            {
                                Module.CallSafeDelayed(
                                    45,
                                    () =>
                                {
                                    DecontaminationController.Singleton.NetworkRoundStartTime = this.GetNewLCZTime(600);
                                }, "RoundModifiersManager.LCZDecontReal10Minutes");
                                break;
                            }

                        case RandomEvents.LCZ_DECONTAMINATION_REAL_5_MINUTES:
                            {
                                Module.CallSafeDelayed(
                                    45,
                                    () =>
                                {
                                    DecontaminationController.Singleton.NetworkRoundStartTime = this.GetNewLCZTime(300);
                                }, "RoundModifiersManager.LCZDecontReal5Minutes");
                                break;
                            }

                        case RandomEvents.BLACKOUT_FOR_1_SECONDS_EVERY_30_SECONDS:
                            {
                                API.Utilities.Map.Blackout.Delay = 30;
                                API.Utilities.Map.Blackout.Length = 1;
                                API.Utilities.Map.Blackout.Enabled = true;
                                break;
                            }

                        case RandomEvents.BLACKOUT_FOR_20_SECONDS_EVERY_5_MINUTE:
                            {
                                API.Utilities.Map.Blackout.Delay = 300;
                                API.Utilities.Map.Blackout.Length = 20;
                                API.Utilities.Map.Blackout.Enabled = true;
                                break;
                            }

                        case RandomEvents.BLACKOUT_FOR_5_SECONDS_EVERY_1_MINUTE:
                            {
                                API.Utilities.Map.Blackout.Delay = 60;
                                API.Utilities.Map.Blackout.Length = 5;
                                API.Utilities.Map.Blackout.Enabled = true;
                                break;
                            }

                        case RandomEvents.BLACKOUT_FOR_5_SECONDS_EVERY_2_MINUTE:
                            {
                                API.Utilities.Map.Blackout.Delay = 120;
                                API.Utilities.Map.Blackout.Length = 5;
                                API.Utilities.Map.Blackout.Enabled = true;
                                break;
                            }

                        case RandomEvents.CASSIE_AUTO_SCAN_10_MINUTES:
                            {
                                Module.RunSafeCoroutine(this.ToScan(600), "RoundModifiersManager.ToScan");
                                break;
                            }

                        case RandomEvents.CASSIE_AUTO_SCAN_5_MINUTES:
                            {
                                Module.RunSafeCoroutine(this.ToScan(300), "RoundModifiersManager.ToScan");
                                break;
                            }

                        case RandomEvents.CASSIE_AUTO_SCAN_15_MINUTES:
                            {
                                Module.RunSafeCoroutine(this.ToScan(900), "RoundModifiersManager.ToScan");
                                break;
                            }

                        case RandomEvents.LIFT_A_LOCKDOWN:
                            {
                                locked.Add("LIGHT CONTAINMENT ZONE ELEVATOR A");
                                foreach (var item in Map.Lifts.Where(e => e.Type() == Exiled.API.Enums.ElevatorType.LczA))
                                    item.Network_locked = true;
                                break;
                            }

                        case RandomEvents.LIFT_B_LOCKDOWN:
                            {
                                locked.Add("LIGHT CONTAINMENT ZONE ELEVATOR B");
                                foreach (var item in Map.Lifts.Where(e => e.Type() == Exiled.API.Enums.ElevatorType.LczB))
                                    item.Network_locked = true;
                                break;
                            }

                        case RandomEvents.GATE_A_ELEVATOR_LOCKDOWN:
                            {
                                foreach (var item in Map.Lifts.Where(e => e.Type() == Exiled.API.Enums.ElevatorType.GateA))
                                    item.Network_locked = true;
                                break;
                            }

                        case RandomEvents.GATE_B_ELEVATOR_LOCKDOWN:
                            {
                                foreach (var item in Map.Lifts.Where(e => e.Type() == Exiled.API.Enums.ElevatorType.GateB))
                                    item.Network_locked = true;
                                break;
                            }

                        case RandomEvents.SURFACE_GATE_LOCKDOWN:
                            {
                                if (!Map.Doors.Any(d => d.Type == Exiled.API.Enums.DoorType.SurfaceGate))
                                    break;
                                locked.Add("SURFACE GATE");
                                Map.Doors.First(d => d.Type == Exiled.API.Enums.DoorType.SurfaceGate).Base.ActiveLocks = (byte)DoorLockReason.AdminCommand;
                                break;
                            }

                        case RandomEvents.GATE_A_PERNAMENT_OPEN:
                            {
                                if (!Map.Doors.Any(d => d.Type == Exiled.API.Enums.DoorType.GateA))
                                    break;
                                open.Add("GATE A");
                                var door = Map.Doors.First(d => d.Type == Exiled.API.Enums.DoorType.GateA);
                                door.Base.ActiveLocks = (byte)DoorLockReason.AdminCommand;
                                door.IsOpen = true;
                                break;
                            }

                        case RandomEvents.GATE_B_PERNAMENT_OPEN:
                            {
                                if (!Map.Doors.Any(d => d.Type == Exiled.API.Enums.DoorType.GateB))
                                    break;
                                open.Add("GATE B");
                                var door = Map.Doors.First(d => d.Type == Exiled.API.Enums.DoorType.GateB);
                                door.Base.ActiveLocks = (byte)DoorLockReason.AdminCommand;
                                door.IsOpen = true;
                                break;
                            }

                        case RandomEvents.GATE_A_PERNAMENT_CLOSE:
                            {
                                if (!Map.Doors.Any(d => d.Type == Exiled.API.Enums.DoorType.GateA))
                                    break;
                                locked.Add("GATE A");
                                var door = Map.Doors.First(d => d.Type == Exiled.API.Enums.DoorType.GateA);
                                door.Base.ActiveLocks = (byte)DoorLockReason.AdminCommand;
                                door.IsOpen = false;
                                break;
                            }

                        case RandomEvents.GATE_B_PERNAMENT_CLOSE:
                            {
                                if (!Map.Doors.Any(d => d.Type == Exiled.API.Enums.DoorType.GateB))
                                    break;
                                locked.Add("GATE B");
                                var door = Map.Doors.First(d => d.Type == Exiled.API.Enums.DoorType.GateB);
                                door.Base.ActiveLocks = (byte)DoorLockReason.AdminCommand;
                                door.IsOpen = false;
                                break;
                            }

                        case RandomEvents.CHECKPOINT_EZ_PERNAMENT_OPEN:
                            {
                                if (!Map.Doors.Any(d => d.Type == Exiled.API.Enums.DoorType.CheckpointEntrance))
                                    break;
                                open.Add("CHECKPOINT ENTRANCE ZONE");
                                var door = Map.Doors.First(d => d.Type == Exiled.API.Enums.DoorType.CheckpointEntrance);
                                door.BreakDoor();
                                break;
                            }

                        case RandomEvents.ALL_DOORS_OPEN_AT_BEGINING:
                            {
                                Map.Doors.Where(d => d.Type == Exiled.API.Enums.DoorType.EntranceDoor || d.Type == Exiled.API.Enums.DoorType.HeavyContainmentDoor || d.Type == Exiled.API.Enums.DoorType.LightContainmentDoor).Select(door => door.Base.ActiveLocks = (byte)DoorLockReason.AdminCommand);
                                break;
                            }

                        case RandomEvents.CHECKPOINT_A_PERNAMENT_CLOSE:
                            {
                                if (!Map.Doors.Any(d => d.Type == Exiled.API.Enums.DoorType.CheckpointLczA))
                                    break;
                                locked.Add("CHECKPOINT A");
                                var door = Map.Doors.First(d => d.Type == Exiled.API.Enums.DoorType.CheckpointLczA);
                                door.Base.ActiveLocks = (byte)DoorLockReason.AdminCommand;
                                door.IsOpen = false;
                                break;
                            }

                        case RandomEvents.CHECKPOINT_B_PERNAMENT_CLOSE:
                            {
                                if (!Map.Doors.Any(d => d.Type == Exiled.API.Enums.DoorType.CheckpointLczB))
                                    break;
                                locked.Add("CHECKPOINT B");
                                var door = Map.Doors.First(d => d.Type == Exiled.API.Enums.DoorType.CheckpointLczB);
                                door.Base.ActiveLocks = (byte)DoorLockReason.AdminCommand;
                                door.IsOpen = false;
                                break;
                            }

                        case RandomEvents.CHECKPOINT_A_PERNAMENT_OPEN:
                            {
                                if (!Map.Doors.Any(d => d.Type == Exiled.API.Enums.DoorType.CheckpointLczA))
                                    break;
                                open.Add("CHECKPOINT A");
                                var door = Map.Doors.First(d => d.Type == Exiled.API.Enums.DoorType.CheckpointLczA);
                                door.BreakDoor();
                                break;
                            }

                        case RandomEvents.CHECKPOINT_B_PERNAMENT_OPEN:
                            {
                                if (!Map.Doors.Any(d => d.Type == Exiled.API.Enums.DoorType.CheckpointLczB))
                                    break;
                                open.Add("CHECKPOINT B");
                                var door = Map.Doors.First(d => d.Type == Exiled.API.Enums.DoorType.CheckpointLczB);
                                door.BreakDoor();
                                break;
                            }

                        case RandomEvents.TESLA_GATES_DISABLED:
                            {
                                API.Utilities.Map.TeslaMode = API.Utilities.TeslaMode.DISABLED;
                                teslaOff = true;
                                break;
                            }
                    }
                }
            }

            string cassie = "ATTENTION ALL PERSONNEL . DETECTED CASSIESYSTEM CRITICAL ERROR . . " + (locked.Count != 0 ? "INITIATED LOCKDOWN ON " + string.Join(" . ", locked) : string.Empty) + " . " + (open.Count != 0 ? " SYSTEM OPEN LOCKDOWN " + string.Join(" . ", open) : string.Empty) + " . " + (teslaOff ? "TESLA GATES DISABLED" : string.Empty);

            if (locked.Count != 0 || open.Count != 0)
                Cassie.DelayedMessage(cassie, 60);

            string toWrite = "[RPE] Active: ";
            for (int i = 1; i <= ((uint[])Enum.GetValues(typeof(RandomEvents))).Max(); i *= 2)
            {
                if (this.ActiveEvents.HasFlag((RandomEvents)i))
                    toWrite += $"\n- {(RandomEvents)i}";
            }

            foreach (var item in RealPlayers.List.Where(p => p.RemoteAdminAccess))
                item.SendConsoleMessage(toWrite, "green");
            MapPlus.Broadcast("RPE", 10, "RandomRPEvents activated, check console for more info", Broadcast.BroadcastFlags.AdminChat);
        }

        public IEnumerator<float> ToScan(float interval)
        {
            yield return Timing.WaitForSeconds(1);
            int rid = RoundPlus.RoundId;
            while (Round.IsStarted && rid == RoundPlus.RoundId)
            {
                yield return Timing.WaitForSeconds(interval);
                int sCP = RealPlayers.Get(Team.SCP).Count();
                int cDP = RealPlayers.Get(Team.CDP).Count();
                int rSC = RealPlayers.Get(Team.RSC).Count();
                int mTF = RealPlayers.Get(Team.MTF).Count();
                int cHI = RealPlayers.Get(Team.CHI).Count();
                while (Cassie.IsSpeaking)
                    yield return Timing.WaitForOneFrame;
                Cassie.Message($"FACILITY SCAN RESULT . {(sCP == 0 ? "NO" : sCP.ToString())} SCPSUBJECT{(sCP == 1 ? string.Empty : "S")} . {(cDP == 0 ? "NO" : cDP.ToString())} CLASSD . {(rSC == 0 ? "NO" : rSC.ToString())} scientist{(rSC == 1 ? string.Empty : "S")} . {(mTF == 0 ? "NO" : mTF.ToString())} FoUNDATION FORCES . {(cHI == 0 ? "NO" : cHI.ToString())} CHAOSINSURGENCY");
            }
        }

        private const short RESetTries = 1000;

        private bool SetActiveEvents(int events, int check)
        {
            this.ActiveEvents = 0;
            check++;
            bool broken = false;

            for (int i = 0; i < events; i++)
            {
                bool success = false;
                int tries = 0;
                while (!success && !broken)
                {
                    tries++;
                    var num = UnityEngine.Random.Range(0, RandomEventsLength);
                    RandomEvents re = (RandomEvents)Math.Pow(2, num);
                    if (!this.ActiveEvents.HasFlag(re))
                    {
                        this.ActiveEvents |= re;
                        success = true;
                    }

                    if (tries > 100)
                    {
                        broken = true;
                        break;
                    }
                }

                if (broken)
                    break;
            }

            if (check < (RESetTries ^ Mathf.RoundToInt(events / 2)))
            {
                if (!this.ValidateRandomEvents(this.ActiveEvents) || broken)
                    return this.SetActiveEvents(events, check);
                else
                    return true;
            }
            else
            {
                Log.Warn($"Failed to generate ActiveEvents in {RESetTries} tries !");
                Log.Warn("Target Events Amount: " + events);
                this.ActiveEvents = 0;
                return false;
            }
        }

        private bool ValidateRandomEvents(RandomEvents events)
        {
            if (events.HasFlag(RandomEvents.LIFT_A_LOCKDOWN) && events.HasFlag(RandomEvents.LIFT_B_LOCKDOWN)) return false;
            if ((events.HasFlag(RandomEvents.GATE_A_ELEVATOR_LOCKDOWN) || events.HasFlag(RandomEvents.GATE_A_PERNAMENT_CLOSE)) && (events.HasFlag(RandomEvents.GATE_B_ELEVATOR_LOCKDOWN) || events.HasFlag(RandomEvents.GATE_B_PERNAMENT_CLOSE))) return false;
            if (events.HasFlag(RandomEvents.GATE_A_PERNAMENT_CLOSE) && events.HasFlag(RandomEvents.GATE_A_PERNAMENT_OPEN)) return false;
            if (events.HasFlag(RandomEvents.GATE_B_PERNAMENT_CLOSE) && events.HasFlag(RandomEvents.GATE_B_PERNAMENT_OPEN)) return false;
            if (events.HasFlag(RandomEvents.SURFACE_GATE_LOCKDOWN) && (events.HasFlag(RandomEvents.GATE_A_ELEVATOR_LOCKDOWN) || events.HasFlag(RandomEvents.GATE_A_PERNAMENT_CLOSE) || events.HasFlag(RandomEvents.GATE_B_ELEVATOR_LOCKDOWN) || events.HasFlag(RandomEvents.GATE_B_PERNAMENT_CLOSE))) return false;
            if (events.HasFlag(RandomEvents.CHECKPOINT_A_PERNAMENT_CLOSE) && events.HasFlag(RandomEvents.CHECKPOINT_A_PERNAMENT_OPEN)) return false;
            if (events.HasFlag(RandomEvents.CHECKPOINT_B_PERNAMENT_CLOSE) && events.HasFlag(RandomEvents.CHECKPOINT_B_PERNAMENT_OPEN)) return false;
            if ((events.HasFlag(RandomEvents.CHECKPOINT_A_PERNAMENT_CLOSE) || events.HasFlag(RandomEvents.LIFT_A_LOCKDOWN)) && (events.HasFlag(RandomEvents.CHECKPOINT_B_PERNAMENT_CLOSE) || events.HasFlag(RandomEvents.LIFT_B_LOCKDOWN))) return false;
            if (events.HasFlag(RandomEvents.CHECKPOINT_B_PERNAMENT_CLOSE) && events.HasFlag(RandomEvents.LIFT_A_LOCKDOWN)) return false;
            if (events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_PAUSED) && (events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_10_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_15_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_5_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_20_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_17_MINUTES))) return false;
            if (events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_10_MINUTES) && (events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_PAUSED) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_15_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_5_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_20_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_17_MINUTES))) return false;
            if (events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_15_MINUTES) && (events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_10_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_PAUSED) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_5_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_20_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_17_MINUTES))) return false;
            if (events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_5_MINUTES) && (events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_15_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_10_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_PAUSED) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_20_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_17_MINUTES))) return false;
            if (events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_20_MINUTES) && (events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_5_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_15_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_10_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_PAUSED) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_17_MINUTES))) return false;
            if (events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_17_MINUTES) && (events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_20_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_5_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_15_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_REAL_10_MINUTES) || events.HasFlag(RandomEvents.LCZ_DECONTAMINATION_PAUSED))) return false;
            if (events.HasFlag(RandomEvents.BLACKOUT_FOR_1_SECONDS_EVERY_30_SECONDS) && (events.HasFlag(RandomEvents.BLACKOUT_FOR_20_SECONDS_EVERY_5_MINUTE) || events.HasFlag(RandomEvents.BLACKOUT_FOR_5_SECONDS_EVERY_1_MINUTE) || events.HasFlag(RandomEvents.BLACKOUT_FOR_5_SECONDS_EVERY_2_MINUTE))) return false;
            if (events.HasFlag(RandomEvents.BLACKOUT_FOR_20_SECONDS_EVERY_5_MINUTE) && (events.HasFlag(RandomEvents.BLACKOUT_FOR_1_SECONDS_EVERY_30_SECONDS) || events.HasFlag(RandomEvents.BLACKOUT_FOR_5_SECONDS_EVERY_1_MINUTE) || events.HasFlag(RandomEvents.BLACKOUT_FOR_5_SECONDS_EVERY_2_MINUTE))) return false;
            if (events.HasFlag(RandomEvents.BLACKOUT_FOR_5_SECONDS_EVERY_1_MINUTE) && (events.HasFlag(RandomEvents.BLACKOUT_FOR_20_SECONDS_EVERY_5_MINUTE) || events.HasFlag(RandomEvents.BLACKOUT_FOR_1_SECONDS_EVERY_30_SECONDS) || events.HasFlag(RandomEvents.BLACKOUT_FOR_5_SECONDS_EVERY_2_MINUTE))) return false;
            if (events.HasFlag(RandomEvents.BLACKOUT_FOR_5_SECONDS_EVERY_2_MINUTE) && (events.HasFlag(RandomEvents.BLACKOUT_FOR_20_SECONDS_EVERY_5_MINUTE) || events.HasFlag(RandomEvents.BLACKOUT_FOR_5_SECONDS_EVERY_1_MINUTE) || events.HasFlag(RandomEvents.BLACKOUT_FOR_1_SECONDS_EVERY_30_SECONDS))) return false;
            if (events.HasFlag(RandomEvents.CASSIE_AUTO_SCAN_5_MINUTES) && (events.HasFlag(RandomEvents.CASSIE_AUTO_SCAN_10_MINUTES) || events.HasFlag(RandomEvents.CASSIE_AUTO_SCAN_15_MINUTES))) return false;
            if (events.HasFlag(RandomEvents.CASSIE_AUTO_SCAN_10_MINUTES) && (events.HasFlag(RandomEvents.CASSIE_AUTO_SCAN_5_MINUTES) || events.HasFlag(RandomEvents.CASSIE_AUTO_SCAN_15_MINUTES))) return false;
            if (events.HasFlag(RandomEvents.CASSIE_AUTO_SCAN_15_MINUTES) && (events.HasFlag(RandomEvents.CASSIE_AUTO_SCAN_10_MINUTES) || events.HasFlag(RandomEvents.CASSIE_AUTO_SCAN_5_MINUTES))) return false;
            return true;
        }

        private double GetNewLCZTime(double fullTime)
        {
            if (fullTime < 1) throw new Exception("Full Time can't be smaller than 1");
            var tor = (NetworkTime.time - DecontaminationController.Singleton.DecontaminationPhases.First(i =>
              i.Function == DecontaminationController.DecontaminationPhase.PhaseFunction.Final).TimeTrigger) +
              fullTime;
            tor = tor < 1 ? 1 : tor;
            return tor;
        }
    }
}
