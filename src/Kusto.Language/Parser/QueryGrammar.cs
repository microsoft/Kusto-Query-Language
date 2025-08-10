using System;
using System.Linq;
using System.Collections.Generic;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using Kusto.Language.Editor;

namespace Kusto.Language.Parsing
{
    using static Parsers<LexicalToken>;
    using static SyntaxParsers;
    using Utils;

    /// <summary>
    /// Parsers for the Kusto query grammar.
    /// </summary>
    public class QueryGrammar
    {
        private QueryGrammar(ParseOptions options)
        {
            this.Initialize(options);
        }

        /// <summary>
        /// Gets the <see cref="QueryGrammar"/> associated with the specified <see cref="GlobalState"/>.
        /// </summary>
        public static QueryGrammar From(GlobalState globals)
        {
            var cg = s_currentGrammar;
            if (cg != null && cg.Options.EqualExceptForParseKind(globals.ParseOptions))
            {
                return cg.Grammar;
            }
            else
            {
                CachedGrammar newCachedGrammar;

                if (globals.Cache != null)
                {
                    // try to access/store grammar from globals cache if there is one
                    if (!globals.Cache.TryGetValue<CachedGrammar>(out newCachedGrammar))
                    {
                        newCachedGrammar = globals.Cache.GetOrCreate(() => 
                            new CachedGrammar(
                                new QueryGrammar(globals.ParseOptions),
                                globals.ParseOptions));
                    }
                }
                else
                {
                    // otherwise create a new grammar that corresponds the parse options
                    newCachedGrammar = new CachedGrammar(
                        new QueryGrammar(globals.ParseOptions),
                        globals.ParseOptions);
                }

                // store recent grammar instance so we it can be used later
                Interlocked.CompareExchange(ref s_currentGrammar, newCachedGrammar, cg);

                return newCachedGrammar.Grammar;
            }
        }

        private class CachedGrammar
        {
            public readonly QueryGrammar Grammar;
            public readonly ParseOptions Options;
            public CachedGrammar(QueryGrammar grammar, ParseOptions options)
            {
                this.Grammar = grammar;
                this.Options = options;
            }
        }

        private static CachedGrammar s_currentGrammar;

        public Parser<LexicalToken, QueryBlock> QueryBlock { get; private set; }
        public Parser<LexicalToken, Statement> Statement { get; private set; }
        public Parser<LexicalToken, Directive> Directive { get; private set; }
        public Parser<LexicalToken, SyntaxList<SeparatedElement<Statement>>> StatementList { get; private set; }
        public Parser<LexicalToken, FunctionBody> FunctionBody { get; private set; }
        public Parser<LexicalToken, FunctionParameters> FunctionParameters { get; private set; }
        public Parser<LexicalToken, QueryOperator> QueryOperator { get; private set; }
        public Parser<LexicalToken, Expression> PipeExpression { get; private set; }
        public Parser<LexicalToken, Expression> PipeSubExpression { get; private set; }
        public Parser<LexicalToken, SyntaxList<SeparatedElement<Statement>>> MacroExpandSubQuery { get; private set; }
        public Parser<LexicalToken, QueryOperator> FollowingPipeElementExpression { get; private set; }
        public Parser<LexicalToken, Expression> Expression { get; private set; }
        public Parser<LexicalToken, Expression> NamedExpression { get; private set; }
        public Parser<LexicalToken, Expression> UnnamedExpression { get; private set; }
        public Parser<LexicalToken, NameDeclaration> SimpleNameDeclaration { get; private set; }
        public Parser<LexicalToken, Expression> SimpleNameDeclarationExpression { get; private set; }
        public Parser<LexicalToken, NameDeclaration> BracketedNameDeclaration { get; private set; }
        public Parser<LexicalToken, Name> IdentifierName { get; private set; }
        public Parser<LexicalToken, Name> BracketedName { get; private set; }
        public Parser<LexicalToken, Name> BracedName { get; private set; }
        public Parser<LexicalToken, TypeExpression> ParamTypeExtended { get; private set; }
        public Parser<LexicalToken, SchemaTypeExpression> SchemaType { get; private set; }
        public Parser<LexicalToken, Expression> SimpleNameReference { get; private set; }
        public Parser<LexicalToken, Expression> WildcardedNameReference { get; private set; }
        public Parser<LexicalToken, SyntaxToken> WildcardedIdentifier { get; private set; }
        public Parser<LexicalToken, Expression> Literal { get; private set; }
        public Parser<LexicalToken, Expression> StringLiteral { get; private set; }
        public Parser<LexicalToken, Expression> JsonValue { get; private set; }
        public Parser<LexicalToken, SyntaxList<SeparatedElement<Expression>>> LiteralList { get; private set; }
        public Parser<LexicalToken, SkippedTokens> SkippedTokens { get; private set; }

        private static readonly HashSet<SyntaxKind> _extendedKeywordsAsIdentifiers = KustoFacts.ExtendedKeywordsAsIdentifiers.ToHashSetEx();

        private static bool IsExtendedKeywordAsIdentifier(LexicalToken token) =>
            token.Kind.IsKeyword() && _extendedKeywordsAsIdentifiers.Contains(token.Kind);

        private static bool IsKeywordAsIdentifier(LexicalToken token) =>
            token.Kind.GetCategory() == SyntaxCategory.Keyword && token.Kind.CanBeIdentifier();

