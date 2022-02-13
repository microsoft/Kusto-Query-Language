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
        public string Verb { get; }
        public string Description { get; }
        public string Data { get; }

        internal CodeAction(string actor, string verb, string description, string data)
        {
            this.Actor = actor;
            this.Verb = verb;
            this.Description = description;
            this.Data = data;
        }
    }

    public static class CodeActionVerbs
    {
        public static string Refactor = nameof(Refactor);
        public static string Fix = nameof(Fix);
    }

    public class CodeActionInfo
    {
        public IReadOnlyList<CodeAction> Actions { get; }

        internal CodeActionInfo(IReadOnlyList<CodeAction> actions)
        {
            this.Actions = actions;
        }
    }

    public class CodeActionResult
    {
        public string NewText { get; }
        public int NewPosition { get; }
        public string FailureReason { get; }

        internal CodeActionResult(string newText, int newPosition, string failureReason = null)
        {
            this.NewText = newText;
            this.NewPosition = newPosition;
            this.FailureReason = FailureReason;
        }
    }
}