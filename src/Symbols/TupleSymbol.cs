using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol for a tuple of one or more name/value pairs.
    /// </summary>
    public sealed class TupleSymbol : TypeSymbol
    {
        public IReadOnlyList<ColumnSymbol> Columns { get; }

        public override IReadOnlyList<Symbol> Members => this.Columns;

        public override SymbolKind Kind => SymbolKind.Tuple;

        public TupleSymbol(IEnumerable<ColumnSymbol> columns)
            : base("tuple")
        {
            this.Columns = columns.ToReadOnly();
        }

        public TupleSymbol(params ColumnSymbol[] columns)
            : this((IEnumerable<ColumnSymbol>)columns)
        {
        }

        public TupleSymbol WithColumns(IEnumerable<ColumnSymbol> columns)
        {
            return new TupleSymbol(columns);
        }

        public override Tabularity Tabularity => Tabularity.Scalar;

        protected override string GetDisplay() =>
            $"{{{string.Join(", ", this.Members.Select(m => m.Display))}}}";
    }
}