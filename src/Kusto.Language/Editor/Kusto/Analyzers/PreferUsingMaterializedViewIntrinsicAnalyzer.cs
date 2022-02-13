using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class PreferUsingMaterializedViewIntrinsicAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.PreferUsingMaterializedViewIntrinsic,
                DiagnosticCategory.Performance,
                DiagnosticSeverity.Suggestion,
                description: "Prefer using the `materialized_view()` function where appropriate",
                message: $"Consider using the `materialized_view()` function to query the materialized part only" + Environment.NewLine +
                         $"See: https://aka.ms/mvqueries");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            if (code.Kind == CodeKinds.Query)
            {
                foreach (var nameReference in code.Syntax.GetDescendants<NameReference>())
                {
                    if (nameReference.ReferencedSymbol is MaterializedViewSymbol materializedViewSymbol)
                    {
                        AnalyzeMaterializedViewSymbol(nameReference, materializedViewSymbol, code, _diagnostic, diagnostics);
                    }
                }
            }
        }

        /// <summary>
        /// List of functions that identify down sampling materialized views
        /// </summary>
        private static readonly HashSet<string> s_downSamplingFunctionNames = new HashSet<string>
        {
            "bin", "bin_at", "startofday", "startofmonth", "startofweek", "startofyear"
        };

        /// <summary>
        /// Try to identify whether the materialized view being referenced is a down sampling one, by checking 
        /// if it has a group by key with one of the functions in <see cref="s_downSamplingFunctionNames"/>. 
        /// If it is, suggest to change to using the materialized_view() function. 
        /// </summary>
        /// <remarks>
        /// Rule will only apply if the downsampling function is used directly in group by keys: 
        ///     T | summarize count() by bin(Timestamp, 1d)
        ///     OR 
        ///     T | summarize count() by Day = bin(Timestamp, 1d)
        ///     
        /// If the group by key points to an extended element, rule will not identify this case: 
        ///     T | extend Day=bin(Timestamp, 1d) | summarize count() by Day
        /// </remarks>
        private static void AnalyzeMaterializedViewSymbol(
            NameReference nameReference,
            MaterializedViewSymbol materializedViewSymbol,
            KustoCode code,
            Diagnostic dx,
            List<Diagnostic> diagnostics)
        {
            if (materializedViewSymbol.MaterializedViewKind == MaterializedViewKind.Other)
            {
                // view query was analyzed and is not a downsampling view
                return;
            }
            else if (materializedViewSymbol.MaterializedViewKind == MaterializedViewKind.Downsampling)
            {
                diagnostics.Add(dx.WithLocation(nameReference));
            }
            else
            {
                // Kind=Unknown. Analyze...
                try
                {
                    var mvCode = KustoCode.ParseAndAnalyze(materializedViewSymbol.MaterializedViewQuery, code.Globals);
                    var mvDiagnostics = mvCode.GetDiagnostics();
                    if (mvDiagnostics.Count == 0)
                    {
                        var statements = ((QueryBlock)mvCode.Syntax).Statements;
                        var mvStatement = statements.Last();
                        var aggregations = mvStatement.GetDescendants<SummarizeOperator>();
                        if (aggregations.Count == 1)
                        {
                            var aggregation = aggregations.Single();
                            if (aggregation.ByClause.Expressions.Any(expression =>
                                    (expression.Element is FunctionCallExpression functionCall && s_downSamplingFunctionNames.Contains(functionCall.Name.SimpleName))
                                    ||
                                    (expression.Element is SimpleNamedExpression simpleNamed &&
                                     simpleNamed.Expression is FunctionCallExpression functionCall2 &&
                                     s_downSamplingFunctionNames.Contains(functionCall2.Name.SimpleName))))
                            {
                                diagnostics.Add(dx.WithLocation(nameReference));
                                materializedViewSymbol.MaterializedViewKind = MaterializedViewKind.Downsampling;
                                return;
                            }
                        }
                    }
                }
                catch (Exception) { } // ignore all parsing failures

                // Mark as analyzed to avoid recalculations
                materializedViewSymbol.MaterializedViewKind = MaterializedViewKind.Other;
            }
        }
    }
}