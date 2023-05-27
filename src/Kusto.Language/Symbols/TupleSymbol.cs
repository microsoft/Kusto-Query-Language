using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Syntax;
    using Utils;

    /// <summary>
    /// A symbol for a tuple of one or more name/value pairs.
    /// </summary>
    public sealed class TupleSymbol : ScalarSymbol
    {
        public IReadOnlyList<ColumnSymbol> Columns { get; }

        public override IReadOnlyList<Symbol> Members => this.Columns;

        public override SymbolKind Kind => SymbolKind.Tuple;

        public TableSymbol RelatedTable { get; }

        public TupleSymbol(IEnumerable<ColumnSymbol> columns, TableSymbol relatedTable = null)
            : base("tuple")
        {
            this.Columns = columns.ToReadOnly().CheckArgumentNullOrElementNull(nameof(columns));
            this.RelatedTable = relatedTable;
        }

        public TupleSymbol(params ColumnSymbol[] columns)
            : this((IEnumerable<ColumnSymbol>)columns)
        {
        }

        /// <summary>
        /// Create a <see cref="TupleSymbol"/> instance from a schema description: (col: type, ...)
        /// </summary>
        public static new TupleSymbol From(string schema) =>
            ScalarTypes.GetTuple(schema);

        /// <summary>
        /// If true, then a single column tuple can be reduced to the scalar value of that column.
        /// </summary>
        public bool IsReducibleToScalar => this.Columns.Count == 1 && this.RelatedTable == null;

        /// <summary>
        /// Returns a new <see cref="TupleSymbol"/> instance with the specified columns. 
        /// </summary>
        public TupleSymbol WithColumns(IEnumerable<ColumnSymbol> columns)
        {
            return new TupleSymbol(columns, this.RelatedTable);
        }

        /// <summary>
        /// Returns a new <see cref="TupleSymbol"/> instance with the columns updated to have the specified source. 
        /// </summary>
        public TupleSymbol WithSource(SyntaxNode source)
        {
            return this.WithColumns(this.Columns.Select(c => c.WithSource(source)));
        }

        public static readonly TupleSymbol Empty = new TupleSymbol(null);
    }
}