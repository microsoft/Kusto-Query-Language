using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// A <see cref="KustoAnalyzer"/> implements additional semantic analysis beyond that done by <see cref="KustoCode"/>.
    /// Analyzers are invoked via <see cref="CodeService.GetAnalyzerDiagnostics"/>.
    /// The analyzer may also provide fix actions for the diagnostics they report.
    /// </summary>
    internal abstract class KustoAnalyzer
    {
        /// <summary>
        /// The name of this analyzer.
        /// This is used to find the analyzer that created the <see cref="CodeAction"/> when the action is being applied.
        /// </summary>
        public virtual string Name => this.GetType().Name;

        /// <summary>
        /// Override this method to suppy the example set of diagnostics that the analyzer produces.
        /// </summary>
        protected abstract IEnumerable<Diagnostic> GetDiagnostics();

        /// <summary>
        /// The cached set of example diagnostics
        /// </summary>
        private IReadOnlyList<Diagnostic> _diagnostics;

        /// <summary>
        /// The diagnostics produced by this analyzer.
        /// </summary>
        public IReadOnlyList<Diagnostic> Diagnostics
        {
            get
            {
                if (_diagnostics == null)
                {
                    _diagnostics = GetDiagnostics().ToReadOnly();
                }

                return _diagnostics;
            }
        }

        /// <summary>
        /// Analyzes the <see cref="KustoCode"/> and outputs any diagnostics found into the diagnostics list.
        /// </summary>
        public abstract void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the fix actions for the diagnostic
        /// </summary>
        public virtual void GetFixActions(
            KustoCode code,
            Diagnostic cursorDiagnostic,
            IReadOnlyList<Diagnostic> selectionDiagnostics,
            CodeActionOptions options,
            List<CodeAction> actions,
            CancellationToken cancellationToken)
        {
            var originalCount = actions.Count;
            GetFixAction(code, cursorDiagnostic, options, actions, cancellationToken);

            if (actions.Count == originalCount + 1
                && selectionDiagnostics.Count > 1)
            {
                var primaryAction = actions[originalCount];
                var relatedActions = new List<ApplyAction>();
                var tmpActions = new List<CodeAction>();

                foreach (var dx in selectionDiagnostics)
                {
                    tmpActions.Clear();
                    GetFixAction(code, dx, options, tmpActions, cancellationToken);
                    relatedActions.AddRange(tmpActions.OfType<ApplyAction>().Where(a => a.Kind == primaryAction.Kind));
                }

                // if there are multiple related actions then change action to a menu
                // with the original action and a fix all action.
                if (relatedActions.Count > 1)
                {
                    // replace action with menu of choices
                    var newAction =
                        CodeAction.CreateMenu(
                            primaryAction.Title,
                            primaryAction.Description,
                            new []
                            {
                                primaryAction.WithTitle("Apply"),
                                CodeAction.CreateFixAll("Fix All", "Apply to all occurences in query or selection.", relatedActions)
                            });

                    actions[originalCount] = newAction;
                }
            }
        }

        /// <summary>
        /// Gets the fix actions for the diagnostic
        /// </summary>
        protected virtual void GetFixAction(
            KustoCode code,
            Diagnostic dx,
            CodeActionOptions options,
            List<CodeAction> actions,
            CancellationToken cancellationToken)
        {
        }

        /// <summary>
        /// Applies the fix action
        /// </summary>
        public virtual CodeActionResult ApplyFixAction(
            KustoCode code,
            ApplyAction action,
            int caretPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            if (action is MultiAction ma)
            {
                // apply the step actions
                return ApplyFixActions(code, ma.Actions, caretPosition, options, cancellationToken);
            }
            else
            {
                return ApplyFixActions(code, new[] { action }, caretPosition, options, cancellationToken);
            }
        }

        protected virtual CodeActionResult ApplyFixActions(
            KustoCode code,
            IReadOnlyList<ApplyAction> actions,
            int caretPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            var suggestedCaretPosition = caretPosition;
            var suggestedCaretBias = PositionBias.Left;

            var edits = new List<StringEdit>();
            foreach (var action in actions)
            {
                var result = GetFixEdits(code, action, caretPosition, options, cancellationToken);
                edits.AddRange(result.Edits);

                if (EditContainsPosition(result.Edits, caretPosition))
                {
                    suggestedCaretPosition = result.NewCaretPosition;
                    suggestedCaretBias = result.Bias;
                }
            }

            // put in sorted order
            edits.Sort((a, b) => a.Start - b.Start);

            if (edits.Count > 0
                && AreNonOverlapping(edits))
            {
                var newText = new EditString(code.Text).ApplyAll(edits);
                var newCursorPosition = newText.GetCurrentPosition(suggestedCaretPosition, suggestedCaretBias);
                return new CodeActionResult(newText, newCursorPosition);
            }

            return CodeActionResult.Nothing;
        }

        private static bool EditContainsPosition(IReadOnlyList<StringEdit> edits, int position)
        {
            if (edits.Count > 0)
            {
                var min = edits.Min(a => a.Start);
                var max = edits.Max(a => a.Start + a.DeleteLength);

                return position >= min && position < max;
            }
            else
            {
                return false;
            }
        }

        private bool AreNonOverlapping(IReadOnlyList<StringEdit> edits)
        {
            for (int i = 1; i < edits.Count; i++)
            {
                if (edits[i].Start < edits[i - 1].Start + edits[i - 1].DeleteLength)
                    return false;
            }

            return true;
        }

        protected virtual FixResult GetFixEdits(
            KustoCode code,
            ApplyAction action,
            int cursorPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            return new FixResult(cursorPosition, null);
        }

        /// <summary>
        /// Represents the result from a call to GetFixEdits
        /// </summary>
        protected class FixResult
        {
            public IReadOnlyList<StringEdit> Edits { get; }
            public int NewCaretPosition { get; }
            public PositionBias Bias { get; }

            /// <summary>
            /// Construct a new <see cref="FixResult"/>
            /// </summary>
            /// <param name="newCaretPosition">The new position of the cursor in pre-edit units.</param>
            /// <param name="bias">The bias to use when computing the new cursor position in post-edit units.</param>
            /// <param name="edits">A list of non-overlapping edits in pre-edit units.</param>
            public FixResult(int newCaretPosition, PositionBias bias, IReadOnlyList<StringEdit> edits)
            {
                this.NewCaretPosition = newCaretPosition;
                this.Bias = bias;
                this.Edits = edits ?? EmptyReadOnlyList<StringEdit>.Instance;
            }

            /// <summary>
            /// Construct a new <see cref="FixResult"/>
            /// </summary>
            /// <param name="newCaretPosition">The new position of the cursor in pre-edit units.</param>
            /// <param name="edits">A list of non-overlapping edits in pre-edit units.</param>
            public FixResult(int newCaretPosition, IReadOnlyList<StringEdit> edits)
                : this(newCaretPosition, PositionBias.Left, edits)
            {
            }

            /// <summary>
            /// Construct a new <see cref="FixResult"/>
            /// </summary>
            /// <param name="newCaretPosition">The new position of the cursor in pre-edit units.</param>
            /// <param name="bias">The bias to use when computing the new cursor position in post-edit units.</param>
            /// <param name="edits">A list of non-overlapping edits in pre-edit units.</param>
            public FixResult(int newCaretPosition, PositionBias bias, params StringEdit[] edits)
                : this(newCaretPosition, bias, (IReadOnlyList<StringEdit>)edits)
            {
            }

            /// <summary>
            /// Construct a new <see cref="FixResult"/>
            /// </summary>
            /// <param name="newCaretPosition">The new position of the cursor in pre-edit units.</param>
            /// <param name="edits">A list of non-overlapping edits in pre-edit units.</param>
            public FixResult(int newCaretPosition, params StringEdit[] edits)
                : this(newCaretPosition, PositionBias.Left, (IReadOnlyList<StringEdit>)edits)
            {
            }
        }
    }
}