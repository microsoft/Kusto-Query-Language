using System;

namespace Kusto.Language
{
    using Syntax;
    using System.Collections.Generic;

    /// <summary>
    /// A <see cref="SyntaxTree"/> that represents the evaluated body of the function called,
    /// as if it were expanded inline at the location of the call with the arguments and local variables in scope considered.
    /// </summary>
    internal class FunctionCallExpansion : SyntaxTree
    {
        public FunctionBody Body => (FunctionBody)this.Root;

        public FunctionCallExpansion(FunctionBody body)
            : base(body)
        {
        }
    }
}