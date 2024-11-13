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
            new Dictionary<CallSiteInfo, FunctionCallExpansion>();

        internal Dictionary<CallSiteInfo, TypeSymbol> CallSiteToResultTypeMap =
            new Dictionary<CallSiteInfo, TypeSymbol>();

        internal readonly Dictionary<Signature, FunctionBodyFacts> NonDatabaseFunctionBodyFacts =
            new Dictionary<Signature, FunctionBodyFacts>();

        internal readonly Dictionary<Signature, int> FunctionExpansionCounts =
            new Dictionary<Signature, int>();   
    }
}