using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Utils;

    /// <summary>
    /// An individual text segment for a <see cref="CompletionItem"/>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Text}")]
    public class CompletionText
    {
        /// <summary>
        /// The text to apply to insert.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// True if the caret should be moved to start of this text after being inserted.
        /// </summary>
        public bool Caret { get; }

        /// <summary>
        /// True if the text should be selected after being inserted. 
        /// </summary>
        public bool Select { get; }

        protected CompletionText(string text, bool caret, bool select) 
        {
            this.Text = text;
            this.Caret = caret;
            this.Select = select;
        }

        /// <summary>
        /// Creates a new <see cref="CompletionText"/>.
        /// </summary>
        public static CompletionText Create(string text, bool caret = false, bool select = false) =>
            new CompletionText(text, caret, select);

        /// <summary>
        /// Creates a new <see cref="CompletionText"/> that is intended to be editted.
        /// </summary>
        public static CompletionText CreateEdit(string text) =>
            new CompletionText(text, caret: true, select: true);


        private const string DefaultCursorMarker = "|";
        private const string DefaultStartMarker = "[[";
        private const string DefaultEndMarker = "]]";

        /// <summary>
        /// Parses the text into a list of <see cref="CompletionText"/>.
        /// </summary>
        public static IReadOnlyList<CompletionText> Parse(
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
                list.Add(CompletionText.Create(beforeCursorText));
                list.Add(CompletionText.Create(afterCursorText, caret: true));
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
                                list.Add(CompletionText.Create(textWithMarkers.Substring(pos, startPos - pos)));
                            }

                            list.Add(CompletionText.CreateEdit(textWithMarkers.Substring(startPos + startMarker.Length, endPos - (startPos + startMarker.Length))));
                            pos = endPos + endMarker.Length;
                            continue;
                        }
                    }

                    list.Add(CompletionText.Create(textWithMarkers.Substring(pos)));
                    pos = textWithMarkers.Length;
                }
            }

            return list.ToReadOnly();
        }
    }
}
