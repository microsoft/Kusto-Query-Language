using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    using Utils;

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
        /// The text to show in the completion list.
        /// </summary>
        public string DisplayText { get; }

        /// <summary>
        /// The text to match on when typing.
        /// </summary>
        public string MatchText { get; }

        /// <summary>
        /// The text segments that are applied on completion of this item.
        /// </summary>
        public IReadOnlyList<CompletionText> ApplyTexts { get; }

        /// <summary>
        /// The text to apply before the cursor/caret.
        /// </summary>
        public string BeforeText => 
            ApplyTexts.Count > 0 
                ? ApplyTexts[0].Text 
                : "";

        /// <summary>
        /// The text apply after the cursor/caret.
        /// </summary>
        public string AfterText
        {
            get
            {
                if (_afterText == null && this.ApplyTexts.Count > 1)
                {
                    _afterText = string.Concat(this.ApplyTexts.Skip(1).Select(t => t.Text));
                }

                return _afterText;
            }
        }

        private string _afterText;

        [Obsolete("Use BeforeText or ApplyTexts")]
        public string EditText => BeforeText;

        /// <summary>
        /// True if any segment of the <see cref="ApplyTexts"/> is editable after completion.
        /// </summary>
        public bool IsEditable => 
            this.ApplyTexts.Any(t => t is CompletionEditableText);

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
            CompletionRank rank,
            CompletionPriority priority)
        {
            this.Kind = kind;
            this.DisplayText = displayText ?? "";
            this.MatchText = matchText ?? displayText;
            this.ApplyTexts = applyTexts.ToReadOnly();
            this.Rank = rank;
            this.Priority = priority;
        }

        /// <summary>
        /// Creates a <see cref="CompletionItem"/> instance.
        /// </summary>
        /// <param name="kind">The kind of completion item.</param>
        /// <param name="displayText">The text to display in the completion list for the item.</param>
        /// <param name="beforeText">The text to apply before the cursor/caret. If not specified the displayText is used.</param>
        /// <param name="afterText">The text to apply after the cursor/caret.</param>
        /// <param name="matchText">The text to match against user typing. If not specified the displayText is used.</param>
        /// <param name="rank">The rank of the completion item determines the category for ordering in the completion list.</param>
        /// <param name="priority">The priority of the completion item determines the ordering within the items rank.</param>
        /// <returns></returns>
        public CompletionItem(
            CompletionKind kind,
            string displayText,
            string beforeText = null,
            string afterText = null,
            string matchText = null,
            CompletionRank rank = CompletionRank.Default,
            CompletionPriority priority = CompletionPriority.Normal)
            : this(kind, displayText, CreateApplyTexts(beforeText ?? displayText, afterText), matchText, rank, priority)
        {
        }

        private static IReadOnlyList<CompletionText> CreateApplyTexts(string beforeText, string afterText)
        {
            var list = new List<CompletionText>(2);
            list.Add(CompletionText.Fixed(beforeText ?? ""));
            if (afterText != null)
                list.Add(CompletionText.Fixed(afterText));
            return list.ToReadOnly();
        }

        public CompletionItem WithKind(CompletionKind kind)
        {
            return new CompletionItem(kind, this.DisplayText, this.ApplyTexts, this.MatchText, this.Rank, this.Priority);
        }

        public CompletionItem WithDisplayText(string displayText)
        {
            return new CompletionItem(this.Kind, displayText, this.ApplyTexts, this.MatchText, this.Rank, this.Priority);
        }

        public CompletionItem WithMatchText(string matchText)
        {
            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                this.ApplyTexts,
                matchText,
                this.Rank,
                this.Priority
                );
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="ApplyTexts"/> specified.
        /// </summary>
        public CompletionItem WithApplyTexts(IReadOnlyList<CompletionText> applyTexts)
        {
            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                applyTexts,
                this.MatchText,
                this.Rank,
                this.Priority
                );
        }

        /// <summary>
        /// Creates a new <see cref="CompletionItem"/> with the <see cref="ApplyTexts"/> specified.
        /// </summary>
        public CompletionItem WithApplyTexts(params CompletionText[] applyTexts)
        {
            return WithApplyTexts((IReadOnlyList<CompletionText>)applyTexts);
        }

        public CompletionItem WithBeforeText(string beforeText)
        {
            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                this.ApplyTexts.Replace(0, CompletionText.Fixed(beforeText)),
                this.MatchText,
                this.Rank,
                this.Priority);
        }

        [Obsolete("Use WithBeforeText")]
        public CompletionItem WithEditText(string editText)
        {
            return WithBeforeText(editText);
        }

        public CompletionItem WithAfterText(string afterText)
        {
            var newParts = this.ApplyTexts;
            if (afterText == null)
            {
                newParts = newParts.Count == 2 ? newParts.RemoveAt(1) 
                    : newParts;
            }
            else
            {
                var newAfterText = CompletionText.Fixed(afterText);
                newParts = newParts.Count == 2 ? newParts.Replace(1, newAfterText)
                        : newParts.Count == 1 ? newParts.Insert(1, newAfterText)
                        : newParts;
            }

            if (newParts != this.ApplyTexts)
            {
                return new CompletionItem(
                    this.Kind,
                    this.DisplayText,
                    newParts,
                    this.MatchText,
                    this.Rank,
                    this.Priority
                    );
            }

            return this;
        }

        public CompletionItem WithRank(CompletionRank rank)
        {
            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                this.ApplyTexts,
                this.MatchText,
                rank,
                this.Priority);
        }

        public CompletionItem WithPriority(CompletionPriority priority)
        {
            return new CompletionItem(
                this.Kind,
                this.DisplayText,
                this.ApplyTexts,
                this.MatchText,
                this.Rank,
                priority
                );
        }

        private const string DefaultCursorMarker = "|";
        private const string DefaultStartMarker = "[[";
        private const string DefaultEndMarker = "]]";

        /// <summary>
        /// Parses the text into a list of <see cref="CompletionText"/>.
        /// </summary>
        public static IReadOnlyList<CompletionText> ParseApplyTexts(
            string textWithMarkers, 
            string cursorMarker = DefaultCursorMarker, 
            string startMarker = DefaultStartMarker, 
            string endMarker = DefaultEndMarker)
        {
            var list = new List<CompletionText>();

            var cursorPos = textWithMarkers.IndexOf(cursorMarker);
            if (cursorPos >= 0)
            {
                var beforeCursorText = cursorPos > 0 ? textWithMarkers.Substring(0, cursorPos) : "";
                var afterCursorText = textWithMarkers.Substring(cursorPos + cursorMarker.Length);
                list.Add(CompletionText.Fixed(beforeCursorText));
                list.Add(CompletionText.Fixed(afterCursorText));
            }
            else
            {
                var pos = 0;
                while (pos < textWithMarkers.Length)
                {
                    var startPos = textWithMarkers.IndexOf(startMarker, pos);
                    if (startPos >= 0)
                    {
                        var endPos = textWithMarkers.IndexOf(endMarker, startPos + startMarker.Length);
                        if (endPos >= 0)
                        {
                            if (startPos > pos)
                            {
                                // add fixed segment between edit segments
                                list.Add(CompletionText.Fixed(textWithMarkers.Substring(pos, startPos - pos)));
                            }

                            list.Add(CompletionText.Editable(textWithMarkers.Substring(startPos + startMarker.Length, endPos - (startPos + startMarker.Length))));
                            pos = endPos + endMarker.Length;
                            continue;
                        }
                    }

                    list.Add(CompletionText.Fixed(textWithMarkers.Substring(pos)));
                    pos = textWithMarkers.Length;
                }
            }

            return list.ToReadOnly();
        }
    }

    public abstract class CompletionText
    {
        public string Text { get; }

        protected CompletionText(string text) 
        {
            this.Text = text;
        }

        /// <summary>
        /// Creates a new <see cref="CompletionText"/> that is fixed text.
        /// </summary>
        public static CompletionFixedText Fixed(string text) =>
            new CompletionFixedText(text);

        /// <summary>
        /// Creates a new <see cref="CompletionText"/> that is editable after applied.
        /// </summary>
        public static CompletionEditableText Editable(string text) =>
            new CompletionEditableText(text);
    }

    public sealed class CompletionFixedText : CompletionText
    {
        internal CompletionFixedText(string text)
            : base (text)
        {
        }
    }

    public sealed class CompletionEditableText : CompletionText
    {
        internal CompletionEditableText(string text)
            : base(text)
        {
        }
    }
}
