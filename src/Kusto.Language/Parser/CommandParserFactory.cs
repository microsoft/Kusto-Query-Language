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
    using Q = QueryGrammar;
    using Utils;
    using System.Text;

    /// <summary>
    /// A factory that creates parsers for individual control commands.
    /// </summary>
    public class CommandParserFactory
    {
        private readonly GrammarTranslator _translator;

        /// <summary>
        /// Constructs a new <see cref="CommandParserFactory"/>
        /// </summary>
        /// <param name="queryParser">A query parser that can parse the query parts of a command.</param>
        /// <param name="commandParser">An command parser used that represents the set of all parsable commands.</param>
        internal CommandParserFactory(QueryGrammar queryParser, Parser<LexicalToken, Command> commandParser)
        {
            var rulesMap = CreateRulesMap(queryParser, commandParser);
            _translator = new GrammarTranslator(rulesMap);
        }

        /// <summary>
        /// Constructs a new <see cref="CommandParserFactory"/>
        /// </summary>
        /// <param name="queryParser">A query parser that can parse the query parts of a command.</param>
        public CommandParserFactory(QueryGrammar queryParser)
            : this(queryParser, null)
        {
        }

        /// <summary>
        /// Creates the parser for a <see cref="CommandSymbol"/>
        /// </summary>
        public Parser<LexicalToken, Command> CreateCommandParser(CommandSymbol symbol) =>
            CreateCommandParser(symbol.Name, symbol.Grammar);

        /// <summary>
        /// Creates the parser for an entire command (including the intial dot)
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="commandGrammar">The gramamr of the command (does not include the initial dot).</param>
        public Parser<LexicalToken, Command> CreateCommandParser(string commandName, string commandGrammar)
        {
            var contentParser = CreateContentParser(commandName, commandGrammar);
            if (contentParser != null)
            {
                return Rule(
                    Token(SyntaxKind.DotToken),
                    contentParser,
                    (dot, custom) => (Command)new CustomCommand(commandName, dot, custom))
                    .WithTag($"<{commandName}>");
            }
            else
            {
                return Match(t => false, lt => (Command)null);
            }
        }

        public Parser<LexicalToken, Command> CreateCommandParser(string commandName, Grammar contentGrammar)
        {
            var contentParser = CreateContentParser(commandName, contentGrammar);
            if (contentParser != null)
            {
                return Rule(
                    Token(SyntaxKind.DotToken),
                    contentParser,
                    (dot, custom) => (Command)new CustomCommand(commandName, dot, custom))
                    .WithTag($"<{commandName}>");
            }
            else
            {
                return Match(t => false, lt => (Command)null);
            }
        }

        private struct CommandAndGrammar
        {
            public readonly CommandSymbol Symbol;
            public readonly Grammar Grammar;

            public CommandAndGrammar(CommandSymbol symbol, Grammar grammar)
            {
                this.Symbol = symbol;
                this.Grammar = grammar;
            }
        }

        /// <summary>
        /// Creates a unified command parser for all command symbols
        /// </summary>
        public Parser<LexicalToken, Command> CreateCommandParser(IReadOnlyList<CommandSymbol> commandSymbols)
        {
            var commandAndGrammars = commandSymbols.Select(s => new CommandAndGrammar(s, ParseCommandGrammar(s.Name, s.Grammar)))
                .Where(x => x.Grammar != null)
                .ToList();

            var commandAndAdjustedGrammars = commandAndGrammars.Select(
                cag => new CommandAndGrammar(cag.Symbol, CommandGrammarUtils.Adjust(cag.Grammar)))
                .ToList();

            var orderedCommandAndGrammars = GrammarReorderer.Reorder(commandAndAdjustedGrammars, cag => cag.Grammar);

            var commandParsers = orderedCommandAndGrammars.Select(cag => CreateCommandParser(cag.Symbol.Name, cag.Grammar))
                .ToArray();

            // use Best combinator with function to pick which output is better when there are ambiguities
            var bestCommandParsers = Best(commandParsers, (command1, command2) => CompareBetterSyntax(command1, command2));
            return bestCommandParsers;
        }

        private static int CompareBetterSyntax(SyntaxElement command1, SyntaxElement command2)
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
        }

        /// <summary>
        /// Creates the parser for a command's content (everything after the first dot)
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="commandGrammar">The grammar of the command (does not include the initial dot).</param>
        public Parser<LexicalToken, SyntaxElement> CreateContentParser(string commandName, string commandGrammar)
        {
            var contentGrammar = ParseCommandGrammar(commandName, commandGrammar);
            
            if (contentGrammar != null)
            {
                var adjustedGrammar = CommandGrammarUtils.Adjust(contentGrammar);
                return CreateContentParser(commandName, adjustedGrammar);
            }

            return null;
        }

        private Grammar ParseCommandGrammar(string commandName, string commandGrammar)
        {
            try
            {
                if (GrammarParser.TryParse(commandGrammar, out var contentGrammar, out var length))
                {
                    if (length != commandGrammar.Length)
                    {
                        Ensure.IsTrue(false, $"control command grammar {commandName} failed to parse fully at offset ({length}): {commandGrammar}");
                    }

                    return contentGrammar;
                }
                else
                {
                    Ensure.IsTrue(false, $"control command grammar {commandName} failed to parse");
                }
            }
            finally
            {
            }

            return null;
        }

        private Parser<LexicalToken, SyntaxElement> CreateContentParser(string commandName, Grammar contentGrammar)
        {
            var info = _translator.Translate(contentGrammar);
            return info.Parser;
        }

        internal class ParserInfo
        {
            /// <summary>
            /// The parser that is to be used.
            /// </summary>
            public Parser<LexicalToken, SyntaxElement> Parser { get; }

            /// <summary>
            /// The <see cref="CustomElementDescriptor"/> for <see cref="CustomNode"/> child indices expected to contain this parser's output.
            /// </summary>
            public CustomElementDescriptor Element { get; }

            /// <summary>
            /// A function that constructs an empty version of this parsers output when it is expected but missing.
            /// </summary>
            public Func<SyntaxElement> Missing { get; }

            /// <summary>
            /// True if this element is considered a term (default == false)
            /// </summary>
            public bool IsTerm { get; }

            /// <summary>
            /// Creates a <see cref="ParserInfo"/> instance.
            /// </summary>
            /// <param name="parser">The parser that is to be used.</param>
            /// <param name="element">The <see cref="CustomElementDescriptor"/> for <see cref="CustomNode"/> child indices expected to contain this parser's output.</param>
            /// <param name="missing">A function that constructs an empty version of this parsers output when it is expected but missing.</param>
            /// <param name="isTerm">True if this element is considered a term (default == false)</param>
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

        private class GrammarTranslator : GrammarVisitor<ParserInfo>
        {
            private readonly IReadOnlyDictionary<string, ParserInfo> _rulesMap;

            public GrammarTranslator(IReadOnlyDictionary<string, ParserInfo> rulesMap)
            {
                _rulesMap = rulesMap;
            }

            private Grammar _first;

            public ParserInfo Translate(Grammar root)
            {
                if (root is SequenceGrammar seq)
                {
                    _first = seq.Steps[0];
                }

                return root.Accept(this);
            }

            public override ParserInfo VisitAlternation(AlternationGrammar grammar)
            {
                var infos = grammar.Alternatives.Select(a => a.Accept(this)).ToArray();

                return new ParserInfo(
                    //Best(infos.Select(t => GetElementParser(t)).ToArray(), CompareBetterSyntax),
                    First(infos.Select(t => GetElementParser(t)).ToArray()),
                    new CustomElementDescriptor(infos[0].Element.CompletionHint, isOptional: false),
                    infos[0].Missing,
                    infos[0].IsTerm);
            }

            public override ParserInfo VisitOneOrMore(OneOrMoreGrammar grammar)
            {
                var elem = grammar.Repeated.Accept(this);
                var sep = grammar.Separator?.Accept(this);

                if (sep == null)
                {
                    return new ParserInfo(
                        List(
                            GetElementParser(elem),
                            elem.Missing,
                            oneOrMore: true,
                            producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                        new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: false),
                        () => new SyntaxList<SyntaxElement>(new SyntaxElement[] { elem.Missing() }));
                }
                else
                {
                    return new ParserInfo(
                        OList(
                            GetElementParser(elem),
                            sep.Parser,
                            elem.Missing,
                            missingSeparator: null,
                            endOfList: null,
                            oneOrMore: true,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: false),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new[] { new SeparatedElement<SyntaxElement>(elem.Missing()) }));
                }
            }

            public override ParserInfo VisitOptional(OptionalGrammar grammar)
            {
                var elem = grammar.Optioned.Accept(this);

                return new ParserInfo(
                    Optional(GetElementParser(elem)),
                    new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: true),
                    elem.Missing);
            }

            public override ParserInfo VisitRequired(RequiredGrammar grammar)
            {
                var elem = grammar.Required.Accept(this);

                return new ParserInfo(
                    Required(GetElementParser(elem), elem.Missing),
                    new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: false),
                    elem.Missing);
            }

            public override ParserInfo VisitRule(RuleGrammar grammar)
            {
                _rulesMap.TryGetValue(grammar.RuleName, out var info);
                return info;
            }

            public override ParserInfo VisitSequence(SequenceGrammar grammar)
            {
                var list = grammar.Steps.Select(s => s.Accept(this)).ToList();

                if (list.Count == 1)
                {
                    return list[0];
                }
                else
                {
                    var shape = list.Select(t => t.Element).ToArray();

                    var parsers = list.Select(t => t.Parser).ToArray();

                    var parser = Produce<SyntaxElement>(
                        Sequence(parsers.ToArray()),
                        (IReadOnlyList<object> elemList) =>
                            new CustomNode(shape, elemList.Cast<SyntaxElement>().ToArray()));

                    return new ParserInfo(
                        parser,
                        new CustomElementDescriptor(list[0].Element.CompletionHint, isOptional: false),
                        () => new CustomNode(shape, list.Select(t => t.Missing()).ToArray()));
                }
            }

            public override ParserInfo VisitTagged(TaggedGrammar grammar)
            {
                var elem = grammar.Tagged.Accept(this);
                return elem.WithElement(elem.Element.WithName(grammar.Tag));
            }

            public override ParserInfo VisitToken(TokenGrammar grammar)
            {
                return new ParserInfo(
                    Token(grammar.TokenText, GetCompletionKind(grammar)).Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Syntax),
                    () => CreateMissingToken(grammar.TokenText),
                    isTerm: true);
            }

            private CompletionKind GetCompletionKind(TokenGrammar token)
            {
                // the first term is always a command prefix
                if (token == _first)
                {
                    return CompletionKind.CommandPrefix;
                }
                // otherwise its based on the text
                else if (SyntaxFacts.TryGetKind(token.TokenText, out var kind))
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

            public override ParserInfo VisitZeroOrMore(ZeroOrMoreGrammar grammar)
            {
                var elem = grammar.Repeated.Accept(this);
                var sep = grammar.Separator?.Accept(this);

                if (sep == null)
                {
                    return new ParserInfo(
                        List(
                            GetElementParser(elem),
                            elem.Missing,
                            oneOrMore: false,
                            producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                        new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: false),
                        () => new SyntaxList<SyntaxElement>(new SyntaxElement[] { }));
                }
                else
                {
                    return new ParserInfo(
                        OList(
                            GetElementParser(elem),
                            sep.Parser,
                            elem.Missing,
                            missingSeparator: null, //sep.Missing,
                            endOfList: null, //notEndOfList
                            oneOrMore: false,
                            allowTrailingSeparator: false,
                            producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)),
                        new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: false),
                        () => new SyntaxList<SeparatedElement<SyntaxElement>>(new SeparatedElement<SyntaxElement>[] { }));
                }
            }

            private static Parser<LexicalToken, SyntaxElement> GetElementParser(ParserInfo info)
            {
                // element has a tag name then we need to preserve it by wrapping in CustomNode
                // so it appears in the syntax tree
                if (info.Element.Name == "")
                {
                    return info.Parser;
                }
                else
                {
                    return Produce<SyntaxElement>(
                        info.Parser,
                        elem =>
                            new CustomNode(new[] { info.Element }, (SyntaxElement)elem[0]));
                }
            }
        }

        private static bool IsHex(string text) => text.All(c => TextFacts.IsHexDigit(c));

        private static IReadOnlyDictionary<string, ParserInfo> CreateRulesMap(QueryGrammar queryParser, Parser<LexicalToken, Command> command)
        {
            command = command ?? First(CommandGrammar.UnknownCommand, CommandGrammar.BadCommand);

            var StringName =
                Rule(Token(SyntaxKind.StringLiteralToken), token => (Name)new TokenName(token));

            var RawGuidLiteral =
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

            var GuidLiteral =
                  Rule(
                      Token(SyntaxKind.GuidLiteralToken),
                      (token) => (Expression)new LiteralExpression(SyntaxKind.GuidLiteralExpression, token));

            var AnyGuidLiteralOrString =
                First(GuidLiteral, queryParser.StringLiteral, RawGuidLiteral);

            var Name =
                First(
                    queryParser.IdentifierName,
                    queryParser.BracketedName,
                    queryParser.BracedName,
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

            var ExternalTableNameReference =
                Rule(Name, name => (Expression)new NameReference(name, SymbolMatch.ExternalTable))
                    .WithCompletionHint(Editor.CompletionHint.ExternalTable)
                    .WithTag("<externaltable>");

            var MaterializedViewNameReference =
                Rule(Name, name => (Expression)new NameReference(name, SymbolMatch.MaterializedView))
                    .WithCompletionHint(Editor.CompletionHint.MaterializedView)
                    .WithTag("<materializedview>");

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
                    queryParser.StringLiteral.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Literal),
                    () => (SyntaxElement)Q.MissingStringLiteral());

            var BracketedStringLiteral =
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
                        return (Expression)new LiteralExpression(SyntaxKind.StringLiteralExpression,
                            SyntaxToken.Literal(list[0].Trivia, text, SyntaxKind.StringLiteralToken));
                    }).WithTag("<bracketed-string>");

            var KustoBracketedStringLiteralInfo =
                new ParserInfo(
                    BracketedStringLiteral.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Literal),
                    () => (SyntaxElement)Q.MissingStringLiteral());

            var KustoGuidLiteralInfo =
                new ParserInfo(
                    AnyGuidLiteralOrString.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Literal),
                    () => (SyntaxElement)Q.MissingValue());

            var KustoValueInfo =
                new ParserInfo(
                    First(GuidLiteral.Cast<SyntaxElement>(), queryParser.Literal.Cast<SyntaxElement>()),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Literal),
                    () => (SyntaxElement)Q.MissingValue());

            var KustoTypeInfo =
                new ParserInfo(
                    queryParser.ParamTypeExtended.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.Syntax),
                    () => (SyntaxElement)Q.MissingType());

            var NameDeclarationOrStringLiteral =
                First(
                    queryParser.SimpleNameDeclarationExpression,
                    queryParser.StringLiteral);

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

            var KustoExternalTableNameInfo =
                new ParserInfo(
                    ExternalTableNameReference.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.ExternalTable),
                    () => (SyntaxElement)Q.MissingNameReference());

            var KustoMaterializedViewNameInfo =
                new ParserInfo(
                    MaterializedViewNameReference.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.MaterializedView),
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
                        queryParser.FunctionParameters,
                        queryParser.FunctionBody,
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

            var KustoFunctionBody =
                new ParserInfo(
                    parser: queryParser.FunctionBody.Cast<SyntaxElement>(),
                    element: new CustomElementDescriptor(hint: Editor.CompletionHint.Syntax),
                    missing: () => new FunctionBody(
                        CreateMissingToken(SyntaxKind.OpenBraceToken),
                        SyntaxList<SeparatedElement<Statement>>.Empty(),
                        null,
                        null,
                        CreateMissingToken(SyntaxKind.CloseBraceToken)));

            var CommandInput =
                First(
                    If(Token(SyntaxKind.DotToken),
                        command.Cast<SyntaxElement>()),
                    queryParser.StatementList.Cast<SyntaxElement>());

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

            return new Dictionary<string, ParserInfo>()
                {
                    { "value", KustoValueInfo },
                    { "timespan", KustoValueInfo },
                    { "datetime", KustoValueInfo },
                    { "string", KustoStringLiteralInfo },
                    { "bracketed_string", KustoBracketedStringLiteralInfo },
                    { "bool", KustoValueInfo },
                    { "long", KustoValueInfo },
                    { "int", KustoValueInfo },
                    { "decimal", KustoValueInfo },
                    { "real", KustoValueInfo },
                    { "type", KustoTypeInfo },
                    { "guid", KustoGuidLiteralInfo },
                    { "name", KustoNameDeclarationInfo },
                    { "column", KustoColumnNameInfo },
                    { "table_column", KustoTableColumnNameInfo },
                    { "database_table_column", KustoDatabaseTableColumnNameInfo },
                    { "table", KustoTableNameInfo },
                    { "externaltable", KustoExternalTableNameInfo },
                    { "materializedview", KustoMaterializedViewNameInfo },
                    { "database_table", KustoDatabaseTableNameInfo },
                    { "database", KustoDatabaseNameInfo },
                    { "cluster", KustoClusterNameInfo },
                    { "function", KustoFunctionNameInfo },
                    { "function_declaration", KustoFunctionDeclaration },
                    { "function_body", KustoFunctionBody },
                    { "input_query", KustoCommandInputInfo },
                    { "input_data", KustoInputText },
                    { "bracketed_input_data", KustoBracketedInputText }
                };
        }
    }
}