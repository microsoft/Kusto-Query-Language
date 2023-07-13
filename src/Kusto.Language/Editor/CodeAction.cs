using Kusto.Language.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Kusto.Language.Editor
{
    public abstract class CodeAction
    {
        /// <summary>
        /// The kind of <see cref="CodeAction"/> used to common kinds of actions
        /// so they may be grouped together into a fix-all action.
        /// </summary>
        public string Kind { get; }

        /// <summary>
        /// The the text of the action displayed in the menu
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The longer description displayed in the hover tip.
        /// </summary>
        public string Description { get; }

        protected CodeAction(string kind, string title, string description)
        {
            this.Kind = kind ?? "";
            this.Title = title ?? "";
            this.Description = description ?? "";
        }

        protected abstract CodeAction New(string kind, string title, string description);

        public CodeAction WithTitle(string title)
        {
            return New(this.Kind, title, this.Description);
        }

        public CodeAction WithDescription(string description)
        {
            return New(this.Kind, this.Title, description);
        }

        public static ApplyAction Create(string kind, string title, string description, IReadOnlyList<string> data = null)
        {
            return new SingleAction(kind, title, description, data);
        }

        public static ApplyAction Create(string title, string description, IReadOnlyList<string> data = null)
        {
            return new SingleAction(title, title, description, data);
        }

        public static ApplyAction CreateFixAll(string title, string description, IReadOnlyList<ApplyAction> actions)
        {
            return new MultiAction(FixAllKind, title, description, actions);
        }

        public static MenuAction CreateMenu(string title, string description, IReadOnlyList<CodeAction> actions)
        {
            return new MenuAction(title, description, actions);
        }

        public const string FixAllKind = "FixAll";
        public const string MenuKind = "Menu";
    }

    /// <summary>
    /// An <see cref="CodeAction"/> that can be applied.
    /// </summary>
    public abstract class ApplyAction : CodeAction
    {
        /// <summary>
        /// Additional data used by <see cref="CodeService"/> when action is applied.
        /// </summary>
        public IReadOnlyList<string> Data { get; }

        protected ApplyAction(string kind, string title, string description, IReadOnlyList<string> data)
            : base(kind, title, description)
        {
            this.Data = data ?? EmptyReadOnlyList<string>.Instance;
        }

        protected override CodeAction New(string kind, string title, string description)
        {
            return New(kind, title, description, this.Data);
        }

        protected abstract ApplyAction New(string kind, string title, string description, IReadOnlyList<string> data);

        /// <summary>
        /// Returns a new <see cref="CodeAction"/> with the data values changed.
        /// </summary>
        public ApplyAction WithData(IReadOnlyList<string> data)
        {
            if (this.Data != data)
            {
                return New(this.Kind, this.Title, this.Description, data);
            }

            return this;
        }

        /// <summary>
        /// Returns a new <see cref="CodeAction"/> with the data values changed.
        /// </summary>
        public ApplyAction WithData(params string[] data) =>
            WithData((IReadOnlyList<string>)data);

        /// <summary>
        /// Returns a new <see cref="CodeAction"/> with the data values changed.
        /// </summary>
        public ApplyAction WithData(IEnumerable<string> data) =>
            WithData(data.ToReadOnly());

        /// <summary>
        /// Returns a new <see cref="CodeAction"/> with the additional data appended to the list of data values.
        /// </summary>
        public ApplyAction AddData(IEnumerable<string> additionalData) =>
            WithData(this.Data.Concat(additionalData));

        /// <summary>
        /// Returns a new <see cref="CodeAction"/> with the additional data appended to the list of data values.
        /// </summary>
        public ApplyAction AddData(params string[] additionalData) =>
            AddData((IEnumerable<string>)additionalData);

        /// <summary>
        /// Returns a new <see cref="CodeAction"/> with a number of data values removed from the end of the data value list.
        /// </summary>
        public ApplyAction RemoveData(int count) =>
            WithData(this.Data.Take(this.Data.Count - count));
    }

    /// <summary>
    /// An <see cref="ApplyAction"/> that is just a single action.
    /// </summary>
    public class SingleAction : ApplyAction
    {
        public SingleAction(string kind, string title, string description, IReadOnlyList<string> data = null)
            : base(kind, title, description, data)
        {
        }

        protected override ApplyAction New(string kind, string title, string description, IReadOnlyList<string> data)
        {
            return new SingleAction(kind, title, description, data);
        }
    }

    /// <summary>
    /// An <see cref="ApplyAction"/> action that is composed of multiple actions.
    /// </summary>
    public class MultiAction : ApplyAction
    {
        public IReadOnlyList<ApplyAction> Actions { get; }

        private MultiAction(string kind, string title, string description, IReadOnlyList<string> data, IReadOnlyList<ApplyAction> actions)
            : base(kind, title, description, data)
        {
            this.Actions = actions ?? EmptyReadOnlyList<ApplyAction>.Instance;
        }

        public MultiAction(string kind, string title, string description, IReadOnlyList<ApplyAction> actions)
            : this(kind, title, description, null, actions)
        {
        }

        protected override ApplyAction New(string kind, string title, string description, IReadOnlyList<string> data)
        {
            return new MultiAction(kind, title, description, data, this.Actions);
        }
    }

    /// <summary>
    /// A <see cref="CodeAction"/> that offers a menu of alternative actions.
    /// </summary>
    public class MenuAction : CodeAction
    {
        public IReadOnlyList<CodeAction> Actions { get; }

        private MenuAction(string kind, string title, string description, IReadOnlyList<CodeAction> actions)
            : base(kind, title, description)
        {
            this.Actions = actions ?? EmptyReadOnlyList<CodeAction>.Instance;
        }

        public MenuAction(string title, string description, IReadOnlyList<CodeAction> actions)
            : this(MenuKind, title, description, actions)
        {
        }

        protected override CodeAction New(string kind, string title, string description)
        {
            return new MenuAction(kind, title, description, this.Actions);
        }

        public MenuAction WithActions(IReadOnlyList<CodeAction> actions)
        {
            return new MenuAction(this.Kind, this.Title, this.Description, actions);
        }

        public MenuAction WithActions(params CodeAction[] actions)
        {
            return WithActions((IReadOnlyList<CodeAction>) actions); 
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
        public DisabledDiagnostics DiagnosticFilter { get; }
        public FormattingOptions FormattingOptions { get; }

        private CodeActionOptions(
            DisabledDiagnostics diagnosticFilter,
            FormattingOptions formattingOptions)
        {
            this.DiagnosticFilter = diagnosticFilter ?? DisabledDiagnostics.Default;
            this.FormattingOptions = formattingOptions ?? FormattingOptions.Default;
        }

        public CodeActionOptions WithFormattingOptions(FormattingOptions options)
        {
            return new CodeActionOptions(this.DiagnosticFilter, options);
        }

        public CodeActionOptions WithDiagnosticFilter(DisabledDiagnostics filter)
        {
            return new CodeActionOptions(filter, this.FormattingOptions);
        }

        public static readonly CodeActionOptions Default = new CodeActionOptions(null, null);
    }

    /// <summary>
    /// An action taken by the client to apply the result of a code action.
    /// </summary>
    public abstract class ResultAction
    {
    }

    /// <summary>
    /// An action taken by the client to change the text of the <see cref="CodeBlock"/>.
    /// </summary>
    public sealed class ChangeTextAction : ResultAction
    {
        /// <summary>
        /// A set of in order, non-overlapping edits, each relative to the original text.
        /// </summary>
        public IReadOnlyList<TextEdit> Changes { get; }

        /// <summary>
        /// The new text after all the changes have been applied to the original text.
        /// </summary>
        public string ChangedText { get; }

        /// <summary>
        /// Constructs an action taken by the client to change the text of the <see cref="CodeBlock"/>.
        /// </summary>
        public ChangeTextAction(IReadOnlyList<TextEdit> changes, string changedText)
        {
            this.Changes = changes;
            this.ChangedText = changedText;
        }

        /// <summary>
        /// Constructs an action taken by the client to change the text of the <see cref="CodeBlock"/>.
        /// </summary>
        public ChangeTextAction(EditString editString)
            : this(editString.GetChanges(), editString.CurrentText)
        {
        }
    }

    /// <summary>
    /// An action taken by the client to move the caret to a position relative to the start of the <see cref="CodeBlock"/>
    /// </summary>
    public sealed class MoveCaretAction : ResultAction
    {
        public int NewCaretPosition { get; }

        /// <summary>
        /// Constructs an action taken by the client to move the caret to a position relative to the start of the <see cref="CodeBlock"/>
        /// </summary>
        public MoveCaretAction(int newCaretPosition)
        {
            this.NewCaretPosition = newCaretPosition;
        }
    }

    /// <summary>
    /// An action taken by the client to intiate a rename of the named element at the current caret position.
    /// </summary>
    public sealed class RenameAction : ResultAction
    {
        /// <summary>
        /// Constructs an action taken by the client to intiate a rename of the named element at the current caret position.
        /// </summary>
        public RenameAction()
        {
        }

        /// <summary>
        /// An action taken by the client to intiate a rename of the named element at the current caret position.
        /// </summary>
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

        public static CodeActionResult Change(IReadOnlyList<TextEdit> changes, string changedText)
        {
            return new CodeActionResult(new ChangeTextAction(changes, changedText));
        }

        public static CodeActionResult Change(EditString changedText)
        {
            return Change(changedText.GetChanges(), changedText.CurrentText);
        }

        public static CodeActionResult ChangeAndMove(IReadOnlyList<TextEdit> changes, string changedText, int newCaretPosition)
        {
            return new CodeActionResult(
                new ChangeTextAction(changes, changedText),
                new MoveCaretAction(newCaretPosition));
        }

        public static CodeActionResult ChangeAndMove(EditString changedText, int newCaretPosition)
        {
            return ChangeAndMove(changedText.GetChanges(), changedText.CurrentText, newCaretPosition);
        }

        public static CodeActionResult Failure(string failureReason = null)
        {
            if (failureReason == null)
                return Failed;
            return new CodeActionResult(null, failureReason);
        }

        private static readonly CodeActionResult Failed = 
            CodeActionResult.Failure("Action failed");


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
        /// A <see cref="CodeActionResult"/> with no actions.
        /// </summary>
        public static readonly CodeActionResult Nothing = new CodeActionResult(EmptyReadOnlyList<ResultAction>.Instance);
    }
}