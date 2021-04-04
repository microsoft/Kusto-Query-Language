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

    internal class KustoRelatedElementFinder
    {
        private readonly KustoCode _code;

        public KustoRelatedElementFinder(KustoCode code)
        {
            _code = code;
        }

        public RelatedInfo FindRelatedElements(int position, FindRelatedOptions options = FindRelatedOptions.None)
        {
            var token = _code.Syntax.GetTokenAt(position);

            if (token == null || position > token.TriviaStart && position < token.TextStart - 1)
            {
                // position is inside trivia and not attached to any token.
                return RelatedInfo.Empty;
            }

            var prev = token?.GetPreviousToken();

            if (position == token.TriviaStart)
            {
                if (token.Trivia.Length > 0 || (prev != null && IsRelatable(prev) && !IsRelatable(token)))
                {
                    token = token.GetPreviousToken();
                }
            }

            if (token == null)
            {
                return RelatedInfo.Empty;
            }

            var elements = new List<RelatedElement>();

            if (IsBracket(token))
            {
                GetRelatedBrackets(token, elements);
            }
            else if (IsPartOfNameReferenceOrDeclaration(token))
            {
                GetRelatedNameReferencesAndDeclarations(token, elements, options);
            }

            if (elements.Count > 1)
            {
                var current = elements.FirstOrDefault(r => r.Start == token.TextStart);
                if (current != null)
                {
                    var currentIndex = elements.IndexOf(current);
                    return new RelatedInfo(elements, currentIndex);
                }
            }

            return RelatedInfo.Empty;
        }

        private bool IsRelatable(SyntaxToken token)
        {
            return IsBracket(token) || IsPartOfNameReferenceOrDeclaration(token);
        }

        private bool IsBracket(SyntaxToken token)
        {
            switch (token.Text)
            {
                case "{":
                case "}":
                case "(":
                case ")":
                    return true;
                case "[":
                case "]":
                    return !IsPartOfNameReferenceOrDeclaration(token);
                default:
                    return false;
            }
        }

        private void GetRelatedBrackets(SyntaxToken token, List<RelatedElement> elements)
        {
            switch (token.Text)
            {
                case "{":
                    GetNextTokenReference(token, "}", elements);
                    break;
                case "}":
                    GetPreviousTokenReference(token, "{", elements);
                    break;
                case "(":
                    GetNextTokenReference(token, ")", elements);
                    break;
                case ")":
                    GetPreviousTokenReference(token, "(", elements);
                    break;
                case "[":
                    GetNextTokenReference(token, "]", elements);
                    break;
                case "]":
                    GetPreviousTokenReference(token, "[", elements);
                    break;
            }
        }

        private void GetNextTokenReference(SyntaxToken token, string matchingText, List<RelatedElement> elements)
        {
            var parent = token.Parent;
            var index = parent.GetChildIndex(token);

            for (int i = index + 1, n = parent.ChildCount; i < n; i++)
            {
                var child = parent.GetChild(i);
                if (child is SyntaxToken t && t.Text == matchingText)
                {
                    elements.Add(new RelatedElement(token.TextStart, token.Text.Length, RelatedElementKind.Syntax));
                    elements.Add(new RelatedElement(t.TextStart, t.Text.Length, RelatedElementKind.Syntax));
                    return;
                }
            }
        }

        private void GetPreviousTokenReference(SyntaxToken token, string matchingText, List<RelatedElement> elements)
        {
            var parent = token.Parent;
            var index = parent.GetChildIndex(token);

            for (int i = index - 1; i >= 0; i--)
            {
                var child = parent.GetChild(i);
                if (child is SyntaxToken t && t.Text == matchingText)
                {
                    elements.Add(new RelatedElement(t.TextStart, t.Text.Length, RelatedElementKind.Syntax));
                    elements.Add(new RelatedElement(token.TextStart, token.Text.Length, RelatedElementKind.Syntax));
                    return;
                }
            }
        }

        private static bool IsPartOfNameReferenceOrDeclaration(SyntaxToken token)
        {
            var name = token.Parent.GetFirstAncestorOrSelf<Name>();
            return name != null && (name.Parent is NameReference || name.Parent is NameDeclaration);
        }

        public void GetRelatedNameReferencesAndDeclarations(SyntaxToken token, List<RelatedElement> elements, FindRelatedOptions options)
        {
            var symbol = token.Parent.GetFirstAncestorOrSelf<SyntaxNode>(n => n.ReferencedSymbol != null)?.ReferencedSymbol;

            if (symbol != null)
            {
                _code.Syntax.WalkElements(e =>
                {
                    if (AreSymbolsEqual(symbol, e, options))
                    {
                        RelatedElementKind kind;

                        if (e is NameDeclaration)
                        {
                            kind = RelatedElementKind.Declaration;
                        }
                        else if (e is NameReference)
                        {
                            kind = RelatedElementKind.Reference;
                        }
                        else
                        {
                            kind = RelatedElementKind.Other;
                        }

                        elements.Add(new RelatedElement(e.TextStart, e.Width, kind, e.TextStart, e.TextStart));
                    }
                });
            }
        }

        /// <summary>
        /// Returns true if the symbol of element <paramref name="element"/>
        /// is equal to <paramref name="symbol"/>
        /// </summary>
        private static bool AreSymbolsEqual(Symbol symbol, SyntaxElement element, FindRelatedOptions options)
        {
            var canSee = (options & FindRelatedOptions.SeeThroughVariables) != 0;
            return (element is NameReference n && (n.ReferencedSymbol == symbol || (canSee && n.ReferencedSymbol == Symbol.GetExpressionResultType(symbol))))
                || (element is NameDeclaration d && (d.ReferencedSymbol == symbol || (canSee && Symbol.GetExpressionResultType(d.ReferencedSymbol) == symbol)));
        }
    }
}
