using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Utils;

    internal class RenameActor : KustoActor
    {
        private static readonly ApplyAction RenameAction =
            CodeAction.Create("Rename", "Rename selected item");

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
            if (code.Syntax.GetTokenAt(position) is SyntaxToken token
                && token.Parent.GetFirstAncestorOrSelf<Name>() is Name name
                && service.GetRelatedElements(name.TextStart, FindRelatedOptions.None, cancellationToken) is RelatedInfo relatedInfo
                && CanRename(relatedInfo))
            {
                actions.Add(RenameAction.WithData(name.TextStart.ToString()));
            }
        }

        private static bool CanRename(RelatedInfo info)
        {
            return info.Elements.Any(e => e.Kind == RelatedElementKind.Declaration);
        }

        public override CodeActionResult ApplyAction(
            KustoCodeService service,
            KustoCode code,
            ApplyAction action,
            int caretPosition,
            CodeActionOptions options,
            CancellationToken cancellationToken)
        {
            if (action.Data.Count > 0
                && Int32.TryParse(action.Data[0], out var position))
            {
                // move caret to start of name
                // and request client to start rename
                return new CodeActionResult(
                    new MoveCaretAction(position),
                    new RenameAction());
            }

            return CodeActionResult.Failure();
        }
    }
}