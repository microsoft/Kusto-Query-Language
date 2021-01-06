using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol for scalar types: long, real, string, bool, etc.
    /// </summary>
    public sealed class ScalarSymbol : TypeSymbol
    {
        public IReadOnlyList<string> Aliases { get; }

        public ScalarFlags Flags { get; }

        /// <summary>
        /// The set of scalar types that this type is considered wider than.
        /// Narrower symbols can be implicitly converted to wider symbols.
        /// </summary>
        public IReadOnlyList<ScalarSymbol> WiderThan { get; }

        public ScalarSymbol(string name, string[] aliases = null, ScalarFlags flags = ScalarFlags.None, ScalarSymbol[] widerThan = null)
            : base(name)
        {
            this.Aliases = aliases.ToReadOnly();
            this.Flags = flags;
            this.WiderThan = widerThan.ToReadOnly();
        }

        /// <summary>
        /// Gets the <see cref="ScalarSymbol"/> for the type name.
        /// </summary>
        public static ScalarSymbol From(string typeName)
        {
            return ScalarTypes.GetSymbol(typeName);
        }

        public override SymbolKind Kind => SymbolKind.Scalar;

        public override Tabularity Tabularity => Tabularity.Scalar;

        public bool IsInteger => (this.Flags & ScalarFlags.Integer) != 0;

        public bool IsNumeric => (this.Flags & ScalarFlags.Numeric) != 0;

        public bool IsInterval => (this.Flags & ScalarFlags.Interval) != 0;

        public bool IsSummable => (this.Flags & ScalarFlags.Summable) != 0;

        public bool IsOrderable => (this.Flags & ScalarFlags.Orderable) != 0;

        /// <summary>
        /// True if this symbol is wider than the specified symbol.
        /// </summary>
        public bool IsWiderThan(ScalarSymbol scalar)
        {
            for (int i = 0; i < this.WiderThan.Count; i++)
            {
                if (this.WiderThan[i] == scalar)
                    return true;
            }

            return false;
        }
    }

    [Flags]
    public enum ScalarFlags
    {
        None     = 0b0000_0000,

        /// <summary>
        /// Is an integer type
        /// </summary>
        Integer  = 0b0000_0001,

        /// <summary>
        /// Is a numeric type
        /// </summary>
        Numeric  = 0b0000_0010,

        /// <summary>
        /// Is an interval type (typically add/subtract operator is defined for this)
        /// </summary>
        Interval = 0b0000_0100,

        /// <summary>
        /// Can be used in the sum aggregate
        /// </summary>
        Summable = 0b0000_1000,

        /// <summary>
        /// Can be used in order by or arg_max aggregate
        /// </summary>
        Orderable = 0b0001_0000,

        /// <summary>
        /// All flags
        /// </summary>
        All = Integer | Numeric | Interval | Summable | Orderable
    }
}