using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using System.Linq;
    using Utils;

    /// <summary>
    /// This is <see cref="KustoActor"/> that produces code actions for diagnostics that can be fixed.
    /// </summary>
    internal class DiagnosticFixActor : KustoActor
    {
        private readonly Dictionary<string, KustoFixer> _codeToFixerMap;
        private readonly Dictionary<string, KustoFixer> _nameToFixerMap;

        public DiagnosticFixActor(
            IReadOnlyList<KustoFixer> fixers = null)
        {
            fixers = fixers ?? KustoFixers.All;

            _nameToFixerMap = fixers.ToDictionary(a => a.Name);

            // build code to fixer map
            _codeToFixerMap = new Dictionary<string, KustoFixer>();
            foreach (var fixer in fixers)
            {
                foreach (var dx in fixer.Diagnostics)
                {
                    _codeToFixerMap[dx.Code] = fixer;
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
                    // look for the associated kusto fixer and get its fix actions
                    if (_codeToFixerMap.TryGetValue(dx.Code, out var fixer))
                    {
                        // get all diagnostics in selection range that have this same code
                        // so the fixer may choose to create fix all actions.
                        List<Diagnostic> selectionDiagnostics = null;
                        diagnosticsInSelectionMap?.TryGetValue(dx.Code, out selectionDiagnostics);

                        fixActions.Clear();
                        fixer.GetFixActions(code, dx, selectionDiagnostics ?? EmptyReadOnlyList<Diagnostic>.Instance, options, fixActions, cancellationToken);

                        if (fixActions.Count > 0)
                        {
                            // add fixer name to action data so we can route this action back to the same fixer when applied later
                            actions.AddRange(fixActions.Select(a => EncodeFixerName(a, fixer.Name)));
                        }
                    }
                }
            }
        }

        private CodeAction EncodeFixerName(CodeAction action, string fixerName)
        {
            return ActorUtilities.AddData(action, fixerName);
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
            var fixerName = action.Data.LastOrDefault();          
            if (!string.IsNullOrEmpty(fixerName)
                && _nameToFixerMap.TryGetValue(fixerName, out var fixer))
            {
                // return action to original state (before fixer name was added)
                action = action.RemoveData(1);
                return fixer.ApplyFixAction(code, action, caretPosition, options, cancellationToken);
            }

            return CodeActionResult.Failure("Unknown fixer");
        }
    }
}