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
    internal class AvoidUsingIsNullStringComparisonAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic_equals =
            new Diagnostic(
                "KS501",
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                description: "Avoid using isnull on string arguments, use isempty() instead");

        private static readonly Diagnostic _diagnostic_not_equals =
            new Diagnostic(
                "KS502",
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                description: "Avoid using isnotnull on string arguments, use isnotempty() instead");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic_equals, _diagnostic_not_equals };
        }

        public override IReadOnlyList<Diagnostic> Analyze(KustoCode code, CancellationToken cancellationToken)
        {
            var diagnostics = new List<Diagnostic>();

            foreach (var node in code.Syntax.GetDescendants<FunctionCallExpression>())
            {
                if ((node.ReferencedSymbol == Functions.IsNull ||
                    node.ReferencedSymbol == Functions.IsNotNull) && 
                    node.ArgumentList.Expressions.Count > 0 && 
                    node.ArgumentList.Expressions[0].Element.ResultType == ScalarTypes.String)
                {
                    if (node.ReferencedSymbol == Functions.IsNull)
                    {
                        diagnostics.Add(_diagnostic_equals.WithLocation(node));
                    }
                    else
                    {
                        diagnostics.Add(_diagnostic_not_equals.WithLocation(node));
                    }
                }

            }

            return diagnostics;
        }
    }
}