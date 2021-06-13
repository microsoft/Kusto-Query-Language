using System;
using System.Collections.Generic;
using System.Text;

namespace Kusto.Language.Editor
{
    using Syntax;
    using Parsing;

    /// <summary>
    /// Rewrites whitespace to conform to formatting options.
    /// </summary>
    internal class KustoFormatter
    {
        private readonly FormattingOptions _options;
        private readonly StringBuilder _builder;
        private readonly int _cursorPosition;
        private int _newCursorPosition;

        private readonly Dictionary<SyntaxElement, SpacingRule> _spacingRules =
            new Dictionary<SyntaxElement, SpacingRule>();

        private readonly Dictionary<SyntaxElement, AlignmentRule> _alignmentRules =
            new Dictionary<SyntaxElement, AlignmentRule>();

        private readonly Dictionary<SyntaxElement, int> _elementIndentations =
            new Dictionary<SyntaxElement, int>();

        internal KustoFormatter(int cursorPosition, FormattingOptions options)
        {
            _cursorPosition = cursorPosition;
            _newCursorPosition = -1;
            _options = options;
            _builder = new StringBuilder();
        }

        /// <summary>
        /// Gets the formatted text for the node.
        /// </summary>
        public static FormattedText GetFormattedText(SyntaxNode node, FormattingOptions options, int cursorPosition)
        {
            var formatter = new KustoFormatter(cursorPosition, options ?? FormattingOptions.Default);
            formatter.Format(node);
            return new FormattedText(formatter._builder.ToString(), formatter._newCursorPosition);
        }

        private void Format(SyntaxNode node)
        {
            // identify formatting rules for all nodes and tokens
            IdentifyFormattingRules(node);

            // write out text by applying the formatting rules
            WriteFormattedText(node, 0);

            // adjust cursor position if not already chosen,
            // as it may need to move if it was inside the formatted area.
            if (_newCursorPosition == -1)
            {
                if (_cursorPosition <= 0)
                {
                    // position was logically before this text, leave it alone
                    _newCursorPosition = _cursorPosition;
                }
                else if (_cursorPosition <= node.End)
                {
                    // position with within this text, but not already recomputed?
                    // put it at the new end position
                    _newCursorPosition = _builder.Length;
                }
                else
                {
                    // position was logically somewhere after this text, so adjust based on change delta
                    _newCursorPosition = _cursorPosition + (_builder.Length - node.Width);
                }
            }
        }

        #region Writing formatted text
        /// <summary>
        /// Writes the node to text, applying the formatting rules
        /// </summary>
        private void WriteFormattedText(SyntaxNode node, int indentation)
        {
            if (node == null)
                return;

            for (int i = 0, n = node.ChildCount; i < n; i++)
            {
                var child = node.GetChild(i);

                if (child != null)
                {
                    var spacingKind = SpacingKind.AlignOnly;
                    var childIndentation = indentation;

                    if (TryGetSpacingRule(child, out var spacing))
                    {
                        spacingKind = spacing.GetKind();
                    }

                    if (TryGetAlignmentRule(child, out var alignment))
                    {
                        if (alignment.RelativeToElement == null)
                        {
                            childIndentation += alignment.IndentationDelta;
                        }
                        else if (TryGetIndentation(alignment.RelativeToElement, out var elementIndentation))
                        {
                            childIndentation = elementIndentation + alignment.IndentationDelta;
                        }
                    }

                    if (child is SyntaxNode sn)
                    {
                        WriteFormattedText(sn, childIndentation);
                    }
                    else if (child is SyntaxToken t2)
                    {
                        WriteToken(t2, childIndentation, spacingKind);
                        _elementIndentations.Add(t2, childIndentation);
                    }
                }
            }
        }

        private bool TryGetSpacingRule(SyntaxElement element, out SpacingRule rule)
        {
            return _spacingRules.TryGetValue(element, out rule);
        }

        private bool TryGetAlignmentRule(SyntaxElement element, out AlignmentRule rule)
        {
            return _alignmentRules.TryGetValue(element, out rule);
        }

        private bool TryGetIndentation(SyntaxElement element, out int indentation)
        {
            if (_elementIndentations.TryGetValue(element, out indentation))
                return true;

            // if no specific indentation is recorded for a node,
            // use the indentation of its first token
            if (element is SyntaxNode node)
            {
                var token = element.GetFirstToken();

                if (token != null && _elementIndentations.TryGetValue(token, out indentation))
                    return true;
            }

            return false;
        }