        /// <summary>
        /// Constructs the grammar as a Parser
        /// </summary>
        private void Initialize(ParseOptions options)
        {
            #region Forwards
            Parser<LexicalToken, Expression> ExpressionCore = null;
            Parser<LexicalToken, Expression> UnnamedExpressionCore = null;
            Parser<LexicalToken, NameAndTypeDeclaration> NameAndTypeDeclarationCore = null;
            Parser<LexicalToken, Expression> LiteralCore = null;
            Parser<LexicalToken, Expression> StringOrCompoundStringLiteralCore = null;
            Parser<LexicalToken, Expression> JsonValueCore = null;
            Parser<LexicalToken, Expression> PrimaryExpressionCore = null;
            Parser<LexicalToken, Expression> FunctionCallOrPathCore = null;
            Parser<LexicalToken, QueryOperator> ForkPipeOperatorCore = null;
            Parser<LexicalToken, Expression> ForkPipeExpressionCore = null;
            Parser<LexicalToken, Statement> LetStatementCore = null;
            Parser<LexicalToken, Statement> DeclareQueryParametersStatementCore = null;
            Parser<LexicalToken, FunctionParameter> FunctionParameterCore = null;
            Parser<LexicalToken, Expression> PipeExpressionCore = null;
            Parser<LexicalToken, Expression> PipeSubExpressionCore = null;
            Parser<LexicalToken, Expression> ContextualSubExpressionCore = null;

            this.Expression =
                Forward(() => ExpressionCore)
                .WithTag("<expression>");

            this.UnnamedExpression =
                Forward(() => UnnamedExpressionCore)
                .WithTag("<expression>");

            var NameAndTypeDeclaration =
                Forward(() => NameAndTypeDeclarationCore)
                .WithTag("<name-and-type>");

            this.Literal =
                Forward(() => LiteralCore)
                .WithTag("<literal>");

            var StringOrCompoundStringLiteral =
                Forward(() => StringOrCompoundStringLiteralCore)
                .WithTag("<string-literal>");

            this.JsonValue =
                Forward(() => JsonValueCore)
                .WithTag("<json-value>");

            var PrimaryExpression =
                Forward(() => PrimaryExpressionCore)
                .WithTag("<primary-expression>");

            var FunctionCallOrPath =
                Forward(() => FunctionCallOrPathCore)
                .WithTag("<function-call-or-path>");

            var ForkPipeOperator =
                Forward(() => ForkPipeOperatorCore)
                .WithTag("<fork-pipe-operator>");

            var ForkPipeExpression =
                Forward(() => ForkPipeExpressionCore)
                .WithTag("<fork-pipe-expression>");

            this.PipeExpression =
                Forward(() => PipeExpressionCore)
                .WithTag("<pipe-expression>");

            this.PipeSubExpression =
                Forward(() => PipeSubExpressionCore)
                .WithTag("<pipe-sub-expression>");

            this.MacroExpandSubQuery =
                Forward(() => StatementList)
                .WithTag("<macro-expand-subquery>");

            var ContextualSubExpression =
                Forward(() => ContextualSubExpressionCore)
                .WithTag("<contextual-sub-expression>");

            var LetStatement =
                Forward(() => LetStatementCore)
                .WithTag("<let>");

            var DeclareQueryParametersStatement =
                Forward(() => DeclareQueryParametersStatementCore)
                .WithTag("<query-parameters>");

            var FunctionParameter =
                Forward(() => FunctionParameterCore)
                .WithTag("<function-parameter>");
            #endregion

            #region Names
            var ScanIdentifierName =
                Token(SyntaxKind.IdentifierToken);

            var ScanExtendedKeywordAsIdentifier =
                Match(t => IsExtendedKeywordAsIdentifier(t))
                .WithTag("<extendedKeywordAsIdentifier>");

            var ScanKeywordAsIdentifier =
                Match(t => IsKeywordAsIdentifier(t))
                .WithTag("<keywordAsIdentifier>");

            var KeywordAsIdentifier =
                Match(t => IsKeywordAsIdentifier(t), t => SyntaxToken.From(t))
                .WithTag("<keywordAsIdentifier>");

            var IdentifierOrKeyword =
                First(Token(SyntaxKind.IdentifierToken), KeywordAsIdentifier);

            this.IdentifierName =
                Rule(IdentifierOrKeyword,
                    token => (Name)new TokenName(token));

            this.BracketedName =
                Rule(
                    Token(SyntaxKind.OpenBracketToken),
                    StringOrCompoundStringLiteral,
                    Token(SyntaxKind.CloseBracketToken),
                    (open, name, close) => (Name)new BracketedName(open, name, close));

            this.BracedName =
                // only match rule if name parts are adjacent (no whitespace)
                Match(
                    // consume (scan)
                    (Source<LexicalToken> source, int start) =>
                    {
                        var pos = start;

                        var token = source.Peek(pos);
                        if (token == null || token.Kind != SyntaxKind.OpenBraceToken)    
                            return -1;
                        pos++;

                        token = source.Peek(pos);
                        if (token == null
                            || (token.Kind != SyntaxKind.IdentifierToken && token.Kind.GetCategory() != SyntaxCategory.Keyword)
                            || token.Trivia.Length > 0)
                            return -1;
                        pos++;

                        token = source.Peek(pos);
                        if (token != null 
                            && token.Kind == SyntaxKind.OpenBracketToken
                            && token.Trivia.Length == 0)
                        {
                            pos++;

                            token = source.Peek(pos);
                            if (token != null
                                && token.Kind == SyntaxKind.MinusToken
                                && token.Trivia.Length == 0)
                                pos++;

                            token = source.Peek(pos);
                            if (token == null
                                || token.Kind != SyntaxKind.LongLiteralToken
                                || token.Trivia.Length > 0)
                                return -1;
                            pos++;

                            token = source.Peek(pos);
                            if (token == null
                                || token.Kind != SyntaxKind.CloseBracketToken
                                || token.Trivia.Length > 0)
                                return -1;
                            pos++;
                        }

                        token = source.Peek(pos);
                        if (token == null
                            || token.Kind != SyntaxKind.CloseBraceToken
                            || token.Trivia.Length > 0)
                            return -1;
                        pos++;

                        return pos - start;
                    },
                    // produce (convert)
                    (Source<LexicalToken> source, int start, int length) =>
                    {
                        var open = SyntaxToken.From(source.Peek(start));
                        var nameText = GetCombinedTokenText(source, start + 1, length - 2);
                        var nameToken = SyntaxToken.Identifier("", nameText);
                        var close = SyntaxToken.From(source.Peek(start + length - 1));
                        return (Name)new BracedName(open, nameToken, close);
                    });

            var IdentifierNameDeclaration =
                Rule(
                    Token(SyntaxKind.IdentifierToken),
                    id => (NameDeclaration)new NameDeclaration(id))
                    .WithTag("<identifer>");

            var IdentifierNameReference =
                Rule(
                    Token(SyntaxKind.IdentifierToken),
                    id => (Expression)new NameReference(id))
                    .WithTag("<identifer>");

            var ClientParameterReference =
                Rule(
                    BracedName,
                    name => (Expression)new NameReference(name, SymbolMatch.None))
                .WithTag("<client-parameter>");

            var ScanBracketedName =
                And(Token(SyntaxKind.OpenBracketToken),
                    OneOrMore(Token(SyntaxKind.StringLiteralToken)),
                    Optional(Token(SyntaxKind.CloseBracketToken)));

            this.BracketedNameDeclaration =
                Rule(
                    Token(SyntaxKind.OpenBracketToken),
                    Required(StringOrCompoundStringLiteral, CreateMissingStringLiteral),
                    RequiredToken(SyntaxKind.CloseBracketToken),
                    (openBracket, name, closeBracket) =>
                        (NameDeclaration)new NameDeclaration(new BracketedName(openBracket, name, closeBracket)));

            var BracketedNameReference =
                Rule(
                    Token(SyntaxKind.OpenBracketToken),
                    Required(StringOrCompoundStringLiteral, CreateMissingStringLiteral),
                    RequiredToken(SyntaxKind.CloseBracketToken),
                    (openBracket, name, closeBracket) =>
                        (Expression)new NameReference(new BracketedName(openBracket, name, closeBracket)));

            var KeywordAsIdentifierNameDeclaration =
                AsIdentifierNameDeclaration(KeywordAsIdentifier)
                .WithTag("<keywordAsIdentifier>");


            var ExtendedKeywordAsIdentifierToken =
                Match(t => IsExtendedKeywordAsIdentifier(t), lt => SyntaxToken.From(lt));

            var ExtendedKeywordAsIdentifierNameDeclaration =
                AsIdentifierNameDeclaration(ExtendedKeywordAsIdentifierToken)
                .WithTag("<extendedKeywordAsIdentifierNameDeclaration>");

            var KeywordAsIdentifierNameReference =
                AsIdentifierNameReference(KeywordAsIdentifier)
                .WithTag("<keywordAsIdentifierNameReference>");

            var ExtendedKeywordAsIdentifierNameReference =
                AsIdentifierNameReference(ExtendedKeywordAsIdentifierToken)
                .WithTag("<extendedKeywordAsIdentifierNameReference>");

            var NameTokenLiteral =
                AsTokenLiteral(First(Token(SyntaxKind.IdentifierToken), KeywordAsIdentifier));

            var ScanSimpleName =
                Or(ScanIdentifierName, ScanBracketedName, ScanKeywordAsIdentifier);

            var ScanExtendedName =
                Or(ScanIdentifierName, ScanBracketedName, ScanExtendedKeywordAsIdentifier);

            this.SimpleNameDeclaration =
                First(
                    IdentifierNameDeclaration,
                    BracketedNameDeclaration,
                    KeywordAsIdentifierNameDeclaration)
                .WithTag("<name>");

            this.SimpleNameDeclarationExpression =
                Rule(SimpleNameDeclaration, nd => (Expression)nd);

            this.SimpleNameReference =
                First(
                    IdentifierNameReference,
                    BracketedNameReference,
                    KeywordAsIdentifierNameReference,
                    ClientParameterReference)
                .WithTag("<name>");

            var ExtendedNameDeclaration =
                First(
                    IdentifierNameDeclaration,
                    BracketedNameDeclaration,
                    ExtendedKeywordAsIdentifierNameDeclaration)
                .WithTag("<name>");

            var ExtendedNameDeclarationExpression =
                Rule(ExtendedNameDeclaration, nd => (Expression)nd);

            var ExtendedNameReference =
                First(
                    IdentifierNameReference,
                    BracketedNameReference,
                    ExtendedKeywordAsIdentifierNameReference,
                    ClientParameterReference)
                .WithTag("<name>");

            var InvalidKeywordAsNameReference =
                Match(
                    (source, start) =>
                        QueryParser.IsKeywordInNamePosition(source, start) ? 1 : -1,
                    (source, start, length) =>
                        (Expression)new NameReference(
                            new TokenName(SyntaxToken.From(source.Peek(start))),
                            new[] { DiagnosticFacts.GetNameRequiresBrackets(source.Peek(start).Text) })
                    );

            var DashedName =
                Match((source, start) =>
                    QueryParser.ScanDashedName(source, start),
                (source, start, length) =>
                    new TokenName(SyntaxParsers.ProduceSyntaxToken(source, start, length))
                );


            #endregion

            #region Schema and Types
            var ParamTypeToken =
                First(
                    Token(SyntaxKind.BoolKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.BooleanKeyword).Hide(),
                    Token(SyntaxKind.DateKeyword).Hide(),
                    Token(SyntaxKind.DateTimeKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.DecimalKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.DoubleKeyword).Hide(),
                    Token(SyntaxKind.DynamicKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.GuidKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.IntKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.Int64Keyword).Hide(),
                    Token(SyntaxKind.Int8Keyword).Hide(),
                    Token(SyntaxKind.LongKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.RealKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.StringKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.TimeKeyword).Hide(),
                    Token(SyntaxKind.TimespanKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.UniqueIdKeyword).Hide()
                    );

            var ParamType =
                AsPrimitiveTypeExpression(ParamTypeToken).WithTag("<param-type>");

            var ParamTypeExtendedToken =
                First(
                    Token(SyntaxKind.BoolKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.BooleanKeyword).Hide(),
                    Token(SyntaxKind.DateKeyword).Hide(),
                    Token(SyntaxKind.DateTimeKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.DecimalKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.DoubleKeyword).Hide(),
                    Token(SyntaxKind.DynamicKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.FloatKeyword).Hide(),
                    Token(SyntaxKind.GuidKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.IntKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.Int16Keyword).Hide(),
                    Token(SyntaxKind.Int32Keyword).Hide(),
                    Token(SyntaxKind.Int64Keyword).Hide(),
                    Token(SyntaxKind.Int8Keyword).Hide(),
                    Token(SyntaxKind.LongKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.RealKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.DecimalKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.StringKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.TimeKeyword).Hide(),
                    Token(SyntaxKind.TimespanKeyword, CompletionKind.ScalarType),
                    Token(SyntaxKind.UIntKeyword).Hide(),
                    Token(SyntaxKind.UInt16Keyword).Hide(),
                    Token(SyntaxKind.UInt32Keyword).Hide(),
                    Token(SyntaxKind.UInt64Keyword).Hide(),
                    Token(SyntaxKind.UInt8Keyword).Hide(),
                    Token(SyntaxKind.ULongKeyword).Hide(),
                    Token(SyntaxKind.UniqueIdKeyword).Hide()
                    );

            this.ParamTypeExtended =
                AsPrimitiveTypeExpression(ParamTypeExtendedToken);

            var IdentifierTypeExpression =
                AsPrimitiveTypeExpression(
                    First(Token(SyntaxKind.IdentifierToken), KeywordAsIdentifier));

            var InvalidParamType =
                Convert(
                    First(
                        ParamTypeExtendedToken,
                        Token(SyntaxKind.IdentifierToken),
                        Match(tk => tk.Kind.IsKeyword())),
                    (token) => (TypeExpression)new PrimitiveTypeExpression(SyntaxToken.From(token), new[] { DiagnosticFacts.GetInvalidTypeName(token.Text) }));

            var ScanSchemaTypeStart =
                And(
                    ScanExtendedName,
                    Token(SyntaxKind.ColonToken),
                    Token(SyntaxKind.OpenParenToken));

            var StarExpression =
                Rule(
                    Token(SyntaxKind.AsteriskToken).Hide(),
                    (star) => (Expression)new StarExpression(star));

            var SchemaAsteriskType =
                Rule(
                    Token(SyntaxKind.OpenParenToken),
                    StarExpression,
                    Token(SyntaxKind.CloseParenToken),

                    (openParen, star, closeParen) =>
                        new SchemaTypeExpression(
                            openParen,
                            new SyntaxList<SeparatedElement<Expression>>(new SeparatedElement<Expression>(star)),
                            closeParen));

            var SchemaMultipartType =
                    Rule(
                        Token(SyntaxKind.OpenParenToken),
                        CommaList<Expression>(
                            Rule(NameAndTypeDeclaration, nat => (Expression)nat),
                            CreateMissingNameAndTypeDeclaration,
                            allowTrailingComma: true),
                        RequiredToken(SyntaxKind.CloseParenToken),

                        (openParen, columns, closeParen) =>
                            new SchemaTypeExpression(openParen, columns, closeParen));

            this.SchemaType =
                First(
                    If(And(Token(SyntaxKind.OpenParenToken), Token(SyntaxKind.AsteriskToken)), SchemaAsteriskType),
                    SchemaMultipartType);

            NameAndTypeDeclarationCore =
                First(
                    Rule( // error case: missing name
                        Token(SyntaxKind.ColonToken),
                        Required(First(ParamType, InvalidParamType), CreateMissingType),
                        (colon, type) => new NameAndTypeDeclaration(CreateMissingNameDeclaration(), colon, type)),

                    If(ScanSchemaTypeStart,
                        Rule(
                            ExtendedNameDeclaration,
                            Token(SyntaxKind.ColonToken),
                            SchemaType,
                            (name, colon, type) => new NameAndTypeDeclaration(name, colon, type))),

                    Rule(
                        ExtendedNameDeclaration,
                        RequiredToken(SyntaxKind.ColonToken),
                        Required(First(ParamType, InvalidParamType), CreateMissingType),
                        (name, colon, type) => new NameAndTypeDeclaration(name, colon, type)));
            #endregion

            #region Literals
            var BooleanLiteral =
                Rule(
                    Token(SyntaxKind.BooleanLiteralToken),
                    token => (Expression)new LiteralExpression(SyntaxKind.BooleanLiteralExpression, token))
                .WithTag("<bool-literal>");

            var BooleanLiteralWithCompletion =
                First(
                    If(Or(Token("true"), Token("false")), BooleanLiteral), // completion will see ScanToken's
                    BooleanLiteral);

            var LongLiteral =
                Rule(
                    Token(SyntaxKind.LongLiteralToken),
                    token => (Expression)new LiteralExpression(SyntaxKind.LongLiteralExpression, token))
                .WithTag("<long-literal>")
                .WithCompletion(new CompletionItem(CompletionKind.ScalarPrefix, "long()", "long(", ")", "long", rank: CompletionRank.Function));

            var RealLiteral =
                Rule(
                    Token(SyntaxKind.RealLiteralToken),
                    token => (Expression)new LiteralExpression(SyntaxKind.RealLiteralExpression, token))
                .WithTag("<real-literal>")
                .WithCompletion(
                    new CompletionItem(CompletionKind.ScalarPrefix, "real()", "real(", ")", "real", rank: CompletionRank.Function),
                    new CompletionItem(CompletionKind.ScalarPrefix, "double()", "double(", ")", "double", rank: CompletionRank.Function));

            var DecimalLiteral =
                Rule(
                    Token(SyntaxKind.DecimalLiteralToken),
                    token => (Expression)new LiteralExpression(SyntaxKind.DecimalLiteralExpression, token))
                .WithTag("<decimal-literal>")
                .WithCompletion(new CompletionItem(CompletionKind.ScalarPrefix, "decimal()", "decimal(", ")", "decimal", rank: CompletionRank.Function));

            var IntLiteral =
                Rule(
                    Token(SyntaxKind.IntLiteralToken),
                    token => (Expression)new LiteralExpression(SyntaxKind.IntLiteralExpression, token))
                .WithTag("<int-literal>")
                .WithCompletion(new CompletionItem(CompletionKind.ScalarPrefix, "int()", "int(", ")", "int", rank: CompletionRank.Function));

            var GuidLiteral =
                Rule(
                    Token(SyntaxKind.GuidLiteralToken),
                    token => (Expression)new LiteralExpression(SyntaxKind.GuidLiteralExpression, token))
                .WithTag("<guid-literal>")
                .WithCompletion(new CompletionItem(CompletionKind.ScalarPrefix, "guid()", "guid(", ")", "guid", rank: CompletionRank.Function));

            var RawGuidLiteral =
                Rule(
                    Token(SyntaxKind.RawGuidLiteralToken),
                    token => (Expression)new LiteralExpression(SyntaxKind.GuidLiteralExpression, token))
                .WithTag("<raw-guid-literal>");

            var DateTimeLiteral =
                Rule(Token(SyntaxKind.DateTimeLiteralToken),
                    token => (Expression)new LiteralExpression(SyntaxKind.DateTimeLiteralExpression, token))
                .WithTag("<datetime-literal>")
                .WithCompletion(new CompletionItem(CompletionKind.ScalarPrefix, "datetime()", "datetime(", ")", "datetime", rank: CompletionRank.Function));

            var TimespanLiteral =
                Rule(Token(SyntaxKind.TimespanLiteralToken),
                    token => (Expression)new LiteralExpression(SyntaxKind.TimespanLiteralExpression, token))
                .WithTag("<timespan-literal>")
                .WithCompletion(new CompletionItem(CompletionKind.ScalarPrefix, "timespan()", "timespan(", ")", "timespan", rank: CompletionRank.Function));

            this.StringLiteral =
                Rule(
                    Token(SyntaxKind.StringLiteralToken),
                    token => (Expression)new LiteralExpression(SyntaxKind.StringLiteralExpression, token))
                .WithTag("<string-literal>");

            var TypeofElement =
                First(
                    Rule(NameAndTypeDeclaration, nat => (Expression)nat),
                    StarExpression);

            var ScanTypeOfScalar =
                And(Token(SyntaxKind.TypeOfKeyword).Hide(),
                    Token(SyntaxKind.OpenParenToken),
                    Or(
                        ParamType,
                        ParamTypeExtended.Hide(),
                        And(Token(SyntaxKind.IdentifierToken).Hide(), Token(SyntaxKind.CloseParenToken))));

            var ScanTypeOfTabular =
                And(Token(SyntaxKind.TypeOfKeyword).Hide(), Token(SyntaxKind.OpenParenToken));

            var TypeofLiteral =
                First(
                    If(ScanTypeOfScalar,
                        Rule(
                            Token(SyntaxKind.TypeOfKeyword).Hide(),
                            Token(SyntaxKind.OpenParenToken),
                            First(ParamType, ParamTypeExtended.Hide(), IdentifierTypeExpression.Hide()),
                            RequiredToken(SyntaxKind.CloseParenToken),
                            (keyword, openParen, type, closeParen) =>
                                (Expression)new TypeOfLiteralExpression(
                                    keyword,
                                    openParen,
                                    new SyntaxList<SeparatedElement<Expression>>(new SeparatedElement<Expression>(type)),
                                    closeParen))),
                    If(ScanTypeOfTabular,
                        Rule(
                            Token(SyntaxKind.TypeOfKeyword).Hide(),
                            RequiredToken(SyntaxKind.OpenParenToken),
                            CommaList(TypeofElement, CreateMissingType, oneOrMore: true),
                            RequiredToken(SyntaxKind.CloseParenToken),
                            (keyword, openParen, list, closeParen) =>
                                (Expression)new TypeOfLiteralExpression(keyword, openParen, list, closeParen)
                            )))
                .WithTag("<typeof-literal>");

            StringOrCompoundStringLiteralCore =
                OneOrMore(
                    Token(SyntaxKind.StringLiteralToken),
                    list => list.Count == 1
                        ? (Expression)new LiteralExpression(SyntaxKind.StringLiteralExpression, list[0])
                        : new CompoundStringLiteralExpression(new SyntaxList<SyntaxToken>(list)))
                .WithTag("<string-literal>");

            var IsSignedNumericLiteral =
                Match((source, start) =>
                {
                    var sign = source.Peek(start);
                    var number = source.Peek(start + 1);
                    if (sign != null && number != null
                        && (sign.Kind == SyntaxKind.PlusToken || sign.Kind == SyntaxKind.MinusToken)
                        && number.Trivia.Length == 0
                        && (number.Kind == SyntaxKind.LongLiteralToken
                            || number.Kind == SyntaxKind.RealLiteralToken
                            || number.Kind == SyntaxKind.TimespanLiteralToken)
                        && number.Text.Length > 0 && char.IsDigit(number.Text[0]))
                    {
                        return 2;
                    }
                    else
                    {
                        return -1;
                    }
                });

            var SignedNumericLiteral =
                If(IsSignedNumericLiteral,
                    Rule(
                        First(Token(SyntaxKind.MinusToken), Token(SyntaxKind.PlusToken)).Hide(),
                        First(LongLiteral, RealLiteral, TimespanLiteral),
                        (sign, expr) =>
                        {
                            // combine sign and literal into single literal token and expression
                            var lit = (LiteralExpression)expr;
                            var combinedToken = SyntaxToken.Literal(sign.Trivia, sign.Text + lit.Token.Text, lit.Token.Kind);
                            return (Expression)new LiteralExpression(lit.Kind, combinedToken);
                        }));

            var NumericLiteral =
                First(
                    LongLiteral,
                    IntLiteral,
                    RealLiteral,
                    DateTimeLiteral,
                    TimespanLiteral,
                    SignedNumericLiteral)
                .WithTag("<numeric-constant>");

            var IdentifierTokenLiteral =
                AsTokenLiteral(Token(SyntaxKind.IdentifierToken)).WithTag("<identifier>");

            var KeywordTokenLiteral =
                AsTokenLiteral(KeywordAsIdentifier).WithTag("<keyword>");

            var IdentifierOrKeywordTokenLiteral =
                First(IdentifierTokenLiteral, KeywordTokenLiteral);

            var NullLiteralExpression =
                Rule(
                    Token("null"),
                    (token) => (Expression)new LiteralExpression(SyntaxKind.NullLiteralExpression, token))
                .WithCompletion(new CompletionItem(CompletionKind.Syntax, "null"));

            var JsonPair =
                First(
                    Rule(Token(SyntaxKind.StringLiteralToken), RequiredToken(SyntaxKind.ColonToken), Required(JsonValue, CreateMissingJsonValue),
                        (name, colon, value) =>
                            new JsonPair(name, colon, value)),
                    Rule(Token(SyntaxKind.ColonToken).Hide(), Required(JsonValue, CreateMissingJsonValue),
                        (colonToken, value) =>
                            new JsonPair(CreateMissingToken(SyntaxKind.StringLiteralToken, DiagnosticFacts.GetMissingString()), colonToken, value)),
                    Rule(JsonValue.Hide(),
                        (value) =>
                            new JsonPair(CreateMissingToken(SyntaxKind.StringLiteralToken, DiagnosticFacts.GetMissingString()), CreateMissingToken(SyntaxKind.ColonToken), value)))
                .WithTag("<json-pair>");

            var JsonObject =
                Rule(
                    Token(SyntaxKind.OpenBraceToken),
                    CommaList(JsonPair, CreateMissingJsonPair),
                    RequiredToken(SyntaxKind.CloseBraceToken),
                    (openBrace, list, closeBrace) =>
                        (Expression)new JsonObjectExpression(openBrace, list, closeBrace))
                .WithTag("<json-object>");

            var JsonArray =
                Rule(
                    Token(SyntaxKind.OpenBracketToken),
                    CommaList(JsonValue, CreateMissingJsonValue),
                    RequiredToken(SyntaxKind.CloseBracketToken),
                    (openBracket, values, closeBracket) =>
                        (Expression)new JsonArrayExpression(openBracket, values, closeBracket))
                .WithTag("<json-array>");

            var DynamicLiteral =
                Rule(
                    Token(SyntaxKind.DynamicKeyword, CompletionKind.ScalarPrefix),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Required(First(NullLiteralExpression, JsonValue), CreateMissingJsonValue),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (dynamicKeyword, openParen, value, closeParen) =>
                        (Expression)new DynamicExpression(dynamicKeyword, openParen, value, closeParen))
                .WithTag("<dynamic-literal>")
                .WithCompletion(new CompletionItem(CompletionKind.ScalarPrefix, "dynamic()", "dynamic(", ")", "dynamic", rank: CompletionRank.Function));

            var JsonNumber =
                First(
                    Rule(
                        Token(SyntaxKind.MinusToken).Hide(),
                        First(LongLiteral, RealLiteral),
                        (unaryOp, value) => (Expression)new PrefixUnaryExpression(SyntaxKind.UnaryMinusExpression, unaryOp, value)),
                    LongLiteral,
                    RealLiteral);

            JsonValueCore =
                First(
                    StringOrCompoundStringLiteral,
                    JsonNumber,
                    TimespanLiteral,
                    BooleanLiteral,
                    DateTimeLiteral,
                    GuidLiteral,
                    DecimalLiteral,
                    DynamicLiteral,
                    NullLiteralExpression,
                    JsonObject,
                    JsonArray,
                    ClientParameterReference);

            LiteralCore =
                First(
                    StringOrCompoundStringLiteral,
                    BooleanLiteral,
                    LongLiteral,
                    RealLiteral,
                    DecimalLiteral,
                    IntLiteral,
                    GuidLiteral,
                    Rule(Token(SyntaxKind.RawGuidLiteralToken),
                        tk => (Expression)new LiteralExpression(SyntaxKind.GuidLiteralExpression, tk, new[] { DiagnosticFacts.GetRawGuidLiteralNotAllowed() })),
                    DateTimeLiteral,
                    TimespanLiteral,
                    SignedNumericLiteral,
                    DynamicLiteral,
                    TypeofLiteral,
                    ClientParameterReference)
                .WithTag("<literal>");

            var ForcedSignedRealLiteral =
                If(IsSignedNumericLiteral,
                    Rule(
                        First(Token(SyntaxKind.MinusToken), Token(SyntaxKind.PlusToken)).Hide(),
                        First(LongLiteral, RealLiteral),
                        (sign, expr) =>
                        {
                            // combine sign and literal into single literal token and expression
                            var lit = (LiteralExpression)expr;
                            var combinedToken = SyntaxToken.Literal(sign.Trivia, sign.Text + lit.Token.Text, lit.Token.Kind);
                            return (Expression)new LiteralExpression(SyntaxKind.RealLiteralExpression, combinedToken);
                        }));

            // a numeric literal that is always encoded as a double/real literal expression
            var ForcedRealLiteral =
                First(
                    Rule(
                        First(Token(SyntaxKind.LongLiteralToken), Token(SyntaxKind.IntLiteralToken), Token(SyntaxKind.RealLiteralToken), Token(SyntaxKind.DecimalLiteralToken)),
                        token => (Expression)new LiteralExpression(SyntaxKind.RealLiteralExpression, token)),
                    ForcedSignedRealLiteral)
                .WithTag("<forced-real-literal>");

            #endregion

            #region Query Operator Parameters

            Parser<LexicalToken, NamedParameter> QParameter(
                Parser<LexicalToken, SyntaxToken> tokenParser,
                bool hasEquals,
                bool equalsNeeded,
                Parser<LexicalToken, Expression> valueParser,
                Func<Source<LexicalToken>, int, Expression> fnMissingValue = null,
                CompletionHint expressionHint = CompletionHint.None)
            {
                if (hasEquals)
                {
                    if (equalsNeeded)
                    {
                        return
                            If(And(tokenParser, Token(SyntaxKind.EqualToken)),
                                Rule(
                                    AsIdentifierNameDeclaration(tokenParser),
                                    RequiredToken(SyntaxKind.EqualToken),
                                    Required(valueParser, fnMissingValue ?? CreateMissingValue),
                                    (name, equal, value) =>
                                        new NamedParameter(name, equal, value, expressionHint)));
                    }
                    else
                    {
                        return Rule(
                            AsIdentifierNameDeclaration(tokenParser),
                            RequiredToken(SyntaxKind.EqualToken),
                            Required(valueParser, fnMissingValue ?? CreateMissingValue),
                            (name, equal, value) =>
                                new NamedParameter(name, equal, value, expressionHint));
                    }
                }
                else
                {
                    return
                        Rule(
                            AsIdentifierNameDeclaration(tokenParser),
                            Required(valueParser, fnMissingValue ?? CreateMissingValue),
                            (name, value) =>
                                new NamedParameter(name, SyntaxToken.Missing(SyntaxKind.EqualToken), value, expressionHint));
                }
            }

            Parser<LexicalToken, Expression> NameReferenceList(Parser<LexicalToken, NameReference> nameParser) =>
                Rule(
                    OneOrMoreCommaList(nameParser, (source, start) => (NameReference)CreateMissingNameReference(source, start)),
                    list => (Expression)new NameReferenceList(list));

            var AnyQueryOperatorParameterValue =
                First(
                    Literal.Hide(),
                    IdentifierOrKeywordTokenLiteral,
                    SimpleNameReference);

            var AnyQueryOperatorParameterForcedRealValue =
                First(
                    ForcedRealLiteral.Hide(),
                    Literal.Hide(),
                    IdentifierOrKeywordTokenLiteral,
                    SimpleNameReference);

            Parser<LexicalToken, SyntaxToken> QueryParameterName(QueryOperatorParameter parameter)
            {
                Parser<LexicalToken, SyntaxToken> parser;

                if (parameter.IsHidden)
                {
                    parser = HiddenToken(parameter.Name);
                }
                else if (parameter.HasNoEquals)
                {
                    parser = Token(parameter.Name);
                }
                else if (parameter.ValueKind == QueryOperatorParameterValueKind.StringLiteral)
                {
                    parser = Token(parameter.Name, ctext: $"{parameter.Name}=\"|\"");
                }
                else
                {
                    parser = Token(parameter.Name, ctext: $"{parameter.Name}=");
                }

                // add aliases if any, but hide them from intellisense
                if (parameter.Aliases.Count > 0)
                {
                    parser = First(new[] { parser }.Concat(parameter.Aliases.Select(a => HiddenToken(a))).ToArray());
                }

                return parser;
            }

            Parser<LexicalToken, NamedParameter> QueryParameter(
                QueryOperatorParameter parameter,
                bool equalsNeeded = false,  // if true then equals is needed when deciding if the name starts a parameter (unless the parameter does not uses equals syntax)
                IReadOnlyList<QueryOperatorParameter> allParameters = null)
            {
                bool hasEquals = !parameter.HasNoEquals;

                switch (parameter.ValueKind)
                {
                    case QueryOperatorParameterValueKind.Any:
                        return QParameter(
                            QueryParameterName(parameter),
                            hasEquals, equalsNeeded,
                            AnyQueryOperatorParameterValue,
                            CreateMissingValue);
                    case QueryOperatorParameterValueKind.StringLiteral:
                        return QParameter(
                            QueryParameterName(parameter),
                            hasEquals, equalsNeeded,
                            AnyQueryOperatorParameterValue,
                            CreateMissingStringLiteral);
                    case QueryOperatorParameterValueKind.BoolLiteral:
                        return QParameter(
                            QueryParameterName(parameter),
                            hasEquals, equalsNeeded,
                            First(BooleanLiteralWithCompletion, AnyQueryOperatorParameterValue),
                            CreateMissingBooleanLiteral);
                    case QueryOperatorParameterValueKind.IntegerLiteral:
                    case QueryOperatorParameterValueKind.NumericLiteral:
                    case QueryOperatorParameterValueKind.SummableLiteral:
                        return QParameter(
                            QueryParameterName(parameter),
                            hasEquals, equalsNeeded,
                            AnyQueryOperatorParameterValue,
                            CreateMissingLongLiteral);
                    case QueryOperatorParameterValueKind.ForcedRealLiteral:
                        return QParameter(
                            QueryParameterName(parameter),
                            hasEquals, equalsNeeded,
                            AnyQueryOperatorParameterForcedRealValue,
                            CreateMissingRealLiteral);
                    case QueryOperatorParameterValueKind.ScalarLiteral:
                        return QParameter(
                            QueryParameterName(parameter),
                            hasEquals, equalsNeeded,
                            AnyQueryOperatorParameterValue);
                    case QueryOperatorParameterValueKind.String:
                        return QParameter(
                            QueryParameterName(parameter),
                            hasEquals, equalsNeeded,
                            FunctionCallOrPath);
                    case QueryOperatorParameterValueKind.Word:
                    case QueryOperatorParameterValueKind.WordOrNumber:
                        return parameter.Values.Count > 0
                            ? QParameter(
                                QueryParameterName(parameter),
                                hasEquals, equalsNeeded,
                                First(AsTokenLiteral(Token(parameter.Values)), AnyQueryOperatorParameterValue),
                                CreateMissingTokenLiteral(parameter.Values))
                            : QParameter(
                                QueryParameterName(parameter),
                                hasEquals, equalsNeeded,
                                AnyQueryOperatorParameterValue,
                                CreateMissingTokenLiteral("token"));
                    case QueryOperatorParameterValueKind.NameDeclaration:
                        return QParameter(
                            QueryParameterName(parameter),
                            hasEquals, equalsNeeded,
                            First(SimpleNameDeclarationExpression, AnyQueryOperatorParameterValue),
                            CreateMissingNameDeclarationExpression);
                    case QueryOperatorParameterValueKind.Column:
                        return QParameter(
                            QueryParameterName(parameter),
                            hasEquals, equalsNeeded,
                            First(SimpleNameReference, AnyQueryOperatorParameterValue),
                            CreateMissingNameReference,
                            expressionHint: CompletionHint.Column);
                    case QueryOperatorParameterValueKind.ColumnList:
                        var allParameterNames = allParameters.Select(p => p.Name).ToList();
                        var nameRule = If(Not(And(Token(allParameterNames), Token(SyntaxKind.EqualToken))), ExtendedNameReference.Cast<NameReference>());
                        var nameList = NameReferenceList(nameRule);
                        return QParameter(
                            QueryParameterName(parameter),
                            hasEquals, equalsNeeded,
                            First(nameList, AnyQueryOperatorParameterValue),
                            expressionHint: CompletionHint.Column);
                    default:
                        throw new InvalidOperationException($"Unhandled query operator parameter kind: {parameter.ValueKind}");
                }
            }

            IReadOnlyList<Parser<LexicalToken, NamedParameter>> GetQueryOperatorParameterParsers(IReadOnlyList<QueryOperatorParameter> parameters, AllowedNameKind allowedNames = AllowedNameKind.DeclaredOrKnown, bool equalsNeeded = false)
            {
                var paramParsers = parameters.Select(p => QueryParameter(p, equalsNeeded, parameters)).ToList();

                if (allowedNames != AllowedNameKind.DeclaredOnly)
                {
                    var additionalParameters = QueryOperatorParameters.AllParameters.Where(p => !parameters.Any(p2 => p.Name == p2.Name)).ToList();
                    foreach (var ap in additionalParameters)
                    {
                        paramParsers.Add(QueryParameter(ap, equalsNeeded, QueryOperatorParameters.AllParameters));
                    }
                }

                return paramParsers;
            }

            // constructs a parser for query operator parameter lists
            Parser<LexicalToken, SyntaxList<NamedParameter>> QueryParameterList(IReadOnlyList<QueryOperatorParameter> parameters, AllowedNameKind allowedNames = AllowedNameKind.DeclaredOrKnown, bool equalsNeeded = false)
            {
                var paramParsers = GetQueryOperatorParameterParsers(parameters, allowedNames, equalsNeeded);
                var first = First(paramParsers.ToArray());
                return List(first);
            };

            // constructs a parser for comma separated query operator parameter lists
            Parser<LexicalToken, SyntaxList<SeparatedElement<NamedParameter>>> QueryParameterCommaList(IReadOnlyList<QueryOperatorParameter> parameters, AllowedNameKind allowedNames = AllowedNameKind.DeclaredOrKnown, bool equalsNeeded = false)
            {
                var paramParsers = GetQueryOperatorParameterParsers(parameters, allowedNames, equalsNeeded);
                var first = First(paramParsers.ToArray());
                return CommaList(first, CreateMissingNamedParameter);
            };

            #endregion

            #region Expressions
            var ParenthesizedExpression =
                Rule(
                    Token(SyntaxKind.OpenParenToken),
                    Required(Expression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (openParen, expression, closeParen) =>
                        (Expression)new ParenthesizedExpression(openParen, expression, closeParen));

            var ScanRenameName = Or(
                ScanIdentifierName,
                ScanExtendedKeywordAsIdentifier,
                ScanBracketedName);

            var ScanRenameList =
                And(Token(SyntaxKind.OpenParenToken),
                    ZeroOrMore(And(ScanRenameName, Optional(Token(SyntaxKind.CommaToken)))),
                    Optional(Token(SyntaxKind.CloseParenToken)));

            var RenameNameDeclaration =
                First(
                    IdentifierNameDeclaration,
                    ExtendedKeywordAsIdentifierNameDeclaration,
                    BracketedNameDeclaration)
                .WithTag("<name>");

            var RenameName =
                ExtendedNameDeclaration;

            var RenameList =
                Rule(
                    Token(SyntaxKind.OpenParenToken),
                    CommaList(RenameName, CreateMissingNameDeclaration, oneOrMore: true),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (openParen, list, closeParen) =>
                        new RenameList(openParen, list, closeParen));

            this.NamedExpression =
                First(
                    If(And(RenameName, Token(SyntaxKind.EqualToken)),
                        Rule(RenameName, Token(SyntaxKind.EqualToken), Required(UnnamedExpression, CreateMissingExpression),
                            (name, equals, expr) =>
                                (Expression)new SimpleNamedExpression(name, equals, expr))),

                    // special case for invalid named-expression names
                    If(And(DashedName, Token(SyntaxKind.EqualToken)),
                        Rule(DashedName, Token(SyntaxKind.EqualToken), Required(UnnamedExpression, CreateMissingExpression),
                            (name, equals, expr) =>
                                (Expression)new SimpleNamedExpression(
                                    new NameDeclaration(name, new[] { DiagnosticFacts.GetNameRequiresBrackets(name.Name.Text) }),
                                    equals, expr))),

                    If(And(ScanRenameList, Token(SyntaxKind.EqualToken)),
                        Rule(RenameList, Token(SyntaxKind.EqualToken), Required(UnnamedExpression, CreateMissingExpression),
                            (list, equals, expr) =>
                                (Expression)new CompoundNamedExpression(list, equals, expr))),

                    UnnamedExpression)
                .WithTag("<expression>");

            var Argument =
                First(
                    If(And(Token(SyntaxKind.AsteriskToken), Or(Token(SyntaxKind.CloseParenToken), Token(SyntaxKind.CommaToken))),
                        StarExpression),
                    NamedExpression);

            var FunctionArgumentList =
                Rule(
                    Token(SyntaxKind.OpenParenToken),
                    CommaList(Argument, CreateMissingExpression),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (openParen, list, closeParen) => new ExpressionList(openParen, list, closeParen));

            var knownFunctionNames =
                Functions.All.Select(f => f.Name).Concat(
                Aggregates.All.Select(f => f.Name)).ToArray();

            // some built-in function names are not identifiers so we add the ones we know about here.
            var ScanKnownFunctionNames =
                Token(knownFunctionNames)
                .Hide();

            var KnownFunctionNameReference =
                AsIdentifierNameReference(Token(knownFunctionNames).Hide());

            var ScanFunctionCall =
                And(Or(ScanIdentifierName, ScanBracketedName, ScanKnownFunctionNames), Token(SyntaxKind.OpenParenToken));

            var FunctionCallNames =
                First(SimpleNameReference, KnownFunctionNameReference);

            var FunctionCall =
                Rule(
                    FunctionCallNames,
                    FunctionArgumentList,
                    (name, arguments) =>
                        (Expression)new FunctionCallExpression((NameReference)name, arguments))
                .WithTag("<FunctionCall>");

            var RequiredFunctionCall =
                Required(
                    First(
                        FunctionCall,
                        Rule(FunctionCallNames,
                            (name) => (Expression)new FunctionCallExpression((NameReference)name, CreateMissingArgumentList()))),
                    CreateMissingFunctionCallExpression);

            var DotCompositeFunctionCall =
                ApplyZeroOrMore(
                    FunctionCall,
                    _left =>
                        Rule(
                            _left,
                            Token(SyntaxKind.DotToken),
                            If(ScanFunctionCall, FunctionCall),
                            (left, op, right) =>
                                (Expression)new PathExpression(left, op, right)));

            var AtTokenSelector =
                Rule(Token(SyntaxKind.AtToken), token => (Expression)new AtExpression(token));

            var SpecialKeywordsAsIdentifierName =
                Rule(Token(KustoFacts.SpecialKeywordsAsIdentifiers),
                    tk => (Expression)new NameReference(new TokenName(tk))).Hide();

            // this is unquoted name after dot in dotted path
            var BarePathElementSelector =
                First(
                    AtTokenSelector,
                    IdentifierNameReference,
                    KeywordAsIdentifierNameReference,
                    SpecialKeywordsAsIdentifierName,
                    ClientParameterReference
                    );

            // wild cards can use any keyword (but will need an asterisk somewhere)
            var ScanWildcard =
                And(
                    First(
                        And(
                            Match(t => t.Kind == SyntaxKind.IdentifierToken || IsExtendedKeywordAsIdentifier(t)), // can have leading trivia
                            Match(t => t.Kind == SyntaxKind.AsteriskToken && (t.Trivia.Length == 0) || options.AllowNonAdjacentWildcardParts)),
                        Token(SyntaxKind.AsteriskToken)), // can have leading trivia
                    ZeroOrMore(
                        Match(t => (t.Kind == SyntaxKind.IdentifierToken
                                || t.Kind == SyntaxKind.LongLiteralToken
                                || t.Kind == SyntaxKind.AsteriskToken
                                || IsExtendedKeywordAsIdentifier(t))
                                && (t.Trivia.Length == 0 || options.AllowNonAdjacentWildcardParts))));

            this.WildcardedIdentifier =
                Convert(
                    ScanWildcard.Hide(),
                    (Source<LexicalToken> source, int start, int len) =>
                    {
                        var trivia = source.Peek(start).Trivia;
                        var text = SyntaxParsers.GetCombinedTokenText(source, start, len);
                        var valueText = options.AllowNonAdjacentWildcardParts
                            ? SyntaxParsers.GetCombinedTokenText(source, start, len, includeInnerTrivia: false)
                            : text;
                        return SyntaxToken.Identifier(trivia, text, valueText);
                    })
                .WithTag("<wildcard>");

            this.WildcardedNameReference =
                Rule(
                    WildcardedIdentifier,
                    (token) => (Expression)new NameReference(new WildcardedName(token)))
                .WithTag("<wildcard>");

            var ScanBracketedWildcardName =
                And(
                    Token(SyntaxKind.OpenBracketToken),
                    ScanWildcard);

            var BracketedWildcardedNameReference =
                Rule(
                    Token(SyntaxKind.OpenBracketToken),
                    WildcardedIdentifier,
                    RequiredToken(SyntaxKind.CloseBracketToken),
                    (open, wildcard, close) =>
                        (Expression)new NameReference(new BracketedWildcardedName(open, wildcard, close)))
                .WithTag("<bracketed-wildcard>");

            var InvocationExpression =
                First(
                    Rule(
                        Token(SyntaxKind.MinusToken).Hide(),
                        FunctionCallOrPath,
                        (op, expr) => (Expression)new PrefixUnaryExpression(SyntaxKind.UnaryMinusExpression, op, expr)),
                    Rule(
                        Token(SyntaxKind.PlusToken).Hide(),
                        FunctionCallOrPath,
                        (op, expr) => (Expression)new PrefixUnaryExpression(SyntaxKind.UnaryPlusExpression, op, expr)),
                    FunctionCallOrPath);

            var BracketedExpression =
                Rule(
                    Token(SyntaxKind.OpenBracketToken),
                    Required(UnnamedExpression, CreateMissingNameReference),
                    RequiredToken(SyntaxKind.CloseBracketToken),
                    (openBracket, expr, closeBracket) =>
                        (Expression)new BracketedExpression(openBracket, expr, closeBracket));

         

            var BracketedPathElementSelector =
                First(
                    If(ScanBracketedName, BracketedNameReference),
                    If(ScanBracketedWildcardName, BracketedWildcardedNameReference),
                    BracketedExpression);

            // this is a name after a dot in a dotted path
            var PathElementSelector =
                First(
                    BarePathElementSelector,
                    BracketedPathElementSelector);

            var BracketedEntityNamePathElementSelector =
                First(
                    If(ScanBracketedWildcardName, BracketedWildcardedNameReference),
                    BracketedNameReference);

            var PathElementSelectorOrFunctionCall =
                First(
                    If(ScanFunctionCall, FunctionCall),
                    PathElementSelector);

            var EntityPathExpression =
                ApplyZeroOrMore(
                    PathElementSelectorOrFunctionCall,
                    _left =>
                        First(
                            Rule(_left, Token(SyntaxKind.DotToken), Required(PathElementSelectorOrFunctionCall, CreateMissingNameReference),
                                (left, dot, selector) =>
                                    (Expression)new PathExpression(left, dot, selector)),
                            Rule(_left, BracketedPathElementSelector,
                                (left, right) =>
                                    (Expression)new ElementExpression(left, right))));

            var ScanQualifiedEntityStart =
                And(Or(
                    HiddenToken(Functions.Database.Name),
                    HiddenToken(Functions.Cluster.Name)),
                    Token(SyntaxKind.OpenParenToken));

            var SimplePathExpression =
                ApplyZeroOrMore(
                    PathElementSelector,
                    _left =>
                        First(
                            Rule(_left, Token(SyntaxKind.DotToken), Required(PathElementSelector, CreateMissingNameReference),
                                (left, dot, selector) =>
                                    (Expression)new PathExpression(left, dot, selector)),
                            Rule(_left, BracketedPathElementSelector,
                                (left, right) =>
                                    (Expression)new ElementExpression(left, right))));

            var ScanWildcardedEntityReferenceOrFunctionCall =
                Or(ScanWildcard, ScanFunctionCall);

            var SimpleOrWildcardedEntityReference =
                First(
                    WildcardedNameReference,
                    SimpleNameReference)
                .WithTag("<simple-or-wildcarded-entity>");

            var WildcardedEntityReferencePathSelector =
                First(
                    WildcardedNameReference,
                    BarePathElementSelector,
                    BracketedEntityNamePathElementSelector);

            // everything up until the dot-wildcard
            var WildcardedEntityPathRoot =
                ApplyZeroOrMore(
                    PathElementSelectorOrFunctionCall,
                    _left =>
                        First(
                            If(And(Token(SyntaxKind.DotToken), Not(ScanWildcard)),
                                Rule(_left, Token(SyntaxKind.DotToken), Required(PathElementSelectorOrFunctionCall, CreateMissingNameReference),
                                    (left, dot, selector) =>
                                        (Expression)new PathExpression(left, dot, selector))
                            ),
                            Rule(_left, BracketedPathElementSelector,
                                (left, right) =>
                                    (Expression)new ElementExpression(left, right))));

            var WildcardedEntityReference =
                First(
                    WildcardedNameReference,
                    ApplyOptional(
                        WildcardedEntityPathRoot,
                        _left =>
                            Rule(
                                _left,
                                Token(SyntaxKind.DotToken),
                                Required(WildcardedNameReference, CreateMissingNameReference),
                                (path, dot, selector) =>
                                    (Expression)new PathExpression(path, dot, selector))))
                .WithTag("<wildcarded-entity-reference>");

            var ToScalarExpression =
                Rule(
                    Token(SyntaxKind.ToScalarKeyword, CompletionKind.ScalarPrefix),
                    Optional(QueryParameter(QueryOperatorParameters.ToScalarKindParameter, equalsNeeded: false)),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Required(Expression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (name, kind, openParen, expression, closeParen) =>
                        (Expression)new ToScalarExpression(name, kind, openParen, expression, closeParen));

            var ToTableExpression =
                Rule(
                    Token(SyntaxKind.ToTableKeyword, CompletionKind.TabularPrefix).Hide(),
                    Optional(QueryParameter(QueryOperatorParameters.ToTableKindParameter, equalsNeeded: false)),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Required(Expression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (name, kind, openParen, expression, closeParen) =>
                        (Expression)new ToTableExpression(name, kind, openParen, expression, closeParen));

            FunctionCallOrPathCore =
                First(
                    ToTableExpression, // first to preempt being seen as function call

                    ApplyZeroOrMore(
                        First(
                            ToScalarExpression, // first to preempt being seen as function call
                            If(ScanFunctionCall, FunctionCall),
                            PrimaryExpression),

                        _left =>
                            First(
                                //If(And(Token(SyntaxKind.DotToken), ScanFunctionCall),
                                //    Rule(_left, Token(SyntaxKind.DotToken), FunctionCall,
                                //        (left, dot, fc) => (Expression)new PathExpression(left, dot, fc))),
                                Rule(_left, Token(SyntaxKind.DotToken), Required(PathElementSelectorOrFunctionCall, CreateMissingNameReference),
                                    (left, dot, selector) => (Expression)new PathExpression(left, dot, selector)),
                                Rule(_left, BracketedExpression,
                                    (left, right) => (Expression)new ElementExpression(left, right)))));

            var RequiredFunctionCallOrPath =
                Required(FunctionCallOrPath, CreateMissingExpression);

            var UnaryPlusOrMinus =
                First(
                    Rule(
                        Token(SyntaxKind.PlusToken).Hide(),
                        RequiredFunctionCallOrPath,
                        (op, expr) => (Expression)new PrefixUnaryExpression(SyntaxKind.UnaryPlusExpression, op, expr)),
                    Rule(
                        Token(SyntaxKind.MinusToken).Hide(),
                        RequiredFunctionCallOrPath,
                        (op, expr) => (Expression)new PrefixUnaryExpression(SyntaxKind.UnaryMinusExpression, op, expr)),
                    FunctionCallOrPath);

            var RequiredUnaryPlusOrMinus =
                Required(UnaryPlusOrMinus, CreateMissingExpression);

            var StringOperatorTokens =
                First(StringOperatorMap.Keys.Select(
                    k => IsTokenVisible(k)
                        ? SyntaxParsers.Token((SyntaxKind)k, CompletionKind.ScalarInfix, ctext: (string)(SyntaxFacts.GetText((SyntaxKind)k) + " \"|\""))
                        : SyntaxParsers.Token((SyntaxKind)k).Hide()
                        ).ToArray());

            var StringOperation =
                First(
                    Rule(
                        Token(SyntaxKind.AsteriskToken).Hide(),
                        StringOperatorTokens,
                        RequiredUnaryPlusOrMinus,
                        (left, op, right) =>
                            (Expression)new BinaryExpression(StringOperatorMap[op.Kind], new StarExpression(left), op, right)),
                    ApplyOptional(
                        UnaryPlusOrMinus,
                        _left =>
                            Rule(
                                _left,
                                StringOperatorTokens,
                                RequiredUnaryPlusOrMinus,
                                (left, op, right) =>
                                    (Expression)new BinaryExpression(StringOperatorMap[op.Kind], left, op, right))));

            var RequiredStringOperation =
                Required(StringOperation, CreateMissingExpression);

            var Multiplicative =
                ApplyZeroOrMore(StringOperation, _left =>
                    First(
                        Rule(_left, Token(SyntaxKind.AsteriskToken, CompletionKind.ScalarInfix), RequiredStringOperation,
                            (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.MultiplyExpression, left, op, right)),

                        Rule(_left, Token(SyntaxKind.SlashToken, CompletionKind.ScalarInfix), RequiredStringOperation,
                            (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.DivideExpression, left, op, right)),

                        Rule(_left, Token(SyntaxKind.PercentToken, CompletionKind.ScalarInfix), RequiredStringOperation,
                            (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.ModuloExpression, left, op, right))
                        ));

            var RequiredMultiplicative =
                Required(Multiplicative, CreateMissingExpression);

            var Additive =
                ApplyZeroOrMore(Multiplicative, _left =>
                    First(
                        Rule(_left, Token(SyntaxKind.PlusToken, CompletionKind.ScalarInfix), RequiredMultiplicative,
                            (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.AddExpression, left, op, right)),

                        Rule(_left, Token(SyntaxKind.MinusToken, CompletionKind.ScalarInfix), RequiredMultiplicative,
                            (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.SubtractExpression, left, op, right))
                        ));

            var RequiredAdditive =
                Required(Additive, CreateMissingExpression);

            var Relational =
                ApplyOptional(Additive, _left =>
                    First(
                        Rule(_left, Token(SyntaxKind.LessThanToken, CompletionKind.ScalarInfix), RequiredAdditive,
                            (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.LessThanExpression, left, op, right)),

                        Rule(_left, Token(SyntaxKind.LessThanOrEqualToken, CompletionKind.ScalarInfix), RequiredAdditive,
                            (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.LessThanOrEqualExpression, left, op, right)),

                        Rule(_left, Token(SyntaxKind.GreaterThanToken, CompletionKind.ScalarInfix), RequiredAdditive,
                            (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.GreaterThanExpression, left, op, right)),

                        Rule(_left, Token(SyntaxKind.GreaterThanOrEqualToken, CompletionKind.ScalarInfix), RequiredAdditive,
                            (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.GreaterThanOrEqualExpression, left, op, right))
                        ));

            var RequiredRelational =
                Required(Relational, CreateMissingExpression);

            var ExpressionCouple =
                Rule(
                    Token(SyntaxKind.OpenParenToken),
                    Required(UnnamedExpression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.DotDotToken),
                    Required(UnnamedExpression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.CloseParenToken),

                    (openParen, first, dotDot, second, closeParen) =>
                        new ExpressionCouple(openParen, first, dotDot, second, closeParen));

            var InOperatorExpressionList =
                Rule(
                    Token(SyntaxKind.OpenParenToken),
                    Best(
                        // this is a special path meant to influence completion for first argument only when an extra parenthesis is typed 
                        If(And(Token(SyntaxKind.OpenParenToken), AnyToken.WithCompletionHint(CompletionHint.NonScalar | CompletionHint.Scalar)),
                            CommaList(UnnamedExpression, CreateMissingExpression, oneOrMore: true)),
                        // normal list of expressions
                        CommaList(UnnamedExpression, CreateMissingExpression, oneOrMore: true),
                        // allows full query expression as only item in list
                        Rule(
                            this.Expression.WithCompletionHint(CompletionHint.NonScalar | CompletionHint.Scalar),
                            expr => new SyntaxList<SeparatedElement<Expression>>(new SeparatedElement<Expression>(expr)))
                        ),
                    RequiredToken(SyntaxKind.CloseParenToken),

                    (openParen, list, closeParen) =>
                        new ExpressionList(openParen, list, closeParen));

            Parser<LexicalToken, SyntaxToken> InToken(SyntaxKind inKind)
            {
                var opText = SyntaxFacts.GetText(inKind);
                return Token(inKind,
                    CreateCompletionItem(opText, CompletionKind.ScalarInfix, CompletionPriority.Normal, ctext: opText + " (|)", matchText: opText)
                    );
            }

            var Equality =
                First(
                    Rule(Token(SyntaxKind.AsteriskToken).Hide(), Token(SyntaxKind.EqualEqualToken), Relational, (asterisk, equal, expression) =>
                            (Expression)new BinaryExpression(SyntaxKind.EqualExpression, new StarExpression(asterisk), equal, expression)),

                    ApplyOptional(Relational, _left =>
                        First(
                            Rule(_left, Token(SyntaxKind.EqualEqualToken, CompletionKind.ScalarInfix, CompletionPriority.Top), RequiredRelational,
                                (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.EqualExpression, left, op, right)),

                            Rule(_left, Token(SyntaxKind.BangEqualToken, CompletionKind.ScalarInfix), RequiredRelational,
                                (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.NotEqualExpression, left, op, right)),

                            Rule(_left, Token(SyntaxKind.LessThanGreaterThanToken, CompletionKind.ScalarInfix).Hide(), RequiredRelational,
                                (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.NotEqualExpression, left, op, right)),

                            Rule(_left, InToken(SyntaxKind.InKeyword), InOperatorExpressionList,
                                (left, op, right) => (Expression)new InExpression(SyntaxKind.InExpression, left, op, right)),

                            Rule(_left, InToken(SyntaxKind.InCsKeyword), InOperatorExpressionList,
                                (left, op, right) => (Expression)new InExpression(SyntaxKind.InCsExpression, left, op, right)),

                            Rule(_left, InToken(SyntaxKind.NotInKeyword), InOperatorExpressionList,
                                (left, op, right) => (Expression)new InExpression(SyntaxKind.NotInExpression, left, op, right)),

                            Rule(_left, InToken(SyntaxKind.NotInCsKeyword), InOperatorExpressionList,
                                (left, op, right) => (Expression)new InExpression(SyntaxKind.NotInCsExpression, left, op, right)),

                            Rule(_left, InToken(SyntaxKind.HasAnyKeyword), InOperatorExpressionList,
                                (left, op, right) => (Expression)new HasAnyExpression(SyntaxKind.HasAnyKeyword, left, op, right)),

                              Rule(_left, InToken(SyntaxKind.HasAllKeyword), InOperatorExpressionList,
                                (left, op, right) => (Expression)new HasAllExpression(SyntaxKind.HasAllKeyword, left, op, right)),

                            Rule(_left, Token(SyntaxKind.BetweenKeyword, CompletionKind.ScalarInfix, ctext: SyntaxFacts.GetText(SyntaxKind.BetweenKeyword) + " (| .. )"), ExpressionCouple,
                                (left, op, right) => (Expression)new BetweenExpression(SyntaxKind.BetweenExpression, left, op, right)),

                            Rule(_left, Token(SyntaxKind.NotBetweenKeyword, CompletionKind.ScalarInfix, ctext: SyntaxFacts.GetText(SyntaxKind.NotBetweenKeyword) + " (| .. )"), ExpressionCouple,
                                (left, op, right) => (Expression)new BetweenExpression(SyntaxKind.NotBetweenExpression, left, op, right))
                            )));

            var LogicalAnd =
                ApplyZeroOrMore(Equality, _left =>
                    Rule(_left, Token(SyntaxKind.AndKeyword, CompletionKind.ScalarInfix), Required(Equality, CreateMissingExpression),
                        (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.AndExpression, left, op, right)));

            var LogicalOr =
                ApplyZeroOrMore(LogicalAnd, _left =>
                    Rule(_left, Token(SyntaxKind.OrKeyword, CompletionKind.ScalarInfix), Required(LogicalAnd, CreateMissingExpression),
                        (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.OrExpression, left, op, right)));
            #endregion

            #region Query Operators

            this.LiteralList = CommaList(Literal, CreateMissingExpression, allowTrailingComma: true);

            var RowSchema =
                    Rule(
                        Token(SyntaxKind.OpenParenToken),
                        Optional(Token(SyntaxKind.CommaToken)),
                        CommaList<NameAndTypeDeclaration>(
                            NameAndTypeDeclaration,
                            CreateMissingNameAndTypeDeclaration,
                            allowTrailingComma: true),
                        RequiredToken(SyntaxKind.CloseParenToken),
                        (openParen, leadingComma, columns, closeParen) =>
                            new RowSchema(openParen, leadingComma, columns, closeParen));

            var MandatoryRowSchema =
                Rule(
                    Token(SyntaxKind.OpenParenToken),
                    Optional(Token(SyntaxKind.CommaToken)),
                    CommaList<NameAndTypeDeclaration>(
                        NameAndTypeDeclaration,
                        CreateMissingNameAndTypeDeclaration,
                        oneOrMore: true,
                        allowTrailingComma: true),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (openParen, leadingComma, columns, closeParen) =>
                        new RowSchema(openParen, leadingComma, columns, closeParen));

            var EvaluateRowSchema =
                    Rule(
                        Token(SyntaxKind.OpenParenToken),
                        Optional(Token(SyntaxKind.CommaToken)),
                        Optional(Token(SyntaxKind.AsteriskToken)),
                        Optional(Token(SyntaxKind.CommaToken)),
                        CommaList<NameAndTypeDeclaration>(
                            NameAndTypeDeclaration,
                            CreateMissingNameAndTypeDeclaration,
                            allowTrailingComma: true),
                        RequiredToken(SyntaxKind.CloseParenToken),
                        (openParen, leadingComma, asteriskToken, asteriskTokenComma, columns, closeParen) =>
                            new EvaluateRowSchema(openParen, leadingComma, asteriskToken, asteriskTokenComma, columns, closeParen));

            var DataTableExpression =
                Rule(
                    Token(SyntaxKind.DataTableKeyword, CompletionKind.QueryPrefix),
                    QueryParameterList(QueryOperatorParameters.DataTableParameters),
                    Required(RowSchema, CreateMissingRowSchema),
                    RequiredToken(SyntaxKind.OpenBracketToken),
                    Optional(Token(SyntaxKind.CommaToken)),
                    LiteralList,
                    RequiredToken(SyntaxKind.CloseBracketToken),
                    (keyword, parameters, schema, openBracket, leadingComma, values, closeBracket) =>
                        (Expression)new DataTableExpression(keyword, parameters, schema, openBracket, leadingComma, values, closeBracket));

            var ContextualDataTableExpression =
                Rule(
                    Token(SyntaxKind.ContextualDataTableKeyword).Hide(),
                    Required(UnnamedExpression, CreateMissingExpression), // guid literal expected, though parse any expression
                    Required(RowSchema, CreateMissingRowSchema),
                    (keyword, id, schema) =>
                        (Expression)new ContextualDataTableExpression(keyword, id, schema));

            var ExternalDataWithClausePropertyValue =
                First(
                    StringLiteral,
                    LongLiteral,
                    RealLiteral,
                    BooleanLiteral,
                    DateTimeLiteral,
                    TypeofLiteral,
                    RawGuidLiteral,
                    Rule(RenameName, n => (Expression)n));

            var ExternalDataWithClauseProperty =
                Rule(
                    RenameName,
                    Token(SyntaxKind.EqualToken),
                    Required(ExternalDataWithClausePropertyValue, CreateMissingValue),
                    (name, equals, value) => new NamedParameter(name, equals, value));

            var ExternalDataWithClause =
                Rule(
                    Token(SyntaxKind.WithKeyword),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    CommaList(ExternalDataWithClauseProperty, CreateMissingNamedParameter),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (keyword, openParen, list, closeParen) =>
                        new ExternalDataWithClause(keyword, openParen, list, closeParen));

            var ExternalDataExpression =
                Rule(
                    First(
                        Token(SyntaxKind.ExternalDataKeyword, CompletionKind.QueryPrefix),
                        Token(SyntaxKind.External_DataKeyword).Hide()),
                    QueryParameterList(QueryOperatorParameters.ExternalDataWithClauseProperties),
                    Required(RowSchema, CreateMissingRowSchema),
                    RequiredToken(SyntaxKind.OpenBracketToken),
                    CommaList(UnnamedExpression, fnMissingElement: CreateMissingExpression, allowTrailingComma: true, oneOrMore: true),
                    RequiredToken(SyntaxKind.CloseBracketToken),
                    Optional(ExternalDataWithClause),
                    (keyword, parameters, schema, openBracket, name, closeBracket, withClause) =>
                        (Expression)new ExternalDataExpression(keyword, parameters, schema, openBracket, name, closeBracket, withClause));

            // Inline External Table Expression

            var InlineExternalTableKindClause = Rule(
                Token(SyntaxKind.KindKeyword),
                RequiredToken(SyntaxKind.EqualToken),
                RequiredToken(KustoFacts.InlineExternalTableKinds),
                (keyword, equals, kind) =>
                    new InlineExternalTableKindClause(keyword, equals, kind));

            var InlineExternalTableDataFormatClause = Rule(
                Token(SyntaxKind.DataFormatKeyword),
                RequiredToken(SyntaxKind.EqualToken),
                RequiredToken(SyntaxKind.IdentifierToken),
                (keyword, equals, type) =>
                    new InlineExternalTableDataFormatClause(keyword, equals, type));

            var ParseInlineExternalTablePathFormat =
                Rule(
                    First(
                        IdentifierNameReference,
                        Rule(
                            Token(SyntaxKind.DateTimePatternKeyword),
                            RequiredToken(SyntaxKind.OpenParenToken),
                            Required(StringLiteral, CreateMissingStringLiteral),
                            RequiredToken(SyntaxKind.CommaToken),
                            Required(IdentifierNameReference, CreateMissingNameReference),
                            RequiredToken(SyntaxKind.CloseParenToken),
                            (keyword, openParen, pattern, comma, partitionColumn, closeBracket) =>
                                (Expression)new DateTimePattern(keyword, openParen, (LiteralExpression)pattern, comma, (NameReference)partitionColumn, closeBracket))),
                    Optional(StringLiteral),
                    (partitionColumnReference, optionalSeparator) =>
                        new InlineExternalTablePathFormatPartitionColumnReference(partitionColumnReference, (LiteralExpression)optionalSeparator)
                );
            
            var InlineExternalTablePathFormatClause =
                Rule(
                    Token(SyntaxKind.PathFormatKeyword),
                    RequiredToken(SyntaxKind.EqualToken),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Optional(StringLiteral),
                    List(ParseInlineExternalTablePathFormat, fnMissingElement: CreateMissingPathFormatTokens, oneOrMore: true),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (keyword, equals, openBracket, optionalSeparator, pathFormat, closeBracket ) =>
                        new InlineExternalTablePathFormatClause(keyword, equals, openBracket, (LiteralExpression)optionalSeparator, pathFormat, closeBracket));

            var PartitionColumnType =
                AsPrimitiveTypeExpression(
                    First(
                        Token(SyntaxKind.DateTimeKeyword, CompletionKind.ScalarType),
                        Token(SyntaxKind.LongKeyword, CompletionKind.ScalarType),
                        Token(SyntaxKind.StringKeyword, CompletionKind.ScalarType)));

            var PartitionColumnDeclaration =
                Rule(
                    ExtendedNameDeclaration,
                    RequiredToken(SyntaxKind.ColonToken),
                    Required(First(PartitionColumnType, InvalidParamType), CreateMissingType),
                    Optional(Token(SyntaxKind.EqualToken)),
                    Optional(UnnamedExpression),
                    (name, colon, type, equal, expr) => new PartitionColumnDeclaration(name, colon, type, equal, expr));


            var InlineExternalTablePartitionClause =
                Rule(
                    Token(SyntaxKind.PartitionKeyword),
                    RequiredToken(SyntaxKind.ByKeyword),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Optional(Token(SyntaxKind.CommaToken)),
                    CommaList<PartitionColumnDeclaration>(PartitionColumnDeclaration, CreateMissingPartitionColumnDeclaration, allowTrailingComma: true),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (keyword, byKeyword, openBracket, optionalComma, partitions, closeBracket) =>
                        new InlineExternalTablePartitionClause(keyword, byKeyword, openBracket, optionalComma, partitions, closeBracket));

            var InlineExternalTableConnectionStringsClause =
                Rule(
                    RequiredToken(SyntaxKind.OpenParenToken),
                    CommaList(UnnamedExpression, fnMissingElement: CreateMissingExpression, allowTrailingComma: true, oneOrMore: true),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (openBracket, connectionStrings, closeBracket) =>
                        new InlineExternalTableConnectionStringsClause(openBracket, connectionStrings, closeBracket));
            
            var InlineExternalTableExpression =
                // Support usage of inline_external_table as identifier
                If(And(Token(SyntaxKind.InlineExternalTableKeyword, CompletionKind.QueryPrefix), Token(SyntaxKind.OpenParenToken)),
                    Rule(
                        Token(SyntaxKind.InlineExternalTableKeyword, CompletionKind.QueryPrefix),
                        QueryParameterList(QueryOperatorParameters.InlineExternalTableProperties),
                        Required(MandatoryRowSchema, CreateMissingRowSchema),
                        Required(InlineExternalTableKindClause, CreateMissingInlineExternalTableKindClause),
                        Optional(InlineExternalTablePartitionClause),
                        Optional(InlineExternalTablePathFormatClause),
                        Required(InlineExternalTableDataFormatClause, CreateMissingInlineExternalTableDataFormatClause),
                        Required(InlineExternalTableConnectionStringsClause, CreateMissingInlineExternalTableConnectionStringsClause),
                        Optional(ExternalDataWithClause),
                        (keyword, parameters, schema, kindClause, partitionClause, pathFormatClause, dataFormat, connectionStrings, withClause) =>
                            (Expression)new InlineExternalTableExpression(keyword, parameters, schema, kindClause, partitionClause, pathFormatClause, dataFormat, connectionStrings, withClause)));

            // End of Inline External Table Expression


            var ConsumeOperator =
                Rule(
                    Token(SyntaxKind.ConsumeKeyword, CompletionKind.QueryPrefix).Hide(),
                    QueryParameterList(QueryOperatorParameters.ConsumeParameters),
                    (consume, parameters) =>
                        (QueryOperator)new ConsumeOperator(consume, parameters))
                .WithTag("<consume>");

            var CountAsIdentifierClause =
                Rule(
                    Token(SyntaxKind.AsKeyword).Hide(),
                    RequiredToken(SyntaxKind.IdentifierToken),
                    (asKeyword, identifier) =>
                        new CountAsIdentifierClause(asKeyword, identifier));

            var CountOperator =
                Rule(
                    Token(SyntaxKind.CountKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                    Optional(CountAsIdentifierClause),
                    (countKeyword, asIdentifier) =>
                        (QueryOperator)new CountOperator(countKeyword, asIdentifier))
                .WithTag("<count>");

            var ExecuteAndCacheOperator =
                Rule(
                    Token(SyntaxKind.ExecuteAndCacheKeyword, CompletionKind.QueryPrefix),
                    (keyword) =>
                        (QueryOperator)new ExecuteAndCacheOperator(keyword))
                .WithTag("<execute-and-cache>");

            var ExtendOperator =
                Rule(
                    Token(SyntaxKind.ExtendKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                    CommaList<Expression>(NamedExpression, CreateMissingExpression, oneOrMore: true),
                    (keyword, list) =>
                        (QueryOperator)new ExtendOperator(keyword, list))
                .WithTag("<extend>");

            var FacetWithClause =
                First(
                    Rule(Token(SyntaxKind.WithKeyword), Token(SyntaxKind.OpenParenToken), Required(ForkPipeExpression, CreateMissingQueryOperatorExpression), RequiredToken(SyntaxKind.CloseParenToken),
                        (withKeyword, openParen, expr, closeParen) =>
                            (FacetWithClause)new FacetWithExpressionClause(withKeyword, openParen, expr, closeParen)),

                    Rule(Token(SyntaxKind.WithKeyword), Required(ForkPipeOperator, CreateMissingQueryOperator),
                        (withKeyword, op) =>
                            (FacetWithClause)new FacetWithOperatorClause(withKeyword, op)));

            var FacetOperator =
                Rule(
                    Token(SyntaxKind.FacetKeyword, CompletionKind.QueryPrefix).Hide(),
                    RequiredToken(SyntaxKind.ByKeyword),
                    SeparatedList<Expression>(SimplePathExpression, SyntaxKind.CommaToken, fnMissingElement: CreateMissingNameReference, oneOrMore: true),
                    Optional(FacetWithClause),
                    (facetKeyword, byKeyword, list, withClause) =>
                        (QueryOperator)new FacetOperator(facetKeyword, byKeyword, list, withClause))
                .WithTag("<facet>");

            var FilterOperator =
                Rule(
                    First(
                        Token(SyntaxKind.WhereKeyword, CompletionKind.QueryPrefix, CompletionPriority.Top),
                        Token(SyntaxKind.FilterKeyword).Hide()),
                    QueryParameterList(QueryOperatorParameters.FilterParameters, equalsNeeded: true),
                    Required(NamedExpression, CreateMissingExpression),
                    (keyword, parameters, condition) =>
                        (QueryOperator)new FilterOperator(keyword, parameters, condition))
                .WithTag("<filter>");

            var GetSchemaOperator =
                Rule(
                    Token(SyntaxKind.GetSchemaKeyword, CompletionKind.QueryPrefix, CompletionPriority.Low),
                    Optional(QueryParameter(QueryOperatorParameters.GetSchemaKind, equalsNeeded: false)),
                    (keyword, kind) => (QueryOperator)new GetSchemaOperator(keyword, kind))
                .WithTag("<get-schema>");

            var AsOperator =
                Rule(
                    Token(SyntaxKind.AsKeyword, CompletionKind.QueryPrefix, CompletionPriority.Low),
                    QueryParameterList(QueryOperatorParameters.AsParameters, equalsNeeded: true),
                    Required(SimpleNameDeclaration, CreateMissingNameDeclaration),
                    (keyword, parameters, name) => (QueryOperator)new AsOperator(keyword, parameters, name))
                .WithTag("<as>");


            Parser<LexicalToken, DataScopeClause> DataScopeClause(CompletionKind ckind) =>
                Rule(
                    Token(SyntaxKind.DataScopeKeyword, ckind).Hide(),
                    RequiredToken(SyntaxKind.EqualToken),
                    RequiredToken(KustoFacts.DataScopeValues),
                    (dataScopeKeyword, equalToken, valueToken) =>
                        new DataScopeClause(dataScopeKeyword, equalToken, valueToken));

            var FindOperand =
                Best(
                    WildcardedEntityReference,
                    ApplyOptional(
                        First(
                            BracketedEntityNamePathElementSelector,
                            BarePathElementSelector),
                        _left =>
                            Rule(_left, Token(SyntaxKind.BarToken), Required(AsOperator, CreateMissingQueryOperator),
                                (left, bar, op) => (Expression)new PipeExpression(left, bar, op)))
                    );

            var FindInClause =
                Rule(
                    Token(SyntaxKind.InKeyword),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    CommaList(FindOperand, CreateMissingExpression, oneOrMore: true),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (inKeyword, openParen, exprs, closeParen) =>
                        new FindInClause(inKeyword, openParen, exprs, closeParen));

            var ColumnNameReference =
                First(
                    IdentifierNameReference,
                    KeywordAsIdentifierNameReference,
                    BracketedNameReference,
                    ClientParameterReference)
                .WithTag("<column>");

            var TypedColumnNameReference =
                ApplyOptional(
                    ColumnNameReference,
                    _left => Rule(
                        _left,
                        Token(SyntaxKind.ColonToken),
                        Required(First(ParamTypeExtended, InvalidParamType), CreateMissingType),
                        (name, colon, type) => (Expression)new TypedColumnReference((NameReference)name, colon, type)));

            var PackExpression =
                Rule(
                    Token(SyntaxKind.PackKeyword),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    RequiredToken(SyntaxKind.AsteriskToken, CompletionKind.Syntax),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (pack, openParen, asterisk, closeParen) =>
                        (Expression)new PackExpression(pack, openParen, asterisk, closeParen));

            var FindProjectColumn =
                First(
                    PackExpression,
                    StarExpression,
                    TypedColumnNameReference);

            var FindProjectClause =
                First(
                    Rule(
                        Token(SyntaxKind.ProjectKeyword),
                        CommaList(FindProjectColumn, CreateMissingExpression, oneOrMore: true),
                        (token, list) => new FindProjectClause(token, list)),
                    Rule(Token(SyntaxKind.ProjectSmartKeyword),
                        (token) => new FindProjectClause(token, SyntaxList<SeparatedElement<Expression>>.Empty())));

            var FindProjectAwayClause =
                Rule(
                    Token(SyntaxKind._ProjectAwayKeyword),
                    CommaList(FindProjectColumn, CreateMissingExpression, oneOrMore: true),
                    (token, list) => new FindProjectClause(token, list));

            var FindOperator =
                Rule(
                    Token(SyntaxKind.FindKeyword, CompletionKind.QueryPrefix),
                    Optional(DataScopeClause(CompletionKind.Syntax)),
                    QueryParameterList(QueryOperatorParameters.FindParameters, equalsNeeded: true),
                    Optional(FindInClause),
                    Optional(Token(SyntaxKind.WhereKeyword)),
                    Required(UnnamedExpression, CreateMissingExpression), // condition
                    Optional(FindProjectClause),
                    Optional(FindProjectAwayClause).Hide(), // internal use?
                    (findKeyword, dataScope, parameters, inClause, whereKeyword, condition, project, projectAway) =>
                        (QueryOperator)new FindOperator(
                            findKeyword,
                            dataScope,
                            parameters,
                            inClause,
                            whereKeyword ?? ((parameters.ChildCount > 0 || inClause != null) ? CreateMissingToken(SyntaxKind.WhereKeyword) : null),
                            condition,
                            project,
                            projectAway))
                .WithTag("<find>");

            var SearchPredicate =
                First(
                    UnnamedExpression,
                    ApplyOptional(
                        StarExpression,
                        _left =>
                            Rule(
                                _left,
                                Token(SyntaxKind.AndKeyword),
                                Required(UnnamedExpression, CreateMissingExpression),
                                (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.AndExpression, left, op, right))));

            var SearchOperator =
                Rule(
                    Token(SyntaxKind.SearchKeyword, CompletionKind.QueryPrefix),
                    QueryParameterList(QueryOperatorParameters.SearchParameters, equalsNeeded: true),
                    Optional(DataScopeClause(CompletionKind.Syntax)),
                    Optional(FindInClause),
                    Required(SearchPredicate, CreateMissingExpression),
                    (keyword, parameters, dataScope, inClause, predicate) =>
                        (QueryOperator)new SearchOperator(keyword, parameters, dataScope, inClause, predicate))
                .WithTag("<search>");

            var NameEqualsClause =
                If(Or(Token(SyntaxKind.IdentifierToken), Token(SyntaxKind.OpenBracketToken)),
                    Rule(SimpleNameDeclaration, RequiredToken(SyntaxKind.EqualToken),
                        (name, equalsToken) => new NameEqualsClause(name, equalsToken)));

            var ForkExpression =
                Rule(
                    Optional(NameEqualsClause),
                    Token(SyntaxKind.OpenParenToken),
                    Required(First(ForkPipeExpression, Expression.Hide()), CreateMissingExpression),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (nameClause, openParen, expr, closeParen) =>
                        new ForkExpression(nameClause, openParen, expr, closeParen));

            var ForkOperator =
                Rule(
                    Token(SyntaxKind.ForkKeyword, CompletionKind.QueryPrefix).Hide(),
                    List(ForkExpression, fnMissingElement: CreateMissingForkExpression, oneOrMore: true),
                    (forkKeyword, list) => (QueryOperator)new ForkOperator(forkKeyword, list))
                .WithTag("<fork>");

            var JoinEqualityExpression =
                ApplyOptional(
                    FunctionCallOrPath,
                    _left =>
                        Rule(
                            _left,
                            Token(SyntaxKind.EqualEqualToken, CompletionKind.ScalarInfix), RequiredFunctionCallOrPath,
                            (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.EqualExpression, left, op, right)));

            var JoinAndExpression =
                ApplyZeroOrMore(
                    JoinEqualityExpression,
                    _left =>
                        Rule(_left, Token(SyntaxKind.AndKeyword, CompletionKind.ScalarInfix), Required(JoinEqualityExpression, CreateMissingExpression),
                            (left, op, right) => (Expression)new BinaryExpression(SyntaxKind.AndExpression, left, op, right)));

            var JoinOnExpression =
                Best(
                    JoinAndExpression, // only legal join expressions will show in intellisense
                    UnnamedExpression.Hide()); // otherwise parse any expression and tag it in semantic analysis.

            var JoinOnClause =
                Rule(
                    Token(SyntaxKind.OnKeyword),
                    CommaList(JoinOnExpression, CreateMissingExpression, oneOrMore: true),
                    (onKeyword, list) => (JoinConditionClause)new JoinOnClause(onKeyword, list));

            var JoinWhereClause =
                Rule(
                    Token(SyntaxKind.WhereKeyword),
                    Required(UnnamedExpression, CreateMissingExpression),
                    (keyword, predicate) => (JoinConditionClause)new JoinWhereClause(keyword, predicate));

            var JoinOperator =
                Rule(
                    Token(SyntaxKind.JoinKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                    QueryParameterList(QueryOperatorParameters.JoinParameters, equalsNeeded: true),
                    Required(UnnamedExpression, CreateMissingExpression),
                    Optional(First(
                        JoinOnClause,
                        JoinWhereClause.Hide())),
                    (joinKeyword, parameters, expr, condition) =>
                        (QueryOperator)new JoinOperator(joinKeyword, parameters, expr, condition))
                .WithTag("<join>");

            var LookupOperator =
                Rule(
                    Token(SyntaxKind.LookupKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                    QueryParameterList(QueryOperatorParameters.LookupParameters, equalsNeeded: true),
                    Required(UnnamedExpression, CreateMissingExpression),
                    Required(JoinOnClause, CreateMissingJoinOnClause),
                    (LookupKeyword, parameters, expr, onClause) =>
                        (QueryOperator)new LookupOperator(LookupKeyword, parameters, expr, onClause))
                .WithTag("<lookup>");

            var MakeSeriesOnClause =
                Rule(
                    RequiredToken(SyntaxKind.OnKeyword),
                    Required(NamedExpression, CreateMissingExpression),
                    (keyword, expr) => new MakeSeriesOnClause(keyword, expr));

            var MakeSeriesInRangeClause =
                Rule(
                    RequiredToken(SyntaxKind.InKeyword).Hide(), // this syntax is deprecated so hide the first keyword
                    RequiredToken(SyntaxKind.RangeKeyword).Hide(),
                    //new CompletionItem(CompletionKind.Keyword, "range (start, stop, step)", "range (", ")", "range")),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    CommaList(NamedExpression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (inKeyword, rangeKeyword, openParen, list, closeParen) =>
                        (MakeSeriesRangeClause)new MakeSeriesInRangeClause(inKeyword, rangeKeyword, new ExpressionList(openParen, list, closeParen))); ;

            var MakeSeriesFromClause =
                Rule(
                    Token(SyntaxKind.FromKeyword),
                    Required(UnnamedExpression, CreateMissingExpression),
                    (FromToken, fromEx) =>
                        new MakeSeriesFromClause(FromToken, fromEx));

            var MakeSeriesToClause =
               Rule(
                   Token(SyntaxKind.ToKeyword),
                   Required(UnnamedExpression, CreateMissingExpression),
                   (ToToken, toEx) =>
                       new MakeSeriesToClause(ToToken, toEx));

            var MakeSeriesStepClause =
              Rule(
                  RequiredToken(SyntaxKind.StepKeyword),
                  Required(UnnamedExpression, CreateMissingExpression),
                  (stepToken, stepEx) =>
                      new MakeSeriesStepClause(stepToken, stepEx));

            var MakeSeriesFromToStepClause =
                If(First(Token(SyntaxKind.FromKeyword), Token(SyntaxKind.ToKeyword), Token(SyntaxKind.StepKeyword)),
                    Rule(
                        Optional(MakeSeriesFromClause),
                        Optional(MakeSeriesToClause),
                        MakeSeriesStepClause,
                        (fromClause, toClause, stepClause) =>
                            (MakeSeriesRangeClause)new MakeSeriesFromToStepClause(fromClause, toClause, stepClause)));

            var MakeSeriesByClause =
                Rule(
                    Token(SyntaxKind.ByKeyword),
                    CommaList(NamedExpression, CreateMissingExpression, oneOrMore: true),
                    (keyword, list) => new MakeSeriesByClause(keyword, list));

            var DefaultExpressionClause =
                Rule(
                    Token(SyntaxKind.DefaultKeyword),
                    RequiredToken(SyntaxKind.EqualToken),
                    Required(NamedExpression, CreateMissingExpression),
                    (defaultKeyword, equalToken, expr) =>
                        new DefaultExpressionClause(defaultKeyword, equalToken, expr));

            var MakeSeriesExpression =
                Rule(
                    NamedExpression,
                    Optional(DefaultExpressionClause),
                    (agg, defexp) => new MakeSeriesExpression(agg, defexp));

            var MakeSeriesOperator =
                Rule(
                    Token(SyntaxKind.MakeSeriesKeyword, CompletionKind.QueryPrefix),
                    QueryParameterList(QueryOperatorParameters.MakeSeriesParameters, equalsNeeded: true),
                    SeparatedList(MakeSeriesExpression, SyntaxKind.CommaToken, CreateMissingMakeSeriesExpression, oneOrMore: true),
                    MakeSeriesOnClause,
                    First(MakeSeriesFromToStepClause, MakeSeriesInRangeClause),
                    Optional(MakeSeriesByClause),
                    (keyword, parameters, aggregates, onClause, rangeClause, byClause) =>
                        (QueryOperator)new MakeSeriesOperator(keyword, parameters, aggregates, onClause, rangeClause, byClause))
                .WithTag("<make-series>");

            var ToTypeOfClause =
                Rule(
                    Token(SyntaxKind.ToKeyword),
                    Required(TypeofLiteral, CreateMissingTypeOfLiteral),
                    (toKeyword, typeOfLiteral) => new ToTypeOfClause(toKeyword, (TypeOfLiteralExpression)typeOfLiteral));

            var MvExpandExpression =
                First(
                    // check for missing initial expression error case
                    If(Token(SyntaxKind.ToKeyword).Hide(),
                        Rule(
                            Required(NamedExpression, CreateMissingExpression),
                            ToTypeOfClause,
                            (expr, toTypeOfClause) =>
                                new MvExpandExpression(expr, toTypeOfClause))),
                    Rule(NamedExpression, Optional(ToTypeOfClause),
                        (expr, toTypeOfClause) =>
                            new MvExpandExpression(expr, toTypeOfClause)));

            var MvExpandExpressionList =
               SeparatedList(MvExpandExpression, SyntaxKind.CommaToken, fnMissingElement: CreateMissingMvExpandExpression, oneOrMore: true);

            var MvExpandRowLimitClause =
                Rule(
                    Token(SyntaxKind.LimitKeyword),
                    Required(UnnamedExpression, CreateMissingExpression),
                    (keyword, expr) => new MvExpandRowLimitClause(keyword, expr));

            var MvExpandOperator =
                Rule(
                    First(
                        Token(SyntaxKind.MvExpandKeyword, CompletionKind.QueryPrefix).Hide(),
                        Token(SyntaxKind.MvDashExpandKeyword, CompletionKind.QueryPrefix)),
                    QueryParameterList(QueryOperatorParameters.MvExpandParameters, equalsNeeded: true),
                    MvExpandExpressionList,
                    Optional(MvExpandRowLimitClause),
                    (keyword, parameters, list, rowLimit) =>
                        (QueryOperator)new MvExpandOperator(keyword, parameters, list, rowLimit))
                .WithTag("<mvexpand>");

            var MvApplyExpression =
                First(
                    // check for missing initial expression error case
                    If(Token(SyntaxKind.ToKeyword).Hide(),
                        Rule(ToTypeOfClause,
                            (clause) =>
                                new MvApplyExpression(CreateMissingExpression(), clause))),
                    Rule(NamedExpression, Optional(ToTypeOfClause),
                        (expr, toTypeOfClause) =>
                            new MvApplyExpression(expr, toTypeOfClause)));

            var MvApplyExpressionList =
                First(
                    // if only one item that is just "to typeof(xxx)" then allow expression to be null w/o error
                    If(And(Token(SyntaxKind.ToKeyword), TypeofLiteral, Fails(Token(SyntaxKind.CommaToken))),
                        Rule(ToTypeOfClause,
                            clause => new SyntaxList<SeparatedElement<MvApplyExpression>>(new[] {
                                new SeparatedElement<MvApplyExpression>(new MvApplyExpression(null, clause)) })))
                        .Hide(),
                    SeparatedList(MvApplyExpression, SyntaxKind.CommaToken, fnMissingElement: CreateMissingMvApplyExpression, oneOrMore: true));

            var MvApplyRowLimitClause =
                Rule(
                    Token(SyntaxKind.LimitKeyword),
                    Required(UnnamedExpression, CreateMissingExpression),
                    (keyword, expr) => new MvApplyRowLimitClause(keyword, expr));

            var MvApplyContextIdClause =
              Rule(
                  Token(SyntaxKind.IdKeyword),
                  Required(UnnamedExpression, CreateMissingExpression),
                  (keyword, expr) => new MvApplyContextIdClause(keyword, expr));

            var MvApplySubqueryExpression =
                Rule(
                    Token(SyntaxKind.OpenParenToken),
                    Required(ContextualSubExpression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (openParen, expr, closeParen) =>
                        new MvApplySubqueryExpression(openParen, expr, closeParen));

            var MvApplyOperator =
                Rule(
                    First(
                        Token(SyntaxKind.MvApplyKeyword, CompletionKind.QueryPrefix).Hide(),
                        Token(SyntaxKind.MvDashApplyKeyword, CompletionKind.QueryPrefix)),
                    QueryParameterList(QueryOperatorParameters.MvApplyParameters, equalsNeeded: true),
                    MvApplyExpressionList,
                    Optional(MvApplyRowLimitClause),
                    Optional(MvApplyContextIdClause).Hide(),
                    RequiredToken(SyntaxKind.OnKeyword),
                    Required(MvApplySubqueryExpression, CreateMissingMvApplySubqueryExpression),
                    (keyword, parameters, list, rowLimit, contextId, onKeyword, subquery) =>
                        (QueryOperator)new MvApplyOperator(keyword, parameters, list, rowLimit, contextId, onKeyword, subquery))
                .WithTag("<mvapply>");

            var EvaluateSchemaClause =
                Rule(
                    Token(SyntaxKind.ColonToken),
                    Required(EvaluateRowSchema, CreateMissingEvaluateRowSchema),
                    (keyword, expr) =>
                        new EvaluateSchemaClause(keyword, expr));

            var EvaluateOperator =
                Rule(
                    Token(SyntaxKind.EvaluateKeyword, CompletionKind.QueryPrefix, CompletionPriority.Low),
                    QueryParameterList(QueryOperatorParameters.EvaluateParameters),
                    RequiredFunctionCall,
                    Optional(EvaluateSchemaClause),
                    (keyword, parameters, expr, schema) =>
                        (QueryOperator)new EvaluateOperator(keyword, parameters, (FunctionCallExpression)expr, schema))
                .WithTag("<evaluate>");

            var NameAndOptionalTypeDeclaration =
                First(
                    ApplyOptional(
                        ExtendedNameDeclarationExpression,
                        _left =>
                            Rule(
                                _left,
                                Token(SyntaxKind.ColonToken),
                                Required(First(ParamType, InvalidParamType), CreateMissingType),
                                (name, colon, type) =>
                                    (Expression)new NameAndTypeDeclaration((NameDeclaration)name, colon, type))),
                    Rule(
                        Token(SyntaxKind.ColonToken).Hide(),
                        Required(First(ParamType, InvalidParamType), CreateMissingType),
                        (colon, type) =>
                            (Expression)new NameAndTypeDeclaration(CreateMissingNameDeclaration(), colon, type)));

            var ParseWithExpression =
                First(
                    StarExpression,
                    StringOrCompoundStringLiteral,
                    NameAndOptionalTypeDeclaration);

            var ParseOperator =
                Rule(
                    Token(SyntaxKind.ParseKeyword, CompletionKind.QueryPrefix, CompletionPriority.Low),
                    QueryParameterList(QueryOperatorParameters.ParseParameters, equalsNeeded: true),
                    Required(UnnamedExpression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.WithKeyword),
                    List(Rule(ParseWithExpression, e => (SyntaxNode)e)),
                    (parseKeyword, parameters, expr, withKeyword, expressions) =>
                        (QueryOperator)new ParseOperator(parseKeyword, parameters, expr, withKeyword, expressions))
                .WithTag("<parse>");

            var ParseWhereOperator =
                Rule(
                    Token(SyntaxKind.ParseWhereKeyword, CompletionKind.QueryPrefix, CompletionPriority.Low),
                    QueryParameterList(QueryOperatorParameters.ParseParameters, equalsNeeded: true),
                    Required(UnnamedExpression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.WithKeyword),
                    List(Rule(ParseWithExpression, e => (SyntaxNode)e)),
                    (parseKeyword, parameters, expr, withKeyword, expressions) =>
                        (QueryOperator)new ParseWhereOperator(parseKeyword, parameters, expr, withKeyword, expressions))
                .WithTag("<parse-where>");

            var ParseKvWithClause =
                Rule(
                    Token(SyntaxKind.WithKeyword).Hide(),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    QueryParameterCommaList(QueryOperatorParameters.ParseKvWithProperties),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (withKeyword, openParen, properties, closeParen) =>
                        new ParseKvWithClause(withKeyword, openParen, properties, closeParen));

            var ParseKvOperator =
                Rule(
                    Token(SyntaxKind.ParseKvKeyword, CompletionKind.QueryPrefix, CompletionPriority.Low),
                    Required(UnnamedExpression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.AsKeyword),
                    Required(RowSchema, CreateMissingRowSchema),
                    Optional(ParseKvWithClause),
                    (parseKvKeyword, expression, asKeyword, keys, withClause) =>
                        (QueryOperator)new ParseKvOperator(parseKvKeyword, expression, asKeyword, keys, withClause))
                .WithTag("<parse-kv>");

            var PartitionScopeClause =
                Rule(
                    Token(SyntaxKind.InKeyword).Hide(),
                    Required(First(FunctionCall, DynamicLiteral), CreateMissingExpression),
                    (inKeyword, expr) => new PartitionScope(inKeyword, expr));

            var PartitionQueryExpression =
               Rule(
                   Token(SyntaxKind.OpenBraceToken),
                   Required(Expression, CreateMissingExpression),
                   RequiredToken(SyntaxKind.CloseBraceToken),
                   (openBrace, expr, closeBrace) =>
                       (PartitionOperand)new PartitionQuery(openBrace, expr, closeBrace));

            var PartitionSubqueryExpression =
                Rule(
                    Token(SyntaxKind.OpenParenToken),
                    Required(First(PipeSubExpression, Expression.Hide()), CreateMissingExpression),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (openParen, expr, closeParen) =>
                        (PartitionOperand)new PartitionSubquery(openParen, expr, closeParen));

            var PartitionOperator =
                Rule(
                    Token(SyntaxKind.PartitionKeyword, CompletionKind.QueryPrefix, CompletionPriority.Low),
                    QueryParameterList(QueryOperatorParameters.PartitionParameters),
                    RequiredToken(SyntaxKind.ByKeyword),
                    Required(SimplePathExpression, CreateMissingNameReference),
                    Optional(PartitionScopeClause),
                    Required(
                        First(
                            PartitionSubqueryExpression,
                            PartitionQueryExpression),
                        CreateMissingPartitionOperand),
                    (partitionKeyword, parameters, byKeyword, byExpression, scope, operand) =>
                        (QueryOperator)new PartitionOperator(partitionKeyword, parameters, byKeyword, byExpression, scope, operand))
                .WithTag("<partition>");

            var PartitionByIdClause =
                Rule(
                    Token(SyntaxKind.IdKeyword),
                    Literal,
                    (keyword, value) =>
                        new PartitionByIdClause(keyword, value));

            var PartitionByOperator =
                Rule(
                    HiddenToken(SyntaxKind.PartitionByKeyword),
                    QueryParameterList(QueryOperatorParameters.PartitionByParameters),
                    Required(SimplePathExpression, CreateMissingNameReference),
                    Optional(PartitionByIdClause),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Required(ContextualSubExpression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (keyword, parameters, entity, idClause, openParen, subQuery, closeParen) =>
                        (QueryOperator)new PartitionByOperator(keyword, parameters, entity, idClause, openParen, subQuery, closeParen));

            var ProjectOperator =
                Rule(
                    Token(SyntaxKind.ProjectKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                    CommaList(NamedExpression, CreateMissingExpression),
                    (keyword, list) => (QueryOperator)new ProjectOperator(keyword, list))
                .WithTag("<project>");

            var ProjectAwayOperator =
                Rule(
                    Token(SyntaxKind.ProjectAwayKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                    CommaList(SimpleOrWildcardedEntityReference, CreateMissingExpression),
                    (keyword, list) => (QueryOperator)new ProjectAwayOperator(keyword, list))
                .WithTag("<project-away>");

            var ProjectByNamesOperator =
                Rule(
                    Token(SyntaxKind.ProjectByNamesKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                    CommaList(UnnamedExpression, CreateMissingExpression),
                    (keyword, list) => (QueryOperator)new ProjectByNamesOperator(keyword, list))
                .WithTag("<project-by-names>")
                .Hide();

            var ProjectKeepOperator =
               Rule(
                   Token(SyntaxKind.ProjectKeepKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                   CommaList(SimpleOrWildcardedEntityReference, CreateMissingExpression, oneOrMore: true),
                   (keyword, list) => (QueryOperator)new ProjectKeepOperator(keyword, list))
               .WithTag("<project-keep>");

            var ProjectRenameOperator =
                Rule(
                    Token(SyntaxKind.ProjectRenameKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                    CommaList(NamedExpression, CreateMissingExpression),
                    (keyword, list) => (QueryOperator)new ProjectRenameOperator(keyword, list))
                .WithTag("<project-rename>");

            var SampleOperator =
                Rule(
                    Token(SyntaxKind.SampleKeyword, CompletionKind.QueryPrefix, CompletionPriority.Low),
                    QueryParameterList(QueryOperatorParameters.SampleParameters, equalsNeeded: true),
                    Required(NamedExpression, CreateMissingExpression),
                    (sampleKeyword, parameters, expression) => (QueryOperator)new SampleOperator(sampleKeyword, parameters, expression))
                .WithTag("<sample>");

            var SampleDistinctOperator =
                Rule(
                    Token(SyntaxKind.SampleDistinctKeyword, CompletionKind.QueryPrefix, CompletionPriority.Low),
                    QueryParameterList(QueryOperatorParameters.SampleDistinctParameters, equalsNeeded: true),
                    Required(NamedExpression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.OfKeyword),
                    Required(NamedExpression, CreateMissingExpression),
                    (keyword, parameters, expr, ofKeyword, ofExpr) =>
                        (QueryOperator)new SampleDistinctOperator(keyword, parameters, expr, ofKeyword, ofExpr))
                .WithTag("<sample-distinct>");

            var ReduceByWithClause =
                Rule(
                    Token(SyntaxKind.WithKeyword),
                    QueryParameterCommaList(QueryOperatorParameters.ReduceWithParameters),
                    (keyword, list) => new ReduceByWithClause(keyword, list));

            var ReduceByOperator =
                Rule(
                    Token(SyntaxKind.ReduceKeyword, CompletionKind.QueryPrefix, CompletionPriority.Low),
                    QueryParameterList(QueryOperatorParameters.ReduceParameters),
                    RequiredToken(SyntaxKind.ByKeyword),
                    Required(NamedExpression, CreateMissingExpression),
                    Optional(ReduceByWithClause),
                    (reduceKeyword, parameters, byKeyword, expr, withClause) =>
                        (QueryOperator)new ReduceByOperator(reduceKeyword, parameters, byKeyword, expr, withClause))
                .WithTag("<reduce-by>");

            var SummarizeBinClause =
                If(And(Token(SyntaxKind.BinKeyword), Token(SyntaxKind.EqualToken)),
                    Rule(
                        Token(SyntaxKind.BinKeyword),
                        Token(SyntaxKind.EqualToken),
                        Required(UnnamedExpression, CreateMissingExpression),
                        (bin, equal, value) =>
                            new SimpleNamedExpression(new NameDeclaration(new TokenName(bin)), equal, value)));

            var SummarizeByExpression =
                If(Not(And(Token(SyntaxKind.BinKeyword), Token(SyntaxKind.EqualToken), Match(tk => tk.Kind.IsLiteral()))),
                    NamedExpression);

            var SummarizeByClause =
                Rule(
                    Token(SyntaxKind.ByKeyword),
                    CommaList(SummarizeByExpression, fnMissingElement: CreateMissingExpression, oneOrMore: true),
                    Optional(SummarizeBinClause.Hide()), // legacy syntax
                    (byKeyword, expressions, binClause) =>
                        new SummarizeByClause(byKeyword, expressions, binClause))
                .WithTag("<summarize-by>");

            var SummarizeExpression =
                If(Not(Token(SyntaxKind.ByKeyword)),
                    NamedExpression);

            var SummarizeOperator =
                Rule(
                    Token(SyntaxKind.SummarizeKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                    QueryParameterList(QueryOperatorParameters.SummarizeParameters, equalsNeeded: true),
                    SeparatedList(SummarizeExpression, SyntaxKind.CommaToken, fnMissingElement: CreateMissingExpression, oneOrMore: false),
                    Optional(SummarizeByClause),
                    (summarizeKeyword, parameters, aggregates, byClause) =>
                        (QueryOperator)new SummarizeOperator(summarizeKeyword, parameters, aggregates, byClause))
                .WithTag("<summarize>");

            var DistinctOperator =
                Rule(
                    Token(SyntaxKind.DistinctKeyword, CompletionKind.QueryPrefix),
                    QueryParameterList(QueryOperatorParameters.DistinctParameters, equalsNeeded: true),
                    CommaList(First(StarExpression, NamedExpression), CreateMissingExpression, oneOrMore: true),
                    (keyword, parameters, list) => (QueryOperator)new DistinctOperator(keyword, parameters, list))
                .WithTag("<distinct>");

            var TakeOperator =
                Rule(
                    First(
                        Token(SyntaxKind.LimitKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                        Token(SyntaxKind.TakeKeyword, CompletionKind.QueryPrefix)),
                    QueryParameterList(QueryOperatorParameters.TakeParameters, equalsNeeded: true),
                    Required(NamedExpression.Examples(KustoFacts.LimitExamples), CreateMissingExpression),
                    (keyword, parameters, expression) =>
                        (QueryOperator)new TakeOperator(keyword, parameters, expression))
                .WithTag("<take>");

            var MissingFirstOrLastToken =
                SyntaxToken.Missing("", SyntaxKind.FirstKeyword, new[] { DiagnosticFacts.GetMissingFirstOrLast() });

            var OrderingNullsClause =
                Rule(
                    Token(SyntaxKind.NullsKeyword),
                    Required(First(Token(SyntaxKind.FirstKeyword), Token(SyntaxKind.LastKeyword)), () => MissingFirstOrLastToken.Clone()),
                    (keyword, firstOrLast) => new OrderingNullsClause(keyword, firstOrLast));

            var OrderingClause =
                Rule(
                    Optional(Token(new[] { SyntaxKind.AscKeyword, SyntaxKind.DescKeyword })),
                    Optional(OrderingNullsClause),
                    (ascOrDesc, nullsClause) => new OrderingClause(ascOrDesc, nullsClause));

            var SortExpression =
                ApplyOptional(
                    NamedExpression,
                    _left =>
                        Rule(
                            _left,
                            If(Or(Token(SyntaxKind.AscKeyword), Token(SyntaxKind.DescKeyword), Token(SyntaxKind.NullsKeyword)),
                                OrderingClause),
                            (left, right) => (Expression)new OrderedExpression(left, right)));

            var SortOperator =
                Rule(
                    First(
                        Token(SyntaxKind.OrderKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                        Token(SyntaxKind.SortKeyword, CompletionKind.QueryPrefix, CompletionPriority.High)),
                    // we hide these on purpose since understanding them requires understanding the internal
                    // implemenation of sort operator and we don't want to document it.
                    QueryParameterList(QueryOperatorParameters.SortParameters).Hide(),
                    RequiredToken(SyntaxKind.ByKeyword),
                    CommaList(SortExpression, CreateMissingExpression, oneOrMore: true),
                    (keyword, parameters, byKeyword, list) =>
                        (QueryOperator)new SortOperator(keyword, parameters, byKeyword, list))
                .WithTag("<sort>");

            var ReorderOrderingClause =
               Rule(
                   Token(new[] { SyntaxKind.AscKeyword, SyntaxKind.DescKeyword, SyntaxKind.GrannyAscKeyword, SyntaxKind.GrannyDescKeyword }),
                   (ascOrDesc) => new OrderingClause(ascOrDesc, null));

            var ReorderExpression =
                ApplyOptional(
                    SimpleOrWildcardedEntityReference,
                    _left =>
                        Rule(
                            _left,
                            ReorderOrderingClause,
                            (left, right) => (Expression)new OrderedExpression(left, right)));

            var ProjectReorderOperator =
                Rule(
                   Token(SyntaxKind.ProjectReorderKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                   CommaList(ReorderExpression, CreateMissingExpression),
                   (keyword, list) => (QueryOperator)new ProjectReorderOperator(keyword, list))
                .WithTag("<project-reorder>");

            var ScanAssignment =
                Rule(ExtendedNameReference, Token(SyntaxKind.EqualToken), Required(UnnamedExpression, CreateMissingExpression),
                    (name, equals, expr) =>
                        new ScanAssignment((NameReference)name, equals, expr))
                .WithTag("<assignment>");

            var ScanComputationClause =
                Rule(
                    Token(SyntaxKind.FatArrowToken),
                    CommaList(ScanAssignment, CreateMissingScanAssignment, oneOrMore: true),
                    (token, list) => new ScanComputationClause(token, list));

            var ScanStepOutput =
                Rule(
                    Token(SyntaxKind.OutputKeyword),
                    RequiredToken(SyntaxKind.EqualToken),
                    RequiredToken(KustoFacts.ScanStepOutputValues),
                    (output, equality, outputKind) => new ScanStepOutput(output, equality, outputKind));

            var ScanStep =
                Rule(
                    Token(SyntaxKind.StepKeyword),
                    Required(RenameName, CreateMissingNameDeclaration), // name                    
                    Optional(HiddenToken(SyntaxKind.OptionalKeyword)), // not yet supported                    
                    Optional(ScanStepOutput.Hide()),
                    RequiredToken(SyntaxKind.ColonToken),
                    Required(UnnamedExpression, CreateMissingExpression),
                    Optional(ScanComputationClause),
                    RequiredToken(SyntaxKind.SemicolonToken),
                    (step, name, optional, output, colon, predicate, computation, semi) =>
                        new ScanStep(step, name, optional, output, colon, predicate, computation, semi));

            var ScanOrderByClause =
                Rule(
                    HiddenToken(SyntaxKind.OrderKeyword), // not yet supported
                    RequiredToken(SyntaxKind.ByKeyword),
                    CommaList(SortExpression, CreateMissingExpression, oneOrMore: true, endKinds: new[] { SyntaxKind.PartitionKeyword, SyntaxKind.DeclareKeyword, SyntaxKind.WithKeyword }),
                    (order, by, list) =>
                        new ScanOrderByClause(order, by, list));

            var ScanPartitionByClause =
                Rule(
                    HiddenToken(SyntaxKind.PartitionKeyword), // not yet supported
                    RequiredToken(SyntaxKind.ByKeyword),
                    CommaList(UnnamedExpression, CreateMissingExpression, oneOrMore: true, endKinds: new[] { SyntaxKind.DeclareKeyword, SyntaxKind.WithKeyword }),
                    (partition, by, list) =>
                        new ScanPartitionByClause(partition, by, list));

            var ScanDeclareClause =
                Rule(
                    Token(SyntaxKind.DeclareKeyword),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    CommaList(FunctionParameter, CreateMissingFunctionParameter, endKinds: new[] { SyntaxKind.WithKeyword }),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (declare, open, declarations, close) => new ScanDeclareClause(declare, open, declarations, close));

            var ScanOperator =
                Rule(
                    Token(SyntaxKind.ScanKeyword, CompletionKind.QueryPrefix),
                    QueryParameterList(QueryOperatorParameters.ScanParameters),
                    Optional(ScanOrderByClause),
                    Optional(ScanPartitionByClause),
                    Optional(ScanDeclareClause),
                    RequiredToken(SyntaxKind.WithKeyword,
                        new CompletionItem(CompletionKind.Syntax, "with", "with (", ")")),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    List(ScanStep),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (scan, parameters, orderBy, partitionBy, declare, with, openParen, steps, closeParen) =>
                        (QueryOperator)new ScanOperator(scan, parameters, orderBy, partitionBy, declare, with, openParen, steps, closeParen));

            var TopHittersByClause =
                Rule(
                    Token(SyntaxKind.ByKeyword),
                    Required(NamedExpression, CreateMissingExpression),
                    (keyword, expression) => new TopHittersByClause(keyword, expression));

            var TopHittersOperator =
                Rule(
                    Token(SyntaxKind.TopHittersKeyword, CompletionKind.QueryPrefix),
                    Required(NamedExpression.Examples(KustoFacts.TopExamples), CreateMissingExpression),
                    RequiredToken(SyntaxKind.OfKeyword),
                    Required(NamedExpression, CreateMissingExpression),
                    Optional(TopHittersByClause),
                    (keyword, expr, ofKeyword, ofExpr, byClause) =>
                        (QueryOperator)new TopHittersOperator(keyword, expr, ofKeyword, ofExpr, byClause))
                .WithTag("<top-hitters>");

            var TopOperator =
                Rule(
                    Token(SyntaxKind.TopKeyword, CompletionKind.QueryPrefix),
                    QueryParameterList(QueryOperatorParameters.TopParameters, equalsNeeded: true),
                    Required(NamedExpression.Examples(KustoFacts.TopExamples), CreateMissingExpression),
                    RequiredToken(SyntaxKind.ByKeyword),
                    Required(SortExpression, CreateMissingExpression),
                    (keyword, parameters, expr, byKeyword, byExpr) =>
                        (QueryOperator)new TopOperator(keyword, parameters, expr, byKeyword, byExpr))
                .WithTag("<top>");

            var TopNestedWithOthersClause =
                Rule(
                    Token(SyntaxKind.WithKeyword),
                    RequiredToken(SyntaxKind.OthersKeyword),
                    RequiredToken(SyntaxKind.EqualToken),
                    Literal,
                    (withKeyword, othersKeyword, equals, expression) =>
                        new TopNestedWithOthersClause(withKeyword, othersKeyword, equals, expression));

            var TopNestedByExpression =
                ApplyOptional(
                    NamedExpression,
                    _left =>
                        Rule(
                            _left,
                            If(Or(Token(SyntaxKind.AscKeyword), Token(SyntaxKind.DescKeyword), Token(SyntaxKind.NullsKeyword)),
                                OrderingClause),
                            (expr, orderingClause) => (Expression)new OrderedExpression(expr, orderingClause)));

            var TopNestedClause =
                Rule(
                    Token(SyntaxKind.TopNestedKeyword),
                    Optional(NamedExpression),
                    RequiredToken(SyntaxKind.OfKeyword),
                    Required(NamedExpression, CreateMissingExpression),
                    Optional(TopNestedWithOthersClause),
                    RequiredToken(SyntaxKind.ByKeyword),
                    Required(TopNestedByExpression, CreateMissingExpression),

                    (keyword, expr, ofKeyword, ofExpr, withOthersClause, byKeyword, byExpr) =>
                        new TopNestedClause(keyword, expr, ofKeyword, ofExpr, withOthersClause, byKeyword, byExpr));

            var TopNestedOperator =
                If(Token(SyntaxKind.TopNestedKeyword, CompletionKind.QueryPrefix),
                    Rule(CommaList(TopNestedClause, CreateMissingTopNestedClause, oneOrMore: true),
                        list => (QueryOperator)new TopNestedOperator(list)))
                .WithTag("<top-nested>");

            var UnionExpression =
                First(
                    ParenthesizedExpression,
                    WildcardedEntityReference,
                    BracketedEntityNamePathElementSelector,
                    BarePathElementSelector);

            var UnionOperator =
                Rule(
                    Token(SyntaxKind.UnionKeyword, CompletionKind.QueryPrefix),
                    QueryParameterList(QueryOperatorParameters.UnionParameters, equalsNeeded: true),
                    CommaList<Expression>(UnionExpression, CreateMissingExpression, oneOrMore: true),
                    (keyword, parameters, list) => (QueryOperator)new UnionOperator(keyword, parameters, list))
                .WithTag("<union>");

            var SerializeOperator =
                Rule(
                    Token(SyntaxKind.SerializeKeyword, CompletionKind.QueryPrefix, CompletionPriority.Low),
                    QueryParameterList(QueryOperatorParameters.SerializedParameters, equalsNeeded: true),
                    CommaList(NamedExpression, CreateMissingExpression, oneOrMore: false),
                    (keyword, parameters, exprs) =>
                        (QueryOperator)new SerializeOperator(keyword, parameters, exprs))
                .WithTag("<serialize>");

            var RangeOperator =
                // don't parse as range operator if it looks like the range function
                If(And(Token(SyntaxKind.RangeKeyword, CompletionKind.QueryPrefix), Fails(Token("("))),
                    Rule(
                        Token(SyntaxKind.RangeKeyword, CompletionKind.QueryPrefix),
                        Required(SimpleNameDeclaration, CreateMissingNameDeclaration),
                        RequiredToken(SyntaxKind.FromKeyword),
                        Required(UnnamedExpression, CreateMissingExpression),
                        RequiredToken(SyntaxKind.ToKeyword),
                        Required(UnnamedExpression, CreateMissingExpression),
                        RequiredToken(SyntaxKind.StepKeyword),
                        Required(UnnamedExpression, CreateMissingExpression),
                        (rangeToken, name, FromToken, fromEx, ToToken, toEx, stepToken, stepEx) =>
                            (QueryOperator)new RangeOperator(rangeToken, name, FromToken, fromEx, ToToken, toEx, stepToken, stepEx)))
                .WithTag("<range>");

            var InvokeOperator =
                Rule(
                    Token(SyntaxKind.InvokeKeyword, CompletionKind.QueryPrefix, CompletionPriority.Low),
                    Required(DotCompositeFunctionCall, CreateMissingExpression),
                    (keyword, function) => (QueryOperator)new InvokeOperator(keyword, function))
                .WithTag("<invoke>");

            var RenderWithClause =
                Rule(
                    Token(SyntaxKind.WithKeyword),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Optional(Token(SyntaxKind.CommaToken)),
                    QueryParameterCommaList(QueryOperatorParameters.RenderWithProperties),
                    RequiredToken(SyntaxKind.CloseParenToken),

                    (withKeyword, openParen, leadingComma, properties, closeParen) =>
                        new RenderWithClause(withKeyword, openParen, leadingComma, properties, closeParen));

            var RenderChartType =
                First(
                    First(KustoFacts.ChartTypes.Select(c => KustoFacts.HiddenChartTypes.Contains(c) ? Token(c).Hide() : Token(c, CompletionKind.RenderChart)).ToArray()),
                    Token(SyntaxKind.IdentifierToken)); // allow any identifier as a chart type and flag it later during semantic analysis (binding)

            var DeprecatedRenderByPropertyName =
                If(Not(Or(
                        Token("kind"),  // exclude other property names from possibly by-names
                        Token("title"),
                        Token("accumulate"),
                        Token("with"))),
                    SimpleNameReference.Cast<NameReference>());

            var DeprecatedRenderProperty =
                First(
                    QueryParameter(QueryOperatorParameters.RenderKind.Hide(), equalsNeeded: false),
                    QueryParameter(QueryOperatorParameters.RenderTitle.Hide(), equalsNeeded: false),
                    QueryParameter(QueryOperatorParameters.RenderAccumulate.Hide(), equalsNeeded: false),
                    If(And(Token(SyntaxKind.WithKeyword).Hide(), Not(Token(SyntaxKind.OpenParenToken))),
                        Rule(Token(SyntaxKind.WithKeyword).Hide(), Required(Literal, CreateMissingValue),
                            (keyword, value) => new NamedParameter(new NameDeclaration(new TokenName(keyword)), SyntaxToken.Missing(SyntaxKind.EqualToken), value))),
                    Rule(Token(SyntaxKind.ByKeyword).Hide(), NameReferenceList(DeprecatedRenderByPropertyName),
                        (keyword, list) => new NamedParameter(new NameDeclaration(new TokenName(keyword)), SyntaxToken.Missing(SyntaxKind.EqualToken), list)));

            var RenderOperator =
                Rule(
                    Token(SyntaxKind.RenderKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                    Required(RenderChartType, () => CreateMissingToken(KustoFacts.ChartTypes)),
                    List(DeprecatedRenderProperty),
                    Optional(RenderWithClause),
                    (keyword, chart, parameters, withClause) => (QueryOperator)new RenderOperator(keyword, chart, parameters, withClause, null))
                .WithTag("<render>");

            var PrintOperator =
                Rule(
                    Token(SyntaxKind.PrintKeyword, CompletionKind.QueryPrefix),
                    CommaList(NamedExpression, CreateMissingExpression, oneOrMore: true),
                    (keyword, exprs) =>
                        (QueryOperator)new PrintOperator(keyword, exprs))
                .WithTag("<print>");

            var AssertSchemaOperator =
                Rule(
                    Token(SyntaxKind.AssertSchemaKeyword, CompletionKind.QueryPrefix),
                    Required(RowSchema, CreateMissingRowSchema),
                    (keyword, schema) =>
                        (QueryOperator)new AssertSchemaOperator(keyword, schema)).Hide();

            var EntityGroup = Rule(
                Token(SyntaxKind.EntityGroupKeyword),
                RequiredToken(SyntaxKind.OpenBracketToken),
                CommaList(UnnamedExpression, CreateMissingExpression, oneOrMore: true),
                RequiredToken(SyntaxKind.CloseBracketToken),
                (keyword, open, entitiesList, close) =>
                    (Expression)(new EntityGroup(keyword, open, entitiesList, close)));

            var macroExpandScopeReferenceName =
                Rule(
                    RequiredToken(SyntaxKind.AsKeyword),
                    IdentifierNameDeclaration,
                    (asKeyword, macroReferenceName) => new MacroExpandScopeReferenceName(asKeyword, macroReferenceName));

            var MacroExpandOperator =
                Rule(
                    Token(SyntaxKind.MacroExpandKeyword, CompletionKind.QueryPrefix, CompletionPriority.High),
                    QueryParameterList(QueryOperatorParameters.MacroExpandParameters, equalsNeeded: true),
                    First(EntityGroup, If(ScanQualifiedEntityStart, EntityPathExpression), FunctionCall, SimpleNameReference),
                    Optional(macroExpandScopeReferenceName),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    MacroExpandSubQuery,
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (macroExpandKeyword, parameters, entitygroup, scopeReferenceName, openParen, statementList, closeParen) =>
                        (QueryOperator)new MacroExpandOperator(macroExpandKeyword, parameters, entitygroup, scopeReferenceName, openParen, statementList, closeParen))
                .WithTag("<macro-expand>");

            var MakeGraphTableAndKeyClause =
                Rule(
                    InvocationExpression,
                    RequiredToken(SyntaxKind.OnKeyword),
                    Required(SimpleNameReference, CreateMissingNameReference),
                    (table, onKeyword, column) =>
                        new MakeGraphTableAndKeyClause(table, onKeyword, (NameReference)column))
                .WithTag("<table-and-key-clause>");

            var MakeGraphWithTablesAndKeysClause =
                Rule(
                    Token(SyntaxKind.WithKeyword),
                    CommaList(MakeGraphTableAndKeyClause, CreateMissingMakeGraphTableAndKeyClause, oneOrMore: true, endKinds: new[] { SyntaxKind.PartitionedByKeyword }),
                    (withKeyword, tablesAndKeys) =>
                        (MakeGraphWithClause)new MakeGraphWithTablesAndKeysClause(withKeyword, tablesAndKeys))
                .WithTag("<make-graph-with-tables-and-keys-clause>");

            var MakeGraphWithImplicitIdClause =
                Rule(
                    Token(SyntaxKind.WithNodeIdKeyword, ctext: $"{SyntaxFacts.GetText(SyntaxKind.WithNodeIdKeyword)}="),
                    RequiredToken(SyntaxKind.EqualToken),
                    Required(SimpleNameDeclaration, CreateMissingNameDeclaration),
                    (withNodeId, equals, name) =>
                        (MakeGraphWithClause)new MakeGraphWithImplicitIdClause(withNodeId, equals, name))
                .WithTag("<make-graph-with-implicit-node-id-clause>");

            var MakeGraphPartitionedByClause =
                Rule(
                    Token(SyntaxKind.PartitionedByKeyword),
                    Required(SimplePathExpression, CreateMissingNameReference),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Required(ContextualSubExpression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (keyword, entity, openParen, subQuery, closeParen) =>
                        new MakeGraphPartitionedByClause(keyword, (NameReference)entity, openParen, subQuery, closeParen))
                .WithTag("<make-graph-partitioned-by-clause>");

            var MakeGraphOperator =
                Rule(
                    Token(SyntaxKind.MakeGraphKeyword, CompletionKind.QueryPrefix),
                    QueryParameterList(QueryOperatorParameters.GraphMakeParameters, equalsNeeded: true),
                    Required(SimpleNameReference, CreateMissingNameReference),
                    Required(
                        First(
                            Token("-->", SyntaxKind.DashDashGreaterThanToken),
                            Token("--", SyntaxKind.DashDashToken).Hide()),
                        () => CreateMissingToken(new[] { SyntaxKind.DashDashGreaterThanToken, SyntaxKind.DashDashToken })),
                    Required(SimpleNameReference, CreateMissingNameReference),
                    Optional(
                        First(
                            MakeGraphWithImplicitIdClause, 
                            MakeGraphWithTablesAndKeysClause)),
                    Optional(MakeGraphPartitionedByClause),
                    (keyword, parameters, sourceColumn, direction, targetColumn, withClause, partitionedByClause) =>
                        (QueryOperator)new MakeGraphOperator(keyword, parameters, (NameReference)sourceColumn, direction, (NameReference)targetColumn, withClause, partitionedByClause)
                    )
                .WithTag("<make-graph>");

            var GraphMarkComponentsOperator =
                Rule(
                    Token(SyntaxKind.GraphMarkComponentsKeyword, CompletionKind.QueryPrefix),
                    QueryParameterList(QueryOperatorParameters.GraphMarkComponentsParameters, equalsNeeded: true),
                    (graphMarkComponentsKeyword, parameters) =>
                        (QueryOperator)new GraphMarkComponentsOperator(graphMarkComponentsKeyword, parameters)
                    )
                .WithTag("<graph-mark-components>");

            var GraphWhereNodesOperator =
                Rule(
                    Token(SyntaxKind.GraphWhereNodesKeyword, CompletionKind.QueryPrefix),
                    Required(Expression, CreateMissingExpression),
                    (graphWhereNodesKeyword, expression) =>
                        (QueryOperator)new GraphWhereNodesOperator(graphWhereNodesKeyword, expression)
                    )
                .WithTag("<graph-where-nodes>");

            var GraphWhereEdgesOperator =
                Rule(
                    Token(SyntaxKind.GraphWhereEdgesKeyword, CompletionKind.QueryPrefix),
                    Required(Expression, CreateMissingExpression),
                    (graphWhereEdgesKeyword, expression) =>
                        (QueryOperator)new GraphWhereEdgesOperator(graphWhereEdgesKeyword, expression)
                    )
                .WithTag("<graph-where-edges>");

            var GraphToTableAsClause =
                Rule(
                    Token(SyntaxKind.AsKeyword, CompletionKind.Keyword, CompletionPriority.Low),
                    Required(SimpleNameDeclaration, CreateMissingNameDeclaration),
                    (keyword, name) => new GraphToTableAsClause(keyword, name))
                .WithTag("<graph-to-table-as-clause>");

            var GraphToTableNodesClause =
                Rule(
                    Token(SyntaxKind.NodesKeyword, CompletionKind.Keyword),
                    Optional(GraphToTableAsClause),
                    QueryParameterList(QueryOperatorParameters.GraphToTableNodesParameters, equalsNeeded: true),
                    (keyword, asClause, parameters) => new GraphToTableOutputClause(keyword, asClause, parameters))
                .WithTag("graph-to-table-output-nodes-clause");

            var GraphToTableEdgesClause =
                Rule(
                     Token(SyntaxKind.GraphEdgesKeyword, CompletionKind.Keyword),
                     Optional(GraphToTableAsClause),
                     QueryParameterList(QueryOperatorParameters.GraphToTableEdgesParameters, equalsNeeded: true),
                     (keyword, asClause, parameters) => new GraphToTableOutputClause(keyword, asClause, parameters))
                .WithTag("graph-to-table-output-edges-clause");

            var GraphToTableOutputClause =
                First(
                    GraphToTableNodesClause,
                    GraphToTableEdgesClause
                ).WithTag("<graph-to-table-output-clause>");

            var GraphToTableOperator =
                Rule(
                    Token(SyntaxKind.GraphToTableKeyword, CompletionKind.QueryPrefix),
                    CommaList(GraphToTableOutputClause, CreateMissingGraphToTableOutputClause, oneOrMore: true),
                    (keyword, outputClause) => (QueryOperator)new GraphToTableOperator(keyword, outputClause))
                .WithTag("<graph-to-table>");

            var WhereClause =
                Rule(
                    Token(SyntaxKind.WhereKeyword),
                    Required(Expression, CreateMissingExpression),
                    (keyword, expression) =>
                        new WhereClause(keyword, expression));

            var ProjectClause =
                Rule(
                    Token(SyntaxKind.ProjectKeyword),
                    CommaList(NamedExpression, CreateMissingExpression, oneOrMore: true),
                    (keyword, list) =>
                        new ProjectClause(keyword, list));

            var GraphMatchPatternEdgeRange =
                Rule(
                    Token(SyntaxKind.AsteriskToken),
                    Required(InvocationExpression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.DotDotToken),
                    Required(InvocationExpression, CreateMissingExpression),
                    (asterisk, rangeStart, dotDotToken, rangeEnd) =>
                        new GraphMatchPatternEdgeRange(asterisk, rangeStart, dotDotToken, rangeEnd));

            var GraphMatchPatternEdge =
                First(
                    Rule(
                        First(
                            MatchText("-->", SyntaxKind.DashDashGreaterThanToken),
                            MatchText("<--", SyntaxKind.LessThanDashDashToken),
                            MatchText("--", SyntaxKind.DashDashToken)),
                        (token) => (GraphMatchPatternNotation)new GraphMatchPatternEdge(token, null, null, null)),
                    Rule(
                        First(
                            MatchText("<-[", SyntaxKind.LessThanDashBracketToken),
                            MatchText("-[", SyntaxKind.DashBracketToken)),
                        Optional(SimpleNameDeclaration),
                        Optional(GraphMatchPatternEdgeRange),
                        First(
                            MatchText("]->", SyntaxKind.BracketDashGreaterThanToken),
                            MatchText("]-", SyntaxKind.BracketDashToken)),
                        (firstToken, name, range, lastToken) =>
                            (GraphMatchPatternNotation)new GraphMatchPatternEdge(firstToken, name, range, lastToken))
                    );

            var GraphMatchPatternNode =
                Rule(
                    Token(SyntaxKind.OpenParenToken),
                    Optional(SimpleNameDeclaration),
                    Token(SyntaxKind.CloseParenToken),
                    (open, name, close) =>
                        (GraphMatchPatternNotation)new GraphMatchPatternNode(open, name, close));

            var GraphMatchPattern = Rule(
                List(
                    First(GraphMatchPatternNode, GraphMatchPatternEdge),
                    CreateMissingGraphMatchPatternNotation,
                    oneOrMore: true
                ),
                (elements) => new GraphMatchPattern(elements))
                .WithCompletion(
                    new CompletionItem(CompletionKind.Syntax, "(n1)-[e]->(n2)"),
                    new CompletionItem(CompletionKind.Syntax, "(n1)-[e1]->(n2)-[e2]->(n3)"),
                    new CompletionItem(CompletionKind.Syntax, "(n1)-[e*1..3]->(n2)"));

            var GraphMatchPatternClause =
                CommaList(
                   GraphMatchPattern,
                    CreateMissingGraphMatchPattern,
                    oneOrMore: true,
                    endKinds: new[] { SyntaxKind.WhereKeyword, SyntaxKind.ProjectKeyword }
                ).WithTag("<graph-match-pattern-clause>");

            var GraphMatchOperator =
                Rule(
                    Token(SyntaxKind.GraphMatchKeyword, CompletionKind.QueryPrefix),
                    QueryParameterList(QueryOperatorParameters.GraphMatchParameters, equalsNeeded: true),
                    GraphMatchPatternClause,
                    Optional(WhereClause),
                    Optional(ProjectClause),
                    (keyword, parameters, patterns, whereClause, projectClause) =>
                        (QueryOperator)new GraphMatchOperator(keyword, parameters, patterns, whereClause, projectClause))
                .WithTag("<graph-match-operator>");

            var GraphShortestPathsOperator =
                Rule(
                    Token(SyntaxKind.GraphShortestPathsKeyword, CompletionKind.QueryPrefix),
                    QueryParameterList(QueryOperatorParameters.GraphShortestPathsParameters, equalsNeeded: true),
                    GraphMatchPatternClause,
                    Optional(WhereClause),
                    Optional(ProjectClause),
                    (keyword, parameters, patterns, whereClause, projectClause) =>
                        (QueryOperator)new GraphShortestPathsOperator(keyword, parameters, patterns, whereClause, projectClause))
                .WithTag("<graph-shortest-paths-operator>");

            var PrePipeQueryOperator =
                First(
                    EvaluateOperator,
                    FindOperator,
                    SearchOperator,
                    UnionOperator,
                    MacroExpandOperator,
                    RangeOperator,
                    PrintOperator);

            var BadQueryOperator =
                Rule(
                    Token(SyntaxKind.IdentifierToken),
                    id => (QueryOperator)new BadQueryOperator((SyntaxToken)id, new[] { DiagnosticFacts.GetQueryOperatorExpected() }));

            var PostPipeQueryOperator =
                First(
                    AsOperator,
                    AssertSchemaOperator,
                    ConsumeOperator,
                    CountOperator,
                    DistinctOperator,
                    EvaluateOperator,
                    ExecuteAndCacheOperator,
                    ExtendOperator,
                    FacetOperator,
                    FilterOperator,
                    ForkOperator,
                    GetSchemaOperator,
                    GraphMatchOperator,
                    GraphShortestPathsOperator,
                    GraphMarkComponentsOperator,
                    // currently hidden until we document this feature.
                    GraphWhereNodesOperator.Hide(),
                    GraphWhereEdgesOperator.Hide(),
                    GraphToTableOperator,
                    InvokeOperator,
                    JoinOperator,
                    LookupOperator,
                    MakeGraphOperator,
                    MakeSeriesOperator,
                    MvApplyOperator,
                    MvExpandOperator,
                    ParseOperator,
                    ParseWhereOperator,
                    ParseKvOperator,
                    PartitionByOperator,
                    PartitionOperator,
                    ProjectOperator,
                    ProjectAwayOperator,
                    ProjectByNamesOperator,
                    ProjectKeepOperator,
                    ProjectRenameOperator,
                    ProjectReorderOperator,
                    ReduceByOperator,
                    RenderOperator,
                    SampleOperator,
                    SampleDistinctOperator,
                    ScanOperator,
                    SearchOperator,
                    SerializeOperator,
                    SortOperator,
                    SummarizeOperator,
                    TakeOperator,
                    TopHittersOperator,
                    TopOperator,
                    TopNestedOperator,
                    UnionOperator);

            // all operators
            this.QueryOperator =
                First(PrePipeQueryOperator, PostPipeQueryOperator);

            ForkPipeOperatorCore =
                First(
                    CountOperator,
                    ExtendOperator,
                    FilterOperator,
                    ParseOperator,
                    ParseWhereOperator,
                    ParseKvOperator,
                    TakeOperator,
                    TopNestedOperator,
                    ProjectOperator,
                    ProjectAwayOperator,
                    ProjectByNamesOperator,
                    ProjectKeepOperator,
                    ProjectRenameOperator,
                    ProjectReorderOperator,
                    SummarizeOperator,
                    DistinctOperator,
                    TopHittersOperator,
                    TopOperator,
                    SortOperator,
                    MvExpandOperator,
                    MvApplyOperator,
                    ReduceByOperator,
                    SampleOperator,
                    SampleDistinctOperator,
                    AsOperator,
                    InvokeOperator,
                    ExecuteAndCacheOperator,
                    ScanOperator,
                    QueryOperator.Hide()); // allow other query operators to parser, but fail in binding

            ForkPipeExpressionCore =
                ApplyZeroOrMore(
                    Rule(ForkPipeOperator, o => (Expression)o),
                    _left =>
                        Rule(_left, Token(SyntaxKind.BarToken), Required(ForkPipeOperator, CreateMissingQueryOperator),
                            (left, pipeToken, right) => (Expression)new PipeExpression(left, pipeToken, right)));

            var InitialPipeElementExpression =
                First(
                    Rule(PrePipeQueryOperator, o => (Expression)o),
                    If(Not(ScanSimpleName), // allow post-pipe operators that don't start with a legal name to parse and fail in binding
                        Rule(PostPipeQueryOperator, o => (Expression)o).Hide()), 
                    UnnamedExpression);

            this.FollowingPipeElementExpression =
                First(
                    PostPipeQueryOperator,
                    PrePipeQueryOperator.Hide(), // allow any pre-pipe operator, but fail in binding
                    BadQueryOperator.Hide()); // allow these to parse, but fail in binding

            PipeExpressionCore =
                ApplyZeroOrMore(
                    InitialPipeElementExpression,
                    _left =>
                        Rule(_left, Token(SyntaxKind.BarToken), Required(FollowingPipeElementExpression, CreateMissingQueryOperator),
                            (left, op, right) => (Expression)new PipeExpression(left, op, right)));

            PipeSubExpressionCore =
                ApplyZeroOrMore(
                    First(
                        PostPipeQueryOperator.Cast<Expression>(),
                        InitialPipeElementExpression.Hide()),
                    _left =>
                        Rule(_left, Token(SyntaxKind.BarToken), Required(FollowingPipeElementExpression, CreateMissingQueryOperator),
                            (left, op, right) => (Expression)new PipeExpression(left, op, right)));

            ContextualSubExpressionCore =
                First(
                    ApplyZeroOrMore(
                        ContextualDataTableExpression,
                        _left =>
                            Rule(_left, Token(SyntaxKind.BarToken), Required(FollowingPipeElementExpression, CreateMissingQueryOperator),
                                (left, op, right) => (Expression)new PipeExpression(left, op, right))),
                    PipeSubExpression);

            UnnamedExpressionCore =
                LogicalOr;

            ExpressionCore =
                PipeExpressionCore;
#endregion

#region Statements
            var AliasStatement =
                Rule(
                    Token(SyntaxKind.AliasKeyword, CompletionKind.QueryPrefix).Hide(),
                    RequiredToken(SyntaxKind.DatabaseKeyword),
                    Required(SimpleNameDeclaration, CreateMissingNameDeclaration),
                    RequiredToken(SyntaxKind.EqualToken),
                    Required(UnnamedExpression, CreateMissingExpression),
                    (aliasKeyword, databaseKeyword, name, equalToken, expression) =>
                        (Statement)new AliasStatement(aliasKeyword, databaseKeyword, name, equalToken, expression))
                .WithTag("<alias>");

            var MaterializeExpression =
                Rule(
                    Token(SyntaxKind.MaterializeKeyword),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Required(PipeExpression, CreateMissingExpression),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (keyword, openParen, expr, closeParen) =>
                        (Expression)new MaterializeExpression(keyword, openParen, expr, closeParen));

            var DefaultValueDeclaration =
                Rule(
                    Token(SyntaxKind.EqualToken),
                    Required(First(Literal, NameTokenLiteral), CreateMissingExpression),
                    (equalToken, value) => new DefaultValueDeclaration(equalToken, value));

            FunctionParameterCore =
                Rule(
                    NameAndTypeDeclaration,
                    Optional(DefaultValueDeclaration),
                    (nameAndType, defaultValue) => new FunctionParameter(nameAndType, defaultValue));

            this.FunctionParameters =
                Rule(
                    RequiredToken(SyntaxKind.OpenParenToken),
                    CommaList(FunctionParameter, CreateMissingFunctionParameter, oneOrMore: false),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (openParen, parameters, closeParen) => new FunctionParameters(openParen, parameters, closeParen));

            var FunctionBodyStatement =
                First(
                    Rule(LetStatement, RequiredToken(SyntaxKind.SemicolonToken),
                        (statement, semicolon) => new SeparatedElement<Statement>(statement, semicolon)),
                    Rule(DeclareQueryParametersStatement, RequiredToken(SyntaxKind.SemicolonToken),
                        (statement, semicolon) => new SeparatedElement<Statement>(statement, semicolon)));

            var FunctionBodyStatementList =
                List(FunctionBodyStatement, fnMissingElement: CreateMissingStatementElement, oneOrMore: false)
                .WithTag("<statement-list>");

            this.FunctionBody =
                Rule(
                    RequiredToken(SyntaxKind.OpenBraceToken),
                    FunctionBodyStatementList,
                    Optional(Expression),
                    Optional(Token(SyntaxKind.SemicolonToken)),
                    RequiredToken(SyntaxKind.CloseBraceToken),
                    (openBrace, statements, expr, semicolon, closeBrace) =>
                        new FunctionBody(openBrace, statements, expr, semicolon, closeBrace));

            var FunctionDeclaration =
                Rule(
                    Optional(Token(SyntaxKind.ViewKeyword)),
                    FunctionParameters,
                    FunctionBody,
                    (view, parameters, body) => new FunctionDeclaration(view, parameters, body))
                .WithTag("<function-declaration>");

            LetStatementCore =
                First(
                    // looks like let with function declaration?
                    If(
                        And(Token(SyntaxKind.LetKeyword, CompletionKind.QueryPrefix),
                            ScanSimpleName,
                            Token(SyntaxKind.EqualToken),
                            Optional(Token(SyntaxKind.ViewKeyword)),
                            Token(SyntaxKind.OpenParenToken),
                            Or(Token(SyntaxKind.CloseParenToken), Token(SyntaxKind.AsteriskToken), And(ScanSimpleName, Token(SyntaxKind.ColonToken)))),
                        Rule(
                            Token(SyntaxKind.LetKeyword),
                            SimpleNameDeclaration,
                            Token(SyntaxKind.EqualToken),
                            FunctionDeclaration,
                            (letKeyword, name, equal, expression) =>
                                (Statement)new LetStatement(letKeyword, name, equal, expression))),
                    // let with materialize?
                    If(
                        And(
                            Token(SyntaxKind.LetKeyword, CompletionKind.QueryPrefix),
                            ScanSimpleName,
                            Token(SyntaxKind.EqualToken),
                            Token(SyntaxKind.MaterializeKeyword)),
                        Rule(
                            Token(SyntaxKind.LetKeyword),
                            Required(SimpleNameDeclaration, CreateMissingNameDeclaration),
                            Token(SyntaxKind.EqualToken),
                            MaterializeExpression,
                            (keyword, name, equal, expr) =>
                                (Statement)new LetStatement(keyword, name, equal, expr))),
                    If(
                        And(
                            Token(SyntaxKind.LetKeyword, CompletionKind.QueryPrefix),
                            ScanSimpleName,
                            Token(SyntaxKind.EqualToken),
                            Token(SyntaxKind.EntityGroupKeyword)),
                        Rule(
                            Token(SyntaxKind.LetKeyword),
                            Required(SimpleNameDeclaration, CreateMissingNameDeclaration),
                            Token(SyntaxKind.EqualToken),
                            EntityGroup,
                            (keyword, name, equal, expr) =>
                                (Statement)new LetStatement(keyword, name, equal, expr))),
                    // otherwise regular let statement
                    Rule(
                        Token(SyntaxKind.LetKeyword, CompletionKind.QueryPrefix),
                        Required(SimpleNameDeclaration, CreateMissingNameDeclaration),
                        RequiredToken(SyntaxKind.EqualToken),
                        Required(Expression, CreateMissingExpression),
                        (letKeyword, name, equalToken, expression) =>
                            (Statement)new LetStatement(letKeyword, name, equalToken, expression)));

            var OptionValueClause =
                Rule(
                    Token(SyntaxKind.EqualToken),
                    Required(UnnamedExpression, CreateMissingExpression),
                    (equal, expr) => new OptionValueClause(equal, expr));

            var SetOptionStatement =
                Rule(
                    Token(SyntaxKind.SetKeyword, CompletionKind.QueryPrefix),
                    Required(SimpleNameDeclaration, CreateMissingNameDeclaration),
                    Optional(OptionValueClause),
                    (keyword, name, value) =>
                        (Statement)new SetOptionStatement(keyword, name, value))
                .WithTag("<set-option>");

            DeclareQueryParametersStatementCore =
                Rule(
                    Token(SyntaxKind.DeclareKeyword, CompletionKind.QueryPrefix).Hide(),
                    RequiredToken(SyntaxKind.QueryParametersKeyword),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    CommaList(FunctionParameter, CreateMissingFunctionParameter, oneOrMore: true),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (declareKeyword, queryParametersKeyword, open, list, close) =>
                        (Statement)new QueryParametersStatement(declareKeyword, queryParametersKeyword, open, list, close));

            var Restriction =
                First(
                    If(ScanWildcardedEntityReferenceOrFunctionCall, WildcardedEntityReference),
                    SimpleNameReference)
                    .WithCompletionHint(CompletionHint.Table | CompletionHint.MaterializedView | CompletionHint.ExternalTable | CompletionHint.GraphModel);

            var RestrictStatementWithClause = 
                Rule(
                    Token(SyntaxKind.WithKeyword),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    QueryParameterCommaList(QueryOperatorParameters.RestrictStatementParameters),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (withKeyword, openParen, properties, closeParen) =>
                        new RestrictStatementWithClause(withKeyword, openParen, properties, closeParen));

            var RestrictStatement =
                Rule(
                    Token(SyntaxKind.RestrictKeyword, CompletionKind.QueryPrefix).Hide(),
                    RequiredToken(SyntaxKind.AccessKeyword),
                    RequiredToken(SyntaxKind.ToKeyword),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    CommaList<Expression>(Restriction, CreateMissingExpression, oneOrMore: true),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    Optional(RestrictStatementWithClause),
                    (restrictKeyword, accessKeyword, toKeyword, openParen, list, closeParen, withProperties) =>
                        (Statement)new RestrictStatement(restrictKeyword, accessKeyword, toKeyword, openParen, list, closeParen, withProperties))
                .WithTag("<restrict>");

            var PatternPathValue =
                Rule(
                    Token(SyntaxKind.DotToken),
                    RequiredToken(SyntaxKind.OpenBracketToken),
                    Required(StringLiteral, CreateMissingStringLiteral),
                    RequiredToken(SyntaxKind.CloseBracketToken),
                    (dot, openBracket, value, closeBracket) =>
                        new PatternPathValue(dot, openBracket, value, closeBracket));

            var PatternMatchStatementElement =
                Rule(
                    LetStatement,
                    RequiredToken(SyntaxKind.SemicolonToken),
                    (statement, semicolon) =>
                        new SeparatedElement<Statement>(statement, semicolon));

            var PatternMatchBody =
                Rule(
                    RequiredToken(SyntaxKind.OpenBraceToken),
                    List(PatternMatchStatementElement, CreateMissingStatementElement, oneOrMore: false),
                    Optional(Expression),
                    Optional(Token(SyntaxKind.SemicolonToken)),
                    RequiredToken(SyntaxKind.CloseBraceToken),
                    (open, statements, expression, semi, close) =>
                        new FunctionBody(open, statements, expression, semi, close));

            var PatternMatchValue =
                First(
                    StringLiteral,
                    UnnamedExpression.Hide());

            var PatternMatch =
                Rule(
                    Token(SyntaxKind.OpenParenToken),
                    CommaList<Expression>(PatternMatchValue, CreateMissingStringLiteral, oneOrMore: true),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    Optional(PatternPathValue),
                    RequiredToken(SyntaxKind.EqualToken),
                    PatternMatchBody,
                    RequiredToken(SyntaxKind.SemicolonToken),
                    (openParen, exprs, closeParen, path, equalToken, body, semicolon) =>
                        new PatternMatch(new Syntax.ExpressionList(openParen, exprs, closeParen), path, equalToken, body, semicolon));

            var PatternPathParameter =
                Rule(
                    Token(SyntaxKind.OpenBracketToken),
                    Required(NameAndTypeDeclaration, CreateMissingNameAndTypeDeclaration),
                    RequiredToken(SyntaxKind.CloseBracketToken),
                    (openBracket, parameter, closeBracket) =>
                        new PatternPathParameter(openBracket, parameter, closeBracket));

            var PatternDeclaration =
                Rule(
                    Token(SyntaxKind.EqualToken),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    CommaList(NameAndTypeDeclaration, CreateMissingNameAndTypeDeclaration, oneOrMore: false),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    Optional(PatternPathParameter),
                    RequiredToken(SyntaxKind.OpenBraceToken),
                    List(PatternMatch, fnMissingElement: CreateMissingPatternMatch, oneOrMore: true),
                    RequiredToken(SyntaxKind.CloseBraceToken),

                    (equal, openParen, parameters, closeParen, pathParameter, openBrace, patterns, closeBrace) =>
                        new PatternDeclaration(equal, openParen, parameters, closeParen, pathParameter, openBrace, patterns, closeBrace));

            var DeclarePatternStatement =
                Rule(
                    Token(SyntaxKind.DeclareKeyword, CompletionKind.QueryPrefix).Hide(),
                    Token(SyntaxKind.PatternKeyword),
                    Required(SimpleNameDeclaration, CreateMissingNameDeclaration),
                    Optional(PatternDeclaration),
                    (declareKeyword, patternKeyword, name, pattern) =>
                        (Statement)new PatternStatement(declareKeyword, patternKeyword, name, pattern))
                .WithTag("<pattern-statement>");

            var QueryStatement =
                Rule(
                    Expression,
                    expr => (Statement)new ExpressionStatement(expr));

            var PrimaryPathSelector =
                ApplyOptional(
                    PathElementSelector,
                    _left =>
                        Rule(
                            _left,
                            DataScopeClause(CompletionKind.TabularSuffix),
                            (path, clause) =>
                                (Expression)new DataScopeExpression(path, clause)));

            var ParenthesizedSummarizeOperator =
                Rule(
                    Token("("),
                    Required(SummarizeOperator, CreateMissingQueryOperator),
                    RequiredToken(")"),
                    (openParen, summarize, closeParen) =>
                        (Expression)new ParenthesizedExpression(openParen, summarize, closeParen));

            var MaterializedViewCombineViewNameClause =
                Rule(
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Required(Expression, CreateMissingExpression).WithCompletionHint(CompletionHint.Literal),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (open, expression, close) =>
                        new MaterializedViewCombineNameClause(open, expression, close));

            var MaterializedViewCombineBaseClause =
                Rule(
                    Token("base")
                        .WithCompletion(new CompletionItem(CompletionKind.Syntax, "base", "base (", ")")),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Required(Expression, CreateMissingExpression).WithCompletionHint(CompletionHint.Table),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (keyword, open, expression, close) =>
                        new MaterializedViewCombineClause(keyword, open, expression, close));

            var MaterializedViewCombineDeltaClause =
                Rule(
                    Token("delta")
                        .WithCompletion(new CompletionItem(CompletionKind.Syntax, "delta", "delta (", ")")),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Required(Expression, CreateMissingExpression).WithCompletionHint(CompletionHint.NonScalar),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (keyword, open, expression, close) =>
                        new MaterializedViewCombineClause(keyword, open, expression, close));

            var MaterializedViewCombineAggregationsClause =
                Rule(
                    Token("aggregations")
                        .WithCompletion(new CompletionItem(CompletionKind.Syntax, "aggregations", "aggregations (summarize ", ")")),
                    RequiredToken(SyntaxKind.OpenParenToken),
                    Required(SummarizeOperator, CreateMissingQueryOperator).WithCompletionHint(CompletionHint.Query),
                    RequiredToken(SyntaxKind.CloseParenToken),
                    (keyword, open, summarize, close) =>
                        new MaterializedViewCombineClause(keyword, open, summarize, close));

            var MaterializedViewCombineExpression =
                Rule(
                    Token(SyntaxKind.MaterializedViewCombineKeyword, CompletionKind.TabularPrefix).Hide(),
                    Required(MaterializedViewCombineViewNameClause, CreateMissingMaterializedViewCombineNameClause),
                    Required(MaterializedViewCombineBaseClause, CreateMissingMaterializedViewCombineBaseClause),
                    Required(MaterializedViewCombineDeltaClause, CreateMissingMaterializedViewCombineDeltaClause),
                    Required(MaterializedViewCombineAggregationsClause, CreateMissingMaterializedViewCombineAggregatesClause),
                    (keyword, viewname, baseClause, deltaClause, aggregatesClause) =>
                        (Expression)new MaterializedViewCombineExpression(keyword, viewname, baseClause, deltaClause, aggregatesClause));

            var ScanKeywordInNamePosition =
                Match((source, start) =>
                    QueryParser.IsKeywordInNamePosition(source, start) ? 1 : -1);

            PrimaryExpressionCore =
                First(
                    Literal,
                    ParenthesizedExpression,
                    DataTableExpression,
                    ContextualDataTableExpression,
                    ExternalDataExpression,
                    InlineExternalTableExpression,
                    MaterializedViewCombineExpression,
                    PrimaryPathSelector,
                    InvalidKeywordAsNameReference);

            this.Statement =
                First(
                    AliasStatement,
                    LetStatement,
                    SetOptionStatement,
                    DeclarePatternStatement,
                    DeclareQueryParametersStatement,
                    RestrictStatement,
                    QueryStatement)
                .WithTag("<statement>");
#endregion

#region QueryBlock
            this.StatementList =
                SeparatedList(
                    Statement, SyntaxKind.SemicolonToken, 
                    fnMissingElement: CreateMissingStatement, 
                    endOfList: EndOfText,
                    allowTrailingSeparator: true);

            this.SkippedTokens =
                Convert(
                    OneOrMore(AnyTokenButEnd),
                    (IReadOnlyList<LexicalToken> list) => 
                        new SkippedTokens(new SyntaxList<SyntaxToken>(
                            list.Select((tok, i) =>
                                i == 0 // only tag first token with diagnostic
                                    ? SyntaxToken.From(tok, DiagnosticFacts.GetIncompleteFragment()) 
                                    : SyntaxToken.From(tok)))))
                .WithTag("<skipped-tokens>");

            this.Directive =
                Rule(
                    Token(SyntaxKind.DirectiveToken),
                    token => new Directive(token));

            this.QueryBlock =
                Rule(
                    List(Directive),
                    StatementList,
                    Optional(SkippedTokens),
                    Optional(Token(SyntaxKind.EndOfTextToken)),

                    (directives, statements, skipped, end) =>
                        new QueryBlock(directives, statements, skipped, end));
#endregion
        }

#region Missing Element Factories
        public static NameDeclaration CreateMissingNameDeclaration(Source<LexicalToken> source = null, int start = 0) =>
            new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetMissingName() });

        public static Expression CreateMissingNameDeclarationExpression(Source<LexicalToken> source, int start) =>
           new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetMissingName() });

        public static Expression CreateMissingNameReference(Source<LexicalToken> source, int start) =>
            new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetMissingName() });

        public static SyntaxToken CreateMissingNameToken(Source<LexicalToken> source, int start) =>
            SyntaxToken.Missing(SyntaxKind.IdentifierToken);

        public static Expression CreateMissingExpression(Source<LexicalToken> source = null, int start = 0)
        {
            // check to see if following token was a keyword and if so report enhanced diagnostic
            var dx = (source != null && source.Peek(start) is LexicalToken token && token.Kind.IsKeyword())
                ? DiagnosticFacts.GetMissingExpressionWithKeyword(token.Text)
                : DiagnosticFacts.GetMissingExpression();
            return new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { dx });
        }

        public static NamedExpression CreateMissingNamedExpression(Source<LexicalToken> source, int start) =>
            new SimpleNamedExpression(
                new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.EqualToken),
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                new[] { DiagnosticFacts.GetMissingName() });

        public static ScanAssignment CreateMissingScanAssignment(Source<LexicalToken> source, int start) =>
            new ScanAssignment(
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.EqualEqualToken),
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                new[] { DiagnosticFacts.GetMissingName() });

        public static Expression CreateMissingValue(Source<LexicalToken> source, int start) =>
            new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetMissingValue() });

        public static TypeExpression CreateMissingType(Source<LexicalToken> source, int start) =>
            new PrimitiveTypeExpression(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetMissingTypeName() });

        public static Expression CreateMissingLongLiteral(Source<LexicalToken> source, int start) =>
            new LiteralExpression(SyntaxKind.LongLiteralExpression, SyntaxToken.Missing(SyntaxKind.LongLiteralToken), new[] { DiagnosticFacts.GetMissingNumber() });

        public static Expression CreateMissingRealLiteral(Source<LexicalToken> source, int start) =>
            new LiteralExpression(SyntaxKind.RealLiteralExpression, SyntaxToken.Missing(SyntaxKind.RealLiteralToken), new[] { DiagnosticFacts.GetMissingNumber() });

        public static Expression CreateMissingStringLiteral(Source<LexicalToken> source, int start) =>
            new LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxToken.Missing(SyntaxKind.StringLiteralToken), new[] { DiagnosticFacts.GetMissingString() });

        public static Expression CreateMissingBooleanLiteral(Source<LexicalToken> source, int start) =>
            new LiteralExpression(SyntaxKind.BooleanLiteralExpression, SyntaxToken.Missing(SyntaxKind.BooleanLiteralToken), new[] { DiagnosticFacts.GetMissingBoolean() });

        public static Expression CreateMissingTypeOfLiteral(Source<LexicalToken> source, int start) =>
            new TypeOfLiteralExpression(
                SyntaxToken.Missing(SyntaxKind.TypeOfKeyword),
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                SyntaxList<SeparatedElement<Expression>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingTypeOfLiteral() });

        public static Expression CreateMissingJsonValue(Source<LexicalToken> source, int start) =>
            new LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                SyntaxToken.Missing(SyntaxKind.StringLiteralToken),
                new[] { DiagnosticFacts.GetMissingJsonValue() });

        public static JsonPair CreateMissingJsonPair(Source<LexicalToken> source, int start) =>
            new JsonPair(
                SyntaxToken.Missing(SyntaxKind.StringLiteralToken),
                SyntaxToken.Missing(SyntaxKind.ColonToken),
                new LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxToken.Missing(SyntaxKind.StringLiteralToken)),
                new[] { DiagnosticFacts.GetMissingJsonPair() });

        public static JoinConditionClause CreateMissingJoinOnClause(Source<LexicalToken> source, int start) =>
            new JoinOnClause(
                SyntaxToken.Missing(SyntaxKind.JoinOnClause),
                SyntaxList<SeparatedElement<Expression>>.Empty(),
                new[] { DiagnosticFacts.GetMissingJoinOnClause() });

        private static ExpressionList CreateMissingArgumentList(Source<LexicalToken> source = null, int start = 0) =>
            new ExpressionList(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken, DiagnosticFacts.GetTokenExpected(SyntaxKind.OpenParenToken)),
                SyntaxList<SeparatedElement<Expression>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken, DiagnosticFacts.GetTokenExpected(SyntaxKind.CloseParenToken)));

        public static FunctionCallExpression CreateMissingFunctionCall(Source<LexicalToken> source, int start) =>
            new FunctionCallExpression(
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                new ExpressionList(
                    SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                    SyntaxList<SeparatedElement<Expression>>.Empty(),
                    SyntaxToken.Missing(SyntaxKind.CloseParenToken)),
                new[] { DiagnosticFacts.GetMissingFunctionCall() });

        public static Expression CreateMissingFunctionCallExpression(Source<LexicalToken> source, int start) =>
            new FunctionCallExpression(
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                new ExpressionList(
                    SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                    SyntaxList<SeparatedElement<Expression>>.Empty(),
                    SyntaxToken.Missing(SyntaxKind.CloseParenToken)),
                new[] { DiagnosticFacts.GetMissingFunctionCall() });

        public static SchemaTypeExpression CreateMissingSchemaType(Source<LexicalToken> source, int start) =>
            new SchemaTypeExpression(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                SyntaxList<SeparatedElement<Expression>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingSchemaDeclaration() });

        public static RowSchema CreateMissingRowSchema(Source<LexicalToken> source, int start) =>
            new RowSchema(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                null,
                SyntaxList<SeparatedElement<NameAndTypeDeclaration>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingSchemaDeclaration() });

