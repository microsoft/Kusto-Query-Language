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
            if (TryGetRootCollapsedText(code, options, out var collapsedText))
            {
                var length = TextFacts.TrimEnd(code.Text, 0, code.Text.Length);
                outlines.Add(new OutlineRange(0, length, collapsedText));
            }
        }

        private static bool TryGetRootCollapsedText(KustoCode code, OutliningOptions options, out string collapsedText)
        {
            return TryGetCollapsedText(code, 0, code.Text.Length, options, out collapsedText);
        }

        private static bool TryGetCollapsedText(KustoCode code, int start, int length, OutliningOptions options, out string collapsedText)
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

            var newText = builder.ToString();

            if (!TextFacts.IsWhitespaceOnly(newText))
            {
                if (braces)
                {
                    collapsedText = newText + " { ... }";
                }
                else
                {
                    collapsedText = newText + " ...";
                }
                return true;
            }
            else
            {
                collapsedText = null;
                return false;
            }
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

            if (TryGetCollapsedText(code, firstToken.TextStart, lastToken.End - firstToken.TextStart, options, out var collapsedText))
            {
                outlines.Add(new OutlineRange(firstToken.TextStart, lastToken.End - firstToken.TextStart, collapsedText));
            }
        }
    }
}
