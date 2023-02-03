using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
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

#if false
        protected override void GetFixAction(KustoCode code, Diagnostic dx, CodeActionOptions options, List<CodeAction> actions, CancellationToken cancellationToken)
        {
            if (code.Syntax.GetNodeAt(dx.Start, dx.Length) is FunctionCallExpression fc)
            {
                if (fc.ArgumentList.Expressions.Count == 2
                    && fc.ArgumentList.Expressions[1].Element.ConstantValue is string format
                    && GetAlternativeFunctionName(format) is string alternateFunctionName)
                {
                    actions.Add(new CodeAction(
                        $"Change to '{alternateFunctionName}'",
                        $"Change use of function 'format_datetime' to function '{alternateFunctionName}'",
                        dx.Start.ToString(),
                        dx.Length.ToString(),
                        alternateFunctionName));
                }
            }
        }

        private static string GetAlternativeFunctionName(string format)
        {
            switch (format)
            {
                case "Y":
                case "y":
                case "YY":
                case "yy":
                case "YYYY":
                case "yyyy":
                    return "getyear";
                default:
                    return null;
            }
        }

        protected override FixResult GetFixEdits(KustoCode code, CodeAction action, int cursorPosition, CodeActionOptions options, CancellationToken cancellationToken)
        {
            if (action.Data.Count == 3
                && Int32.TryParse(action.Data[0], out var start)
                && Int32.TryParse(action.Data[1], out var length)
                && action.Data[2] is string alternateFunctionName
                && code.Syntax.GetNodeAt(start, length) is FunctionCallExpression fc
                && fc.ArgumentList.Expressions.Count == 2)
            {
                return new FixResult(fc.Name.TextStart,
                    TextEdit.Replacement(fc.Name.TextStart, fc.Name.Width, alternateFunctionName),
                    TextEdit.Deletion(fc.ArgumentList.Expressions[0].Element.End, fc.ArgumentList.CloseParen.TextStart - fc.ArgumentList.Expressions[0].Element.End));
            }

            return new FixResult(cursorPosition);
        }
#endif
    }
}
