namespace Kusto.Language.Parsing
{
    public struct ParseResult<TOutput>
    {
        /// <summary>
        /// The number of input items consumed by the parser or a negative number if the parsing failed.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// The single produced result of the parser.
        /// </summary>
        public TOutput Value { get; }

        /// <summary>
        /// True if the parse was successful.
        /// </summary>
        public bool Succeeded => Length >= 0;

        public ParseResult(int length, TOutput value)
        {
            this.Length = length;
            this.Value = value;
        }
    }
}
