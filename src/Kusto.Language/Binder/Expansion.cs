using System;

namespace Kusto.Language.Binding
{
    using Syntax;

    /// <summary>
    /// A <see cref="SyntaxTree"/> that represents the expanded body of a function call,
    /// a re-evaluation of the function body as if it were expanded inline at the location of the call
    /// with the arguments and local variables in scope considered.
    /// </summary>
    internal class Expansion : SyntaxTree
    {
        public FunctionBody Body => (FunctionBody)this.Root;

        public Expansion(FunctionBody body)
            : base(body)
        {
        }
    }
}