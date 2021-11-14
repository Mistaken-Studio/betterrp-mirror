// -----------------------------------------------------------------------
// <copyright file="ForceRPEvents.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

/*
using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Mistaken.API.Commands;

namespace Mistaken.BetterRP
{

    [CommandSystem.CommandHandler(typeof(CommandSystem.RemoteAdminCommandHandler))]
    internal class ForceRPEvents : IBetterCommand, IPermissionLocked
    {
        public string Permission => "rp_events_force";

        public string PluginName => PluginHandler.Instance.Name;

        public override string Command => "rpe";

        public override string[] Aliases => new string[] { };

        public override string Description => "Adds rp event";

        public override string[] Execute(ICommandSender sender, string[] args, out bool success)
        {
            if (!RoundModifiersManager.Instance.SetActiveEvents(int.Parse(args[0])))
            {
                success = false;
                return new string[] { "Failed to generate random events" };
            }
            else
            {
                List<string> tor = new List<string>();
                success = true;
                for (int i = 0; i < RoundModifiersManager.RandomEventsLength; i++)
                {
                    RoundModifiersManager.RandomEvents re = (RoundModifiersManager.RandomEvents)Math.Pow(2, i);
                    if (RoundModifiersManager.Instance.ActiveEvents.HasFlag(re))
                    {
                        tor.Add(re.ToString());
                    }
                }

                return new string[] { "Activated:", string.Join("\n", tor) };
            }
        }

        public string GetUsage() =>
            "rpevents (Id)";

    }
}
*/