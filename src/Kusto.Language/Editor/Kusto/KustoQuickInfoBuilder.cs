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
        private readonly KustoCodeService _service;
        private readonly KustoCode _code;

        public KustoQuickInfoBuilder(KustoCodeService service, KustoCode code)
        {
            _service = service;
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
                        return new QuickInfo(symbolInfo, diagnosticInfo);
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

        private QuickInfoItem GetSymbolInfo(int position)
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
                                //return $"(literal) {expr.ResultType.Display}";
                                return new QuickInfoItem(QuickInfoKinds.Literal, expr.ResultType.Display);
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

        private QuickInfoItem GetSymbolInfo(Symbol symbol, TypeSymbol type)
        {
            if (type == ScalarTypes.Type)
            {
                return new QuickInfoItem(QuickInfoKinds.Type, symbol.Display);
            }

            switch (symbol)
            {
                case ColumnSymbol c:
                    return GetItem(QuickInfoKinds.Column, c.Name, c.Type);

                case TableSymbol t:
                    return GetItem(QuickInfoKinds.Table, t.Name, t);

                case VariableSymbol v:
                    if (v.Type is FunctionSymbol
                        || v.Type is PatternSymbol
                        || v.Type is TableSymbol)
                    {
                        return GetSymbolInfo(v.Type, type);
                    }
                    else 
                    {
                        var kind = (v.Type is ScalarSymbol) ? QuickInfoKinds.Scalar : QuickInfoKinds.Variable;
                        return GetItem(kind, v.Name, v.Type);
                    }

                case FunctionSymbol f:
                    var fkind = _code.Globals.IsBuiltInFunction(f) ? QuickInfoKinds.BuiltInFunction
                              : _code.Globals.IsDatabaseFunction(f) ? QuickInfoKinds.DatabaseFunction
                              : QuickInfoKinds.LocalFunction;

                    return GetItem(fkind, f.Display, type);

                case OperatorSymbol o:
                    return GetItem(QuickInfoKinds.Operator, o.Name, type);

                case PatternSymbol p:
                    return GetItem(QuickInfoKinds.Pattern, p.Name, type);

                case DatabaseSymbol d:
                    return GetItem(QuickInfoKinds.Database, d.Name, null);

                case ClusterSymbol c:
                    return GetItem(QuickInfoKinds.Cluster, c.Name, null);

                case ParameterSymbol p:
                    return GetItem(QuickInfoKinds.Parameter, p.Name, p.Type);

                case TupleSymbol t:
                    return GetItem(QuickInfoKinds.Tuple, t.Name, t);

                case GroupSymbol g:
                    var gtext = string.Join("\n", g.Members.Select(m => GetSymbolInfo(m, null).Text));
                    return new QuickInfoItem(QuickInfoKinds.Group, gtext);

                default:
                    return null;
            }
        }

        private static QuickInfoItem GetItem(string kind, string name, TypeSymbol type)
        {
            var typeDisplay = GetTypeDisplay(type);
            if (typeDisplay != null)
            {
                return new QuickInfoItem(kind, $"{name}: {typeDisplay}");
            }
            else
            {
                return new QuickInfoItem(kind, name);
            }
        }

        private static string GetTypeDisplay(TypeSymbol type)
        {
            if (type == null || type == ScalarTypes.Unknown || type.IsError)
            {
                return null;
            }
            else if (type.IsTabular)
            {
                return "table";
            }
            else
            {
                return type.Display;
            }
        }

        private QuickInfoItem GetSyntaxInfo(int position)
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

        private QuickInfoItem GetDiagnosticInfo(int position, CancellationToken cancellationToken)
        {
            var diagnostics = _service.GetDiagnostics(waitForAnalysis: false, cancellationToken: cancellationToken)
                .Concat(_service.GetExtendedDiagnostics(waitForAnalysis: false, cancellationToken: cancellationToken));

            Diagnostic bestDx = null;

            foreach (var d in diagnostics)
            {
                var end = d.End > d.Start ? d.End : d.End + 1;
                if (position >= d.Start && position < end)
                {
                    // a later matching diagnostic is better if it starts closer to the position
                    if (bestDx == null || d.Start > bestDx.Start)
                    {
                        bestDx = d;
                    }
                }
            }

            if (bestDx != null)
            {
                return new QuickInfoItem(GetQuickInfoKind(bestDx.Severity), bestDx.Message);
            }

            return null;
        }

        private static string GetQuickInfoKind(string severity)
        {
            switch (severity)
            {
                case DiagnosticSeverity.Error:
                    return QuickInfoKinds.Error;
                case DiagnosticSeverity.Warning:
                    return QuickInfoKinds.Warning;
                case DiagnosticSeverity.Suggestion:
                    return QuickInfoKinds.Suggestion;
                default:
                    return QuickInfoKinds.Text;
            }
        }
    }
}