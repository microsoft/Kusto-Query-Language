using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{DisplayText}")]
    public class CompletionItem
    {
        /// <summary>
        /// The kind of <see cref="CompletionItem"/>.
        /// </summary>
        public CompletionKind Kind { get; }

        /// <summary>
        /// The text to display for the <see cref="CompletionItem"/>.
        /// </summary>
        public string DisplayText { get; }

        /// <summary>
        /// The text to match
        /// </summary>
        public string MatchText { get; }

        /// <summary>
        /// The text to insert/replace with.
        /// </summary>
        public string EditText { get; }

        /// <summary>
        /// The text insert/replace with after the cursor.
        /// </summary>
        public string AfterText { get; }

        /// <summary>
        /// The priority of the completion item.
        /// </summary>
        public CompletionPriority Priority { get; }

        public CompletionItem(CompletionKind kind, string displayText, string editText = null, string afterText = null, string matchText = null, CompletionPriority priority = CompletionPriority.Normal)
        {
            this.DisplayText = displayText ?? "";
            this.Kind = kind;
            this.EditText = editText ?? this.DisplayText;
            this.AfterText = afterText;
            this.MatchText = matchText ?? displayText;
            this.Priority = priority;
        }

        public CompletionItem WithKind(CompletionKind kind)
        {
            return new CompletionItem(kind, this.DisplayText, this.EditText, this.AfterText, this.MatchText, this.Priority);
        }

        public CompletionItem WithDisplayText(string displayText)
        {
            return new CompletionItem(this.Kind, displayText, this.EditText, this.AfterText, this.MatchText, this.Priority);
        }

        public CompletionItem WithEditText(string editText)
        {
            return new CompletionItem(this.Kind, this.DisplayText, editText, this.AfterText, this.MatchText, this.Priority);
        }

        public CompletionItem WithAfterText(string afterText)
        {
            return new CompletionItem(this.Kind, this.DisplayText, this.EditText, afterText, this.MatchText, this.Priority);
        }

        public CompletionItem WithMatchText(string matchText)
        {
            return new CompletionItem(this.Kind, this.DisplayText, this.EditText, this.AfterText, matchText, this.Priority);
        }

        public CompletionItem WithPriority(CompletionPriority priority)
        {
            return new CompletionItem(this.Kind, this.DisplayText, this.EditText, this.AfterText, this.MatchText, priority);
        }
    }
}
