using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Binding
{
    using Symbols;

    /// <summary>
    /// Models nested scoping for let variables and function parameters.
    /// </summary>
    internal class LocalScope
    {
        private readonly LocalScope _outerScope;
        private readonly Dictionary<string, Symbol> _declaredSymbols;

        public LocalScope(LocalScope outerScope = null)
        {
            _outerScope = outerScope;
            _declaredSymbols = new Dictionary<string, Symbol>();
        }

        public bool AddDeclaration(Symbol symbol)
        {
            if (symbol != null && !_declaredSymbols.ContainsKey(symbol.Name))
            {
                _declaredSymbols.Add(symbol.Name, symbol);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets all the matching symbols in the scope.
        /// </summary>
        public void GetSymbols(string name, SymbolMatch match, List<Symbol> symbols)
        {
            Symbol decl;
            if (name != null)
            {
                if (_declaredSymbols.TryGetValue(name, out decl) && decl.Matches(name, match))
                {
                    symbols.Add(decl);
                    return;
                }
            }
            else
            {
                foreach (var symbol in _declaredSymbols.Values)
                {
                    if (symbol.Matches(name, match))
                    {
                        symbols.Add(symbol);
                    }
                }
            }

            if (_outerScope != null)
            {
                _outerScope.GetSymbols(name, match, symbols);
            }
        }

        /// <summary>
        /// Gets all the matching symbols in the scope.
        /// </summary>
        public void GetSymbols(SymbolMatch match, List<Symbol> symbols)
        {
            GetSymbols(null, match, symbols);
        }
    }
}