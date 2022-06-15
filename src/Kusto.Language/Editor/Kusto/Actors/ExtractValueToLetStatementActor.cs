using System;
using System.Collections.Generic;

namespace Kusto.Language.Editor
{
    using Kusto.Language.Parsing;
    using Kusto.Language.Symbols;
    using Kusto.Language.Syntax;
    using System.Diagnostics;
    using System.Linq;
    using Utils;
    using static ActorUtilities;

    public class ExtractExpressionActor : KustoActor
    {
        private const string ExtractValueName = "Extract Value";
        private const string ExtractExpressionName = "Extract Expression";
        private const string ExtractFunctionName = "Extract Function";

        private static readonly CodeAction ExtractValueAction = new CodeAction(
            nameof(ExtractExpressionActor), ExtractValueName, "Extract value into new let statement variable declaration.", "");

        private static readonly CodeAction ExtractExpressionAction = new CodeAction(
            nameof(ExtractExpressionActor), ExtractExpressionName, "Extract expression into new let statement variable declaration.", "");

        private static readonly CodeAction ExtractFunctionAction = new CodeAction(
            nameof(ExtractExpressionActor), ExtractFunctionName, "Extract expression into new let statement function declaration.", "");

        public override void GetActions(
            KustoCode code,
            int position,
            int length,
            CodeActionOptions options,
            List<CodeAction> actions,
            CancellationToken cancellationToken)
        {
            if (length > 0 )
            {
                if (CanExtractExpression(code, position, length))
                {
                    actions.Add(ExtractExpressionAction);
                }

                if (CanExtractFunction(code, position, length))
                {
                    actions.Add(ExtractFunctionAction);
                }
            }
            else if (CanExtractValue(code, position))
            {
                actions.Add(ExtractValueAction);
            }
        }

        private bool CanExtractValue(KustoCode code, int position)
        {
            var token = GetTokenNear(code, position);

            if (position >= token.TextStart
                && position <= token.End
                && token.Parent is LiteralExpression lit
                && !IsExpressionOfLetStatement(lit))
            {
                return true;
            }

            return false;
        }

        private bool CanExtractExpression(KustoCode code, int position, int length)
        {
            TrimRange(code, ref position, ref length);

            if (!(code.Syntax.GetNodeAt(position, length) is Expression ex))
                return false;

            if (ex.TextStart < position || ex.End > position + length)
                return false;

            if (IsExpressionOfLetStatement(ex) || !IsFreeStandingExpression(ex))
                return false;

            // reject if there are any references to columns in scope (that would need to be parameters)
            if (GetCandidateParameters(code, position, length).Count > 0)
                return false;

            return true;
        }

        private bool CanExtractFunction(KustoCode code, int position, int length)
        {
            TrimRange(code, ref position, ref length);

            // this will find the overall pipe expression if range is only over a portion.
            if (code.Syntax.GetNodeAt(position, length) is Expression ex)
            {
                if (ex is PipeExpression pe && TryGetAdjustedSubqueryRange(code, ref position, ref length))
                {
                    // subquery must have columns in scope from prior context/operator to be considered for extract as a tabular function
                    return GetColumnsInScope(code, position).Count > 0;
                }
                else if (ex is QueryOperator && GetColumnsInScope(code, position).Count > 0)
                {
                    // a stand alone query operator but it is in a context that has columns in scope
                    return true;
                }
                else
                {
                    // as a scalar function if this is a scalar expression that has references to columns, 
                    // unless its already the expression of a let statement, etc.
                    return 
                        !IsExpressionOfLetStatement(ex)
                        && (ex.ResultType != null && ex.ResultType.IsScalar)
                        && GetCandidateParameters(code, position, length).Count > 0;
                }
            }

            return false;
        }

        public override CodeActionResult ApplyAction(
            KustoCode code,
            int position,
            int length,
            CodeActionOptions options,
            CodeAction action,
            CancellationToken cancellationToken)
        {
            System.Diagnostics.Debug.Assert(options != null);

            if (action.Name == ExtractExpressionName)
            {
                return GetExpressionResult(code, position, length);
            }
            else if (action.Name == ExtractFunctionName)
            {
                return GetFunctionResult(code, position, length, options);
            }
            else if (action.Name == ExtractValueName)
            {
                return GetValueResult(code, position);
            }

            return CodeActionResult.Failure("Unknown action");
        }

        private CodeActionResult GetValueResult(KustoCode code, int position)
        {
            var token = GetTokenNear(code, position);
            if (position >= token.TextStart
                && position <= token.End
                && token.Parent is LiteralExpression lit)
            {
                return GetExpressionResult(code, token.TextStart, token.Width);
            }
            else
            {
                return CodeActionResult.Failure("Invalid literal value");
            }
        }

