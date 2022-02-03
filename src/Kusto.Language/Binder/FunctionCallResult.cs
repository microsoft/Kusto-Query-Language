using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Binding
{
    using Parsing;
    using Symbols;
    using Syntax;
    using Utils;

    /// <summary>
    /// Represents the result information for a function or operator invocation.
    /// </summary>
    internal struct FunctionCallResult
    {
        /// <summary>
        /// The result type of this signature.
        /// </summary>
        public TypeSymbol Type { get; }

        /// <summary>
        /// The extended semantic info for the function call.
        /// </summary>
        public FunctionCallInfo Info { get; }

        public FunctionCallResult(TypeSymbol type, FunctionCallInfo info = null)
        {
            this.Type = type;
            this.Info = info;
        }

        public static implicit operator FunctionCallResult(TypeSymbol type)
        {
            return new FunctionCallResult(type, null);
        }
    }
}
