using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    using Binding;
    using Parsing;
    using Symbols;
    using Syntax;
    using Utils;

    internal static class KustoClassifier
    {
        public static void GetClassifications(SyntaxNode root, int start, int length, bool clipToRange, List<ClassifiedRange> list, CancellationToken cancellationToken)
        {
            var end = start + length;

            // don't let classification ranges escape the requested bounds.
            Action<ClassifiedRange> limiter = range =>
            {
                // only if the classification range overlaps window
                if (range.End > start && range.Start < end)
                {
                    // adjust range it if starts before or ends after the window
                    if (clipToRange && (range.Start < start || range.End > end))
                    {
                        var clipStart = Math.Max(range.Start, start);
                        var clipEnd = Math.Min(range.End, end);
                        range = new ClassifiedRange(range.Kind, clipStart, clipEnd - clipStart);
                    }

                    // merge ranges if this range is adjacent to the last range and the same kind
                    if (list.Count > 0)
                    {
                        var last = list[list.Count - 1];
                        if (last.Kind == range.Kind
                            && last.End == range.Start)
                        {
                            list[list.Count - 1] = new ClassifiedRange(last.Kind, last.Start, last.Length + range.Length);
                            return;
                        }
                    }

                    list.Add(range);
                }
            };

            // produce classification ranges for all tokens that coincide with the window
            root.WalkTokens(start, end, token =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                GetTriviaClassifications(token, limiter);

                if (token.IsLiteral && token.Prefix.Length > 0)
                {
                    // special case for literals with prefix
                    limiter(new ClassifiedRange(ClassificationKind.Keyword, token.TextStart, token.Prefix.Length));
                    limiter(new ClassifiedRange(ClassificationKind.Literal, token.TextStart + token.Prefix.Length, token.Text.Length - token.Prefix.Length));
                }
                else
                {
                    var kind = GetKind(token);

                    limiter(new ClassifiedRange(kind, token.TextStart, token.Width));
                }
            });
        }

        private static ClassificationKind GetKind(SyntaxToken token)
        {
            if (IsSkipped(token))
                return ClassificationKind.PlainText;

            if (IsCommandVerbToken(token))
                return ClassificationKind.Command;

            if (token.Parent is BracedName)
                return ClassificationKind.ClientParameter;

            switch (token.Kind.GetCategory())
            {
                case SyntaxCategory.Identifier:
                    if (IsQueryParameter(token))
                    {
                        return ClassificationKind.QueryParameter;
                    }
                    else
                    {
                        return GetIdentifierKind(token);
                    }

                case SyntaxCategory.Literal:
                    if (token.Kind == SyntaxKind.StringLiteralToken)
                    {
                        return ClassificationKind.StringLiteral;
                    }
                    else
                    {
                        return ClassificationKind.Literal;
                    }

                case SyntaxCategory.Keyword:
                    if (IsIdentifierKeyword(token))
                    {
                        goto case SyntaxCategory.Identifier;
                    }
                    else if (IsTypeToken(token))
                    {
                        return ClassificationKind.Type;
                    }
                    else if (IsScalarOperatorToken(token))
                    {
                        return ClassificationKind.ScalarOperator;
                    }
                    else if (IsQueryOperatorToken(token))
                    {
                        return ClassificationKind.QueryOperator;
                    }
                    else if (IsQueryParameter(token))
                    {
                        return ClassificationKind.QueryParameter;
                    }
                    else if (IsCommandVerbToken(token))
                    {
                        return ClassificationKind.Command;
                    }
                    else
                    {
                        return ClassificationKind.Keyword;
                    }

                case SyntaxCategory.Punctuation:
                    return ClassificationKind.Punctuation;

                case SyntaxCategory.Operator:
                    return ClassificationKind.MathOperator;

                default:
                    if (token.Kind == SyntaxKind.DirectiveToken)
                    {
                        return ClassificationKind.Directive;
                    }
                    else
                    {
                        return ClassificationKind.PlainText;
                    }
            }
        }

        private static ClassificationKind GetIdentifierKind(SyntaxToken token)
        {
            var symbolKind = token.Parent.GetFirstAncestorOrSelf<SyntaxNode>(n => n.ReferencedSymbol != null)
                                    ?.ReferencedSymbol.Kind ?? SymbolKind.None;
            switch (symbolKind)
            {
                case SymbolKind.Column:
                    return ClassificationKind.Column;
                case SymbolKind.Table:
                    return ClassificationKind.Table;
                case SymbolKind.Database:
                    return ClassificationKind.Database;
                case SymbolKind.Function:
                case SymbolKind.Pattern:
                    return ClassificationKind.Function;
                case SymbolKind.Variable:
                    return ClassificationKind.Variable;
                case SymbolKind.Parameter:
                    return ClassificationKind.Parameter;
                case SymbolKind.MaterializedView:
                    return ClassificationKind.MaterializedView;
                case SymbolKind.Option:
                    return ClassificationKind.Option;
                default:
                    if (IsName(token))
                    {
                        return ClassificationKind.Identifier;
                    }
                    else
                    {
                        return ClassificationKind.Keyword;
                    }
            }
        }

        private static bool IsSkipped(SyntaxToken token)
        {
            return token.Parent is SyntaxList ls && ls.Parent is SkippedTokens;
        }

        /// <summary>
        /// Determines if an identifier token is being used as a keyword (not a name)
        /// </summary>
        private static bool IsName(SyntaxToken token)
        {
            // the token is part of an actual name
            return token.Parent is Name;
        }

        private static bool IsIdentifierKeyword(SyntaxToken token)
        {
            return token.Parent is TokenName;
        }

        private static bool IsTypeToken(SyntaxToken token)
        {
            if (token.Parent is PrimitiveTypeExpression pt)
            {
                var type = Binder.GetType(pt);
                return type != null && !type.IsError;
            }

            return false;
        }

        private static bool IsScalarOperatorToken(SyntaxToken token)
        {
            return token.Parent is BinaryExpression b && b.Operator == token
                   || token.Parent is PrefixUnaryExpression u && u.Operator == token;
        }

        private static bool IsQueryOperatorToken(SyntaxToken token)
        {
            return (token.Parent is QueryOperator q && q.GetChild(0) == token)
                || (token.Parent is TopNestedClause tc && tc.GetChild(0) == token);
        }

        private static bool IsQueryParameter(SyntaxToken token)
        {
            return token.Parent is Name n && n.Parent is NameDeclaration d && d.Parent is NamedParameter np && np.GetChild(0) == d;
        }

        private static bool IsCommandVerbToken(SyntaxToken token)
        {
            return (token.Parent is Command c && c.GetChildIndex(token) < 2) // dot == 0, keyword == 1
                || (token.Parent is CustomNode cn && cn.Parent is CustomCommand && cn.GetChildIndex(token) == 0)
                || (token.Parent is SyntaxList list && list.Parent is UnknownCommand uc && uc.Parts[0] == token);
        }

        private static void GetTriviaClassifications(SyntaxToken token, Action<ClassifiedRange> action)
        {
            if (token.TriviaWidth > 0)
            {
                var trivia = token.Trivia;

                for (int i = 0; i < trivia.Length; i++)
                {
                    // Tag anything that is not whitespace as the start of comment! 
                    // if it wasn't the start of a comment, it wouldn't be in the trivia!
                    // TODO: if we ever get more kinds of trivia, this will need updating.
                    if (!TextFacts.IsWhitespace(trivia[i]))
                    {
                        var start = i;
                        for (;  i < trivia.Length; i++)
                        {
                            if (TextFacts.IsLineBreakStart(trivia[i]))
                                break;
                        }

                        action(new ClassifiedRange(ClassificationKind.Comment, token.TriviaStart + start, i - start));
                    }
                }
            }
        }
    }
}