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

        private CommandGrammar(
            Parser<LexicalToken, CommandBlock> commandBlockParser)
        {
            this.CommandBlock = commandBlockParser;
        }

        /// <summary>
        /// Gets or creates the <see cref="CommandGrammar"/> corresponding to the <see cref="GlobalState"/>
        /// </summary>
        public static CommandGrammar From(GlobalState globals)
        {
            // if this is same set of commands as the default set, then just return the default grammar
            if (globals.Commands == GlobalState.Default.Commands)
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
                if (recent != null && recent.Commands == globals.Commands)
                {
                    grammar = recent.Grammar;
                }
                else
                {
                    grammar = Create(globals);

                    // remember this grammar for next time
                    var newRecent = new RecentGrammar(globals.Commands, grammar);
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
            public IReadOnlyList<CommandSymbol> Commands { get; }
            public CommandGrammar Grammar { get; }

            public RecentGrammar(IReadOnlyList<CommandSymbol> commands, CommandGrammar grammar)
            {
                this.Commands = commands;
                this.Grammar = grammar;
            }
        }

        private static CommandGrammar s_defaultCommandGrammar;

        private static CommandGrammar GetDefaultCommandGrammar()
        {
            if (s_defaultCommandGrammar == null)
            {
                var grammar = Create(GlobalState.Default);
                Interlocked.CompareExchange(ref s_defaultCommandGrammar, grammar, null);
            }

            return s_defaultCommandGrammar;
        }

        private static Parser<LexicalToken, SyntaxToken> UnknownCommandToken =
            If(Not(First(
                Token(SyntaxKind.BarToken),
                Token(SyntaxKind.LessThanBarToken),
                Token(SyntaxKind.EndOfTextToken))),
                AnyToken);

        public static Parser<LexicalToken, Command> UnknownCommand =
            Rule(
                Token(SyntaxKind.DotToken),
                OneOrMore(UnknownCommandToken,
                    (tokens) => new SyntaxList<SyntaxToken>(tokens)),
                (dot, parts) => (Command)new UnknownCommand(dot, parts))
                .WithTag("<unknown-command>");

        public static Parser<LexicalToken, Command> BadCommand =
            Rule(
                Token(SyntaxKind.DotToken),
                dot => (Command)new BadCommand(dot, new Diagnostic[] { DiagnosticFacts.GetMissingCommand() }))
                .WithTag("<bad-command>");


        /// <summary>
        /// Creates a new <see cref="CommandGrammar"/> given the <see cref="GlobalState"/>
        /// </summary>
        private static CommandGrammar Create(GlobalState globals)
        {
            var q = Q.From(globals);

            Parser<LexicalToken, Command> commandCore = null;

            var command = Forward(() => commandCore)
                .WithTag("<command>");

            // include parsers for all command symbols
            var queryParser = QueryGrammar.From(globals);
            var parserFactory = new CommandParserFactory(queryParser, command);
            var bestCommandParsers = parserFactory.CreateCommandParser(globals.Commands);

            commandCore =
                First(
                    bestCommandParsers, // pick whichever command will successfully consume most input
                    UnknownCommand, // fall back for commands that are not defined
                    BadCommand) // otherwise its just bad
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

            var skippedTokens =
                If(AnyTokenButEnd,
                    Rule(
                        List(AnyTokenButEnd), // consumes all remaining tokens
                        tokens => new SkippedTokens(tokens)));

            var commandBlock =
                Rule(
                    SeparatedList(
                        commandStatement, // first one is a command statement
                        SyntaxKind.SemicolonToken,
                        q.Statement,      // all others elements are query statements
                        MissingCommandStatementNode,
                        endOfList: EndOfText,
                        oneOrMore: true,
                        allowTrailingSeparator: true),
                    Optional(skippedTokens), // consumes all remaining tokens (no diagnostic)
                    Optional(Token(SyntaxKind.EndOfTextToken)),
                    (cmd, skipped, end) =>
                        new CommandBlock(cmd, skipped, end));

            return new CommandGrammar(commandBlock);
        }

        private static Statement MissingCommandStatementNode =
            new ExpressionStatement(new BadCommand(SyntaxToken.Missing(SyntaxKind.DotToken), new[] { DiagnosticFacts.GetMissingCommand() }));
    }
}