using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    /// <summary>
    /// Analyzer for detecting usage of short string comparisons
    /// </summary>
    internal class AvoidUsingShortStringComparisonRule : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                "KustoAvoidUsingShortStringComparison",
                category: DiagnosticCategory.Performance,
                severity: DiagnosticSeverity.Suggestion,
                message: "Avoid using short strings (less than 4 characters) for string comparison operations (see: https://aka.ms/kusto.stringterms).");

        public override IReadOnlyList<Diagnostic> Analyze(KustoCode code, CancellationToken cancellationToken)
        {
            var diagnostics = new List<Diagnostic>();

            foreach (var node in code.Syntax.GetDescendants<BinaryExpression>())
            {
                if (!node.Right.IsConstant && !node.Left.IsConstant)
                {
                    continue;
                }

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
                        diagnostics.Add(_diagnostic.WithLocation(node));
                    }
                }
            }

            return diagnostics;
        }
    }
}