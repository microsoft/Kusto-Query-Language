using System;
using System.Collections.Generic;
using System.Text;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Parsing;

    internal static class KustoOutliner
    {
        public static OutlineInfo GetOutlines(KustoCode code, OutliningOptions options)
        {
            var outlines = new List<OutlineRange>();

            if (options.Queries)
            {
                AddRootOutline(code, options, outlines);
            }

            if (options.Statements)
            {
                code.Syntax.WalkNodes(node =>
                {
                    if (node is Statement s)
                    {
                        AddStatementOutline(code, s, options, outlines);
                    }
                });
            }

            return new OutlineInfo(outlines);
        }

        private static bool OnSameLine(KustoCode code, SyntaxToken a, SyntaxToken b)
        {
            return code.TryGetLineAndOffset(a.TextStart, out var lineA, out _)
                && code.TryGetLineAndOffset(b.TextStart, out var lineB, out _)
                && lineA == lineB;
        }

        private static void AddRootOutline(KustoCode code, OutliningOptions options, List<OutlineRange> outlines)
        {
            var collapsedText = GetRootCollapsedText(code, options);
            var length = TextFacts.TrimEnd(code.Text, 0, code.Text.Length);
            outlines.Add(new OutlineRange(0, length, collapsedText));
        }

        private static string GetRootCollapsedText(KustoCode code, OutliningOptions options)
        {
            return GetCollapsedText(code, 0, code.Text.Length, options);
        }

        private static string GetCollapsedText(KustoCode code, int start, int length, OutliningOptions options)
        {
            var builder = new StringBuilder();

            bool braces = false;
            var end = start + length;

            for (var token = code.Syntax.GetTokenAt(start); 
                token != null && token.TextStart < end; 
                token = token.GetNextToken())
            {
                if (token.Text == "{")
                {
                    braces = true;
                    break;
                }
                else if (token.Text == "|" || token.Text == ";")
                {
                    break;
                }

                if (token.Trivia.Length > 0 && token.TriviaStart >= start)
                {
                    if (builder.Length == 0 && token.Trivia.Length > 0)
                    {
                        var len = Math.Min(token.Trivia.Length, options.MaxCollapsedTextLength);
                        builder.Append(token.Trivia, 0, len);
                    }
                    else if (token.Text.Length > 0)
                    {
                        builder.Append(" ");
                    }
                }

                if (builder.Length + token.Text.Length <= options.MaxCollapsedTextLength)
                {
                    builder.Append(token.Text);
                }
                else
                {
                    break;
                }
            }

            if (braces)
            {
                builder.Append(" { ... }");
            }
            else
            {
                builder.Append(" ...");
            }

            return builder.ToString();
        }

        private static void AddStatementOutline(KustoCode code, Statement statement, OutliningOptions options, List<OutlineRange> outlines)
        {
            var firstToken = statement.GetFirstToken();
            var lastToken = statement.GetLastToken();

            if (firstToken == null
                || lastToken == null
                || OnSameLine(code, firstToken, lastToken))
            {
                return;
            }

            var collapsed = GetCollapsedText(code, firstToken.TextStart, lastToken.End - firstToken.TextStart, options);
            outlines.Add(new OutlineRange(firstToken.TextStart, lastToken.End - firstToken.TextStart, collapsed));
        }

#if false
        private static void AddBracketOutline(KustoCode code, SyntaxToken startToken, List<OutlineRange> outlines)
        {
            switch (startToken.Text)
            {
                case "{":
                    AddBracketOutline(code, startToken, "}", outlines);
                    break;
                case "[":
                    AddBracketOutline(code, startToken, "]", outlines);
                    break;
                case "(":
                    AddBracketOutline(code, startToken, ")", outlines);
                    break;
            }
        }

        private static void AddBracketOutline(KustoCode code, SyntaxToken startToken, string endTokenText, List<OutlineRange> outlines)
        {
            if (KustoRelatedElementFinder.GetNextMatchingToken(startToken, endTokenText) is SyntaxToken endToken
                && !OnSameLine(code, startToken, endToken))
            {
                var outline =
                    new OutlineRange(
                        startToken.TextStart,
                        endToken.End - startToken.TextStart,
                        startToken.Text + " ... " + endToken.Text);
                outlines.Add(outline);
            }
        }
#endif
    }
}
