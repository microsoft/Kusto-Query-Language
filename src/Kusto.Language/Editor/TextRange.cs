using System;

namespace Kusto.Language.Editor
{
    /// <summary>
    /// A range of text.
    /// </summary>
    public struct TextRange
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

        public static TextRange FromBounds(int start, int end)
        {
            return new TextRange(start, end - start);
        }

        public static readonly TextRange Empty = new TextRange(0, 0);

        /// <summary>
        /// True if this text range overlaps the other text range.
        /// </summary>
        public bool Overlaps(TextRange other)
        {
            return Overlaps(this.Start, this.Length, other.Start, other.Length);
        }

        /// <summary>
        /// True if the range A overlaps the range B
        /// </summary>
        public static bool Overlaps(int startA, int lengthA, int startB, int lengthB)
        {
            var endA = startA + lengthA;
            var endB = startB + lengthB;
            return Math.Max(startA, startB) <= Math.Min(endA, endB);
        }
    }
}