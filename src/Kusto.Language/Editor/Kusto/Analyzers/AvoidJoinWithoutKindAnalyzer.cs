using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class AvoidJoinWithoutKindAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidJoinWithoutKind,
                DiagnosticCategory.Correctness,
                DiagnosticSeverity.Warning,
                description: "Avoid using joins without specifying the join kind.",
                message: "Avoid using joins without specifying the join kind. The default join behavior may be unexpected.");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<JoinOperator>(op =>
                op.ConditionClause != null
                && !op.Parameters.Any(p => p.Name.SimpleName == "kind")
                ))
            {
                diagnostics.Add(_diagnostic.WithLocation(node.JoinKeyword));
            }
        }
    }
}