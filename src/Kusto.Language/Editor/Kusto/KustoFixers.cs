using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    /// <summary>
    /// The known Kusto code fixers
    /// </summary>
    internal static class KustoFixers
    {
        /// <summary>
        /// The set of all known Kusto code fixers
        /// </summary>
        public static IReadOnlyList<KustoFixer> All =
             new KustoFixer[]
             {
                 // add new fixers here
             }
             .Concat(KustoAnalyzers.All.Select(a => a.Fixer))
             .ToReadOnly();
    }
}