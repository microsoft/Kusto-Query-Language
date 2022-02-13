using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class ColumnHasSameNameAsVariableAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.ColumnHasSameNameAsVariable,
                category: DiagnosticCategory.Correctness,
                severity: DiagnosticSeverity.Warning,
                description: "A variable or parameter is ignored in favor of a column with the same name.");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            var variableNames = new HashSet<string>();

            SyntaxNode.WalkNodes(code.Syntax, node =>
            {
                if (node is LetStatement ls
                    && ls.Name.ReferencedSymbol is VariableSymbol vs
                    && vs.Type is ScalarSymbol)
                {
                    variableNames.Add(ls.Name.SimpleName);
                }
                else if (node is FunctionParameter fp
                    && fp.NameAndType.Name.ResultType is ScalarSymbol)
                {
                    variableNames.Add(fp.NameAndType.Name.SimpleName);
                }
                else if (node is NameReference name)
                {
                    // check if a column reference has the same name as a variable
                    if (name.ReferencedSymbol is ColumnSymbol c
                        && variableNames.Contains(c.Name))
                    {
                        // if the name would have bound to a variable or parameter then add a warning
                        var symbol = code.GetSpeculativeReferencedSymbol(name.TextStart, c.Name, SymbolMatch.Local, cancellationToken);
                        if (symbol != null)
                        {
                            diagnostics.Add(_diagnostic.WithLocation(name));
                        }
                    }
                }
            });
        }
    }
}