        private void WriteToken(SyntaxToken token, int indentation, SpacingKind spacingKind = SpacingKind.AsIs)
        {
            if (token.Text.Length > 0 || token.Trivia.Length > 0 || (token.IsMissing && _options.InsertMissingTokens))
            {
                WriteTrivia(token, indentation, spacingKind, token.Kind != SyntaxKind.EndOfTextToken);
            }

            if (_newCursorPosition == -1)
            {
                if (_cursorPosition >= token.TextStart && _cursorPosition <= token.End)
                {
                    _newCursorPosition = _builder.Length + (_cursorPosition - token.TextStart);
                }
                else if (_cursorPosition < token.TextStart)
                {
                    _newCursorPosition = _builder.Length;
                }
            }

            if (token.IsMissing && _options.InsertMissingTokens)
            {
                var text = SyntaxFacts.GetText(token.Kind);

                if (!string.IsNullOrEmpty(text))
                {
                    _builder.Append(text);
                }
            }
            else
            {
                _builder.Append(token.Text);
            }
        }

        private void WriteTrivia(SyntaxToken token, int indentation, SpacingKind spacingKind, bool hasFollowingToken)
        {
            var trivia = token.Trivia;
            var cursorInTrivia = _cursorPosition >= token.TriviaStart && _cursorPosition < token.TextStart;

            switch (spacingKind)
            {
                case SpacingKind.NoSpace:
                    // all spacing is removed
                    return;

                case SpacingKind.SingleSpace:
                    // all spacing is replace by a single space
                    _builder.Append(" ");
                    return;
            }

            if (spacingKind != SpacingKind.AsIs && TextFacts.HasLineBreaks(trivia))
            {
                // adjust and write all trivia lines
                for (int lineStart = 0, lineEnd = 0; lineStart < trivia.Length; lineStart = lineEnd)
                {
                    int whitespaceEnd = SkipWhitespace(trivia, lineStart);

                    if (lineStart == 0)
                    {
                        // write existing whitespace for first line, since this is whitespace that follows the last
                        // token on the last line (may contain trailing comments)
                        _builder.Append(trivia, lineStart, whitespaceEnd - lineStart);
                    }
                    else
                    {
                        // write standardized indentation instead of existing whitespace
                        WriteIndentation(indentation);
                    }

                    // write remainder of line (possible comments and/or EOL)
                    var nextLineBreakStart = TextFacts.GetNextLineBreakStart(trivia, whitespaceEnd);
                    var nextLineStart = TextFacts.GetNextLineStart(trivia, whitespaceEnd);
                    lineEnd = nextLineStart >= 0 ? nextLineStart : trivia.Length;
                    _builder.Append(trivia, whitespaceEnd, lineEnd - whitespaceEnd);

                    // if the last thing in the trivia was a line break, add indentation for following token.
                    if (lineEnd >= trivia.Length && nextLineBreakStart >= 0 && hasFollowingToken)
                    {
                        WriteIndentation(indentation);
                    }
                }
            }
            else
            {
                switch (spacingKind)
                {
                    case SpacingKind.NoSpaceIfOnSameLine:
                        // there was no line break, so make it have no spacing by writing nothing
                        break;

                    case SpacingKind.SingleSpaceIfOnSameLine:
                        // there was no line break, so make it a single space by writing a single space
                        _builder.Append(" ");
                        break;

                    case SpacingKind.NewLine:
                        // there was no line break so add one
                        _builder.AppendLine();
                        WriteIndentation(indentation);
                        break;

                    case SpacingKind.AsIs:
                    case SpacingKind.AlignOnly:
                    default:
                        _builder.Append(trivia);
                        break;
                }
            }
        }

        private static int SkipWhitespace(string text, int start)
        {
            int p = start;

            while (p < text.Length
                && TextFacts.IsWhitespace(text[p])
                && !TextFacts.IsLineBreakStart(text[p]))
            {
                p++;
            }

            return p;
        }

        private static string s_Spaces = new string(' ', 256);

        private void WriteIndentation(int indentation)
        {
            while (indentation > s_Spaces.Length)
            {
                _builder.Append(s_Spaces);
                indentation -= s_Spaces.Length;
            }

            _builder.Append(s_Spaces, 0, Math.Min(indentation, s_Spaces.Length));
        }
#endregion

#region Identifying formatting rules
        /// <summary>
        /// Identify formatting rules for all nodes and tokens.
        /// </summary>
        private void IdentifyFormattingRules(SyntaxNode node)
        {
            if (node != null)
            {
                // visit children first so token rules (being more general) get added first
                // and node rules (being more specific) get added later.
                for (int i = 0, n = node.ChildCount; i < n; i++)
                {
                    var child = node.GetChild(i);
                    if (child is SyntaxNode sn)
                    {
                        IdentifyFormattingRules(sn);
                    }
                    else if (child is SyntaxToken t)
                    {
                        AddTokenRules(t);
                    }
                }

                AddNodeRules(node);
            }
        }

