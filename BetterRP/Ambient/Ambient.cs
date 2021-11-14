// -----------------------------------------------------------------------
// <copyright file="Ambient.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Mistaken.BetterRP.Ambients
{
    internal abstract class Ambient
    {
        public abstract int Id { get; }

        public abstract string Message { get; }

        public abstract bool IsJammed { get; }

        public virtual bool IsReusable { get; } = false;

        public virtual bool CanPlay()
        {
            return !BetterRPHandler.UsedAmbients.Contains(this.Id);
        }
    }
}
