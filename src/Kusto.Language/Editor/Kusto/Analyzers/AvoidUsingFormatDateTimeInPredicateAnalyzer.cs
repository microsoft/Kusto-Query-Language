using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class AvoidUsingFormatDateTimeInPredicateAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidUsingFormatDateTimeInPredicate,
                DiagnosticCategory.Performance,
                DiagnosticSeverity.Suggestion,
                description: "Avoid using format_datetime() in a filter or predicate",
                message: $"Avoid using the 'format_datetime' function in a filter or predicate." + Environment.NewLine +
                         $"Instead, use specific datetime functions like startofday or bin.");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<FunctionCallExpression>(fc =>
                fc.ReferencedSymbol == Functions.FormatDatetime))
            {
                if (IsInFilter(node) || IsInPredicate(node))
                {
                    diagnostics.Add(_diagnostic.WithLocation(node));
                }
            }
        }

        private static bool IsInFilter(SyntaxNode node)
        {
            return node.GetFirstAncestor<FilterOperator>() != null;
        }

        private static bool IsInPredicate(SyntaxNode node)
        {
            return node.GetFirstAncestor<BinaryExpression>(be => IsComparisonOperator(be.Kind)) != null;
        }

        private static bool IsComparisonOperator(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.EqualExpression:
                case SyntaxKind.NotEqualExpression:
                case SyntaxKind.EqualTildeExpression:
                case SyntaxKind.GreaterThanOrEqualExpression:
                case SyntaxKind.GreaterThanExpression:
                case SyntaxKind.LessThanExpression:
                case SyntaxKind.LessThanOrEqualExpression:
                    return true;

                default:
                    return false;
            }
        }
    }
}
