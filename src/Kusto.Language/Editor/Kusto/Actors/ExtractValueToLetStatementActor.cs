using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Kusto.Language.Parsing;
    using Kusto.Language.Symbols;
    using Kusto.Language.Syntax;
    using Utils;
    using static ActorUtilities;

    public class ExtractExpressionActor : KustoActor
    {
        public static readonly CodeAction ExtractValueAction = new CodeAction(
            nameof(ExtractExpressionActor), "Extract Value", "Extract value into new let statement.", "");

        public static readonly CodeAction ExtractSelectionAction = new CodeAction(
            nameof(ExtractExpressionActor), "Extract Selection", "Extract selection into new let statement.", "");

        public override void GetActions(
            KustoCode code,
            int position, int length,
            IReadOnlyList<Diagnostic> additionalDiagnostics,
            List<CodeAction> actions,
            CancellationToken cancellationToken)
        {
            // either extract selection range or extract single literal value at position
            if (length > 0 )
            {
                var node = GetNodeInRange(code, position, length);
                if (node is Expression ex 
                    && CanExtractExpression(code, ex))
                {
                    actions.Add(ExtractSelectionAction);
                }
            }
            else
            {
                var token = code.Syntax.GetTokenAt(position);

                if (position >= token.TextStart
                    && position <= token.End
                    && token.Parent is LiteralExpression lit
                    && !IsExpressionOfLetStatement(lit))
                {
                    actions.Add(ExtractValueAction);
                }
            }
        }

        private bool CanExtractExpression(KustoCode code, Expression ex)
        {
            if (IsExpressionOfLetStatement(ex) || !IsFreeStandingExpression(ex))
                return false;

            // reject if there are any references to columns in scope
            var columnsInScope = code.GetColumnsInScope(ex.TextStart);
            if (columnsInScope != null)
            {
                foreach (var nameRef in ex.GetDescendants<NameReference>())
                {
                    if (nameRef.ReferencedSymbol is ColumnSymbol c
                        && columnsInScope.GetColumn(c.Name) == c)
                    {
                        // there was a reference to an implicit column
                        return false;
                    }
                }
            }

            return true;
        }

        public override CodeActionResult ApplyAction(
            KustoCode code,
            int position, int length,
            IReadOnlyList<Diagnostic> additionalDiagnostics,
            CodeAction action,
            CancellationToken cancellationToken)
        {
            if (action == ExtractSelectionAction)
            {
                var node = GetNodeInRange(code, position, length);
                if (node is Expression ex)
                {
                    return GetResult(code, position, ex);
                }
            }
            else if (action == ExtractValueAction)
            {
                var token = code.Syntax.GetTokenAt(position);
                if (position >= token.TextStart 
                    && position <= token.End
                    && token.Parent is LiteralExpression lit)
                {
                    return GetResult(code, position, lit);
                }
            }

            // nothing happens?
            return CodeActionResult.Nothing;
        }

        private CodeActionResult GetResult(KustoCode code, int position, Expression ex)
        {
            if (TryGetNearestStatementInsertionPosition(code, position, out var insertionPosition))
            {
                var baseName = GetLetVariableName(ex);
                var newName = GetNewName(code, position, baseName);
                var indentation = TextFacts.GetWhitespace(code.Text, insertionPosition);
                var letStatement = $"{indentation}let {newName} = {ex.ToString(IncludeTrivia.Interior)};\n";
                var newText = new EditString(code.Text)
                    .ReplaceAt(ex.TextStart, ex.Width, newName)
                    .Insert(insertionPosition, letStatement);
                var newPosition = newText.GetCurrentPosition(ex.TextStart);

                return new CodeActionResult(
                    new ChangeTextAction(newText),
                    new MoveCaretAction(newPosition),
                    RenameAction.Instance);
            }

            return CodeActionResult.Failure("No insertion location available.");
        }

        private static string GetLetVariableName(Expression expr)
        {
            if (expr is PipeExpression || expr is QueryOperator || (expr.ResultType != null && expr.ResultType.IsTabular))
            {
                return "queryName";
            }
            else
            {
                return "varName";
            }
        }
    }
}