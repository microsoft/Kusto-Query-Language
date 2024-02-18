namespace Kusto.Language.Parsing
{
    /// <summary>
    /// A parsed value and its source offset.
    /// </summary>
    public struct OffsetValue<TValue>
    {
        /// <summary>
        /// The text offset of the value in the source.
        /// </summary>
        public readonly int Offset;

        /// <summary>
        /// The value located at the offset.
        /// </summary>
        public readonly TValue Value;

        /// <summary>
        /// Constructs a new <see cref="OffsetValue{TValue}"/>
        /// </summary>
        /// <param name="offset">The text offset of the value in the source.</param>
        /// <param name="value">The value located at the offset.</param>
        public OffsetValue(int offset, TValue value)
        {
            this.Offset = offset;
            this.Value = value;
        }
    }
}
