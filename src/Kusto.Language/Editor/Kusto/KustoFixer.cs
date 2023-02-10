using System;
using System.Linq;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// A <see cref="KustoFixer"/> fixes problems identified by diagnostics.
    /// </summary>
    internal abstract class KustoFixer
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
                            new[]
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

            var edits = new List<TextEdit>();
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

            var startingText = new EditString(code.Text);

            if (edits.Count > 0)
            {
                if (!startingText.CanApplyAll(edits))
                {
                    return CodeActionResult.Failure("Invalid fix changes");
                }

                var newText = startingText.ApplyAll(edits);
                var newCaretPosition = newText.GetCurrentPosition(suggestedCaretPosition, suggestedCaretBias);
                return CodeActionResult.ChangeAndMove(newText, newCaretPosition);
            }

            return CodeActionResult.Nothing;
        }

        private static bool EditContainsPosition(IReadOnlyList<TextEdit> edits, int position)
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

        /// <summary>
        /// Returns the edits to apply to the query text and the new caret position
        /// for the specified action.
        /// </summary>
        protected virtual FixEdits GetFixEdits(
            KustoCode code,
            ApplyAction action,
            int caretPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            return new FixEdits(caretPosition, null);
        }
    }

    /// <summary>
    /// A set of text edits meant to be applied to a query's source text and the new position 
    /// of the caret after the edits have been applied.
    /// </summary>
    internal class FixEdits
    {
        /// <summary>
        /// One or more edits to be applied to the text of the query.
        /// </summary>
        public IReadOnlyList<TextEdit> Edits { get; }

        /// <summary>
        /// The new position of the caret after the edits are applied.
        /// </summary>
        public int NewCaretPosition { get; }

        /// <summary>
        /// The <see cref="PositionBias"/> to use when adjusting the caret position 
        /// when applying multiple fix results.
        /// </summary>
        public PositionBias Bias { get; }

        /// <summary>
        /// Construct a new <see cref="FixEdits"/>
        /// </summary>
        /// <param name="newCaretPosition">The new position of the cursor in pre-edit units.</param>
        /// <param name="bias">The bias to use when computing the new cursor position in post-edit units.</param>
        /// <param name="edits">A list of non-overlapping edits in pre-edit units.</param>
        public FixEdits(int newCaretPosition, PositionBias bias, IReadOnlyList<TextEdit> edits)
        {
            this.NewCaretPosition = newCaretPosition;
            this.Bias = bias;
            this.Edits = edits ?? EmptyReadOnlyList<TextEdit>.Instance;
        }

        /// <summary>
        /// Construct a new <see cref="FixEdits"/>
        /// </summary>
        /// <param name="newCaretPosition">The new position of the cursor in pre-edit units.</param>
        /// <param name="edits">A list of non-overlapping edits in pre-edit units.</param>
        public FixEdits(int newCaretPosition, IReadOnlyList<TextEdit> edits)
            : this(newCaretPosition, PositionBias.Left, edits)
        {
        }

        /// <summary>
        /// Construct a new <see cref="FixEdits"/>
        /// </summary>
        /// <param name="newCaretPosition">The new position of the cursor in pre-edit units.</param>
        /// <param name="bias">The bias to use when computing the new cursor position in post-edit units.</param>
        /// <param name="edits">A list of non-overlapping edits in pre-edit units.</param>
        public FixEdits(int newCaretPosition, PositionBias bias, params TextEdit[] edits)
            : this(newCaretPosition, bias, (IReadOnlyList<TextEdit>)edits)
        {
        }

        /// <summary>
        /// Construct a new <see cref="FixEdits"/>
        /// </summary>
        /// <param name="newCaretPosition">The new position of the cursor in pre-edit units.</param>
        /// <param name="edits">A list of non-overlapping edits in pre-edit units.</param>
        public FixEdits(int newCaretPosition, params TextEdit[] edits)
            : this(newCaretPosition, PositionBias.Left, (IReadOnlyList<TextEdit>)edits)
        {
        }
    }
}
