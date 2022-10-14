using System;

namespace Kusto.Language
{
    using Syntax;

    /// <summary>
    /// A <see cref="SyntaxTree"/> that represents the evaluated body of the function called,
    /// as if it were expanded inline at the location of the call with the arguments and local variables in scope considered.
    /// </summary>
    internal class FunctionCallExpansion : SyntaxTree
    {
        public FunctionCallExpansion(SyntaxNode root, SyntaxTree original = null, int offsetInOriginal = 0)
            : base(root, original, offsetInOriginal)
        {
        }
    }
}