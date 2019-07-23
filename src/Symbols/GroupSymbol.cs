using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol corresponding to a group of symbols.
    /// This symbol occurs when a name reference is ambigous.
    /// </summary>
    public sealed class GroupSymbol : TypeSymbol
    {
        public override SymbolKind Kind => SymbolKind.Group;

        private readonly IReadOnlyList<Symbol> members;

        public override IReadOnlyList<Symbol> Members => this.members;

        public GroupSymbol(IEnumerable<Symbol> symbols)
            : base("group")
        {
            this.members = symbols.ToReadOnly();
        }

        public GroupSymbol(params Symbol[] symbols)
            : this((IEnumerable<Symbol>)symbols)
        {
        }

        protected override string GetDisplay()
        {
            return "[" + string.Join(", ", this.Members.Select(s => s.Display)) + "]";
        }

        public override Tabularity Tabularity =>
            this.Members[0].Tabularity;
    }
}