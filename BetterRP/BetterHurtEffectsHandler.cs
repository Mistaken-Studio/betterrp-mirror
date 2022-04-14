// -----------------------------------------------------------------------
// <copyright file="BetterHurtEffectsHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using Mistaken.API;
using Mistaken.API.Diagnostics;
using Mistaken.API.Extensions;
using Mistaken.API.GUI;
using PlayerStatsSystem;

namespace Mistaken.BetterRP
{
    /// <inheritdoc/>
    public class BetterHurtEffectsHandler : Module
    {
        /// <inheritdoc/>
        public override string Name => "BetterHurtEffects";

        /// <inheritdoc/>
        public override void OnEnable()
        {
            Exiled.Events.Handlers.Server.RoundStarted += this.Server_RoundStarted;

            // Exiled.Events.Handlers.Player.ChangingRole += this.Player_ChangingRole;
            Exiled.Events.Handlers.Player.Hurting += this.Player_Hurting;
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= this.Server_RoundStarted;

            // Exiled.Events.Handlers.Player.ChangingRole -= this.Player_ChangingRole;
            Exiled.Events.Handlers.Player.Hurting -= this.Player_Hurting;
        }

        internal BetterHurtEffectsHandler(PluginHandler plugin)
            : base(plugin)
        {
        }

        private readonly List<(float HPTreshold, EffectType Type)> effectsPerHP = new List<(float HPTreshold, EffectType Type)>()
        {
            (20, EffectType.Deafened),
            (15, EffectType.Concussed),
            (10, EffectType.Disabled),
            (5, EffectType.Blinded),
        };

        private readonly Dictionary<Player, List<CustomPlayerEffects.PlayerEffect>> healthEffects = new Dictionary<Player, List<CustomPlayerEffects.PlayerEffect>>();

        private void Player_Hurting(Exiled.Events.EventArgs.HurtingEventArgs ev)
        {
            return; // Powoduje zwiększone obrażenia
            if (!ev.Target.IsHuman)
                return;

            if (ev.Target.WillDie(ev.Handler.Base))
                return;

            if (!(ev.Attacker is null) && UnityEngine.Random.Range(0, 100) < ev.Amount / 5)
            {
                if (ev.Attacker.Role == RoleType.Scp0492)
                    ev.Target.EnableEffect<CustomPlayerEffects.Poisoned>();
            }

            if (ev.Handler.Type == DamageType.Bleeding)
                ev.Amount *= 0.45f;

            bool isSpecificDmgType = false;

            switch (ev.Handler.Type)
            {
                case DamageType.Firearm:
                case DamageType.Explosion:
                    isSpecificDmgType = true;

                    break;

                case DamageType.Scp:
                    {
                        if (!(ev.Attacker is null) && (ev.Attacker.Role == RoleType.Scp93953 || ev.Attacker.Role == RoleType.Scp93989))
                            isSpecificDmgType = true;
                    }

                    break;
            }

            if (isSpecificDmgType)
            {
                if (UnityEngine.Random.Range(0, 101) < ev.Amount / 5)
                    ev.Target.EnableEffect<CustomPlayerEffects.Bleeding>();
            }
            else if (ev.Handler.Type == DamageType.Falldown)
            {
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
                foreach (var player in RealPlayers.List.Where(x => x.IsHuman))
                {
                    if (!this.healthEffects.TryGetValue(player, out var effects))
                        this.healthEffects.Add(player, new List<CustomPlayerEffects.PlayerEffect>());

                    foreach (var item in this.effectsPerHP)
                    {
                        CustomPlayerEffects.PlayerEffect effect = player.GetEffect(item.Type);
                        if (player.Health <= item.HPTreshold)
                        {
                            if (!effect.IsEnabled)
                            {
                                effect.IsEnabled = true;
                                this.healthEffects[player].Add(effect);
                            }
                        }
                        else if (this.healthEffects[player].Contains(effect))
                        {
                            if (effect.IsEnabled)
                            {
                                effect.IsEnabled = false;
                                this.healthEffects[player].Remove(effect);
                            }
                        }
                    }
                }

                yield return Timing.WaitForSeconds(5);
            }

            this.Log.Info("DoHeathEffects END");
        }

        [System.Obsolete("Wymagane testy czy to jest dalej potrzebne")]
        private void Player_ChangingRole(Exiled.Events.EventArgs.ChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleType.Spectator)
                this.CallDelayed(0.2f, () => ev.Player.DisableAllEffects(), "ClearEffects");
        }

        private void Server_RoundStarted()
        {
            this.healthEffects.Clear();
            this.RunCoroutine(this.DoHeathEffects(), "DoHeathEffects");
        }
    }
}
