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
                if (node is NameReference nr 
                    && !(nr.Parent is FunctionCallExpression) 
                    && nr.CalledFunctionHasErrors
                    && nr.ReferencedSignature is Signature sig1)
                {
                    AddErrors(code, sig1, nr.GetCalledFunctionDiagnostics(), nr, diagnostics);
                }
                else if (node is FunctionCallExpression fc 
                    && fc.CalledFunctionHasErrors
                    && fc.Name.ReferencedSignature is Signature sig2)
                {
                    AddErrors(code, sig2, fc.GetCalledFunctionDiagnostics(), fc.Name, diagnostics);
                }
            });
        }

        private static void AddErrors(KustoCode code, Signature sig, IReadOnlyList<Diagnostic> calledFunctionDiagnostics, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            // add called function errors if the function is declared in same query (not from db, etc)
            if (sig.Symbol != null
                && sig.Declaration != null
                && sig.Declaration.Tree == code.Tree)
            {
                diagnostics.AddRange(calledFunctionDiagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));
            }

            diagnostics.Add(_diagnostic.WithMessage($"The function '{sig.Symbol.Name}' has errors in its definition").WithLocation(location));
        }
    }
}