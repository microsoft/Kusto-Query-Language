using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Parsing;
    using Symbols;
    using Syntax;
    using Utils;
    using static ActorUtilities;

    internal class ConvertToMacroExpandActor : KustoActor
    {
        public const string ConvertSelectionKind = "ConvertSelection";
        public const string ConvertUnionKind = "ConvertUnion";

        private static readonly ApplyAction ConvertSelectionToMacroExpandAction = CodeAction.Create(
            ConvertSelectionKind, "Convert selection to macro-expand", "Move the selection into macro-expand body.");

        private static readonly ApplyAction ConvertUnionToMacroExpandAction = CodeAction.Create(
            ConvertUnionKind, "Convert union to macro-expand", "Convert union inputs into entity group and move remaining query into a macro-expand expression.");

        private static string InitialEntityGroupElementName = "_scope";

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

            if (position >= token.TextStart && position <= token.End
                && !IsInsideMacroExpand(token))
            {
                if (GetEntityExpressionStart(token, code.Globals) is Expression entity)
                {
                    if (selectionLength == 0)
                    {
                        selectionStart = 0;
                        selectionLength = code.Text.Length;
                    }

                    actions.Add(
                        ConvertSelectionToMacroExpandAction
                        .WithData(
                            selectionStart.ToString(),
                            selectionLength.ToString(),
                            position.ToString()));
                }

                if (TryGetUnionQuery(token, out var query)
                    && TryGetUnionEntityExpressions(query, code.Globals, out _))
                {
                    actions.Add(
                        ConvertUnionToMacroExpandAction
                        .WithData(
                            position.ToString()));
                }
            }
        }

        private static bool IsInsideMacroExpand(SyntaxElement element)
        {
            return element.GetFirstAncestor<MacroExpandOperator>() != null;
        }

        public override CodeActionResult ApplyAction(
            KustoCodeService service,
            KustoCode code,
            ApplyAction action,
            int caretPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            switch (action.Kind)
            {
                case ConvertSelectionKind:
                    return ConvertSelection(code, action, options);
                case ConvertUnionKind:
                    return ConvertUnion(code, action, options);
                default:
                    return CodeActionResult.Failure($"Unknown action kind: {action.Kind}");
            }
        }

        private CodeActionResult ConvertSelection(KustoCode code, ApplyAction action, CodeActionOptions options)
        {
            if (action.Data.Count >= 3
                && Int32.TryParse(action.Data[0], out var selectionStart)
                && Int32.TryParse(action.Data[1], out var selectionLength)
                && Int32.TryParse(action.Data[2], out var position)
                && code.Syntax.GetTokenAt(position) is SyntaxToken token
                && GetEntityExpressionStart(token, code.Globals) is Expression entityStart
                && entityStart.ResultType is Symbol entitySymbol)
            {
                TextFacts.TrimRangeEnd(code.Text, selectionStart, ref selectionLength);
                AdjustRangeToNode(code, ref selectionStart, ref selectionLength);

                var originalText = new EditString(code.Text);

                // wrap selection in macro-expand
                var textWithBrackets = originalText.InsertBrackets(
                    selectionStart, $"macro-expand entity_group [{entityStart.ToString(IncludeTrivia.Interior)}] as {InitialEntityGroupElementName}", "(",
                    selectionStart + selectionLength, ")",
                    options.FormattingOptions);

                // replace all references to entities with entity group element
                var entityExpressions = GetEntityExpressionStarts(code, entitySymbol);
                var textWithReplacements = textWithBrackets.ApplyAll(
                    entityExpressions.Select(ex =>
                    {
                        // convert to updated positions
                        textWithBrackets.GetCurrentRange(ex.TextStart, ex.Width, out var newStart, out var newWidth);
                        return TextEdit.Replacement(newStart, newWidth, InitialEntityGroupElementName);
                    }).ToArray());

                var newCaretPosition = textWithReplacements.CurrentText.IndexOf(InitialEntityGroupElementName);

                return new CodeActionResult(
                    new ChangeTextAction(textWithReplacements),
                    new MoveCaretAction(newCaretPosition),
                    new RenameAction());
            }

            return CodeActionResult.Failure();
        }

        /// <summary>
        /// Gets all the entity expression starts referring to the specified symbol.
        /// </summary>
        private List<Expression> GetEntityExpressionStarts(KustoCode code, Symbol symbol)
        {
            return code.Syntax
                .GetDescendantsOrSelf<Expression>(ex => ex.ResultType == symbol && !(ex.Parent is PathExpression pe && pe.Selector == ex))
                .Select(ex => GetEntityExpressionStart(ex, code.Globals))
                .Where(ex => ex != null)
                .Distinct()
                .ToList();
        }

        private CodeActionResult ConvertUnion(KustoCode code, ApplyAction action, CodeActionOptions options)
        {
            if (action.Data.Count >= 1
                && Int32.TryParse(action.Data[0], out var position)
                && code.Syntax.GetTokenAt(position) is SyntaxToken token
                && TryGetUnionQuery(token, out var unionQuery)
                && TryGetUnionEntityExpressions(unionQuery, code.Globals, out var entities)
                && TryGetQuery(token, out var query))
            {
                var originalText = new EditString(code.Text);

                // wrap entire query in macro-expand
                var entityList = string.Join(", ", entities.Select(e => e.ToString(IncludeTrivia.Interior)));
                var textWithBrackets = originalText.InsertBrackets(
                    query.TextStart, $"macro-expand entity_group [{entityList}] as {InitialEntityGroupElementName}", "(",
                    query.End, ")",
                    options.FormattingOptions);

                // replace union query with entity group element
                textWithBrackets.GetCurrentRange(unionQuery.TextStart, unionQuery.End - unionQuery.TextStart, out var newUnionQueryStart, out var newUnionQueryLength);
                var textWithReplacements = textWithBrackets.ReplaceAt(newUnionQueryStart, newUnionQueryLength, InitialEntityGroupElementName);

                var newCaretPosition = textWithReplacements.CurrentText.IndexOf(InitialEntityGroupElementName);

                return new CodeActionResult(
                    new ChangeTextAction(textWithReplacements),
                    new MoveCaretAction(newCaretPosition),
                    new RenameAction());
            }

            return CodeActionResult.Failure();
        }

        private bool TryGetUnionQuery(SyntaxToken token, out Expression query)
        {
            // look for ancestor that is a union operator
            query = token.Parent?.GetFirstAncestorOrSelf<UnionOperator>();

            // also might be on left of pipe into union operator
            if (query == null)
            {
                query = token.Parent?.GetFirstAncestorOrSelf<PipeExpression>(p => p.Operator is UnionOperator);
            }

            // navigate to root of entire union query
            while (query?.Parent is PipeExpression pipe)
            {
                if (pipe.Operator is UnionOperator)
                {
                    query = pipe;
                    continue;
                }

                break;
            }

            return query != null;
        }

        private bool TryGetQuery(SyntaxToken token, out Expression query)
        {
            // look for ancestor this is a query operator
            query = token.Parent?.GetFirstAncestorOrSelf<QueryOperator>();

            // also might be on left of pipe into query
            if (query == null)
            {
                query = token.Parent?.GetFirstAncestorOrSelf<PipeExpression>();
            }

            // navigate to root of entire query
            while (query?.Parent is PipeExpression pipe)
            {
                query = pipe;
                continue;
            }

            return query != null;
        }

        private bool TryGetUnionEntityExpressions(Expression query, GlobalState globals, out List<Expression> entities)
        {
            entities = new List<Expression>();
            return GetUnionEntityExpressions(query, globals, entities);
        }

        private bool GetUnionEntityExpressions(Expression query, GlobalState globals, List<Expression> entities)
        {
            // gather all entity expressions in union query
            if (query is PipeExpression pipe)
            {
                if (!GetUnionEntityExpressions(pipe.Expression, globals, entities))
                    return false;

                if (!GetUnionEntityExpressions(pipe.Operator, globals, entities))
                    return false;

                return true;
            }
            else if (query is UnionOperator union)
            {
                for (int i = 0; i < union.Expressions.Count; i++)
                {
                    var expr = union.Expressions[i].Element;
                    var entity = GetEntityExpression(expr, globals);
                    if (entity == null)
                        return false;
                    entities.Add(entity);
                }

                return true;
            }
            else
            {
                var entity = GetEntityExpression(query, globals);
                if (entity == null)
                    return false;
                entities.Add(entity);
                return true;
            }
        }
    }
}