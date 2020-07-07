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
    internal class AvoidUsingToBoolOnNumericsAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                "AvoidUsingToBoolOnNumerics",
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                message: "Avoid using tobool on numeric arguments, use comparison operators instead");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override IReadOnlyList<Diagnostic> Analyze(KustoCode code, CancellationToken cancellationToken)
        {
            var diagnostics = new List<Diagnostic>();

            foreach (var node in code.Syntax.GetDescendants<FunctionCallExpression>())
            {
                if ((node.ReferencedSymbol == Functions.ToBool || node.ReferencedSymbol == Functions.ToBoolean) && node.ArgumentList.Expressions.Count > 0)
                {
                    var firstArgumentType = node.ArgumentList.Expressions[0].Element.ResultType;
                    if (firstArgumentType == ScalarTypes.DateTime ||
                        firstArgumentType == ScalarTypes.Int ||
                        firstArgumentType == ScalarTypes.Decimal ||
                        firstArgumentType == ScalarTypes.Guid ||
                        firstArgumentType == ScalarTypes.Long ||
                        firstArgumentType == ScalarTypes.Real ||
                        firstArgumentType == ScalarTypes.TimeSpan)
                    {
                        diagnostics.Add(_diagnostic.WithLocation(node));
                    }
                }
            }

            return diagnostics;
        }
    }
}