        private CodeActionResult GetExpressionResult(KustoCode code, int position, int length)
        {
            TrimRange(code, ref position, ref length);

            if (code.Syntax.GetNodeAt(position, length) is Expression ex
                && ex.TextStart >= position && ex.End <= position + length)
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
                    var newPosition = insertionPosition + indentation.Length + 4; // start of name in let statement

                    return new CodeActionResult(
                        new ChangeTextAction(newText),
                        new MoveCaretAction(newPosition),
                        RenameAction.Instance);
                }
                else
                {
                    return CodeActionResult.Failure("No insertion location available.");
                }
            }
            else
            {
                return CodeActionResult.Failure("Invalid selection range");
            }
        }

        private CodeActionResult GetFunctionResult(KustoCode code, int position, int length, CodeActionOptions options)
        {
            TrimRange(code, ref position, ref length);

            if (code.Syntax.GetNodeAt(position, length) is Expression ex)
            {
                if (ex is PipeExpression pe || ex.ResultType != null && ex.ResultType.IsTabular)
                {
                    return GetTabularFunctionResult(code, position, length, options);
                }
                else if (ex.ResultType != null && ex.ResultType.IsScalar)
                {
                    return GetScalarFunctionResult(code, position, length, options);
                }
            }

            return CodeActionResult.Failure("Invalid selection range");
        }

        private CodeActionResult GetScalarFunctionResult(KustoCode code, int position, int length, CodeActionOptions options)
        {
            if (TryGetNearestStatementInsertionPosition(code, position, out var insertionPosition))
            {
                var candidateParameters = GetCandidateParameters(code, position, length);
                var parameterList = GetSchema(candidateParameters);
                var argumentList = "(" + string.Join(", ", candidateParameters.Select(s => KustoFacts.BracketNameIfNecessary(s.Name))) + ")";
                var baseName = "functionName";
                var newName = GetNewName(code, position, baseName);
                var indentation = TextFacts.GetWhitespace(code.Text, insertionPosition);
                var letStatement = $"{indentation}let {newName} = {parameterList} {{ {code.Text.Substring(position, length)} }};\n";
                var invocation = $"{newName}{argumentList}";
                var newText = new EditString(code.Text)
                    .ReplaceAt(position, length, invocation)
                    .Insert(insertionPosition, letStatement);
                var newPosition = insertionPosition + indentation.Length + 4; // start of name in let statement

                return new CodeActionResult(
                    new ChangeTextAction(newText),
                    new MoveCaretAction(newPosition),
                    RenameAction.Instance);
            }

            return CodeActionResult.Failure("No insertion location available.");
        }

        private CodeActionResult GetTabularFunctionResult(KustoCode code, int position, int length, CodeActionOptions options)
        {
            if (TryGetAdjustedSubqueryRange(code, ref position, ref length)                
                && TryGetNearestStatementInsertionPosition(code, position, out var insertionPosition))
            {
                // all the columns (candidate parameters) come from the operator's input table
                // so just make one tabular paramenter that will get supplied via the invoke operator
                var candidateParameters = GetCandidateParameters(code, position, length);
                var tableArgumentType = candidateParameters.Count > 0 ? GetSchema(candidateParameters) : "(*)";
                var tableArgumentName = GetNewName(code, position, "inputTable");
                var parameterList = $"({tableArgumentName}: {tableArgumentType})";

                var baseName = "functionName";
                var newName = GetNewName(code, position, baseName);
                var lineIndentation = TextFacts.GetWhitespace(code.Text, insertionPosition);

                var nestedIdentation = lineIndentation + new string(' ', options.FormattingOptions.IndentationSize);
                var insertionText = code.Text.Substring(position, length);
                insertionText = ChangeIndentation(insertionText, nestedIdentation, includeFirstLine: false);

                var letStatement = $"{lineIndentation}let {newName} = {parameterList} {{\n{nestedIdentation}{tableArgumentName}\n{nestedIdentation}| {insertionText}\n{lineIndentation}}};\n";
                var invocation = $"invoke {newName}()";
                var newText = new EditString(code.Text)
                    .ReplaceAt(position, length, invocation)
                    .Insert(insertionPosition, letStatement);
                var newPosition = insertionPosition + lineIndentation.Length + 4; // start of name in let statement

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

        private static IReadOnlyList<ColumnSymbol> GetCandidateParameters(KustoCode code, int start, int length)
        {
            var externalSymbols = GetExternalSymbolsReferenced(code, start, length)
                .OfType<ColumnSymbol>()
                .ToList();
            
            if (externalSymbols.Count == 0
                || !TryGetNearestStatementInsertionPosition(code, start, out var insertionPosition))
            {
                return EmptyReadOnlyList<ColumnSymbol>.Instance;
            }

            var symbolsAtInsertionPoint = code.GetSymbolsInScope(insertionPosition);

            // any referenced symbol declared before the fragment less all the symbols declared before the insertion point
            var remainingSymbols = externalSymbols.Except(symbolsAtInsertionPoint).OfType<ColumnSymbol>().ToReadOnly();

            return remainingSymbols;
        }
    }
}