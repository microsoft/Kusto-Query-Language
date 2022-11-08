using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class PreferUsingOptimizedAlternative : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.PreferUsingOptimizedAlternative,
                DiagnosticCategory.Performance,
                DiagnosticSeverity.Suggestion,
                description: "Prefer using more optimized alternative when possible.");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<FunctionCallExpression>(
                fc => fc.ReferencedSymbol is FunctionSymbol fs
                    && fs.OptimizedAlternative != null))
            {
                if (node.ReferencedSymbol is FunctionSymbol fs)
                {
                    diagnostics.Add(
                        _diagnostic.WithMessage(
                            $"If possible, consider switching to '{fs.OptimizedAlternative}', which is more optimized than '{fs.Name}'.")
                        .WithLocation(node.Name)
                        );
                }
            }
        }
    }
}