namespace Kusto.Language.Editor
{
    public class FormattedText
    {
        /// <summary>
        /// The formatted text
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// The new cursor position within the newly formatted text.
        /// </summary>
        public int Position { get; }

        public FormattedText(string newText, int newPosition)
        {
            this.Text = newText;
            this.Position = newPosition;
        }
    }
}