using System.Collections.Generic;

namespace Kusto.Language.Symbols
{
    using System.Linq;
    using Utils;

    /// <summary>
    /// A symbol representing an entity group element.
    /// </summary>
    public sealed class EntityGroupElementSymbol : TypeSymbol
    {
        public EntityGroupSymbol EntityGroup { get; }
        public TypeSymbol UnderlyingSymbol { get; }

        public EntityGroupElementSymbol(string name, EntityGroupSymbol entityGroup)
            : base(name)
        {
            this.EntityGroup = entityGroup.CheckArgumentNull(nameof(entityGroup));

            // currently, the underlying symbol is just this first symbol in the group
            this.UnderlyingSymbol = entityGroup?.Members.OfType<TypeSymbol>().FirstOrDefault(s => s.Members.Count > 0);

            if (this.UnderlyingSymbol == null)
                this.UnderlyingSymbol = entityGroup?.Members.OfType<TypeSymbol>().FirstOrDefault();

            if (this.UnderlyingSymbol == null)
                this.UnderlyingSymbol = ErrorSymbol.Instance;
        }

        public override IReadOnlyList<Symbol> Members => SpecialMembers;
        public override Tabularity Tabularity => this.UnderlyingSymbol.Tabularity;
        public override SymbolKind Kind => SymbolKind.EntityGroupElement;

        public static readonly IReadOnlyList<Symbol> SpecialMembers =
            new Symbol[]
            {
                new VariableSymbol("$current_database", ScalarTypes.String),
                new VariableSymbol("$current_cluster_endpoint", ScalarTypes.String)
            };
    }
}