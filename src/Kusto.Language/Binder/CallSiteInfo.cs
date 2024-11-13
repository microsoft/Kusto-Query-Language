using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Binding
{
    using Symbols;
    using Utils;

    /// <summary>
    /// Represents the call site info needed used to identify a unique function call.
    /// </summary>
    internal class CallSiteInfo : IEquatable<CallSiteInfo>
    {
        /// <summary>
        /// The signature of the function being called.
        /// </summary>
        public Signature Signature { get; }

        /// <summary>
        /// The parameters that make this call site unique.
        /// </summary>
        public IReadOnlyList<Parameter> Parameters { get; }

        /// <summary>
        /// The list of values corresponding to the parameters.
        /// </summary>
        public IReadOnlyList<object> Values { get; }

        public CallSiteInfo(
            Signature signature, 
            IReadOnlyList<Parameter> parameters, 
            IReadOnlyList<object> values)
        {
            this.Signature = signature;
            this.Parameters = parameters ?? EmptyReadOnlyList<Parameter>.Instance;
            this.Values = values ?? EmptyReadOnlyList<object>.Instance;
            System.Diagnostics.Debug.Assert(this.Parameters.Count == this.Values.Count, "parameter/values count mismatch");
        }

        public override string ToString()
        {
            return Signature.Symbol.Name + "("
                + string.Join(",", Enumerable.Range(0, this.Values.Count).Select(i => $"{this.Parameters[i].Name}={this.Values[i]}"))
                + ")";
        }

        public bool Equals(CallSiteInfo other)
        {
            if (this.Signature != other.Signature)
                return false;

            if (this.Parameters.Count != other.Parameters.Count)
                return false;

            for (int i = 0; i < this.Parameters.Count; i++)
            {
                if (this.Parameters[i] != other.Parameters[i])
                    return false;
            }

            for (int i = 0; i < this.Values.Count; i++)
            {
                var vx = this.Values[i];
                var vy = other.Values[i];

                if (vx is TableSymbol tx && vy is TableSymbol ty)
                {
                    if (!TableSymbol.AreResultEquivalent(tx, ty))
                        return false;
                }
                else
                {
                    if (!object.Equals(vx, vy))
                        return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.Signature.GetHashCode();
        }
    }
}