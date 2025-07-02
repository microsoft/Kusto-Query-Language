using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Parsing;
    using Syntax;
    using Utils;

    /// <summary>
    /// A symbol representing a table
    /// </summary>
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
        private readonly TableState _state;

        protected TableSymbol(string name, TableState state, IEnumerable<ColumnSymbol> columns, string description)
            : base(name)
        {
            _state = state;
            this.Columns = columns.ToReadOnly().CheckArgumentNullOrElementNull(nameof(columns));
            this.Description = description ?? "";
        }

        private TableSymbol(TableState state, IEnumerable<ColumnSymbol> columns, string description)
            : this("", state, columns, description)
        {
        }

        internal TableSymbol(TableSymbol sourceTable)
            : this(sourceTable.Name, sourceTable._state, sourceTable.Columns, sourceTable.Description)
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

            schema = schema.Trim();

            if (schema.Length > 0 && schema[0] != '(')
                schema = "(" + schema;

            if (schema.Length > 0 && schema[schema.Length - 1] != ')')
                schema = schema + ")";

            var rowSchema = QueryParser.ParseRowSchema(schema);
            if (rowSchema == null)
            {
                throw new InvalidOperationException($"Invalid schema: {schema}");
            }

            var columns = new List<ColumnSymbol>();
            Binding.Binder.CreateColumnsFromRowSchema(rowSchema.Columns, columns);
            return new TableSymbol(columns);
        }

        public override SymbolKind Kind => SymbolKind.Table;

        public override IReadOnlyList<Symbol> Members => this.Columns;

        public override Tabularity Tabularity => Tabularity.Tabular;

        /// <summary>
        /// True if the table is sorted.
        /// </summary>
        public bool IsSorted => (_state & TableState.Sorted) != 0;

        /// <summary>
        /// True if the table is serialized.
        /// </summary>
        public bool IsSerialized => (_state & TableState.Serialized) != 0;

        /// <summary>
        /// True if the table is open.
        /// </summary>
        public bool IsOpen => (_state & TableState.Open) != 0;

        /// <summary>
        /// True if the table is external.
        /// </summary>
        public bool IsExternal => this is ExternalTableSymbol;

        /// <summary>
        /// True if the table is a materialized view.
        /// </summary>
        public bool IsMaterializedView => this is MaterializedViewSymbol;

        /// <summary>
        /// True if the table is a stored query result.
        /// </summary>
        public bool IsStoredQueryResult => this is StoredQueryResultSymbol;

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
            var useState = state.HasValue ? state.Value : _state;
            var useColumns = columns ?? this.Columns;
            var useDescription = description ?? this.Description;

            if (useName != this.Name
                || useState != _state
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
            return WithState(isSerialized ? (_state | TableState.Serialized) : (_state & ~TableState.Serialized));
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified <see cref="IsSorted"/> property.
        /// </summary>
        public TableSymbol WithIsSorted(bool isSorted)
        {
            return WithState(isSorted ? (_state | TableState.Sorted) : (_state & ~TableState.Sorted));
        }

        /// <summary>
        /// Returns a version of this <see cref="TableSymbol"/> with the specified <see cref="IsOpen"/> property.
        /// </summary>
        public TableSymbol WithIsOpen(bool isOpen)
        {
            return WithState(isOpen ? (_state | TableState.Open) : (_state & ~TableState.Open));
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

            var newState = (_state & ~InheritableState) | (table._state & InheritableState);
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

        /// <summary>
        /// Gets the <see cref="ColumnSymbol"/>s with names that match the pattern.
        /// </summary>
        public void GetMatchingColumns(string pattern, List<ColumnSymbol> columns)
        {
            if (!pattern.Contains("*"))
            {
                if (TryGetColumn(pattern, out var column))
                {
                    columns.Add(column);
                }
            }
            else
            {
                foreach (var col in this.Columns)
                {
                    if (KustoFacts.Matches(pattern, col.Name))
                        columns.Add(col);
                }
            }
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

        /// <summary>
        /// Returns a new <see cref="TableSymbol"/> instance with all columns
        /// modified to reference the specified source.
        /// </summary>
        public TableSymbol WithSource(SyntaxNode source)
        {
            return this.WithColumns(this.Columns.Select(p => p.WithSource(source)));
        }

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

        /// <summary>
        /// Returns true if the two tables have the same name, columns and properties.
        /// </summary>
        public static bool AreEquivalent(TableSymbol x, TableSymbol y)
        {
            if (x == y)
                return true;
            if (x.GetType() != y.GetType())
                return false;
            if (x.Name != y.Name
                || x.AlternateName != y.AlternateName
                || x.Description != y.Description)
                return false;
            return AreResultEquivalent(x, y);
        }

        /// <summary>
        /// Returns true if the two tables have the same columns and properties,
        /// such that they would be considered the logically equivalent result type.
        /// </summary>
        public static bool AreResultEquivalent(TableSymbol x, TableSymbol y)
        {
            if (x == y)
                return true;
            if (x._state != y._state)
                return false;
            return AreColumnsEquivalent(x, y);
        }

        /// <summary>
        /// Returns true if the two tables have the same column names and types.
        /// </summary>
        public static bool AreColumnsEquivalent(TableSymbol x, TableSymbol y)
        {
            if (x == y)
                return true;
            if (x.Columns.Count != y.Columns.Count)
                return false;
            for (int i = 0; i < x.Columns.Count; i++)
            {
                var xc = x.Columns[i];
                var yc = y.Columns[i];
                if (xc.Name != yc.Name
                    || xc.Type != yc.Type)
                    return false;
            }
            return true;
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

        public override SymbolKind Kind => SymbolKind.MaterializedView;
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

    public sealed class StoredQueryResultSymbol : TableSymbol
    {
        public StoredQueryResultSymbol(string name)
            : base(name, EmptyReadOnlyList<ColumnSymbol>.Instance)
        {
        }

        public StoredQueryResultSymbol(string name, IEnumerable<ColumnSymbol> columns)
            : base(name, columns)
        {
        }

        public override SymbolKind Kind => SymbolKind.StoredQueryResult;
    }
}