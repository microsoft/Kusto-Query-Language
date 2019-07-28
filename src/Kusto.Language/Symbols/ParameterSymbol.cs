using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol for a declared function's parameter
    /// </summary>
    public sealed class ParameterSymbol : Symbol
    {
        public TypeSymbol Type { get; }

        public override SymbolKind Kind => SymbolKind.Parameter;

        public ParameterSymbol(string name, TypeSymbol type)
            : base(name)
        {
            this.Type = type;
        }

        public override Tabularity Tabularity => this.Type.Tabularity;

        protected override string GetDisplay() =>
            $"{this.Name}: {this.Type.Display}";
    }
}