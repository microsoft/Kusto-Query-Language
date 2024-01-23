using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    using Syntax;

    internal static class KustoServiceHelpers
    {
        /// <summary>
        /// Get the token that the position has affinity with,
        /// or null if the position does not have affinity (in whitespace between tokens).
        /// </summary>
        internal static SyntaxToken GetTokenWithAffinity(KustoCode code, int position)
        {
            var token = code.Syntax.GetTokenAt(position);
            var previous = token.GetPreviousToken();

            if (HasAffinity(token, position))
            {
                return token;
            }
            else if (previous != null && HasAffinity(previous, position))
            {
                return previous;
            }
            else
            {
                // fully inside trivia between tokens, no affinity
                return null;
            }
        }

        /// <summary>
        /// Returns true if the token has affinity with the position.
        /// </summary>
        public static bool HasAffinity(SyntaxToken token, int position)
        {
            return (position > token.TextStart && position < token.End)
                || position == token.TextStart && IsNameLikeToken(token.Kind)
                || position == token.End && IsNameLikeToken(token.Kind);
        }

        internal static bool IsNameLikeToken(SyntaxKind kind)
        {
            switch (kind.GetCategory())
            {
                case SyntaxCategory.Identifier:
                case SyntaxCategory.Keyword:
                    return true;
                default:
                    // ! can be start of some keywords
                    return kind == SyntaxKind.BangToken;
            }
        }

        /// <summary>
        /// Gets the token left of the token that has affinity with the position.
        /// </summary>
        public static SyntaxToken GetTokenLeftOfPosition(KustoCode code, int position)
        {
            var token = code.Syntax.GetTokenAt(position);
            var hasAffinity = token != null && HasAffinity(token, position);

            if (token != null && (position <= token.TextStart || !hasAffinity || token.Kind == SyntaxKind.EndOfTextToken))
            {
                token = token.GetPreviousToken();
            }

            return token;
        }

        /// <summary>
        /// Returns the complete expression that exists to the left of the position.
        /// </summary>
        public static Expression GetCompleteExpressionLeftOfPosition(KustoCode code, int position)
        {
            var token = GetTokenLeftOfPosition(code, position);

            var expr = token?.GetFirstAncestorOrSelf<Expression>();
            if (expr != null && expr.End == token.End && !expr.HasMissingChildren())
                return expr;

            return null;
        }
    }
}
