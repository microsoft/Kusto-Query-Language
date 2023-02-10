using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// A <see cref="KustoAnalyzer"/> implements additional semantic analysis beyond that done by <see cref="KustoCode"/>.
    /// Analyzers are invoked via <see cref="CodeService.GetAnalyzerDiagnostics"/>.
    /// The analyzer may also provide fix actions for the diagnostics they report.
    /// </summary>
    internal abstract class KustoAnalyzer
    {
        /// <summary>
        /// The name of this analyzer.
        /// This is used to find the analyzer that created the <see cref="CodeAction"/> when the action is being applied.
        /// </summary>
        public virtual string Name => this.GetType().Name;

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
        /// Analyzes the <see cref="KustoCode"/> and outputs any diagnostics found into the diagnostics list.
        /// </summary>
        public abstract void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a <see cref="KustoFixer"/> for this <see cref="KustoAnalyzer"/>
        /// </summary>
        protected virtual KustoFixer CreateFixer()
        {
            return new AnalyzerFixer(this);
        }

        /// <summary>
        /// The cached <see cref="KustoFixer"/>
        /// </summary>
        private KustoFixer _fixer;

        /// <summary>
        /// The <see cref="KustoFixer"/> associated with this <see cref="KustoAnalyzer"/>
        /// </summary>
        public KustoFixer Fixer
        {
            get
            {
                if (_fixer == null)
                    _fixer = CreateFixer();
                return _fixer;
            }
        }

        /// <summary>
        /// A <see cref="KustoFixer"/> implementation that defers back to the analyzer itself.
        /// </summary>
        private class AnalyzerFixer : KustoFixer
        {
            private readonly KustoAnalyzer _analyzer;

            public AnalyzerFixer(KustoAnalyzer analyzer)
            {
                _analyzer = analyzer;
            }

            public override string Name => _analyzer.Name;

            protected override IEnumerable<Diagnostic> GetDiagnostics()
            {
                return _analyzer.GetDiagnostics();
            }

            protected override void GetFixAction(KustoCode code, Diagnostic dx, CodeActionOptions options, List<CodeAction> actions, CancellationToken cancellationToken)
            {
                _analyzer.GetFixAction(code, dx, options, actions, cancellationToken);
            }

            protected override FixEdits GetFixEdits(KustoCode code, ApplyAction action, int caretPosition, CodeActionOptions options, CancellationToken cancellationToken)
            {
                return _analyzer.GetFixEdits(code, action, caretPosition, options, cancellationToken);
            }
        }

        /// <summary>
        /// Gets the fix actions for the diagnostic
        /// </summary>
        protected virtual void GetFixAction(
            KustoCode code,
            Diagnostic dx,
            CodeActionOptions options,
            List<CodeAction> actions,
            CancellationToken cancellationToken)
        {
        }

        /// <summary>
        /// Gets the <see cref="FixEdits"/> for the <see cref="ApplyAction"/>.
        /// </summary>
        protected virtual FixEdits GetFixEdits(
            KustoCode code,
            ApplyAction action,
            int caretPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            return new FixEdits(caretPosition, null);
        }
    }
}