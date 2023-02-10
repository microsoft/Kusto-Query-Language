using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Utils;
    using static AnalyzerUtilities;

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

        protected override void GetFixAction(KustoCode code, Diagnostic dx, CodeActionOptions options, List<CodeAction> actions, CancellationToken cancellationToken)
        {
            if (code.Syntax.GetNodeAt(dx.Start, dx.Length) is FunctionCallExpression fc)
            {
                if (fc.ArgumentList.Expressions.Count == 2
                    && fc.ArgumentList.Expressions[1].Element.ConstantValue is string format)
                {
                    if (GetAlternativeFunctionOrDateTimePart(format) is string altFunctionOrPart)
                    {
                        var fn = KustoFacts.DateTimeParts.Contains(altFunctionOrPart)
                            ? $"datetime_part('{altFunctionOrPart}',)"
                            : altFunctionOrPart;

                        actions.Add(CodeAction.Create(
                            kind: "convert_format_datetime",
                            title: $"Change to '{fn}'",
                            description: $"Change use of function 'format_datetime' to function '{fn}'",
                            data: new string[] {
                            dx.Start.ToString(),
                            dx.Length.ToString(),
                            altFunctionOrPart }));
                    }
                }
            }
        }

        private static string GetAlternativeFunctionOrDateTimePart(string format)
        {
            switch (format)
            {
                case "d":
                case "dd":
                    return "dayofmonth";
                case "fff":
                case "FFF":
                    return "millisecond";
                case "ffffff":
                case "FFFFFF":
                    return "microsecond";
                case "H":
                case "HH":
                    return "hourofday";
                case "m":
                case "mm":
                    return "minute";
                case "M":
                case "MM":
                    return "monthofyear";
                case "s":
                case "ss":
                    return "second";
                case "y":
                case "yy":
                case "yyyy":
                    return "getyear";
                default:
                    return null;
            }
        }

        protected override FixEdits GetFixEdits(KustoCode code, ApplyAction action, int cursorPosition, CodeActionOptions options, CancellationToken cancellationToken)
        {
            if (action.Data.Count == 3
                && Int32.TryParse(action.Data[0], out var start)
                && Int32.TryParse(action.Data[1], out var length)
                && action.Data[2] is string altFunctionOrPart
                && code.Syntax.GetNodeAt(start, length) is FunctionCallExpression fc
                && fc.ArgumentList.Expressions.Count == 2)
            {
                var edits = new List<TextEdit>();

                if (KustoFacts.DateTimeParts.Contains(altFunctionOrPart))
                {
                    // change function name
                    edits.AddRenameEdits(fc.Name.Name, "datetime_part");

                    // insert new first argument
                    edits.AddInsertArgumentEdits(fc, 0, $"'{altFunctionOrPart}'");
                }
                else
                {
                    // change function name
                    edits.AddRenameEdits(fc.Name.Name, altFunctionOrPart);
                }

                // remove format argument
                if (fc.ArgumentList.Expressions.Count > 1)
                    edits.AddRemoveArgumentEdits(fc.ArgumentList.Expressions[1].Element);

                if (GetFunctionCall(fc) is FunctionCallExpression outerFc 
                    && (outerFc.Name.SimpleName == Functions.ToLong.Name
                        || outerFc.Name.SimpleName == Functions.ToInt.Name))
                {
                    // remove outer tolong/toint function call
                    edits.AddRemoveOuterFunctionCallEdits(outerFc, fc);
                }

                return new FixEdits(fc.Name.TextStart, edits);
            }

            return new FixEdits(cursorPosition);
        }
    }
}
