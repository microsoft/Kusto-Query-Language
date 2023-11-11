namespace Kusto.Language.Syntax
{
    /// <summary>
    /// Represents the associated info of a literal found in syntax.
    /// </summary>
    public class ValueInfo
    {
        /// <summary>
        /// The value as a CLR value.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// The text of the value.
        /// This is the unescaped value of a string literal or the raw interior text of other literals.
        /// </summary>
        public string ValueText { get; }

        /// <summary>
        /// The text of the value as specified in the expression.
        /// </summary>
        public string RawText { get; }

        internal ValueInfo(string rawText, string valueText, object value)
        {
            this.RawText = rawText;
            this.ValueText = valueText;
            this.Value = value;
        }
    }
}
