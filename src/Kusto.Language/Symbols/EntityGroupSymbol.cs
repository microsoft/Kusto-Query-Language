using System.Collections.Generic;
using System.Text;

namespace Kusto.Language.Symbols
{
    using Syntax;
    using System;
    using System.Linq;
    using Utils;

    /// <summary>
    /// A symbol representing an entity group.
    /// </summary>
    public sealed class EntityGroupSymbol : TypeSymbol
    {
        internal Signature Signature { get; }
        public string Description { get; }

        public EntityGroupSymbol(string name, string definition = null, string description = null)
            : base(name)
        {
            this.Signature = new Signature(definition ?? "", Tabularity.Tabular);
            this.Signature.Symbol = this;
            _members = EmptyReadOnlyList<Symbol>.Instance;
        }

        private readonly IReadOnlyList<Symbol> _members;

        public EntityGroupSymbol(string name, IEnumerable<Symbol> members)
            : base(name)
        {
            _members = members.ToReadOnly();
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

        public override IReadOnlyList<Symbol> Members => _members;

        public string Definition => this.Signature.Body;

        public override Tabularity Tabularity => Tabularity.None;

        public override SymbolKind Kind => SymbolKind.EntityGroup;

        protected override string GetDisplay() =>
            $"entity_group({this.Name}:{string.Join(", ", this.Members.Select(m => m.Display))})";
    }
}