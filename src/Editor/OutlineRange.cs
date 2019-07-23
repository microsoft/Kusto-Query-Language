namespace Kusto.Language.Editor
{
    public class OutlineRange
    {
        /// <summary>
        /// The start of the outline range.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// The length of the outline range.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// The text to show when the range is collapsed.
        /// </summary>
        public string CollapsedText { get; }

        public OutlineRange(int start, int length, string collapsedText)
        {
            this.Start = start;
            this.Length = length;
            this.CollapsedText = collapsedText ?? "";
        }
    }
}