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

        /// <summary>
        /// The description of the table.
        /// </summary>
        public string Description { get; }

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

        private TableSymbol(string name, TableState state, IEnumerable<ColumnSymbol> columns, string description)
            : base(name)
        {
            this.state = state;
            this.Columns = columns.ToReadOnly();
            this.Description = description ?? "";
        }

        private TableSymbol(TableState state, IEnumerable<ColumnSymbol> columns, string description)
            : this("", state, columns, description)
        {
        }

        public TableSymbol(string name, IEnumerable<ColumnSymbol> columns, string description = null)
            : this(name, TableState.None, columns, description)
        {
        }

        public TableSymbol(string name, string schema, string description = null)
            : this(name, TableSymbol.From(schema).Columns, description)
        {
        }

        public TableSymbol(string name, params ColumnSymbol[] columns)
            : this(name, TableState.None, columns, null)
        {
        }

        public TableSymbol(IEnumerable<ColumnSymbol> columns)
            : this("", TableState.None, columns, null)
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

            var schemaType = QueryParser.ParseSchemaType(schema);
            if (schemaType == null)
            {
                throw new InvalidOperationException($"Invalid schema: {schema}");
            }

            return (TableSymbol)Binding.Binder.GetDeclaredType(schemaType);
        }

        public override SymbolKind Kind => IsMaterializedView ? SymbolKind.MaterializedView : SymbolKind.Table;

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
        /// Returns a version of this <see cref="TableSymbol"/> with the specified name.
        /// </summary>
        public TableSymbol WithName(string name)
        {
            if (this.Name != (name ?? ""))
            {
                return new TableSymbol(name, this.state, this.Columns, this.Description);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified description.
        /// </summary>
        public TableSymbol WithDescripton(string description)
        {
            if (this.Description != (description ?? ""))
            {
                return new TableSymbol(this.Name, this.state, this.Columns, this.Description);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified columns.
        /// </summary>
        public TableSymbol WithColumns(IEnumerable<ColumnSymbol> columns)
        {
            if (this.Columns != columns)
            {
                return new TableSymbol(this.Name, this.state, columns, this.Description);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified columns.
        /// </summary>
        public TableSymbol WithColumns(params ColumnSymbol[] columns)
        {
            return WithColumns((IEnumerable<ColumnSymbol>)columns);
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with additional columns.
        /// </summary>
        public TableSymbol AddColumns(IEnumerable<ColumnSymbol> columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            return new TableSymbol(this.Name, this.state, this.Columns.Concat(columns), this.Description);
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with additional columns.
        /// </summary>
        public TableSymbol AddColumns(params ColumnSymbol[] columns)
        {
            return AddColumns((IEnumerable<ColumnSymbol>)columns);
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified state.
        /// </summary>
        private TableSymbol WithState(TableState newState)
        {
            if (this.state != newState)
            {
                return new TableSymbol(this.Name, newState, this.Columns, this.Description);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified <see cref="IsSerialized"/> property.
        /// </summary>
        public TableSymbol WithIsSerialized(bool isSerialized)
        {
            return WithState(isSerialized ? (this.state | TableState.Serialized) : (this.state & ~TableState.Serialized));
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified <see cref="IsSorted"/> property.
        /// </summary>
        public TableSymbol WithIsSorted(bool isSorted)
        {
            return WithState(isSorted ? (this.state | TableState.Sorted) : (this.state & ~TableState.Sorted));
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified <see cref="IsOpen"/> property.
        /// </summary>
        public TableSymbol WithIsOpen(bool isOpen)
        {
            return WithState(isOpen ? (this.state | TableState.Open) : (this.state & ~TableState.Open));
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified <see cref="IsExternal"/> property.
        /// </summary>
        public TableSymbol WithIsExternal(bool isExternal)
        {
            return WithState(isExternal ? (this.state | TableState.External) : (this.state & ~TableState.External));
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified <see cref="IsMaterializedView"/> property.
        /// </summary>
        public TableSymbol WithIsMaterializedView(bool isMaterializedView)
        {
            return WithState(isMaterializedView ? (this.state | TableState.MaterializedView) : (this.state & ~TableState.MaterializedView));
        }

        /// <summary>
        /// The state flags that are inheritable via <see cref="WithInheritableProperties(TableSymbol)"/>
        /// </summary>
        private static readonly TableState InheritableState =
            TableState.Serialized | TableState.Sorted | TableState.Open;

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the same inheritable state properties as the specified table;
        /// IsSerialized, IsSorted, and IsOpen.
        /// </summary>
        public TableSymbol WithInheritableProperties(TableSymbol table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            var newState = (table.state & ~InheritableState) | (table.state & InheritableState);
            return WithState(newState);
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