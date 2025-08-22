using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Parsing
{
    using Utils;
    using static Parsers<LexicalToken>;
    using static SyntaxParsers;
    using CompletionHint = Editor.CompletionHint;
    using CompletionKind = Editor.CompletionKind;
    using SP = SyntaxParsers;

    /// <summary>
    /// Parsers for the Kusto command grammar.
    /// </summary>
    public class CommandGrammar
    {
        internal GlobalState Globals { get; }

        /// <summary>
        /// The entry point grammar for commands
        /// </summary>
        public Parser<LexicalToken, CommandBlock> CommandBlock { get; }

        public PredefinedRuleParsers PredefinedRules { get; }

        public CommandGrammar(GlobalState globals)
        {
            Parser<LexicalToken, SyntaxElement> queryInputCore = null;
            Parser<LexicalToken, SyntaxElement> scriptInputCore = null;

            var queryInput = Forward(() => queryInputCore)
                .WithTag("<query-input>");

            var scriptInput = Forward(() => scriptInputCore)
                .WithTag("<script-input>");

            // include parsers for all command symbols
            var queryParser = QueryGrammar.From(globals);

            // all known commands
            var rules = this.PredefinedRules = new PredefinedRuleParsers(queryParser, queryInput, scriptInput);
            var commandParserInfos = this.CreateCommandParsers(rules).ToArray();
            var commandParsers = commandParserInfos.Select(cpi => cpi.Parser.WithTag(cpi.Kind)).ToArray();

            var bestCommand = 
                Best(commandParsers, (command1, command2) => IsBetterSyntax(command1, command2));

            // partial parsing of known commands
            var partialCommand =
                    Match(
                        (input, start) =>
                        {
                            var len = PartialParser.ScanPartialBest(commandParsers, input, start);
                            // fail if only dot (first token). It will become UnknownCommand
                            return (len < 2) ? -1 : len;
                        },
                        (input, start, length) =>
                        {
                            var output = new List<object>();
                            var bestParsers = new List<Parser<LexicalToken>>();
                            var _ = PartialParser.ParsePartialBest(commandParsers, input, start, output, 0, bestParsers, OnFailure);
                            if (output.Count == 1 && output[0] is Command successful)
                            {
                                return successful;
                            }
                            else
                            {
                                var elements = output.OfType<SyntaxElement>();
                                var dotToken = elements.FirstOrDefault() as SyntaxToken;
                                var parts = elements.Skip(1).ToReadOnly();
                                var kinds = bestParsers.Select(p => p.Tag).ToArray();
                                return (Command) new PartialCommand(kinds, dotToken, new SyntaxList<SyntaxElement>(parts));
                            }
                        });

            var bestCommandOrPartial =
                Best(
                    bestCommand,
                    partialCommand
                    );

            var commandSkippedTokens =
                If(UnknownCommandToken,
                    Rule(
                        List(UnknownCommandToken),
                        tokens => new SkippedTokens(tokens)));

            var commandAndSkippedTokens =
                ApplyOptional(
                    bestCommandOrPartial,
                    _left =>
                        Rule(_left, commandSkippedTokens,
                            (cmd, skippedTokens) =>
                                (Command)new CommandAndSkippedTokens(cmd, skippedTokens)
                            ));
            var command =
                First(
                    commandAndSkippedTokens, // pick whichever command will successfully consume most input
                    UnknownCommand, // fall back for commands that are not defined
                    BadCommand) // otherwise its just bad
                .WithTag("<command>");

            var commandPipeExpression =
                ApplyZeroOrMore(
                    command.Cast<Expression>(),
                    _left =>
                        Rule(
                            _left,
                            SP.Token(SyntaxKind.BarToken),
                            Required(queryParser.FollowingPipeElementExpression, QueryGrammar.CreateMissingQueryOperator),
                            (left, op, right) => (Expression)new PipeExpression(left, op, right))
                            .WithTag("<command-pipe-expression>"));

            queryInputCore =
                First(
                    If(SP.Token(SyntaxKind.DotToken),
                        commandPipeExpression.Cast<SyntaxElement>()),
                        queryParser.StatementList.Cast<SyntaxElement>());

            var commandStatement =
                Rule(
                    commandPipeExpression,
                    cmd => (Statement)new ExpressionStatement(cmd));

            var commandBlockSkippedTokens =
                If(AnyTokenButEnd,
                    Rule(
                        List(AnyTokenButEnd), // consumes all remaining tokens
                        tokens => new SkippedTokens(tokens)));

            this.CommandBlock =
                Rule(
                    List(queryParser.Directive), 
                    SeparatedList(
                        commandStatement, // first one is a command statement
                        SyntaxKind.SemicolonToken,
                        queryParser.Statement,      // all others elements are query statements
                        CreateMissingCommandStatement,
                        endOfList: EndOfText,
                        oneOrMore: true,
                        allowTrailingSeparator: true),
                    Optional(commandBlockSkippedTokens), // consumes all remaining tokens (no diagnostic)
                    Optional(SP.Token(SyntaxKind.EndOfTextToken)),
                    (directives, statements, skipped, end) =>
                        new CommandBlock(directives, statements, skipped, end));

            var scriptElement =
                Rule(
                    commandPipeExpression,
                    Optional(SP.Token(SyntaxKind.SemicolonToken)),
                    (cmd, semi) => (SyntaxElement)new SeparatedElement<Expression>(cmd, semi));

            scriptInputCore =
                OneOrMoreList(
                    Limit(CommandLimiter, scriptElement),  // limit parsing up until next command starts
                    fnMissingElement: CreateMissingCommandStatement);
        }

        /// <summary>
        /// Add diagnostics for missing syntax.
        /// </summary>
        private static void OnFailure(Parser<LexicalToken> parser, List<object> output)
        {
            var tags = new List<string>();
            parser.Accept(TagFinder.Instance, tags);

            if (tags.Count == 0)
                tags.Add("<token>");

            var distinctTags = tags.Distinct().OrderBy(x => x).ToArray();
            var missingToken = SyntaxToken.Missing("", SyntaxKind.IdentifierToken, new[] { DiagnosticFacts.GetTermsExpected(distinctTags) });
            output.Add(missingToken);
        }

        private class TagFinder : ParserVisitor<LexicalToken, List<string>, bool>
        {
            public static readonly TagFinder Instance = new TagFinder();

            /// <summary>
            /// Gets the non-empty tag or null.
            /// </summary>
            private static bool GetTag(Parser<LexicalToken> parser, List<string> tags)
            {
                if (parser.Annotations.Count > 0)
                {
                    var text = parser.Annotations.OfType<Editor.CompletionItem>().Select(ci => ci.DisplayText).FirstOrDefault();
                    if (text != null)
                    {
                        AddTag(text, tags);
                        return true;
                    }
                }

                if (!string.IsNullOrEmpty(parser.Tag))
                {
                    AddTag(parser.Tag, tags);
                    return true;
                }

                return false;
            }

            private static void AddTag(string tag, List<string> tags)
            {
                if (tag.Contains("|"))
                {
                    // split the tag by | and add each part separately
                    var parts = tag.Split('|');
                    foreach (var part in parts)
                    {
                        if (!string.IsNullOrWhiteSpace(part))
                        {
                            tags.Add(part.Trim());
                        }
                    }
                }
                else if (!string.IsNullOrWhiteSpace(tag))
                {
                    tags.Add(tag.Trim());
                }
            }

            private bool All(IReadOnlyList<Parser<LexicalToken>> parsers, List<string> tags)
            {
                var any = false;

                foreach (var parser in parsers)
                {
                    any |= parser.Accept(this, tags);
                }

                return any;
            }

            public override bool VisitApply<TLeft, TOutput>(ApplyParser<LexicalToken, TLeft, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.LeftParser.Accept(this, tags);
            }

            public override bool VisitBest<TOutput>(BestParser<LexicalToken, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || All(parser.Parsers, tags);
            }

            public override bool VisitBest(BestParser<LexicalToken> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || All(parser.Parsers, tags);
            }

            public override bool VisitConvert<TOutput>(ConvertParser<LexicalToken, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags);
            }

            public override bool VisitFails(FailsParser<LexicalToken> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.Pattern.Accept(this, tags);
            }

            public override bool VisitFirst<TOutput>(FirstParser<LexicalToken, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || All(parser.Parsers, tags);
            }

            public override bool VisitFirst(FirstParser<LexicalToken> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || All(parser.Parsers, tags);
            }

            public override bool VisitForward<TOutput>(ForwardParser<LexicalToken, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.DeferredParser().Accept(this, tags);
            }

            public override bool VisitIf<TOutput>(IfParser<LexicalToken, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.Parser.Accept(this, tags);
            }

            public override bool VisitIf(IfParser<LexicalToken> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.Test.Accept(this, tags);
            }

            public override bool VisitLimit<TOutput>(LimitParser<LexicalToken, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.Limited.Accept(this, tags);
            }

            public override bool VisitMap<TOutput>(MapParser<LexicalToken, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags);
            }

            public override bool VisitMatch(MatchParser<LexicalToken> parser, List<string> tags)
            {
                return GetTag(parser, tags);
            }

            public override bool VisitMatch<TOutput>(MatchParser<LexicalToken, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags);
            }

            public override bool VisitNot(NotParser<LexicalToken> parser, List<string> tags)
            {
                return GetTag(parser, tags);
            }

            public override bool VisitOneOrMore(OneOrMoreParser<LexicalToken> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.Parser.Accept(this, tags);
            }

            public override bool VisitOptional<TOutput>(OptionalParser<LexicalToken, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.Parser.Accept(this, tags);
            }

            public override bool VisitProduce<TOutput>(ProduceParser<LexicalToken, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.Parser.Accept(this, tags);
            }

            public override bool VisitRequired<TOutput>(RequiredParser<LexicalToken, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.Parser.Accept(this, tags);
            }

            public override bool VisitRule<TOutput>(RuleParser<LexicalToken, TOutput> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.Parsers[0].Accept(this, tags);
            }

            public override bool VisitSequence(SequenceParser<LexicalToken> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.Parsers[0].Accept(this, tags);
            }

            public override bool VisitZeroOrMore(ZeroOrMoreParser<LexicalToken> parser, List<string> tags)
            {
                return GetTag(parser, tags)
                    || parser.Parser.Accept(this, tags);
            }
        }


        /// <summary>
        /// Gets or creates the <see cref="CommandGrammar"/> corresponding to the <see cref="GlobalState"/>
        /// </summary>
        public static CommandGrammar From(GlobalState globals)
        {
            // if this is same set of commands as the default set, then just return the default grammar
            if (globals.ServerKind == GlobalState.Default.ServerKind)
            {
                return GetDefaultCommandGrammar();
            }

            CommandGrammar grammar = null;
            bool foundInCache = false;

            // if the globals has a cache, look in the cache
            if (globals.Cache != null)
            {
                foundInCache = globals.Cache.TryGetValue<CommandGrammar>(out grammar);
            }

            // if we still don't have a grammar check the recently created grammar
            // or create a new one
            if (grammar == null)
            {
                var recent = s_recentGrammar;

                if (recent != null && recent.ServerKind == globals.ServerKind)
                {
                    grammar = recent.Grammar;
                }
                else
                {
                    grammar = CreateCommandGrammar(globals);

                    // remember this grammar for next time
                    var newRecent = new RecentGrammar(globals.ServerKind, grammar);
                    Interlocked.CompareExchange(ref s_recentGrammar, newRecent, recent);
                }
            }

            // if the grammar did not come from the cache, put it into the cache if there is one.
            if (globals.Cache != null && !foundInCache)
            {
                grammar = globals.Cache.GetOrCreate<CommandGrammar>(() => grammar);
            }

            return grammar;
        }

        private static RecentGrammar s_recentGrammar;

        private class RecentGrammar
        {
            public string ServerKind { get; }
            public CommandGrammar Grammar { get; }

            public RecentGrammar(string serverKind, CommandGrammar grammar)
            {
                this.ServerKind = serverKind;
                this.Grammar = grammar;
            }
        }

        private static CommandGrammar s_defaultCommandGrammar;

        private static CommandGrammar GetDefaultCommandGrammar()
        {
            if (s_defaultCommandGrammar == null)
            {
                var grammar = CreateCommandGrammar(GlobalState.Default);
                Interlocked.CompareExchange(ref s_defaultCommandGrammar, grammar, null);
            }

            return s_defaultCommandGrammar;
        }

        /// <summary>
        /// Creates the correct kind of command grammar given the global state.
        /// </summary>
        public static CommandGrammar CreateCommandGrammar(GlobalState globals)
        {
            switch (globals.ServerKind)
            {
                case ServerKinds.Engine:
                    return new EngineCommandGrammar(globals);
                case ServerKinds.DataManager:
                    return new DataManagerCommandGrammar(globals);
                case ServerKinds.ClusterManager:
                    return new ClusterManagerCommandGrammar(globals);
                case ServerKinds.AriaBridge:
                    return new AriaBridgeCommandGrammar(globals);
                default:
                    // no defined commands
                    return new CommandGrammar(globals);
            }
        }

        private static bool IsBetterSyntax(SyntaxElement command1, SyntaxElement command2)
        {
            // neither command has diagnostics, neither is better
            if (!command1.ContainsSyntaxDiagnostics && !command2.ContainsSyntaxDiagnostics)
                return false;

            // command1 has diagnostics, command1 is not better than command2
            if (command1.ContainsSyntaxDiagnostics && !command2.ContainsSyntaxDiagnostics)
                return false;

            // command2 has diagnostics, command1 is better
            if (!command1.ContainsSyntaxDiagnostics && command2.ContainsSyntaxDiagnostics)
                return true;

            var dx1 = command1.GetContainedSyntaxDiagnostics();
            var dx2 = command2.GetContainedSyntaxDiagnostics();

            // command1 first diagnostic occurs lexically after command2 first diagnostics, command1 is better
            if (dx1[0].Start > dx2[0].Start)
                return true;

            // command1 first diagnostic occurs lexically before command2 first diagnostic, command1 is not better
            if (dx1[0].Start < dx2[0].Start)
                return false;

            // don't compare number of diagnostics, since we want to favor what happens early rather than later

            // otherwise neither is better
            return false;
        }

        /// <summary>
        /// Derived command parser's override this method to create the set of individual command parsers.
        /// </summary>
        internal virtual CommandParserInfo[] CreateCommandParsers(PredefinedRuleParsers rules)
        {
            // no commands
            return new CommandParserInfo[0];
        }

        internal class CommandParserInfo
        {
            public readonly string Kind;
            public readonly Parser<LexicalToken, Command> Parser;

            public CommandParserInfo(string kind, Parser<LexicalToken, Command> parser)
            {
                this.Kind = kind;
                this.Parser = parser;
            }
        }

        internal static readonly Parser<LexicalToken, SyntaxToken> UnknownCommandToken =
            If(Not(First(
                SP.Token(SyntaxKind.BarToken),
                SP.Token(SyntaxKind.LessThanBarToken),
                SP.Token(SyntaxKind.EndOfTextToken),
                SP.Token(SyntaxKind.SemicolonToken)
                )),
                AnyToken);

        internal static readonly Parser<LexicalToken, Command> UnknownCommand =
            Rule(
                SP.Token(SyntaxKind.DotToken),
                OneOrMore(UnknownCommandToken, tokens => new SyntaxList<SyntaxElement>(tokens)),
                (dot, parts) => (Command)new UnknownCommand(dot, parts))
                .WithTag("<unknown-command>");

        internal static bool IsPossibleStartOfCommand(LexicalToken token, bool isFirstToken = false)
        {
            if (token.Kind == SyntaxKind.DotToken)
            {
                if (TextFacts.HasLineBreaks(token.Trivia))
                {
                    var lastLineStart = TextFacts.GetLastLineBreakEnd(token.Trivia);
                    if (lastLineStart >= 0 && TextFacts.IsWhitespaceOnly(token.Trivia, lastLineStart, token.Trivia.Length - lastLineStart))
                    {
                        // first non-whitespace on line is a dot token
                        return true;
                    }
                }
                else if (isFirstToken && (token.Trivia.Length == 0 || TextFacts.IsWhitespaceOnly(token.Trivia)))
                {
                    return true;
                }
            }

            return false;
        }

        internal static Parser<LexicalToken> CommandLimiter =
            Match((source, start) =>
            {
                // skip first token since it is assumed to be the start of the current command
                var pos = start + 1; 

                // allow any token up until end of input or when the next command starts
                while (!source.IsEnd(pos) && !IsPossibleStartOfCommand(source.Peek(pos)))
                {
                    pos++;
                }

                // return number of tokens allowed
                return pos - start;
            });

        internal static readonly Parser<LexicalToken, Command> BadCommand =
            Rule(
                SP.Token(SyntaxKind.DotToken),
                dot => (Command)new BadCommand(dot, new Diagnostic[] { DiagnosticFacts.GetMissingCommand() }))
                .WithTag("<bad-command>");

        internal static Statement CreateMissingCommandStatement(Source<LexicalToken> source, int start) =>
            new ExpressionStatement(new BadCommand(SyntaxToken.Missing(SyntaxKind.DotToken), new[] { DiagnosticFacts.GetMissingCommand() }));

        /// <summary>
        /// Constructs a <see cref="CustomNode"/> parser.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Custom(
            IReadOnlyList<Parser<LexicalToken>> parsers,
            IReadOnlyList<CustomElementDescriptor> shape = null)
        {
            if (shape == null)
            {
                shape = CustomNode.GetDefaultShape(parsers.Count);
            }

            return Produce(
                Sequence(parsers),
                (IReadOnlyList<object> items) =>
                    (SyntaxElement)new CustomNode(shape, items.Cast<SyntaxElement>().ToArray()));
        }

        /// <summary>
        /// Constructs a <see cref="CustomNode"/> parser.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Custom(
            Parser<LexicalToken> parser,
            CustomElementDescriptor shape = null) =>
            Custom(new[] { parser }, shape != null ? new[] { shape } : null);

        /// <summary>
        /// Constructs a <see cref="CustomNode"/> parser.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Custom(
            Parser<LexicalToken> parser1,
            Parser<LexicalToken> parser2,
            IReadOnlyList<CustomElementDescriptor> shape = null) =>
            Custom(new[] { parser1, parser2 }, shape);

        /// <summary>
        /// Constructs a <see cref="CustomNode"/> parser.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Custom(
            Parser<LexicalToken> parser1,
            Parser<LexicalToken> parser2,
            Parser<LexicalToken> parser3,
            IReadOnlyList<CustomElementDescriptor> shape = null) =>
            Custom(new[] { parser1, parser2, parser3 }, shape);

        /// <summary>
        /// Constructs a <see cref="CustomNode"/> parser.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Custom(
            Parser<LexicalToken> parser1,
            Parser<LexicalToken> parser2,
            Parser<LexicalToken> parser3,
            Parser<LexicalToken> parser4,
            IReadOnlyList<CustomElementDescriptor> shape = null) =>
            Custom(new[] { parser1, parser2, parser3, parser4 }, shape);

        /// <summary>
        /// Constructs a <see cref="CustomNode"/> parser.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Custom(
            Parser<LexicalToken> parser1,
            Parser<LexicalToken> parser2,
            Parser<LexicalToken> parser3,
            Parser<LexicalToken> parser4,
            Parser<LexicalToken> parser5,
            IReadOnlyList<CustomElementDescriptor> shape = null) =>
            Custom(new[] { parser1, parser2, parser3, parser4, parser5 }, shape);


        /// <summary>
        /// Constructs a <see cref="CustomNode"/> parser.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Custom(
            Parser<LexicalToken> parser1,
            Parser<LexicalToken> parser2,
            Parser<LexicalToken> parser3,
            Parser<LexicalToken> parser4,
            Parser<LexicalToken> parser5,
            Parser<LexicalToken> parser6,
            IReadOnlyList<CustomElementDescriptor> shape = null) =>
            Custom(new[] { parser1, parser2, parser3, parser4, parser5, parser6 }, shape);

        /// <summary>
        /// Constructs a <see cref="CustomNode"/> parser.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Custom(
            Parser<LexicalToken> parser1,
            Parser<LexicalToken> parser2,
            Parser<LexicalToken> parser3,
            Parser<LexicalToken> parser4,
            Parser<LexicalToken> parser5,
            Parser<LexicalToken> parser6,
            Parser<LexicalToken> parser7,
            IReadOnlyList<CustomElementDescriptor> shape = null) =>
            Custom(new[] { parser1, parser2, parser3, parser4, parser5, parser6, parser7 }, shape);

        /// <summary>
        /// Constructs a <see cref="CustomNode"/> parser.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Custom(
            Parser<LexicalToken> parser1,
            Parser<LexicalToken> parser2,
            Parser<LexicalToken> parser3,
            Parser<LexicalToken> parser4,
            Parser<LexicalToken> parser5,
            Parser<LexicalToken> parser6,
            Parser<LexicalToken> parser7,
            Parser<LexicalToken> parser8,
            IReadOnlyList<CustomElementDescriptor> shape = null) =>
            Custom(new[] { parser1, parser2, parser3, parser4, parser5, parser6, parser7, parser8 }, shape);

        /// <summary>
        /// Constructs a <see cref="CustomNode"/> parser.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Custom(
            Parser<LexicalToken> parser1,
            Parser<LexicalToken> parser2,
            Parser<LexicalToken> parser3,
            Parser<LexicalToken> parser4,
            Parser<LexicalToken> parser5,
            Parser<LexicalToken> parser6,
            Parser<LexicalToken> parser7,
            Parser<LexicalToken> parser8,
            Parser<LexicalToken> parser9,
            IReadOnlyList<CustomElementDescriptor> shape = null) =>
            Custom(new[] { parser1, parser2, parser3, parser4, parser5, parser6, parser7, parser8, parser9 }, shape);

        /// <summary>
        /// Constructs a <see cref="CustomElementDescriptor"/>
        /// </summary>
        public static CustomElementDescriptor CD(string name, CompletionHint hint = CompletionHint.Syntax, bool isOptional = false) =>
            CustomElementDescriptor.From(name, hint, isOptional);

        /// <summary>
        /// Constructs a <see cref="CustomElementDescriptor"/>
        /// </summary>
        public static CustomElementDescriptor CD(CompletionHint hint = CompletionHint.Syntax, bool isOptional = false) =>
            CustomElementDescriptor.From(hint, isOptional);

        /// <summary>
        /// Constructs a custom command parser.
        /// </summary>
        public static Parser<LexicalToken, Command> Command(string commandName, Parser<LexicalToken, SyntaxElement> contentParser)
        {
            return Rule(
                SP.Token(SyntaxKind.DotToken),
                contentParser,
                (dot, custom) => (Command)new CustomCommand(commandName, dot, custom))
                .WithTag($"<{commandName}>");
        }

        public static SyntaxElement CreateMissingToken(string text)
        {
            return SyntaxParsers.CreateMissingToken(text);
        }

        public static SyntaxElement CreateMissingToken(IReadOnlyList<string> texts)
        {
            return SyntaxParsers.CreateMissingToken(texts);
        }

        public static SyntaxElement CreateMissingToken(params string[] texts)
        {
            return SyntaxParsers.CreateMissingToken(texts);
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has the specified text, producing a single <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Token(string text, SyntaxKind? asKind = null, CompletionKind? ckind = null)
        {
            // the default completion kind won't be known for most command keywords since they are not encoded in the SyntaxFacts table,
            // so change the default to Keyword to handle this common case.
            return SyntaxParsers.Token(text, asKind, ckind ?? SP.GetCompletionKind(text, CompletionKind.Keyword)).Cast<SyntaxElement>();
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has the specified text, producing a single <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Token(string text, CompletionKind ckind)
        {
            // the default completion kind won't be known for most command keywords since they are not encoded in the SyntaxFacts table,
            // so change the default to Keyword to handle this common case.
            return SyntaxParsers.Token(text, null, ckind).Cast<SyntaxElement>();
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has one of the specified texts, producing a single <see cref="SyntaxToken"/>.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> Token(params string[] texts)
        {
            // the default completion kind won't be known for most command keywords since they are not encoded in the SyntaxFacts table,
            // so change the default to Keyword to handle this common case.
            return SyntaxParsers.Token(texts, defaultKind: CompletionKind.Keyword).Cast<SyntaxElement>();
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has the specified text, producing a corresponding <see cref="SyntaxToken"/> or an equivalent missing token otherwise.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> RequiredToken(string text, SyntaxKind? asKind = null, CompletionKind? ckind = null)
        {
            return Required(Token(text, asKind, ckind), () => (SyntaxElement)CreateMissingToken(text));
        }

        /// <summary>
        /// A parser that consumes the next <see cref="LexicalToken"/> (or series of adjacent tokens) if it has one of the specified texts, producing a corresponding <see cref="SyntaxToken"/> or an equivalent missing token otherwise.
        /// </summary>
        public static Parser<LexicalToken, SyntaxElement> RequiredToken(params string[] texts)
        {
            return Required(Token(texts), () => (SyntaxElement)CreateMissingToken(texts));
        }

        public static Parser<LexicalToken, SyntaxElement> ZeroOrMoreList(
            Parser<LexicalToken, SyntaxElement> elementParser,
            Parser<LexicalToken, SyntaxElement> separatorParser = null,
            Func<Source<LexicalToken>, int, SyntaxElement> fnMissingElement = null,
            bool allowTrailingSeparator = false)
        {
            if (separatorParser != null)
            {
                return OList(
                    primaryElementParser: elementParser,
                    secondaryElementParser: null,
                    separatorParser: separatorParser,
                    fnMissingPrimaryElement: null,
                    fnMissingSecondaryElement: fnMissingElement,
                    fnMissingSeparator: null,
                    endOfList: null,
                    oneOrMore: false,
                    allowTrailingSeparator: allowTrailingSeparator,
                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)
                    );
            }
            else
            {
                return List(
                    elementParser: elementParser,
                    fnMissingElement: null,
                    oneOrMore: false,
                    producer: elements => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.OfType<SyntaxElement>().ToArray())
                    );
            }
        }

        public static Parser<LexicalToken, SyntaxElement> ZeroOrMoreCommaList(
            Parser<LexicalToken, SyntaxElement> elementParser,
            Func<Source<LexicalToken>, int, SyntaxElement> fnMissingElement = null,
            bool allowTrailingSeparator = false)
        {
            return ZeroOrMoreList(elementParser, Token(","), fnMissingElement, allowTrailingSeparator);
        }

        public static Parser<LexicalToken, SyntaxElement> OneOrMoreList(
            Parser<LexicalToken, SyntaxElement> elementParser,
            Parser<LexicalToken, SyntaxElement> separatorParser = null,
            Func<Source<LexicalToken>, int, SyntaxElement> fnMissingElement = null,
            bool allowTrailingSeparator = false)
        {
            if (separatorParser != null)
            {
                return OList(
                    primaryElementParser: elementParser,
                    secondaryElementParser: null,
                    separatorParser: separatorParser,
                    fnMissingPrimaryElement: null,
                    fnMissingSecondaryElement: fnMissingElement,
                    fnMissingSeparator: null,
                    endOfList: null,
                    oneOrMore: true,
                    allowTrailingSeparator: allowTrailingSeparator,
                    producer: list => (SyntaxElement)MakeSeparatedList<SyntaxElement>(list)
                    );
            }
            else
            {
                return List(
                    elementParser: elementParser,
                    fnMissingElement: null,
                    oneOrMore: true,
                    producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.OfType<SyntaxElement>().ToArray())
                    );
            }
        }

        public static Parser<LexicalToken, SyntaxElement> OneOrMoreCommaList(
            Parser<LexicalToken, SyntaxElement> elementParser,
            Func<Source<LexicalToken>, int, SyntaxElement> fnMissingElement = null,
            bool allowTrailingSeparator = false)
        {
            return OneOrMoreList(elementParser, Token(","), fnMissingElement, allowTrailingSeparator);
        }
    }
}