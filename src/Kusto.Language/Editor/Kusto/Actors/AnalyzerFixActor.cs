using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using System.Linq;
    using Utils;

    /// <summary>
    /// This is kusto actor that offers up code actions for code analyzers.
    /// </summary>
    public class AnalyzerFixActor : KustoActor
    {
        private readonly Dictionary<string, KustoAnalyzer> _codeToAnalyzerMap;
        private readonly Dictionary<string, KustoAnalyzer> _nameToAnalyzerMap;

        public AnalyzerFixActor(IReadOnlyList<KustoAnalyzer> analyzers = null)
        {
            analyzers = analyzers ?? KustoAnalyzers.All;

            _nameToAnalyzerMap = analyzers.ToDictionary(a => a.Name);

            // build code to analyzer map
            _codeToAnalyzerMap = new Dictionary<string, KustoAnalyzer>();
            foreach (var analyzer in analyzers)
            {
                foreach (var dx in analyzer.Diagnostics)
                {
                    _codeToAnalyzerMap[dx.Code] = analyzer;
                }
            }
        }

        public override void GetActions(
            KustoCode code,
            int position,
            int length,
            CodeActionOptions options,
            List<CodeAction> actions,
            CancellationToken cancellationToken)
        {
            var fixActions = new List<CodeAction>();

            foreach (var dx in options.RelatedDiagnostics)
            {
                // look for kusto analyzers that can fix their diagnostics
                if (_codeToAnalyzerMap.TryGetValue(dx.Code, out var analyzer))
                {
                    fixActions.Clear();
                    analyzer.GetFixActions(code, dx, options, fixActions, cancellationToken);

                    if (fixActions.Count > 0)
                    {
                        // add analyzer name to data so we can route this to the appropriate analyzer later when applied
                        actions.AddRange(fixActions.Select(a => a.AddData(analyzer.Name)));
                    }
                }
            }
        }

        public override CodeActionResult ApplyAction(
            KustoCode code,
            int position,
            int length,
            CodeActionOptions options,
            CodeAction action,
            CancellationToken cancellationToken)
        {
            var analyzerName = action.Data.LastOrDefault();          
            if (analyzerName != null)
            {
                // remove analyzer name from data to return it to its original state
                action = action.RemoveData(1);

                if (_nameToAnalyzerMap.TryGetValue(analyzerName, out var analyzer))
                {
                    return analyzer.ApplyFixAction(code, action, options, cancellationToken);
                }
            }

            return CodeActionResult.Failure("Unknown analyzer");
        }
    }
}