// -----------------------------------------------------------------------
// <copyright file="ForceAmbient.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;

namespace Mistaken.BetterRP
{
    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class ForceAmbient : IBetterCommand, IPermissionLocked
    {
        public string Permission => "force_ambient";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "ambient";

        public override string[] Aliases => new string[] { };

        public override string Description => "Plays Random Ambient or one with supplied Id";

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            string msg = "ERROR";
            bool jammed = true;
            if (args.Length == 0)
            {
                msg = AmbientHandler.GetAmbient(out jammed);
            }
            else if (int.TryParse(args[0], out int ambientId))
            {
                msg = AmbientHandler.GetAmbient(out jammed, ambientId);
            }

            if (msg != null)
            {
                if (jammed)
                    NineTailedFoxAnnouncer.singleton.ServerOnlyAddGlitchyPhrase(msg, 0.1f, 0.07f);
                else
                    Cassie.Message(msg, false, false);
            }

            success = true;

            return new string[] { "Done" };
        }

        public string GetUsage() =>
            "ambient (Id)";
    }
}
