using Kusto.Language.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Kusto.Language.Editor
{
    public class CodeAction
    {
        public string Actor { get; }
        public string Name { get; }
        public string Description { get; }
        public string Data { get; }

        internal CodeAction(string actor, string name, string description, string data)
        {
            this.Actor = actor;
            this.Name = name;
            this.Description = description;
            this.Data = data;
        }
    }

    public class CodeActionInfo
    {
        public IReadOnlyList<CodeAction> Actions { get; }

        internal CodeActionInfo(IReadOnlyList<CodeAction> actions)
        {
            this.Actions = actions;
        }

        public static readonly CodeActionInfo NoActions =
            new CodeActionInfo(EmptyReadOnlyList<CodeAction>.Instance);
    }

    public class CodeActionOptions
    {
        public IReadOnlyList<CodeActor> Actors { get; }
        public IReadOnlyList<Diagnostic> AdditionalDiagnostics { get; }
        public FormattingOptions FormattingOptions { get; }

        private CodeActionOptions(
            IReadOnlyList<CodeActor> actors,
            IReadOnlyList<Diagnostic> additionalDiagnostics,
            FormattingOptions formattingOptions)
        {
            this.Actors = actors ?? EmptyReadOnlyList<CodeActor>.Instance;
            this.AdditionalDiagnostics = additionalDiagnostics ?? EmptyReadOnlyList<Diagnostic>.Instance;
            this.FormattingOptions = formattingOptions ?? FormattingOptions.Default;
        }

        public CodeActionOptions WithActors(IReadOnlyList<CodeActor> actors)
        {
            return new CodeActionOptions(actors, this.AdditionalDiagnostics, this.FormattingOptions);
        }

        public CodeActionOptions WithAdditionalDiagnostics(IReadOnlyList<Diagnostic> diagnostics)
        {
            return new CodeActionOptions(this.Actors, diagnostics, this.FormattingOptions);
        }

        public CodeActionOptions WithFormattingOptions(FormattingOptions options)
        {
            return new CodeActionOptions(this.Actors, this.AdditionalDiagnostics, options);
        }

        public static readonly CodeActionOptions Default = new CodeActionOptions(null, null, null);
    }

    /// <summary>
    /// An action taken by the caller to apply the <see cref="CodeActionResult"/>
    /// </summary>
    public abstract class ResultAction
    {
    }

    /// <summary>
    /// A <see cref="ResultAction"/> that changes the text of the associated query.
    /// </summary>
    public sealed class ChangeTextAction : ResultAction
    {
        public EditString Changes { get; }

        public string NewText => this.Changes;

        public ChangeTextAction(EditString changes)
        {
            this.Changes = changes;
        }
    }

    /// <summary>
    /// A <see cref="ResultAction"/> that moves the current caret position
    /// relative to the start of the <see cref="CodeBlock"/>
    /// </summary>
    public sealed class MoveCaretAction : ResultAction
    {
        public int NewCaretPosition { get; }

        public MoveCaretAction(int newCaretPosition)
        {
            this.NewCaretPosition = newCaretPosition;
        }
    }

    /// <summary>
    /// A <see cref="ResultAction"/> that initiates a rename of the named element at the current caret position.
    /// </summary>
    public sealed class RenameAction : ResultAction
    {
        public RenameAction()
        {
        }

        public static readonly RenameAction Instance = new RenameAction();
    }

    /// <summary>
    /// The resulting effects of applying a <see cref="CodeAction"/>.
    /// These result actions must be 
    /// </summary>
    public sealed class CodeActionResult
    {
        /// <summary>
        /// The list of <see cref="ResultAction"/> that to be applied in order.
        /// </summary>
        public IReadOnlyList<ResultAction> Actions { get; }

        /// <summary>
        /// The reason the application of the <see cref="CodeAction"/> failed (or null).
        /// </summary>
        public string FailureReason { get; }

        private CodeActionResult(IReadOnlyList<ResultAction> actions, string failureReason)
        {
            this.Actions = actions.ToReadOnly();
            this.FailureReason = failureReason;
        }

        public CodeActionResult(IReadOnlyList<ResultAction> actions)
            : this(actions, null)
        {
        }

        public CodeActionResult(params ResultAction[] actions)
            : this((IReadOnlyList<ResultAction>)actions)
        {
        }

        public CodeActionResult(EditString newText)
            : this(new ResultAction[] { new ChangeTextAction(newText) })
        {
        }

        public CodeActionResult(EditString newText, int newPosition)
            : this(new ResultAction[] { new ChangeTextAction(newText), new MoveCaretAction(newPosition) })
        {
        }

        public static CodeActionResult Failure(string failureReason)
        {
            return new CodeActionResult(null, failureReason);
        }

        public CodeActionResult WithAction(ResultAction action)
        {
            return new CodeActionResult(this.Actions.Concat(new[] { action }).ToList(), null);
        }

        public CodeActionResult WithAdjustedPosition(int delta)
        {
            if (this.Actions.Count > 0 && delta != 0)
            {
                var adjustedActions = new List<ResultAction>();

                foreach (var act in this.Actions)
                {
                    switch (act)
                    {
                        case MoveCaretAction mca:
                            adjustedActions.Add(new MoveCaretAction(mca.NewCaretPosition + delta));
                            break;
                        default:
                            adjustedActions.Add(act);
                            break;
                    }
                }

                return new CodeActionResult(adjustedActions);
            }

            return this;
        }

        /// <summary>
        /// A <see cref="CodeActionResult"/> where nothing happens.
        /// </summary>
        public static readonly CodeActionResult Nothing = new CodeActionResult(EmptyReadOnlyList<ResultAction>.Instance);
    }
}