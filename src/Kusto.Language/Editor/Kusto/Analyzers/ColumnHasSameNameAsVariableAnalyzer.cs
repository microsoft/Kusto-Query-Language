using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    /// <summary>
    /// An analyzer that warns if any column reference is hiding a possible reference to
    /// a local variable or parameter in scope, so the user is aware and can choose
    /// to change the name of the variable or parameter to not conflict.
    /// </summary>
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
            var localNames = new HashSet<string>();

            SyntaxNode.WalkNodes(code.Syntax, node =>
            {
                if (node is NameDeclaration nd
                    && (nd.ReferencedSymbol is VariableSymbol || nd.ReferencedSymbol is ParameterSymbol)
                    && nd.ReferencedSymbol.IsScalar)
                {
                    localNames.Add(nd.SimpleName);
                }
                else if (node is NameReference name
                    && !IsPathSelector(name))  // locals are not accessible by paths
                {
                    // Check if a column reference has the same name as a known local
                    // note: this is merely to improve performance by reducing the number of
                    // speculative lookups to only those that match known local names.
                    if (name.ReferencedSymbol is ColumnSymbol c
                        && localNames.Contains(c.Name))
                    {
                        // search for an otherwise matching local symbol in scope
                        var symbol = code.GetSpeculativeReferencedSymbol(name.TextStart, c.Name, SymbolMatch.Local, cancellationToken);
                        if (symbol != null)
                        {
                            // since this name would have bound to a local symbol in scope if a column did not have the same name
                            // add a warning so the user 
                            diagnostics.Add(_diagnostic.WithLocation(name));
                        }
                    }
                }
            });
        }

        private static bool IsPathSelector(NameReference name) =>
            name.Parent is PathExpression p && p.Selector == name;
    }
}