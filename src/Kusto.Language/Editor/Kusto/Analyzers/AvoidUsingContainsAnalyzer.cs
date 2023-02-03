using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Utils;
    using static AnalyzerUtilities;

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
                if (GetHasOperatorKind(node.Operator.Kind) != SyntaxKind.None
                    && IsDbColumn(node.Left, code.Globals)
                    && !IsShortStringConstant(node.Right, code.Globals))  // don't warn for short strings
                {
                    diagnostics.Add(_diagnostic.WithLocation(node.Operator));
                }
            }
        }

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
                        actions.Add(
                            CodeAction.Create(
                                $"Change to '{hasOp}'", 
                                $"Replace operator '{opToken.Text}' with operator '{hasOp}'", 
                                data: new[] { dx.Start.ToString(), hasOp }));
                    }
                }
            }
        }

        protected override FixEdits GetFixEdits(
            KustoCode code,
            ApplyAction action,
            int caretPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            if (action.Data.Count == 2
                && Int32.TryParse(action.Data[0], out var opTokenStart))
            {
                var opToken = code.Syntax.GetTokenAt(opTokenStart);
                var newOpName = action.Data[1];
                return new FixEdits(
                    opToken.TextStart,
                    TextEdit.Replacement(opToken.TextStart, opToken.Width, newOpName));
            }
            else
            {
                return new FixEdits(caretPosition);
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