        public static InlineExternalTableKindClause CreateMissingInlineExternalTableKindClause(Source<LexicalToken> source, int start) =>
            new InlineExternalTableKindClause(
                SyntaxToken.Missing(SyntaxKind.KindKeyword),
                SyntaxToken.Missing(SyntaxKind.EqualToken),
                SyntaxToken.Missing(SyntaxKind.StringLiteralToken),
                new[] { DiagnosticFacts.GetMissingExternalTableKind() });

        public static InlineExternalTableDataFormatClause CreateMissingInlineExternalTableDataFormatClause(Source<LexicalToken> source, int start) =>
            new InlineExternalTableDataFormatClause(
                SyntaxToken.Missing(SyntaxKind.DataFormatKeyword),
                SyntaxToken.Missing(SyntaxKind.EqualToken),
                SyntaxToken.Missing(SyntaxKind.StringLiteralToken),
                new[] { DiagnosticFacts.GetMissingDataFormat() });

        public static InlineExternalTableConnectionStringsClause CreateMissingInlineExternalTableConnectionStringsClause(Source<LexicalToken> source, int start) =>
            new InlineExternalTableConnectionStringsClause(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                SyntaxList<SeparatedElement<Expression>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingConnectionStrings() });

        public static InlineExternalTablePathFormatPartitionColumnReference CreateMissingPathFormatTokens(Source<LexicalToken> source, int start) =>
            new InlineExternalTablePathFormatPartitionColumnReference(
                CreateMissingExpression(),
                new LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxToken.Missing(SyntaxKind.StringLiteralToken)),
                new[] { DiagnosticFacts.GetMissingPathFormatTokens() });


        public static PartitionColumnDeclaration CreateMissingPartitionColumnDeclaration(Source<LexicalToken> source, int start) =>
            new PartitionColumnDeclaration(
                new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.ColonToken),
                new PrimitiveTypeExpression(SyntaxToken.Missing(SyntaxKind.StringKeyword)),
                SyntaxToken.Missing(SyntaxKind.EqualToken),
                CreateMissingExpression(source, start),
                new[] { DiagnosticFacts.GetMissingPartitionColumnDeclaration() });

        public static EvaluateRowSchema CreateMissingEvaluateRowSchema(Source<LexicalToken> source, int start) =>
            new EvaluateRowSchema(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                null,
                null,
                null,
                SyntaxList<SeparatedElement<NameAndTypeDeclaration>>.Empty(),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingSchemaDeclaration() });

        public static QueryOperator CreateMissingQueryOperator(Source<LexicalToken> source, int start) =>
            new BadQueryOperator(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetQueryOperatorExpected() });

        public static Expression CreateMissingQueryOperatorExpression(Source<LexicalToken> source, int start) =>
            new BadQueryOperator(SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { DiagnosticFacts.GetQueryOperatorExpected() });

        public static MakeSeriesExpression CreateMissingMakeSeriesExpression(Source<LexicalToken> source, int start) =>
            new MakeSeriesExpression(CreateMissingExpression(source, start), null);

        public static MvExpandExpression CreateMissingMvExpandExpression(Source<LexicalToken> source, int start) =>
            new MvExpandExpression(CreateMissingExpression(source, start), null);

        public static MvApplyExpression CreateMissingMvApplyExpression(Source<LexicalToken> source, int start) =>
            new MvApplyExpression(CreateMissingExpression(source, start), null);

        public static MvApplySubqueryExpression CreateMissingMvApplySubqueryExpression(Source<LexicalToken> source, int start) =>
            new MvApplySubqueryExpression(
                CreateMissingToken(SyntaxKind.OpenParenToken),
                CreateMissingExpression(source, start),
                CreateMissingToken(SyntaxKind.CloseParenToken));

        public static ForkExpression CreateMissingForkExpression(Source<LexicalToken> source, int start) =>
            new ForkExpression(
                null,
                CreateMissingToken(SyntaxKind.OpenParenToken),
                CreateMissingExpression(source, start),
                CreateMissingToken(SyntaxKind.CloseParenToken));

        public static PartitionOperand CreateMissingPartitionOperand(Source<LexicalToken> source, int start) =>
            new PartitionSubquery(
                CreateMissingToken(SyntaxKind.OpenParenToken),
                CreateMissingExpression(source, start),
                CreateMissingToken(SyntaxKind.CloseParenToken));

        public static Statement CreateMissingStatement(Source<LexicalToken> source, int start) =>
            new ExpressionStatement(
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                new[] { DiagnosticFacts.GetMissingStatement() });

        public static SeparatedElement<Statement> CreateMissingStatementElement(Source<LexicalToken> source, int start) =>
            new SeparatedElement<Statement>(CreateMissingStatement(source, start));

        public static NameAndTypeDeclaration CreateMissingNameAndTypeDeclaration(Source<LexicalToken> source, int start) =>
            new NameAndTypeDeclaration(
                new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.ColonToken),
                new PrimitiveTypeExpression(SyntaxToken.Missing(SyntaxKind.StringKeyword)),
                new[] { DiagnosticFacts.GetMissingParameter() });

        public static FunctionParameter CreateMissingFunctionParameter(Source<LexicalToken> source, int start) =>
            new FunctionParameter(
                new NameAndTypeDeclaration(
                    new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                    SyntaxToken.Missing(SyntaxKind.ColonToken),
                    new PrimitiveTypeExpression(SyntaxToken.Missing(SyntaxKind.StringKeyword))),
                 null,
                 new[] { DiagnosticFacts.GetMissingParameter() });

        public static NamedParameter CreateMissingNamedParameter(Source<LexicalToken> source, int start) =>
            new NamedParameter(
                new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.EqualToken),
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                CompletionHint.None,
                new[] { DiagnosticFacts.GetMissingParameter() });

        public static FunctionDeclaration CreateMissingFunctionDeclaration(Source<LexicalToken> source, int start) =>
             new FunctionDeclaration(null,
                new FunctionParameters(
                    SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                    SyntaxList<SeparatedElement<FunctionParameter>>.Empty(),
                    SyntaxToken.Missing(SyntaxKind.CloseParenToken)),
                new FunctionBody(
                    SyntaxToken.Missing(SyntaxKind.OpenBraceToken),
                    SyntaxList<SeparatedElement<Statement>>.Empty(),
                    null,
                    null,
                    SyntaxToken.Missing(SyntaxKind.CloseBraceToken)),
                new[] { DiagnosticFacts.GetMissingFunctionDeclaration() });

        public static Func<Source<LexicalToken>, int, Expression> CreateMissingTokenLiteral(IReadOnlyList<string> tokens)
        {
            var diagnostic = DiagnosticFacts.GetTokenExpected(tokens);
            return (source, start) => new LiteralExpression(SyntaxKind.TokenLiteralExpression,
                SyntaxToken.Missing(SyntaxKind.IdentifierToken), new[] { diagnostic });
        }

        public static Func<Source<LexicalToken>, int, Expression> CreateMissingTokenLiteral(params string[] tokens) =>
            CreateMissingTokenLiteral((IReadOnlyList<string>)tokens);

        private static MakeGraphTableAndKeyClause CreateMissingMakeGraphTableAndKeyClause(Source<LexicalToken> source, int start) =>
            new MakeGraphTableAndKeyClause(
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.OnKeyword),
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                new[] { DiagnosticFacts.GetMissingExpression() });

        private static GraphMatchPatternNotation CreateMissingGraphMatchPatternNotation(Source<LexicalToken> source, int start) =>
            new GraphMatchPatternNode(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                new NameDeclaration(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingGraphMatchPatternElement() });

        private static GraphMatchPattern CreateMissingGraphMatchPattern(Source<LexicalToken> source, int start) =>
            new GraphMatchPattern(
                new SyntaxList<GraphMatchPatternNotation>(
                    new[] { CreateMissingGraphMatchPatternNotation(source, start) },
                    new[] { DiagnosticFacts.GetMissingGraphMatchPattern() }
                )
            );

        private static GraphToTableOutputClause CreateMissingGraphToTableOutputClause(Source<LexicalToken> source, int start) =>
            new GraphToTableOutputClause(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                new GraphToTableAsClause(SyntaxToken.Missing(SyntaxKind.OpenParenToken), CreateMissingNameDeclaration()),
                null);

        private static TopNestedClause CreateMissingTopNestedClause(Source<LexicalToken> source = null, int start = 0) =>
            new TopNestedClause(
                SyntaxToken.Missing(SyntaxKind.TopNestedKeyword),
                expression: null,
                ofKeyword: SyntaxToken.Missing(SyntaxKind.OfKeyword),
                ofExpression: new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                withOthersClause: null,
                byKeyword: SyntaxToken.Missing(SyntaxKind.ByKeyword),
                byExpression: new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken)),
                diagnostics: new[] { DiagnosticFacts.GetMissingClause() }
                );

        private static PatternMatch CreateMissingPatternMatch(Source<LexicalToken> source = null, int start = 0) =>
            new PatternMatch(
                new ExpressionList(
                    SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                    SyntaxList<SeparatedElement<Expression>>.Empty(),
                    SyntaxToken.Missing(SyntaxKind.CloseParenToken)),
                null, // path value okay to be null
                SyntaxToken.Missing(SyntaxKind.EqualToken),
                new FunctionBody(
                    SyntaxToken.Missing(SyntaxKind.OpenBraceToken),
                    SyntaxList<SeparatedElement<Statement>>.Empty(),
                    null, // expression okay to be null
                    null, // semicolon okay to be null
                    SyntaxToken.Missing(SyntaxKind.CloseBraceToken)),
                SyntaxToken.Missing(SyntaxKind.SemicolonToken),
                new[] { DiagnosticFacts.GetMissingPatternMatch() });

        public static MaterializedViewCombineClause CreateMissingMaterializedViewCombineBaseClause(Source<LexicalToken> source = null, int start = 0) =>
            new MaterializedViewCombineClause(
                SyntaxToken.Missing(SyntaxKind.MaterializedViewCombineClause),
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                CreateMissingExpression(source, start),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingClause("base") });

        public static MaterializedViewCombineClause CreateMissingMaterializedViewCombineDeltaClause(Source<LexicalToken> source = null, int start = 0) =>
            new MaterializedViewCombineClause(
                SyntaxToken.Missing(SyntaxKind.MaterializedViewCombineClause),
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                CreateMissingExpression(source, start),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingClause("delta") });

        public static MaterializedViewCombineClause CreateMissingMaterializedViewCombineAggregatesClause(Source<LexicalToken> source = null, int start = 0) =>
            new MaterializedViewCombineClause(
                SyntaxToken.Missing(SyntaxKind.MaterializedViewCombineClause),
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                CreateMissingExpression(source, start),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken),
                new[] { DiagnosticFacts.GetMissingClause("aggregates") });

        public static MaterializedViewCombineNameClause CreateMissingMaterializedViewCombineNameClause(Source<LexicalToken> source = null, int start = 0) =>
            new MaterializedViewCombineNameClause(
                SyntaxToken.Missing(SyntaxKind.OpenParenToken),
                CreateMissingExpression(source, start),
                SyntaxToken.Missing(SyntaxKind.CloseParenToken));

        #endregion

        #region other
        private static Parser<LexicalToken, NameDeclaration> AsIdentifierNameDeclaration(Parser<LexicalToken, SyntaxToken> tokenParser) =>
            Rule(tokenParser, (keyword) => new NameDeclaration(keyword));

        private static Parser<LexicalToken, Expression> AsIdentifierNameReference(Parser<LexicalToken, SyntaxToken> tokenParser) =>
            Rule(tokenParser, (keyword) => (Expression)new NameReference(keyword));

        private static Parser<LexicalToken, TypeExpression> AsPrimitiveTypeExpression(Parser<LexicalToken, SyntaxToken> tokenParser) =>
            Rule(tokenParser,
                typeToken => (TypeExpression)new PrimitiveTypeExpression(typeToken));

        private static Parser<LexicalToken, Expression> AsTokenLiteral(Parser<LexicalToken, SyntaxToken> tokenParser) =>
            Rule(tokenParser, (token) => (Expression)new LiteralExpression(SyntaxKind.TokenLiteralExpression, token));

        public static readonly Dictionary<SyntaxKind, SyntaxKind> StringOperatorMap = new Dictionary<SyntaxKind, SyntaxKind>
            {
                { SyntaxKind.EqualTildeToken, SyntaxKind.EqualTildeExpression },
                { SyntaxKind.BangTildeToken, SyntaxKind.BangTildeExpression },
                { SyntaxKind.HasKeyword, SyntaxKind.HasExpression },
                { SyntaxKind.ColonToken, SyntaxKind.SearchExpression },
                { SyntaxKind.NotHasKeyword, SyntaxKind.NotHasExpression },
                { SyntaxKind.HasCsKeyword, SyntaxKind.HasCsExpression },
                { SyntaxKind.NotHasCsKeyword, SyntaxKind.NotHasCsExpression },
                { SyntaxKind.HasPrefixKeyword, SyntaxKind.HasPrefixExpression },
                { SyntaxKind.NotHasPrefixKeyword, SyntaxKind.NotHasPrefixExpression },
                { SyntaxKind.HasPrefixCsKeyword,  SyntaxKind.HasPrefixCsExpression },
                { SyntaxKind.NotHasPrefixCsKeyword, SyntaxKind.NotHasPrefixCsExpression },
                { SyntaxKind.HasSuffixKeyword, SyntaxKind.HasSuffixExpression },
                { SyntaxKind.NotHasSuffixKeyword, SyntaxKind.NotHasSuffixExpression },
                { SyntaxKind.HasSuffixCsKeyword, SyntaxKind.HasSuffixCsExpression },
                { SyntaxKind.NotHasSuffixCsKeyword, SyntaxKind.NotHasSuffixCsExpression },
                { SyntaxKind.LikeKeyword, SyntaxKind.LikeExpression },
                { SyntaxKind.NotLikeKeyword, SyntaxKind.NotLikeExpression },
                { SyntaxKind.LikeCsKeyword, SyntaxKind.LikeCsExpression },
                { SyntaxKind.NotLikeCsKeyword, SyntaxKind.NotLikeCsExpression },
                { SyntaxKind.ContainsKeyword, SyntaxKind.ContainsExpression },
                { SyntaxKind.NotContainsKeyword, SyntaxKind.NotContainsExpression },
                { SyntaxKind.NotBangContainsKeyword, SyntaxKind.NotContainsExpression },
                { SyntaxKind.ContainsCsKeyword, SyntaxKind.ContainsCsExpression },
                { SyntaxKind.Contains_CsKeyword, SyntaxKind.ContainsCsExpression },
                { SyntaxKind.NotContainsCsKeyword, SyntaxKind.NotContainsCsExpression },
                { SyntaxKind.NotBangContainsCsKeyword, SyntaxKind.NotContainsCsExpression },
                { SyntaxKind.StartsWithKeyword, SyntaxKind.StartsWithExpression },
                { SyntaxKind.NotStartsWithKeyword, SyntaxKind.NotStartsWithExpression },
                { SyntaxKind.StartsWithCsKeyword, SyntaxKind.StartsWithCsExpression },
                { SyntaxKind.NotStartsWithCsKeyword, SyntaxKind.NotStartsWithCsExpression },
                { SyntaxKind.EndsWithKeyword, SyntaxKind.EndsWithExpression },
                { SyntaxKind.NotEndsWithKeyword, SyntaxKind.NotEndsWithExpression },
                { SyntaxKind.EndsWithCsKeyword, SyntaxKind.EndsWithCsExpression },
                { SyntaxKind.NotEndsWithCsKeyword, SyntaxKind.NotEndsWithCsExpression },
                { SyntaxKind.MatchesRegexKeyword, SyntaxKind.MatchesRegexExpression }
            };

        private static bool IsTokenVisible(SyntaxKind tokenKind)
        {
            switch (tokenKind)
            {
                case SyntaxKind.LikeCsKeyword:
                case SyntaxKind.LikeKeyword:
                case SyntaxKind.NotLikeCsKeyword:
                case SyntaxKind.NotLikeKeyword:
                case SyntaxKind.ContainsCsKeyword:      // use contains_cs
                case SyntaxKind.NotContainsCsKeyword:   // use !contains_cs
                case SyntaxKind.NotContainsKeyword:     // use !contains
                    return false;
                default:
                    return true;
            }
        }

        // keep this blank line separation before the #endregion to keep BRIDGE.Net from crashing
#endregion
    }
}
