using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class AvoidUsingToBoolOnNumericsAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidUsingToBoolOnNumerics,
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                description: "Avoid using tobool on numeric arguments",
                message: "Avoid using tobool on numeric arguments, use comparison operators instead");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<FunctionCallExpression>())
            {
                if ((node.ReferencedSymbol == Functions.ToBool || node.ReferencedSymbol == Functions.ToBoolean)
                    && node.ArgumentList.Expressions.Count > 0)
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
        }
    }
}