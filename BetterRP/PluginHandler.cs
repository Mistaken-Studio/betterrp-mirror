// -----------------------------------------------------------------------
// <copyright file="PluginHandler.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace Mistaken.BetterRP
{
    /// <inheritdoc/>
    internal class PluginHandler : Plugin<Config>
    {
        /// <inheritdoc/>
        public override string Author => "Mistaken Devs";

        /// <inheritdoc/>
        public override string Name => "BetterRP";

        /// <inheritdoc/>
        public override string Prefix => "MBetterRP";

        /// <inheritdoc/>
        public override PluginPriority Priority => PluginPriority.Default;

        /// <inheritdoc/>
        public override Version RequiredExiledVersion => new Version(4, 1, 2);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Instance = this;

            new BetterRPHandler(this);
            new AmbientHandler(this);
            new ClearPostRoundHandler(this);
            new AdrenalinHandler(this);
            new BetterHurtEffectsHandler(this);

            new RoundModifiers.RoundModifiersHandler(this);

            API.Diagnostics.Module.OnEnable(this);

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            API.Diagnostics.Module.OnDisable(this);

            base.OnDisabled();
        }

        internal static PluginHandler Instance { get; private set; }
    }
}
