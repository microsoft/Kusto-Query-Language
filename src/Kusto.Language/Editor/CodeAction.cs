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