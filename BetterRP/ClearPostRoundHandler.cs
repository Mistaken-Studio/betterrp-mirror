// -----------------------------------------------------------------------
// <copyright file="BetterRPHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Exiled.API.Features;
using Mistaken.API;
using Mistaken.API.Diagnostics;

namespace Mistaken.BetterRP
{
    /// <inheritdoc/>
    public class ClearPostRoundHandler : Module
    {
        /// <inheritdoc/>
        public override string Name => "ClearPostRound";

        /// <inheritdoc/>
        public override void OnEnable()
        {
            Exiled.Events.Handlers.Server.RoundEnded += this.Server_RoundEnded;
        }

        /// <inheritdoc/>
        public override void OnDisable()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= this.Server_RoundEnded;
        }

        internal ClearPostRoundHandler(PluginHandler plugin)
            : base(plugin)
        {
        }

        private void Server_RoundEnded(Exiled.Events.EventArgs.RoundEndedEventArgs ev)
        {
            foreach (var item in Map.Pickups.ToArray())
                item.Destroy();

            foreach (var player in RealPlayers.List.ToArray())
                player.ClearInventory();
        }
    }
}
