using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    public abstract class Scope
    {
        /// <summary>
        /// Gets all symbols in the scope with the specified name and kind.
        /// </summary>
        public abstract void GetSymbols(string name, SymbolMatch match, List<Symbol> symbols);

        /// <summary>
        /// Gets all the symbols in the scope with the specified kind.
        /// </summary>
        public void GetSymbols(SymbolMatch match, List<Symbol> symbols)
        {
            this.GetSymbols(null, match, symbols);
        }
    }
}