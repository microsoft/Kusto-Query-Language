using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// The base class for any <see cref="KustoCode"/> analyzer.
    /// </summary>
    public abstract class KustoAnalyzer
    {
        /// <summary>
        /// The name of the analyzer
        /// </summary>
        public virtual string Name { get { return this.GetType().Name; } }

        protected abstract IEnumerable<Diagnostic> GetDiagnostics();

        private IReadOnlyList<Diagnostic> _diagnostics;

        /// <summary>
        /// The diagnostics produced by this analyzer.
        /// </summary>
        public IReadOnlyList<Diagnostic> Diagnostics
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
        /// Analyzes the <see cref="KustoCode"/> and returns any diagnostics found.
        /// </summary>
        public abstract IReadOnlyList<Diagnostic> Analyze(KustoCode code, CancellationToken cancellationToken);
    }
}