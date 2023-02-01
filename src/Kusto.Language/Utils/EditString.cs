using System;
using System.Collections.Generic;
using System.Linq;
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
        /// The list of in order, non-overlapping edits, each relative the the original text.
        /// </summary>
        private IReadOnlyList<Edit> _edits;

        /// <summary>
        /// Constructs a new <see cref="EditString"/>.
        /// </summary>
        private EditString(string originalText, string currentText, IReadOnlyList<Edit> edits)
        {
            this.OriginalText = originalText ?? "";
            this.CurrentText = currentText ?? "";
            _edits = edits;
        }

        /// <summary>
        /// Create a new <see cref="EditString"/> in a pre-edit state.
        /// </summary>
        public EditString(string text)
            : this(text, text, EmptyReadOnlyList<Edit>.Instance)
        {
        }

        /// <summary>
        /// The length of the current text.
        /// </summary>
        public int Length => this.CurrentText.Length;

        /// <summary>
        /// The character at the index of the current text.
        /// </summary>
        public char this[int index] => this.CurrentText[index];

        /// <summary>
        /// The starting index of the value within the current text.
        /// </summary>
        public int IndexOf(string value) => this.CurrentText.IndexOf(value);

        /// <summary>
        /// Converts the <see cref="EditString"/> to just its current text.
        /// </summary>
        public static implicit operator string(EditString editString)
        {
            return editString != null ? editString.CurrentText : null;
        }

        /// <summary>
        /// Converts a string into an <see cref="EditString"/> without any edits.
        /// </summary>
        public static implicit operator EditString(string text)
        {
            return text != null ? new EditString(text) : null;
        }

        /// <summary>
        /// Returns a list of changes between the original and current text
        /// such that if applied to the original text (via ApplyAll) would produce the current text.
        /// This list of changes is not guaranteed to match the exact sequence of edits originally applied.
        /// </summary>
        public IReadOnlyList<StringEdit> GetChanges()
        {
            var result = new List<StringEdit>();

            var delta = 0;
            foreach (var edit in _edits)
            {
                result.Add(StringEdit.Replacement(edit.Start, edit.DeleteLength, this.CurrentText.Substring(edit.Start + delta, edit.InsertLength)));
                delta = delta - edit.DeleteLength + edit.InsertLength;
            }

            return result;
        }

        /// <summary>
        /// Convert a list of changes to a cummulative list, that can be applied one by one (via Apply)
        /// </summary>
        public static IReadOnlyList<StringEdit> ConvertToCummulativeChanges(IReadOnlyList<StringEdit> changes)
        {
            var result = new List<StringEdit>();

            var delta = 0;
            foreach (var edit in changes)
            {
                result.Add(StringEdit.Replacement(edit.Start + delta, edit.DeleteLength, edit.InsertText));
                delta = delta - edit.DeleteLength + edit.InsertText.Length;
            }

            return result;
        }

        /// <summary>
        /// Returns the current text.
        /// </summary>
        public override string ToString() => this.CurrentText;

        /// <summary>
        /// Replace the range of characters with the specified text.
        /// </summary>
        public EditString ReplaceAt(int start, int length, string text)
        {
            return Apply(StringEdit.Replacement(start, length, text));
        }

        /// <summary>
        /// Inserts the text at the specified position.
        /// </summary>
        public EditString Insert(int position, string text)
        {
            return ReplaceAt(position, 0, text);
        }

        /// <summary>
        /// Appends the text to the end of the <see cref="EditString"/>
        /// </summary>
        public EditString Append(string text)
        {
            return ReplaceAt(this.CurrentText.Length, 0, text);
        }

        /// <summary>
        /// Prepends the text at the start of the <see cref="EditString"/>
        /// </summary>
        public EditString Prepend(string text)
        {
            return ReplaceAt(0, 0, text);
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

            var newEdits = new List<Edit>(2);

            var endDeleteStart = start + length;
            var endDeleteLength = this.CurrentText.Length - endDeleteStart;

            if (start > 0)
            {
                // remove first 'start' characters
                newEdits.Add(new Edit(0, start, 0));
            }

            if (endDeleteStart > 0)
            {
                // remove last 'endDeleteLength' characters
                newEdits.Add(new Edit(endDeleteStart, endDeleteLength, 0));
            }

            return ApplyEdits(newText, newEdits);
        }

        /// <summary>
        /// Replaces all occurrances of <see cref="p:oldValue"/> with <see cref="p:newValue"/>
        /// </summary>
        public EditString Replace(string oldValue, string newValue)
        {
            if (oldValue == null)
                throw new ArgumentNullException(nameof(oldValue));

            if (newValue == null)
                throw new ArgumentNullException(nameof(newValue));

            var newText = this.CurrentText.Replace(oldValue, newValue);

            // if nothing changed, return same EditString
            if (newText == this.CurrentText)
                return this;

            int startIndex = 0;
            var newEdits = new List<Edit>();

            while (true)
            {
                int oldValueStart = this.CurrentText.IndexOf(oldValue, startIndex);
                if (oldValueStart < startIndex)
                    break;

                newEdits.Add(new Edit(oldValueStart, oldValue.Length, newValue.Length));

                startIndex = oldValueStart + oldValue.Length;
            }

            return ApplyEdits(newText, newEdits);
        }

        /// <summary>
        /// Returns a new <see cref="EditString"/> with the single edit applied.
        /// </summary>
        public EditString Apply(StringEdit edit)
        {
            if (edit.Start < 0 || edit.Start > this.Length + 1)
                throw new ArgumentOutOfRangeException(nameof(edit.Start));

            if (edit.Start + edit.DeleteLength > this.Length + 1)
                throw new ArgumentOutOfRangeException(nameof(edit.DeleteLength));

            var newText = this.CurrentText;

            if (edit.DeleteLength > 0)
            {
                newText = newText.Remove(edit.Start, edit.DeleteLength);
            }

            if (edit.InsertText.Length > 0)
            {
                newText = newText.Insert(edit.Start, edit.InsertText);
            }

            // only apply edit if it actually makes a change.
            if (edit.InsertText.Length != edit.DeleteLength
                || string.Compare(this.CurrentText, edit.Start, edit.InsertText, 0, edit.InsertText.Length) != 0)
            {
                return ApplyEdits(newText, new[] { new Edit(edit.Start, edit.DeleteLength, edit.InsertText.Length) });
            }
            else
            {
                return new EditString(this.OriginalText, newText, _edits);
            }
        }

        /// <summary>
        /// Returns a new <see cref="EditString"/> with all the edits applied.
        /// Each edit is specified against positions in the current text, must be in order and non-overlapping.
        /// </summary>
        public EditString ApplyAll(IReadOnlyList<StringEdit> edits)
        {
            if (edits == null)
                return this;

            if (!CanApplyAll(edits))
                throw new InvalidOperationException("An edit occurs out of order, overlaps a prior edit or is specified out of bounds of the current text.");

            var newText = GetNewText(this.CurrentText, edits);
            var newEdits = edits.Select(e => new Edit(e.Start, e.DeleteLength, e.InsertText.Length)).ToArray();

            return ApplyEdits(newText, newEdits);
        }

        /// <summary>
        /// Return true if the list of edits can be applied via ApplyAll.
        /// Returns false if the edits are not in order, overlapping and out of bounds of the current text.
        /// </summary>
        public bool CanApplyAll(IReadOnlyList<StringEdit> edits)
        {
            var priorEnd = 0;

            foreach (var edit in edits)
            {
                if (edit.Start < priorEnd 
                    || edit.Start > this.CurrentText.Length + 1)
                {
                    return false;
                }

                priorEnd = edit.Start + edit.DeleteLength;
            }

            return true;
        }

        /// <summary>
        /// Apply edits to text to get new text.
        /// </summary>
        private static string GetNewText(string text, IEnumerable<StringEdit> edits)
        {
            var builder = new StringBuilder();

            // the end position of the last edit in the newest text. 
            var priorEnd = 0;

            // construct the new current text and the new list of edits
            foreach (var edit in edits)
            {
                if (edit.Start > priorEnd)
                {
                    // append anything between this point and the edit start
                    builder.Append(text, priorEnd, edit.Start - priorEnd);
                }

                priorEnd = edit.Start + edit.DeleteLength;

                if (edit.InsertText.Length > 0)
                {
                    builder.Append(edit.InsertText);
                }
            }

            // add any remaining text
            if (priorEnd < text.Length)
            {
                builder.Append(text, priorEnd, text.Length - priorEnd);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Returns a new <see cref="EditString"/> containing both old and new edits.
        /// </summary>
        private EditString ApplyEdits(string newText, IReadOnlyList<Edit> newEdits)
        {
            var combinedEdits = CombineEdits(_edits, newEdits);
            return new EditString(this.OriginalText, newText, combinedEdits);
        }

        /// <summary>
        /// Combines a list of parallel old edits with a list of parallel new edits.
        /// The new edits' positions are relative to after the old edits have been applied.
        /// </summary>
        private static IReadOnlyList<Edit> CombineEdits(IReadOnlyList<Edit> oldEdits, IReadOnlyList<Edit> newEdits)
        {
            if (newEdits.Count == 0)
                return oldEdits;

            var oldDelta = 0;

            var nextOldIndex = 1;
            var nextNewIndex = 1;
            var hasOldEdit = oldEdits.Count > 0;
            var hasNewEdit = newEdits.Count > 0;
            Edit oldEdit = hasOldEdit ? oldEdits[0] : default(Edit);
            Edit newEdit = hasNewEdit ? newEdits[0] : default(Edit);

            var combinedEdits = new List<Edit>(oldEdits.Count + newEdits.Count);

            while (hasOldEdit && hasNewEdit)
            {
                if (oldEdit.DeleteLength == 0 && oldEdit.InsertLength == 0)
                {
                    // there is no actual old edit here, so go to next old edit.
                    if (hasOldEdit = nextOldIndex < oldEdits.Count)
                    {
                        oldEdit = oldEdits[nextOldIndex];
                        nextOldIndex++;
                        continue;
                    }
                    break;
                }
                else if (newEdit.DeleteLength == 0 && newEdit.InsertLength == 0)
                {
                    // there is no actual new edit here, go to the next new edit
                    if (hasNewEdit = nextNewIndex < newEdits.Count)
                    {
                        newEdit = newEdits[nextNewIndex];
                        nextNewIndex++;
                        continue;
                    }
                    break;
                }
                else if (newEdit.DeleteEnd <= oldEdit.Start + oldDelta)
                {
                    // new edit is entirely before the old edit, so this where the new edit belongs

                    // add adjusted new edit
                    combinedEdits.Add(new Edit(newEdit.Start - oldDelta, newEdit.DeleteLength, newEdit.InsertLength));
                    
                    // go to the next new edit
                    if (hasNewEdit = nextNewIndex < newEdits.Count)
                    {
                        newEdit = newEdits[nextNewIndex];
                        nextNewIndex++;
                        continue;
                    }
                    break;
                }
                else if (newEdit.Start >= oldEdit.InsertEnd + oldDelta)
                {
                    // new edit is entirely after the old edit, so go to next old edit and try again
                    combinedEdits.Add(oldEdit);
                    oldDelta = oldDelta - oldEdit.DeleteLength + oldEdit.InsertLength;

                    if (hasOldEdit = nextOldIndex < oldEdits.Count)
                    {
                        oldEdit = oldEdits[nextOldIndex];
                        nextOldIndex++;
                        continue;
                    }
                    break;
                }
                else if (newEdit.Start < oldEdit.Start + oldDelta)
                {
                    // new edit starts before the old edit but the new edit deletion overlaps with the old edit insertion
                    var partialDeleteLength = oldEdit.Start - newEdit.Start;

                    // add the portion of the delete before the overlap
                    combinedEdits.Add(new Edit(newEdit.Start - oldDelta, partialDeleteLength, 0));

                    // adjust new edit to coincide with old edit with the remaining delete
                    newEdit = new Edit(oldEdit.Start + oldDelta, newEdit.DeleteLength - partialDeleteLength, newEdit.InsertLength);
                    continue;
                }
                else if (newEdit.Start > oldEdit.Start + oldDelta)
                {
                    // new edit starts after old edit, but overlaps
                    // split up old edit around new edit start and try again
                    var partialInsertLength = newEdit.Start - oldEdit.Start;
                    var partialDeleteLength = Math.Min(oldEdit.DeleteLength, partialInsertLength);
                    
                    // add the old edits partial delete & insert
                    combinedEdits.Add(new Edit(oldEdit.Start, partialDeleteLength, partialInsertLength));
                    oldDelta = oldDelta - partialDeleteLength + partialInsertLength;

                    // adjust old edit now coincide with new edit with the remaining delete & insert
                    oldEdit = new Edit(newEdit.Start - oldDelta, oldEdit.DeleteLength - partialDeleteLength, oldEdit.InsertLength - partialInsertLength);
                    continue;
                }
                // otherwise both edits start at the same position
                else if (newEdit.DeleteLength <= oldEdit.InsertLength)
                {
                    // new edit deletes less than old edit inserts
                    
                    // adjust old edit to insert less and insert new edit w/deletes before it.
                    oldEdit = new Edit(oldEdit.Start, oldEdit.DeleteLength, oldEdit.InsertLength - newEdit.DeleteLength);
                    oldDelta = oldDelta + newEdit.DeleteLength;

                    // add the remaining portion of the new edit here with only the insert part
                    combinedEdits.Add(new Edit(newEdit.DeleteEnd - oldDelta, 0, newEdit.InsertLength));

                    // go to next new edit
                    if (hasNewEdit = nextNewIndex < newEdits.Count)
                    {
                        newEdit = newEdits[nextNewIndex];
                        nextNewIndex++;
                        continue;
                    }
                    break;
                }
                else
                {
                    // new edit deletes more than old edit inserts

                    // adjust beyond this old edit
                    oldDelta = oldDelta - oldEdit.DeleteLength + oldEdit.InsertLength;

                    // adjust new edit to delete less (and subsume this old edit)
                    var newDeletion = newEdit.DeleteLength + oldEdit.DeleteLength - oldEdit.InsertLength;
                    newEdit = new Edit(oldEdit.Start + oldDelta, newDeletion, newEdit.InsertLength);

                    // go to next old edit
                    if (hasOldEdit = nextOldIndex < oldEdits.Count)
                    {
                        oldEdit = oldEdits[nextOldIndex];
                        nextOldIndex++;
                        continue;
                    }
                    break;
                }
            }

            // add remaining new edits
            if (hasNewEdit)
            {
                // add adjusted new edit
                combinedEdits.Add(new Edit(newEdit.Start - oldDelta, newEdit.DeleteLength, newEdit.InsertLength));
            }

            for (; nextNewIndex < newEdits.Count; nextNewIndex++)
            {
                newEdit = newEdits[nextNewIndex];
                // add adjusted new edit
                combinedEdits.Add(new Edit(newEdit.Start - oldDelta, newEdit.DeleteLength, newEdit.InsertLength));
            }

            // add remaining old edits
            if (hasOldEdit)
            {
                combinedEdits.Add(oldEdit);
            }

            for (; nextOldIndex < oldEdits.Count; nextOldIndex++)
            {
                combinedEdits.Add(oldEdits[nextOldIndex]);
            }

            CombineAdjacentEdits(combinedEdits);

            return combinedEdits;
        }

        private static void CombineAdjacentEdits(List<Edit> edits)
        {
            for (int i = edits.Count - 1; i >= 1; i--)
            {
                var edit = edits[i];
                var prevEdit = edits[i - 1];
                if (prevEdit.DeleteEnd == edit.Start)
                {
                    edits[i - 1] = new Edit(prevEdit.Start, prevEdit.DeleteLength + edit.DeleteLength, prevEdit.InsertLength + edit.InsertLength);
                    edits.RemoveAt(i);
                }
            }
        }

        [System.Diagnostics.DebuggerDisplay("Start: {Start}, Delete: {DeleteLength}, Insert: {InsertLength}")]
        private struct Edit
        {
            public int Start { get; }
            public int DeleteLength { get; }
            public int InsertLength { get; }

            public int DeleteEnd => Start + DeleteLength;
            public int InsertEnd => Start + InsertLength;

            public Edit(int start, int deleteLength, int insertLength)
            {
                this.Start = start;
                this.DeleteLength = deleteLength;
                this.InsertLength = insertLength;
            }
        }

        /// <summary>
        /// Gets the position in the current text corresponding to the position in the original text.
        /// If original position corresponds to a region of text that was removed or replaced, it will return the position 
        /// at the start of where the change occurred.
        /// </summary>
        public int GetCurrentPosition(int originalPosition, PositionBias bias = PositionBias.Right)
        {
            int currentPosition = originalPosition;
            int delta = 0;

            for (int i = 0; i < _edits.Count; i++)
            {
                var edit = _edits[i];

                if (currentPosition < edit.Start + delta)
                    break;

                currentPosition = Translate(
                    currentPosition,
                    edit.Start + delta,
                    edit.DeleteLength,
                    edit.InsertLength,
                    bias);

                delta = delta - edit.DeleteLength + edit.InsertLength;
            }

            return currentPosition;
        }

        /// <summary>
        /// Gets the position in the original text corresponding to the position in the current text.
        /// If current position corresponds to a region of the text that was inserted, it will return the position
        /// at the start of where change occurred.
        /// </summary>
        public int GetOriginalPosition(int currentPosition, PositionBias bias = PositionBias.Right)
        {
            int originalPosition = currentPosition;
            int delta = 0;

            for (int i = 0; i < _edits.Count; i++)
            {
                var edit = _edits[i];

                if (originalPosition < edit.Start + delta)
                    break;

                // since we are doing the reverse operation
                // deletes are inserts and inserts are deletes
                originalPosition = Translate(
                    position: originalPosition,
                    start: edit.Start + delta,
                    deleteLength: edit.InsertLength,
                    insertLength: edit.DeleteLength,
                    bias: bias);

                //delta = delta - edit.InsertLength + edit.DeleteLength;
            }

            return originalPosition;
        }

        /// <summary>
        /// Translate a position across an edit.
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

        /// <summary>
        /// Gets the smallest range within the current text encompassing all of the changes.
        /// </summary>
        public EditRange GetChangeRange()
        {
            if (_edits.Count > 0)
            {
                var firstStart = GetCurrentPosition(_edits[0].Start, PositionBias.Left);
                var lastEnd = GetCurrentPosition(_edits[_edits.Count - 1].Start, PositionBias.Right);
                return EditRange.FromBounds(firstStart, lastEnd);
            }
            else
            {
                return EditRange.Empty;
            }
        }

#if false
        /// <summary>
        /// Gets all the change ranges within the current text.
        /// </summary>
        public IReadOnlyList<EditRange> GetChangeRanges()
        {
            if (_edits.Count > 0)
            {
                var ranges = new List<EditRange>(_edits.Count);
                for (int i = 0; i < _edits.Count; i++)
                {
                    var edit = _edits[i];
                    var start = GetCurrentPosition(edit.Start, PositionBias.Left, i);
                    var end = GetCurrentPosition(edit.Start + edit.InsertText.Length - edit.DeleteLength, PositionBias.Right, i);
                    ranges.Add(EditRange.FromBounds(start, end));
                }
                return ranges.ToReadOnly();
            }
            else
            {
                return EmptyReadOnlyList<EditRange>.Instance;
            }
        }
#endif

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
        /// Replaces all line breaks in the text with the specied value.
        /// </summary>
        public EditString ReplaceLineBreaks(string newValue)
        {
            if (newValue == null)
                throw new ArgumentNullException(nameof(newValue));

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
    }

    [System.Diagnostics.DebuggerDisplay("Start: {Start}, Delete: {DeleteLength}, Insert: {InsertText}")]
    public struct StringEdit
    {
        /// <summary>
        /// The text position of the start of the edit.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// The number of characters to delete starting at the start position.
        /// </summary>
        public int DeleteLength { get; }

        /// <summary>
        /// The number of characters to insert at the start position, (after any deletion).
        /// </summary>
        public string InsertText { get; }

        public StringEdit(int start, int deleteLength, string insertText)
        {
            if (insertText == null)
                throw new ArgumentNullException(nameof(insertText));

            this.Start = start;
            this.DeleteLength = deleteLength;
            this.InsertText = insertText;
        }

        public static StringEdit Replacement(int start, int deleteLength, string insertText)
        {
            return new StringEdit(start, deleteLength, insertText);
        }

        public static StringEdit Deletion(int start, int deleteLength)
        {
            return new StringEdit(start, deleteLength, "");
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

    public struct EditRange
    {
        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length;

        public EditRange(int start, int length)
        {
            this.Start = start;
            this.Length = length;
        }

        public static EditRange FromBounds(int start, int end)
        {
            var trueStart = Math.Min(start, end);
            var trueEnd = Math.Max(start, end);
            return new EditRange(trueStart, trueEnd - trueStart);
        }

        public static EditRange Empty = new EditRange(0, 0);
    }
}