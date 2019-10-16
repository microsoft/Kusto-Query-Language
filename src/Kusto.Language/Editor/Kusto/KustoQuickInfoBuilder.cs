using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Kusto.Language.Parsing;
    using Kusto.Language.Symbols;
    using Syntax;
    using System.Linq;
    using System.Text;
    using Utils;

    internal class KustoQuickInfoBuilder
    {
        private readonly KustoCode _code;

        public KustoQuickInfoBuilder(KustoCode code)
        {
            _code = code;
        }

        /// <summary>
        /// Gets the quick info for the item near the specified text position.
        /// </summary>
        public QuickInfo GetQuickInfo(int position, CancellationToken cancellationToken)
        {
            if (_code.HasSemantics)
            {
                var builder = new StringBuilder();

                var symbolInfo = GetSymbolInfo(position) ?? GetSyntaxInfo(position);
                var diagnosticInfo = GetDiagnosticInfo(position, cancellationToken);

                if (symbolInfo != null)
                {
                    if (diagnosticInfo != null)
                    {
                        return new QuickInfo($"{symbolInfo}\n\n{diagnosticInfo}");
                    }
                    else
                    {
                        return new QuickInfo(symbolInfo);
                    }
                }
                else if (diagnosticInfo != null)
                {
                    return new QuickInfo(diagnosticInfo);
                }
            }

            return QuickInfo.Empty;
        }

        private string GetSymbolInfo(int position)
        {
            var token = _code.Syntax.GetTokenAt(position);
            if (token != null)
            {
                if (token.Parent is SyntaxNode node)
                {
                    var expr = node.GetFirstAncestorOrSelf<Expression>();

                    if (node.ReferencedSymbol != null && expr != node)
                    {
                        var info = GetSymbolInfo(node.ReferencedSymbol, null);
                        if (info != null)
                        {
                            return info;
                        }
                    }
                    else if (expr != null)
                    {
                        if (expr.IsLiteral)
                        {
                            if (expr.Parent is BrackettedExpression br)
                            {
                                // special case for ['column name']
                                expr = br;
                            }
                            else if (expr.ResultType != null && expr.ResultType.IsScalar)
                            {
                                return $"(literal) {expr.ResultType.Display}";
                            }
                        }

                        var info = GetSymbolInfo(expr.ReferencedSymbol, expr.ResultType);
                        if (info != null)
                        {
                            return info;
                        }
                    }
                }
            }

            return null;
        }

        private string GetSymbolInfo(Symbol symbol, TypeSymbol type)
        {
            if (type == ScalarTypes.Type)
            {
                return $"(type) {symbol.Display}";
            }

            switch (symbol)
            {
                case ColumnSymbol c:
                    if (c.Type != ScalarTypes.Unknown)
                    {
                        return $"(column) {c.Name}: {c.Type.Display}";
                    }
                    else
                    {
                        return $"(column) {c.Name}";
                    }

                case TableSymbol t:
                    return $"(table) {t.Name}: {t.Display}";

                case VariableSymbol v:
                    if (v.Type is FunctionSymbol
                        || v.Type is PatternSymbol
                        || v.Type is TableSymbol)
                    {
                        return GetSymbolInfo(v.Type, type);
                    }
                    else 
                    {
                        var kind = (v.Type is ScalarSymbol ? "(scalar)" : "(variable)");
                        if (v.Type != ScalarTypes.Unknown)
                        {
                            return $"{kind} {v.Name}: {v.Type.Display}";
                        }
                        else
                        {
                            return $"{kind} {v.Name}";
                        }
                    }

                case FunctionSymbol f:
                    if (type != null && !type.IsError && type != ScalarTypes.Unknown)
                    {
                        return $"(function) {f.GetDisplay(verbose: true)}: {type.Display}";
                    }
                    else
                    {
                        return $"(function) {f.GetDisplay(verbose: true)}";
                    }

                case OperatorSymbol o:
                    if (type != null && !type.IsError && type != ScalarTypes.Unknown)
                    {
                        return $"(operator) {o.Name}: {type.Display}";
                    }
                    else
                    {
                        return $"(operator) {o.Name}";
                    }

                case PatternSymbol p:
                    if (type != null && !type.IsError && type != ScalarTypes.Unknown)
                    {
                        return $"(pattern) {p.Display}: {type.Display}";
                    }
                    else
                    {
                        return $"(pattern) {p.Display}";
                    }

                case DatabaseSymbol d:
                    return $"(database) {d.Name}";

                case ClusterSymbol c:
                    return $"(cluster) {c.Name}";

                case ParameterSymbol p:
                    return $"(parameter) {p.Display}";

                case TupleSymbol t:
                    return $"(tuple) {t.Display}";

                case GroupSymbol g:
                    return string.Join("\n", g.Members.Select(m => GetSymbolInfo(m, null)));

                default:
                    return null;
            }
        }

        private string GetSyntaxInfo(int position)
        {
#if false
            var token = _code.Syntax.GetTokenAt(position);
            if (token != null && (token.Kind == SyntaxKind.IdentifierToken || token.Kind.GetCategory() == SyntaxCategory.Keyword))
            {
                var grammar = GetBestGrammarAtPosition(position);
                if (grammar != null)
                {
                    return grammar.Description;
                }
            }
#endif
            return null;
        }

        /// <summary>
        /// Scans for all the grammar rules that are considered for the token at the specified text position.
        /// </summary>
        private Parser<LexicalToken> GetBestGrammarAtPosition(int position)
        {
            var offset = GetTokenIndex(position);
            var source = new ArraySource<LexicalToken>(_code.LexerTokens);

            Parser<LexicalToken> bestGrammar = null;
            int bestLength = -1;

            _code.Grammar.Search(source, (_parser, _source, _start, _prevWasMissing) =>
            {
                // capture the best grammar
                if (_start == offset && _parser.Tag != null)
                {
                    var scanLength = _parser.Scan(source, _start);
                    if (scanLength > bestLength)
                    {
                        bestGrammar = _parser;
                        bestLength = scanLength;
                    }
                }
            });

            return bestGrammar;
        }

        /// <summary>
        /// Gets the index of the token that includes the text position.
        /// </summary>
        private int GetTokenIndex(int position)
        {
            if (_code.LexerTokens.Count == 0)
                return 0;

            if (position >= _code.LexerTokens[_code.LexerTokens.Count - 1].End)
                return _code.LexerTokens.Count - 1;

            var offset = _code.LexerTokens.BinarySearch(t =>
            {
                if (position < t.TriviaStart)
                    return 1;
                else if (position >= t.End)
                    return -1;
                else
                    return 0;
            });

            return offset >= 0 ? offset : 0;
        }

        private string GetDiagnosticInfo(int position, CancellationToken cancellationToken)
        {
            StringBuilder builder = null;

            foreach (var d in _code.GetDiagnostics(cancellationToken))
            {
                var end = d.End > d.Start ? d.End : d.End + 1;
                if (position >= d.Start && position < end)
                {
                    if (builder == null)
                        builder = new StringBuilder();

                    if (builder.Length > 0)
                        builder.AppendLine();

                    builder.Append(d.Message);
                }
            }

            return builder?.ToString();
        }
    }
}