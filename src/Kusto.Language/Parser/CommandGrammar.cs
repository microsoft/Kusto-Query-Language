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
    using Utils;
    using System.Text;

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

        public CommandGrammar(GlobalState globals)
        {
            this.CommandBlock = CreateCommandBlockParser(globals);
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

        /// <summary>
        /// Creates a new <see cref="CommandGrammar"/> given the <see cref="GlobalState"/>
        /// </summary>
        private Parser<LexicalToken, CommandBlock> CreateCommandBlockParser(GlobalState globals)
        {
            Parser<LexicalToken, Command> commandCore = null;

            var command = Forward(() => commandCore)
                .WithTag("<command>");

            // include parsers for all command symbols
            var queryParser = QueryGrammar.From(globals);
            var rules = new PredefinedRuleParsers(queryParser, command);
            var commandParsers = this.CreateCommandParsers(rules).ToArray();
            var bestCommand = Best(commandParsers, (command1, command2) => IsBetterSyntax(command1, command2));

            commandCore =
                First(
                    bestCommand, // pick whichever command will successfully consume most input
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
                            Required(queryParser.FollowingPipeElementExpression, Q.MissingQueryOperator),
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
                        queryParser.Statement,      // all others elements are query statements
                        MissingCommandStatementNode,
                        endOfList: EndOfText,
                        oneOrMore: true,
                        allowTrailingSeparator: true),
                    Optional(skippedTokens), // consumes all remaining tokens (no diagnostic)
                    Optional(Token(SyntaxKind.EndOfTextToken)),
                    (cmd, skipped, end) =>
                        new CommandBlock(cmd, skipped, end));

            return commandBlock;
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
        internal virtual Parser<LexicalToken, Command>[] CreateCommandParsers(PredefinedRuleParsers rules)
        {
            // no commands
            return new Parser<LexicalToken, Command>[0];
        }

        internal static readonly Parser<LexicalToken, SyntaxToken> UnknownCommandToken =
            If(Not(First(
                Token(SyntaxKind.BarToken),
                Token(SyntaxKind.LessThanBarToken),
                Token(SyntaxKind.EndOfTextToken))),
                AnyToken);

        internal static readonly Parser<LexicalToken, Command> UnknownCommand =
            Rule(
                Token(SyntaxKind.DotToken),
                OneOrMore(UnknownCommandToken,
                    (tokens) => new SyntaxList<SyntaxToken>(tokens)),
                (dot, parts) => (Command)new UnknownCommand(dot, parts))
                .WithTag("<unknown-command>");

        internal static readonly Parser<LexicalToken, Command> BadCommand =
            Rule(
                Token(SyntaxKind.DotToken),
                dot => (Command)new BadCommand(dot, new Diagnostic[] { DiagnosticFacts.GetMissingCommand() }))
                .WithTag("<bad-command>");

        internal static readonly Statement MissingCommandStatementNode =
            new ExpressionStatement(new BadCommand(SyntaxToken.Missing(SyntaxKind.DotToken), new[] { DiagnosticFacts.GetMissingCommand() }));
    }
}