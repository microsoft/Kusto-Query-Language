using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Utils;

    /// <summary>
    /// Detects expressions that use 'contains' operator
    /// </summary>
    internal class AvoidUsingContainsAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                "KustoAvoidUsingContains",
                DiagnosticCategory.Performance,
                DiagnosticSeverity.Suggestion,
                $"Avoid using the 'contains' operator as it has a high compute price." + Environment.NewLine +
                $"Use the 'has' operator in cases when full term match is desired (see: https://aka.ms/kusto.stringterms).");

        public override IReadOnlyList<Diagnostic> Analyze(KustoCode code, CancellationToken cancellationToken)
        {
            var diagnostics = new List<Diagnostic>();

            foreach (var node in code.Syntax.GetDescendants<BinaryExpression>())
            {
                if (node.Kind == SyntaxKind.ContainsExpression ||
                    node.Kind == SyntaxKind.NotContainsExpression ||
                    node.Kind == SyntaxKind.ContainsCsExpression ||
                    node.Kind == SyntaxKind.NotContainsCsExpression)
                {
                    diagnostics.Add(_diagnostic.WithLocation(node.Operator));
                }
            }

            return diagnostics;
        }
    }
}