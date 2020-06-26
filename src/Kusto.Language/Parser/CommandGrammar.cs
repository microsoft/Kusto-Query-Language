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
    using System.Text;

    /// <summary>
    /// Parsers for the Kusto command grammar.
    /// </summary>
    public class CommandGrammar
    {
        public Parser<LexicalToken, CommandBlock> CommandBlock { get; }

        private readonly Parser<char, Parser<LexicalToken, SyntaxElement>> commandGrammarParser;
        private readonly Dictionary<CommandSymbol, Parser<LexicalToken, Command>> commandToParserMap;

        private CommandGrammar(
            Parser<LexicalToken, CommandBlock> commandBlockParser,
            Parser<char, Parser<LexicalToken, SyntaxElement>> commandGrammarParser,
            Dictionary<CommandSymbol, Parser<LexicalToken, Command>> commandToParserMap)
        {
            this.CommandBlock = commandBlockParser;
            this.commandGrammarParser = commandGrammarParser;
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

            Parser<LexicalToken, Command> commandCore = null;

            var command = Forward(() => commandCore)
                .WithTag("<command>");

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
            var grammarParser = CreateCommandGrammarParser(globals, command);
            var commandParsers = globals.Commands.Select(c => CreateCommandParser(c, grammarParser)).ToArray();
            var map = Enumerable.Range(0, commandParsers.Length).ToDictionary(i => globals.Commands[i], i => commandParsers[i]);

            // use Best combinator with function to pick which output is better when there are ambiguities
            var bestCommandParsers = Best(commandParsers, (command1, command2) =>
            {
                // neither command has diagnostics, neither is better
                if (!command1.ContainsSyntaxDiagnostics && !command2.ContainsSyntaxDiagnostics)
                    return 0;

                // command1 has diagnostics, command1 is not better than command2
                if (command1.ContainsSyntaxDiagnostics && !command2.ContainsSyntaxDiagnostics)
                    return -1;

                // command2 has diagnostics, command1 is better
                if (!command1.ContainsSyntaxDiagnostics && command2.ContainsSyntaxDiagnostics)
                    return 1;

                var dx1 = command1.GetContainedSyntaxDiagnostics();
                var dx2 = command2.GetContainedSyntaxDiagnostics();

                // command1 first diagnostic occurs lexically after command2 first diagnostics, command1 is better
                if (dx1[0].Start > dx2[0].Start)
                    return 1;

                // command1 first diagnostic occurs lexically before command2 first diagnostic, command1 is not better
                if (dx1[0].Start < dx2[0].Start)
                    return -1;

                // don't compare number of diagnostics, since we want to favor what happens early rather than later

                // otherwise neither is better
                return 0;
            });

            commandCore =
                First(
                    bestCommandParsers, // pick whichever command will successfully consume most input
                    unknownCommand, // fall back for commands that are not defined
                    badCommand) // otherwise its just bad
                .WithTag("<command>");

            var commandOutputPipeExpression =
                ApplyZeroOrMore(
                    command.Cast<Expression>(),
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
                        endOfList: EndOfText,
                        oneOrMore: true,
                        allowTrailingSeparator: true),
                    Optional(q.SkippedTokens), // consumes all remaining tokens
                    Optional(Token(SyntaxKind.EndOfTextToken)),
                    (cmd, skipped, end) =>
                        new CommandBlock(cmd, skipped, end));

            return new CommandGrammar(commandBlock, grammarParser, map);
        }

        private static Statement MissingCommandStatementNode =
            new ExpressionStatement(new BadCommand(SyntaxToken.Missing(SyntaxKind.DotToken), new[] { DiagnosticFacts.GetMissingCommand() }));

        private static Func<Statement> MissingCommandStatement =
            () => (Statement)MissingCommandStatementNode.Clone();

        /// <summary>
        /// Creates a command parser for a <see cref="CommandSymbol"/>
        /// </summary>
        private static Parser<LexicalToken, Command> CreateCommandParser(
            CommandSymbol symbol, 
            Parser<char, Parser<LexicalToken, SyntaxElement>> commandGrammarParser)
        {
            Parser<LexicalToken, Command> commandParser = null;

            try
            {
                var result = commandGrammarParser.Parse(symbol.Grammar);
                var customParser = result.Value;

                if (result.Length != symbol.Grammar.Length)
                {
                    Ensure.IsTrue(result.Length == symbol.Grammar.Length, $"control command grammar {symbol.Name} failed to parse fully at offset ({result.Length}): {symbol.Grammar}");
                }

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
                        return CompletionKind.ScalarPrefix;
                    default:
                        return CompletionKind.Keyword;
                }
            }
            else
            {
                return CompletionKind.Keyword;
            }
        }

        /// <summary>
        /// Gets the command grammar parser associated with the specified <see cref="GlobalState"/>.
        /// </summary>
        public static Parser<char, Parser<LexicalToken, SyntaxElement>> GetCommandGrammarParser(GlobalState globals)
        {
            var grammar = From(globals);
            return grammar.commandGrammarParser;
        }

        /// <summary>
        /// Creates a parser that parsers command grammars to produce command parsers.. obviously.
        /// </summary>
        private static Parser<char, Parser<LexicalToken, SyntaxElement>> CreateCommandGrammarParser(
            GlobalState globals,
            Parser<LexicalToken, Command> command)
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
                    q.BracketedName,
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

            var NameDeclarationOrStringLiteral =
                First(
                    q.SimpleNameDeclarationExpression,
                    q.StringLiteral);

            var KustoNameDeclarationInfo =
                new ParserInfo(
                    NameDeclarationOrStringLiteral.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.None),
                    () => (SyntaxElement)Q.MissingNameDeclaration());

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

            var CommandInput =
                First(
                    If(Token(SyntaxKind.DotToken),
                        command.Cast<SyntaxElement>()),
                    q.StatementList.Cast<SyntaxElement>());

            var KustoCommandInputInfo =
                new ParserInfo(
                    CommandInput,
                    new CustomElementDescriptor(CompletionHint.Tabular),
                    () => (SyntaxElement)Q.MissingExpression());

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

            var InputText =
                Convert(InputTextTokens, InputTextBuilder);

            var KustoInputText =
                new ParserInfo(
                    InputText.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(CompletionHint.None),
                    () => (SyntaxElement)SyntaxToken.Other("", "", SyntaxKind.InputTextToken));

            var BracketedInputTextTokens =
                ZeroOrMore(Not(Token("]")));

            var BracketedInputText =
                Convert(BracketedInputTextTokens, InputTextBuilder);

            var KustoBracketedInputText =
                new ParserInfo(
                    BracketedInputText.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(CompletionHint.None),
                    () => (SyntaxElement)SyntaxToken.Other("", "", SyntaxKind.InputTextToken));

            var grammar = GrammarGrammar.CreateParser(
                getRule: name => {
                    switch (name)
                    {
                        case "value": return KustoValueInfo;
                        case "timespan": return KustoValueInfo;
                        case "datetime": return KustoValueInfo;
                        case "string": return KustoStringLiteralInfo;
                        case "bool": return KustoValueInfo;
                        case "long": return KustoValueInfo;
                        case "int": return KustoValueInfo;
                        case "decimal": return KustoValueInfo;
                        case "real": return KustoValueInfo;
                        case "type": return KustoTypeInfo;
                        case "guid": return KustoGuidLiteralInfo;
                        case "name": return KustoNameDeclarationInfo;
                        case "column": return KustoColumnNameInfo;
                        case "table_column": return KustoTableColumnNameInfo;
                        case "database_table_column": return KustoDatabaseTableColumnNameInfo;
                        case "table": return KustoTableNameInfo;
                        case "database_table": return KustoDatabaseTableNameInfo;
                        case "database": return KustoDatabaseNameInfo;
                        case "cluster": return KustoClusterNameInfo;
                        case "function": return KustoFunctionNameInfo;
                        case "function_declaration": return KustoFunctionDeclaration;
                        case "input_query": return KustoCommandInputInfo;
                        case "input_data": return KustoInputText;
                        case "bracketed_input_data": return KustoBracketedInputText;
                        default: return null;
                    }
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

                        bool required = false;
                        for (int i = 0; i < list.Count; i++)
                        {
                            var t = list[i];

                            // everything after first element or first sequence of terms will be required
                            // this enables building appropriate custom nodes with missing elements that cue correct completion.
                            var notRequired = i == 0 || (i == 1 && t.IsTerm);
                            required |= !notRequired;

                            var p = required && !t.Parser.IsOptional
                                ? Required(t.Parser, t.Missing)
                                : t.Parser;

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
                            missingSeparator: null, //sep.Missing,
                            endOfList: null, //notEndOfList
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
                            missingSeparator: null,
                            endOfList: null,
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