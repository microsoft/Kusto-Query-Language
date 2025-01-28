namespace Kusto.Language.Editor
{
    public class ClientParameter : SyntaxReference
    {
        public string Name { get; }
        public int? Index { get; }

        public ClientParameter(string name, int? index, TextRange range)
            : base(range)
        {
            this.Name = name;
            this.Index = index;
        }

        public ClientParameter(string name, int? index, int start, int length)
            : this(name, index, new TextRange(start, length))
        {
        }

        public ClientParameter(string name, TextRange range)
            : this(name, null, range)
        {
        }

        public ClientParameter(string name, int start, int length)
            : this(name, null, start, length)
        {
        }

        public override string ToString()
        {
            if (Index != null)
            {
                return $"{Name}[{Index}]";
            }
            else
            {
                return Name;
            }
        }
    }
}