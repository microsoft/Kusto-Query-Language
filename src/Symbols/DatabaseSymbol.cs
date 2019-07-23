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

        public override IReadOnlyList<Symbol> Members => this.members;

        public override SymbolKind Kind => SymbolKind.Database;

        /// <summary>
        /// If true, then the definition of the database is not fully known.
        /// </summary>
        public bool IsOpen { get; }

        // caches
        private IReadOnlyList<TableSymbol> tables;
        private IReadOnlyList<FunctionSymbol> functions;
        private HashSet<Symbol> symbolSet;

        public DatabaseSymbol(string name, IEnumerable<Symbol> members, bool isOpen = false)
            : base(name)
        {
            this.members = members.ToReadOnly();
            this.IsOpen = isOpen;
        }

        public DatabaseSymbol(string name, params Symbol[] members)
            : this(name, (IEnumerable<Symbol>)members)
        {
        }

        public override Tabularity Tabularity => Tabularity.Tabular;


        /// <summary>
        /// The tables in the database.
        /// </summary>
        public IReadOnlyList<TableSymbol> Tables
        {
            get
            {
                if (this.tables == null)
                {
                    this.tables = this.Members.OfType<TableSymbol>().ToReadOnly();
                }

                return this.tables;
            }
        }

        /// <summary>
        /// The functions in the database.
        /// </summary>
        private IReadOnlyList<FunctionSymbol> Functions
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
        /// Gets the symbol with the specified name or returns null.
        /// </summary>
        public Symbol GetSymbol(string name)
        {
            return this.members.FirstOrDefault(m => m.Name == name);
        }

        protected override string GetDisplay() =>
            $"database({this.Name})";

        /// <summary>
        /// Creates a new database that includes the additional symbols (tables and functions).
        /// </summary>
        public DatabaseSymbol AddSymbols(IEnumerable<Symbol> symbols)
        {
            return new DatabaseSymbol(this.Name, this.Members.Concat(symbols), this.IsOpen);
        }

        /// <summary>
        /// Creates a new database that includes the additional symbols (tables and functions).
        /// </summary>
        public DatabaseSymbol AddSymbols(params Symbol[] symbols)
        {
            return AddSymbols((IEnumerable<Symbol>)symbols);
        }

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
    }
}