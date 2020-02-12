using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;
    using Parsing;

    /// <summary>
    /// A symbol representing a table
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Symbol: {Kind} {DebugDisplay}")]
    public sealed class TableSymbol : TypeSymbol
    {
        /// <summary>
        /// The columns of the table.
        /// </summary>
        public IReadOnlyList<ColumnSymbol> Columns { get; }

        [Flags]
        private enum TableState : ushort
        {
            None =              0b0000_0000,
            Serialized =        0b0000_0001,
            Sorted =            0b0000_0010,
            Open =              0b0000_0100,
            External =          0b0000_1000,
            MaterializedView =  0b0001_0000
        }

        /// <summary>
        /// The state of the table as bit flags
        /// </summary>
        private readonly TableState state;

        private TableSymbol(string name, TableState state, IEnumerable<ColumnSymbol> columns)
            : base(name)
        {
            this.state = state;
            this.Columns = columns.ToReadOnly();
        }

        private TableSymbol(TableState state, IEnumerable<ColumnSymbol> columns)
            : this("", state, columns)
        {
        }

        public TableSymbol(string name, IEnumerable<ColumnSymbol> columns)
            : this(name, TableState.None, columns)
        {
        }

        public TableSymbol(string name, params ColumnSymbol[] columns)
            : this(name, TableState.None, columns)
        {
        }

        public TableSymbol(IEnumerable<ColumnSymbol> columns)
            : this("", TableState.None, columns)
        {
        }

        public TableSymbol(params ColumnSymbol[] columns)
            : this((IEnumerable<ColumnSymbol>)columns)
        {
        }

        /// <summary>
        /// Gets a <see cref="TableSymbol"/> for the schema: (name:type, ...)
        /// </summary>
        public static TableSymbol From(string schema)
        {
            if (schema == null)
            {
                throw new ArgumentNullException(nameof(schema));
            }

            // Use null for GlobalState to avoid cycle in definitions.
            var parser = QueryGrammar.From(null).SchemaType;

            var schemaType = parser.ParseFirst(schema);
            if (schemaType == null)
            {
                throw new InvalidOperationException($"Invalid schema: {schema}");
            }

            return (TableSymbol)Binding.Binder.GetDeclaredType(schemaType);
        }

        public override SymbolKind Kind => SymbolKind.Table;

        public override IReadOnlyList<Symbol> Members => this.Columns;

        public override Tabularity Tabularity => Tabularity.Tabular;

        /// <summary>
        /// True if the table is sorted.
        /// </summary>
        public bool IsSorted => (this.state & TableState.Sorted) != 0;

        /// <summary>
        /// True if the table is serialized.
        /// </summary>
        public bool IsSerialized => (this.state & TableState.Serialized) != 0;

        /// <summary>
        /// True if the table is open.
        /// </summary>
        public bool IsOpen => (this.state & TableState.Open) != 0;

        /// <summary>
        /// True if the table is external.
        /// </summary>
        public bool IsExternal => (this.state & TableState.External) != 0;

        /// <summary>
        /// True if the table is a materialized view.
        /// </summary>
        public bool IsMaterializedView => (this.state & TableState.MaterializedView) != 0;

        /// <summary>
        /// Creates a new <see cref="TableSymbol"/> with the specified name.
        /// </summary>
        public TableSymbol WithName(string name)
        {
            return new TableSymbol(name ?? "", this.state, this.Columns);
        }

        /// <summary>
        /// Creates a new <see cref="TableSymbol"/> with the specified columns.
        /// </summary>
        public TableSymbol WithColumns(IEnumerable<ColumnSymbol> columns)
        {
            return new TableSymbol(this.state, columns);
        }

        /// <summary>
        /// Creates a new <see cref="TableSymbol"/> with the specified columns.
        /// </summary>
        public TableSymbol WithColumns(params ColumnSymbol[] columns)
        {
            return WithColumns((IEnumerable<ColumnSymbol>)columns);
        }

        /// <summary>
        /// Creates a new <see cref="TableSymbol"/> with additional columns.
        /// </summary>
        public TableSymbol AddColumns(IEnumerable<ColumnSymbol> columns)
        {
            return new TableSymbol(this.state, this.Columns.Concat(columns));
        }

        /// <summary>
        /// Creates a new <see cref="TableSymbol"/> with additional columns.
        /// </summary>
        public TableSymbol AddColumns(params ColumnSymbol[] columns)
        {
            return AddColumns((IEnumerable<ColumnSymbol>)columns);
        }

        /// <summary>
        /// Creates a new <see cref="TableSymbol"/> that has the serialized state.
        /// </summary>
        public TableSymbol Serialized()
        {
            return new TableSymbol(this.state | TableState.Serialized, this.Columns);
        }

        /// <summary>
        /// Creates a new <see cref="TableSymbol"/> that has the sorted state.
        /// </summary>
        public TableSymbol Sorted()
        {
            return new TableSymbol(this.state | TableState.Sorted, this.Columns);
        }

        /// <summary>
        /// Creates a new <see cref="TableSymbol"/> that does not have the sorted state.
        /// </summary>
        public TableSymbol Unsorted()
        {
            return new TableSymbol(this.state & ~TableState.Sorted, this.Columns);
        }

        /// <summary>
        /// Creates a new <see cref="TableSymbol"/> that contains an explicit list of columns plus any unspecified column referenced by a query.
        /// </summary>
        public TableSymbol Open()
        {
            return new TableSymbol(this.state | TableState.Open, this.Columns);
        }

        /// <summary>
        /// Creates a new <see cref="TableSymbol"/> that is considered external.
        /// </summary>
        public TableSymbol External()
        {
            return new TableSymbol(this.Name, this.state | TableState.External, this.Columns);
        }

        /// <summary>
        /// Creates a new <see cref="TableSymbol"/> that is considered a materialized view.
        /// </summary>
        public TableSymbol MaterializedView()
        {
            return new TableSymbol(this.Name, this.state | TableState.MaterializedView, this.Columns);
        }

        private Dictionary<string, ColumnSymbol> lazyColumnMap;

        private Dictionary<string, ColumnSymbol> ColumnMap
        {
            get
            {
                if (this.lazyColumnMap == null)
                {
                    var map = new Dictionary<string, ColumnSymbol>(this.Columns.Count);
                    
                    foreach (var col in this.Columns)
                    {
                        // do not add duplicate columns to dictionary
                        // having duplicate columns is an error that should be caught elsewhere (via diagnostic)
                        if (!map.ContainsKey(col.Name))
                        {
                            map.Add(col.Name, col);
                        }
                    }

                    Interlocked.CompareExchange(ref this.lazyColumnMap, map, null);
                }

                return this.lazyColumnMap;
            }
        }

        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        public ColumnSymbol GetColumn(string name)
        {
            if (TryGetColumn(name, out var column))
            {
                return column;
            }
            else
            {
                throw new InvalidOperationException($"The column '{name}' does not exist.");
            }
        }

        /// <summary>
        /// Gets the <see cref="ColumnSymbol"/> with the specified name.
        /// Returns true if the column is found, or false if there is no column with the specified name.
        /// </summary>
        public bool TryGetColumn(string name, out ColumnSymbol column)
        {
            return this.ColumnMap.TryGetValue(name, out column);
        }

        public override void GetMembers(string name, SymbolMatch match, List<Symbol> symbols, bool ignoreCase = false)
        {
            if (this.Columns.Count > 0)
            {
                if (name != null)
                {
                    if (this.ColumnMap.TryGetValue(name, out var column) && column.Matches(name, match, ignoreCase))
                    {
                        symbols.Add(column);
                    }
                }
                else
                {
                    base.GetMembers(name, match, symbols);
                }
            }
        }

        protected override string GetDisplay() =>
            $"({string.Join(", ", this.Members.Select(m => m.Display))})";

        private string DebugDisplay => this.Name + this.Display;

        /// <summary>
        /// An empty table.
        /// </summary>
        public static readonly TableSymbol Empty = new TableSymbol();

        /// <summary>
        /// Combine the columns of multiple tables into a new table.
        /// </summary>
        public static TableSymbol Combine(CombineKind kind, IEnumerable<TableSymbol> tables)
        {
            return new TableSymbol(ColumnSymbol.Combine(kind, tables.Select(t => t.Columns)));
        }

        /// <summary>
        /// Combine the columns of multiple tables into a new table.
        /// </summary>
        public static TableSymbol Combine(CombineKind kind, params TableSymbol[] tables)
        {
            return Combine(kind, (IEnumerable<TableSymbol>)tables);
        }
    }
}