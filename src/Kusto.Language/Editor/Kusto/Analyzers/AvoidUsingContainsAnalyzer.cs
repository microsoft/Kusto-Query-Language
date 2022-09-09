using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Symbols;
    using Utils;

    internal class AvoidUsingContainsAnalyzer : KustoAnalyzer
    {
        private static readonly Diagnostic _diagnostic =
            new Diagnostic(
                KustoAnalyzerCodes.AvoidUsingContains,
                DiagnosticCategory.Performance,
                DiagnosticSeverity.Suggestion,
                description: "Avoid using contains operator",
                message: $"Avoid using the 'contains' operator as it has a high compute price." + Environment.NewLine +
                         $"Use the 'has' operator in cases when full term match is desired (see: https://aka.ms/kusto.stringterms).");

        protected override IEnumerable<Diagnostic> GetDiagnostics()
        {
            return new[] { _diagnostic };
        }

        public override void Analyze(KustoCode code, List<Diagnostic> diagnostics, CancellationToken cancellationToken)
        {
            foreach (var node in code.Syntax.GetDescendants<BinaryExpression>())
            {
                if (GetHasOperatorKind(node.Operator.Kind) != SyntaxKind.None)
                {
                    // only report if db column is being used directly
                    if (IsDbColumn(node.Left, code.Globals))
                    {
                        diagnostics.Add(_diagnostic.WithLocation(node.Operator));
                    }
                }
            }
        }

        protected static bool IsDbColumn(Expression expr, GlobalState globals) =>
            expr.ReferencedSymbol is ColumnSymbol c && globals.GetTable(c) != null;

        protected override void GetFixAction(
            KustoCode code, 
            Diagnostic dx, 
            CodeActionOptions options, 
            List<CodeAction> actions,
            CancellationToken cancellationToken)
        {
            if (dx.Code == KustoAnalyzerCodes.AvoidUsingContains)
            {
                var opToken = code.Syntax.GetTokenAt(dx.Start);
                if (options != null)
                {
                    var hasKind = GetHasOperatorKind(opToken.Kind);
                    if (hasKind != SyntaxKind.None)
                    {
                        var hasOp = hasKind.GetText();
                        actions.Add(new CodeAction(
                            $"Change to '{hasOp}'", 
                            $"Replace operator '{opToken.Text}' with operator '{hasOp}'", 
                            dx.Start.ToString(), 
                            hasOp));
                    }
                }
            }
        }

        protected override FixResult GetFixEdits(
            KustoCode code,
            CodeAction action,
            int caretPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            if (action.Data.Count == 2
                && Int32.TryParse(action.Data[0], out var opTokenStart))
            {
                var opToken = code.Syntax.GetTokenAt(opTokenStart);
                var newOpName = action.Data[1];
                return new FixResult(
                    opToken.TextStart,
                    StringEdit.Replacement(opToken.TextStart, opToken.Width, newOpName));
            }
            else
            {
                return new FixResult(caretPosition);
            }
        }

        private static SyntaxKind GetHasOperatorKind(SyntaxKind tokenKind)
        {
            switch (tokenKind)
            {
                case SyntaxKind.ContainsKeyword:
                    return SyntaxKind.HasKeyword;
                case SyntaxKind.ContainsCsKeyword:
                case SyntaxKind.Contains_CsKeyword:
                    return SyntaxKind.HasCsKeyword;
                case SyntaxKind.NotContainsKeyword:
                case SyntaxKind.NotBangContainsKeyword:
                    return SyntaxKind.NotHasKeyword;
                case SyntaxKind.NotContainsCsKeyword:
                case SyntaxKind.NotBangContainsCsKeyword:
                    return SyntaxKind.NotHasCsKeyword;
                default:
                    return SyntaxKind.None;
            }
        }
    }
}