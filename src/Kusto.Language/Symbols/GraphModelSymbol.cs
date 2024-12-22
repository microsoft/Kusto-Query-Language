using System.Collections.Generic;
using System.Text;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol representing a graph model.
    /// </summary>
    public sealed class GraphModelSymbol : TypeSymbol
    {
        public string Definition { get; }
        public string Description { get; }
        private readonly IReadOnlyList<Symbol> _members;
        internal Signature Signature { get; }

        public GraphModelSymbol(string name, string definition, string description = null)
            : base(name)
        {
            this.Definition = definition;
            this.Description = description ?? "";
            this.Signature = CreateSignature(definition);
            this.Signature.Symbol = this;
            _members = EmptyReadOnlyList<Symbol>.Instance;
        }

        public GraphModelSymbol(string name, IEnumerable<Symbol> members, string description = null)
            : base(name)
        {
            this.Definition = null;
            this.Description = description ?? "";
            _members = members.ToReadOnly().CheckArgumentNullOrElementNull(nameof(members));
            this.Signature = new Signature(this);
            this.Signature.Symbol = this;
        }

        public GraphModelSymbol(string name, params Symbol[] members)
            : this(name, (IEnumerable<Symbol>)members)
        {
        }

        public GraphModelSymbol(params Symbol[] members)
            : this("", (IEnumerable<Symbol>)members)
        {
        }

        private static Signature CreateSignature(string definition)
        {
            var body = GetBodyFromDefinition(definition);
            return new Signature(body, Tabularity.Tabular);
        }

        internal static string GetBodyFromDefinition(string definition)
        {
            if (definition == null)
            {
                return "graph_model []";
            }

            definition = definition.Trim();

            // already a graph model expression
            if (definition.StartsWith("graph_model"))
            {
                return definition;
            }

            string expressionList = definition;

            // remove brackets
            if (definition.StartsWith("[") && definition.EndsWith("]"))
            {
                expressionList = definition.Substring(1, definition.Length - 2);           
            }

            expressionList = expressionList.Trim();

            // get literal value
            if (expressionList.StartsWith("\"") || expressionList.StartsWith("'"))
            {
                expressionList = KustoFacts.GetStringLiteralValue(expressionList);
            }

            return $"graph_model [{expressionList}]";
        }

        public override IReadOnlyList<Symbol> Members => _members;

        public override Tabularity Tabularity => Tabularity.None;

        public override SymbolKind Kind => SymbolKind.Graph;

        public static readonly GraphModelSymbol Empty = new GraphModelSymbol();
    }
}