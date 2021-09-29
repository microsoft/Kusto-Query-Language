using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Binding
{
    using Symbols;

    /// <summary>
    /// Represents the information known at the location of a user function call.
    /// </summary>
    internal class CallSiteInfo
    {
        /// <summary>
        /// The signature of the function being called.
        /// </summary>
        public Signature Signature { get; }

        /// <summary>
        /// The parameters of the function represented as local variables.
        /// </summary>
        public IReadOnlyList<VariableSymbol> Locals { get; }

        public CallSiteInfo(Signature signature, IReadOnlyList<VariableSymbol> locals)
        {
            this.Signature = signature;
            this.Locals = locals;
        }

        public override string ToString()
        {
            return Signature.Symbol.Name + "("
                + string.Join(",", this.Locals.Select(v => v.IsConstant && v.ConstantValue != null ? $"{v.Name}={v.ConstantValue}" : v.Name))
                + ")";
        }

        internal class Comparer : IEqualityComparer<CallSiteInfo>
        {
            public static readonly Comparer Instance = new Comparer();

            public bool Equals(CallSiteInfo x, CallSiteInfo y)
            {
                if (x.Signature != y.Signature)
                    return false;

                if (x.Locals.Count != y.Locals.Count)
                    return false;

                for (int i = 0; i < x.Locals.Count; i++)
                {
                    var lx = x.Locals[i];
                    var ly = y.Locals[i];

                    if (lx.Name != ly.Name
                        || lx.Type != ly.Type
                        || lx.IsConstant != ly.IsConstant
                        || !object.Equals(lx.ConstantValue, ly.ConstantValue))
                        return false;
                }

                return true;
            }

            public int GetHashCode(CallSiteInfo obj)
            {
                return obj.Signature.GetHashCode();
            }
        }
    }
}