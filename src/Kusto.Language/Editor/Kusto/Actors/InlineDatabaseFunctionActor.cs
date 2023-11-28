using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Kusto.Language.Symbols;
    using Kusto.Language.Syntax;
    using Utils;
    using static ActorUtilities;

    internal class InlineDatabaseFunctionActor : KustoActor
    {
        public const string InlineOnceKind = "InlineOnce";
        public const string InlineRecursiveKind = "InlineRecursive";
        public const string InlineAllOnceKind = "InlineAllOnce";
        public const string InlineAllRecursiveKind = "InlineAllRecursive";

        private static readonly MenuAction InlineFunctionMenuAction = CodeAction.CreateMenu(
            "Inline Function", "Copy database function into query.", null);

        private static readonly ApplyAction InlineOnceAction = CodeAction.Create(
            InlineOnceKind, "Inline", "Inline this function only.");

        private static readonly ApplyAction InlineRecursiveAction = CodeAction.Create(
            InlineRecursiveKind, "Inline Recursive", "Inline this function recursively.");

        private static readonly ApplyAction InlineAllOnceAction = CodeAction.Create(
            InlineAllOnceKind, "Inline All", "Inline all functions in selection or query.");

        private static readonly ApplyAction InlineAllRecursiveAction = CodeAction.Create(
            InlineAllRecursiveKind, "Inline All, Recursive", "Inline all functions in selection or query, recursively.");

        public override void GetActions(
            KustoCodeService service,
            KustoCode code, 
            int position,
            int selectionStart, 
            int selectionLength,
            CodeActionOptions options,           
            List<CodeAction> actions,
            bool waitForAnalysis,
            CancellationToken cancellationToken)
        {
            var token = code.Syntax.GetTokenAt(position);
            if (position >= token.TextStart && position <= token.End)
            {
                var node = code.Syntax.GetNodeAt(token.TextStart, token.Text.Length);
                if (node is Name name
                    && name.Parent is NameReference nr
                    && IsDatabaseFunctionReference(nr, code.Globals))
                {
                    if (selectionLength == 0)
                    {
                        selectionStart = 0;
                        selectionLength = code.Text.Length;
                    }

                    var menuActions = new List<CodeAction>();
                    menuActions.Add(InlineOnceAction.WithData(position.ToString()));
                    menuActions.Add(InlineRecursiveAction.WithData(position.ToString()));

                    // if there are additional database functions in the selection, then add the *all* options
                    var functionsInRange = GetDatabaseFunctionsReferencedInRange(code, selectionStart, selectionLength);
                    if (functionsInRange.Count > 1)
                    {
                        menuActions.Add(InlineAllOnceAction.WithData(selectionStart.ToString(), selectionLength.ToString()));
                        menuActions.Add(InlineAllRecursiveAction.WithData(selectionStart.ToString(), selectionLength.ToString()));
                    }

                    // add a menu of all the actions
                    actions.Add(InlineFunctionMenuAction.WithActions(menuActions));
                }
            }
        }

        private static bool IsDatabaseFunctionReference(NameReference nameRef, GlobalState globals)
        {
            return nameRef.ReferencedSymbol is FunctionSymbol fs
                && globals.IsDatabaseFunction(fs)
                && IsGoodReference(nameRef, fs);
        }

        private static bool IsGoodReference(NameReference nameRef, FunctionSymbol fs)
        {
            // disallow argument-list-less function references to functions that require arguments
            // as these won't have expansions later when applied.
            return nameRef.Parent is FunctionCallExpression
                || fs.MinArgumentCount == 0;
        }

        public override CodeActionResult ApplyAction(
            KustoCodeService service,
            KustoCode code,
            ApplyAction action,
            int caretPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            if (action.Data.Count >= 1
                && Int32.TryParse(action.Data[0], out var position))
            {
                var length = 0;
                if (action.Data.Count >= 2)
                {
                    Int32.TryParse(action.Data[1], out length);
                }

                switch (action.Kind)
                {
                    case InlineOnceKind:
                        return InlineOnce(code, position, caretPosition);
                    case InlineRecursiveKind:
                        return InlineRecursive(code, position, caretPosition);
                    case InlineAllOnceKind:
                        return InlineAllOnce(code, position, length, caretPosition);
                    case InlineAllRecursiveKind:
                        return InlineAllRecursive(code, position, length, caretPosition);
                }
            }

            return CodeActionResult.Failure("Bad action data");
        }

        /// <summary>
        /// Inline the fuction referenced at the position.
        /// </summary>
        private static CodeActionResult InlineOnce(KustoCode code, int functionPosition, int caretPosition)
        {
            return InlineAllOnce(code, functionPosition, 0, caretPosition);
        }

        /// <summary>
        /// Inline the function referenced at the position, recursively.
        /// </summary>
        private static CodeActionResult InlineRecursive(KustoCode code, int functionPosition, int caretPosition)
        {
            return InlineAllRecursive(code, functionPosition, 0, caretPosition);
        }

        /// <summary>
        /// Inline all functions referenced within the text range.
        /// </summary>
        private static CodeActionResult InlineAllOnce(KustoCode code, int selectionStart, int selectionLength, int caretPosition)
        {
            var functions = GetDatabaseFunctionsReferencedInRange(code, selectionStart, selectionLength);
            return InlineFunctions(code, functions, caretPosition, recursive: false);
        }

        /// <summary>
        /// Inline all functions referenced within the text range, recursively.
        /// </summary>
        private static CodeActionResult InlineAllRecursive(KustoCode code, int selectionStart, int selectionLength, int caretPosition)
        {
            var functions = GetDatabaseFunctionsReferencedInRange(code, selectionStart, selectionLength);
            return InlineFunctions(code, functions, caretPosition, recursive: true);
        }

        /// <summary>
        /// Gets a list of the database functions referenced the specified range.
        /// </summary>
        private static IReadOnlyList<FunctionSymbol> GetDatabaseFunctionsReferencedInRange(KustoCode code, int selectionStart, int selectionLength)
        {
            return
                code.Syntax.GetDescendants<NameReference>(nr => IsDatabaseFunctionReference(nr, code.Globals))
                .Where(nr => RangeOverlaps(nr.TextStart, nr.Width, selectionStart, selectionLength))
                .Select(nr => (FunctionSymbol)nr.ReferencedSymbol)
                .Distinct()
                .ToList();
        }

        private static bool RangeOverlaps(int start1, int length1, int start2, int length2)
        {
            var minEnd = Math.Min(start1 + length1, start2 + length2);
            var maxStart = Math.Max(start1, start2);
            return minEnd >= maxStart;
        }

        /// <summary>
        /// Inline the specified database functions.
        /// </summary>
        private static CodeActionResult InlineFunctions(KustoCode code, IReadOnlyList<FunctionSymbol> functions, int caretPosition, bool recursive)
        {
            CodeActionResult resultAction = CodeActionResult.Nothing;

            var remaining = functions.ToHashSetEx();
            var alreadyInlined = new HashSet<FunctionSymbol>();

            while (remaining.Count > 0)
            {
                var function = remaining.First();
                remaining.Remove(function);
                alreadyInlined.Add(function);

                var action = InlineFunction(code, function, caretPosition);
                if (action.FailureReason != null)
                {
                    // if this is the first inlining attempt then use this action as the result action
                    if (resultAction == CodeActionResult.Nothing)
                    {
                        resultAction = action;
                    }
                    break;
                }

                resultAction = action;

                // check for more references to expand by re-analyzing changed text and trying again
                var changeAction = resultAction.Actions.OfType<ChangeTextAction>().First();
                var moveAction = resultAction.Actions.OfType<MoveCaretAction>().First();
                caretPosition = moveAction.NewCaretPosition;
                code = KustoCode.ParseAndAnalyze(changeAction.ChangedText, code.Globals);

                if (recursive)
                {
                    // look for any additional functions in the inlined function body and add them to the work load.
                    var range = new EditString(code.Text).ApplyAll(changeAction.Changes).GetChangeRange();
                    // subtract one from length so to not include any reference immediately adjacent to end of range
                    var additionalFunctions = 
                        GetDatabaseFunctionsReferencedInRange(code, range.Start, range.Length - 1)
                        .Where(f => !alreadyInlined.Contains(f))
                        .ToList();
                    remaining.AddRange(additionalFunctions);
                }
            }

            return resultAction;
        }

        /// <summary>
        /// Inline the specified database function.
        /// </summary>
        private static CodeActionResult InlineFunction(KustoCode code, FunctionSymbol function, int caretPosition)
        {
            // we need to insert the function before the first reference to it.
            var nr = GetFirstReference(code.Syntax, function);
            if (nr != null)
            {
                if (TryGetNearestTopLevelStatementInsertionPosition(code, nr.TextStart, out var insertPosition))
                {
                    var body = nr.Parent is FunctionCallExpression fc
                        ? fc.GetCalledFunctionBody()
                        : nr.GetCalledFunctionBody();

                    if (body != null)
                    {
                        var requalifiedBody = GetBodyWithBraces(GetRequalifiedDatabaseFunctionBody(body, code.Globals));
                        var declaration = GetLetStatement(function, requalifiedBody);
                        var requalifiedQuery = GetQueryWithDatabaseQualifiersRemoved(code.Syntax, function, code.Globals);
                        var newText = requalifiedQuery.Insert(insertPosition, declaration + "\n");
                        var newCaretPosition = newText.GetCurrentPosition(caretPosition);
                        return CodeActionResult.ChangeAndMove(newText, newCaretPosition);
                    }
                    else
                    {
                        return CodeActionResult.Failure("Could not access definition of referenced function");
                    }
                }
            }

            // nothing happened
            return CodeActionResult.Failure("Reference to database function not found");
        }

        private static string GetLetStatement(FunctionSymbol function, string body = null)
        {
            if (body == null)
                body = GetBodyWithBraces(function.Signatures[0].Body);
            var parameters = Parameter.GetParameterListDeclaration(function.Signatures[0].Parameters);
            return $"let {KustoFacts.BracketNameIfNecessary(function.Name)} = {parameters} {body};";
        }

        private static EditString GetBodyWithBraces(string body)
        {
            var edit = new EditString(body);

            if (!edit.CurrentText.StartsWith("{", StringComparison.Ordinal))
                edit = edit.Prepend("{\n");

            if (!edit.CurrentText.EndsWith("}", StringComparison.Ordinal))
                edit = edit.Append("\n}");

            return edit;
        }

        private static string GetRequalifiedDatabaseFunctionBody(SyntaxNode body, GlobalState globals)
        {
            var text = new EditString(body.ToString(IncludeTrivia.All));
            return KustoQualifier.Requalify(text, body, globals, globals.Cluster, globals.Database);
        }
    }
}