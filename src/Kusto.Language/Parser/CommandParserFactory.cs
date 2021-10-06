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
        private readonly IReadOnlyDictionary<string, ParserInfo> _rulesMap;

        /// <summary>
        /// Constructs a new <see cref="CommandParserFactory"/>
        /// </summary>
        /// <param name="queryParser">A query parser that can parse the query parts of a command.</param>
        /// <param name="commandParser">An command parser used that represents the set of all parsable commands.</param>
        internal CommandParserFactory(QueryGrammar queryParser, Parser<LexicalToken, Command> commandParser)
        {
            _rulesMap = CreateRulesMap(queryParser, commandParser);
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

        public Parser<LexicalToken, Command> CreateCommandParser(string commandName, Grammar contentGrammar, GrammarAnalysis analysis)
        {
            var contentParser = CreateContentParser(commandName, contentGrammar, analysis);
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

        [System.Diagnostics.DebuggerDisplay("{DebugText}")]
        private struct CommandAndGrammar
        {
            public readonly CommandSymbol Symbol;
            public readonly Grammar Grammar;

            public CommandAndGrammar(CommandSymbol symbol, Grammar grammar)
            {
                this.Symbol = symbol;
                this.Grammar = grammar;
            }

            private string DebugText =>
                $"{Symbol.Name}: {Grammar.ToString()}";
        }

        /// <summary>
        /// Creates a unified command parser for all command symbols
        /// </summary>
        public Parser<LexicalToken, Command> CreateCommandParser(IReadOnlyList<CommandSymbol> commandSymbols)
        {
            var commandAndGrammars = commandSymbols.Select(s => new CommandAndGrammar(s, ParseCommandGrammar(s.Name, s.Grammar)))
                .Where(x => x.Grammar != null)
                .ToList();

            var adjustedResult = Adjust(commandAndGrammars, cag => cag.Grammar, (cag, g) => new CommandAndGrammar(cag.Symbol, g));

            var commandParsers = adjustedResult.items.Select(cag => CreateCommandParser(cag.Symbol.Name, cag.Grammar, adjustedResult.analysis))
                .ToArray();

            // use Best combinator with function to pick which output is better when there are ambiguities
            var bestCommandParsers = Best(commandParsers, (command1, command2) => IsBetterSyntax(command1, command2));
            return bestCommandParsers;
            //return First(commandParsers);
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
        /// Creates the parser for a command's content (everything after the first dot)
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="commandGrammar">The grammar of the command (does not include the initial dot).</param>
        public Parser<LexicalToken, SyntaxElement> CreateContentParser(string commandName, string commandGrammar)
        {
            var contentGrammar = ParseCommandGrammar(commandName, commandGrammar);
            
            if (contentGrammar != null)
            {
                var adjustedGrammar = Adjust(contentGrammar);
                return CreateContentParser(commandName, adjustedGrammar, GrammarAnalysis.Empty);
            }

            return null;
        }

        /// <summary>
        /// Transform all the item grammars to work best when put together as a single parser
        /// </summary>
        private static (IReadOnlyList<T> items, GrammarAnalysis analysis) Adjust<T>(IReadOnlyList<T> items, Func<T, Grammar> grammarSelector, Func<T, Grammar, T> grammarUpdater)
        {
            var unrolled = items.Select(i => grammarUpdater(i, GrammarUnroller.Unroll(grammarSelector(i)))).ToList();

            var simplified = unrolled.Select(i => grammarUpdater(i, GrammarSimplifier.Simplify(grammarSelector(i)))).ToList();

            // reorder nested alternations so keywords don't get swallowed by name/identifier rules.
            var reordered = simplified.Select(i => grammarUpdater(i, GrammarReorderer.Reorder(grammarSelector(i)))).ToList();

            var requireResult = GrammarRequirer.Require(reordered, grammarSelector, grammarUpdater);

            return requireResult;
        }

        public static IReadOnlyList<CommandSymbol> GetAdjustedCommands(IReadOnlyList<CommandSymbol> commandSymbols)
        {
            var commandAndGrammars = commandSymbols.Select(s => new CommandAndGrammar(s, ParseCommandGrammar(s.Name, s.Grammar)))
                .Where(x => x.Grammar != null)
                .ToList();

            var adjustedResult = Adjust(commandAndGrammars, cag => cag.Grammar, (cag, g) => new CommandAndGrammar(cag.Symbol, g));

            return adjustedResult.items.Select(cag => new CommandSymbol(cag.Symbol.Name, cag.Grammar.ToString(), cag.Symbol.ResultSchema)).ToList();
        }

        /// <summary>
        /// includes all the grammar transformations used by command parser factory
        /// </summary>
        public static Grammar Adjust(Grammar grammar)
        {
            grammar = GrammarUnroller.Unroll(grammar);

            grammar = GrammarSimplifier.Simplify(grammar);

            // reorder alternations so keywords don't get swallowed by name/identifier rules.
            grammar = GrammarReorderer.Reorder(grammar);

            // add require rules to make grammar partially parseable
            grammar = GrammarRequirer.Require(grammar).grammar;

            return grammar;
        }

        private static Grammar ParseCommandGrammar(string commandName, string commandGrammar)
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

        private Parser<LexicalToken, SyntaxElement> CreateContentParser(string commandName, Grammar contentGrammar, GrammarAnalysis analysis)
        {
            var translator = new GrammarTranslator(_rulesMap, analysis);
            var info = translator.Translate(contentGrammar);
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

            public ParserInfo WithParser(Parser<LexicalToken, SyntaxElement> parser)
            {
                if (this.Parser == parser)
                    return this;
                return new ParserInfo(parser, this.Element, this.Missing, this.IsTerm);
            }

            public ParserInfo WithElement(CustomElementDescriptor element)
            {
                if (this.Element == element)
                    return this;
                return new ParserInfo(this.Parser, element, this.Missing, this.IsTerm);
            }

            public ParserInfo WithMissing(Func<SyntaxElement> missing)
            {
                if (this.Missing == missing)
                    return this;
                return new ParserInfo(this.Parser, this.Element, missing, this.IsTerm);
            }
        }

        private class GrammarTranslator : GrammarVisitor<ParserInfo>
        {
            private readonly IReadOnlyDictionary<string, ParserInfo> _rulesMap;
            private readonly GrammarAnalysis _analysis;

            public GrammarTranslator(
                IReadOnlyDictionary<string, ParserInfo> rulesMap,
                GrammarAnalysis analysis)
            {
                _rulesMap = rulesMap;
                _analysis = analysis;
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

                var elementParser = GetElementParser(elem);

                if (sep == null)
                {
                    return new ParserInfo(
                        List(
                            elementParser,
                            missingElement: null,
                            oneOrMore: true,
                            producer: (elements) => (SyntaxElement)new SyntaxList<SyntaxElement>(elements.ToArray())),
                        new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: false),
                        () => new SyntaxList<SyntaxElement>(new SyntaxElement[] { elem.Missing() }));
                }
                else
                {

                    return new ParserInfo(
                        OList(
                            elementParser,
                            sep.Parser,
                            elementParser,
                            missingPrimaryElement: null,
                            missingSeparator: null,
                            missingSecondaryElement: elem.Missing,
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

                // because required grammar rules can be used as catch-alls
                // if there are alternate terms, replace diagnostic for missing case
                // with one that includes all alternates.
                var alts = _analysis.GetAlternativeTerms(grammar.Required);
                if (alts.Count > 1
                    && alts.Any(a => !a.IsEquivalentTo(grammar.Required)))
                {
                    if (grammar.Required is TokenGrammar tg
                        || grammar.Required is RuleGrammar rg)
                    {
                        var minAlts = alts
                            .Where(g => g is TokenGrammar || g is RuleGrammar)
                            .Distinct(GrammarComparer.Instance)
                            .OrderBy(g => g, GrammarComparer.Instance)
                            .ToArray();

                        // put elem into separate captured local
                        var oldElem = elem;
                        elem = elem.WithMissing(() =>
                        {
                            var syntax = oldElem.Missing().Clone(includeDiagnostics: false);
                            return syntax.WithAdditionalDiagnostics(GetTermsExpected(minAlts));
                        });
                    }
                }

                return new ParserInfo(
                    Required(GetElementParser(elem), elem.Missing),
                    new CustomElementDescriptor(elem.Element.CompletionHint, isOptional: false),
                    elem.Missing);
            }

            private static Diagnostic GetTermsExpected(Grammar[] grammars)
            {
                var terms = grammars.Select(g => GetDiagnosticTerm(g)).Distinct().ToArray();
                return DiagnosticFacts.GetTermsExpected(terms);
            }

            private static string GetDiagnosticTerm(Grammar grammar)
            {
                switch (grammar)
                {
                    case TokenGrammar tg:
                        return $"'{tg.TokenText}'";
                    case RuleGrammar rg:
                        switch (rg.RuleName)
                        {
                            case "name":
                                return "a name";
                            case "wildcarded_name":
                            case "qualified_wildcarded_name":
                                return "a wildcarded name";
                            case "column":
                            case "table_column":
                            case "database_table_column":
                                return "a column name";
                            case "table":
                            case "database_table":
                                return "a table name";
                            case "externaltable":
                                return "an external table name";
                            case "materializedview":
                                return "a materialized view name";
                            case "database":
                                return "a database name";
                            case "cluster":
                                return "a cluster name";
                            case "function":
                                return "a database function name";
                            default:
                                return rg.RuleName;
                        }
                    default:
                        throw new InvalidOperationException($"Unhandled grammar type: '{grammar.GetType().Name}'");
                }
            }

            public override ParserInfo VisitRule(RuleGrammar grammar)
            {
                if (_rulesMap.TryGetValue(grammar.RuleName, out var info))
                {
                    // if this rule is a name-like rule, then return a parser that 
                    // ignores all alterative tokens that can appear at the same lexical position.
                    if (IsNameLike(grammar.RuleName))
                    {
                        var altTerms = _analysis.GetAlternativeTerms(grammar);
                        if (altTerms.Count > 1) // do not count this grammar
                        {
                            var altTokens = altTerms
                                .OfType<TokenGrammar>()
                                .Select(g => g.TokenText)
                                .Distinct()
                                .ToList();

                            if (altTokens.Count > 0)
                            {
                                var altParsers = altTokens.Select(tk => MatchText(tk)).ToArray();
                                var newParser = altParsers.Length == 1
                                    ? If(Not(altParsers[0]), info.Parser)
                                    : If(Not(First(altParsers)), info.Parser);
                                return new ParserInfo(newParser, info.Element, info.Missing, info.IsTerm);
                            }
                        }
                    }
                }

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

            public override ParserInfo VisitHidden(HiddenGrammar grammar)
            {
                var elem = grammar.Hidden.Accept(this);
                return elem.WithParser(elem.Parser.Hide());
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

            var WildcardedNameDeclaration =
                Rule(queryParser.WildcardedIdentifier,
                    id => new NameDeclaration(
                        new WildcardedName(id)));

            var KustoWildcardedNameDeclarationInfo =
                new ParserInfo(
                    WildcardedNameDeclaration.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.None),
                    () => (SyntaxElement)Q.MissingNameReference());;

            // either name.wildname or wildname
            var QualifiedWildcardedNameDeclaration =
                First(
                    If(And(queryParser.SimpleNameDeclaration, Token("."), queryParser.WildcardedIdentifier),
                        Rule(queryParser.SimpleNameDeclaration, Token("."), Required(WildcardedNameDeclaration.Cast<Expression>(), Q.MissingNameReference),
                            (qual, dot, name) => (Expression)new PathExpression(qual, dot, name))),
                    WildcardedNameDeclaration.Cast<Expression>());

            var KustoQualifiedWildcardedNameDeclarationInfo =
                new ParserInfo(
                    QualifiedWildcardedNameDeclaration.Cast<SyntaxElement>(),
                    new CustomElementDescriptor(hint: Editor.CompletionHint.None),
                    () => (SyntaxElement)Q.MissingNameReference()); ;

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
                    { "wildcarded_name", KustoWildcardedNameDeclarationInfo },
                    { "qualified_wildcarded_name", KustoQualifiedWildcardedNameDeclarationInfo },
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

        private static bool IsNameLike(string ruleName)
        {
            switch (ruleName)
            {
                case "name":
                case "wildcarded_name":
                case "qualified_wildcarded_name":
                case "column":
                case "table_column":
                case "database_table_column":
                case "table":
                case "externaltable":
                case "materializedview":
                case "database_table":
                case "database":
                case "cluster":
                case "function":
                    return true;
                default:
                    return false;
            }
        }
    }
}