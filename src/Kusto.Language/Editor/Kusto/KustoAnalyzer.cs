using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// The base class for any <see cref="KustoCode"/> analyzer.
    /// </summary>
    public abstract class KustoAnalyzer : CodeAnalyzer
    {
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
        public override IReadOnlyList<Diagnostic> Diagnostics
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
                var relatedActions = new List<CodeAction>();
                var tmpActions = new List<CodeAction>();

                foreach (var dx in selectionDiagnostics)
                {
                    tmpActions.Clear();
                    GetFixAction(code, dx, options, tmpActions, cancellationToken);
                    relatedActions.AddRange(tmpActions.Where(a => a.Name == primaryAction.Name));
                }

                // if there are multiple related actions then add a fix all action too.
                // (note: if there is only one action this is just the primary action.
                if (relatedActions.Count > 1)
                {
                    var fixAllAction = 
                        new CodeAction(
                            primaryAction.Name + " - all", 
                            "Apply to all occurences in query or selection.")
                        .WithRelatedActions(relatedActions);

                    actions.Add(fixAllAction);
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
            CodeAction action,
            int caretPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            if (action.RelatedActions.Count > 0)
            {
                // assume this is an fix all action
                return ApplyFixActions(code, action.RelatedActions, caretPosition, options, cancellationToken);
            }
            else
            {
                return ApplyFixActions(code, new[] { action }, caretPosition, options, cancellationToken);
            }
        }

        protected virtual CodeActionResult ApplyFixActions(
            KustoCode code,
            IReadOnlyList<CodeAction> actions,
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
            var min = edits.Min(a => a.Start);
            var max = edits.Max(a => a.Start + a.DeleteLength);

            return position >= min && position < max;
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
            CodeAction action,
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