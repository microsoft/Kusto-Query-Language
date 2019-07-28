using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    /// <summary>
    /// A symbol representing an unknown type due to a semantic error.
    /// </summary>
    public sealed class ErrorSymbol : TypeSymbol
    {
        public override SymbolKind Kind => SymbolKind.Error;

        private ErrorSymbol()
            : base("error")
        {
        }

        public override bool IsError => true;

        public override Tabularity Tabularity => Tabularity.None;

        public static readonly ErrorSymbol Instance = new ErrorSymbol();
    }
}