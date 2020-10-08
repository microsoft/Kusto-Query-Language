using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Kusto.Language.Utils
{
    using Parsing;

    /// <summary>
    /// A string that remembers it edits.
    /// </summary>
    public class EditString
    {
        /// <summary>
        /// The text before the edits
        /// </summary>
        public string OriginalText { get; }

        /// <summary>
        /// The text after the edits
        /// </summary>
        public string CurrentText { get; }

        /// <summary>
        /// The map recording the positional changes between the original and current texts.
        /// </summary>
        private EditMap _map;

        private EditString(string originalText, string currentText, EditMap map)
        {
            this.OriginalText = originalText ?? "";
            this.CurrentText = currentText ?? "";
            _map = map;
        }

        /// <summary>
        /// Create a new <see cref="EditString"/> in a pre-edit state.
        /// </summary>
        public EditString(string text)
            : this(text, text, new EditMap())
        {
        }

        public int Length => this.CurrentText.Length;
        public char this[int index] => this.CurrentText[index];
        public int IndexOf(string value) => this.CurrentText.IndexOf(value);

        public static implicit operator string(EditString editString)
        {
            return editString != null ? editString.CurrentText : null;
        }

        public static implicit operator EditString(string text)
        {
            return text != null ? new EditString(text) : null;
        }

        public override string ToString() => this.CurrentText;

        /// <summary>
        /// Replace the range of characters with the specified text.
        /// </summary>
        public EditString ReplaceAt(int start, int length, string text)
        {
            var newText = this.CurrentText;
            
            if (length > 0)
            {
                newText = newText.Remove(start, length);
            }

            if (text.Length > 0)
            {
                newText = newText.Insert(start, text);
            }

            var newMap = _map.Replace(start, length, text.Length);

            return new EditString(this.OriginalText, newText, newMap);
        }
 
        /// <summary>
        /// Inserts the text at the specified position.
        /// </summary>
        public EditString Insert(int position, string text)
        {
            return ReplaceAt(position, 0, text);
        }

        /// <summary>
        /// Removes the specified range of characters.
        /// </summary>
        public EditString Remove(int start, int length)
        {
            return ReplaceAt(start, length, "");
        }

        /// <summary>
        /// Gets the substring at the specified range.
        /// </summary>
        public EditString Substring(int start, int length)
        {
            var newText = this.CurrentText.Substring(start, length);

            var newMap = _map;

            var endDeleteStart = start + length;
            var endDeleteLength = this.CurrentText.Length - endDeleteStart;

            if (endDeleteLength > 0)
            {
                newMap = newMap.Delete(endDeleteStart, endDeleteLength);
            }

            if (start > 0)
            {
                newMap = newMap.Delete(0, start);
            }

            return new EditString(this.OriginalText, newText, newMap);
        }

        /// <summary>
        /// Applies the edit.
        /// </summary>
        public EditString Apply(StringEdit edit)
        {
            return ReplaceAt(edit.Start, edit.DeleteLength, edit.InsertText);
        }

        /// <summary>
        /// Applies all the edits. 
        /// Each edit must refer to a position or range in the current text, be in order and non-overlapping.
        /// </summary>
        public EditString ApplyAll(IReadOnlyList<StringEdit> edits)
        {
            if (edits == null)
                return this;

            var builder = new StringBuilder();
            var mapEdits = new List<EditMap.Edit>();

            var pos = 0;

            foreach (var edit in edits)
            {
                if (edit.Start > pos)
                {
                    // append anything between this point and the edit start
                    builder.Append(this.CurrentText, pos, edit.Start - pos);
                }

                pos = edit.Start + edit.DeleteLength;

                if (edit.InsertText.Length > 0)
                {
                    builder.Append(edit.InsertText);
                }

                if (edit.InsertText.Length != edit.DeleteLength
                    || string.Compare(this.CurrentText, edit.Start, edit.InsertText, 0, edit.InsertText.Length) != 0)
                {
                    mapEdits.Add(new EditMap.Edit(edit.Start, edit.DeleteLength, edit.InsertText.Length));
                }
            }

            // add any remaining text
            if (pos < this.CurrentText.Length)
            {
                builder.Append(this.CurrentText, pos, this.CurrentText.Length - pos);
            }

            var newText = builder.ToString();
            var newMap = _map.ReplaceAll(mapEdits);
            return new EditString(this.OriginalText, newText, newMap);
        }

        /// <summary>
        /// Removes the blank lines from the text.
        /// </summary>
        public EditString RemoveBlankLines()
        {
            var pos = 0;
            var text = this.CurrentText;

            List<StringEdit> edits = null;

            while (pos < text.Length)
            {
                var len = TextFacts.GetLineLength(text, pos, includeLineBreak: true);

                if (TextFacts.IsBlankLine(text, pos))
                {
                    if (edits == null)
                        edits = new List<StringEdit>();
                    edits.Add(StringEdit.Deletion(pos, len));
                }

                pos += len;
            }

            return this.ApplyAll(edits);
        }

        /// <summary>
        /// Replaces all occurrances of <see cref="p:oldValue"/> with <see cref="p:newValue"/>
        /// </summary>
        public EditString Replace(string oldValue, string newValue)
        {
            int startIndex = 0;
            List<StringEdit> edits = null;

            while (true)
            {
                int oldValueStart = this.CurrentText.IndexOf(oldValue, startIndex);
                if (oldValueStart < startIndex)
                    break;

                if (edits == null)
                    edits = new List<StringEdit>();

                edits.Add(StringEdit.Replacement(oldValueStart, oldValue.Length, newValue));

                startIndex = oldValueStart + oldValue.Length;
            }

            return this.ApplyAll(edits);
        }

        /// <summary>
        /// Replaces all line breaks in the text with the specied value.
        /// </summary>
        public EditString ReplaceLineBreaks(string newValue)
        {
            var text = this.CurrentText;
            List<StringEdit> edits = null;

            for (int i = 0; i < text.Length; i++)
            {
                if (TextFacts.IsLineBreakStart(text[i]))
                {
                    var len = TextFacts.GetLineBreakLength(text, i);

                    // only make the edit if the newValue is different than the existing line break
                    if (len != newValue.Length || string.Compare(text, i, newValue, 0, len) != 0)
                    {
                        if (edits == null)
                            edits = new List<StringEdit>();
                        edits.Add(StringEdit.Replacement(i, len, newValue));
                    }

                    // add one less because for-loop will add one back
                    i += len - 1;
                }
            }

            return this.ApplyAll(edits);
        }

        /// <summary>
        /// Gets the position in the current text corresponding to the position in the original text.
        /// If original position corresponds to a region of text that was removed or replaced, it will return the position 
        /// at the start of where the change occurred.
        /// </summary>
        public int GetCurrentPosition(int originalPosition, PositionBias bias = PositionBias.Right)
        {
            return this._map.GetTargetPosition(originalPosition, bias);
        }

        /// <summary>
        /// Gets the position in the original text corresponding to the position in the current text.
        /// If current position corresponds to a region of the text that was inserted, it will return the position
        /// at the start of where change occurred.
        /// </summary>
        public int GetOriginalPosition(int currentPosition, PositionBias bias = PositionBias.Right)
        {
            return this._map.GetSourcePosition(currentPosition, bias);
        }

        /// <summary>
        /// Represents a map between source positions and target positions
        /// that can be adjusted through cumulative edits.
        /// </summary>
        private class EditMap
        {
            private SafeList<Edit> _edits;

            private EditMap(SafeList<Edit> edits)
            {
                _edits = edits;
            }

            public EditMap()
                : this(new SafeList<Edit>(new Edit[] { }))
            {
            }

            public IReadOnlyList<Edit> Edits => _edits;

            /// <summary>
            /// Creates a new source-to-target map that includes replacing a span with another span.
            /// </summary>
            public EditMap Replace(int start, int deleteLength, int insertLength)
            {
                var newEdits = _edits.WithItem(new Edit(start, deleteLength, insertLength));
                return new EditMap(newEdits);
            }

            /// <summary>
            /// Creates a new source-to-target map that includes inserting a span.
            /// </summary>
            public EditMap Insert(int start, int length)
            {
                return Replace(start, 0, length);
            }

            /// <summary>
            /// Creates a new source-to-target map that includes removing of a span.
            /// </summary>
            public EditMap Delete(int start, int length)
            {
                return Replace(start, length, 0);
            }

            /// <summary>
            /// Creates a new source-to-target map that includes all the in order, non overlapping, edits.
            /// </summary>
            public EditMap ReplaceAll(IReadOnlyList<Edit> edits)
            {
                var adjustedEdits = new Edit[edits.Count];

                var delta = 0;
                var minStart = 0;

                for (int i = 0; i < edits.Count; i++)
                {
                    var edit = edits[i];

                    // edits must be must be in order otherwise they will conflict with prior edits in this sequence
                    if (edit.Start < minStart)
                        throw new InvalidOperationException("Edit occurs out of order or overlapping a prior edit.");

                    adjustedEdits[i] = new Edit(edit.Start + delta, edit.DeleteLength, edit.InsertLength);
                    delta += (edit.InsertLength - edit.DeleteLength);

                    // the next edit must occur after any deletion
                    minStart = edit.Start + edit.DeleteLength;
                }

                var newEdits = _edits.WithItems(adjustedEdits);
                return new EditMap(newEdits);
            }

            /// <summary>
            /// Maps a position in the source space to the corresponding position in the target space.
            /// </summary>
            public int GetTargetPosition(int sourcePosition, PositionBias bias = PositionBias.Right)
            {
                int targetPosition = sourcePosition;

                foreach (var edit in _edits)
                {
                    targetPosition = Translate(
                        targetPosition,
                        edit.Start,
                        edit.DeleteLength,
                        edit.InsertLength,
                        bias);
                }

                return targetPosition;
            }

            /// <summary>
            /// Reverse maps a position from the target space to the correpsonding position in the source space.
            /// </summary>
            public int GetSourcePosition(int targetPosition, PositionBias bias = PositionBias.Right)
            {
                int sourcePosition = targetPosition;

                for (int i = _edits.Count - 1; i >= 0; i--)
                {
                    var edit = _edits[i];

                    // since we are doing the reverse operation
                    // deletes are inserts and inserts are deletes
                    sourcePosition = Translate(
                        position: sourcePosition,
                        start: edit.Start,
                        deleteLength: edit.InsertLength,
                        insertLength: edit.DeleteLength,
                        bias: bias);
                }

                return sourcePosition;
            }

            /// <summary>
            /// Translate a position across an edit
            /// </summary>
            private static int Translate(int position, int start, int deleteLength, int insertLength, PositionBias bias)
            {
                if (deleteLength > 0 && position > start)
                {
                    if (position > start + deleteLength)
                    {
                        // after deleted range, adjust downward
                        position -= deleteLength;
                    }
                    else
                    {
                        // inside deleted range, adjust to start
                        position = start;
                    }
                }

                if (insertLength > 0)
                {
                    // if after the insert position, adjust upward
                    // or at the insert position and bias is toward right, also adjust upward
                    if (position > start || (bias == PositionBias.Right && position == start))
                        position += insertLength;
                }

                return position;
            }

            public struct Edit
            {
                public int Start { get; }
                public int DeleteLength { get; }
                public int InsertLength { get; }

                public Edit(int start, int deleteLength, int insertLength)
                {
                    this.Start = start;
                    this.DeleteLength = deleteLength;
                    this.InsertLength = insertLength;
                }
            }
        }
    }

    public struct StringEdit
    {
        public int Start { get; }
        public int DeleteLength { get; }
        public string InsertText { get; }

        private StringEdit(int start, int deleteLength, string insertText)
        {
            this.Start = start;
            this.DeleteLength = deleteLength;
            this.InsertText = insertText ?? "";
        }

        public static StringEdit Replacement(int start, int deleteLength, string insertText)
        {
            return new StringEdit(start, deleteLength, insertText);
        }

        public static StringEdit Deletion(int start, int deleteLength)
        {
            return new StringEdit(start, deleteLength, null);
        }

        public static StringEdit Insertion(int start, string text)
        {
            return new StringEdit(start, 0, text);
        }
    }

    public enum PositionBias
    {
        /// <summary>
        /// Translate positions to the left on insertion boundaries
        /// </summary>
        Left,

        /// <summary>
        /// Translate positions to the right on insertion boundaries.
        /// </summary>
        Right
    }
}