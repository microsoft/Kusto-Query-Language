using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class AvoidUsingLegacyPartitionAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidUsingLegacyPartition,
                category: DiagnosticCategory.General,
                severity: DiagnosticSeverity.Suggestion,
                description: "It is recommended to use the `native` or `shuffle` strategy of the partition operator. The legacy mode is limited to 64 partitions and is less efficient than the `native` or `shuffle` strategies.");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<PartitionOperator>())
            {
                if (node.Operand is PartitionSubquery)
                {
                    bool isUsingLegacyStrategy = true;
                    if (node.Parameters.Count > 0)
                    {
                        foreach (var parameter in node.Parameters)
                        {
                            if (parameter.Name.SimpleName.Equals(SyntaxKind.HintDotStrategyKeyword.GetText()))
                            {
                                if (parameter.Expression.IsLiteral)
                                {
                                    isUsingLegacyStrategy = parameter.Expression.LiteralValue.Equals("legacy");
                                }
                                else
                                {
                                    // invalid value is provided to the hint.strategy. it will fail anyway. no need to produce a warning message.
                                    return;
                                }
                            }
                            else if (parameter.Name.SimpleName.Equals(SyntaxKind.HintDotShuffleKeyKeyword.GetText()))
                            {
                                isUsingLegacyStrategy = false;
                            }
                        }
                    }

                    if (isUsingLegacyStrategy)
                    {
                        diagnostics.Add(_diagnostic.WithLocation(node));
                    }
                }
            }
        }
    }
}