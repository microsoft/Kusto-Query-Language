using System;
using System.Linq;
using System.Collections.Generic;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;

namespace Kusto.Language.Parsing
{
    using static Parsers<LexicalToken>;
    using static SyntaxParsers;
    using Q = QueryGrammar;
    using System.Text;

    /// <summary>
    /// All predefined rules used by command grammar parsers
    /// </summary>
    public class PredefinedRuleParsers
    {
        public Parser<LexicalToken, SyntaxElement> RawGuidLiteral { get; }
        public Parser<LexicalToken, SyntaxElement> GuidLiteral { get; }
        public Parser<LexicalToken, SyntaxElement> AnyGuidLiteralOrString { get; }
        public Parser<LexicalToken, SyntaxElement> StringLiteral { get; }
        public Parser<LexicalToken, SyntaxElement> ColumnNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> TableNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> ExternalTableNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> MaterializedViewNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> DatabaseNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> ClusterNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> DatabaseFunctionNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> DatabaseOrTableNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> DatabaseTableNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> DatabaseOrTableOrColumnNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> DatabaseTableColumnNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> TableOrColumnNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> TableColumnNameReference { get; }
        public Parser<LexicalToken, SyntaxElement> BracketedStringLiteral { get; }
        public Parser<LexicalToken, SyntaxElement> Value { get; }
        public Parser<LexicalToken, SyntaxElement> Type { get; }
        public Parser<LexicalToken, SyntaxElement> NameDeclarationOrStringLiteral { get; }
        public Parser<LexicalToken, SyntaxElement> WildcardedNameDeclaration { get; }
        public Parser<LexicalToken, SyntaxElement> QualifiedWildcardedNameDeclaration { get; }
        public Parser<LexicalToken, SyntaxElement> FunctionDeclaration { get; }
        public Parser<LexicalToken, SyntaxElement> FunctionBody { get; }
        public Parser<LexicalToken, SyntaxElement> CommandInput { get; }
        public Parser<LexicalToken, SyntaxElement> InputText { get; }
        public Parser<LexicalToken, SyntaxElement> BracketedInputText { get; }

        public Func<SyntaxElement> MissingStringLiteral { get; }
        public Func<SyntaxElement> MissingValue { get; }
        public Func<SyntaxElement> MissingType { get; }
        public Func<SyntaxElement> MissingNameReference { get; }
        public Func<SyntaxElement> MissingNameDeclaration { get; }
        public Func<SyntaxElement> MissingFunctionDeclaration { get; }
        public Func<SyntaxElement> MissingFunctionBody { get; }
        public Func<SyntaxElement> MissingExpression { get; }
        public Func<SyntaxElement> MissingInputText { get; }

