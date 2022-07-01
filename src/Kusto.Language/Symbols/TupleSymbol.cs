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

        public TableSymbol RelatedTable { get; }

        public TupleSymbol(IEnumerable<ColumnSymbol> columns, TableSymbol relatedTable = null)
            : base("tuple")
        {
            this.Columns = columns.ToReadOnly();
            this.RelatedTable = relatedTable;
        }

        public TupleSymbol(params ColumnSymbol[] columns)
            : this((IEnumerable<ColumnSymbol>)columns)
        {
        }

        /// <summary>
        /// Create a <see cref="TupleSymbol"/> instance from a schema description: (col: type, ...)
        /// </summary>
        public static TupleSymbol From(string schema)
        {
            return new TupleSymbol(TableSymbol.From(schema).Columns);
        }

        public override Tabularity Tabularity => Tabularity.Scalar;

        /// <summary>
        /// If true, then a single column tuple can be reduced to the scalar value of that column.
        /// </summary>
        public bool IsReducibleToScalar => this.Columns.Count == 1 && this.RelatedTable == null;

        public TupleSymbol WithColumns(IEnumerable<ColumnSymbol> columns)
        {
            return new TupleSymbol(columns, this.RelatedTable);
        }

        protected override string GetDisplay() =>
            $"{{{string.Join(", ", this.Members.Select(m => m.Display))}}}";

        public static readonly TupleSymbol Empty = new TupleSymbol(null);
    }
}