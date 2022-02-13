using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class CalledFunctionHasErrorsAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.CalledFunctionHasErrors,
                category: DiagnosticCategory.General,
                severity: DiagnosticSeverity.Warning,
                description: "A called function has errors in its definition");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            SyntaxNode.WalkNodes(code.Syntax, node =>
            {
                if (node is NameReference nr && !(nr.Parent is FunctionCallExpression) && nr.CalledFunctionHasErrors)
                {
                    diagnostics.Add(_diagnostic.WithMessage($"The function '{nr.SimpleName}' has errors in its definition.").WithLocation(nr));
                }
                else if (node is FunctionCallExpression fc && fc.CalledFunctionHasErrors)
                {
                    diagnostics.Add(_diagnostic.WithMessage($"The function '{fc.Name.SimpleName}' has errors in its definition.").WithLocation(fc.Name));
                }
            });
        }
    }
}