using System.Collections.Generic;
using System.Text;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol representing an entity group.
    /// </summary>
    public sealed class EntityGroupSymbol : TypeSymbol
    {
        public string Definition { get; }
        public string Description { get; }
        private readonly IReadOnlyList<Symbol> _members;
        internal Signature Signature { get; }

        public EntityGroupSymbol(string name, string definition, string description = null)
            : base(name)
        {
            this.Definition = definition;
            this.Description = description ?? "";
            this.Signature = CreateSignature(definition);
            this.Signature.Symbol = this;
            _members = EmptyReadOnlyList<Symbol>.Instance;
        }

        public EntityGroupSymbol(string name, IEnumerable<Symbol> members, string description = null)
            : base(name)
        {
            this.Definition = null;
            this.Description = description ?? "";
            _members = members.ToReadOnly().CheckArgumentNullOrElementNull(nameof(members));
            this.Signature = new Signature(this);
            this.Signature.Symbol = this;
        }

        public EntityGroupSymbol(string name, params Symbol[] members)
            : this(name, (IEnumerable<Symbol>)members)
        {
        }

        public EntityGroupSymbol(params Symbol[] members)
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
                return "entity_group []";
            }

            definition = definition.Trim();

            // already a entity group expression
            if (definition.StartsWith("entity_group"))
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

            return $"entity_group [{expressionList}]";
        }

        public override IReadOnlyList<Symbol> Members => _members;

        public override Tabularity Tabularity => Tabularity.None;

        public override SymbolKind Kind => SymbolKind.EntityGroup;

        public static readonly EntityGroupSymbol Empty = new EntityGroupSymbol();
    }
}