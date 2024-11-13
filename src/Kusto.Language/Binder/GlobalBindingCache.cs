using System;
using System.Collections.Generic;

namespace Kusto.Language.Binding
{
    using Kusto.Language;
    using Symbols;
    using Utils;

    /// <summary>
    /// Binding state that persists across multiple bindings (lifetime of <see cref="KustoCache"/>)
    /// </summary>
    internal class GlobalBindingCache
    {
        internal readonly ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol> UnifiedNameColumnsMap =
            new ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol>(ReadOnlyListComparer<TableSymbol>.Default);

        internal readonly ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol> UnifiedNameAndTypeColumnsMap =
            new ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol>(ReadOnlyListComparer<TableSymbol>.Default);

        internal readonly ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol> CommonColumnsMap =
            new ThreadSafeDictionary<IReadOnlyList<TableSymbol>, TableSymbol>(ReadOnlyListComparer<TableSymbol>.Default);

        internal ThreadSafeDictionary<Signature, MostRecentlyUsedCache<CallSiteInfo, FunctionCallExpansion>> CallSiteToExpansionMap =
            new ThreadSafeDictionary<Signature, MostRecentlyUsedCache<CallSiteInfo, FunctionCallExpansion>>();

        internal ThreadSafeDictionary<Signature, MostRecentlyUsedCache<CallSiteInfo, TypeSymbol>> CallSiteToResultTypeMap =
            new ThreadSafeDictionary<Signature, MostRecentlyUsedCache<CallSiteInfo, TypeSymbol>>();

        internal readonly ThreadSafeDictionary<Signature, FunctionBodyFacts> DatabaseFunctionBodyFacts =
            new ThreadSafeDictionary<Signature, FunctionBodyFacts>();
    }
}