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
        /// <summary>
        /// The associated <see cref="EntityGroupSymbol"/>.
        /// </summary>
        public EntityGroupSymbol EntityGroup { get; }

        /// <summary>
        /// The actual entity symbol within the entity group that is currently being referenced.
        /// </summary>
        public TypeSymbol UnderlyingSymbol { get; }

        public EntityGroupElementSymbol(string name, EntityGroupSymbol entityGroup, TypeSymbol underlyingSymbol)
            : base(name)
        {
            this.EntityGroup = entityGroup ?? EntityGroupSymbol.Empty;

            if (underlyingSymbol == null)
            {
                // Use the first entity in the group with members, otherwise the first entity.
                underlyingSymbol = entityGroup?.Members.OfType<TypeSymbol>().FirstOrDefault(entity => entity.Members.Count > 0)
                    ?? entityGroup?.Members.OfType<TypeSymbol>().FirstOrDefault();
            }
            else
            {
                // if specified in constructor, must be one of symbols in group.
                System.Diagnostics.Debug.Assert(entityGroup.Members.Any(m => m == underlyingSymbol));
            }

            if (underlyingSymbol == null)
                underlyingSymbol = ErrorSymbol.Instance;

            this.UnderlyingSymbol = underlyingSymbol;
        }

        public EntityGroupElementSymbol(string name, EntityGroupSymbol entityGroup)
            : this(name, entityGroup, null)
        {
        }

        public EntityGroupElementSymbol(string name)
            : this(name, null, null)
        {
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