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
    public class TableSymbol : TypeSymbol
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
        protected enum TableState : ushort
        {
            None =              0b0000_0000,
            Serialized =        0b0000_0001,
            Sorted =            0b0000_0010,
            Open =              0b0000_0100,
        }

        /// <summary>
        /// The state of the table as bit flags
        /// </summary>
        private readonly TableState state;

        protected TableSymbol(string name, TableState state, IEnumerable<ColumnSymbol> columns, string description)
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

        internal TableSymbol(TableSymbol sourceTable)
            : this(sourceTable.Name, sourceTable.state, sourceTable.Columns, sourceTable.Description)
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
        public bool IsExternal => this is ExternalTableSymbol;

        /// <summary>
        /// True if the table is a materialized view.
        /// </summary>
        public bool IsMaterializedView => this is MaterializedViewSymbol;

        /// <summary>
        /// Construct a new <see cref="TableSymbol"/> if one of the optional arguments is different that the current values.
        /// </summary>
        protected TableSymbol With(
            string name = null,
            TableState? state = null,
            IEnumerable<ColumnSymbol> columns = null,
            string description = null)
        {
            var useName = name ?? this.Name;
            var useState = state.HasValue ? state.Value : this.state;
            var useColumns = columns ?? this.Columns;
            var useDescription = description ?? this.Description;

            if (useName != this.Name
                || useState != this.state
                || useColumns != this.Columns
                || useDescription != this.Description)
            {
                return Create(useName, useState, useColumns, useDescription);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Constructs a new <see cref="TableSymbol"/> given the specified values.
        /// </summary>
        protected virtual TableSymbol Create(string name, TableState state, IEnumerable<ColumnSymbol> columns, string description)
        {
            return new TableSymbol(name, state, columns, description);
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified name.
        /// </summary>
        public TableSymbol WithName(string name)
        {
            return With(name: name);
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified description.
        /// </summary>
        public TableSymbol WithDescripton(string description)
        {
            return With(description: description ?? "");
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified columns.
        /// </summary>
        public TableSymbol WithColumns(IEnumerable<ColumnSymbol> columns)
        {
            return With(columns: columns);
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

            return With(columns: this.Columns.Concat(columns));
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
            return With(state: newState);
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
            if (this is ExternalTableSymbol == isExternal)
            {
                return this;
            }
            else if (isExternal)
            {
                return new ExternalTableSymbol(this);
            }
            else
            {
                return new TableSymbol(this);
            }
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified <see cref="IsMaterializedView"/> property.
        /// </summary>
        public TableSymbol WithIsMaterializedView(bool isMaterializedView)
        {
            if (this is MaterializedViewSymbol == isMaterializedView)
            {
                return this;
            }
            else if (isMaterializedView)
            {
                return new MaterializedViewSymbol(this);
            }
            else
            {
                return new TableSymbol(this);
            }
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

    public enum MaterializedViewKind
    {
        /// <summary>
        /// View symbol has not been analyzed yet
        /// </summary>
        Unknown, 
        /// <summary>
        /// View is a downsampling type
        /// </summary>
        Downsampling, 
        /// <summary>
        /// View was analyzed and is not downsampling
        /// </summary>
        Other
    }

    public class MaterializedViewSymbol : TableSymbol
    {
        /// <summary>
        /// The query that that is the source of the materialized view.
        /// </summary>
        public string MaterializedViewQuery { get; private set; }

        // TODO: find better solution for storing this data for analyzer
        internal MaterializedViewKind MaterializedViewKind { get; set; }

        private MaterializedViewSymbol(string name, TableState state, IEnumerable<ColumnSymbol> columns, string description, string query)
            : base(name, state, columns, description)
        {
            this.MaterializedViewQuery = query;
        }

        internal MaterializedViewSymbol(TableSymbol sourceTable, string query = null)
            : base(sourceTable)
        {
            this.MaterializedViewQuery = query;
        }

        public MaterializedViewSymbol(string name, IEnumerable<ColumnSymbol> columns, string query, string description = null)
            : this(name, TableState.None, columns, description, query)
        {
        }

        public MaterializedViewSymbol(string name, string columns, string query, string description = null)
            : this(name, TableSymbol.From(columns).Columns, query, description)
        {
        }

        protected override TableSymbol Create(string name, TableState state, IEnumerable<ColumnSymbol> columns, string description)
        {
            return new MaterializedViewSymbol(name, state, columns, description, this.MaterializedViewQuery);
        }
    }

    /// <summary>
    /// A table declared external to Kusto
    /// </summary>
    public class ExternalTableSymbol : TableSymbol
    {
        private ExternalTableSymbol(string name, TableState state, IEnumerable<ColumnSymbol> columns, string description)
            : base(name, state, columns, description)
        {
        }

        internal ExternalTableSymbol(TableSymbol sourceTable)
            : base(sourceTable)
        {
        }

        public ExternalTableSymbol(string name, IEnumerable<ColumnSymbol> columns, string description = null)
            : this(name, TableState.None, columns, description)
        {
        }

        public ExternalTableSymbol(string name, params ColumnSymbol[] columns)
            : this(name, (IEnumerable<ColumnSymbol>)columns)
        {
        }

        public ExternalTableSymbol(string name, string columns, string description = null)
            : this(name, TableSymbol.From(columns).Columns, description)
        {
        }

        protected override TableSymbol Create(string name, TableState state, IEnumerable<ColumnSymbol> columns, string description)
        {
            return new ExternalTableSymbol(name, state, columns, description);
        }
    }
}