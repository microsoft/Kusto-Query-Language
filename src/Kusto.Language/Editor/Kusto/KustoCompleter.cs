using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Editor
{
    using Binding;
    using Parsing;
    using Symbols;
    using Syntax;
    using System.Text;
    using Utils;
    using static KustoServiceHelpers;

    internal class KustoCompleter
    {
        private readonly KustoCode _code;
        private readonly CompletionOptions _options;
        private readonly CancellationToken _cancellationToken;

        public KustoCompleter(
            KustoCode code,
            CompletionOptions options, 
            CancellationToken cancellationToken)
        {
            _code = code;
            _options = options;
            _cancellationToken = cancellationToken;
        }

        #region Completion Builder

        private class CompletionBuilder
        {
            private readonly List<CompletionItem> list = new List<CompletionItem>();
            private readonly Dictionary<string, int> indexMap = new Dictionary<string, int>();

            public CompletionBuilder()
            {
            }

            public int Count => list.Count;


            private IReadOnlyList<CompletionItem> _frozenList;

            public IReadOnlyList<CompletionItem> Items
            {
                get
                {
                    if (_frozenList == null)
                    {
                        var tmp = list.ToReadOnly();
                        Interlocked.CompareExchange(ref _frozenList, tmp, null);
                    }

                    return _frozenList;
                }
            }

            public void Add(CompletionItem item)
            {
                _frozenList = null;

                if (indexMap.TryGetValue(item.DisplayText, out var existingItemIndex))
                {
                    var better = GetBetterItem(list[existingItemIndex], item);
                    if (better == item)
                    {
                        list[existingItemIndex] = item;
                    }
                }
                else
                {
                    indexMap.Add(item.DisplayText, list.Count);
                    list.Add(item);
                }
            }

            public void AddRange(IEnumerable<CompletionItem> items)
            {
                foreach (var item in items)
                {
                    Add(item);
                }
            }

            private static CompletionItem GetBetterItem(CompletionItem existing, CompletionItem other)
            {
                // promote query operators over keywords
                if (existing.Kind == CompletionKind.Keyword && other.Kind == CompletionKind.QueryPrefix)
                    return other;

                return existing;
            }
        }

        #endregion

        #region Public API

        public CompletionInfo GetCompletionItems(int position)
        {
            GetEditRange(position, out var editStart, out var editLength);

            if (!ShouldComplete(position))
            {
                return new CompletionInfo(EmptyReadOnlyList<CompletionItem>.Instance, editStart, editLength);
            }

            var builder = new CompletionBuilder();
            var mode = CompletionMode.Combined;

            // use editStart instead of actual cursor position to adjust for token affinity
            if (_options.IncludeSymbols)
            {
                mode = GetSymbolCompletions(editStart, builder);
            }

            if (mode == CompletionMode.Combined && _options.IncludeSyntax)
            {
                GetSyntaxCompletions(editStart, builder);
            }

            var items = builder.Items
                .Select(it => Retriggered(it))
                .ToList();

            // order completions by rank, priority & text
            var orderedItems = items
                 .Select(i => i.WithOrderText(CreateOrderText(i)))
                 .OrderBy(i => i.OrderText)
                 .ToArray();

            return new CompletionInfo(orderedItems, editStart, editLength);
        }

        private string CreateOrderText(CompletionItem item)
        {
            return
                ((int)GetOrderingRank(item)).ToString("D2")
                + ((int)item.Priority).ToString("D1")
                + "_"
                + item.OrderText.ToLower(); // existing order text should not have rank & priority already
        }

        /// <summary>
        /// Enable retrigger of completion automatically from text before caret after insertion
        /// </summary>
        private CompletionItem Retriggered(CompletionItem item)
        {
            if (!item.Retrigger)
            {
                var prevChar = item.BeforeText.Length > 0
                    ? item.BeforeText[item.BeforeText.Length - 1]
                    : '\0';

                if (char.IsWhiteSpace(prevChar) || prevChar == '(')
                {
                    item = item.WithRetrigger(true);
                }
            }

            return item;
        }

        /// <summary>
        /// Gets the text range that will be replaced by a selected completion item
        /// </summary>
        private void GetEditRange(int position, out int editStart, out int editLength)
        {
            var affinityToken = GetTokenWithAffinity(_code, position);

            if (affinityToken != null)
            {
                // check for possible partially typed token with internal dashes
                // that currently parse as multiple tokens
                var curr = affinityToken;

                // back up to start of adjacent dash-joined tokens
                var prev = curr.GetPreviousToken();
                while (prev != null
                    && IsKeywordOrIdentifierOrDash(prev.Kind)
                    && IsKeywordOrIdentifierOrDash(curr.Kind)
                    && prev.End == curr.TextStart)
                {
                    curr = prev;
                    prev = curr.GetPreviousToken();
                }

                var first = curr;

                // find last token of adjacent dashed-join tokens
                var next = curr.GetNextToken();
                StringBuilder builder = null;
                while (next != null
                    && IsKeywordOrIdentifierOrDash(next.Kind)
                    && IsKeywordOrIdentifierOrDash(curr.Kind)
                    && curr.End == next.TextStart)
                {
                    if (builder == null)
                        builder = new StringBuilder();
                    builder.Append(curr.Text);
                    curr = next;
                    next = curr.GetNextToken();
                }

                var last = curr;

                // use the full dashed sequence as the edit range if
                // it is the start of any known name with dashes.
                if (first != last 
                    && builder != null
                    && IsStartOfKnownNameWithDashes(builder.ToString()))
                {
                    editStart = first.TextStart;
                    editLength = last.End - editStart;
                }
                else
                {
                    editStart = first.TextStart;
                    editLength = first.Width;
                }
            }
            else
            {
                editStart = position;
                editLength = 0;
            }
        }

        private static bool IsKeywordOrIdentifierOrDash(SyntaxKind kind) =>
            kind == SyntaxKind.IdentifierToken
            || kind == SyntaxKind.MinusToken
            || kind.GetCategory() == SyntaxCategory.Keyword;

        private static IReadOnlyList<string> s_namesWithDashes =
            SyntaxFacts.GetKindsWithFixedText().Select(k => k.GetText()).Where(t => t.Contains("-")).ToList();

        private static bool IsStartOfKnownNameWithDashes(string text)
        {
            return s_namesWithDashes.Any(n => n.StartsWith(text));
        }

        /// <summary>
        /// Determines if a completion list should be shown automatically during typing.
        /// </summary>
        /// <param name="position">The text position of the caret.</param>
        /// <param name="key">The last key typed.</param>
        public bool ShouldAutoComplete(int position, char key)
        {
            if (key == '\0')
                return false;

            // don't auto complete when we know we won't produce completions
            if (!ShouldComplete(position))
                return false;

            // don't auto complete when cursor is just being moved around
            if (char.IsControl(key) && key != '\b')
                return false;

            // don't auto complete just because a new line was added
            if (key == '\r' || key == '\n')
                return false;

            var token = GetTokenLeftOfPosition(_code, position);
            if (token != null)
            {
                // insert whitespace immediately after token?
                if (char.IsWhiteSpace(key) && position == token.End + 1)
                {
                    // punctuation that usually has expressions following
                    switch (token.Kind)
                    {
                        case SyntaxKind.OpenParenToken:
                        case SyntaxKind.OpenBracketToken:
                        case SyntaxKind.OpenBraceToken:
                        case SyntaxKind.CommaToken:
                        case SyntaxKind.ColonToken:
                        case SyntaxKind.BarToken:
                        case SyntaxKind.EqualToken:
                        case SyntaxKind.FatArrowToken:
                        case SyntaxKind.CloseParenToken: // some clauses end in ) but there is more to go
                        case SyntaxKind.DashDashToken:
                        case SyntaxKind.DashDashGreaterThanToken:
                        case SyntaxKind.LessThanDashDashToken:
                            return true;
                    }

                    // can have syntax or expressions following
                    switch (token.Kind.GetCategory())
                    {
                        case SyntaxCategory.Identifier:
                        case SyntaxCategory.Operator:
                        case SyntaxCategory.Keyword:
                            return true;
                    }

                    // after a complete expression
                    var expr = GetCompleteExpressionLeftOfPosition(_code, position);
                    if (expr != null)
                        return true;
                }

                // at ending edge of token
                else if (token.End == position)
                {
                    // auto complete if backspacing into ending edge of identifier
                    if (key == '\b')
                    {
                        return token.Kind.GetCategory() == SyntaxCategory.Identifier;
                    }

                    // inserting punctuation that usually has immediate followers?
                    switch (token.Kind)
                    {
                        case SyntaxKind.OpenParenToken:
                        case SyntaxKind.OpenBracketToken:
                        case SyntaxKind.OpenBraceToken:
                        case SyntaxKind.DotToken:
                        case SyntaxKind.ColonToken:
                        case SyntaxKind.EqualToken:
                            return true;

                        // could be leading part of some keywords
                        case SyntaxKind.BangToken:
                            return true;
                    }

                    // just typed leading part of one of these tokens
                    switch (token.Kind.GetCategory())
                    {
                        case SyntaxCategory.Identifier:
                        case SyntaxCategory.Operator:
                        case SyntaxCategory.Keyword:
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if completion should be shown at the specified text position.
        /// </summary>
        private bool ShouldComplete(int position)
        {
            var token = _code.Syntax.GetTokenAt(position);
            var previous = token.GetPreviousToken();
            var affinity = GetTokenWithAffinity(_code, position) ?? token;

            var leftOrSurrounding = (position == token.TriviaStart)
                ? (previous ?? token) : token;

            // don't show completions if position is inside a literal or a directive
            if (IsInsideLiteral(leftOrSurrounding, position)
                || IsInsideDirective(leftOrSurrounding, position))
            {
                return false;
            }

            // inside trivia?
            if (position <= affinity.TextStart && affinity.Trivia.Length > 0)
            {
                // don't allow completions if the position is within a comment
                return !IsInsideComment(token.Trivia, position - token.TriviaStart);
            }

            return true;
        }

        /// <summary>
        /// True if the trivia offset is within a comment
        /// </summary>
        public static bool IsInsideComment(string trivia, int triviaOffset)
        {
            if (TriviaFacts.TryGetCommentSpan(trivia, triviaOffset, out var start, out var length))
            {
                // the carat is before the comment, so allow completions
                if (triviaOffset == start)
                    return false;

                if (triviaOffset == start + length && TextFacts.HasLineBreaks(trivia))
                {
                    // after the line break so allow completions
                    return false;
                }

                return true;
            }

            return false;
        }

        private static bool IsInsideLiteral(SyntaxToken token, int position)
        {
            return token.Kind.GetCategory() == SyntaxCategory.Literal
                && (position >= token.TextStart && position <= token.End);
        }

        private static bool IsInsideDirective(SyntaxToken token, int position)
        {
            return token.Kind == SyntaxKind.DirectiveToken
                && (position >= token.TextStart && position <= token.End);
        }

        private CompletionRank GetOrderingRank(CompletionItem item)
        {
            // allow for cancellation during ordering of completion items
            this._cancellationToken.ThrowIfCancellationRequested();

            if (item.Rank != CompletionRank.Default)
                return item.Rank;

            switch (item.Kind)
            {
                case CompletionKind.Example:
                    return CompletionRank.Literal;

                case CompletionKind.QueryPrefix:
                    return CompletionRank.Keyword;

                case CompletionKind.Keyword:
                    return CompletionRank.Keyword;

                case CompletionKind.AggregateFunction:
                    return CompletionRank.Aggregate;

                case CompletionKind.Column:
                    return CompletionRank.Column;

                case CompletionKind.Table:
                    return CompletionRank.Table;

                case CompletionKind.MaterialiedView:
                case CompletionKind.EntityGroup:
                case CompletionKind.Graph:
                case CompletionKind.StoredQueryResult:
                    return CompletionRank.Entity;

                case CompletionKind.Variable:
                case CompletionKind.Parameter:
                    if (item.DisplayText == "$left" || item.DisplayText == "$right")
                    {
                        return CompletionRank.Literal;
                    }
                    return CompletionRank.Variable;

                case CompletionKind.BuiltInFunction:
                case CompletionKind.LocalFunction:
                case CompletionKind.DatabaseFunction:
                    return CompletionRank.Function;

                case CompletionKind.ScalarInfix:
                    if (TextFacts.IsLetterOrDigit(item.DisplayText[0]))
                    {
                        return CompletionRank.StringOperator;
                    }
                    return CompletionRank.MathOperator;

                case CompletionKind.ScalarPrefix:
                case CompletionKind.TabularPrefix:
                case CompletionKind.TabularSuffix:
                case CompletionKind.Identifier:
                case CompletionKind.Cluster:
                case CompletionKind.Database:
                case CompletionKind.Punctuation:
                case CompletionKind.Syntax:
                case CompletionKind.Unknown:
                case CompletionKind.RenderChart:
                default:
                    return CompletionRank.Other;
            }
        }

        #endregion

        #region Symbol Completions

        private CompletionMode GetSymbolCompletions(int position, CompletionBuilder builder)
        {
            CompletionHint hint = CompletionHint.None;

            if (TryGetCompletionContext(position, out var contextNode, out var contextChildIndex))
            {
                var mode = GetSpecialCaseCompletions(position, contextNode, contextChildIndex, builder);
                if (mode == CompletionMode.Isolated)
                    return mode;

                hint = GetCompletionHint(position, contextNode, contextChildIndex);
            }

            var symbols = new List<Symbol>();

            var match = GetSymbolMatch(position);

            if (_options.IncludeFunctions == IncludeFunctionKind.None)
            {
                match &= ~SymbolMatch.Function;
            }

            var isCommand = IsInCommand(contextNode, contextChildIndex);

            var include = _options.IncludeFunctions;

            if (isCommand)
            {
                // commands only reference database functions
                include &= IncludeFunctionKind.DatabaseFunctions;
            }

            Binder.GetSymbolsInScope(this._code.Tree, position, this._code.Globals, match, include, symbols, this._cancellationToken);

            var isInvoke = IsInvokeFunctionContext(contextNode);
            var rowScope = (isInvoke)
                ? Binder.GetRowScope(this._code.Tree, position, this._code.Globals, this._cancellationToken)
                : null;

            var symbolItems = new List<CompletionItem>();

            foreach (var symbol in symbols)
            {
                // don't show completion for hidden symbols
                if (symbol.IsHidden)
                    continue;

                // don't show invoke functions that would not match the row schema
                if (isInvoke && rowScope != null && !rowScope.IsOpen)
                {
                    if (symbol is FunctionSymbol fs && !IsInvokeApplicable(fs, rowScope))
                        continue;
                    if (symbol is VariableSymbol vs && vs.Type is FunctionSymbol vfs && !IsInvokeApplicable(vfs, rowScope))
                        continue;
                }

                symbolItems.Clear();
                GetCompletionItemsForSymbol(symbol, contextNode, nameOnly: isCommand, items: symbolItems);

                foreach (var item in symbolItems)
                {
                    if (ShouldAddSpaceToCompletionItem(symbol, hint))
                    {
                        builder.Add(AddSpaceToCompletionItem(item));
                    }
                    else
                    {
                        builder.Add(item);
                    }
                }
            }

            return CompletionMode.Combined;
        }

        private bool IsInCommand(SyntaxElement element, int contextChildIndex)
        {
            if (element == null)
                return false;

            if (contextChildIndex > 0
                && element.GetChild(contextChildIndex - 1) is SyntaxToken e
                && e.Text == "<|")
            {
                return false;
            }

            // actually inside a function body, even if it is part of a create command this is not part of the command proper.
            var body = element.GetFirstAncestorOrSelf<FunctionBody>();
            if (body != null)
                return false;

            var statementList = element.GetFirstAncestorOrSelf<SyntaxList<SeparatedElement<Statement>>>();
            var command = element.GetFirstAncestorOrSelf<Command>();

            if (command != null)
            {
                if (statementList != null && command.IsAncestorOf(statementList))
                {
                    // statements inside commands are part of input pipe and are not considered part of the command for completion
                    return false;
                }
                else
                {
                    // this is truly part of a command
                    return true;
                }
            }

            return false;
        }

        private bool IsQueryPart(SyntaxElement element, int contextChildIndex)
        {
            if (element.Root is QueryBlock)
                return true;

            if (element.Root is CommandBlock)
            {
                // right of <| or | is query
                if (contextChildIndex > 0
                    && element.GetChild(contextChildIndex - 1) is SyntaxToken e
                    && (e.Text == "<|" || e.Text == "|"))
                {
                    return true;
                }

                // part of a function body
                if (element.GetFirstAncestorOrSelf<FunctionBody>() != null)
                    return true;
            }

            return false;
        }

        private bool IsInvokeApplicable(FunctionSymbol function, Symbol implicitFirstArgumentType)
        {
            if (function == Functions.Cluster
                || function == Functions.Database)
            {
                return true;
            }

            foreach (var sig in function.Signatures)
            {
                if (sig.Parameters.Count == 0)
                    continue;

                var p = sig.Parameters[0];
                switch (p.TypeKind)
                {
                    case ParameterTypeKind.Declared:
                        foreach (var t in p.DeclaredTypes)
                        {
                            if (t is TableSymbol ts && implicitFirstArgumentType.IsAssignableTo(t))
                                return true;
                        }
                        break;

                    case ParameterTypeKind.Tabular:
                        return implicitFirstArgumentType is TableSymbol;
                }
            }

            return false;
        }

        private bool ShouldAddSpaceToCompletionItem(Symbol symbol, CompletionHint hint)
        {
            // only append space if the symbol is in a boolean context and is not itself boolean
            return (hint & CompletionHint.Boolean) != 0
                && Symbol.GetResultType(symbol) != ScalarTypes.Bool;
        }

        private static bool IsStartOfQuery(SyntaxNode context)
        {
            while (context is Name || context is NameReference || context is ExpressionStatement)
            {
                context = context.Parent;
            }

            return context is QueryBlock
                || context is SyntaxList<SeparatedElement<Statement>>
                || context is SeparatedElement<Statement>;
        }

        private static readonly string AfterQueryStart = "\n| ";

        private void GetCompletionItemsForSymbol(Symbol symbol, SyntaxNode contextNode, bool nameOnly, List<CompletionItem> items)
        {
            var kind = GetCompletionKind(symbol);
            string editName = KustoFacts.BracketNameIfNecessary(symbol.Name, _code.Dialect);
            var displayText = CompletionDisplay.GetText(symbol, nameOnly);

            switch (symbol)
            {
                case TableSymbol t:
                    var addExternalTableFuncText = !nameOnly && t.IsExternal;

                    // editName gets bracketted: ['name with blanks']
                    // when the name has characters that cannot be represented in a variable name.
                    // This problem doesn't exist in external tables case as their name is put into a string anyway.
                    var insertionText =
                        addExternalTableFuncText
                        ? $"external_table('{t.Name}')"
                        : editName;

                    // Lower ordering priority for external tables so that
                    // “regular” tables are displayed first and then the external ones.
                    var priority = t.IsExternal ? CompletionPriority.Low : CompletionPriority.Normal;

                    // Enables the user to be able to just type the name of the external table
                    // and not the entire expression (including the external_table('...') part)
                    var matchText = t.Name;

                    if (IsStartOfQuery(contextNode))
                    {
                        items.Add(
                            new CompletionItem(kind, displayText, beforeText: insertionText + AfterQueryStart, matchText: matchText, priority: priority));
                    }
                    else
                    {
                        items.Add(
                            new CompletionItem(kind, displayText, beforeText: insertionText, matchText: matchText, priority: priority));
                    }
                    break;

                case FunctionSymbol f:
                    if (nameOnly)
                    {
                        items.Add(new CompletionItem(kind, displayText, beforeText: editName, matchText: displayText));
                        return;
                    }

                    var builtIn = this._code.Globals.IsBuiltInFunction(f);
                    if (builtIn)
                    {
                        // built-in functions don't need to be escaped even if they are keywords
                        editName = f.Name;
                    }

                    if (f.Signatures
                         .Where(s => !s.IsHidden && !s.IsObsolete)
                         .Max(s => s.Parameters.Count) == 0)
                    {
                        // function does not have parameters (does not count obsolete forms)
                        if (builtIn)
                        {
                            editName = editName + "()";
                        }

                        if (IsStartOfQuery(contextNode))
                        {
                            items.Add(new CompletionItem(kind, displayText, editName + AfterQueryStart, matchText: f.Name));
                        }
                        else
                        {
                            items.Add(new CompletionItem(kind, displayText, editName, matchText: f.Name));
                        }
                    }
                    else
                    {
                        // function has parameters
                        var isInvoke = IsInvokeFunctionContext(contextNode);

                        if (this._options.EnableParameterInjection && f.MaxArgumentCount == 1 && !builtIn && !isInvoke)
                        {
                            items.Add(new CompletionItem(kind, displayText, editName + "({parameter})", matchText: f.Name));
                        }
                        else
                        {
                            items.Add(new CompletionItem(kind, displayText, editName + "(", ")", matchText: f.Name));
                        }
                    }
                    break;

                case PatternSymbol p:
                    items.Add(new CompletionItem(kind, displayText, editName + "(", ")", matchText: p.Name));
                    break;

                case VariableSymbol v:
                    if (v.Type is FunctionSymbol)
                    {
                        GetCompletionItemsForSymbol(v.Type, contextNode, nameOnly, items);
                    }
                    else if (v.Type is TableSymbol && IsStartOfQuery(contextNode))
                    {
                        items.Add(new CompletionItem(kind, displayText, editName + AfterQueryStart));
                    }
                    else
                    {
                        items.Add(new CompletionItem(kind, displayText, editName));
                    }
                    break;

                case ParameterSymbol p:
                    if (p.Type is FunctionSymbol)
                    {
                        GetCompletionItemsForSymbol(p.Type, contextNode, nameOnly, items);
                    }
                    else
                    {
                        items.Add(new CompletionItem(kind, displayText, editName));
                    }
                    break;

                case DatabaseSymbol d:
                    items.Add(new CompletionItem(CompletionKind.Database, displayText, editName));
                    if (!string.IsNullOrEmpty(d.AlternateName))
                    {
                        string altEditName = KustoFacts.BracketNameIfNecessary(symbol.AlternateName, _code.Dialect);
                        items.Add(new CompletionItem(CompletionKind.Database, d.AlternateName, altEditName));
                    }
                    break;

                case ClusterSymbol cl:
                    items.Add(new CompletionItem(CompletionKind.Cluster, displayText, KustoFacts.GetBracketedName(cl.Name)));
                    break;

                case OptionSymbol opt:
                    items.Add(new CompletionItem(CompletionKind.Option, opt.Name, editName));
                    break;

                default:
                    items.Add(new CompletionItem(kind, displayText, editName));
                    break;
            }
        }

        private static bool IsInvokeFunctionContext(SyntaxNode node)
        {
            // check to see if an ancestor is the invoke operator and not within an existing invoke function argument
            var op = node?.GetFirstAncestorOrSelf<QueryOperator>();
            var fc = node?.GetFirstAncestorOrSelf<FunctionCallExpression>();
            return op is InvokeOperator && (fc == null || fc.TextStart < op.TextStart);
        }

        private SymbolMatch GetSymbolMatch(int position)
        {
            var match = GetSymbolMatchFromGrammar(position);
            if (match != SymbolMatch.None)
            {
                // grammar based completion hints only exist to make matching more specific than what syntax tree can provide,
                // so grammar hints wins over syntax hints (as opposed to unioning them together)
                return match;
            }

            return GetSymbolMatchFromSyntaxTree(position);
        }

        private SymbolMatch GetSymbolMatchFromGrammar(int position)
        {
            var match = SymbolMatch.None;

            var annotations = GetGrammarAnnotations(position);

            foreach (var hint in annotations.OfType<CompletionHint>())
            {
                match |= GetSymbolMatch(hint);
            }

            return match;
        }

        private SymbolMatch GetSymbolMatchFromSyntaxTree(int position)
        {
            SymbolMatch match = SymbolMatch.None;

            if (TryGetCompletionContext(position, out var contextNode, out var index))
            {
                var hint = GetCompletionHint(position, contextNode, index);
                match |= GetSymbolMatch(hint);
            }

            // special case for parenthesis; get hint from outside
            while (contextNode is ParenthesizedExpression && contextNode.Parent != null)
            {
                var hint = GetCompletionHint(position, contextNode.Parent, contextNode.IndexInParent);
                match |= GetSymbolMatch(hint);
                contextNode = contextNode.Parent;
            }

            return match;
        }

        /// <summary>
        /// True if the node only contains one descendant <see cref="SyntaxToken"/> 
        /// </summary>
        private static bool ContainsOnlyOneToken(SyntaxNode node)
        {
            while (node != null && node.ChildCount == 1)
            {
                var child = node.GetChild(0);
                if (child is SyntaxToken)
                    return true;
                node = child as SyntaxNode;
            }

            return false;
        }

        private bool TryGetFunctionOrOperatorArgument(
            SyntaxNode contextNode, int childIndex,
            out IReadOnlyList<Signature> signatures,
            out List<Expression> arguments,
            out int argumentIndex, out int argumentCount, out string parameterName)
        {
            parameterName = null;

            var n = contextNode;

            if (n is NamedExpression)
            {
                if (n is SimpleNamedExpression sn)
                {
                    parameterName = sn.Name.SimpleName;
                }

                childIndex = n.Parent?.GetChildIndex(n) ?? 0;
                n = n.Parent;
            }

            // if this is a primitive expression that is a list element, move context up
            while (ContainsOnlyOneToken(n) && !(n.Parent is SyntaxList))
            {
                childIndex = n.Parent?.GetChildIndex(n) ?? 0;
                n = n.Parent;
            }

            // if this is a list element (not separator), move context up to list
            if (n is SeparatedElement && childIndex == 0)
            {
                childIndex = n.Parent?.GetChildIndex(n) ?? 0;
                n = n.Parent;
            }

            FunctionCallExpression functionCall = null;

            if (n is SyntaxList
                && n.Parent is ExpressionList el
                && el.Parent is FunctionCallExpression fc1
                && fc1.ReferencedSymbol is FunctionSymbol fs)
            {
                signatures = fs.Signatures;
                functionCall = fc1;
                arguments = fc1.ArgumentList.Expressions.Select(e => e.Element).ToList();
                argumentIndex = childIndex;
                argumentCount = n.ChildCount;
            }
            else if (n is ExpressionList
                && n.Parent is FunctionCallExpression fc2
                && fc2.ReferencedSymbol is FunctionSymbol fs2)
            {
                // this happens for the first argument
                signatures = fs2.Signatures;
                functionCall = fc2;
                arguments = fc2.ArgumentList.Expressions.Select(e => e.Element).ToList();
                argumentIndex = 0;
                argumentCount = 1;
            }
            else if (n is BinaryExpression be
                && be.ReferencedSymbol is OperatorSymbol os)
            {
                signatures = os.Signatures;
                arguments = new List<Expression>(2) { be.Left, be.Right };
                argumentIndex = childIndex == 0 ? 0 : 1;
                argumentCount = 2;
                return true;
            }
            else
            {
                signatures = null;
                arguments = null;
                argumentIndex = 0;
                argumentCount = 0;
                return false;
            }

            if (functionCall != null && arguments != null && IsInvokeFunctionContext(functionCall.Parent))
            {
                // add implicit argument
                arguments.Insert(0, null);
                argumentIndex++;
                argumentCount++;
            }

            if (signatures != null && !signatures.Any(s => s.AllowsNamedArguments))
            {
                parameterName = null;
            }

            return true;
        }

        private enum CompletionMode
        {
            /// <summary>
            /// The completions should be shown alone
            /// </summary>
            Isolated,

            /// <summary>
            /// The completions should be shown with other completions
            /// </summary>
            Combined
        }

        private CompletionMode GetSpecialCaseCompletions(int position, SyntaxNode contextNode, int childIndex, CompletionBuilder builder)
        {
            var mode = GetFunctionArgumentCompletions(position, contextNode, childIndex, builder);
            if (mode == CompletionMode.Isolated)
                return mode;

            mode = GetSetOptionCompletions(position, contextNode, childIndex, builder);
            if (mode == CompletionMode.Isolated)
                return mode;

            mode = GetColumnExampleCompletions(position, contextNode, childIndex, builder);
            if (mode == CompletionMode.Isolated)
                return mode;

            mode = GetDeconstructCompletions(position, contextNode, childIndex, builder);

            return mode;
        }

        private CompletionMode GetDeconstructCompletions(int position, SyntaxNode contextNode, int childIndex, CompletionBuilder builder)
         {
            // after insertion of open paren in front of existing function call that returns a tuple,
            // offer to complete a rename deconstruction
            var pe = contextNode.GetFirstAncestorOrSelf<ParenthesizedExpression>();
            if (pe != null
                && !pe.OpenParen.IsMissing
                && !pe.Expression.IsMissing
                && pe.CloseParen.IsMissing
                && position == pe.OpenParen.End
                && GetUnpipedExpression(pe.Expression) is FunctionCallExpression fc
                && !(fc.Parent is NamedExpression)
                && !(fc.Parent is LetStatement) // maybe future
                && fc.ResultType is TupleSymbol ts
                && ts.Columns.Count > 1)
            {
                var displayText = "(" + ts.Columns[0].Name + ", ...) = ";

                var applyTextWithMarkers = string.Join(", ", ts.Columns.Select(c => $"[[{c.Name}]]")) + ") = ";

                var matchText = fc.Name.SimpleName;

                var item = new CompletionItem(CompletionKind.Example, displayText, matchText: matchText, priority: CompletionPriority.Top)
                    .WithApplyTexts(applyTextWithMarkers);

                builder.Add(item);
            }

            return CompletionMode.Combined;
        }

        private static Expression GetUnpipedExpression(Expression exp)
        {
            while (exp is PipeExpression pe)
            {
                exp = pe.Expression;
            }

            return exp;
        }

        private CompletionMode GetColumnExampleCompletions(int position, SyntaxNode contextNode, int childIndex, CompletionBuilder builder)
        {
            if (contextNode is BinaryExpression be 
                && IsEqualityExpression(contextNode.Kind)
                && childIndex == 2
                && be.Left.ReferencedSymbol is ColumnSymbol col
                && col.Examples.Count > 0)
            {
                AddColumnExamples(builder, col);
            }
            else
            {
                if (contextNode is SyntaxList<SeparatedElement<Expression>> list)
                {
                    contextNode = list.Parent;
                }

                if (contextNode is ExpressionList exprList
                    && exprList.Parent is InExpression inOp
                    && inOp.Left.ReferencedSymbol is ColumnSymbol inCol
                    && inCol.Examples.Count > 0)
                {
                    AddColumnExamples(builder, inCol);
                }
            }

            // TODO: check for in operator too
            return CompletionMode.Combined;
        }

        private static bool IsEqualityExpression(SyntaxKind kind) =>
            kind == SyntaxKind.EqualExpression
            || kind == SyntaxKind.EqualTildeExpression
            || kind == SyntaxKind.NotEqualExpression;

        private static void AddColumnExamples(CompletionBuilder builder, ColumnSymbol column)
        {
            foreach (var example in column.Examples)
            {
                builder.Add(CreateColumnExampleCompletion(example, column.Type));
            }
        }

        private static CompletionItem CreateColumnExampleCompletion(string example, TypeSymbol type)
        {
            var displayText = example;
            var editText = example;
            var matchText = example;

            // fix example is it was not supplied as a quoted string literal
            if (type == ScalarTypes.String)
            {
                if (TokenParser.ScanStringLiteral(example) == example.Length)
                {
                    displayText = matchText = KustoFacts.GetStringLiteralValue(example);
                }
                else
                {
                    editText = KustoFacts.GetStringLiteral(example);
                }
            }
            else if (type == ScalarTypes.Bool)
            {
                if (example.StartsWith("bool("))
                {
                    displayText = matchText = GetGooLiteralContent(example);

                    if (TokenParser.ScanBooleanLiteral(displayText) == displayText.Length)
                    {
                        editText = displayText;
                    }
                }
                else if (TokenParser.ScanBooleanLiteral(example) != example.Length)
                {
                    editText = "bool(" + example + ")";
                }
            }
            else if (type == ScalarTypes.Int)
            {
                if (example.StartsWith("int("))
                {
                    displayText = matchText = GetGooLiteralContent(example);
                }
                else
                {
                    editText = "int(" + example + ")";
                }
            }
            else if (type == ScalarTypes.Long)
            {
                if (example.StartsWith("long("))
                {
                    displayText = matchText = GetGooLiteralContent(example);

                    if (TokenParser.ScanLongLiteral(displayText) == displayText.Length)
                    {
                        editText = displayText;
                    }
                }
                else if (TokenParser.ScanLongLiteral(example) != example.Length)
                {
                    editText = "long(" + example + ")";
                }
            }
            else if (type == ScalarTypes.Real)
            {
                if (example.StartsWith("real(") || example.StartsWith("double("))
                {
                    displayText = matchText = GetGooLiteralContent(example);

                    if (TokenParser.ScanRealLiteral(displayText) == displayText.Length)
                    {
                        editText = displayText;
                    }
                }
                else if (TokenParser.ScanRealLiteral(example) != example.Length)
                {
                    editText = "real(" + example + ")";
                }
            }
            else if (type == ScalarTypes.Decimal)
            {
                if (example.StartsWith("decimal("))
                {
                    displayText = matchText = GetGooLiteralContent(example);
                }
                else
                {
                    editText = "decimal(" + example + ")";
                }
            }
            else if (type == ScalarTypes.TimeSpan)
            {
                if (example.StartsWith("timespan(") || example.StartsWith("time("))
                {
                    displayText = matchText = GetGooLiteralContent(example);

                    if (TokenParser.ScanTimespanLiteral(displayText) == displayText.Length)
                    {
                        editText = displayText;
                    }
                }
                else if (TokenParser.ScanTimespanLiteral(example) != example.Length)
                {
                    editText = "timespan(" + example + ")";
                }
            }
            else if (type == ScalarTypes.DateTime)
            {
                if (example.StartsWith("datetime(") || example.StartsWith("date("))
                {
                    displayText = matchText = GetGooLiteralContent(example);
                }
                else
                {
                    editText = "datetime(" + example + ")";
                }
            }
            else if (type == ScalarTypes.Guid)
            {
                if (example.StartsWith("guid("))
                {
                    displayText = matchText = GetGooLiteralContent(example);
                }
                else
                {
                    editText = "guid(" + example + ")";
                }
            }
            else if (type is DynamicSymbol)
            {
                if (example.StartsWith("dynamic("))
                {
                    displayText = matchText = GetGooLiteralContent(example);
                }
                else
                {
                    editText = "dynamic(" + example + ")";
                }
            }

            return new CompletionItem(CompletionKind.Example, displayText, editText, matchText: matchText);
        }

        private static string GetGooLiteralContent(string literal)
        {
            var openParenIndex = literal.IndexOf('(');
            var closeParenIndex = literal.LastIndexOf(')');

            if (openParenIndex >= 0 && closeParenIndex >= 0)
            {
                return literal.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1);
            }
            else
            {
                return literal;
            }
        }

        private CompletionMode GetSetOptionCompletions(int position, SyntaxNode contextNode, int childIndex, CompletionBuilder builder)
        {
            if (contextNode is OptionValueClause clause 
                && childIndex == 1
                && clause.Parent is SetOptionStatement statement
                && statement.Name.ReferencedSymbol is OptionSymbol option
                && option.Examples != null)
            {
                foreach (var example in option.Examples)
                {
                    var value = KustoFacts.GetStringLiteralValue(example);
                    builder.Add(new CompletionItem(CompletionKind.Example, example, example, matchText: value));
                }

                return CompletionMode.Isolated;
            }

            return CompletionMode.Combined;
        }

        private CompletionMode GetFunctionArgumentCompletions(int position, SyntaxNode contextNode, int childIndex, CompletionBuilder builder)
        {
            if (TryGetFunctionOrOperatorArgument(contextNode, childIndex, out var signatures, out var arguments, out var argumentIndex, out var argumentCount, out var name))
            {
                // argument names
                if (!(contextNode is NamedExpression) && signatures[0].Symbol is FunctionSymbol fs && ShowParameterNames(fs))
                {
                    var suggestions = GetArgumentNameSuggestions(signatures, arguments, argumentIndex);

                    foreach (var s in suggestions)
                    {
                        builder.Add(new CompletionItem(CompletionKind.Parameter, s + "=", s + "=", matchText: s));
                    }

                    if (suggestions.Count > 0 && IsArgumentNameRequired(signatures, arguments, argumentIndex))
                    {
                        return CompletionMode.Isolated;
                    }
                }

                var examples = new HashSet<string>();
                var possibleParameters = new List<Parameter>();
                var knownParameter = false;

                foreach (var sig in signatures)
                {
                    // check for examples based off of ReturnTypeKind.Parameter0XXX
                    if (TryGetReturnTypeKindCompletions(sig.ReturnKind, position, contextNode, argumentIndex, builder))
                    {
                        return CompletionMode.Isolated;
                    }

                    // check for examples based on parameters
                    possibleParameters.Clear();
                    GetPossibleParameters(sig, arguments, name, argumentIndex, possibleParameters);
                    knownParameter |= possibleParameters.Count > 0;
                    GetParameterExamples(sig, arguments, possibleParameters, examples);
                }

                // if we got through all the signature and did not match a known parameter
                // then return no results and 'isolated' so there will be no completions
                if (!knownParameter)
                {
                    return CompletionMode.Isolated;
                }

                if (examples != null)
                {
                    foreach (var x in examples)
                    {
                        builder.Add(new CompletionItem(CompletionKind.Example, x));
                    }
                }

                // if only literal values can be specified, then do not allow other completions to show
                if (possibleParameters.All(p => p.ArgumentKind == ArgumentKind.Literal || p.ArgumentKind == ArgumentKind.LiteralNotEmpty))
                {
                    return CompletionMode.Isolated;
                }
            }

            return CompletionMode.Combined;
        }

        private static void GetParameterExamples(Signature signature, IReadOnlyList<Expression> arguments, IReadOnlyList<Parameter> possibleParameters, HashSet<string> examples)
        {
            foreach (var p in possibleParameters)
            {
                if (p.Examples.Count > 0)
                {
                    foreach (var ex in p.Examples)
                    {
                        examples.Add(ex);
                    }
                }
                else if (p.Values.Count > 0)
                {
                    foreach (var ex in p.Values.Select(v => CreateValueExamples(p, v)))
                    {
                        examples.Add(ex);
                    }
                }

                if (signature.Symbol is FunctionSymbol)
                {
                    if (p.TypeKind == ParameterTypeKind.Declared)
                    {
                        foreach (var type in p.DeclaredTypes)
                        {
                            GetTypeExamples(type, examples);
                        }
                    }
                }
                else if (signature.Symbol is OperatorSymbol op)
                {
                    GetOperatorExamples(op, arguments, possibleParameters, examples);
                }
            }
        }

        private static void GetTypeExamples(TypeSymbol type, HashSet<string> examples)
        {
            if (type == ScalarTypes.Bool)
            {
                examples.Add("false");
                examples.Add("true");
            }
        }

        private static string CreateValueExamples(Parameter p, object value)
        {
            switch (value)
            {
                case string s:
                    // convert example to lower case (if not case sensitive) to improve matching during completion
                    return KustoFacts.GetSingleQuotedStringLiteral(p.IsCaseSensitive ? s : s.ToLower());
                case DateTime dt:
                    return $"datetime({dt})";
                case TimeSpan ts:
                    return $"timespan({ts})";
                case Guid g:
                    return $"guid({g})";
                case Decimal d:
                    return $"decimal({d})";
                default:
                    return value.ToString();
            }
        }

        private static void GetOperatorExamples(OperatorSymbol symbol, IReadOnlyList<Expression> arguments, IReadOnlyList<Parameter> possibleParameters, HashSet<string> examples)
        {
            if (symbol == Operators.Equal
                || symbol == Operators.NotEqual
                || symbol == Operators.EqualTilde
                || symbol == Operators.GreaterThan
                || symbol == Operators.GreaterThanOrEqual
                || symbol == Operators.LessThan
                || symbol == Operators.LessThanOrEqual)
            {
                if (arguments.Count > 0)
                {
                    var type = arguments[0].ResultType;
                    GetTypeExamples(type, examples);
                }
            }
        }

        private bool TryGetReturnTypeKindCompletions(ReturnTypeKind kind, int position, SyntaxNode contextNode, int argumentIndex, CompletionBuilder builder)
        {
            var path = GetInstanceExpressionOfFunctionCall(contextNode) as Expression;

            switch (kind)
            {
                case ReturnTypeKind.Parameter0Cluster:
                    if (argumentIndex == 0)
                    {
                        // show the known cluster names
                        builder.AddRange(GetMemberNameExamples(_code.Globals.Clusters));
                        GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                        return true;
                    }
                    break;

                case ReturnTypeKind.Parameter0Database:
                    if (argumentIndex == 0)
                    {
                        // show either the dotted cluster's database names or the global cluster's database names
                        if (path?.ResultType is ClusterSymbol cc)
                        {
                            builder.AddRange(GetMemberNameExamples(cc.Databases));
                        }
                        else
                        {
                            builder.AddRange(GetMemberNameExamples(_code.Globals.Cluster.Databases));
                        }

                        GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                        return true;
                    }
                    break;

                case ReturnTypeKind.Parameter0Table:
                    if (argumentIndex == 0)
                    {
                        // show either the dotted database's table names or the global database's table names
                        if (path?.ResultType is DatabaseSymbol ds)
                        {
                            builder.AddRange(GetMemberNameExamples(ds.Tables));
                        }
                        else
                        {
                            builder.AddRange(GetMemberNameExamples(_code.Globals.Database.Tables));
                        }

                        GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                        return true;
                    }
                    break;

                case ReturnTypeKind.Parameter0ExternalTable:
                    if (argumentIndex == 0)
                    {
                        // show either the dotted database's table names or the global database's table names
                        if (path?.ResultType is DatabaseSymbol ds)
                        {
                            builder.AddRange(GetMemberNameExamples(ds.ExternalTables));
                        }
                        else
                        {
                            builder.AddRange(GetMemberNameExamples(_code.Globals.Database.ExternalTables));
                        }

                        GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                        return true;
                    }
                    break;

                case ReturnTypeKind.Parameter0MaterializedView:
                    if (argumentIndex == 0)
                    {
                        // show either the dotted database's table names or the global database's table names
                        if (path?.ResultType is DatabaseSymbol ds)
                        {
                            builder.AddRange(GetMemberNameExamples(ds.MaterializedViews));
                        }
                        else
                        {
                            builder.AddRange(GetMemberNameExamples(_code.Globals.Database.MaterializedViews));
                        }

                        GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                        return true;
                    }
                    break;

                case ReturnTypeKind.Parameter0EntityGroup:
                    if (argumentIndex == 0)
                    {
                        // show either the dotted database's entity group names or the global database's entity group names
                        if (path?.ResultType is DatabaseSymbol ds)
                        {
                            builder.AddRange(GetMemberNameExamples(ds.EntityGroups));
                        }
                        else
                        {
                            builder.AddRange(GetMemberNameExamples(_code.Globals.Database.EntityGroups));
                        }

                        GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                        return true;
                    }
                    break;

                case ReturnTypeKind.Parameter0StoredQueryResult:
                    if (argumentIndex == 0)
                    {
                        // show either the dotted database's table names or the global database's table names
                        if (path?.ResultType is DatabaseSymbol ds)
                        {
                            builder.AddRange(GetMemberNameExamples(ds.StoredQueryResults));
                        }
                        else
                        {
                            builder.AddRange(GetMemberNameExamples(_code.Globals.Database.StoredQueryResults));
                        }

                        GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                        return true;
                    }
                    break;

                case ReturnTypeKind.Parameter0Graph:
                    if (argumentIndex == 0)
                    {
                        if (path?.ResultType is DatabaseSymbol ds)
                        {
                            builder.AddRange(GetMemberNameExamples(ds.GraphModels));
                        }
                        else
                        {
                            builder.AddRange(GetMemberNameExamples(_code.Globals.Database.GraphModels));
                        }

                        GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                        return true;
                    }
                    else if (argumentIndex == 1)
                    {
                        if (contextNode is SyntaxList<SeparatedElement<Expression>> list
                            && list.Count > 0
                            && list[0].Element.ConstantValueInfo?.Value is string modelName)
                        {
                            var model = path?.ResultType is DatabaseSymbol ds
                                ? ds.GetGraphModel(modelName)
                                : _code.Globals.Database.GetGraphModel(modelName);
                            if (model != null)
                            {
                                builder.AddRange(GetMemberNameExamples(model.Snapshots));
                            }
                        }

                        GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                        return true;
                    }
                    break;
            }

            return false;
        }

        private static void GetPossibleParameters(Signature sig, IReadOnlyList<Expression> arguments, string parameterName, int argumentIndex, List<Parameter> possibleParameters)
        {
            if (parameterName != null)
            {
                var ap = sig.GetParameter(parameterName);
                if (ap != Signature.UnknownParameter)
                {
                    possibleParameters.Add(sig.GetParameter(parameterName));
                }
            }
            else if (argumentIndex < arguments.Count)
            {
                if (argumentIndex == arguments.Count - 1 && arguments[argumentIndex].IsMissing)
                {
                    var prevArgs = arguments.Take(argumentIndex).ToList();
                    sig.GetNextPossibleParameters(prevArgs, possibleParameters);
                }
                else
                {
                    var argParams = sig.GetArgumentParameters(arguments);
                    var ap = argParams[argumentIndex];
                    if (ap != Signature.UnknownParameter)
                    {
                        possibleParameters.Add(ap);
                    }
                }
            }
            else
            {
                sig.GetNextPossibleParameters(arguments, possibleParameters);
            }
        }

        /// <summary>
        /// Get's completion items for local variables in scope that are the requested type
        /// </summary>
        private void GetMatchingSymbolCompletions(SymbolMatch match, ScalarSymbol type, int position, SyntaxNode contextNode, CompletionBuilder builder)
        {
            // first get all items in scope
            var symbols = new List<Symbol>();
            Binder.GetSymbolsInScope(this._code.Tree, position, this._code.Globals, match, IncludeFunctionKind.All, symbols, this._cancellationToken);

            // get completion items for the symbols with a matching scalar type
            var symbolItems = new List<CompletionItem>();
            foreach (var symbol in symbols)
            {
                if (GetScalarType(symbol) == type)
                {
                    symbolItems.Clear();
                    GetCompletionItemsForSymbol(symbol, contextNode, nameOnly: false, items: symbolItems);

                    foreach (var item in symbolItems)
                    {
                        builder.Add(item);
                    }
                }
            }
        }

        private ScalarSymbol GetScalarType(Symbol symbol)
        {
            switch (symbol)
            {
                case ParameterSymbol ps:
                    return ps.Type as ScalarSymbol;
                case VariableSymbol vs:
                    return vs.Type as ScalarSymbol;
                case ColumnSymbol cs:
                    return cs.Type as ScalarSymbol;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the instance expression of a function call given a function call or one of its arguments.
        /// This is the expression on the left-side of a path expression dot with the function call on the right.
        /// Returns null if the function call is not a selector (right-side) of a path expression.
        /// </summary>
        private static SyntaxNode GetInstanceExpressionOfFunctionCall(SyntaxNode functionCallOrArgument)
        {
            // walk up parents to find function call, and then get expression left of dot (path)
            return functionCallOrArgument.GetFirstAncestorOrSelf<FunctionCallExpression>() is FunctionCallExpression fcall
                && fcall.Parent is PathExpression path
                ? path.Expression
                : null;
        }

        private IEnumerable<CompletionItem> GetMemberNameExamples(IEnumerable<Symbol> symbols)
        {
            var items = new List<CompletionItem>();

            foreach (var symbol in symbols)
            {
                items.Add(new CompletionItem(CompletionKind.Example, KustoFacts.GetStringLiteral(symbol.Name)));

                if (!string.IsNullOrEmpty(symbol.AlternateName))
                {
                    items.Add(new CompletionItem(CompletionKind.Example, KustoFacts.GetStringLiteral(symbol.AlternateName)));
                }
            }

            return items;
        }

        private bool ShowParameterNames(FunctionSymbol function)
        {
            return !this._code.Globals.IsBuiltInFunction(function) && function.MaxArgumentCount >= 2;
        }

        private bool IsArgumentNameRequired(IReadOnlyList<Signature> signatures, IReadOnlyList<Expression> arguments, int argumentIndex)
        {
            foreach (var sig in signatures)
            {
                if (argumentIndex < sig.MaxArgumentCount)
                {
                    if (IsArgumentNameRequired(sig, arguments, argumentIndex))
                        return true;
                }
            }

            return false;
        }

        private static readonly ObjectPool<List<Expression>> s_expressionListPool =
            new ObjectPool<List<Expression>>(() => new List<Expression>(), list => list.Clear());

        private bool IsArgumentNameRequired(Signature signature, IReadOnlyList<Expression> arguments, int argumentIndex)
        {
            if (argumentIndex > 0)
            {
                var unnamedArguments = s_expressionListPool.AllocateFromPool();
                try
                {
                    GetUnnamedArguments(arguments, unnamedArguments);
                    var argumentParameters = signature.GetArgumentParameters(arguments);
                    var unnamedArgumentParameters = signature.GetArgumentParameters(unnamedArguments);

                    for (int i = 0; i < argumentIndex; i++)
                    {
                        // unordered named argument causes requirement to use named arguments
                        if (argumentParameters[i] != unnamedArgumentParameters[i])
                            return true;
                    }
                }
                finally
                {
                    s_expressionListPool.ReturnToPool(unnamedArguments);
                }
            }

            return false;
        }

        private static void GetUnnamedArguments(IReadOnlyList<Expression> arguments, List<Expression> unnamedArguments)
        {
            foreach (var arg in arguments)
            {
                if (arg is SimpleNamedExpression sn)
                {
                    unnamedArguments.Add(sn.Expression);
                }
                else
                {
                    unnamedArguments.Add(arg);
                }
            }
        }

        private HashSet<string> GetSpecifiedArgumentNames(IReadOnlyList<Expression> arguments)
        {
            var names = new HashSet<string>();

            for (int i = 0; i < arguments.Count; i++)
            {
                var arg = arguments[i];
                if (Binder.GetExpressionDeclaredName(arg) is string name)
                {
                    names.Add(name);
                }
            }

            return names;
        }

        private HashSet<string> GetArgumentNameSuggestions(IReadOnlyList<Signature> signatures, IReadOnlyList<Expression> arguments, int argumentIndex)
        {
            var specifiedNames = GetSpecifiedArgumentNames(arguments);
            var unspecifiedNames = new HashSet<string>();
            var suggestedNames = new HashSet<string>();

            foreach (var sig in signatures)
            {
                // only consider signatures that could match
                if (arguments.Count <= sig.MaxArgumentCount
                    && specifiedNames.All(n => sig.Parameters.Any(p => p.Name == n)))
                {
                    unspecifiedNames.Clear();
                    var argumentParameters = sig.GetArgumentParameters(arguments);

                    // check for argument with unspecified names
                    for (int i = 0, n = arguments.Count; i < n; i++)
                    {
                        if (i != argumentIndex)
                        {
                            var arg = arguments[i];
                            var p = argumentParameters[i];
                            unspecifiedNames.Add(p.Name);
                        }
                    }

                    foreach (var p in sig.Parameters)
                    {
                        if (!specifiedNames.Contains(p.Name) && !unspecifiedNames.Contains(p.Name))
                        {
                            suggestedNames.Add(p.Name);
                        }
                    }
                }
            }

            return suggestedNames;
        }

        private static SymbolMatch GetSymbolMatch(CompletionHint hint)
        {
            var match = SymbolMatch.None;

            if ((hint & CompletionHint.Column) != 0)
                match |= SymbolMatch.Column;

            if ((hint & CompletionHint.TabularFunction) != 0)
                match |= SymbolMatch.Function | SymbolMatch.Tabular | SymbolMatch.Local;

            if ((hint & (CompletionHint.ScalarFunction | CompletionHint.Aggregate)) != 0)
                match |= SymbolMatch.Function | SymbolMatch.Scalar | SymbolMatch.Local;

            if ((hint & (CompletionHint.Function | CompletionHint.DatabaseFunction)) != 0)
                match |= SymbolMatch.Function;

            if ((hint & (CompletionHint.Scalar | CompletionHint.Boolean | CompletionHint.Number)) != 0)
                match |= SymbolMatch.Scalar | SymbolMatch.Column | SymbolMatch.Function | SymbolMatch.Local;

            if ((hint & CompletionHint.NonScalar) != 0)
                match |= SymbolMatch.NonScalar | SymbolMatch.Table | SymbolMatch.ExternalTable | SymbolMatch.MaterializedView | SymbolMatch.Function | SymbolMatch.Local | SymbolMatch.EntityGroupElement | SymbolMatch.Graph;

            if ((hint & CompletionHint.Tabular) != 0)
                match |= SymbolMatch.Tabular | SymbolMatch.Table | SymbolMatch.ExternalTable | SymbolMatch.MaterializedView | SymbolMatch.Function | SymbolMatch.Local | SymbolMatch.EntityGroupElement | SymbolMatch.Graph;

            // any expression scalar or tabular or other
            // incluide scalar & non-scalar in case others have added either.
            if ((hint & CompletionHint.Expression) != 0)
                match |= SymbolMatch.Scalar | SymbolMatch.NonScalar | SymbolMatch.Column | SymbolMatch.Table | SymbolMatch.ExternalTable | SymbolMatch.MaterializedView | SymbolMatch.Function | SymbolMatch.Local | SymbolMatch.EntityGroup | SymbolMatch.EntityGroupElement | SymbolMatch.Graph;

            if ((hint & CompletionHint.Table) != 0)
                match |= SymbolMatch.Table;

            if ((hint & CompletionHint.ExternalTable) != 0)
                match |= SymbolMatch.ExternalTable;

            if ((hint & CompletionHint.MaterializedView) != 0)
                match |= SymbolMatch.MaterializedView;

            if ((hint & CompletionHint.Database) != 0)
                match |= SymbolMatch.Database;

            if ((hint & CompletionHint.Cluster) != 0)
                match |= SymbolMatch.Cluster;

            if ((hint & CompletionHint.EntityGroup) != 0)
                match |= SymbolMatch.EntityGroup;

            if ((hint & CompletionHint.Option) != 0)
                match |= SymbolMatch.Option;

            if ((hint & CompletionHint.Graph) != 0)
                match |= SymbolMatch.Graph;

            if ((hint & CompletionHint.GraphModel) != 0)
                match |= SymbolMatch.GraphModel;

            if ((hint & CompletionHint.GraphSnapshot) != 0)
                match |= SymbolMatch.GraphSnapshot;

            if ((hint & CompletionHint.StoredQueryResult) != 0)
                match |= SymbolMatch.StoredQueryResult;

            return match;
        }

        /// <summary>
        /// Gets the <see cref="CompletionHint"/> at the specified text position.
        /// </summary>
        private CompletionHint GetCompletionHint(int position)
        {
            if (TryGetCompletionContext(position, out var contextNode, out var index))
            {
                return GetCompletionHint(position, contextNode, index, CompletionHint.Query);
            }
            else
            {
                // no context node?
                return CompletionHint.None;
            }
        }

        /// <summary>
        /// Gets the <see cref="CompletionHint"/> for the specified child slot of the context node
        /// and any following slots that can offer additional hints.
        /// </summary>
        private CompletionHint GetCompletionHint(int position, SyntaxNode contextNode, int childIndex, CompletionHint defaultHint = CompletionHint.None)
        {
            var hint = GetChildHint(contextNode, childIndex, defaultHint);

            if (contextNode is SeparatedElement && childIndex == 1)
            {
                // list element separators are encoded as optional in the tree,
                // but are not really optional in the grammar, so don't add follow on hints.
                return hint;
            }

            // if this child is actually required but missing then stop getting more hints
            if (IsChildMissing(contextNode, childIndex))
                return hint;

            // if this child was optional and empty then also get hints from following child slots
            // if contextNode is list, then it can have followers too
            if (IsChildEmpty(contextNode, childIndex) 
                || IsChildOnNewLine(contextNode, childIndex)
                || contextNode is SyntaxList)
            {
                while (contextNode != null
                    && IsWithinLineConstraint(contextNode, position))
                {
                    // get hints for all following missing or empty children
                    for (int i = childIndex + 1, n = contextNode.ChildCount; i < n; i++)
                    {
                        // if next child is not missing or empty then we've already got all the hints
                        if (!IsChildMissingOrEmpty(contextNode, i)
                            && !IsChildOnNewLine(contextNode, i))
                        {
                            return hint;
                        }

                        hint |= GetChildHint(contextNode, i, defaultHint);

                        // if this child is actually required but missing then stop getting more hints
                        if (IsChildMissing(contextNode, i))
                            return hint;
                    }

                    // also get hints for contextNode's missing or empty following siblings in parent
                    var parent = contextNode.Parent as SyntaxNode;
                    var indexInParent = parent != null ? parent.GetChildIndex(contextNode) : 0;

                    if (parent != null 
                        && HasLineConstraint(contextNode)
                        && !HasLineConstraint(parent))
                    {
                        // siblings of parent w/o a constraint are not within child's line constraint
                        return hint;
                    }

                    contextNode = parent;
                    childIndex = indexInParent;
                }
            }

            return hint;
        }

        /// <summary>
        /// True if the node is line constrained (it must exist in a single line).
        /// </summary>
        private static bool HasLineConstraint(SyntaxElement element)
        {
            return TryGetLineConstraint(element, out _);
        }

        /// <summary>
        /// Gets the ancestor element that has a line constraint.
        /// </summary>
        private static bool TryGetLineConstraint(SyntaxElement element, out SyntaxNode constrained)
        {
            constrained = element.GetFirstAncestorOrSelf<Directive>();
            return constrained != null;
        }

        /// <summary>
        /// True if the position is within the line range of the specified syntax element.
        /// </summary>
        private bool IsWithinLineConstraint(SyntaxElement element, int position)
        {
            if (TryGetLineConstraint(element, out var constraint))
            {
                return constraint.GetFirstToken() is SyntaxToken firstToken
                    && constraint.GetLastToken() is SyntaxToken lastToken
                    && _code.TryGetLineAndOffset(firstToken.TextStart, out var firstLine, out _)
                    && _code.TryGetLineAndOffset(Math.Max(firstToken.TextStart, lastToken.End - 1), out var lastLine, out _)
                    && _code.TryGetLineAndOffset(position, out var positionLine, out _)
                    && positionLine >= firstLine
                    && positionLine <= lastLine;
            }
            else
            {
                // has no line constraint.
                return true;
            }
        }

        /// <summary>
        /// Gets the <see cref="CompletionHint"/> for the specified child slot of the context node.
        /// </summary>
        private CompletionHint GetChildHint(SyntaxNode contextNode, int childIndex, CompletionHint @default = CompletionHint.None)
        {
            // if the context/child is an argument to a function or operator then determine the hint based on the defined parameter
            if (TryGetFunctionOrOperatorArgument(contextNode, childIndex, out var signatures, out var arguments, out var argumentIndex, out var argumentCount, out var parameterName))
            {
                return GetParameterHint(signatures, arguments, parameterName, argumentIndex);
            }

            while (true)
            {
                var hint = contextNode.GetCompletionHint(childIndex);
                if (hint != CompletionHint.Inherit)
                    return hint;

                if (contextNode.Parent is SyntaxNode parent)
                {
                    childIndex = parent.GetChildIndex(contextNode);
                    contextNode = parent;
                }
                else
                {
                    // walked all the way to the top of the syntax tree
                    return @default;
                }
            }
        }

        private static bool IsChildMissingOrEmpty(SyntaxNode contextNode, int childIndex)
        {
            return IsChildMissing(contextNode, childIndex) || IsChildEmpty(contextNode, childIndex);
        }

        private static bool IsChildMissing(SyntaxNode node, int index)
        {
            var child = node.GetChild(index);
            return child != null && child.IsMissing;
        }

        private static bool IsChildEmpty(SyntaxNode node, int index)
        {
            var child = node.GetChild(index);
            return (child == null || child.Width == 0) && !IsChildMissing(node, index);
        }

        private static bool IsChildOnNewLine(SyntaxNode node, int index)
        {
            if (node.GetChild(index) is SyntaxElement child
                && child.GetFirstToken() is SyntaxToken token)
            {
                var firstLB = TextFacts.GetNextLineBreakStart(token.Trivia, 0);
                return firstLB >= 0 && TextFacts.IsWhitespaceOnly(token.Trivia, 0, firstLB);
            }

            return false;
        }

        private static ObjectPool<List<Parameter>> s_parameterListPool =
            new ObjectPool<List<Parameter>>(() => new List<Parameter>(), list => list.Clear());

        public CompletionHint GetParameterHint(IReadOnlyList<Signature> signatures, IReadOnlyList<Expression> arguments, string parameterName, int iArgument)
        {
            var hint = CompletionHint.None;

            // if iArgument is last argument, but also missing, don't use the last argument
            if (iArgument == arguments.Count - 1 && arguments[iArgument].IsMissing)
            {
                arguments = arguments.Take(iArgument).ToList();
            }

            foreach (var signature in signatures)
            {
                if (!signature.IsHidden)
                {
                    if (parameterName != null)
                    {
                        hint |= GetParameterHint(signature, signature.GetParameter(parameterName));
                    }
                    else
                    {
                        if (iArgument < arguments.Count)
                        {
                            hint |= GetArgumentParameterHints(signature, arguments, iArgument);
                        }
                        else
                        {
                            hint |= GetNextPossibleParameterHints(signature, arguments);
                        }
                    }
                }
            }

            return hint;
        }

        private static CompletionHint GetNextPossibleParameterHints(Signature signature, IReadOnlyList<Expression> arguments)
        {
            var parameterList = s_parameterListPool.AllocateFromPool();
            try
            {
                CompletionHint hint = CompletionHint.None;

                signature.GetNextPossibleParameters(arguments, parameterList);

                foreach (var p in parameterList)
                {
                    hint |= GetParameterHint(signature, p);
                }

                return hint;
            }
            finally
            {
                s_parameterListPool.ReturnToPool(parameterList);
            }
        }

        private static CompletionHint GetArgumentParameterHints(Signature signature, IReadOnlyList<Expression> arguments, int iArgument)
        {
            var parameterList = s_parameterListPool.AllocateFromPool();
            try
            {
                signature.GetArgumentParameters(arguments, parameterList);
                var p = parameterList[iArgument];
                return GetParameterHint(signature, p);
            }
            finally
            {
                s_parameterListPool.ReturnToPool(parameterList);
            }
        }

        private static CompletionHint GetParameterHint(Signature signature, Parameter parameter)
        {
            if (parameter != null)
            {
                switch (parameter.ArgumentKind)
                {
                    case ArgumentKind.Column:
                    case ArgumentKind.Column_Parameter0:
                    case ArgumentKind.Column_Parameter0_Common:
                        return CompletionHint.Column;
                    case ArgumentKind.StarOnly:
                        return CompletionHint.None;
                }

                switch (parameter.TypeKind)
                {
                    case ParameterTypeKind.Declared:
                        if (parameter.DeclaredTypes[0].IsScalar)
                        {
                            if (parameter.DeclaredTypes.Count == 1 && parameter.DeclaredTypes[0] == ScalarTypes.Bool)
                            {
                                return CompletionHint.Boolean;
                            }
                            else if (parameter.DeclaredTypes.All(t => t is ScalarSymbol s && s.IsNumeric))
                            {
                                return CompletionHint.Number;
                            }
                            else
                            {
                                return CompletionHint.Scalar;
                            }
                        }
                        else
                        {
                            return CompletionHint.NonScalar;
                        }
                    case ParameterTypeKind.Parameter0:
                        return GetParameterHint(signature, signature.Parameters.Count > 0 ? signature.Parameters[0] : null);
                    case ParameterTypeKind.Parameter1:
                        return GetParameterHint(signature, signature.Parameters.Count > 1 ?  signature.Parameters[1] : null);
                    case ParameterTypeKind.Parameter2:
                        return GetParameterHint(signature, signature.Parameters.Count > 2 ? signature.Parameters[2] : null);
                    case ParameterTypeKind.Tabular:
                        return CompletionHint.NonScalar;
                    case ParameterTypeKind.Database:
                        return CompletionHint.Database;
                    case ParameterTypeKind.Cluster:
                        return CompletionHint.Cluster;
                    case ParameterTypeKind.Number:
                    case ParameterTypeKind.NumberOrBool:
                        return CompletionHint.Number;
                    default:
                        return CompletionHint.Scalar;
                }
            }

            return CompletionHint.None;
        }

        /// <summary>
        /// Gets the index of a child node within the parent context corresponding to the text position.
        /// If the position is between nodes (in trivia/whitespace) it chooses the best index between existing nodes, that currently has no child node.
        /// </summary>
        private static int GetChildIndex(SyntaxNode node, int position)
        {
            int firstChildBeyond = node.ChildCount;

            bool lastWasNullOrEmpty = false;

            for (int i = 0, n = node.ChildCount; i < n; i++)
            {
                var child = node.GetChild(i);
                if (child != null)
                {
                    if (position > child.TextStart && position < child.End && child.Width > 0)
                    {
                        // position is in middle of child, so this is the correct child index
                        return i;
                    }
                    else if (position < child.TextStart)
                    {
                        // we went beyond the position, so remember this for next step
                        firstChildBeyond = i;
                        break;
                    }
                    else if (position == child.TextStart)
                    {
                        if (child.Width == 0)
                        {
                            firstChildBeyond = i + 1;
                            break;
                        }
                        else if (lastWasNullOrEmpty)
                        {
                            firstChildBeyond = i;
                            break;
                        }
                    }
                    else
                    {
                        lastWasNullOrEmpty = child.Width == 0;
                    }
                }
                else
                {
                    lastWasNullOrEmpty = true;
                }
            }

            int bestEmptyChild = firstChildBeyond - 1;
            for (int i = firstChildBeyond - 1; i >= 0; i--)
            {
                var child = node.GetChild(i);
                if (child != null && child.Width > 0)
                {
                    break;
                }
                else
                {
                    bestEmptyChild = i;
                }
            }

            return bestEmptyChild;
        }

        /// <summary>
        /// determines if the node has an empty child appropriate for the position.
        /// </summary>
        private static bool HasEmptyChild(SyntaxElement element, int position)
        {
            if (element == null)
                return false;

            // on the right edge of a list.. counts as empty
            if (position >= element.End && element.Kind == SyntaxKind.List)
                return true;

            // look for existence of empty slot before position
            bool hasEmptyChild = false;

            for (int i = 0, n = element.ChildCount; i < n; i++)
            {
                var child = element.GetChild(i);
                if (child != null)
                {
                    // position is before this node and we had a previous empty slot
                    if (position <= child.TextStart && hasEmptyChild)
                        return true;

                    // inside this child (therefore no appropriate empty child for this position)
                    if (position < child.End)
                        return false;
                }

                // child does not exist (optional) or has not content (missing node)
                hasEmptyChild = (child == null || child.Width == 0);
            }

            // got all the way to the end w/o finding our position
            return hasEmptyChild;
        }

        private static SyntaxNode GetNearestAncestorWithEmptyChild(SyntaxToken token, int position)
        {
            var node = token.Parent;

            while (node != null && !HasEmptyChild(node, position))
            {
                node = node.Parent;
            }

            return node;
        }

        /// <summary>
        /// Find the most suitable node and child slot that contains the text position.
        /// </summary>
        public bool TryGetCompletionContext(int position, out SyntaxNode contextNode, out int contextChildIndex)
        {
            contextNode = null;
            contextChildIndex = 0;

            var token = _code.Syntax.GetTokenAt(position);

            if (token == null)
                return false;

            var hasAffinity = HasAffinity(token, position);
            if (hasAffinity)
            {
                // don't have symbol completions for tokens following a missing token
                var prev = token.GetPreviousToken(includeZeroWidthTokens: true);
                if (prev != null && prev.IsMissing)
                {
                    return false;
                }

                if (IsWithinLineConstraint(token, position))
                {
                    contextNode = token.Parent;
                    contextChildIndex = GetChildIndex(contextNode, position);
                    return true;
                }
            }

            var tokenNode = GetNearestAncestorWithEmptyChild(token, position);

            if (position <= token.TextStart && !hasAffinity 
                || token.Kind == SyntaxKind.EndOfTextToken)
            {
                var prevToken = token.GetPreviousToken();
                if (prevToken != null)
                {
                    var prevNode = GetNearestAncestorWithEmptyChild(prevToken, position);
                    if (prevNode != null)
                    {
                        if ((tokenNode == null || prevNode.Depth >= tokenNode.Depth)
                            && IsWithinLineConstraint(prevNode, position))
                        {
                            contextNode = prevNode;
                            contextChildIndex = GetChildIndex(contextNode, position);
                            return true;
                        }
                    }
                }
            }
            else if (position >= token.End && !hasAffinity)
            {
                var nextToken = token.GetNextToken();
                if (nextToken != null)
                {
                    var nextNode = GetNearestAncestorWithEmptyChild(nextToken, position);
                    if (nextNode != null)
                    {
                        if ((tokenNode == null || nextNode.Depth > tokenNode.Depth)
                            && IsWithinLineConstraint(nextNode, position))
                        {
                            contextNode = nextNode;
                            contextChildIndex = GetChildIndex(contextNode, position);
                            return true;
                        }
                    }
                }
            }

            if (tokenNode != null
                && IsWithinLineConstraint(tokenNode, position))
            {
                contextNode = tokenNode;
                contextChildIndex = GetChildIndex(contextNode, position);
                return true;
            }
            
            if (position > token.TriviaStart && position < token.TextStart && !hasAffinity)
            {
                // if we got here then there was no ancestor with empty child, so no syntax hole to fill
                // yet we are also inside trivia and only whitespace until end of line, meaning the next syntax part
                // is on a separate line, so allow completions to produce the same list as for syntax slot that contains
                // the next part
                var nextLBStart = TextFacts.GetNextLineBreakStart(token.Trivia, position - token.TriviaStart);
                if (nextLBStart >= 0 && TextFacts.IsWhitespaceOnly(token.Trivia, 0, nextLBStart))
                {
                    var prevToken = token.GetPreviousToken();
                    if (prevToken != null)
                    {
                        contextNode = SyntaxElement.GetCommonAncestor(prevToken, token);
                        if (contextNode != null)
                        {
                            contextChildIndex = contextNode.GetDescendantIndex(token);
                            return contextChildIndex >= 0;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region Syntax Completions

        private void GetSyntaxCompletions(int position, CompletionBuilder builder)
        {
            var hints = GetCompletionHint(position);
            var match = GetSymbolMatch(position);
            var expr = GetCompleteExpressionLeftOfPosition(_code, position);

            // look for any completion items annotated on a parser corresponding to this location
            var annotations = GetGrammarAnnotations(position);

            // add in any completion hints associated with this parser
            foreach (var hint in annotations.OfType<CompletionHint>())
            {
                hints |= hint;
            }

            // consider all completion items associated with this parser
            foreach (var item in annotations.OfType<CompletionItem>())
            {
                if (IncludeSyntax(item, position, hints, match, expr))
                {
                    if (ShouldAugmentSyntaxCompletionItem(item))
                    {
                        var augmentedItem = AddSpaceToCompletionItem(item);
                        builder.Add(augmentedItem);
                    }
                    else
                    {
                        builder.Add(item);
                    }
                }
            }

            if (_options.IncludeExtendedSyntax)
            {
                // get all extended completions derived from syntax
                GetExtendedSyntaxCompletions(position, builder);
            }
        }

        private CompletionItem AddSpaceToCompletionItem(CompletionItem item)
        {
            // add space after before text to help trigger completion again
            if (item.BeforeText.Length > 0 
                && !char.IsWhiteSpace(item.BeforeText[item.BeforeText.Length - 1]))
            {
                return item.WithBeforeText(item.BeforeText + " ");
            }

            return item;
        }

        private static IReadOnlyList<string> punctuationWithoutSpace =
            new[] { "(", "[", "{", "*", "@", "$" };

        private bool ShouldAugmentSyntaxCompletionItem(CompletionItem item)
        {
            if (!_options.AutoAppendWhitespace)
                return false;

            if (item.AfterText?.Length > 0)
                return false;

            // was this an auto appended '='?
            // make appended additional space depend on whether it is currently separated by a space.
            if (item.BeforeText.EndsWith("=")
                && TextFacts.IsLetterOrDigit(item.BeforeText[0])
                && item.BeforeText.Length > 1
                && !TextFacts.IsWhitespace(item.BeforeText[item.BeforeText.Length - 2]))
                return false;

            switch (item.Kind)
            {
                case CompletionKind.Column:
                case CompletionKind.BuiltInFunction:
                case CompletionKind.LocalFunction:
                case CompletionKind.DatabaseFunction:
                case CompletionKind.AggregateFunction:
                case CompletionKind.Parameter:
                case CompletionKind.Variable:
                case CompletionKind.Table:
                case CompletionKind.Database:
                case CompletionKind.Cluster:
                    // these should't appear, but if they do, then don't have a space.
                    return false;

                case CompletionKind.Punctuation:
                    return punctuationWithoutSpace.Any(p => item.DisplayText == p);

                default:
                    // offer space for syntatic keywords
                    return true;
            }
        }

        private IReadOnlyList<object> _annotations;
        private int _annotationPosition;

        /// <summary>
        /// Gets the annotations on the grammar rules that are invoke for the token at the specified text position.
        /// </summary>
        private IReadOnlyList<object> GetGrammarAnnotations(int position)
        {
            if (_annotations != null && _annotationPosition == position)
                return _annotations;

            // look for completions in the grammar elements corresponding to the text position
            var parsers = GetAnnotatedParsers(position);

            _annotations = parsers.SelectMany(p => p.Annotations).ToList();
            _annotationPosition = position;

            return _annotations;
        }

        private IReadOnlyList<Parser<LexicalToken>> _annotatedParsers;
        private int _annotatedParserPosition;

        /// <summary>
        /// Gets a list of all annotated parsers that are invoked for the token at the specified text position.
        /// </summary>
        private IReadOnlyList<Parser<LexicalToken>> GetAnnotatedParsers(int position)
        {
            if (_annotatedParsers != null 
                && _annotatedParserPosition == position)
            {
                return _annotatedParsers;
            }

            // find a possible better starting point than the start of the whole query
            GetGrammarSearchContext(position, out var startPosition, out var grammar);

            var startIndex = _code.GetTokenIndex(startPosition);
            var matchIndex = _code.GetTokenIndex(position);

            // use a source that only includes relevant tokens (so we dont bother with tokens beyond)
            var tokens = _code.GetLexicalTokens();
            var source = new ArraySource<LexicalToken>(tokens, 0, matchIndex + 1);

            _annotatedParsers = AnnotatedParserFinder<LexicalToken>.FindParsers(source, matchIndex, grammar, startIndex);
            _annotatedParserPosition = position;
            return _annotatedParsers;
        }

        private IReadOnlyList<ParserPath<LexicalToken>> _annotatedParserPaths;
        private int _annotatedParserPathPosition;

        /// <summary>
        /// Gets a list of all cursors to annotated parsers that are invoked for the token at the specified text position.
        /// </summary>
        private IReadOnlyList<ParserPath<LexicalToken>> GetAnnotatedParserPaths(int position)
        {
            if (_annotatedParserPaths != null
                && _annotatedParserPathPosition == position)
            {
                return _annotatedParserPaths;
            }

            // find a possible better starting point than the start of the whole query
            GetGrammarSearchContext(position, out var startPosition, out var grammar);

            var startIndex = _code.GetTokenIndex(startPosition);
            var matchIndex = _code.GetTokenIndex(position);

            // use a source that only includes relevant tokens (so we dont bother with tokens beyond)
            var tokens = _code.GetLexicalTokens();
            var source = new ArraySource<LexicalToken>(tokens, 0, matchIndex + 1);

            _annotatedParserPaths = AnnotatedParserFinder<LexicalToken>.FindPaths(source, matchIndex, grammar, startIndex);
            _annotatedParserPathPosition = position;
            return _annotatedParserPaths;
        }

        private QueryGrammar __queryGrammar;
        private QueryGrammar CurrentQueryGrammar
        {
            get
            {
                if (__queryGrammar == null)
                {
                    __queryGrammar = QueryGrammar.From(_code.Globals);
                }

                return __queryGrammar;
            }
        }

        private CommandGrammar __commandGrammar;
        private CommandGrammar CurrentCommandGrammar
        {
            get
            {
                if (__commandGrammar == null)
                {
                    __commandGrammar = CommandGrammar.From(_code.Globals);
                }

                return __commandGrammar;
            }
        }

        /// <summary>
        /// Find a nearby starting point (contextNode) and grammar rule to search with
        /// </summary>
        private void GetGrammarSearchContext(int position, out int searchStart, out Parser<LexicalToken> grammar)
        {
            var token = GetTokenLeftOfPosition(_code, position);

            if (token != null)
            {
                var queryGrammar = CurrentQueryGrammar;
                var node = token.Parent;
                var nextToken = token.GetNextToken();

                for (; node != null; node = node.Parent)
                {
                    switch (node)
                    {
                        case QueryOperator op:
                            searchStart = op.TextStart;

                            // the query operator on right of pipe
                            if (op.Parent is PipeExpression pipe && pipe.Operator == op)
                            {
                                grammar = queryGrammar.FollowingPipeElementExpression;
                                return;
                            }

                            // places where we know a normal pipe expression to exist
                            if (op.Parent is ParenthesizedExpression
                                || op.Parent is ExpressionStatement
                                || op.Parent is FunctionBody)
                            {
                                grammar = queryGrammar.PipeExpression;
                                return;
                            }

                            // otherwise back up further (to containing operator, etc) for 
                            // better starting point
                            break;
                        case PipeExpression pe:
                            // this is meant to handle the case of: XXX | YYY | $
                            if (position >= pe.Bar.TriviaStart)
                            {
                                if (pe.Expression is PipeExpression priorPipe)
                                {
                                    searchStart = priorPipe.Operator.TextStart;
                                    grammar = queryGrammar.PipeSubExpression;
                                    return;
                                }
                            }                            
                            break;
                        case FunctionBody body:
                            searchStart = body.TextStart;
                            grammar = queryGrammar.FunctionBody;
                            return;
                        case Statement stat:
                            if (IsQueryPart(node, 0))
                            {
                                searchStart = stat.TextStart;
                                grammar = queryGrammar.StatementList;
                                return;
                            }
                            break;
                        case SeparatedElement<Statement> _:
                            if (IsQueryPart(node, 0))
                            {
                                searchStart = node.TextStart;
                                grammar = queryGrammar.StatementList;
                                return;
                            }
                            break;
                        case FunctionCallExpression fc:
                            // for argument list
                            if (position > fc.ArgumentList.OpenParen.End
                                && ((!fc.ArgumentList.CloseParen.IsMissing && position <= fc.ArgumentList.CloseParen.TextStart)
                                    || fc.ArgumentList.CloseParen.IsMissing && nextToken != null && nextToken.Kind == SyntaxKind.EndOfTextToken))
                            {
                                searchStart = fc.TextStart;
                                grammar = queryGrammar.UnnamedExpression;
                                return;
                            }
                            break;
                        case SeparatedElement<Expression> se:
                            var parent = se.Parent?.Parent;
                            if (parent is DataTableExpression)
                            {
                                searchStart = se.TextStart;
                                grammar = queryGrammar.LiteralList;
                                return;
                            }
                            break;
                        case Command _:
                            searchStart = node.TextStart;
                            grammar = _code.Grammar; // use entire command block grammar
                            return;
                    }
                }
            }

            // otherwise use the grammar associated with the entire source
            searchStart = this._code.Syntax.TextStart;
            grammar = this._code.Grammar;
        }

        private bool IncludeSyntax(CompletionItem item, int position, CompletionHint hints, SymbolMatch match, Expression left)
        {
            if (!_options.IncludePunctuationOnlySyntax 
                && item.Kind == CompletionKind.Punctuation)
                return false;

            switch (item.Kind)
            {
                case CompletionKind.QueryPrefix:
                    return (hints & CompletionHint.Query) != 0
                        || (hints & CompletionHint.Keyword) != 0
                        || (match & SymbolMatch.Tabular) != 0
                        || (match & SymbolMatch.NonScalar) != 0;
                case CompletionKind.TabularPrefix:
                    return (match & SymbolMatch.Tabular) != 0
                        || (match & SymbolMatch.NonScalar) != 0;
                case CompletionKind.TabularSuffix:
                    return left != null && left.ResultType != null && left.ResultType.IsTabular;
                case CompletionKind.ScalarPrefix:
                case CompletionKind.Example:
                    return (match & SymbolMatch.Scalar) != 0;
                case CompletionKind.ScalarInfix:
                    return AnyInfixMatches(left, item.MatchText, position);
                default:
                    return true;
            }
        }

        private bool AnyInfixMatches(Expression left, string op, int position)
        {
            var kind = GetOperatorKind(op);
            if (kind == OperatorKind.None)
                return false;

            // walk up expression tree and look for binary operator that would succeed with
            // type on the left
            for (; left != null; left = left.Parent as Expression)
            {
                if (position < left.End)
                    break;

                if (left.ResultType != null)
                {
                    // determine context that the expression is in
                    CompletionHint hint = CompletionHint.None;
                    if (left.Parent is SyntaxNode parent)
                    {
                        hint = GetChildHint(parent, parent.GetChildIndex(left), CompletionHint.Query);
                    }

                    if (HasMatchingInfixOperator(kind, left.ResultType, hint))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the operator is defined for the left operand type.
        /// </summary>
        private bool HasMatchingInfixOperator(OperatorKind kind, TypeSymbol arg0Type, CompletionHint returnTypeHint)
        {
            var op = this._code.Globals.GetOperator(kind);

            foreach (var sig in op.Signatures)
            {
                if (!sig.IsHidden 
                    && ParameterMatches(sig.Parameters[0], arg0Type)
                    && ReturnTypeMatchesContext(sig, arg0Type, returnTypeHint))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ReturnTypeMatchesContext(Signature signature, TypeSymbol arg0Type, CompletionHint returnTypeHint)
        {
            if ((returnTypeHint & CompletionHint.Boolean) != 0)
            {
                return IsBooleanReturnType(signature, arg0Type);
            }
            else if ((returnTypeHint & CompletionHint.Scalar) != 0
                || (returnTypeHint & CompletionHint.Aggregate) != 0)
            {
                return IsNonBooleanScalarReturnType(signature, arg0Type);
            }
            else if ((returnTypeHint & CompletionHint.Number) != 0)
            {
                return IsNumericReturnType(signature, arg0Type);
            }
            else if ((returnTypeHint & CompletionHint.Column) != 0)
            {
                // no operators produce columns
                return false;
            }
            else
            {
                // assumes signature is for scalar operator
                return true;
            }
        }

        private static bool IsBooleanReturnType(Signature signature, TypeSymbol arg0Type)
        {
            switch (signature.ReturnKind)
            {
                case ReturnTypeKind.Common:
                case ReturnTypeKind.CommonNonDynamic:
                case ReturnTypeKind.Widest:
                case ReturnTypeKind.Parameter0:
                    return arg0Type is ScalarSymbol s && s == ScalarTypes.Bool;
                case ReturnTypeKind.Declared:
                    return signature.DeclaredReturnType == ScalarTypes.Bool;
                default:
                    return false;
            }
        }

        private static bool IsNonBooleanScalarReturnType(Signature signature, TypeSymbol arg0Type)
        {
            switch (signature.ReturnKind)
            {
                case ReturnTypeKind.Common:
                case ReturnTypeKind.CommonNonDynamic:
                case ReturnTypeKind.Widest:
                case ReturnTypeKind.Parameter0:
                    return arg0Type is ScalarSymbol s && s != ScalarTypes.Bool;
                case ReturnTypeKind.Declared:
                    return signature.DeclaredReturnType.IsScalar && signature.DeclaredReturnType != ScalarTypes.Bool;
                default:
                    return false;
            }
        }

        private static bool IsNumericReturnType(Signature signature, TypeSymbol arg0Type)
        {
            switch (signature.ReturnKind)
            {
                case ReturnTypeKind.Common:
                case ReturnTypeKind.CommonNonDynamic:
                case ReturnTypeKind.Widest:
                case ReturnTypeKind.Parameter0:
                    return arg0Type is ScalarSymbol s && s.IsNumeric;
                case ReturnTypeKind.Declared:
                    return signature.DeclaredReturnType is ScalarSymbol s2 && s2.IsNumeric;
                default:
                    return false;
            }
        }

        private static bool ParameterMatches(Parameter parameter, TypeSymbol type)
        {
            switch (parameter.TypeKind)
            {
                case ParameterTypeKind.Any:
                    // always matches
                    return true;

                case ParameterTypeKind.Declared:
                    if (type.IsAssignableToAny(parameter.DeclaredTypes))
                    {
                        return true;
                    }
                    else if (type.IsPromotableTo(parameter.DeclaredTypes[0]))
                    {
                        return true;
                    }
                    break;

                case ParameterTypeKind.Scalar:
                    return type.IsScalar;

                case ParameterTypeKind.Integer:
                    return type.IsInteger();

                case ParameterTypeKind.RealOrDecimal:
                    return type.IsRealOrDecimal();

                case ParameterTypeKind.StringOrDynamic:
                    return type.IsStringOrDynamic();

                case ParameterTypeKind.StringOrArray:
                    return type.IsStringOrArray();

                case ParameterTypeKind.IntegerOrArray:
                    return type.IsIntegerOrArray();

                case ParameterTypeKind.Number:
                    return type.IsNumeric();

                case ParameterTypeKind.NumberOrBool:
                    return type.IsNumericOrBool();

                case ParameterTypeKind.Summable:
                    return type.IsSummable();

                case ParameterTypeKind.Orderable:
                    return type.IsOrderable();

                case ParameterTypeKind.Tabular:
                    return type.IsTabular;

                case ParameterTypeKind.Database:
                    return type is DatabaseSymbol;

                case ParameterTypeKind.Cluster:
                    return type is ClusterSymbol;

                case ParameterTypeKind.NotBool:
                    return type.IsAnyScalarExceptBool();

                case ParameterTypeKind.NotRealOrBool:
                    return type.IsAnyScalarExceptReadOrBool();

                case ParameterTypeKind.NotDynamic:
                    return type.IsAnyScalarExceptDynamic();

                case ParameterTypeKind.Parameter0:
                case ParameterTypeKind.Parameter1:
                case ParameterTypeKind.Parameter2:
                case ParameterTypeKind.CommonScalar:
                case ParameterTypeKind.CommonNumber:
                case ParameterTypeKind.CommonSummable:
                case ParameterTypeKind.CommonOrderable:
                case ParameterTypeKind.CommonScalarOrDynamic:
                    return type is ScalarSymbol;
            }

            return false;
        }

        private static OperatorKind GetOperatorKind(string opText)
        {
            return SyntaxFacts.TryGetKind(opText, out var kind) ? kind.GetOperatorKind() : OperatorKind.None;
        }

        private CompletionKind GetCompletionKind(Symbol symbol)
        {
            switch (symbol.Kind)
            {
                case SymbolKind.Cluster:
                    return CompletionKind.Cluster;
                case SymbolKind.Column:
                    return CompletionKind.Column;
                case SymbolKind.Database:
                    return CompletionKind.Database;
                case SymbolKind.Function:
                    var fn = (FunctionSymbol)symbol;
                    if (this._code.Globals.IsAggregateFunction(fn))
                    {
                        return CompletionKind.AggregateFunction;
                    }
                    else if (this._code.Globals.IsBuiltInFunction(fn))
                    {
                        return CompletionKind.BuiltInFunction;
                    }
                    else if (this._code.Globals.IsDatabaseFunction(fn))
                    {
                        return CompletionKind.DatabaseFunction;
                    }
                    else
                    {
                        return CompletionKind.LocalFunction;
                    }

                case SymbolKind.Pattern:
                    return CompletionKind.LocalFunction;

                case SymbolKind.Operator:
                    return CompletionKind.BuiltInFunction;

                case SymbolKind.Variable:
                    return GetVariableCompletionKind((VariableSymbol)symbol);
                case SymbolKind.Parameter:
                    return CompletionKind.Parameter;
                case SymbolKind.Table:
                    return CompletionKind.Table;
                case SymbolKind.MaterializedView:
                    return CompletionKind.MaterialiedView;
                case SymbolKind.EntityGroup:
                    return CompletionKind.EntityGroup;
                case SymbolKind.Graph:
                case SymbolKind.GraphSnapshot:
                    return CompletionKind.Graph;
                case SymbolKind.StoredQueryResult:
                    return CompletionKind.StoredQueryResult;
                case SymbolKind.Primitive:
                case SymbolKind.Tuple:
                case SymbolKind.Array:
                case SymbolKind.Bag:
                case SymbolKind.Group:
                case SymbolKind.Void:
                default:
                    return CompletionKind.Unknown;
            }
        }

        private CompletionKind GetVariableCompletionKind(VariableSymbol vs)
        {
            switch (vs.Type.Kind)
            {
                case SymbolKind.Table:
                case SymbolKind.Function:
                case SymbolKind.Pattern:
                case SymbolKind.Graph:
                    return GetCompletionKind(vs.Type);
                default:
                    return CompletionKind.Variable;
            }
        }
        #endregion

        #region Extended Syntax Completions
        /// <summary>
        /// Adds multi-term completion items generated from syntax look ahead.
        /// </summary>
        private void GetExtendedSyntaxCompletions(int position, CompletionBuilder builder)
        {
            var items = _completionItemListPool.AllocateFromPool();
            try
            {
                var paths = GetAnnotatedParserPaths(position);

                var rootPaths = paths
                    .Where(p => !IsInConditionalTest(p))
                    .Select(p => GetLocalRoot(p))
                    .Where(p => p.Parent != null 
                        && GetDeepCompletionItem(p.Parser) is CompletionItem item 
                        && item.Kind != CompletionKind.Punctuation)
                    .DistinctFirst(p => p.Parser)
                    .ToList();

                foreach (var path in rootPaths)
                {
                    items.Clear();

                    // all paths here should be the immediate child of a sequence
                    // that uses the parent sequence as a limiter for the extension
                    var ceiling = path.Parent;

                    var tmpBuilder = new CompletionBuilder();
                    BuildExtendedSyntaxCompletions(path, ceiling, items, tmpBuilder);
                    builder.AddRange(tmpBuilder.Items);
                }
            }
            catch (Exception e)
            {
                _ = e;
            }
            finally
            {
                _completionItemListPool.ReturnToPool(items);
            }
        }

        private static readonly ObjectPool<List<CompletionItem>> _completionItemListPool =
            new ObjectPool<List<CompletionItem>>(() => new List<CompletionItem>(), list => list.Clear());


        private static bool IsInConditionalTest(ParserPath<LexicalToken> path)
        {
            return TryGetFirstAncestorConditional(path, null, out var _, out int index)
                && index == 0;
        }

        /// <summary>
        /// Gets the parent sequence of the 
        /// </summary>
        private ParserPath<LexicalToken> GetLocalRoot(ParserPath<LexicalToken> path)
        {
            var current = path;

            while (true)
            {
                // find the immediate parent sequence.
                if (TryGetFirstAncestorSequence(current, null, out var parent, out int index))
                {
                    if (parent.ChildCount > 1)
                    {
                        return parent.GetChild(index);
                    }
                    else
                    {
                        current = parent;
                    }
                }
                else
                {
                    return path;
                }
            }
        }

        private static int MaxExtensionSize = 10;
        private static int MaxExtensionCount = 20;

        /// <summary>
        /// Builds the next step of extended completions given the completion texts aggregates so far
        /// and the current grammar parser.
        /// </summary>
        private void BuildExtendedSyntaxCompletions(
            ParserPath<LexicalToken> path,
            ParserPath<LexicalToken> ceiling, // don't extend above this point
            List<CompletionItem> parts,
            CompletionBuilder builder)
        {
            while (path != null)
            {
                // too many extensions
                if (builder.Count >= MaxExtensionCount)
                    return;

                if (path.Parser.IsHidden)
                {
                    // this is not supposed to show up in completion
                    // finish this sequence here
                    break;
                }
                else if (path.Parser.IsForward)
                {
                    // end here to avoid cycles
                    break;
                }
                else if (path.Parser.IsRequired)
                {
                    path = path.GetChild(0);
                    continue;
                }
                else if (path.Parser.IsConditional)
                {
                    // assume it is true and do true branch
                    path = path.GetChild(1);
                    continue;
                }
                else if (TryGetSpecialTexts(path.Parser, parts))
                {
                    var next = GetNextParserInSequence(path, ceiling);
                    path = next;
                    continue;
                }
                else if (path.Parser.IsMatch
                    && GetCompletionItem(path.Parser) is CompletionItem item
                    && item.DisplayText == item.BeforeText
                    && item.AfterText == "")
                {
                    var text = item.BeforeText;

                    // don't start extended completions on punctuation
                    if (parts.Count == 0
                        && IsPunctuation(item))
                        return;

                    parts.Add(item);

                    // handle open parens specially
                    if (IsStartBracket(text))
                    {
                        var endBracket = GetEndBracket(text);
                        parts.Add(new CompletionItem(endBracket, beforeText: "", afterText: endBracket));

                        // skip to ) and continue from there
                        var endParen = FindNextSibling(path, ceiling, endBracket);
                        if (endParen != null)
                        {
                            // continue at next after close paren
                            path = GetNextParserInSequence(endParen, ceiling);
                            continue;
                        }
                        else
                        {
                            // end here
                            break;
                        }
                    }
                    else
                    {
                        // continue to the next in sequence
                        path = GetNextParserInSequence(path, ceiling);
                        continue;
                    }
                }
                else if (path.Parser.IsSequence && path.ChildCount > 0)
                {
                    if (parts.Count < MaxExtensionSize)
                    {
                        // move down to first item in this sequence
                        path = path.GetChild(0);
                        continue;
                    }
                    else
                    {
                        // don't start a new sequence if we are too long already
                        break;
                    }
                }
                else if (path.Parser.IsOptional && !path.Parser.IsRepetition)
                {
                    // do variation with optional item
                    var currentCount = parts.Count;
                    var optPath = path.GetChild(0);
                    BuildExtendedSyntaxCompletions(optPath, ceiling, parts, builder);
                    parts.SetCount(currentCount);

                    // continue to next item
                    path = GetNextParserInSequence(path, ceiling);
                    continue;
                }
                else if (path.Parser.IsRepetition && !path.Parser.IsOptional)
                {
                    // do it once
                    path = path.GetChild(0);
                    continue;
                }
                else if (path.Parser.IsRepetition && path.Parser.IsOptional)
                {
                    // skip over optional repetition
                    path = GetNextParserInSequence(path, ceiling);
                    continue;
                }
                else if (path.Parser.IsAlternation)
                {
                    // do all branches
                    var currentCount = parts.Count;
                    for (int a = 0; a < path.ChildCount; a++)
                    {
                        var altPath = path.GetChild(a);
                        parts.SetCount(currentCount);

                        // if alternation at start of path, then use separate builders
                        // per alternate to allow for the extension count limit to apply per alt.
                        var altBuilder = parts.Count == 0 
                            ? new CompletionBuilder() 
                            : builder;

                        BuildExtendedSyntaxCompletions(altPath, ceiling, parts, altBuilder);

                        if (altBuilder != builder)
                        {
                            builder.AddRange(altBuilder.Items);
                        }
                    }
                    break;
                }
                else if (path.ChildCount == 1)
                {
                    // drill down through this parser looking for match parser w/ completion item
                    path = path.GetChild(0);
                    continue;
                }

                break;
            }

            if (parts.Count > 1)
            {
                var item = CreateExtendedSyntaxCompletion(parts);

                if (item != null 
                    && item.DisplayText != ""
                    && !char.IsWhiteSpace(item.DisplayText[0])
                    && !char.IsPunctuation(item.DisplayText[0]))
                {
                    builder.Add(item);
                }
            }
        }

        /// <summary>
        /// Returns the next sibling with the associated text
        /// </summary>
        private ParserPath<LexicalToken> FindNextSibling(ParserPath<LexicalToken> path, ParserPath<LexicalToken> ceiling, string text)
        {
            if (TryGetFirstAncestorSequence(path, ceiling, out var sequence, out var index))
            {
                for (int i = index; i < sequence.ChildCount; i++)
                {
                    var child = sequence.GetChild(i);
                    var item = GetDeepCompletionItem(child.Parser);
                    if (item != null && item.BeforeText == text && item.AfterText == "")
                        return child;
                }
            }

            return null;
        }

        private static bool IsPunctuation(CompletionItem item) =>
            IsPunctuation(item.DisplayText);

        private static bool IsPunctuation(string text) =>
            text.Length > 0 && char.IsPunctuation(text[0]);

        /// <summary>
        /// Gets the first completion item from this parser one of its descendants.
        /// </summary>
        private CompletionItem GetDeepCompletionItem(Parser<LexicalToken> parser)
        {
            while (parser != null)
            {
                var item = GetCompletionItem(parser);
                if (item != null)
                    return item;

                if (parser.ChildParserCount == 0)
                    return null;

                parser = parser.GetChildParser(0);
            }

            return null;
        }

        /// <summary>
        /// Gets <see cref="CompletionText"/> for special parser rules.
        /// </summary>
        private bool TryGetSpecialTexts(
            Parser<LexicalToken> parser, 
            List<CompletionItem> items)
        {
            var queryGrammar = CurrentQueryGrammar;
            var commandGrammar = CurrentCommandGrammar;
            var rules = commandGrammar.PredefinedRules;

            // cannot start on special text
            if (items.Count == 0)
                return false;

            // check if caret is already positioned in existing items
            var hasCaret = items.Any(item => item.HasCaret);

            if (queryGrammar.SimpleNameReference == parser
                || queryGrammar.WildcardedNameReference == parser
                || queryGrammar.BracedName == parser
                || queryGrammar.BracketedName == parser
                || rules.TableNameReference == parser
                || rules.ExternalTableNameReference == parser
                || rules.MaterializedViewNameReference == parser
                || rules.DatabaseFunctionNameReference == parser
                || rules.DatabaseNameReference == parser
                || rules.ClusterNameReference == parser
                || rules.GraphModelNameReference == parser
                || rules.GraphModelSnapshotNameReference == parser
                || rules.DatabaseOrTableNameReference == parser
                || rules.DatabaseOrTableOrColumnNameReference == parser
                || rules.TableOrColumnNameReference == parser
                || rules.DatabaseTableColumnNameReference == parser
                || rules.ColumnNameReference == parser
                || rules.ColumnNameReference == parser
                || rules.TableColumnNameReference == parser
                || rules.TableOrColumnNameReference == parser
                )
            {
                // don't show anything in completion, but move caret to this point
                items.Add(new CompletionItem("", beforeText: " ", afterText: " "));
                return true;
            }
            else if (rules.ScriptInput == parser
                || rules.QueryInput == parser)
            {
                // don't show anything in completion, but move caret to this point
                items.Add(new CompletionItem("", beforeText: " ", afterText: " "));
                return true;
            }
            else if (queryGrammar.SimpleNameDeclaration == parser
                || queryGrammar.SimpleNameDeclarationExpression == parser
                || queryGrammar.BracketedNameDeclaration == parser
                || rules.NameDeclaration == parser
                || rules.QualifiedNameDeclaration == parser)
            {
                items.Add(new CompletionItem("").WithApplyTexts(CompletionText.Create("name", caret: true, select: true)));
                return true;
            }
            else if (rules.Value == parser
                || queryGrammar.Literal == parser)
            {
                items.Add(new CompletionItem("?").WithApplyTexts(CompletionText.Create("0", caret: true, select: true)));
                return true;
            }
            else if (queryGrammar.Expression == parser
                || queryGrammar.NamedExpression == parser
                || queryGrammar.UnnamedExpression == parser)
            {
                if (hasCaret)
                {
                    items.Add(new CompletionItem("").WithApplyTexts(CompletionText.Create("expr", caret: true, select: true)));
                }
                else
                {
                    items.Add(new CompletionItem("", beforeText: " ", afterText: " "));
                }
                return true;
            }
            else if (rules.Type == parser)
            {
                items.Add(new CompletionItem("").WithApplyTexts(CompletionText.Create("real", caret: true, select: true)));
                return true;
            }
            else if (rules.FunctionDeclaration == parser)
            {
                items.Add(new CompletionItem("() { }"));
                return true;
            }
            else if (rules.FunctionBody == parser
                || queryGrammar.FunctionBody == parser)
            {
                items.Add(new CompletionItem("{ }"));
                return true;
            }
            else if (rules.AnyGuidLiteralOrString == parser
                || rules.GuidLiteral == parser
                || rules.StringLiteral == parser
                || rules.RawGuidLiteral == parser
                || queryGrammar.StringLiteral == parser)
            {
                items.Add(new CompletionItem("\"\"", beforeText: "\"", afterText: "\""));
                return true;
            }
            else if (rules.BracketedStringLiteral == parser
                || rules.BracketedInputText == parser)
            {
                items.Add(new CompletionItem("[]", beforeText: "[", afterText: "]"));
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsStartBracket(string text) =>
            text == "(" || text == "[" || text == "{";

        private static string GetEndBracket(string startBracket) =>
            startBracket == "(" ? ")"
            : startBracket == "[" ? "]"
            : startBracket == "{" ? "}"
            : null;

        /// <summary>
        /// Gets the <see cref="CompletionItem"/> annotated on this parser or null.
        /// </summary>
        private static CompletionItem GetCompletionItem(Parser<LexicalToken> parser) =>
            parser.Annotations.OfType<CompletionItem>().FirstOrDefault() is CompletionItem item ? item : null;

        /// <summary>
        /// Creates a <see cref="CompletionItem"/> from a list of <see cref="CompletionText"/>
        /// </summary>
        private static CompletionItem CreateExtendedSyntaxCompletion(IReadOnlyList<CompletionItem> items)
        {
            var texts = items.SelectMany(item => item.ApplyTexts).ToList();
            var combinedTexts = CombineTexts(texts);

            var displayText = items.Aggregate("", (s, item) => CombineWithSpacing(s, item.DisplayText));

            // remove all whitespace from text matching to aid early completion for non-extended completions
            var matchText = displayText.Filter(c => !TextFacts.IsWhitespace(c));
            
            return new CompletionItem(items[0].Kind, displayText, matchText: matchText)
                .WithApplyTexts(combinedTexts)
                .WithRank(CompletionRank.Other)
                .WithPriority(CompletionPriority.Low);
        }

        /// <summary>
        /// Combines texts together with spacing if necessary.
        /// </summary>
        private static IReadOnlyList<CompletionText> CombineTexts(IReadOnlyList<CompletionText> texts)
        {
            var results = new List<CompletionText>();

            for (int i = 0; i < texts.Count; i++)
            {
                var current = texts[i];

                if (results.Count == 0)
                {
                    results.Add(current);
                }
                else
                {
                    var prev = results[results.Count - 1];
                    if (!prev.Caret 
                        && !current.Caret)
                    {
                        // combine fixed texts that are not caret positions
                        results[results.Count - 1] = CompletionText.Create(CombineWithSpacing(prev.Text, current.Text));
                    }
                    else if (NeedsSpacing(prev.Text, current.Text))
                    {
                        if (!current.Caret)
                        {
                            results.Add(CompletionText.Create(" " + current.Text));
                        }
                        else if (!prev.Caret)
                        {
                            results[results.Count - 1] = CompletionText.Create(prev.Text + " ");
                            results.Add(current);
                        }
                        else
                        {
                            results.Add(current);
                        }
                    }
                    else
                    {
                        results.Add(current);
                    }
                }
            }

            return results;
        }

        private static bool NeedsSpacing(string prev, string next)
        {
            return prev.Length > 0
                && next.Length > 0
                && NeedsSpacing(prev[prev.Length - 1], next[0]);
        }

        private static bool NeedsSpacing(char prev, char next)
        {
            if (IsDoubleTokenChar(prev) || IsDoubleTokenChar(next))
                return true;
            if (char.IsPunctuation(prev) && char.IsPunctuation(next))
                return false;
            if (char.IsWhiteSpace(prev) || char.IsWhiteSpace(next))
                return false;
            return true;
        }

        private static bool IsDoubleTokenChar(char ch) =>
            ch == '=' || ch == '<' || ch == '>' || ch == '!' || ch == '~';

        private static string CombineWithSpacing(string prev, string next)
        {
            if (NeedsSpacing(prev, next))
            {
                return prev + " " + next;
            }
            else
            {
                return prev + next;
            }
        }

        /// <summary>
        /// Gets the next parser path in lexical sequence.
        /// </summary>
        private static ParserPath<LexicalToken> GetNextParserInSequence(
            ParserPath<LexicalToken> path, 
            ParserPath<LexicalToken> ceiling)
        {
            var current = path;

            // find the sequence the parser is in
            while (TryGetFirstAncestorSequence(current, ceiling, out var ancestor, out var indexInAncestor))
            {
                if (indexInAncestor + 1 < ancestor.ChildCount)
                {
                    return ancestor.GetChild(indexInAncestor + 1);
                }

                current = ancestor;
            }

            return null;
        }

        /// <summary>
        /// Finds the first ancestor path that has a sequence parser.
        /// </summary>
        private static bool TryGetFirstAncestorSequence(
            ParserPath<LexicalToken> path, 
            ParserPath<LexicalToken> ceiling,
            out ParserPath<LexicalToken> ancestor,
            out int indexInAncestor)
        {
            return TryGetFirstAncestor(path, ceiling, p => p.Parser.IsSequence, out ancestor, out indexInAncestor);
        }

        /// <summary>
        /// Finds the first ancestor path that has a conditional parser.
        /// </summary>
        private static bool TryGetFirstAncestorConditional(
            ParserPath<LexicalToken> path,
            ParserPath<LexicalToken> ceiling,
            out ParserPath<LexicalToken> ancestor,
            out int indexInAncestor)
        {
            return TryGetFirstAncestor(path, ceiling, p => p.Parser.IsConditional, out ancestor, out indexInAncestor);
        }

        /// <summary>
        /// Finds the first ancestor path that matches.
        /// </summary>
        /// <param name="path">The path to find the ancestor path for.</param>
        /// <param name="ceiling">The top-most path that we don't search beyond.</param>
        /// <param name="fnMatch">A function to match the path.</param>
        /// <param name="ancestor">The first matching ancestor path</param>
        /// <param name="indexInAncestor">The index in the ancestor that corresponds to the starting path.</param>
        /// <returns></returns>
        private static bool TryGetFirstAncestor(
            ParserPath<LexicalToken> path,
            ParserPath<LexicalToken> ceiling,
            Func<ParserPath<LexicalToken>, bool> fnMatch,
            out ParserPath<LexicalToken> ancestor,
            out int indexInAncestor)
        {
            var child = path;

            int count = 0;

            while (child != null)
            {
                // cannot move up beyond the ceiling
                if (child == ceiling
                    || child.Parent == null)
                    break;

                ancestor = child.Parent;

                if (fnMatch(ancestor))
                {
                    indexInAncestor = child.IndexInParent;
                    return true;
                }

                child = ancestor;

                // cycle detection
                count++;
                if (count > 100)
                    break;
            }

            indexInAncestor = -1;
            ancestor = null;
            return false;
        }

        #endregion
    }
}