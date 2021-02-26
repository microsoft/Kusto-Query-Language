using System;
using System.Collections.Generic;

namespace Kusto.Language
{
    using Binding;
    using Syntax;

    internal static class TestHelpers
    {
        public static int GetGlobalExpansionCacheSize(GlobalState globals)
        {
            if (globals.Cache != null && globals.Cache.TryGetValue<GlobalBindingCache>(out var gbc))
            {
                return gbc.CallSiteToExpansionMap.Count;
            }

            return 0;
        }

        public static void Bind(SyntaxNode syntax, GlobalState globals)
        {
            var tree = new SyntaxTree(syntax);
            Binder.TryBind(tree, globals);
        }
    }
}