using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Kusto.Language.Editor
{
    public class AnalyzerInfo
    {
        /// <summary>
        /// The name of the analyzer.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The diagnostic codes produced by this analyzer.
        /// </summary>
        public IReadOnlyList<Diagnostic> Diagnostics { get; }

        public AnalyzerInfo(string name, IReadOnlyList<Diagnostic> diagnostics)
        {
            this.Name = name ?? "";
            this.Diagnostics = diagnostics ?? Utils.EmptyReadOnlyList<Diagnostic>.Instance;
        }
    }
}