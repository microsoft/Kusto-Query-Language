using System;
using System.Collections.Generic;

namespace Kusto.Language.Binding
{
    using Symbols;

    /// <summary>
    /// Binding state that exists for the duration of the binder.
    /// </summary>
    internal class LocalBindingCache
    {
        internal readonly HashSet<Signature> SignaturesComputingExpansion
            = new HashSet<Signature>();

        internal Dictionary<CallSiteInfo, Expansion> CallSiteToExpansionMap =
            new Dictionary<CallSiteInfo, Expansion>(CallSiteInfo.Comparer.Instance);
    }
}