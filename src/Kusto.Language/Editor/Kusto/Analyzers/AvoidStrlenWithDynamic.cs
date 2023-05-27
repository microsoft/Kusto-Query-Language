using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class AvoidStrlenWithDynamicAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidStrlenWithDynamic,
                DiagnosticCategory.Performance,
                DiagnosticSeverity.Suggestion,
                description: "Avoid using the strlen() with dynamic arguments.",
                message: $"Avoid using strlen() with dynamic arguments that may be large bags or arrays.");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var fc in code.Syntax.GetDescendants<FunctionCallExpression>())
            {
                if (fc.ReferencedSymbol == Functions.Strlen
                    && fc.ArgumentList.Expressions.Count == 1
                    && (fc.ArgumentList.Expressions[0].Element.ResultType.IsDynamicArrayOrBag()))
                {
                    diagnostics.Add(_diagnostic.WithLocation(fc));
                }
            }
        }
    }
}