        /// <summary>
        /// Add spacing and alignment rules for individual tokens
        /// </summary>
        private void AddTokenRules(SyntaxToken token)
        {
            // don't adjust spacing if there are already line breaks in the trivia
            if (TextFacts.HasLineBreaks(token.Trivia))
                return;

            // if no previous token then leave as is
            var prev = token.GetPreviousToken();
            if (prev == null)
                return;

            if (IsIdentifierOrKeyword(token) && IsIdentifierOrKeyword(prev))
            {
                // always have space between two adjacent names
                if (token.Trivia != " ")
                {
                    AddRule(token, SpacingRule.From(SpacingKind.SingleSpaceIfOnSameLine));
                }
            }
            else if (token.Parent is BinaryExpression be && be.Operator == token
                || prev.Parent is BinaryExpression pbe && pbe.Operator == prev)
            {
                // space before and after binary operator
                if (token.Trivia != " ")
                {
                    AddRule(token, SpacingRule.From(SpacingKind.SingleSpaceIfOnSameLine));
                }
            }
            else if (prev.Parent is PrefixUnaryExpression pue && pue.Operator == prev)
            {
                // no space after prefix unary operator
                if (token.Trivia != "")
                {
                    AddRule(token, SpacingRule.From(SpacingKind.NoSpaceIfOnSameLine));
                }
            }
            else if (token.Kind != SyntaxKind.EndOfTextToken)
            {
                // spacing before token
                switch (token.Kind)
                {
                    case SyntaxKind.CloseBraceToken:
                    case SyntaxKind.CloseBracketToken:
                    case SyntaxKind.CloseParenToken:
                    case SyntaxKind.CommaToken:
                    case SyntaxKind.ColonToken:
                    case SyntaxKind.SemicolonToken:
                    case SyntaxKind.DotToken:
                    case SyntaxKind.DotDotToken:
                        if (token.Trivia != "")
                        {
                            AddRule(token, SpacingRule.From(SpacingKind.NoSpaceIfOnSameLine));
                        }
                        break;

                    case SyntaxKind.BarToken:
                        if (token.Trivia != " ")
                        {
                            AddRule(token, SpacingRule.From(SpacingKind.SingleSpaceIfOnSameLine));
                        }
                        break;
                }

                // spacing after previous token
                switch (prev.Kind)
                {
                    case SyntaxKind.OpenBraceToken:
                    case SyntaxKind.OpenBracketToken:
                    case SyntaxKind.OpenParenToken:
                    case SyntaxKind.DotDotToken:
                    case SyntaxKind.DotToken:
                        if (token.Trivia != "")
                        {
                            AddRule(token, SpacingRule.From(SpacingKind.NoSpaceIfOnSameLine));
                        }
                        break;

                    case SyntaxKind.CommaToken:
                    case SyntaxKind.ColonToken:
                    case SyntaxKind.BarToken:
                    case SyntaxKind.SemicolonToken:
                        if (token.Trivia != " ")
                        {
                            AddRule(token, SpacingRule.From(SpacingKind.SingleSpaceIfOnSameLine));
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Add spacing and alignment rules specific to types of nodes
        /// </summary>
        private void AddNodeRules(SyntaxNode node)
        {
            AddSubElementRules(node);

            switch (node)
            {
                case PipeExpression pe:
                    AddPipeExpressionRules(pe);
                    break;

                case FunctionDeclaration fd:
                    AddFunctionDeclarationRules(fd);
                    break;

                case SeparatedElement se:
                    // semicolons?
                    if (se.Separator?.Kind == SyntaxKind.SemicolonToken
                        && se.Element is LetStatement)
                    {
                        AddSemicolonRules(se);
                    }
                    break;

                case SyntaxList list:
                    if (list.Count > 1
                        && list[0] is SeparatedElement listElem
                        && listElem.Separator?.Kind == SyntaxKind.CommaToken
                        && IsDirectQueryOperatorPart(list))
                    {
                        AddQueryOperatorCommaListRules(list);
                    }
                    break;

                case BracketedExpression be:
                    AddBracketExpressionRules(be);
                    break;

                case Statement st:
                    AddStatementRules(st);
                    break;

                case DataTableExpression dt:
                    AddDataTableExpressionRules(dt);
                    break;

                case BinaryExpression be:
                    AddBinaryOperatorChainRules(be);
                    break;
            }
        }

        private const int ArbitraryMaxBinaryOperatorChainWidth = 80;

        /// <summary>
        ///  Place operator that is part of an operator chain that is part of a query operator or clause (not nested in parens, etc)
        ///  on new line if the overall chain is large.
        /// </summary>
        private void AddBinaryOperatorChainRules(BinaryExpression be)
        {
            if (IsChainableBinaryOperator(be.Kind)
                && IsDirectQueryOperatorPart(be)
                && !SpansMultipleLines(be)
                && be.Width > ArbitraryMaxBinaryOperatorChainWidth)
            {
                var depth = GetBinaryOperatorChainDepth(be);
                if (depth > 1)
                {
                    var op = be;
                    while (op != null && op.Kind == be.Kind)
                    {
                        AddRule(op.Operator, SpacingRule.From(SpacingKind.NewLine));
                        op = op.Left as BinaryExpression;
                    }
                }
            }
        }

        private static bool IsDirectQueryOperatorPart(SyntaxNode node)
        {
            return node.Parent is QueryOperator 
                || node.Parent is Clause;
        }

        private static bool IsChainableBinaryOperator(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.AndExpression:
                case SyntaxKind.OrExpression:
                //case SyntaxKind.AddExpression:
                    return true;
                default:
                    return false;
            }

        }

        private static int GetBinaryOperatorChainDepth(BinaryExpression be)
        {
            int count = 1;

            while (be != null
                && be.Left is BinaryExpression left 
                && left.Operator.Kind == be.Operator.Kind)
            {
                count++;
                be = left;
            }

            return count;
        }

        private void AddSubElementRules(SyntaxNode n)
        {
            if ((n.Parent is Statement && !(n.Parent is ExpressionStatement))
                || n.Parent is QueryOperator
                || n.Parent is Command)
            {
                // sub elements of statements/commands/query-operators are all indented.
                if (n.IndexInParent > 0) // except for first token
                {
                    AddRule(n, IndentRule());
                }

                // start all clauses on new line if there are any new lines in statement/query-operator/command
                if (n is Clause)
                {
                    AddRule(n.GetFirstToken(),
                        new SpacingRule(SpacingKind.NewLine, () => SpansOrWillSpanMultipleLines(n.Parent)));
                }
            }
        }

        private const int ArbitraryMaxSingleLineQueryWidth = 80;

        private void AddPipeExpressionRules(PipeExpression pe)
        {
            if (pe.Parent is PipeExpression)
                return;

            var entireQuery = pe;

            var style = _options.PipeOperatorStyle;

            if (!SpansMultipleLines(entireQuery) && entireQuery.Width > ArbitraryMaxSingleLineQueryWidth)
                style = PlacementStyle.NewLine;

            for (;  pe != null; pe = pe.Expression as PipeExpression)
            {
                var barToken = pe.Bar;

                switch (style)
                {
                    case PlacementStyle.Smart:
                        // place this pipe expression's | on a new line if there are any new lines in the whole expression
                        AddRule(barToken, new SpacingRule(SpacingKind.NewLine, () => SpansOrWillSpanMultipleLines(entireQuery, excluded: barToken)));

                        // Adjust the spacing of the operator's first token so it snaps to the |'s placement.
                        var opToken = pe.Operator.GetFirstToken();
                        AddRule(opToken, SpacingRule.From(SpacingKind.SingleSpace));

                        // adjust first token to new line if query is part of a parenthesized expression
                        if (entireQuery == pe)
                        {
                            var firstToken = pe.GetFirstToken();
                            var prevToken = firstToken.GetPreviousToken();

                            if (prevToken != null && (
                                prevToken.Kind == SyntaxKind.OpenParenToken
                                || prevToken.Kind == SyntaxKind.OpenBraceToken))
                            {
                                AddRule(firstToken, new SpacingRule(SpacingKind.NewLine, () => SpansOrWillSpanMultipleLines(entireQuery, excluded: firstToken)));
                            }
                        }
                        break;

                    case PlacementStyle.NewLine:
                        // place this pipe expression's | on a new line if there are any new lines in the whole expression
                        AddRule(barToken, SpacingRule.From(SpacingKind.NewLine));

                        // Adjust the spacing of the operator's first token so it snaps to the |'s placement.
                        opToken = pe.Operator.GetFirstToken();
                        AddRule(opToken, SpacingRule.From(SpacingKind.SingleSpace));
                        break;
                }
            }
        }

        private void AddSemicolonRules(SeparatedElement se)
        {
            switch (_options.SemicolonStyle)
            {
                case PlacementStyle.Smart:
                    AddRule(se.Separator, new SpacingRule(SpacingKind.NewLine, () => SpansOrWillSpanMultipleLines((SyntaxNode)se.Element)));
                    AddRule(se.Separator, IndentRule(se));
                    break;

                case PlacementStyle.NewLine:
                    AddRule(se.Separator, SpacingRule.From(SpacingKind.NewLine));
                    AddRule(se.Separator, IndentRule(se));
                    break;
            }
        }

        private const int ArbitraryMaxCommaListWidthWidth = 80;

        private void AddQueryOperatorCommaListRules(SyntaxList list)
        {
            if (list.Width > ArbitraryMaxCommaListWidthWidth
                && !SpansMultipleLines(list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var elem = (SyntaxNode)list[i];
                    AddRule(elem, SpacingRule.From(SpacingKind.NewLine));
                }
            }
#if false   // consider triggering new lines for lists that is already split across lines
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var elem = (SyntaxNode)list[i];
                    AddRule(elem, new SpacingRule(SpacingKind.NewLine, () => SpansOrWillSpanMultipleLines(list)));
                }
            }
#endif
        }

        private void AddFunctionDeclarationRules(FunctionDeclaration fd)
        {
            var letStatement = fd.Parent;

            AddBrackettingStyleRules(fd.Body.OpenBrace, fd.Body.CloseBrace, letStatement);

            // assign spacing for each statement in body
            foreach (var statement in fd.Body.Statements)
            {
                var firstToken = statement.GetFirstToken();
                AddRule(firstToken, new SpacingRule(SpacingKind.NewLine, () => SpansOrWillSpanMultipleLines(fd.Body, inclusive: true, excluded: firstToken)));
            }

            // indent all statements relative to let statement
            AddRule(fd.Body.Statements, IndentRule(letStatement));

            // also apply same rules to final body expression
            if (fd.Body.Expression != null)
            {
                var firstToken = fd.Body.Expression.GetFirstToken();
                AddRule(firstToken, new SpacingRule(SpacingKind.NewLine, () => SpansOrWillSpanMultipleLines(fd.Body, inclusive: true, excluded: firstToken)));
                AddRule(fd.Body.Expression, IndentRule(letStatement));
            }
        }

        private void AddBracketExpressionRules(BracketedExpression be)
        {
            if (be.Parent is ElementExpression)
            {
                // no space between expression and open bracket
                AddRule(be.OpenBracket, SpacingRule.From(SpacingKind.NoSpaceIfOnSameLine));
            }
        }

        private void AddStatementRules(Statement st)
        {
            var first = st.GetFirstToken();
            if (first != null)
            {
                var prev = first.GetPreviousToken();
                if (prev != null)
                {
                    switch (_options.StatementStyle)
                    {
                        case PlacementStyle.Smart:
                            if (st.Parent is SeparatedElement<Statement> se
                                && se.Parent is SyntaxList<SeparatedElement<Statement>> list)
                            {
                                AddRule(first, new SpacingRule(SpacingKind.NewLine, () => SpansOrWillSpanMultipleLines(list, excluded: first)));
                            }
                            break;

                        case PlacementStyle.NewLine:
                            AddRule(first, SpacingRule.From(SpacingKind.NewLine));
                            break;
                    }
                }
            }
        }

        private const int ArbitraryMaxSingleLineDataTableWidth = 80;
        private const int ArbitraryMaxSingleLineSchemaWidth = 60;
        private const int ArbitraryMaxSingleLineRowWidth = 80;
        private const int ArbitraryMaxValueWidth = 40;

        private void AddDataTableExpressionRules(DataTableExpression dt)
        {
            // do nothing if whole declaration is small and on one line
            if (dt.Width <= ArbitraryMaxSingleLineDataTableWidth
                && !SpansMultipleLines(dt))
                return;

            var relativeTo = (SyntaxNode)dt.GetFirstAncestor<Statement>() ?? dt;

            var schemaColumns = dt.Schema.Columns.Count;

            // add schema rules
            if (dt.Schema.Width > ArbitraryMaxSingleLineSchemaWidth
                || SpansMultipleLines(dt.Schema))
            {
                AddBrackettingStyleRules(dt.Schema.OpenParen, dt.Schema.CloseParen, relativeTo);

                for (int i = 0; i < dt.Schema.Columns.Count; i++)
                {
                    var token = dt.Schema.Columns[i].GetFirstToken();
                    AddRule(token, SpacingRule.From(SpacingKind.NewLine));
                    AddRule(token, IndentRule(relativeTo));
                }
            }

            // add value rules
            var valueColumns = schemaColumns > 1
                ? schemaColumns
                : GetBalancedColumnCount(dt.Values);

            AddBrackettingStyleRules(dt.OpenBracket, dt.CloseBracket, relativeTo);

            // place rows of values on separate lines
            for (int i = 0; i < dt.Values.Count; i++)
            {
                var token = dt.Values[i].Element.GetFirstToken();
                if (token != null)
                {
                    if ((i % valueColumns) == 0)
                    {
                        AddRule(token, SpacingRule.From(SpacingKind.NewLine));
                        AddRule(token, IndentRule(relativeTo));
                    }
                    else
                    {
                        AddRule(token, SpacingRule.From(SpacingKind.SingleSpace));
                    }
                }
            }
        }

        private static int GetBalancedColumnCount(SyntaxList<SeparatedElement<Expression>> expressions)
        {
            // if any value is too wide, then place all on separate lines
            if (GetMaximumValueWidth(expressions) >= ArbitraryMaxValueWidth)
                return 1;

            // determine if better to display as multiple columns
            int columns = 10;

            while (true)
            {
                if (GetMaximumRowWidth(expressions, columns) <= ArbitraryMaxSingleLineRowWidth)
                {
                    return columns;
                }

                if (columns > 4)
                {
                    columns -= 2;
                    continue;
                }

                return 1;
            }
        }

        private static int GetMaximumValueWidth(SyntaxList<SeparatedElement<Expression>> expressions)
        {
            int maxWidth = 0;

            for (int i = 0; i < expressions.Count; i++)
            {
                var width = expressions[i].Element.Width;
                if (width > maxWidth)
                    maxWidth = width;
            }

            return maxWidth;
        }

        private static int GetMaximumRowWidth(SyntaxList<SeparatedElement<Expression>> expressions, int columns)
        {
            int maxWidth = 0;

            for (int firstColumnInRow = 0; firstColumnInRow < expressions.Count; firstColumnInRow += columns)
            {
                var lastColumnInRow = Math.Min(expressions.Count - 1, firstColumnInRow + columns - 1);
                var start = expressions[firstColumnInRow].TriviaStart;
                var end = expressions[lastColumnInRow].End;
                var width = end - start;
                if (width > maxWidth)
                    maxWidth = width;
            }

            return maxWidth;
        }

        private void AddBrackettingStyleRules(SyntaxToken open, SyntaxToken close, SyntaxNode alignedTo)
        {
            switch (_options.BrackettingStyle)
            {
                case BrackettingStyle.Vertical:
                    AddRule(open,
                        new SpacingRule(SpacingKind.NewLine, () => SpansOrWillSpanMultipleLines(open, close, inclusive: true, excluded: open))
                            .Otherwise(SpacingRule.From(SpacingKind.SingleSpaceIfOnSameLine)));

                    var next = open.GetNextToken();
                    if (next != null && next != close)
                    {
                        AddRule(next, SpacingRule.From(SpacingKind.SingleSpaceIfOnSameLine));
                    }

                    AddRule(open, new AlignmentRule(alignedTo));
                    AddRule(close,
                        new SpacingRule(SpacingKind.NewLine, () => SpansOrWillSpanMultipleLines(open, close, inclusive: true, excluded: close))
                            .Otherwise(SpacingRule.From(SpacingKind.SingleSpaceIfOnSameLine)));
                    AddRule(close, new AlignmentRule(alignedTo));
                    break;

                case BrackettingStyle.Diagonal:
                    AddRule(open, SpacingRule.From(SpacingKind.SingleSpace));

                    next = open.GetNextToken();
                    if (next != null && next != close)
                    {
                        AddRule(next, SpacingRule.From(SpacingKind.SingleSpaceIfOnSameLine));
                    }

                    AddRule(close,
                        new SpacingRule(SpacingKind.NewLine, () => SpansOrWillSpanMultipleLines(open, close, inclusive: true, excluded: close))
                            .Otherwise(SpacingRule.From(SpacingKind.SingleSpaceIfOnSameLine)));
                    AddRule(close, new AlignmentRule(alignedTo));
                    break;
            }
        }

        private static bool IsIdentifierOrKeyword(SyntaxToken token)
        {
            return token.Kind == SyntaxKind.IdentifierToken
                || SyntaxFacts.IsKeyword(token.Kind);
        }
#endregion

#region Formatting Rules
        [Flags]
        private enum SpacingKind
        {
            /// <summary>
            /// Spacing is unknown.  This value is not used.
            /// </summary>
            Unknown                 = 0,

            /// <summary>
            /// Leave spacing as is.
            /// </summary>
            AsIs                    = 1 << 1,

            /// <summary>
            /// Align leading spacing between tokens if they are on separate lines
            /// </summary>
            AlignOnly               = 1 << 2,

            /// <summary>
            /// No space between tokens
            /// </summary>
            NoSpace                 = 1 << 3,

            /// <summary>
            /// No space between tokens if they are on the same line.
            /// </summary>
            NoSpaceIfOnSameLine     = 1 << 4,

            /// <summary>
            /// A single space between tokens.
            /// </summary>
            SingleSpace             = 1 << 5,

            /// <summary>
            /// A single space between tokens if they are on the same line.
            /// </summary>
            SingleSpaceIfOnSameLine = 1 << 6,

            /// <summary>
            /// A single new line between tokens if they are on the same line
            /// </summary>
            NewLine                 = 1 << 7,
        }

        private enum ComputationState
        {
            Uncomputed = 0,
            Computing,
            Computed
        }

        /// <summary>
        /// A formating rule that dictates the spacing/trivia between tokens.
        /// </summary>
        private class SpacingRule
        {
            /// <summary>
            /// The kind of spacing triggered by the condition
            /// </summary>
            private readonly SpacingKind _kind;

            /// <summary>
            /// The next rule to apply if this rule does not apply.
            /// </summary>
            private readonly SpacingRule _otherwise;

            /// <summary>
            /// The rule is only applied when the condition evalutes to true.
            /// </summary>
            private readonly Func<bool> _condition;

            private ComputationState _computeState;
            private SpacingKind _computedKind;

            public SpacingRule(SpacingKind kind, Func<bool> condition, SpacingRule otherwise = null)
            {
                _kind = kind;
                _otherwise = otherwise;
                _condition = condition;
            }

            public SpacingKind GetKind()
            {
                if (_condition == null)
                {
                    return _kind;
                }

                switch (_computeState)
                {
                    case ComputationState.Computing:
                        return GetPossibleKinds();

                    case ComputationState.Uncomputed:
                        // avoids recursion during computation
                        _computeState = ComputationState.Computing;

                        if (_condition())
                        {
                            _computedKind = _kind;
                        }
                        else if (_otherwise != null)
                        {
                            _computedKind = _otherwise.GetKind();
                        }
                        else
                        {
                            _computedKind = SpacingKind.AlignOnly;
                        }

                        _computeState = ComputationState.Computed;
                        break;
                }

                return _computedKind;
            }

            private SpacingKind GetPossibleKinds()
            {
                if (_condition != null)
                {
                    if (_otherwise != null)
                    {
                        return _kind | _otherwise.GetPossibleKinds();
                    }
                    else
                    {
                        return _kind | SpacingKind.AlignOnly;
                    }
                }
                else
                {
                    return _kind;
                }
            }

            public static SpacingRule From(SpacingKind kind)
            {
                return s_spacings[(int)kind];
            }

            /// <summary>
            /// Create a new <see cref="SpacingRule"/> that applies another rule if this rule does not apply.
            /// </summary>
            public SpacingRule Otherwise(SpacingRule rule)
            {
                if (_condition == null)
                {
                    // this rule is unconditional, so it will never fallback to otherwise, so just return same rule.
                    return this;
                }
                else if (_otherwise == null)
                {
                    return new SpacingRule(_kind, _condition, rule);
                }
                else if (_otherwise._condition == null && rule._condition != null)
                {
                    // if this rule's otherwise is unconditional, but the new rule is conditional, then 
                    // insert the new rule between this rule and the existing unconditional otherwise.
                    return new SpacingRule(_kind, _condition, rule.Otherwise(_otherwise));
                }
                else
                {
                    // otherwise, attempt to chain the new rule after this rule's otherwise
                    return new SpacingRule(_kind, _condition, _otherwise.Otherwise(rule));
                }
            }

            private static readonly SpacingRule[] s_spacings;

            static SpacingRule()
            {
                var nSpacings = (int)SpacingKind.NewLine + 1;
                s_spacings = new SpacingRule[nSpacings];

                for (int i = 0; i < nSpacings; i++)
                {
                    s_spacings[i] = new SpacingRule((SpacingKind)i, null, null);
                }
            }
        }

        /// <summary>
        /// A formatting rule that determines the relative alignment between syntax elements
        /// when they appear on separate lines.
        /// </summary>
        private class AlignmentRule
        {
            /// <summary>
            /// The element to align relative to.
            /// If this value is null, alignment is relative to the current default.
            /// </summary>
            public SyntaxElement RelativeToElement { get; }

            /// <summary>
            /// The relative indentation (number of spaces to indent/exdent?).
            /// </summary>
            public int IndentationDelta { get; }

            public AlignmentRule(SyntaxElement relativeTo, int indentationDelta = 0)
            {
                this.RelativeToElement = relativeTo;
                this.IndentationDelta = indentationDelta;
            }

            public AlignmentRule(int indentationDelta)
                : this(null, indentationDelta)
            {
            }
        }

        /// <summary>
        /// Create and indentation (alignment) rule.
        /// If the relativeTo item is specified, the resulting alignment will be relative to this items alignment.
        /// If the relaetiveTo item is not specified, the resulting alignment will be relative to the current alignment.
        /// </summary>
        private AlignmentRule IndentRule(SyntaxElement relativeTo = null)
        {
            return new AlignmentRule(relativeTo, _options.IndentationSize);
        }

        /// <summary>
        /// Add a spacing rule for the first token of the specified node.
        /// If a spacing rule already exists, this one takes precidence.
        /// </summary>
        private void AddRule(SyntaxNode node, SpacingRule rule)
        {
            if (node != null)
            {
                AddRule(node.GetFirstToken(), rule);
            }
        }

        /// <summary>
        /// Add a spacing rule for the specified token.
        /// If a spacing rule already exists, this one takes precidence.
        /// </summary>
        private void AddRule(SyntaxToken token, SpacingRule rule)
        {
            if (token != null)
            {
                if (_spacingRules.TryGetValue(token, out var existingRule))
                {
                    // chain rules together so more specific rule gets applied after more general rule
                    rule = rule.Otherwise(existingRule);
                }

                _spacingRules[token] = rule;
            }
        }

        private void AddRule(SyntaxElement element, AlignmentRule rule)
        {
            if (element != null)
            {
                _alignmentRules[element] = rule;
            }
        }
#endregion

#region Typical spacing conditions
        /// <summary>
        /// Returns true if the tokens were originally on different lines.
        /// </summary>
        private static bool SpansMultipleLines(SyntaxToken first, SyntaxToken last, bool inclusive = false, SyntaxToken excluded = null)
        {
            if (first != null && last != null)
            {
                if (inclusive && first != excluded && TextFacts.HasLineBreaks(first.Trivia))
                {
                    return true;
                }

                var token = first.GetNextToken();
                while (token != null)
                {
                    if (token != excluded && TextFacts.HasLineBreaks(token.Trivia))
                    {
                        return true;
                    }

                    if (token == last)
                        break;

                    token = token.GetNextToken();
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the tokens of the node are currently on different lines.
        /// </summary>
        private static bool SpansMultipleLines(SyntaxNode node, bool inclusive = false, SyntaxToken excluded = null)
        {
            if (node != null)
            {
                return SpansMultipleLines(node.GetFirstToken(), node.GetLastToken(), inclusive, excluded);
            }

            return false;
        }

        /// <summary>
        /// Returns true if the tokens are currently or will end up on different lines.
        /// Returns null if the outcome is unknown due to cyclic dependencies between formatting rules.
        /// </summary>
        private bool SpansOrWillSpanMultipleLines(SyntaxToken first, SyntaxToken last, bool inclusive = false, SyntaxToken excluded = null)
        {
            if (SpansMultipleLines(first, last, inclusive, excluded))
            {
                return true;
            }

            return WillSpanMultipleLines(first, last, inclusive, excluded);
        }

        /// <summary>
        /// Returns true if the tokens of the node are currently or will end up on different lines.
        /// </summary>
        private bool SpansOrWillSpanMultipleLines(SyntaxNode node, bool inclusive = false, SyntaxToken excluded = null)
        {
            if (SpansMultipleLines(node, inclusive, excluded))
            {
                return true;
            }

            return WillSpanMultipleLines(node, inclusive, excluded);
        }

        /// <summary>
        /// Returns true if the tokens will end up on different lines.
        /// </summary>
        private bool WillSpanMultipleLines(SyntaxToken first, SyntaxToken last, bool inclusive = false, SyntaxToken excluded = null)
        {
            if (first != null && last != null)
            {
                if (inclusive && first != excluded && WillHaveLineBreak(first))
                {
                    return true;
                }

                var token = first.GetNextToken();
                while (token != null)
                {
                    if (token != excluded && WillHaveLineBreak(token))
                    {
                        return true;
                    }

                    if (token == last)
                        break;

                    token = token.GetNextToken();
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the tokens of the node will end up on different lines.
        /// </summary>
        private bool WillSpanMultipleLines(SyntaxNode node, bool inclusive = false, SyntaxToken excluded = null)
        {
            if (node != null)
            {
                var first = node.GetFirstToken();
                var last = node.GetLastToken();
                return WillSpanMultipleLines(first, last, inclusive, excluded);
            }

            return false;
        }

        /// <summary>
        /// Returns true if the token's trivia will end up with at least one line break.
        /// </summary>
        private bool WillHaveLineBreak(SyntaxToken token)
        {
            if (TextFacts.HasLineBreaks(token.Trivia))
            {
                return !WillHaveLineBreakRemoved(token);
            }
            else
            {
                return WillHaveLineBreakAdded(token);
            }
        }

        /// <summary>
        /// Returns true if the token's trivia definitely will have a line break added.
        /// </summary>
        private bool WillHaveLineBreakAdded(SyntaxToken token)
        {
            if (_spacingRules.TryGetValue(token, out var s))
            {
                return s.GetKind() == SpacingKind.NewLine;
            }

            return false;
        }

        /// <summary>
        /// Returns true if the token's trivia will definitely have its line breaks removed.
        /// </summary>
        private bool WillHaveLineBreakRemoved(SyntaxToken token)
        {
            if (_spacingRules.TryGetValue(token, out var s))
            {
                var kind = s.GetKind();
                return kind == SpacingKind.NoSpace || kind == SpacingKind.SingleSpace;
            }

            return false;
        }
#endregion
    }
}