        public PredefinedRuleParsers(
            QueryGrammar queryParser,
            Parser<LexicalToken, Expression> inputCommand)
        {
            // casts are required here, bridge.net has problems with covariant delegates
            this.MissingStringLiteral = () => (SyntaxElement)Q.MissingStringLiteral();
            this.MissingValue = () => (SyntaxElement)Q.MissingValue();
            this.MissingType = () => (SyntaxElement)Q.MissingType();
            this.MissingNameReference = () => (SyntaxElement)Q.MissingNameReference();
            this.MissingNameDeclaration = () => (SyntaxElement)Q.MissingNameDeclaration();
            this.MissingExpression = () => (SyntaxElement)Q.MissingExpression();

            this.MissingFunctionDeclaration = () =>
                 (SyntaxElement)new FunctionDeclaration(
                     null,
                     new FunctionParameters(
                         CreateMissingToken(SyntaxKind.OpenParenToken),
                         SyntaxList<SeparatedElement<FunctionParameter>>.Empty(),
                         CreateMissingToken(SyntaxKind.CloseParenToken)),
                     new FunctionBody(
                         CreateMissingToken(SyntaxKind.OpenBraceToken),
                         SyntaxList<SeparatedElement<Statement>>.Empty(),
                         null,
                         null,
                         CreateMissingToken(SyntaxKind.CloseBraceToken)));

            this.MissingFunctionBody = () =>
                (SyntaxElement)new FunctionBody(
                    CreateMissingToken(SyntaxKind.OpenBraceToken),
                    SyntaxList<SeparatedElement<Statement>>.Empty(),
                    null,
                    null,
                    CreateMissingToken(SyntaxKind.CloseBraceToken));

            this.MissingInputText = () =>
                    (SyntaxElement)SyntaxToken.Other("", "", SyntaxKind.InputTextToken);

            this.RawGuidLiteral =
                  Rule(
                      Token(SyntaxKind.RawGuidLiteralToken),
                      (token) => (SyntaxElement)new LiteralExpression(SyntaxKind.GuidLiteralExpression, token));

            this.GuidLiteral =
                  Rule(
                      Token(SyntaxKind.GuidLiteralToken),
                      (token) => (SyntaxElement)new LiteralExpression(SyntaxKind.GuidLiteralExpression, token));

            this.StringLiteral = queryParser.StringLiteral.Cast<SyntaxElement>();

            this.AnyGuidLiteralOrString =
                First(GuidLiteral, StringLiteral, RawGuidLiteral);

            var StringName =
                Rule(Token(SyntaxKind.StringLiteralToken), token => (Name)new TokenName(token));

            var Name =
                First(
                    queryParser.IdentifierName,
                    queryParser.BracketedName,
                    queryParser.BracedName,
                    StringName)
                    .WithTag("<name>");

            this.ColumnNameReference =
                Rule(Name, name => (SyntaxElement)new NameReference(name, SymbolMatch.Column))
                    .WithCompletionHint(Editor.CompletionHint.Column)
                    .WithTag("<column>");

            this.TableNameReference =
                Rule(Name, name => (SyntaxElement)new NameReference(name, SymbolMatch.Table))
                    .WithCompletionHint(Editor.CompletionHint.Table)
                    .WithTag("<table>");

            this.ExternalTableNameReference =
                Rule(Name, name => (SyntaxElement)new NameReference(name, SymbolMatch.ExternalTable))
                    .WithCompletionHint(Editor.CompletionHint.ExternalTable)
                    .WithTag("<externaltable>");

            this.MaterializedViewNameReference =
                Rule(Name, name => (SyntaxElement)new NameReference(name, SymbolMatch.MaterializedView))
                    .WithCompletionHint(Editor.CompletionHint.MaterializedView)
                    .WithTag("<materializedview>");

            this.DatabaseNameReference =
                Rule(Name, name => (SyntaxElement)new NameReference(name, SymbolMatch.Database))
                    .WithCompletionHint(Editor.CompletionHint.Database)
                    .WithTag("<database>");

            this.ClusterNameReference =
                Rule(Name, name => (SyntaxElement)new NameReference(name, SymbolMatch.Cluster))
                    .WithCompletionHint(Editor.CompletionHint.Cluster)
                    .WithTag("<cluster>");

            this.DatabaseFunctionNameReference =
                Rule(Name, name => (SyntaxElement)new NameReference(name, SymbolMatch.Function))
                    .WithCompletionHint(Editor.CompletionHint.DatabaseFunction)
                    .WithTag("<function>");

            this.DatabaseOrTableNameReference =
                Rule(Name, name => (SyntaxElement)new NameReference(name, SymbolMatch.Database | SymbolMatch.Table))
                    .WithCompletionHint(Editor.CompletionHint.Database | Editor.CompletionHint.Table)
                    .WithTag("<database_or_table>");

            this.DatabaseTableNameReference =
                ApplyZeroOrMore(
                    DatabaseOrTableNameReference,
                    _left =>
                        Rule(_left, Token(".").Hide(), Required(TableNameReference, MissingNameReference),
                            (expr, dot, selector) => (SyntaxElement)new PathExpression((Expression)expr, dot, (Expression)selector)))
                    .WithTag("<database_table>");

            this.DatabaseOrTableOrColumnNameReference =
                Rule(Name, name => (SyntaxElement)new NameReference(name, SymbolMatch.Database | SymbolMatch.Table | SymbolMatch.Column))
                    .WithCompletionHint(Editor.CompletionHint.Database | Editor.CompletionHint.Table | Editor.CompletionHint.Column)
                    .WithTag("<database_or_table_or_column>");

            this.DatabaseTableColumnNameReference =
                ApplyZeroOrMore(
                    DatabaseOrTableOrColumnNameReference,
                    _left =>
                        Rule(_left, Token(".").Hide(), Required(DatabaseOrTableOrColumnNameReference, MissingNameReference),
                            (expr, dot, selector) => (SyntaxElement)new PathExpression((Expression)expr, dot, (Expression)selector)))
                    .WithTag("<database_table_column>");

            this.TableOrColumnNameReference =
                Rule(Name, name => (SyntaxElement)new NameReference(name, SymbolMatch.Table | SymbolMatch.Column))
                    .WithCompletionHint(Editor.CompletionHint.Table | Editor.CompletionHint.Column)
                    .WithTag("<table_or_column>");

            this.TableColumnNameReference =
                ApplyZeroOrMore(
                    TableOrColumnNameReference,
                    _left =>
                        Rule(_left, Token(".").Hide(), Required(TableOrColumnNameReference, MissingNameReference),
                            (expr, dot, selector) => (SyntaxElement)new PathExpression((Expression)expr, dot, (Expression)selector)))
                    .WithTag("<table_column>");

            this.BracketedStringLiteral =
                Convert(
                    And(
                        Token("["),
                        ZeroOrMore(Match(t =>
                            t.Text != "]"
                            && t.Text != "["
                            && !TextFacts.HasLineBreaks(t.Trivia)
                            && !TextFacts.HasLineBreaks(t.Text))),
                        Optional(Token("]"))),
                    (IReadOnlyList<LexicalToken> list) =>
                    {
                        var text = string.Concat(list.Select(e => (e != list[0] ? e.Trivia : "") + e.Text));
                        return (SyntaxElement)new LiteralExpression(SyntaxKind.StringLiteralExpression,
                            SyntaxToken.Literal(list[0].Trivia, text, SyntaxKind.StringLiteralToken));
                    }).WithTag("<bracketed-string>");

            this.Value = First(GuidLiteral, RawGuidLiteral, queryParser.Literal.Cast<SyntaxElement>());

            this.Type = queryParser.ParamTypeExtended.Cast<SyntaxElement>();

            this.NameDeclarationOrStringLiteral =
                First(
                    queryParser.SimpleNameDeclarationExpression.Cast<SyntaxElement>(),
                    queryParser.StringLiteral.Cast<SyntaxElement>());

            this.WildcardedNameDeclaration =
                Rule(queryParser.WildcardedIdentifier,
                    id => (SyntaxElement)new NameDeclaration(new WildcardedName(id)));

            // either name.wildname or wildname
            this.QualifiedWildcardedNameDeclaration =
                First(
                    If(And(queryParser.SimpleNameDeclaration, Token("."), queryParser.WildcardedIdentifier),
                        Rule(queryParser.SimpleNameDeclaration, Token("."), Required(WildcardedNameDeclaration.Cast<Expression>(), Q.MissingNameReference),
                            (qual, dot, name) => (SyntaxElement)new PathExpression(qual, dot, name))),
                    WildcardedNameDeclaration.Cast<SyntaxElement>());

            this.FunctionDeclaration =
                Rule(
                    queryParser.FunctionParameters,
                    queryParser.FunctionBody,
                    (p, b) => (SyntaxElement)new FunctionDeclaration(null, p, b));

            this.FunctionBody =
                queryParser.FunctionBody.Cast<SyntaxElement>();

            this.CommandInput =
                First(
                    If(Token(SyntaxKind.DotToken),
                        inputCommand.Cast<SyntaxElement>()),
                    queryParser.StatementList.Cast<SyntaxElement>());

            var InputTextTokens = ZeroOrMore(AnyTokenButEnd);

            SourceProducer<LexicalToken, SyntaxToken> InputTextBuilder = (source, start, length) =>
            {
                if (length > 0)
                {
                    var builder = new StringBuilder();
                    var token = source.Peek(start);
                    var trivia = token.Trivia;
                    builder.Append(source.Peek(start).Text);

                    for (int i = 1; i < length; i++)
                    {
                        token = source.Peek(start + i);
                        builder.Append(token.Trivia);
                        builder.Append(token.Text);
                    }

                    return SyntaxToken.Other(trivia, builder.ToString(), SyntaxKind.InputTextToken);
                }
                else
                {
                    return SyntaxToken.Other("", "", SyntaxKind.InputTextToken);
                }
            };

            this.InputText =
                Convert(InputTextTokens, InputTextBuilder).Cast<SyntaxElement>();

            var BracketedInputTextTokens =
                ZeroOrMore(Not(Token("]")));

            this.BracketedInputText =
                Convert(BracketedInputTextTokens, InputTextBuilder).Cast<SyntaxElement>();
        }
    }
}