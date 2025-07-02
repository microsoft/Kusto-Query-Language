namespace Kusto.Language.Editor
{
    /// <summary>
    /// A client directive argument info.
    /// </summary>
    public class ClientDirectiveArgument
    {
        /// <summary>
        /// An optional name assigned to the argument: name = value
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The unparsed text of the argument value.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// The parsed value of the argument: double, long or string.
        /// </summary>
        public object Value { get; }

        public ClientDirectiveArgument(string name, string text, object value)
        {
            this.Name = name;
            this.Text = text;
            this.Value = value;
        }
    }
}