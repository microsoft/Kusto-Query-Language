using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using System.Linq;
    using Utils;

    /// <summary>
    /// This is kusto actor that produces code actions for analyzer diagnostics.
    /// It does this via requesting the actions directly from the analyzers that created the diagnostics.
    /// </summary>
    internal class AnalyzerFixActor : KustoActor
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
            KustoCodeService service,
            KustoCode code,
            int position,
            int selectionStart, 
            int selectionLength,
            CodeActionOptions options,
            List<CodeAction> actions,
            bool waitForAnalysis,
            CancellationToken cancellationToken)
        {
            IReadOnlyList<Diagnostic> analyzerDiagnostics = null;
            if (waitForAnalysis)
            {
                analyzerDiagnostics = service.GetAnalyzerDiagnostics(waitForAnalysis, cancellationToken);
            }
            else
            {
                // don't wait for analyzers to run either
                service.TryGetCachedAnalyzerDiagnostics(out analyzerDiagnostics);
            }

            // if analyzer diagnostics are not available then return no actions
            if (analyzerDiagnostics != null)
            {
                var diagnosticsAtPosition =
                    analyzerDiagnostics
                    .Where(d =>
                        TextRange.Overlaps(position, 0, d.Start, d.Length)
                        && options.DiagnosticFilter.IsDiagnosticEnabled(d))
                    .ToList();

                // if no actual selection then assume selection is entire text
                if (selectionLength == 0)
                {
                    selectionStart = 0;
                    selectionLength = code.Text.Length;
                }

                // make a map of all dx codes to any diagnostics within the selection
                // so we can pass along all similar diagnostics in the selection range
                // but only if the position requested is also within the selection range.
                Dictionary<string, List<Diagnostic>> diagnosticsInSelectionMap = null;
                if (position >= selectionStart && position < selectionStart + selectionLength)
                {
                    diagnosticsInSelectionMap = GetCodeToDiagnosticMap(
                        analyzerDiagnostics
                        .Where(d =>
                            TextRange.Overlaps(selectionStart, selectionLength, d.Start, d.Length)
                            && options.DiagnosticFilter.IsDiagnosticEnabled(d)));
                }

                var fixActions = new List<CodeAction>();

                // now get the actions for each diagnostic
                foreach (var dx in diagnosticsAtPosition)
                {
                    // look for the associated kusto analyzer and get its fix actions
                    if (_codeToAnalyzerMap.TryGetValue(dx.Code, out var analyzer))
                    {
                        // get all diagnostics in selection range that have this same code
                        // so the analzyer may choose to create fix all actions.
                        List<Diagnostic> selectionDiagnostics = null;
                        diagnosticsInSelectionMap?.TryGetValue(dx.Code, out selectionDiagnostics);

                        fixActions.Clear();
                        analyzer.GetFixActions(code, dx, selectionDiagnostics ?? EmptyReadOnlyList<Diagnostic>.Instance, options, fixActions, cancellationToken);

                        if (fixActions.Count > 0)
                        {
                            // add analyzer name to action data so we can route this action back to the same analyzer later when applied
                            actions.AddRange(fixActions.Select(a => EncodeAnalyzerName(a, analyzer.Name)));
                        }
                    }
                }
            }
        }

        private CodeAction EncodeAnalyzerName(CodeAction action, string analyzerName)
        {
            return ActorUtilities.AddData(action, analyzerName);
        }

        private static Dictionary<string, List<Diagnostic>> GetCodeToDiagnosticMap(IEnumerable<Diagnostic> diagnostics)
        {
            var map = new Dictionary<string, List<Diagnostic>>();

            foreach (var dx in diagnostics)
            {
                if (!map.TryGetValue(dx.Code, out var list))
                {
                    list = new List<Diagnostic>();
                    map.Add(dx.Code, list);
                }

                list.Add(dx);
            }

            return map;
        }

        public override CodeActionResult ApplyAction(
            KustoCodeService service,
            KustoCode code,
            ApplyAction action,
            int caretPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            var analyzerName = action.Data.LastOrDefault();          
            if (!string.IsNullOrEmpty(analyzerName)
                && _nameToAnalyzerMap.TryGetValue(analyzerName, out var analyzer))
            {
                // return action to original state (before analyzer name was added)
                action = action.RemoveData(1);
                return analyzer.ApplyFixAction(code, action, caretPosition, options, cancellationToken);
            }

            return CodeActionResult.Failure("Unknown analyzer");
        }
    }
}