using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// Represents an individual option in an intellisense completion list.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{DisplayText}")]
    public class CompletionItem
    {
        /// <summary>
        /// The kind of <see cref="CompletionItem"/>.
        /// </summary>
        public CompletionKind Kind { get; }

        /// <summary>
        /// The text to show in the completion list.
        /// </summary>
        public string DisplayText { get; }

        /// <summary>
        /// The text to match on when typing.
        /// </summary>
        public string MatchText { get; }

        /// <summary>
        /// The text to order the item by.
        /// </summary>
        public string OrderText { get; }

        /// <summary>
        /// The text segments that are applied on completion of this item.
        /// </summary>
        public IReadOnlyList<CompletionText> ApplyTexts { get; }

        private string _beforeText;
        private string _afterText;

        /// <summary>
        /// The text to apply before the cursor/caret.
        /// </summary>
        public string BeforeText
        {
            get
            {
                if (_beforeText == null && this.ApplyTexts.Count > 0)
                {
                    var caretIndex = this.CaretIndex;
                    if (caretIndex > 0)
                    {
                        _beforeText = string.Concat(this.ApplyTexts.Take(caretIndex).Select(t => t.Text));
                    }
                    else if (caretIndex < 0)
                    {
                        _beforeText = string.Concat(this.ApplyTexts.Select(t => t.Text));
                    }
                    else
                    {
                        _beforeText = "";
                    }
                }

                return _beforeText ?? "";
            }
        }

        /// <summary>
        /// The text apply after the cursor/caret.
        /// </summary>
        public string AfterText
        {
            get
            {
                if (_afterText == null && this.ApplyTexts.Count > 0)
                {
                    var caretIndex = this.CaretIndex;
                    if (caretIndex >= 0)
                    {
                        _afterText = string.Concat(this.ApplyTexts.Skip(caretIndex).Select(t => t.Text));
                    }
                    else
                    {
                        _afterText = "";
                    }
                }

                return _afterText ?? "";
            }
        }

        [Obsolete("Use BeforeText or ApplyTexts")]
        public string EditText => BeforeText;

        /// <summary>
        /// True if completion should be retriggered automatically after this item is inserted and the caret positioned.
        /// </summary>
        public bool Retrigger { get; }

        /// <summary>
        /// The ranking that controls the ordering of categories of completion items.
        /// </summary>
        internal CompletionRank Rank { get; }

        /// <summary>
        /// The priority of the completion item within its rank.
        /// </summary>
        internal CompletionPriority Priority { get; }

        private CompletionItem(
            CompletionKind kind,
            string displayText,
            IReadOnlyList<CompletionText> applyTexts,
            string matchText,
            string orderText,
            CompletionRank rank,
            CompletionPriority priority,
            bool retrigger)
        {
            this.Kind = kind;
            this.DisplayText = displayText ?? "";
            this.MatchText = matchText ?? displayText;
            this.OrderText = orderText ?? this.MatchText;
            this.ApplyTexts = applyTexts.ToReadOnly();
            this.Rank = rank;
            this.Priority = priority;
            this.Retrigger = retrigger;
        }

        /// <summary>
        /// Creates a <see cref="CompletionItem"/> instance.
        /// </summary>
        /// <param name="kind">The kind of completion item.</param>
        /// <param name="displayText">The text to display in the completion list for the item.</param>
        /// <param name="beforeText">The text to apply before the cursor/caret. If not specified the displayText is used.</param>
        /// <param name="afterText">The text to apply after the cursor/caret.</param>
        /// <param name="matchText">The text to match against user typing. If not specified the displayText is used.</param>
        /// <param name="orderText">The text to order the item by.</param>
        /// <param name="rank">The rank of the completion item determines the category for ordering in the completion list.</param>
        /// <param name="priority">The priority of the completion item determines the ordering within the items rank.</param>
        /// <param name="retrigger">If true, the editor will retrigger completion after this item is inserted.</param>
        public CompletionItem(
            CompletionKind kind,
            string displayText,
            string beforeText = null,
            string afterText = null,
            string matchText = null,
            string orderText = null,
            CompletionRank rank = CompletionRank.Default,
            CompletionPriority priority = CompletionPriority.Normal,
            bool retrigger = false)
            : this(
                  kind, 
                  displayText, 
                  CreateApplyTexts(beforeText ?? displayText, afterText), 
                  matchText, 
                  orderText,
                  rank, 
                  priority, 
                  retrigger)
        {
        }

        /// <summary>
        /// Creates a <see cref="CompletionItem"/> instance.
        /// </summary>
        /// <param name="displayText">The text to display in the completion list for the item.</param>
        /// <param name="beforeText">The text to apply before the cursor/caret. If not specified the displayText is used.</param>
        /// <param name="afterText">The text to apply after the cursor/caret.</param>
        /// <param name="matchText">The text to match against user typing. If not specified the displayText is used.</param>
        /// <param name="orderText">The text to order the item by.</param>
        /// <param name="rank">The rank of the completion item determines the category for ordering in the completion list.</param>
        /// <param name="priority">The priority of the completion item determines the ordering within the items rank.</param>
        /// <param name="retrigger">If true, the editor will retrigger completion after this item is inserted.</param>
        public CompletionItem(
            string displayText,
            string beforeText = null,
            string afterText = null,
            string matchText = null,
            string orderText = null,
            CompletionRank rank = CompletionRank.Default,
            CompletionPriority priority = CompletionPriority.Normal,
            bool retrigger = false)
            : this(
                  CompletionKind.Syntax, 
                  displayText, 
                  CreateApplyTexts(beforeText ?? displayText, afterText), 
                  matchText, 
                  orderText,
                  rank, 
                  priority, 
                  retrigger)
        {
        }

        /// <summary>
        /// True if this <see cref="CompletionItem"/> adjusts the position of the caret.
        /// </summary>
        internal bool HasCaret =>
            this.ApplyTexts.Any(at => at.Caret);

        private static IReadOnlyList<CompletionText> CreateApplyTexts(string beforeText, string afterText)
        {
            var list = new List<CompletionText>(2);
            list.Add(CompletionText.Create(beforeText ?? ""));
            if (afterText != null)
                list.Add(CompletionText.Create(afterText, caret: true));
            return list.ToReadOnly();
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="Kind"/> property modified.
        /// This is typically used to select an icon/glyph for the completion item UI.
        /// </summary>
        public CompletionItem WithKind(CompletionKind kind)
        {
            return new CompletionItem(
                kind, 
                this.DisplayText, 
                this.ApplyTexts, 
                this.MatchText, 
                this.OrderText,
                this.Rank, 
                this.Priority, 
                this.Retrigger);
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="DisplayText"/> property modified.
        /// </summary>
        public CompletionItem WithDisplayText(string displayText)
        {
            return new CompletionItem(
                this.Kind,
                displayText,
                this.ApplyTexts,
                this.MatchText,
                this.OrderText,
                this.Rank,
                this.Priority,
                this.Retrigger);
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="MatchText"/> property modified.
        /// </summary>
        public CompletionItem WithMatchText(string matchText)
        {
            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                this.ApplyTexts,
                matchText,
                this.OrderText,
                this.Rank,
                this.Priority,
                this.Retrigger
                );
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="OrderText"/> property modified.
        /// </summary>
        public CompletionItem WithOrderText(string orderText)
        {
            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                this.ApplyTexts,
                this.MatchText,
                orderText,
                this.Rank,
                this.Priority,
                this.Retrigger
                );
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="ApplyTexts"/> property modified.
        /// </summary>
        public CompletionItem WithApplyTexts(IReadOnlyList<CompletionText> applyTexts)
        {
            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                applyTexts,
                this.MatchText,
                this.OrderText,
                this.Rank,
                this.Priority,
                this.Retrigger
                );
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="ApplyTexts"/> property modified.
        /// </summary>
        public CompletionItem WithApplyTexts(params CompletionText[] applyTexts)
        {
            return WithApplyTexts((IReadOnlyList<CompletionText>)applyTexts);
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="ApplyTexts"/> property modified.
        /// </summary>
        public CompletionItem WithApplyTexts(string textWithMarkers)
        {
            return WithApplyTexts(CompletionText.Parse(textWithMarkers));
        }

        /// <summary>
        /// Returns the index of the <see cref="ApplyTexts"/> that contains the caret
        /// </summary>
        private int CaretIndex
        {
            get
            {
                for (int i = 0; i < this.ApplyTexts.Count; i++)
                {
                    if (this.ApplyTexts[i].Caret)
                        return i;
                }

                return -1;
            }
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="BeforeText"/> property modified.
        /// </summary>
        public CompletionItem WithBeforeText(string beforeText)
        {
            var newBeforeText = CompletionText.Create(beforeText);
            var caretIndex = this.CaretIndex;
            var newApplyTexts = caretIndex >= 0
                ? this.ApplyTexts.ReplaceRange(0, caretIndex, newBeforeText)
                : new[] { newBeforeText };

            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                newApplyTexts,
                this.MatchText,
                this.OrderText,
                this.Rank,
                this.Priority,
                this.Retrigger);
        }


        [Obsolete("Use WithBeforeText")]
        public CompletionItem WithEditText(string editText)
        {
            return WithBeforeText(editText);
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="AfterText"/> property modified.
        /// </summary>
        public CompletionItem WithAfterText(string afterText)
        {
            var newAfterText = CompletionText.Create(afterText, caret: true);
            var caretIndex = this.CaretIndex;
            var newApplyTexts =
                caretIndex > 0 ? this.ApplyTexts.ReplaceRange(caretIndex, this.ApplyTexts.Count - caretIndex, newAfterText)
                : caretIndex == 0 ? new[] { newAfterText }
                : this.ApplyTexts.Append(newAfterText);

            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                newApplyTexts,
                this.MatchText,
                this.OrderText,
                this.Rank,
                this.Priority,
                this.Retrigger
                );
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="Rank"/> property modified.
        /// </summary>
        internal CompletionItem WithRank(CompletionRank rank)
        {
            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                this.ApplyTexts,
                this.MatchText,
                this.OrderText,
                rank,
                this.Priority,
                this.Retrigger);
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="Priority"/> property modified.
        /// </summary>
        internal CompletionItem WithPriority(CompletionPriority priority)
        {
            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                this.ApplyTexts,
                this.MatchText,
                this.OrderText,
                this.Rank,
                priority,
                this.Retrigger
                );
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="Priority"/> property modified.
        /// </summary>
        public CompletionItem WithRetrigger(bool retrigger)
        {
            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                this.ApplyTexts,
                this.MatchText,
                this.OrderText,
                this.Rank,
                this.Priority,
                retrigger
                );
        }
    }
}
