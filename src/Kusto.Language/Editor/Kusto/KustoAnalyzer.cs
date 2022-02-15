using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// The base class for any <see cref="KustoCode"/> analyzer.
    /// </summary>
    public abstract class KustoAnalyzer : CodeAnalyzer
    {
        /// <summary>
        /// Override this method to suppy the example set of diagnostics that the analyzer produces.
        /// </summary>
        protected abstract IEnumerable<Diagnostic> GetDiagnostics();

        /// <summary>
        /// The cached set of example diagnostics
        /// </summary>
        private IReadOnlyList<Diagnostic> _diagnostics;

        /// <summary>
        /// The diagnostics produced by this analyzer.
        /// </summary>
        public override IReadOnlyList<Diagnostic> Diagnostics
        {
            get
            { 
                if (_diagnostics == null)
                {
                    _diagnostics = GetDiagnostics().ToReadOnly();
                }

                return _diagnostics;
            }
        }

        /// <summary>
        /// Analyzes the <see cref="KustoCode"/> and outputs any diagnostics found into the diagnostics list.
        /// </summary>
        public abstract void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken);
    }
}