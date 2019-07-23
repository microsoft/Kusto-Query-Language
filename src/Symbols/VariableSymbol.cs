using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;
    using Syntax;

    /// <summary>
    /// A symbol for a local variable declaration.  (let statement)
    /// </summary>
    public sealed class VariableSymbol : Symbol
    {
        public TypeSymbol Type { get; }

        public override SymbolKind Kind => SymbolKind.Variable;

        public bool IsConstant { get; }

        public object ConstantValue { get; }

        public VariableSymbol(string name, TypeSymbol type, bool isConstant = false, object constantValue = null)
            : base(name)
        {
            this.Type = type;
            this.IsConstant = isConstant;
            this.ConstantValue = constantValue;
        }

        public override Tabularity Tabularity => this.Type.Tabularity;

        protected override string GetDisplay() =>
            $"let {this.Name}: {this.Type.Display}";
    }
}