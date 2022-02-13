using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class AvoidUsingObsoleteFunctionsAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidUsingObsoleteFunctions,
                DiagnosticCategory.Correctness,
                DiagnosticSeverity.Warning,
                description: "Avoid using obsolete/deprecated functions.");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<FunctionCallExpression>(
                fc => fc.ReferencedSymbol is FunctionSymbol fs && fs.IsObsolete))
            {
                var symbol = (FunctionSymbol)node.ReferencedSymbol;
                diagnostics.Add(
                    _diagnostic.WithMessage($"The function '{symbol.Name}' is deprecated; use '{symbol.Alternative}' instead.")
                    .WithLocation(node.Name)
                    );
            }
        }
    }
}