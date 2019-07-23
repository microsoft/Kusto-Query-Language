namespace Kusto.Language.Editor
{
    public class ClientParameter : SyntaxReference
    {
        public string Name { get; }

        public ClientParameter(string name, int position, int length)
            : base(position, length)
        {
            this.Name = name;
        }
    }
}