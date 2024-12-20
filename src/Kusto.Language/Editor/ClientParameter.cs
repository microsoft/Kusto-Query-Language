namespace Kusto.Language.Editor
{
    public class ClientParameter : SyntaxReference
    {
        public string Name { get; }
        public int Index { get; }

        public ClientParameter(string name, int index, TextRange range)
            : base(range)
        {
            this.Name = name;
            this.Index = index;
        }

        public ClientParameter(string name, int index, int start, int length)
            : this(name, index, new TextRange(start, length))
        {
        }

        public ClientParameter(string name, TextRange range)
            : this(name, 0, range)
        {
        }

        public ClientParameter(string name, int start, int length)
            : this(name, 0, start, length)
        {
        }
    }
}