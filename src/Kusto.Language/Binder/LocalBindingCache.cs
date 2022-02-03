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

        internal Dictionary<CallSiteInfo, FunctionCallExpansion> CallSiteToExpansionMap =
            new Dictionary<CallSiteInfo, FunctionCallExpansion>(CallSiteInfo.Comparer.Instance);
    }
}