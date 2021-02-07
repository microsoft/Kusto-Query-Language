using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Syntax;
    using Utils;

    /// <summary>
    /// A symbol representing a database.
    /// </summary>
    public sealed class DatabaseSymbol : TypeSymbol
    {
        private readonly IReadOnlyList<Symbol> members;

        /// <summary>
        /// If true, then the definition of the database is not fully known.
        /// </summary>
        public bool IsOpen { get; }

        // caches
        private IReadOnlyList<TableSymbol> tables;
        private IReadOnlyList<TableSymbol> externalTables;
        private IReadOnlyList<TableSymbol> materializedViews;
        private IReadOnlyList<FunctionSymbol> functions;
        private HashSet<Symbol> symbolSet;

        /// <summary>
        /// Creates a new instance of a <see cref="DatabaseSymbol"/>.
        /// </summary>
        public DatabaseSymbol(string name, IEnumerable<Symbol> members, bool isOpen = false)
            : base(name)
        {
            this.members = members.ToReadOnly();
            this.IsOpen = isOpen;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="DatabaseSymbol"/>.
        /// </summary>
        public DatabaseSymbol(string name, params Symbol[] members)
            : this(name, (IEnumerable<Symbol>)members)
        {
        }

        public override SymbolKind Kind => SymbolKind.Database;

        public override Tabularity Tabularity => Tabularity.Tabular;

        /// <summary>
        /// All the symbols contained by this symbol.
        /// </summary>
        public override IReadOnlyList<Symbol> Members => this.members;

        /// <summary>
        /// The tables contained by the database.
        /// </summary>
        public IReadOnlyList<TableSymbol> Tables
        {
            get
            {
                if (this.tables == null)
                {
                    this.tables = this.Members.OfType<TableSymbol>().Where(ts => !ts.IsExternal && !ts.IsMaterializedView).ToReadOnly();
                }

                return this.tables;
            }
        }

        /// <summary>
        /// The external tables accessible from the database.
        /// </summary>
        public IReadOnlyList<TableSymbol> ExternalTables
        {
            get
            {
                if (this.externalTables == null)
                {
                    this.externalTables = this.Members.OfType<TableSymbol>().Where(ts => ts.IsExternal).ToReadOnly();
                }

                return this.externalTables;
            }
        }

        /// <summary>
        /// The materialized views accessible from the database.
        /// </summary>
        public IReadOnlyList<TableSymbol> MaterializedViews
        {
            get
            {
                if (this.materializedViews == null)
                {
                    this.materializedViews = this.Members.OfType<TableSymbol>().Where(ts => ts.IsMaterializedView).ToReadOnly();
                }

                return this.materializedViews;
            }
        }

        /// <summary>
        /// The functions contained by the database.
        /// </summary>
        public IReadOnlyList<FunctionSymbol> Functions
        {
            get
            {
                if (this.functions == null)
                {
                    this.functions = this.Members.OfType<FunctionSymbol>().ToReadOnly();
                }

                return this.functions;
            }
        }

        /// <summary>
        /// Gets the member with the specified name or returns null.
        /// </summary>
        public Symbol GetMember(string name)
        {
            return this.members.FirstOrDefault(m => m.Name == name);
        }

        /// <summary>
        /// Gets the table with the specified name or returns null.
        /// </summary>
        public TableSymbol GetTable(string name)
        {
            return this.Tables.FirstOrDefault(t => t.Name == name);
        }

        /// <summary>
        /// Gets the table, external table or materialized view with the specified name.
        /// </summary>
        public TableSymbol GetAnyTable(string name)
        {
            return GetTable(name) ?? GetExternalTable(name) ?? GetMaterializedView(name);
        }

        /// <summary>
        /// Gets the external table with the specified name or returns null.
        /// </summary>
        public TableSymbol GetExternalTable(string name)
        {
            return this.ExternalTables.FirstOrDefault(t => t.Name == name);
        }

        /// <summary>
        /// Gets the materialized view with the specified name or returns null.
        /// </summary>
        public TableSymbol GetMaterializedView(string name)
        {
            return this.MaterializedViews.FirstOrDefault(t => t.Name == name);
        }

        /// <summary>
        /// Gets the function with the specified name or returns null.
        /// </summary>
        public FunctionSymbol GetFunction(string name)
        {
            return this.Functions.FirstOrDefault(f => f.Name == name);
        }

        public DatabaseSymbol WithMembers(IEnumerable<Symbol> members)
        {
            return new DatabaseSymbol(this.Name, members, this.IsOpen);
        }

        /// <summary>
        /// Creates a new database that includes the additional symbols (tables and functions).
        /// </summary>
        public DatabaseSymbol AddMembers(IEnumerable<Symbol> symbols)
        {
            return new DatabaseSymbol(this.Name, this.Members.Concat(symbols), this.IsOpen);
        }

        /// <summary>
        /// Creates a new database that includes the additional symbols (tables and functions).
        /// </summary>
        public DatabaseSymbol AddMembers(params Symbol[] symbols)
        {
            return AddMembers((IEnumerable<Symbol>)symbols);
        }

        /// <summary>
        /// Returns true if the symbol is contained by the database.
        /// </summary>
        public bool Contains(Symbol symbol)
        {
            if (this.symbolSet == null)
            {
                this.symbolSet = new HashSet<Symbol>();

                foreach (var member in this.Members)
                {
                    this.symbolSet.Add(symbol);
                }
            }

            return this.symbolSet.Contains(symbol);
        }

        protected override string GetDisplay() =>
            $"database({this.Name})";

        public static readonly DatabaseSymbol Unknown = new DatabaseSymbol(null, members: null, isOpen: true);
    }
}