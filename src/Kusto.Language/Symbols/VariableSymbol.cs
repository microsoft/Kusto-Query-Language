using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;
    using Syntax;

    /// <summary>
    /// A symbol for a variable declaration.
    /// Typically from a let statement.
    /// </summary>
    public sealed class VariableSymbol : Symbol
    {
        /// <summary>
        /// The type of the variable.
        /// </summary>
        public TypeSymbol Type { get; }

        /// <summary>
        /// True if the variable should be considered a constant.
        /// </summary>
        public bool IsConstant { get; }

        /// <summary>
        /// The known constant value (or null if unknown).
        /// </summary>
        public object ConstantValue { get; }

        /// <summary>
        /// Creates a new instance of a <see cref="VariableSymbol"/>
        /// </summary>
        public VariableSymbol(string name, TypeSymbol type, bool isConstant = false, object constantValue = null)
            : base(name)
        {
            this.Type = type;
            this.IsConstant = isConstant;
            this.ConstantValue = constantValue;
        }

        public override SymbolKind Kind => SymbolKind.Variable;

        public override Tabularity Tabularity => this.Type.Tabularity;

        protected override string GetDisplay() =>
            $"let {this.Name}: {this.Type.Display}";
    }
}