using System.Collections.Generic;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol representing an entity group element.
    /// </summary>
    public sealed class EntityGroupElementSymbol : TypeSymbol
    {
        public TypeSymbol UnderlyingSymbol { get; }

        public EntityGroupElementSymbol(string name, TypeSymbol underlyingSymbol)
            : base(name)
        {
            this.UnderlyingSymbol = underlyingSymbol.CheckArgumentNull(nameof(underlyingSymbol));
        }

        public override IReadOnlyList<Symbol> Members => SpecialMembers;
        public override Tabularity Tabularity => this.UnderlyingSymbol.Tabularity;
        public override SymbolKind Kind => SymbolKind.EntityGroupElement;

        protected override string GetDisplay() => this.UnderlyingSymbol.Display;

        public static readonly IReadOnlyList<Symbol> SpecialMembers =
            new Symbol[]
            {
                new VariableSymbol("$current_database", ScalarTypes.String),
                new VariableSymbol("$current_cluster_endpoint", ScalarTypes.String)
            };
    }
}