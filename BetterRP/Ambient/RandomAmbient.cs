// -----------------------------------------------------------------------
// <copyright file="RandomAmbient.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Mistaken.BetterRP.Ambients
{
    internal class RandomAmbient : Ambient
    {
        public override int Id => 100;

        public override string Message
        {
            get
            {
                switch (UnityEngine.Random.Range(0, /*4*/2))
                {
                    case 0:
                        {
                            var randGSound = UnityEngine.Random.Range(1, 7);
                            var randJamReapeat = UnityEngine.Random.Range(2, 6);
                            return $"jam_040_{randJamReapeat} .g{randGSound}";
                        }

                    case 1:
                        {
                            var randGSound = UnityEngine.Random.Range(1, 7);
                            var randPitch = UnityEngine.Random.Range(0.1f, 1.5f);
                            return $"pitch_{randPitch.ToString().Replace(',', '.')} jam_040_2 .g{randGSound}";
                        }

                    case 2:
                        {
                            string[] items = new string[]
                            {
                                "pitch_0.1 SCP pitch_0.1 6 pitch_0.1 8 pitch_0.1 2",
                                "pitch_0.1 scpsubjects",
                                "pitch_0.1 Camera",
                                "pitch_0.1 Celsius",
                                "pitch_0.1 .g7 h h",
                                "pitch_0.1 ContainedSuccessfully",
                                "pitch_0.1 Decontamination",
                                "pitch_0.1 Intersection",
                                "pitch_0.1 NineTailedFox",
                                "pitch_0.1 Personnel h h a c s e",
                                "pitch_0.1 HasEntered",
                                "pitch_0.1 e s s",
                                "pitch_0.1 c c h",
                                "pitch_0.1 ChaosInsurgency",
                            };
                            return items[UnityEngine.Random.Range(0, items.Length)];
                        }

                    default:
                        return string.Empty;
                }
            }
        }

        public override bool IsJammed => false;

        public override bool IsReusable => true;

        internal RandomAmbient()
        {
            string[] items = new string[]
            {
                "pitch_0.1 SCP pitch_0.1 6 pitch_0.1 8 pitch_0.1 2",
                "pitch_0.1 scpsubjects",
                "pitch_0.1 Camera",
                "pitch_0.1 Celsius",
                "pitch_0.1 .g7 h h",
                "pitch_0.1 ContainedSuccessfully",
                "pitch_0.1 Decontamination",
                "pitch_0.1 Intersection",
                "pitch_0.1 NineTailedFox",
                "pitch_0.1 Personnel h h a c s e",
                "pitch_0.1 HasEntered",
                "pitch_0.1 e s s",
                "pitch_0.1 c c h",
                "pitch_0.1 ChaosInsurgency",
            };
        }
    }
}
