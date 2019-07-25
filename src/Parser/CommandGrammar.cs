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
    using Q=QueryGrammar;
    using Utils;

    /// <summary>
    /// Parsers for the Kusto command grammar.
    /// </summary>
    public class CommandGrammar
    {
        public Parser<LexicalToken, CommandBlock> CommandBlock { get; }

        private readonly Dictionary<CommandSymbol, Parser<LexicalToken, Command>> commandToParserMap;

        private CommandGrammar(
            Parser<LexicalToken, CommandBlock> commandBlockParser,
            Dictionary<CommandSymbol, Parser<LexicalToken, Command>> commandToParserMap)
        {
            this.CommandBlock = commandBlockParser;
            this.commandToParserMap = commandToParserMap;
        }

        /// <summary>
        /// Gets or creates the <see cref="CommandGrammar"/> corresponding to the <see cref="GlobalState"/>
        /// </summary>
        public static CommandGrammar From(GlobalState globals)
        {
            if (!globals.Cache.TryGetValue<CommandGrammar>(out var grammar))
            {
                grammar = globals.Cache.GetOrCreate(() => Create(globals));
            }

            return grammar;
        }

        /// <summary>
        /// Creates a new <see cref="CommandGrammar"/> given the <see cref="GlobalState"/>
        /// </summary>
        private static CommandGrammar Create(GlobalState globals)
        {
            var q = Q.From(globals);

            var unknownCommandToken =
                If(Not(First(
                    Token(SyntaxKind.BarToken),
                    Token(SyntaxKind.LessThanBarToken),
                    Token(SyntaxKind.EndOfTextToken))),
                    AnyToken);

            var unknownCommand =
                Rule(
                    Token(SyntaxKind.DotToken),
                    OneOrMore(unknownCommandToken,
                        (tokens) => new SyntaxList<SyntaxToken>(tokens)),
                    (dot, parts) => (Command)new UnknownCommand(dot, parts))
                    .WithTag("<unknown-command>");

            var badCommand =
                Rule(
                    Token(SyntaxKind.DotToken),
                    dot => (Command)new BadCommand(dot, new Diagnostic[] { DiagnosticFacts.GetMissingCommand() }))
                    .WithTag("<bad-command>");

            // include parsers for all command symbol grammars
            var grammarParser = GetCommandGrammarParser(globals);
            var commandParsers = globals.Commands.Select(c => CreateCommandParser(c, grammarParser)).ToArray();
            var map = Enumerable.Range(0, commandParsers.Length).ToDictionary(i => globals.Commands[i], i => commandParsers[i]);

            var command =
                First(
                    //createFunctionCommand,
                    Best(commandParsers), // pick whichever command will successfully consume most input
                    unknownCommand, // fall back for commands that are not defined
                    badCommand) // otherwise its just bad
                .WithTag("<command>");

            var commandInputItem =
                First(
                    If(Token(SyntaxKind.DotToken),
                        command.Cast<SyntaxNode>()),
                    q.StatementList.Cast<SyntaxNode>());

            var commandInputExpression =
                ApplyOptional(
                    command.Cast<Expression>(),
                    _left =>
                        Rule(
                            _left,
                            Token(SyntaxKind.LessThanBarToken),
                            Required(commandInputItem, () => (SyntaxNode)Q.MissingExpression()),
                            (cmd, op, expr) =>
                                (Expression)new CommandInputExpression((Command)cmd, op, expr))
                            .WithTag("<command-input-pipe>"));

            var commandOutputPipeExpression =
                ApplyZeroOrMore(
                    commandInputExpression,
                    _left =>
                        Rule(
                            _left,
                            Token(SyntaxKind.BarToken),
                            Required(q.FollowingPipeElementExpression, Q.MissingQueryOperator),
                            (left, op, right) => (Expression)new PipeExpression(left, op, right))
                            .WithTag("<command-output-pipe>"));

            var commandStatement =
                Rule(
                    commandOutputPipeExpression,
                    cmd => (Statement)new ExpressionStatement(cmd));

            var commandBlock =
                Rule(
                    SeparatedList(
                        commandStatement, // first one is a command statement
                        q.Statement,      // all others are query statements
                        SyntaxKind.SemicolonToken,
                        MissingCommandStatementNode,
                        oneOrMore: true,
                        allowTrailingSeparator: true),
                    Optional(q.SkippedTokens), // consumes all remaining tokens
                    Optional(Token(SyntaxKind.EndOfTextToken)),
                    (cmd, skipped, end) =>
                        new CommandBlock(cmd, skipped, end));

            return new CommandGrammar(commandBlock, map);
        }

        private static Statement MissingCommandStatementNode =
            new ExpressionStatement(new BadCommand(SyntaxToken.Missing(SyntaxKind.DotToken), new[] { DiagnosticFacts.GetMissingCommand() }));

        private static Func<Statement> MissingCommandStatement =
            () => (Statement)MissingCommandStatementNode.Clone();

        /// <summary>
        /// Creates a command parser for a <see cref="CommandSymbol"/>
        /// </summary>
        private static Parser<LexicalToken, Command> CreateCommandParser(CommandSymbol symbol, Parser<char, Parser<LexicalToken, SyntaxElement>> commandGrammarParser)
        {
            Parser<LexicalToken, Command> commandParser = null;

            try
            {
                var customParser = commandGrammarParser.Parse(symbol.Grammar).Value;

                if (customParser != null)
                {
                    commandParser = Rule(
                        Token(SyntaxKind.DotToken),
                        customParser,
                        (dot, custom) => (Command)new CustomCommand(symbol.Name, dot, custom))
                        .WithTag($"<{symbol.Name}>");
                }
            }
            finally
            {
            }

#if DEBUG
            if (commandParser == null)
                throw new InvalidOperationException($"invalid command grammar: {symbol.Grammar}");
#endif

            Ensure.NotNull(commandParser, $"invalid command grammar: {symbol.Grammar}");
            commandParser = commandParser ?? Match(t => false, lt => (Command)null);

            return commandParser;
        }

        /// <summary>
        /// Gets the parser for the command.
        /// </summary>
        public Parser<LexicalToken, Command> GetCommandParser(CommandSymbol command)
        {
            this.commandToParserMap.TryGetValue(command, out var parser);
            return parser;
        }

        private static bool IsHex(string text) => text.All(c => TextFacts.IsHexDigit(c));

        private class ParserInfo
        {
            public Parser<LexicalToken, SyntaxElement> Parser { get; }
            public CustomElementDescriptor Element { get; }
            public Func<SyntaxElement> Missing { get; }
            public bool IsTerm { get; }

            public ParserInfo(
                Parser<LexicalToken, SyntaxElement> parser, 
                CustomElementDescriptor element, 
                Func<SyntaxElement> missing,
                bool isTerm = false)
            {
                this.Parser = parser;
                this.Element = element;
                this.Missing = missing;
                this.IsTerm = isTerm;
            }

            public ParserInfo WithElement(CustomElementDescriptor element)
            {
                if (this.Element == element)
                    return this;
                return new ParserInfo(this.Parser, element, this.Missing, this.IsTerm);
            }
        }

        private static CompletionKind GetCompletionKind(OffsetValue<string> textAndOffset)
        {
            if (textAndOffset.Offset == 0)
            {
                return CompletionKind.CommandPrefix;
            }

            if (SyntaxFacts.TryGetKind(textAndOffset.Value, out var kind))
            {
                switch (kind.GetCategory())
                {
                    case SyntaxCategory.Punctuation:
                        return CompletionKind.Punctuation;
                    case SyntaxCategory.Operator:
                        return CompletionKind.ScalarInfix;
                    case SyntaxCategory.Literal:
                        return CompletionKind.Literal;
                    default:
                        return CompletionKind.Keyword;
                }
            }
            else
            {
                return CompletionKind.Keyword;
            }
        }

        public static Parser<char, Parser<LexicalToken, SyntaxElement>> GetCommandGrammarParser(GlobalState globals)
        {
            if (!globals.Cache.TryGetValue<CommandGrammarParser>(out var cgp))
            {
                cgp = globals.Cache.GetOrCreate(() => new CommandGrammarParser(CreateCommandGrammarParser(globals)));
            }

            return cgp.Parser;
        }

        private class CommandGrammarParser
        {
            public Parser<char, Parser<LexicalToken, SyntaxElement>> Parser { get; }

            public CommandGrammarParser(Parser<char, Parser<LexicalToken, SyntaxElement>> parser)
            {
                this.Parser = parser;
            }
        }

        /// <summary>
        /// Creates a parser that parsers command grammars to produce command parsers.. obviously.
        /// </summary>
        private static Parser<char, Parser<LexicalToken, SyntaxElement>> CreateCommandGrammarParser(GlobalState globals)
        {
            var q = Q.From(globals);

            var StringName =
                Rule(Token(SyntaxKind.StringLiteralToken), token => (Name)new TokenName(token));

            var GuidLiteral =
                Convert(
                    And(
                        Match(t => IsHex(t.Text)),
                        ZeroOrMore(
                            And(
                                Match(t => t.Kind == SyntaxKind.MinusToken && t.Trivia.Length == 0),
                                Match(t => IsHex(t.Text) && t.Trivia.Length == 0)))),
                    (IReadOnlyList<LexicalToken> list) =>
                    {
                        var text = string.Concat(list.Select(e => e.Text));
                        return (Expression)new LiteralExpression(SyntaxKind.GuidLiteralExpression,
                            SyntaxToken.Literal(list[0].Trivia, text, SyntaxKind.GuidLiteralToken));
                    }).WithTag("<guid>");

            var Name =
                First(
                    q.IdentifierName,
                    q.BrackettedName,
                    q.BracedName,
                    StringName)
                    .WithTag("<name>");

            var ColumnNameReference =
                Rule(Name, name => (Expression)new NameReference(name, SymbolMatch.Column))
                    .WithCompletionHint(Editor.CompletionHint.Column)
                    .WithTag("<column>");

            var TableNameReference =
                Rule(Name, name => (Expression)new NameReference(name, SymbolMatch.Table))
                    .WithCompletionHint(Editor.CompletionHint.Table)
                    .WithTag("<table>");

            var DatabaseNameReference =
                Rule(Name, name => (Expression)new NameReference(name, SymbolMatch.Database))
                    .WithCompletionHint(Editor.CompletionHint.Database)
                    .WithTag("<database>");

            var ClusterNameReference =
                Rule(Name, name => (Expression)new NameReference(name, SymbolMatch.Cluster))
                    .WithCompletionHint(Editor.CompletionHint.Cluster)
                    .WithTag("<cluster>");

            var DatabaseFunctionNameReference =
                Rule(Name, name => (Expression)new NameReference(name, SymbolMatch.Function))
                    .WithCompletionHint(Editor.CompletionHint.DatabaseFunction)
                    .WithTag("<function>");

            var DatabaseOrTableReference =
                Rule(Name, name => (Expression)new NameReference(name, SymbolMatch.Database | SymbolMatch.Table))
                    .WithCompletionHint(Editor.CompletionHint.Database | Editor.CompletionHint.Table)
                    .WithTag("<database_or_table>");

            var DatabaseTableNameReference =
                ApplyZeroOrMore(
                    DatabaseOrTableReference,
                    _left =>
                        Rule(_left, Token(".").Hide(), Required(TableNameReference, Q.MissingNameReference),
                            (expr, dot, selector) => (Expression)new PathExpression(expr, dot, selector)))
                    .WithTag("<database_table>");

            var DatabaseOrTableOrColumnReference =
                Rule(Name, name => (Expression)new NameReference(name, SymbolMatch.Database | SymbolMatch.Table | SymbolMatch.Column))
                    .WithCompletionHint(Editor.CompletionHint.Database | Editor.CompletionHint.Table | Editor.CompletionHint.Column)
                    .WithTag("<database_or_table_or_column>");

            var DatabaseTableColumnNameReference =
                ApplyZeroOrMore(
                    DatabaseOrTableOrColumnReference,
                    _left =>
                        Rule(_left, Token(".").Hide(), Required(DatabaseOrTableOrColumnReference, Q.MissingNameReference),
                            (expr, dot, selector) => (Expression)new PathExpression(expr, dot, selector)))
                    .WithTag("<database_table_column>");

            var TableOrColumnReference =
                Rule(Name, name => (Expression)new NameReference(name, SymbolMatch.Table | SymbolMatch.Column))
                    .WithCompletionHint(Editor.CompletionHint.Table | Editor.CompletionHint.Column)
                    .WithTag("<table_or_column>");

            var TableColumnNameReference =
                ApplyZeroOrMore(
                    TableOrColumnReference,
                    _left =>
                        Rule(_left, Token(".").Hide(), Required(TableOrColumnReference, Q.MissingNameReference),
                            (expr, dot, selector) => (Expression)new PathExpression(expr, dot, selector)))
                    .WithTag("<table_column>");

            var KustoStringLiteralInfo =
                new ParserInfo(
                    q.StringLiteral.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Literal),
                    () => (SyntaxElement)Q.MissingStringLiteral());

            var KustoGuidLiteralInfo =
                new ParserInfo(
                    GuidLiteral.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Literal),
                    () => (SyntaxElement)Q.MissingValue());

            var KustoValueInfo =
                new ParserInfo(
                    First(GuidLiteral.Cast<SyntaxElement>(), q.Literal.Cast<SyntaxElement>()),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Literal),
                    () => (SyntaxElement)Q.MissingValue());

            var KustoTypeInfo =
                new ParserInfo(
                    q.ParamTypeExtended.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Syntax),
                    () => (SyntaxElement)Q.MissingType());

            var NameOrStringLiteral =
                First(
                    q.SimpleNameDeclarationExpression,
                    q.StringLiteral);

            var KustoNameInfo =
                new ParserInfo(
                    NameOrStringLiteral.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.None),
                    () => (SyntaxElement)Q.MissingNameReference());

            var KustoColumnNameInfo =
                new ParserInfo(
                    ColumnNameReference.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Column),
                    () => (SyntaxElement)Q.MissingNameReference());

            var KustoTableNameInfo =
                new ParserInfo(
                    TableNameReference.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Table),
                    () => (SyntaxElement)Q.MissingNameReference());

            var KustoDatabaseNameInfo =
                new ParserInfo(
                    DatabaseNameReference.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Database),
                    () => (SyntaxElement)Q.MissingNameReference());

            var KustoDatabaseTableNameInfo =
                new ParserInfo(
                    DatabaseTableNameReference.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Table),
                    () => (SyntaxElement)Q.MissingNameReference());

            var KustoClusterNameInfo =
                new ParserInfo(
                    ClusterNameReference.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Cluster),
                    () => (SyntaxElement)Q.MissingNameReference());

            var KustoFunctionNameInfo =
                new ParserInfo(
                    DatabaseFunctionNameReference.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(Editor.CompletionHint.Function),
                    () => (SyntaxElement)Q.MissingNameReference());

            var KustoDatabaseTableColumnNameInfo =
                new ParserInfo(
                    DatabaseTableColumnNameReference.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Column),
                    () => (SyntaxElement)Q.MissingNameReference());

            var KustoTableColumnNameInfo =
                new ParserInfo(
                    TableColumnNameReference.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Column),
                    () => (SyntaxElement)Q.MissingNameReference());

            var KustoFunctionDeclaration =
                new ParserInfo(
                    Rule(
                        q.FunctionParameters,
                        q.FunctionBody,
                        (p, b) => (SyntaxElement)new FunctionDeclaration(null, p, b)),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Syntax),
                    () => new FunctionDeclaration(
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
                                CreateMissingToken(SyntaxKind.CloseBraceToken))));

            var grammar = GrammarParser.Create(
                rules: new Dictionary<string, ParserInfo>()
                {
                    { "value", KustoValueInfo },
                    { "type", KustoTypeInfo },
                    { "string", KustoStringLiteralInfo },
                    { "guid", KustoGuidLiteralInfo },
                    { "name", KustoNameInfo },
                    { "column", KustoColumnNameInfo },
                    { "table_column", KustoTableColumnNameInfo },
                    { "database_table_column", KustoDatabaseTableColumnNameInfo },
                    { "table", KustoTableNameInfo },
                    { "database_table", KustoDatabaseTableNameInfo },
                    { "database", KustoDatabaseNameInfo },
                    { "cluster", KustoClusterNameInfo },
                    { "function", KustoFunctionNameInfo },
                    { "function_declaration", KustoFunctionDeclaration },
                },

                createTerm: textAndOffset =>
                    new ParserInfo(
                        Token(textAndOffset.Value, GetCompletionKind(textAndOffset)).Cast<SyntaxElement>(),
                        new CustomElementDescriptor(hint:Editor.CompletionHint.Syntax),
                        () => CreateMissingToken(textAndOffset.Value),
                        isTerm: true),

                createOptional: elem =>
                    new ParserInfo(
                        Optional(elem.Parser),
                        new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: true),
                        elem.Missing),

                createRequired: elem =>
                    new ParserInfo(
                        Required(elem.Parser, elem.Missing),
                        elem.Element,
                        elem.Missing),

                createTagged: (elem, tag) =>
                    elem.WithElement(elem.Element.WithName(tag)),

                createSequence: list =>
                {
                    if (list.Count == 1)
                    {
                        return list[0];
                    }
                    else
                    {
                        var shape = list.Select(t => t.Element).ToArray();

                        var parsers = new List<Parser<LexicalToken>>();

                        bool term = true;
                        for (int i = 0; i < list.Count; i++)
                        {
                            var t = list[i];

                            // everything after first element or first sequence of terms will be required
                            // this enables building appropriate custom nodes with missing elements that cue correct completion.
                            term &= i == 0 || t.IsTerm;

                            var p = term || t.Parser.IsOptional
                                ? t.Parser
                                : Required(t.Parser, t.Missing);

                            parsers.Add(p);
                        }

                        var parser = Produce<SyntaxElement>(
                            Sequence(parsers.ToArray()),
                            (IReadOnlyList<object> elemList) => 
                                new CustomNode(shape, elemList.Cast<SyntaxElement>().ToArray()));

                        return new ParserInfo(
                            parser, 
                            list[0].Element, 
                            () => new CustomNode(shape, list.Select(t => t.Missing()).ToArray()));
                    }
                },

                createAlternation: list =>
                    new ParserInfo(
                        Best(list.Select(t => t.Parser).ToArray()), 
                        list[0].Element, 
                        list[0].Missing, 
                        list[0].IsTerm),

                createZeroOrMore: elem =>
                    new ParserInfo(
                        List(
                            elem.Parser,
                            elem.Missing,
                            oneOrMore: false,
                            producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                        new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: false),
                        () => new SyntaxList<SyntaxElement>(new SyntaxElement[] { })),

                createOneOrMore: elem =>
                    new ParserInfo(
                        List(
                            elem.Parser,
                            elem.Missing,
                            oneOrMore: true,
                            producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                        new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: false),
                        () => new SyntaxList<SyntaxElement>(new SyntaxElement[] { elem.Missing() })),

                createZeroOrMoreSeparated: (elem, sep) =>
                    new ParserInfo(
                        SeparatedList<SyntaxElement, SyntaxElement>(
                            elem.Parser,
                            sep.Parser,
                            elem.Missing,
                            sep.Missing,
                            oneOrMore: false,
                            allowTrailingSeparator: false,
                            producer: elements => MakeSeparatedList<SyntaxElement>(elements.ToArray())),
                        new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: false),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>[] { })),

                createOneOrMoreSeparated: (elem, sep) =>
                    new ParserInfo(
                        SeparatedList<SyntaxElement, SyntaxElement>(
                            elem.Parser,
                            sep.Parser,
                            elem.Missing,
                            sep.Missing,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: elements => MakeSeparatedList<SyntaxElement>(elements.ToArray())),
                        new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: false),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new[] { new SeparatedElement<SyntaxElement>(elem.Missing()) }))
                );

            return Parsers<char>.Rule(grammar, g => g.Parser);
        }
   }
}