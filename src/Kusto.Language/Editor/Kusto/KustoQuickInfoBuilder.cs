using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Kusto.Language.Symbols;
    using Syntax;
    using System.Text;
    using Utils;
    using static KustoServiceHelpers;

    internal class KustoQuickInfoBuilder
    {
        private readonly KustoCodeService _service;
        private readonly KustoCode _code;
        private readonly QuickInfoOptions _options;

        public KustoQuickInfoBuilder(KustoCodeService service, KustoCode code, QuickInfoOptions options)
        {
            _service = service;
            _code = code;
            _options = options;
        }

        /// <summary>
        /// Gets the quick info for the item near the specified text position.
        /// </summary>
        public QuickInfo GetQuickInfo(int position, CancellationToken cancellationToken)
        {
            if (_code.HasSemantics)
            {
                var symbolInfo = GetSymbolInfo(position) 
                    ?? GetSyntaxInfo(position);

                var diagnosticInfo = _options.ShowDiagnostics
                    ? GetDiagnosticInfo(position, cancellationToken)
                    : null;

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
            var token = GetTokenWithAffinity(_code, position);

            if (token != null 
                && position >= token.TextStart
                && token.Parent is SyntaxNode node)
            {
                if (node.GetFirstAncestorOrSelf<Expression>() is Expression expr)
                {
                    if (expr.Parent is BracketedName bn
                        && bn.Parent is Expression bne)
                    {
                        // special case for ['column name']
                        expr = bne;
                    }

                    // skip up to function call to pick up referenced signature
                    if (expr.Parent is FunctionCallExpression fc)
                        expr = fc;

                    if (expr.IsLiteral
                        && expr.ResultType != null 
                        && expr.ResultType.IsScalar)
                    {
                        return GetSymbolInfo(
                            expr.ResultType, 
                            null, 
                            expr.ResultType, 
                            expr.ConstantValueInfo, 
                            QuickInfoKind.Literal);
                    }
                    else
                    {
                        return GetSymbolInfo(
                            expr.ReferencedSymbol, 
                            expr.ReferencedSignature, 
                            expr.ResultType, 
                            expr.ConstantValueInfo);
                    }
                }
                else if (node.ReferencedSymbol != null)
                {
                    return GetSymbolInfo(node.ReferencedSymbol, null, null, null);
                }
            }

            return null;
        }

        private QuickInfoItem GetSymbolInfo(
            Symbol symbol, 
            Signature signature,
            TypeSymbol type, 
            ValueInfo value, 
            QuickInfoKind? itemKind = null)
        {
            if (symbol != null)
            {
                var texts = new List<ClassifiedText>();

                GetSymbolDisplayText(
                    symbol, 
                    signature, 
                    type, 
                    value, 
                    texts);

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
                        || v.Type is GraphSymbol
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
                case GraphSymbol _:
                    return QuickInfoKind.Graph;
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
            // place holder for adding information for the current grammar
            return null;
        }

        private QuickInfoItem GetDiagnosticInfo(int position, CancellationToken cancellationToken)
        {
            var diagnostics = _service.GetCombinedDiagnostics(waitForAnalysis: false, filter: _options.DiagnosticFilter, cancellationToken: cancellationToken);

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

        /// <summary>
        /// Gets the classified display text for a symbol.
        /// </summary>
        private void GetSymbolDisplayText(
            Symbol symbol,
            Signature signature,
            TypeSymbol returnType,
            ValueInfo value,
            List<ClassifiedText> texts)
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
                    GetSymbolDisplayText(subsym, null, null, null, texts);
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
                GetSymbolDisplayText(v.Type, null, returnType, value, texts);
            }
            else if (symbol != null)
            {
                var nameKind = GetNameClassificationKind(symbol);
                texts.Add(new ClassifiedText(nameKind, symbol.Name));

                if (symbol is FunctionSymbol fs)
                {
                    GetSignatureDisplay(signature ?? fs.Signatures[0], texts);
                }

                if (returnType == null && symbol is VariableSymbol vs)
                {
                    returnType = vs.Type;
                }

                if (returnType != null && returnType != ScalarTypes.Unknown && !returnType.IsError)
                {
                    texts.Add(new ClassifiedText(ClassificationKind.Punctuation, ": "));
                    texts.Add(new ClassifiedText(ClassificationKind.Type, GetTypeText(returnType)));
                }

                if (value != null && value.RawText != null)
                {
                    texts.Add(new ClassifiedText(ClassificationKind.Punctuation, " = "));
                    var kind = value.Value is string
                        ? ClassificationKind.StringLiteral
                        : ClassificationKind.Literal;

                    var text = value.RawText;
                    if (text.Length > 40)
                        text = text.Substring(0, 40) + " ...";

                    texts.Add(new ClassifiedText(kind, text));
                }

                if (_options.ShowDescriptions)
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
                case EntityGroupSymbol eg:
                    return "Entity Group";
                case EntityGroupElementSymbol eges:
                    return GetDescription(eges.UnderlyingSymbol);
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
                case EntityGroupElementSymbol eges:
                    return GetNameClassificationKind(eges.UnderlyingSymbol);
                //case GraphSymbol _:
                //return ClassificationKind.Graph;
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

        private static void GetSignatureDisplay(Signature sig, List<ClassifiedText> texts)
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

        private static string GetTypeText(TypeSymbol symbol)
        {
            var builder = new StringBuilder();
            GetTypeText(symbol, builder, false);
            return builder.ToString();
        }

        private static void GetTypeText(TypeSymbol type, StringBuilder builder, bool nested)
        {
            switch (type)
            {
                case TableSymbol ts:
                    builder.Append("table(");
                    GetTypeSchemaText(ts.Columns, builder);
                    builder.Append(")");
                    break;

                case TupleSymbol tus:
                    builder.Append("tuple(");
                    GetTypeSchemaText(tus.Columns, builder);
                    builder.Append(")");
                    break;

                case ClusterSymbol cs:
                    builder.Append($"cluster({KustoFacts.GetSingleQuotedStringLiteral(cs.Name)})");
                    break;

                case DatabaseSymbol db:
                    builder.Append($"database({KustoFacts.GetSingleQuotedStringLiteral(db.Name)})");
                    break;

                case GraphSymbol gs:
                    builder.Append("graph(");
                    builder.Append("edge(");
                    GetTypeSchemaText(gs.EdgeShape.Columns, builder);
                    builder.Append(")");
                    if (gs.NodeShape != null)
                    {
                        builder.Append(", ");
                        builder.Append("node(");
                        GetTypeSchemaText(gs.NodeShape.Columns, builder);
                        builder.Append(")");
                    }
                    builder.Append(")");
                    break;

                case EntityGroupElementSymbol eges:
                    GetTypeText(eges.UnderlyingSymbol, builder, false);
                    break;

                case DynamicBagSymbol dbag:
                    if (!nested)
                        builder.Append("dynamic(");
                    builder.Append("bag(");
                    GetTypeSchemaText(dbag.Properties, builder);
                    builder.Append(")");
                    if (!nested)
                        builder.Append(")");
                    break;

                case DynamicArraySymbol darray:
                    if (!nested)
                        builder.Append("dynamic(");
                    builder.Append("array(");
                    GetTypeText(darray.ElementType, builder, true);
                    builder.Append(")");
                    if (!nested)
                        builder.Append(")");
                    break;

                case DynamicPrimitiveSymbol dprim:
                    if (!nested)
                        builder.Append("dynamic(");
                    GetTypeText(dprim.UnderlyingType, builder, true);
                    if (!nested)
                        builder.Append(")");
                    break;

                default:
                    if (type != null)
                    {
                        builder.Append(type.Name);
                    }
                    break;
            }
        }

        private static void GetTypeSchemaText(IReadOnlyList<ColumnSymbol> columns, StringBuilder builder)
        {
            var maxCol = Math.Min(MaxColumns, columns.Count);
            for (int i = 0; i < maxCol; i++)
            {
                if (i > 0)
                    builder.Append(", ");
                builder.Append(columns[i].Name);
            }

            if (maxCol < columns.Count)
            {
                builder.Append(", ...");
            }
        }
    }
}