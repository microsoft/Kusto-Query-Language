using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Utils;
    using static AnalyzerUtilities;

    internal class AvoidUsingShortStringComparisionAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidUsingShortStringComparision,
                category: DiagnosticCategory.Performance,
                severity: DiagnosticSeverity.Suggestion,
                description: "Avoid short string comparisons",
                message: "Avoid using short strings (less than 3 characters) for string comparison operations (see: https://aka.ms/kusto.stringterms).");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var exp in code.Syntax.GetDescendants<Expression>(x => IsRelevantComparison(x)))
            {
                if (HasDbColumnOperand(exp, code.Globals) // only db columns have indices
                    && GetFirstShortStringOperand(exp, code.Globals) is Expression location)
                {
                    diagnostics.Add(_diagnostic.WithLocation(location));
                }
            }
        }

        /// <summary>
        /// Returns the first operand that is a short string constant.
        /// </summary>
        public static Expression GetFirstShortStringOperand(Expression expr, GlobalState globals)
        {
            return GetFirstMatchingOperand(expr, operand => IsShortStringConstant(operand, globals), includeListOperandValues: true);
        }

        /// <summary>
        /// Returns true if the expression is a comparison expression relevant to this analyzer.
        /// </summary>
        private static bool IsRelevantComparison(Expression expr)
        {
            switch (expr.Kind)
            {
                case SyntaxKind.EqualExpression:
                case SyntaxKind.NotEqualExpression:
                case SyntaxKind.HasExpression:
                case SyntaxKind.NotHasExpression:
                case SyntaxKind.HasCsExpression:
                case SyntaxKind.NotHasCsExpression:
                case SyntaxKind.StartsWithExpression:
                case SyntaxKind.NotStartsWithExpression:
                case SyntaxKind.HasAnyExpression:
                case SyntaxKind.HasAllExpression:
                    return true;

                default:
                    return false;
            }
        }
    }
}
