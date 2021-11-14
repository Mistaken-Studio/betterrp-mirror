// -----------------------------------------------------------------------
// <copyright file="Blank.cs" company="Mistaken">
// Copyright (c) Mistaken. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Mistaken.BetterRP.Ambients
{
    internal class Blank : Ambient
    {
        public override int Id => -1;

        public override string Message => string.Empty;

        public override bool IsJammed => false;
    }
}
