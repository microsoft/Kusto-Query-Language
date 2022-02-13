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
            foreach (var node in code.Syntax.GetDescendants<BinaryExpression>())
            {
                if (!node.Right.IsConstant && !node.Left.IsConstant)
                    continue;

                if (node.Kind == SyntaxKind.EqualExpression ||
                    node.Kind == SyntaxKind.HasExpression ||
                    node.Kind == SyntaxKind.NotEqualExpression ||
                    node.Kind == SyntaxKind.NotHasExpression ||
                    node.Kind == SyntaxKind.StartsWithExpression ||
                    node.Kind == SyntaxKind.NotStartsWithExpression)
                {
                    string constValue = null;

                    if (node.Right.IsConstant && node.Right.ResultType == ScalarTypes.String)
                    {
                        constValue = node.Right.ConstantValue as string;
                    }
                    else if (node.Left.IsConstant && node.Left.ResultType == ScalarTypes.String)
                    {
                        constValue = node.Left.ConstantValue as string;
                    }

                    if (!string.IsNullOrEmpty(constValue) && constValue.Length < 4)
                    {
                        // only report if db column is being used directly
                        if (!IsDbColumn(node.Right, code.Globals) && !IsDbColumn(node.Left, code.Globals))
                            continue;

                        diagnostics.Add(_diagnostic.WithLocation(node));
                    }
                }
            }
        }

        protected static bool IsDbColumn(Expression expr, GlobalState globals) =>
            expr.ReferencedSymbol is ColumnSymbol c && globals.GetTable(c) != null;
    }
}
