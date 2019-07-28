namespace Kusto.Language.Editor
{
    /// <summary>
    /// The location of a piece of syntax in the text of the code.
    /// </summary>
    public class SyntaxReference : TextRange
    {
        public SyntaxReference(int start, int length)
            : base(start, length)
        {
        }
    }
}