using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Binding;
    using Syntax;
    using Symbols;

    internal static class TestHelpers
    {
        public static int GetGlobalExpansionCacheSize(GlobalState globals)
        {
            if (globals.Cache != null && globals.Cache.TryGetValue<GlobalBindingCache>(out var gbc))
            {
                return gbc.CallSiteToExpansionMap.Values.Sum(v => v.Count);
            }

            return 0;
        }

        public static int GetGlobalCachedExpansionCount(GlobalState globals, Signature signature)
        {
            if (globals.Cache is KustoCache cache
                && cache.TryGetValue<GlobalBindingCache>(out var gbc)
                && gbc.CallSiteToExpansionMap.TryGetValue(signature, out var globalMru))
            {
                return globalMru.Count;
            }

            return 0;
        }

        public static int GetGlobalCachedResultTypeCount(GlobalState globals, Signature signature)
        {
            if (globals.Cache is KustoCache cache
                && cache.TryGetValue<GlobalBindingCache>(out var gbc)
                && gbc.CallSiteToResultTypeMap.TryGetValue(signature, out var globalMru))
            {
                return globalMru.Count;
            }

            return 0;
        }

        public static int GetLocalCachedExpansionCount(KustoCode code, Signature signature)
        {
            return code._localCache.CallSiteToExpansionMap.Keys.Count(v => v.Signature == signature);
        }

        public static int GetLocalCachedResultTypeCount(KustoCode code, Signature signature)
        {
            return code._localCache.CallSiteToResultTypeMap.Keys.Count(v => v.Signature == signature);
        }

        public static void Bind(SyntaxNode syntax, GlobalState globals)
        {
            var tree = new SyntaxTree(syntax);
            Binder.TryBind(tree, globals);
        }
    }
}