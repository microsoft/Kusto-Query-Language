using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol representing a non-type.
    /// </summary>
    public sealed class VoidSymbol : TypeSymbol
    {
        public override SymbolKind Kind => SymbolKind.Void;

        public override Tabularity Tabularity => Tabularity.None;

        private VoidSymbol()
            : base("void")
        {
        }

        public static readonly VoidSymbol Instance = new VoidSymbol();
    }
}