namespace Kusto.Language.Editor
{
    public class ClientParameter : SyntaxReference
    {
        public string Name { get; }

        public ClientParameter(string name, TextRange range)
            : base(range)
        {
            this.Name = name;
        }

        public ClientParameter(string name, int start, int length)
            : this(name, new TextRange(start, length))
        {
        }
    }
}