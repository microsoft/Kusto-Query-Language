using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    [System.Diagnostics.DebuggerDisplay("Symbol: {Kind} {Display}")]
    public abstract class Symbol
    {
        /// <summary>
        /// The name of the symbol.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The <see cref="SymbolKind"/> of the symbol.
        /// </summary>
        public virtual SymbolKind Kind => SymbolKind.None;

        protected Symbol(string name)
        {
            this.Name = name ?? "";
        }

        /// <summary>
        /// If true, the symbol is hidden from Intellisense.
        /// </summary>
        public virtual bool IsHidden => this.Name.StartsWith("__"); // symbols that start with __ are internal only.

        /// <summary>
        /// True if the symbol is an error symbol.
        /// </summary>
        public virtual bool IsError => false;

        /// <summary>
        /// Identifies whether the symbol is scalar or tabular.
        /// </summary>
        public virtual Tabularity Tabularity => Tabularity.Unknown;

        /// <summary>
        /// True if the symbol is scalar or unknown.
        /// </summary>
        public bool IsScalar
        {
            get
            {
                switch (this.Tabularity)
                {
                    case Tabularity.Scalar:
                    case Tabularity.Unknown:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// True if the symbol is tabular or unknown.
        /// </summary>
        public bool IsTabular
        {
            get
            {
                switch (this.Tabularity)
                {
                    case Tabularity.Tabular:
                    case Tabularity.Unknown:
                        return true;
                    default:
                        return false;
                }
            }
        }

        private string _display;

        protected virtual string GetDisplay() => this.Name;

        /// <summary>
        /// A description of the symbol.
        /// </summary>
        public string Display
        {
            get
            {
                if (this._display == null)
                {
                    this._display = this.GetDisplay();
                }

                return this._display;
            }
        }

        /// <summary>
        /// All the symbols contained by this symbol.
        /// </summary>
        public virtual IReadOnlyList<Symbol> Members => 
            EmptyReadOnlyList<Symbol>.Instance;

        /// <summary>
        /// Gets all the matching members.
        /// </summary>
        public virtual void GetMembers(string name, SymbolMatch match, List<Symbol> symbols, bool ignoreCase = false)
        {
            foreach (var symbol in this.Members)
            {
                if (symbol.Matches(name, match, ignoreCase))
                {
                    symbols.Add(symbol);
                }
            }
        }

        /// <summary>
        /// Gets all the matching members.
        /// </summary>
        public void GetMembers(SymbolMatch match, List<Symbol> symbols, bool ignoreCase = false)
        {
            this.GetMembers(null, match, symbols, ignoreCase);
        }

        /// <summary>
        /// Returns the first member that matches or null.
        /// </summary>
        public Symbol GetFirstMember(string name, SymbolMatch match = SymbolMatch.Any, bool ignoreCase = false)
        {
            foreach (var symbol in this.Members)
            {
                if (symbol.Matches(name, match, ignoreCase))
                {
                    return symbol;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines the result type of an expression that references the specified symbol
        /// </summary>
        public static TypeSymbol GetExpressionResultType(Symbol symbol)
        {
            switch (symbol)
            {
                case ColumnSymbol c:
                    return c.Type;

                case VariableSymbol v:
                    return v.Type;

                case ParameterSymbol p:
                    return p.Type;

                case GroupSymbol g:
                    var resultSymbols = new List<Symbol>();

                    foreach (var m in g.Members)
                    {
                        var rs = GetExpressionResultType(m);
                        if (rs != null)
                        {
                            resultSymbols.Add(rs);
                        }
                    }

                    if (resultSymbols.Count == 1)
                    {
                        return resultSymbols[0] as TypeSymbol;
                    }
                    else if (resultSymbols.Count > 1)
                    {
                        return new GroupSymbol(resultSymbols);
                    }
                    else
                    {
                        return null;
                    }

                case TypeSymbol t:
                    return t;

                default:
                    return null;
            }
        }
    }
}