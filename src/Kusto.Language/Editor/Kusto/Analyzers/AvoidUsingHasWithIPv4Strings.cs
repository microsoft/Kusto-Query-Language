using System;
using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Parsing;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;
    using static Parsers<char>;
    using static CharScanners;
    using static AnalyzerUtilities;

    internal class AvoidUsingHasWithIPv4StringsAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidUsingHasWithIPv4Strings,
                DiagnosticCategory.Performance,
                DiagnosticSeverity.Suggestion,
                description: "Avoid using has operator with IPv4 strings.",
                message: $"Avoid using the 'has' operator with IPv4 strings. Use the 'has_ipv4' function instead.");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<Expression>(e => IsSuspect(e, code.Globals)))
            {
                switch (node.Kind)
                {
                    case SyntaxKind.HasExpression:
                    case SyntaxKind.HasCsExpression:
                    case SyntaxKind.ContainsExpression:
                    case SyntaxKind.ContainsCsExpression:
                        var bx = (BinaryExpression)node;
                        diagnostics.Add(GetDiagnostic(bx.Operator.Text, "has_ipv4").WithLocation(bx.Operator));
                        break;
                    case SyntaxKind.HasPrefixExpression:
                    case SyntaxKind.HasPrefixCsExpression:
                        bx = (BinaryExpression)node;
                        diagnostics.Add(GetDiagnostic(bx.Operator.Text, "has_ipv4_prefix").WithLocation(bx.Operator));
                        break;
                    case SyntaxKind.HasAnyExpression:
                        var hax = (HasAnyExpression)node;
                        diagnostics.Add(GetDiagnostic(hax.Operator.Text, "has_any_ipv4").WithLocation(hax.Operator));
                        break;
                }
            }
        }

        private bool IsSuspect(Expression expr, GlobalState globals)
        {
            if (expr is BinaryExpression be)
            {
                switch (be.Kind)
                {
                    case SyntaxKind.HasExpression:
                    case SyntaxKind.HasCsExpression:
                    case SyntaxKind.ContainsExpression:
                    case SyntaxKind.ContainsCsExpression:
                        return IsDbColumn(be.Left, globals) && IsIPv4Constant(be.Right);

                    case SyntaxKind.HasPrefixExpression:
                    case SyntaxKind.HasPrefixCsExpression:
                        return IsDbColumn(be.Left, globals) && IsIPv4PrefixConstant(be.Right);
                }
            }
            else if (expr is HasAnyExpression hax)
            {
                return IsDbColumn(hax.Left, globals)
                    && hax.Right.Expressions.Count > 0
                    && hax.Right.Expressions.All(e => IsIPv4Constant(e.Element));
            }

            return false;
        }

        private static bool IsIPv4Constant(Expression expr) =>
            expr.ConstantValue is string value
            && IPv4.Matches(value);

        private static bool IsIPv4PrefixConstant(Expression expr) =>
            expr.ConstantValue is string value
            && IPv4_Prefix.Matches(value);

        // matches only 0-255
        private static Parser<char> Number =
            Or(
                And(Match('1'), Digit, Digit),
                And(Match('2'), Or(
                    And(MatchAny('0', '1', '2', '3', '4'), Digit),
                    And(Match('5'), MatchAny('0', '1', '2', '3', '4', '5')))),
                And(Digit, Optional(Digit))
                );
        private static Parser<char> Dot = Match('.');
        private static Parser<char> IPv4 = And(Number, Dot, Number, Dot, Number, Dot, Number);
        private static Parser<char> IPv4_Prefix = And(Number, Dot, Optional(And(Number, Dot, Optional(And(Number, Dot)))));
          
        private static Diagnostic GetDiagnostic(string operatorName, string ipv4FunctionName)
        {
            return _diagnostic.WithMessage(
                $"Avoid using the '{operatorName}' operator with IPv4 strings. Use the '{ipv4FunctionName}' function instead.");
        }

        protected override void GetFixAction(
            KustoCode code, 
            Diagnostic dx, 
            CodeActionOptions options, 
            List<CodeAction> actions, 
            CancellationToken cancellationToken)
        {
            if (dx.Code == _diagnostic.Code)
            {
                var token = code.Syntax.GetTokenAt(dx.Start);
                var ipv4Name = GetIPv4FunctionName(token.Kind);

                // encode diagnostic info and new name into data
                actions.Add(
                    CodeAction.Create(
                        $"Change to '{ipv4Name}'", 
                        $"Replace use of operator '{token.Text}' with a call to function '{ipv4Name}'",
                        data: new [] { dx.Start.ToString(), ipv4Name }));
            }
        }

        protected override FixEdits GetFixEdits(
            KustoCode code,
            ApplyAction action,
            int caretPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            // decode data and apply change
            if (action.Data.Count == 2
                && Int32.TryParse(action.Data[0], out var tokenStart))
            {
                var opToken = code.Syntax.GetTokenAt(tokenStart);
                var node = opToken?.Parent;
                var newName = action.Data[1];

                if (opToken != null)
                {
                    // convert operator into a function call
                    if (node is HasAnyExpression hax)
                    {
                        return new FixEdits(
                            hax.TextStart,
                            TextEdit.Replacement(hax.Operator.TriviaStart, hax.Right.OpenParen.End - hax.Operator.TriviaStart, ", "),
                            TextEdit.Insertion(hax.TextStart, newName + "(")
                            );
                    }
                    else if (node is BinaryExpression bx)
                    {
                        return new FixEdits(
                            node.TextStart,
                            TextEdit.Insertion(bx.End, ")"),
                            TextEdit.Replacement(bx.Operator.TriviaStart, bx.Right.TextStart - bx.Operator.TriviaStart, ", "),
                            TextEdit.Insertion(node.TextStart, newName + "(")
                            );
                    }
                }
            }

            return new FixEdits(caretPosition);
        }

        private static string GetIPv4FunctionName(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.HasPrefixExpression:
                case SyntaxKind.HasPrefixCsExpression:
                case SyntaxKind.HasPrefixKeyword:
                case SyntaxKind.HasPrefixCsKeyword:
                    return "has_ipv4_prefix";
                case SyntaxKind.HasAnyKeyword:
                case SyntaxKind.HasAnyExpression:
                    return "has_any_ipv4";
                default:
                    return "has_ipv4";
            }
        }
    }
}