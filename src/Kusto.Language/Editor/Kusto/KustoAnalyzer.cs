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
        public virtual string Name { get { return this.GetType().Name; } }

        public abstract IReadOnlyList<Diagnostic> Analyze(KustoCode code, CancellationToken cancellationToken);
    }
}