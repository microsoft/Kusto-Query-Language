using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Editor
{
    using Binding;
    using Parsing;
    using Symbols;
    using Syntax;
    using Utils;

    internal class KustoCompleter
    {
        private readonly KustoCode code;
        private readonly CompletionOptions options;
        private readonly CancellationToken cancellationToken;

        public KustoCompleter(
            KustoCode code,
            CompletionOptions options, 
            CancellationToken cancellationToken)
        {
            this.code = code;
            this.options = options;
            this.cancellationToken = cancellationToken;
        }

        public CompletionInfo GetCompletionItems(int position)
        {
            var completionToken = GetTokenWithAffinity(position);
            var editStart = completionToken?.TextStart ?? position;
            var editLength = completionToken?.Width ?? 0;

            if (!ShouldComplete(position))
            {
                return new CompletionInfo(EmptyReadOnlyList<CompletionItem>.Instance, editStart, editLength);
            }

            var builder = new CompletionBuilder();
            var mode = CompletionMode.Combined;

            // use editStart instead of actual cursor position to adjust for token affinity
            if (options.IncludeSymbols)
            {
                mode = GetSymbolCompletions(editStart, builder);
            }

            if (mode == CompletionMode.Combined && options.IncludeSyntax)
            {
                GetSyntaxCompletions(editStart, builder);
            }

            var items = builder.ToList();

            // order completions by rank, priority & display name
            var orderedItems = items
                 .OrderBy(i => GetOrderingRank(i))
                 .ThenBy(i => i.Priority)
                 .ThenBy(i => i.DisplayText.ToLower())
                 .ToArray();

            return new CompletionInfo(orderedItems, editStart, editLength);
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

            var token = GetTokenLeftOfPosition(position);
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
                    var expr = GetCompleteExpressionLeftOfPosition(position);
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
            var token = this.code.Syntax.GetTokenAt(position);
            var previous = token.GetPreviousToken();
            var affinity = GetTokenWithAffinity(position) ?? token;

            var leftOrSurrounding = (position == token.TriviaStart)
                ? (previous ?? token) : token;

            // don't show completions if position is inside a literal
            if (IsInsideLiteral(leftOrSurrounding, position))
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

        private static bool IsInsideComment(string trivia, int position)
        {
            for (int p = 0; p < trivia.Length; p++)
            {
                if (position == p)
                    break;

                var ch = trivia[p];

                // start of comment?  any non-whitespace in trivia is start of a comment
                if (!TextFacts.IsWhitespace(ch))
                {
                    var start = p;
                    var end = p;
                    bool hasLineBreak = false;

                    for (; end < trivia.Length; end++)
                    {
                        var lbLen = TextFacts.GetLineBreakLength(trivia, end);
                        if (lbLen > 0)
                        {
                            end += lbLen;
                            hasLineBreak = true;
                            break;
                        }
                    }

                    p = end - 1;

                    // is position inside this comment?
                    if ((hasLineBreak && position >= start && position < end) ||
                        (!hasLineBreak && position >= start))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsInsideLiteral(SyntaxToken token, int position)
        {
            return token.Kind.GetCategory() == SyntaxCategory.Literal
                && (position >= token.TextStart && position <= token.End);
        }

        private enum OrderingRank
        {
            Literal = 0,
            Aggregate = Literal + 1,
            Column = Aggregate + 1,
            Table = Column + 1,
            Variable = Table + 1,
            Function = Variable + 1,
            MaterializedView = Function + 1,
            Keyword = MaterializedView + 1,
            StringOperator = Keyword + 1,
            MathOperator = StringOperator + 1,
            Other = MathOperator + 1
        }

        private OrderingRank GetOrderingRank(CompletionItem item)
        {
            // allow for cancellation during ordering of completion items
            this.cancellationToken.ThrowIfCancellationRequested();

            switch (item.Kind)
            {
                case CompletionKind.Example:
                    return OrderingRank.Literal;

                case CompletionKind.QueryPrefix:
                    return OrderingRank.Keyword;

                case CompletionKind.Keyword:
                    return OrderingRank.Keyword;

                case CompletionKind.AggregateFunction:
                    return OrderingRank.Aggregate;

                case CompletionKind.Column:
                    return OrderingRank.Column;

                case CompletionKind.Table:
                    return OrderingRank.Table;

                case CompletionKind.MaterialiedView:
                    return OrderingRank.MaterializedView;

                case CompletionKind.Variable:
                case CompletionKind.Parameter:
                    if (item.DisplayText == "$left" || item.DisplayText == "$right")
                    {
                        return OrderingRank.Literal;
                    }
                    return OrderingRank.Variable;

                case CompletionKind.BuiltInFunction:
                case CompletionKind.LocalFunction:
                case CompletionKind.DatabaseFunction:
                    return OrderingRank.Function;

                case CompletionKind.ScalarInfix:
                    if (char.IsLetterOrDigit(item.DisplayText[0]))
                    {
                        return OrderingRank.StringOperator;
                    }
                    return OrderingRank.MathOperator;

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
                    return OrderingRank.Other;
            }
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

        private CompletionMode GetSymbolCompletions(int position, CompletionBuilder builder)
        {
            CompletionHint hint = CompletionHint.None;

            if (TryGetCompletionContext(position, out var contextNode, out var contextChildIndex))
            {
                var mode = GetSpecialCaseCompletions(position, contextNode, contextChildIndex, builder);
                if (mode == CompletionMode.Isolated)
                    return mode;

                hint = GetCompletionHint(contextNode, contextChildIndex);
            }

            var symbols = new List<Symbol>();

            var match = GetSymbolMatch(position);

            if (options.IncludeFunctions == IncludeFunctionKind.None)
            {
                match &= ~SymbolMatch.Function;
            }

            var isCommand = IsInCommand(contextNode, contextChildIndex);

            var include = options.IncludeFunctions;

            if (isCommand)
            {
                // commands only reference database functions
                include &= IncludeFunctionKind.DatabaseFunctions;
            }

            Binder.GetSymbolsInScope(this.code.Tree, position, this.code.Globals, match, include, symbols, this.cancellationToken);

            var isInvoke = IsInvokeFunctionContext(contextNode);
            var rowScope = (isInvoke)
                ? Binder.GetRowScope(this.code.Tree, position, this.code.Globals, this.cancellationToken)
                : null;

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

                var item = GetSymbolCompletionItem(symbol, contextNode, nameOnly: isCommand);

                if (ShouldAugmentSymbolCompletionItem(symbol, hint))
                {
                    item = GetAugmentedCompletionItem(item);
                }

                builder.Add(item);
            }

            return CompletionMode.Combined;
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
                            if (t is TableSymbol ts && Binder.SymbolsAssignable(t, implicitFirstArgumentType))
                                return true;
                        }
                        break;

                    case ParameterTypeKind.Tabular:
                        return implicitFirstArgumentType is TableSymbol;
                }
            }

            return false;
        }

        private bool ShouldAugmentSymbolCompletionItem(Symbol symbol, CompletionHint hint)
        {
            // only append space if the symbol is in a boolean context and is not itself boolean
            return (hint & CompletionHint.Boolean) != 0
                && Symbol.GetExpressionResultType(symbol) != ScalarTypes.Bool;
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

        private CompletionItem GetSymbolCompletionItem(Symbol symbol, SyntaxNode contextNode, bool nameOnly)
        {
            var kind = GetCompletionKind(symbol);
            string editName = KustoFacts.BracketNameIfNecessary(symbol.Name);

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

                    var displayText =
                        addExternalTableFuncText
                        ? $"external_table('{t.Name}')"
                        : t.Name;

                    // Lower ordering priority for external tables so that
                    // “regular” tables are displayed first and then the external ones.
                    var priority = t.IsExternal ? CompletionPriority.Low : CompletionPriority.Normal;

                    // Enables the user to be able to just type the name of the external table
                    // and not the entire expression (including the external_table('...') part)
                    var matchText = t.Name;

                    if (IsStartOfQuery(contextNode))
                    {
                        return new CompletionItem(
                            kind,
                            displayText,
                            insertionText + AfterQueryStart, // add | for start of query
                            matchText: matchText,
                            priority: priority);
                    }
                    else
                    {
                        return new CompletionItem(
                            kind,
                            displayText,
                            insertionText,
                            matchText: matchText,
                            priority: priority);
                    }

                case FunctionSymbol f:
                    if (nameOnly)
                    {
                        return new CompletionItem(kind, f.Name, editName);
                    }

                    var builtIn = this.code.Globals.IsBuiltInFunction(f);
                    if (builtIn)
                    {
                        // built-in functions don't need to be escaped even if they are keywords
                        editName = f.Name;
                    }

                    var fdisplay = f.Display;

                    if (f.Signatures.Max(s => s.Parameters.Count) == 0)
                    {
                        if (builtIn)
                        {
                            editName = editName + "()";
                        }

                        if (IsStartOfQuery(contextNode))
                        {
                            return new CompletionItem(kind, fdisplay, editName + AfterQueryStart, matchText: f.Name);
                        }
                        else
                        {
                            return new CompletionItem(kind, fdisplay, editName, matchText: f.Name);
                        }
                    }
                    else
                    {
                        var isInvoke = IsInvokeFunctionContext(contextNode);

                        if (this.options.EnableParameterInjection && f.MaxArgumentCount == 1 && !builtIn && !isInvoke)
                        {
                            return new CompletionItem(kind, fdisplay, editName + "({parameter})", matchText: f.Name);
                        }
                        else
                        {
                            return new CompletionItem(kind, fdisplay, editName + "(", ")", matchText: f.Name);
                        }
                    }

                case PatternSymbol p:
                    return new CompletionItem(kind, p.Display, editName + "(", ")", matchText: p.Name);

                case VariableSymbol v:
                    if (v.Type is FunctionSymbol)
                    {
                        return GetSymbolCompletionItem(v.Type, contextNode, nameOnly);
                    }
                    else if (v.Type is TableSymbol && IsStartOfQuery(contextNode))
                    {
                        return new CompletionItem(kind, v.Name, editName + AfterQueryStart);
                    }
                    else
                    {
                        return new CompletionItem(kind, v.Name, editName);
                    }

                case ParameterSymbol p:
                    if (p.Type is FunctionSymbol)
                    {
                        return GetSymbolCompletionItem(p.Type, contextNode, nameOnly);
                    }
                    else
                    {
                        return new CompletionItem(kind, symbol.Name, editName);
                    }

                case DatabaseSymbol d:
                    return new CompletionItem(CompletionKind.Database, d.Name, editName);

                case ClusterSymbol cl:
                    return new CompletionItem(CompletionKind.Cluster, cl.Name, KustoFacts.GetBracketedName(cl.Name));

                case OptionSymbol opt:
                    return new CompletionItem(CompletionKind.Option, opt.Name, editName);

                default:
                    return new CompletionItem(kind, symbol.Name, editName);
            }
        }

        private static bool IsInvokeFunctionContext(SyntaxNode node)
        {
            // check to see if an ancestor is the invoke operator and not within an existing invoke function argument
            var op = node?.GetFirstAncestorOrSelf<QueryOperator>();
            var fc = node?.GetFirstAncestorOrSelf<FunctionCallExpression>();
            return op is InvokeOperator && (fc == null || fc.TextStart < op.TextStart);
        }

        private static bool IsNameLikeToken(SyntaxKind kind)
        {
            switch (kind.GetCategory())
            {
                case SyntaxCategory.Identifier:
                case SyntaxCategory.Keyword:
                    return true;
                default:
                    // ! can be start of some keywords
                    return kind == SyntaxKind.BangToken;
            }
        }

        internal static bool HasAffinity(SyntaxToken token, int position)
        {
            return (position > token.TextStart && position < token.End)
                || position == token.TextStart && IsNameLikeToken(token.Kind)
                || position == token.End && IsNameLikeToken(token.Kind);
        }

        /// <summary>
        /// Get the token that the position has affinity with,
        /// or null if the position does not have affinity (in whitespace between tokens).
        /// </summary>
        private SyntaxToken GetTokenWithAffinity(int position)
        {
            var token = this.code.Syntax.GetTokenAt(position);
            var previous = token.GetPreviousToken();

            if (HasAffinity(token, position))
            {
                return token;
            }
            else if (previous != null && HasAffinity(previous, position))
            {
                return previous;
            }
            else
            {
                // fully inside trivia between tokens, no affinity
                return null;
            }
        }

        private SymbolMatch GetSymbolMatch(int position)
        {
            SymbolMatch match = SymbolMatch.None;

            if (TryGetCompletionContext(position, out var contextNode, out var index))
            {
                var hint = GetCompletionHint(contextNode, index);
                match |= GetSymbolMatch(hint);
            }

            // special case for parenthesis; get hint from outside
            while (contextNode is ParenthesizedExpression && contextNode.Parent != null)
            {
                var hint = GetCompletionHint(contextNode.Parent, contextNode.IndexInParent);
                match |= GetSymbolMatch(hint);
                position = contextNode.TextStart;
                contextNode = contextNode.Parent;
            }

            var grammarMatch = GetSymbolMatchFromGrammar(position);
            match |= grammarMatch;

            return match;
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

            return mode;
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
                    if (argumentIndex == 0
                        && TryGetReturnTypeKindCompletions(sig.ReturnKind, position, contextNode, builder))
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

        private bool TryGetReturnTypeKindCompletions(ReturnTypeKind kind, int position, SyntaxNode contextNode, CompletionBuilder builder)
        {
            switch (kind)
            {
                case ReturnTypeKind.Parameter0Cluster:
                    // show the known cluster names
                    builder.AddRange(GetMemberNameExamples(this.code.Globals.Clusters));
                    GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                    return true;

                case ReturnTypeKind.Parameter0Database:
                    // show either the dotted cluster's database names or the global cluster's database names
                    if (GetLeftOfFunctionCall(contextNode) is Expression cpl)
                    {
                        if (cpl.ResultType is ClusterSymbol cc)
                        {
                            builder.AddRange(GetMemberNameExamples(cc.Databases));
                        }
                    }
                    else
                    {
                        builder.AddRange(GetMemberNameExamples(this.code.Globals.Cluster.Databases));
                    }

                    GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                    return true;

                case ReturnTypeKind.Parameter0Table:
                    // show either the dotted database's table names or the global database's table names
                    if (GetLeftOfFunctionCall(contextNode) is Expression dpl)
                    {
                        if (dpl.ResultType is DatabaseSymbol ds)
                        {
                            builder.AddRange(GetMemberNameExamples(ds.Tables));
                        }
                    }
                    else
                    {
                        builder.AddRange(GetMemberNameExamples(this.code.Globals.Database.Tables));
                    }

                    GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                    return true;

                case ReturnTypeKind.Parameter0ExternalTable:
                    // show either the dotted database's table names or the global database's table names
                    if (GetLeftOfFunctionCall(contextNode) is Expression edpl)
                    {
                        if (edpl.ResultType is DatabaseSymbol ds)
                        {
                            builder.AddRange(GetMemberNameExamples(ds.ExternalTables));
                        }
                    }
                    else
                    {
                        builder.AddRange(GetMemberNameExamples(this.code.Globals.Database.ExternalTables));
                    }

                    GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                    return true;

                case ReturnTypeKind.Parameter0MaterializedView:
                    // show either the dotted database's table names or the global database's table names
                    if (GetLeftOfFunctionCall(contextNode) is Expression mvdpl)
                    {
                        if (mvdpl.ResultType is DatabaseSymbol ds)
                        {
                            builder.AddRange(GetMemberNameExamples(ds.MaterializedViews));
                        }
                    }
                    else
                    {
                        builder.AddRange(GetMemberNameExamples(this.code.Globals.Database.MaterializedViews));
                    }

                    GetMatchingSymbolCompletions(SymbolMatch.Local, ScalarTypes.String, position, contextNode, builder);
                    return true;

                default:
                    return false;
            }
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
        /// Get's completion items for local variables in scope that are type requested type
        /// </summary>
        private void GetMatchingSymbolCompletions(SymbolMatch match, ScalarSymbol type, int position, SyntaxNode contextNode, CompletionBuilder builder)
        {
            // first get all items in scope
            var symbols = new List<Symbol>();
            Binder.GetSymbolsInScope(this.code.Tree, position, this.code.Globals, match, IncludeFunctionKind.All, symbols, this.cancellationToken);

            // get completion items for the symbols with a matching scalar type
            foreach (var symbol in symbols)
            {
                if (GetScalarType(symbol) == type)
                {
                    var item = GetSymbolCompletionItem(symbol, contextNode, nameOnly: false);
                    builder.Add(item);
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

        private static SyntaxNode GetLeftOfFunctionCall(SyntaxNode expression)
        {
            var parent = expression.Parent;

            if (parent is ExpressionList el)
                parent = el.Parent;

            if (parent is FunctionCallExpression fc)
                parent = fc.Parent;

            if (parent is PathExpression pe)
            {
                return pe.Expression;
            }

            return null;
        }

        private IEnumerable<CompletionItem> GetMemberNameExamples(IEnumerable<Symbol> symbols)
        {
            return symbols.Select(s => new CompletionItem(CompletionKind.Example, KustoFacts.GetStringLiteral(s.Name)));
        }

        private bool ShowParameterNames(FunctionSymbol function)
        {
            return !this.code.Globals.IsBuiltInFunction(function) && function.MaxArgumentCount >= 2;
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

            if ((hint & CompletionHint.Tabular) != 0)
                match |= SymbolMatch.Tabular | SymbolMatch.Table | SymbolMatch.ExternalTable | SymbolMatch.MaterializedView | SymbolMatch.Function | SymbolMatch.Local;

            if ((hint & CompletionHint.Expression) != 0)
                match |= SymbolMatch.Tabular | SymbolMatch.Table | SymbolMatch.ExternalTable | SymbolMatch.MaterializedView | SymbolMatch.Function | SymbolMatch.Local | SymbolMatch.Scalar | SymbolMatch.Column;

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

            if ((hint & CompletionHint.Option) != 0)
                match |= SymbolMatch.Option;

            return match;
        }

        /// <summary>
        /// Gets the <see cref="CompletionHint"/> at the specified text position.
        /// </summary>
        private CompletionHint GetCompletionHint(int position)
        {
            if (TryGetCompletionContext(position, out var contextNode, out var index))
            {
                return GetCompletionHint(contextNode, index, CompletionHint.Query);
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
        private CompletionHint GetCompletionHint(SyntaxNode contextNode, int childIndex, CompletionHint defaultHint = CompletionHint.None)
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
                while (contextNode != null)
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
                    contextNode = parent;
                    childIndex = indexInParent;
                }
            }

            return hint;
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
                        return CompletionHint.Column;
                    case ArgumentKind.Star:
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
                        else if (parameter.DeclaredTypes[0].IsTabular)
                        {
                            return CompletionHint.Tabular;
                        }
                        break;
                    case ParameterTypeKind.Parameter0:
                        return GetParameterHint(signature, signature.Parameters.Count > 0 ? signature.Parameters[0] : null);
                    case ParameterTypeKind.Parameter1:
                        return GetParameterHint(signature, signature.Parameters.Count > 1 ?  signature.Parameters[1] : null);
                    case ParameterTypeKind.Parameter2:
                        return GetParameterHint(signature, signature.Parameters.Count > 2 ? signature.Parameters[2] : null);
                    case ParameterTypeKind.Tabular:
                        return CompletionHint.Tabular;
                    case ParameterTypeKind.Database:
                        return CompletionHint.Database;
                    case ParameterTypeKind.Cluster:
                        return CompletionHint.Cluster;
                    case ParameterTypeKind.Number:
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

            SyntaxToken token = this.code.Syntax.GetTokenAt(position);

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

                contextNode = token.Parent;
                contextChildIndex = GetChildIndex(contextNode, position);
                return true;
            }

            var tokenNode = GetNearestAncestorWithEmptyChild(token, position);

            if (position <= token.TextStart && !hasAffinity || token.Kind == SyntaxKind.EndOfTextToken)
            {
                var prevToken = token.GetPreviousToken();
                if (prevToken != null)
                {
                    var prevNode = GetNearestAncestorWithEmptyChild(prevToken, position);
                    if (prevNode != null)
                    {
                        if (tokenNode == null || prevNode.Depth >= tokenNode.Depth)
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
                        if (tokenNode == null || nextNode.Depth > tokenNode.Depth)
                        {
                            contextNode = nextNode;
                            contextChildIndex = GetChildIndex(contextNode, position);
                            return true;
                        }
                    }
                }
            }

            if (tokenNode != null)
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


        private void GetSyntaxCompletions(int position, CompletionBuilder builder)
        {
            var hints = GetCompletionHint(position);
            var match = GetSymbolMatch(position);
            var expr = GetCompleteExpressionLeftOfPosition(position);

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
                        var augmentedItem = GetAugmentedCompletionItem(item);
                        builder.Add(augmentedItem);
                    }
                    else
                    {
                        builder.Add(item);
                    }
                }
            }
        }

        private CompletionItem GetAugmentedCompletionItem(CompletionItem item)
        {
            return item.WithEditText(item.EditText + " ");
        }

        private static IReadOnlyList<string> punctuationWithoutSpace =
            new[] { "(", "[", "{", "*", "@", "$" };

        private bool ShouldAugmentSyntaxCompletionItem(CompletionItem item)
        {
            if (!options.AutoAppendWhitespace)
                return false;

            if (item.AfterText?.Length > 0)
                return false;

            // was this an auto appended '='?
            // make appended additional space depend on whether it is currently separated by a space.
            if (item.EditText.EndsWith("=")
                && char.IsLetterOrDigit(item.EditText[0])
                && item.EditText.Length > 1
                && !char.IsWhiteSpace(item.EditText[item.EditText.Length - 2]))
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

            var parsers = new List<Parser<LexicalToken>>();

            // look for completions in the grammar elements corresponding to the text position
            GetAnnotatedParsers(position, parsers);

            _annotations = parsers.SelectMany(p => p.Annotations).ToList();
            _annotationPosition = position;

            return _annotations;
        }

        /// <summary>
        /// Gets a list of all annotated parsers that are invoked for the token at the specified text position.
        /// </summary>
        private void GetAnnotatedParsers(int position, List<Parser<LexicalToken>> annotatedParsers)
        {
            // find a possible better starting point than the start of the whole query
            GetGrammarSearchContext(position, out var startPosition, out var grammar);

            var startIndex = this.code.GetTokenIndex(startPosition);
            var matchIndex = this.code.GetTokenIndex(position);

            // use a source that only includes relevant tokens (so we dont bother with tokens beyond)
            var tokens = this.code.GetLexicalTokens();
            var source = new ArraySource<LexicalToken>(tokens, 0, matchIndex + 1);

            // search through the grammar until we get to all the parsers that are applied to the token at the matching index
            grammar.Search(source, startIndex, false, (_parser, _source, _start, _prevWasMissing) =>
            {
                if (_start == matchIndex
                    && !_prevWasMissing // don't consider parsers that were invoked immediately after a missing value
                    && _parser.Annotations.Count > 0)
                {
                    annotatedParsers.Add(_parser);
                }
            });
        }

        /// <summary>
        /// Find a nearby starting point (contextNode) and grammar rule to search with
        /// </summary>
        private void GetGrammarSearchContext(int position, out int searchStart, out Parser<LexicalToken> grammar)
        {
            var token = GetTokenLeftOfPosition(position);

            if (token != null)
            {
                var queryGrammar = QueryGrammar.From(GlobalState.Default);
                var node = token.Parent;

                for (; node != null; node = node.Parent)
                {
                    switch (node)
                    {
                        case QueryOperator op:
                            searchStart = op.TextStart;

                            // the query operator on right of pipe
                            if (op.Parent is PipeExpression pipe && pipe.Operator == op)
                            {
                                grammar = queryGrammar.PipeSubExpression;
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
                            if (position > fc.Name.End)
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
                            grammar = code.Grammar; // use entire command block grammar
                            return;
                    }
                }
            }

            // otherwise use the grammar associated with the entire source
            searchStart = this.code.Syntax.TextStart;
            grammar = this.code.Grammar;
        }

        private SyntaxToken GetTokenLeftOfPosition(int position)
        {
            var token = this.code.Syntax.GetTokenAt(position);
            var hasAffinity = token != null && HasAffinity(token, position);

            if (token != null && (position <= token.TextStart || !hasAffinity || token.Kind == SyntaxKind.EndOfTextToken))
            {
                token = token.GetPreviousToken();
            }

            return token;
        }

        private Expression GetCompleteExpressionLeftOfPosition(int position)
        {
            var token = GetTokenLeftOfPosition(position);

            var expr = token?.GetFirstAncestorOrSelf<Expression>();
            if (expr != null && expr.End == token.End && !expr.HasMissingChildren())
                return expr;

            return null;
        }

        private bool IncludeSyntax(CompletionItem item, int position, CompletionHint hints, SymbolMatch match, Expression left)
        {
            if (!options.IncludePunctuationOnlySyntax 
                && item.Kind == CompletionKind.Punctuation)
                return false;

            switch (item.Kind)
            {
                case CompletionKind.QueryPrefix:
                    return (hints & CompletionHint.Query) != 0
                        || (hints & CompletionHint.Keyword) != 0
                        || (match & SymbolMatch.Tabular) != 0;
                case CompletionKind.TabularPrefix:
                    return (match & SymbolMatch.Tabular) != 0;
                case CompletionKind.TabularSuffix:
                    return left != null && left.ResultType != null && left.ResultType.IsTabular;
                case CompletionKind.ScalarPrefix:
                case CompletionKind.Example:
                    return (match & SymbolMatch.Scalar) != 0;
                case CompletionKind.ScalarInfix:
                    return AnyInfixMatches(left, item.DisplayText, position);
                default:
                    return true;
            }
        }

        private static bool HasLetters(string text)
        {
            foreach (var ch in text)
            {
                if (char.IsLetter(ch))
                    return true;
            }

            return false;
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
            var op = this.code.Globals.GetOperator(kind);

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
                case ParameterTypeKind.Declared:
                    if (Binder.SymbolsAssignable(parameter.DeclaredTypes, type))
                    {
                        return true;
                    }
                    else if (Binder.IsPromotable(type, parameter.DeclaredTypes[0]))
                    {
                        return true;
                    }
                    break;

                case ParameterTypeKind.Scalar:
                    return type.IsScalar;

                case ParameterTypeKind.Integer:
                    return type is ScalarSymbol s && s.IsInteger;

                case ParameterTypeKind.RealOrDecimal:
                    return type == ScalarTypes.Real || type == ScalarTypes.Decimal;

                case ParameterTypeKind.StringOrDynamic:
                    return type == ScalarTypes.String || type == ScalarTypes.Dynamic;

                case ParameterTypeKind.Number:
                    return type is ScalarSymbol s2 && s2.IsNumeric;

                case ParameterTypeKind.Summable:
                    return type is ScalarSymbol s3 && s3.IsSummable;

                case ParameterTypeKind.Orderable:
                    return type is ScalarSymbol s4 && s4.IsOrderable;

                case ParameterTypeKind.Tabular:
                    return type.IsTabular;

                case ParameterTypeKind.Database:
                    return type is DatabaseSymbol;

                case ParameterTypeKind.Cluster:
                    return type is ClusterSymbol;

                case ParameterTypeKind.NotBool:
                    return type is ScalarSymbol && type != ScalarTypes.Bool;

                case ParameterTypeKind.NotRealOrBool:
                    return type is ScalarSymbol && type != ScalarTypes.Real && type != ScalarTypes.Bool;

                case ParameterTypeKind.NotDynamic:
                    return type is ScalarSymbol && type != ScalarTypes.Dynamic;

                case ParameterTypeKind.IntegerOrDynamic:
                    return (type is ScalarSymbol s5 && s5.IsInteger) || type == ScalarTypes.Dynamic;

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
                    if (this.code.Globals.IsAggregateFunction(fn))
                    {
                        return CompletionKind.AggregateFunction;
                    }
                    else if (this.code.Globals.IsBuiltInFunction(fn))
                    {
                        return CompletionKind.BuiltInFunction;
                    }
                    else if (this.code.Globals.IsDatabaseFunction(fn))
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
                case SymbolKind.Tuple:
                case SymbolKind.Scalar:
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
                    return GetCompletionKind(vs.Type);
                default:
                    return CompletionKind.Variable;
            }
        }

        private class CompletionBuilder
        {
            private readonly List<CompletionItem> list = new List<CompletionItem>();
            private readonly Dictionary<string, int> indexMap = new Dictionary<string, int>();

            public CompletionBuilder()
            {
            }

            public IReadOnlyList<CompletionItem> ToList() => list.AsReadOnly();

            public void Add(CompletionItem item)
            {
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
    }
}