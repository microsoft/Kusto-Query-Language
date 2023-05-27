using System;
using System.Collections.Generic;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A base type for all scalar types.
    /// </summary>
    public abstract class ScalarSymbol : TypeSymbol
    {
        protected ScalarSymbol(string name)
            : base(name)
        {
        }

        public override Tabularity Tabularity => Tabularity.Scalar;

        /// <summary>
        ///  Other names for this type.
        /// </summary>
        public virtual IReadOnlyList<string> Aliases => None;

        private static readonly IReadOnlyList<string> None = new string[] { };

        public virtual bool IsInteger => false;
        public virtual bool IsNumeric => false;
        public virtual bool IsInterval => false;
        public virtual bool IsSummable => false;
        public virtual bool IsOrderable => false;
        public virtual bool IsMultiValue => false;

        /// <summary>
        /// True if this symbol is wider than the specified symbol.
        /// </summary>
        public virtual bool IsWiderThan(ScalarSymbol scalar) => false;

        /// <summary>
        /// Gets the <see cref="ScalarSymbol"/> for the type name.
        /// </summary>
        public static ScalarSymbol From(string typeName)
        {
            return ScalarTypes.GetSymbol(typeName);
        }
    }

    /// <summary>
    /// A symbol for scalar types: long, real, string, bool, etc.
    /// </summary>
    public sealed class PrimitiveSymbol : ScalarSymbol
    {
        private readonly IReadOnlyList<string> _aliases;
        private readonly ScalarFlags _flags;
        private readonly IReadOnlyList<ScalarSymbol> _widerThan;

        public PrimitiveSymbol(string name, string[] aliases = null, ScalarFlags flags = ScalarFlags.None, ScalarSymbol[] widerThan = null)
            : base(name)
        {
            _aliases = aliases.ToReadOnly().CheckArgumentNullOrElementNull(nameof(aliases));
            _flags = flags;
            _widerThan = widerThan.ToReadOnly().CheckArgumentNullOrElementNull(nameof(widerThan));
        }

        public override SymbolKind Kind => SymbolKind.Primitive;
        public override IReadOnlyList<string> Aliases => _aliases;
        public override bool IsInteger => (_flags & ScalarFlags.Integer) != 0;
        public override bool IsNumeric => (_flags & ScalarFlags.Numeric) != 0;
        public override bool IsInterval => (_flags & ScalarFlags.Interval) != 0;
        public override bool IsSummable => (_flags & ScalarFlags.Summable) != 0;
        public override bool IsOrderable => (_flags & ScalarFlags.Orderable) != 0;

        public override bool IsWiderThan(ScalarSymbol scalar)
        {
            for (int i = 0; i < _widerThan.Count; i++)
            {
                if (_widerThan[i] == scalar)
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