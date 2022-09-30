using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class AvoidUsingObsoleteFunctionsAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidUsingObsoleteFunctions,
                DiagnosticCategory.Correctness,
                DiagnosticSeverity.Warning,
                description: "Avoid using obsolete/deprecated functions.");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<FunctionCallExpression>(
                fc => fc.ReferencedSymbol is FunctionSymbol fs 
                    && (fs.IsObsolete || (fc.ReferencedSignature is Signature sig && sig.IsObsolete))))
            {
                if (node.ReferencedSymbol is FunctionSymbol fs)
                {
                    if (fs.IsObsolete)
                    {
                        diagnostics.Add(
                            _diagnostic.WithMessage($"The function '{fs.Name}' is deprecated; use '{fs.Alternative}' instead.")
                            .WithLocation(node.Name)
                            );
                    }
                    else if (node.ReferencedSignature is Signature sig && sig.IsObsolete)
                    {
                        diagnostics.Add(
                            _diagnostic.WithMessage($"This form of the function '{fs.Name}' is deprecated; use '{sig.Alternative}' instead.")
                            .WithLocation(node.Name)
                            );
                    }
                }
            }
        }
    }
}