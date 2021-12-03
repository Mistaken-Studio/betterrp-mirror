// -----------------------------------------------------------------------
// <copyright file="RoundModifiersHandler.cs" company="Mistaken">
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

namespace Mistaken.BetterRP.RoundModifiers
{
    /// <inheritdoc/>
    public class RoundModifiersHandler : Module
    {
        /// <inheritdoc/>
        public override string Name => "RoundModifiers";

        /// <inheritdoc/>
        public override bool Enabled => false;

        /// <inheritdoc/>
        public override void OnEnable()
        {
            Exiled.Events.Handlers.Server.RoundStarted += this.Server_RoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += this.Server_WaitingForPlayers;
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= this.Server_RoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= this.Server_WaitingForPlayers;
        }

        internal RoundModifiersHandler(PluginHandler plugin)
            : base(plugin)
        {
        }

        private void Server_WaitingForPlayers()
        {
            RoundModifiersManager.SetInstance();
            if (UnityEngine.Random.Range(1, 101) < 2)
            {
                RoundModifiersManager.Instance.SetActiveEvents();
                this.Log.Debug("Activating random events", PluginHandler.Instance.Config.VerbouseOutput);
            }
        }

        private void Server_RoundStarted()
        {
            RoundModifiersManager.Instance.ExecuteFags();
        }
    }
}
