using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class AvoidUsingShortStringComparisionAnalyzer : KustoAnalyzer
    {
        private const int MinimumNoWarnLength = 4;

        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidUsingShortStringComparision,
                category: DiagnosticCategory.Performance,
                severity: DiagnosticSeverity.Suggestion,
                description: "Avoid short string comparisons",
                message: "Avoid using short strings (less than 4 characters) for string comparison operations (see: https://aka.ms/kusto.stringterms).");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var exp in code.Syntax.GetDescendants<Expression>(x =>
                x is BinaryExpression
                || x is HasAnyExpression
                || x is HasAllExpression
                ))
            {
                if (exp is BinaryExpression bex)
                {
                    if (!bex.Right.IsConstant && !bex.Left.IsConstant)
                        continue;

                    if (bex.Kind == SyntaxKind.EqualExpression ||
                        bex.Kind == SyntaxKind.NotEqualExpression ||
                        bex.Kind == SyntaxKind.HasExpression ||
                        bex.Kind == SyntaxKind.NotHasExpression ||
                        bex.Kind == SyntaxKind.HasCsExpression ||
                        bex.Kind == SyntaxKind.NotHasCsExpression ||
                        bex.Kind == SyntaxKind.StartsWithExpression ||
                        bex.Kind == SyntaxKind.NotStartsWithExpression)
                    {
                        CheckShortConstants(bex.Left, bex.Right, bex, code.Globals, diagnostics);
                    }
                }
                else if (exp is HasAnyExpression anyex)
                {
                    foreach (var sep in anyex.Right.Expressions)
                    {
                        CheckShortConstants(anyex.Left, sep.Element, sep.Element, code.Globals, diagnostics);
                    }
                }
                else if (exp is HasAllExpression allex)
                {
                    foreach (var sep in allex.Right.Expressions)
                    {
                        CheckShortConstants(allex.Left, sep.Element, sep.Element, code.Globals, diagnostics);
                    }
                }
            }
        }

        private static void CheckShortConstants(Expression left, Expression right, SyntaxNode location, GlobalState globals, List<Diagnostic> diagnostics)
        {
            string constValue = null;

            if (right.IsConstant && right.ResultType == ScalarTypes.String)
            {
                constValue = right.ConstantValue as string;
            }
            else if (left.IsConstant && left.ResultType == ScalarTypes.String)
            {
                constValue = left.ConstantValue as string;
            }

            if (!string.IsNullOrEmpty(constValue) && constValue.Length < MinimumNoWarnLength)
            {
                // only report if db column is being used directly
                if (IsDbColumn(right, globals) || IsDbColumn(left, globals))
                {
                    diagnostics.Add(_diagnostic.WithLocation(location));
                }
            }
        }

        protected static bool IsDbColumn(Expression expr, GlobalState globals) =>
            expr.ReferencedSymbol is ColumnSymbol c && globals.GetTable(c) != null;
    }
}
