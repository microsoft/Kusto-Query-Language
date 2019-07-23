namespace Kusto.Language.Editor
{
    /// <summary>
    /// A range of text.
    /// </summary>
    public class TextRange
    {
        /// <summary>
        /// The starting position of the range in the text.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// The length of the range in the text.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// The ending of the range in the text.
        /// </summary>
        public int End => this.Start + this.Length;

        public TextRange(int start, int length)
        {
            this.Start = start;
            this.Length = length;
        }
    }
}