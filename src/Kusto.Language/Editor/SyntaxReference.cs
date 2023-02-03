namespace Kusto.Language.Editor
{
    /// <summary>
    /// The location of a piece of syntax in the text of the code.
    /// </summary>
    public class SyntaxReference
    {
        /// <summary>
        /// The text range of the <see cref="SyntaxReference"/>
        /// </summary>
        public TextRange Range { get; }

        /// <summary>
        /// The starting text position of the <see cref="SyntaxReference"/>
        /// </summary>
        public int Start => Range.Start;

        /// <summary>
        /// The text length of the <see cref="SyntaxReference"/>
        /// </summary>
        public int Length => Range.Length;

        /// <summary>
        /// The position after the end of the <see cref="SyntaxReference"/>
        /// </summary>
        public int End => Range.End;

        public SyntaxReference(TextRange range)
        {
            this.Range = range;
        }

        public SyntaxReference(int start, int length)
            : this(new TextRange(start, length))
        {
        }
    }
}