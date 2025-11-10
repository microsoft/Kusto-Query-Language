// <#+
#if !T4
namespace Kusto.Language.Generators
{
#endif

    public class CommandInfo
    {
        public string Name { get; }
        public string Grammar { get; }
        public string Schema { get; }
        public string Construction { get; }

        public CommandInfo(string name, string grammar, string schema, string construction = null)
        {
            this.Name = name;
            this.Grammar = grammar;
            this.Schema = schema;
            this.Construction = construction;
        }
    }

#if !T4
}
#endif
// #>