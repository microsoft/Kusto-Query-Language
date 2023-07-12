using System;
using System.Diagnostics;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// Represents a single edit to a text document: insert, delete or replace.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Start: {Start}, Delete: {DeleteLength}, Insert: {InsertText}")]
    public struct TextEdit
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
        /// The text to be inserted at the start position, (after any deletion).
        /// </summary>
        public string InsertText { get; }

        /// <summary>
        /// Create a new <see cref="TextEdit"/>
        /// </summary>
        public TextEdit(int start, int deleteLength, string insertText)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException("start", "negate start position");
            if (deleteLength < 0)
                throw new ArgumentOutOfRangeException("deleteLength", "negative delete length");
            this.Start = start;
            this.DeleteLength = deleteLength;
            this.InsertText = insertText ?? "";
        }

        /// <summary>
        /// Creates a replacement edit.
        /// </summary>
        public static TextEdit Replacement(int start, int deleteLength, string insertText)
        {
            return new TextEdit(start, deleteLength, insertText);
        }

        /// <summary>
        /// Creates a delete edit.
        /// </summary>
        public static TextEdit Deletion(int start, int deleteLength)
        {
            return new TextEdit(start, deleteLength, "");
        }

        /// <summary>
        /// Creates a insert edit.
        /// </summary>
        public static TextEdit Insertion(int start, string text)
        {
            return new TextEdit(start, 0, text);
        }
    }
}