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
        private readonly QuickInfoOptions _options;
        private readonly DisabledDiagnostics _disabled;

        public KustoQuickInfoBuilder(KustoCodeService service, KustoCode code, QuickInfoOptions options)
        {
            _service = service;
            _code = code;
            _options = options;
            _disabled = new DisabledDiagnostics(_options.DisabledDiagnostics);
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
            if (token != null && position >= token.TextStart)
            {
                if (token.Parent is SyntaxNode node)
                {
                    var expr = node.GetFirstAncestorOrSelf<Expression>();

                    if (node.ReferencedSymbol != null && expr != node)
                    {
                        return GetSymbolInfo(node.ReferencedSymbol, null);
                    }
                    else if (expr != null)
                    {
                        if (expr.IsLiteral)
                        {
                            if (expr.Parent is BracketedExpression br)
                            {
                                // special case for ['column name']
                                expr = br;
                            }
                            else if (expr.ResultType != null && expr.ResultType.IsScalar)
                            {
                                return GetSymbolInfo(expr.ResultType, expr.ResultType, QuickInfoKind.Literal);
                            }
                        }

                        return GetSymbolInfo(expr.ReferencedSymbol, expr.ResultType);
                    }
                }
            }

            return null;
        }

        private QuickInfoItem GetSymbolInfo(Symbol symbol, TypeSymbol type, QuickInfoKind? itemKind = null)
        {
            if (symbol != null)
            {
                var texts = new List<ClassifiedText>();
                SymbolDisplay.GetSymbolDisplay(symbol, type, texts);

                if (itemKind == null)
                    itemKind = GetItemKind(symbol);

                return new QuickInfoItem(itemKind.Value, texts);
            }
            else
            {
                return null;
            }
        }

        private QuickInfoKind GetItemKind(Symbol symbol)
        {
            switch (symbol)
            {
                case ClusterSymbol _:
                    return QuickInfoKind.Cluster;
                case ColumnSymbol _:
                    return QuickInfoKind.Column;
                case DatabaseSymbol _:
                    return QuickInfoKind.Database;
                case FunctionSymbol f:
                    if (_code.Globals.IsBuiltInFunction(f))
                    {
                        return QuickInfoKind.BuiltInFunction;
                    }
                    else if (_code.Globals.IsDatabaseFunction(f))
                    {
                        return QuickInfoKind.DatabaseFunction;
                    }
                    else
                    {
                        return QuickInfoKind.LocalFunction;
                    }
                case OperatorSymbol _:
                    return QuickInfoKind.Operator;
                case ParameterSymbol _:
                    return QuickInfoKind.Parameter;
                case PatternSymbol _:
                    return QuickInfoKind.Pattern;
                case TableSymbol _:
                    return QuickInfoKind.Table;
                case VariableSymbol v:
                    if (v.Type is FunctionSymbol
                        || v.Type is PatternSymbol
                        || v.Type is TableSymbol
                        || v.Type is ScalarSymbol)
                    {
                        return GetItemKind(v.Type);
                    }
                    else
                    {
                        return QuickInfoKind.Variable;
                    }
                case ScalarSymbol _:
                    return QuickInfoKind.Type;
                case CommandSymbol _:
                    return QuickInfoKind.Command;
                case OptionSymbol _:
                    return QuickInfoKind.Option;
                case GroupSymbol gs:
                    if (gs.Members.Count > 0)
                        return GetItemKind(gs.Members[0]);
                    return QuickInfoKind.Text;
                default:
                    return QuickInfoKind.Text;
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
            var offset = _code.GetTokenIndex(position);
            var source = new ArraySource<LexicalToken>(_code.GetLexicalTokens());

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

        private QuickInfoItem GetDiagnosticInfo(int position, CancellationToken cancellationToken)
        {
            var diagnostics = _service.GetDiagnostics(waitForAnalysis: false, cancellationToken: cancellationToken)
                .Concat(_service.GetAnalyzerDiagnostics(waitForAnalysis: false, cancellationToken: cancellationToken))
                .Where(dx => _disabled.IsDiagnosticEnabled(dx));

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

        private static QuickInfoKind GetQuickInfoKind(string severity)
        {
            switch (severity)
            {
                case DiagnosticSeverity.Error:
                    return QuickInfoKind.Error;
                case DiagnosticSeverity.Warning:
                    return QuickInfoKind.Warning;
                case DiagnosticSeverity.Suggestion:
                    return QuickInfoKind.Suggestion;
                default:
                    return QuickInfoKind.Text;
            }
        }
    }

    public static class SymbolDisplay
    {
        public static void GetSymbolDisplay(Symbol symbol, TypeSymbol type, List<ClassifiedText> texts, bool showDescription = true)
        {
            if (symbol is GroupSymbol gs)
            {
                var lines = Math.Min(gs.Members.Count, 5);

                for (int i = 0; i < lines; i++)
                {
                    var subsym = gs.Members[i];
                    if (i > 0)
                        texts.Add(new ClassifiedText("\n"));

                    // TODO: get the correct types for the sub symbols
                    GetSymbolDisplay(subsym, null, texts, showDescription: false);
                }

                if (lines < gs.Members.Count)
                {
                    texts.Add(new ClassifiedText($"\n+ ({gs.Members.Count - lines}) additional"));
                }
            }
            else if (symbol is ScalarSymbol)
            {
                texts.Add(new ClassifiedText(ClassificationKind.Identifier, symbol.Name));
            }
            else if (symbol is VariableSymbol v
                && (v.Type is FunctionSymbol || v.Type is PatternSymbol))
            {
                GetSymbolDisplay(v.Type, type, texts);
            }
            else if (symbol != null)
            {
                var nameKind = GetNameClassificationKind(symbol);
                texts.Add(new ClassifiedText(nameKind, symbol.Name));

                if (symbol is FunctionSymbol fs)
                {
                    // TODO: have this use the correct signature?
                    GetSignatureDisplay(fs.Signatures[0], texts);
                }

                if (type == null && symbol is VariableSymbol vs)
                {
                    type = vs.Type;
                }

                if (type != null && type != ScalarTypes.Unknown && !type.IsError)
                {
                    texts.Add(new ClassifiedText(ClassificationKind.Punctuation, ": "));
                    GetTypeDisplay(type, texts);
                }

                if (showDescription)
                {
                    var description = GetDescription(symbol);
                    if (!string.IsNullOrEmpty(description))
                    {
                        texts.Add(new ClassifiedText(ClassificationKind.Comment, "\n\n" + description));
                    }
                }
            }
        }

        private static string GetDescription(Symbol symbol)
        {
            switch (symbol)
            {
                case TableSymbol t:
                    return t.Description;
                case ColumnSymbol c:
                    return c.Description;
                case ParameterSymbol p:
                    return p.Description;
                case FunctionSymbol f:
                    return f.Description;
                case OptionSymbol o:
                    return o.Description;
                default:
                    return "";
            }
        }

        private static ClassificationKind GetNameClassificationKind(Symbol symbol)
        {
            switch (symbol)
            {
                case ColumnSymbol _:
                    return ClassificationKind.Column;
                case DatabaseSymbol _:
                    return ClassificationKind.Database;
                case FunctionSymbol _:
                    return ClassificationKind.Function;
                case OperatorSymbol _:
                    return ClassificationKind.MathOperator;
                case ParameterSymbol _:
                    return ClassificationKind.Parameter;
                case PatternSymbol _:
                    return ClassificationKind.Function; // TODO: need separate classification
                case TableSymbol _:
                    return ClassificationKind.Table;
                case VariableSymbol _:
                    return ClassificationKind.Variable;
                case CommandSymbol _:
                    return ClassificationKind.Command;
                case OptionSymbol _:
                    return ClassificationKind.Option;
                case ScalarSymbol s:
                    if (s == ScalarTypes.String)
                    {
                        return ClassificationKind.StringLiteral;
                    }
                    else
                    {
                        return ClassificationKind.Literal;
                    }
                default:
                    return ClassificationKind.PlainText;
            }
        }

        public static void GetSignatureDisplay(Signature sig, List<ClassifiedText> texts)
        {
            texts.Add(new ClassifiedText(ClassificationKind.Punctuation, "("));

            for (int i = 0; i < sig.Parameters.Count; i++)
            {
                if (i > 0)
                {
                    texts.Add(new ClassifiedText(ClassificationKind.Punctuation, ", "));
                }

                var p = sig.Parameters[i];

                if (p.IsOptional)
                {
                    texts.Add(new ClassifiedText(ClassificationKind.Punctuation, "["));
                    texts.Add(new ClassifiedText(ClassificationKind.SignatureParameter, p.Name));
                    texts.Add(new ClassifiedText(ClassificationKind.Punctuation, "]"));
                }
                else
                {
                    texts.Add(new ClassifiedText(ClassificationKind.SignatureParameter, p.Name));
                }
            }

            if (sig.HasRepeatableParameters)
            {
                texts.Add(new ClassifiedText(ClassificationKind.Punctuation, ", "));
                texts.Add(new ClassifiedText(ClassificationKind.Punctuation, "..."));
            }

            texts.Add(new ClassifiedText(ClassificationKind.Punctuation, ")"));
        }

        private static readonly int MaxColumns = 7;

        public static void GetTypeDisplay(TypeSymbol type, List<ClassifiedText> texts, bool useName = false)
        {
            if (type is TableSymbol ts)
            {
                if (useName && !string.IsNullOrEmpty(ts.Name))
                {
                    texts.Add(new ClassifiedText(ClassificationKind.Table, ts.Name));
                }
                else
                {
                    texts.Add(new ClassifiedText(ClassificationKind.Punctuation, "("));

                    var maxCol = Math.Min(MaxColumns, ts.Columns.Count);
                    for (int i = 0; i < maxCol; i++)
                    {
                        if (i > 0)
                        {
                            texts.Add(new ClassifiedText(ClassificationKind.Punctuation, ", "));
                        }

                        var col = ts.Columns[i];
                        texts.Add(new ClassifiedText(ClassificationKind.SchemaMember, col.Name));
                    }

                    if (maxCol < ts.Columns.Count)
                    {
                        texts.Add(new ClassifiedText(ClassificationKind.Punctuation, ", "));
                        texts.Add(new ClassifiedText(ClassificationKind.Punctuation, "..."));
                    }

                    texts.Add(new ClassifiedText(ClassificationKind.Punctuation, ")"));
                }
            }
            else if (type is TupleSymbol tus)
            {
                texts.Add(new ClassifiedText(ClassificationKind.Punctuation, "{"));

                var maxCol = Math.Min(MaxColumns, tus.Columns.Count);
                for (int i = 0; i < maxCol; i++)
                {
                    if (i > 0)
                    {
                        texts.Add(new ClassifiedText(ClassificationKind.Punctuation, ", "));
                    }

                    var col = tus.Columns[i];
                    texts.Add(new ClassifiedText(ClassificationKind.Column, col.Name));
                }

                if (maxCol < tus.Columns.Count)
                {
                    texts.Add(new ClassifiedText(ClassificationKind.Punctuation, ", "));
                    texts.Add(new ClassifiedText(ClassificationKind.Punctuation, "..."));
                }

                texts.Add(new ClassifiedText(ClassificationKind.Punctuation, "}"));
            }
            else if (type is ClusterSymbol cs)
            {
                texts.Add(new ClassifiedText(ClassificationKind.Type, $"cluster('{cs.Name}')"));
            }
            else if (type is DatabaseSymbol db)
            {
                texts.Add(new ClassifiedText(ClassificationKind.Type, $"database('{db.Name}')"));
            }
            else if (type != null)
            {
                texts.Add(new ClassifiedText(ClassificationKind.Type, type.Display));
            }
        }
    }
}