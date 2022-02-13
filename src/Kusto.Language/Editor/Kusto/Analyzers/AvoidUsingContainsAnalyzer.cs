using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class AvoidUsingContainsAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidUsingContains,
                DiagnosticCategory.Performance,
                DiagnosticSeverity.Suggestion,
                description: "Avoid using contains operator",
                message: $"Avoid using the 'contains' operator as it has a high compute price." + Environment.NewLine +
                         $"Use the 'has' operator in cases when full term match is desired (see: https://aka.ms/kusto.stringterms).");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<BinaryExpression>())
            {
                if (node.Kind == SyntaxKind.ContainsExpression ||
                    node.Kind == SyntaxKind.NotContainsExpression ||
                    node.Kind == SyntaxKind.ContainsCsExpression ||
                    node.Kind == SyntaxKind.NotContainsCsExpression)
                {
                    // only report if db column is being used directly
                    if (IsDbColumn(node.Left, code.Globals))
                    {
                        diagnostics.Add(_diagnostic.WithLocation(node.Operator));
                    }
                }
            }
        }

        protected static bool IsDbColumn(Expression expr, GlobalState globals) =>
            expr.ReferencedSymbol is ColumnSymbol c && globals.GetTable(c) != null;
    }
}