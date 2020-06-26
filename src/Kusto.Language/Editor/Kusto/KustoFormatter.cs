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

            for (int i = 0; i < node.ChildCount; i++)
            {
                var child = node.GetChild(i);

                if (child != null)
                {
                    var spacingKind = SpacingKind.AlignOnly;
                    var childIndentation = indentation;

                    if (_spacingRules.TryGetValue(child, out var spacing))
                    {
                        spacingKind = spacing.GetKind();
                    }

                    if (_alignmentRules.TryGetValue(child, out var alignment))
                    {
                        if (alignment.RelativeToElement == null)
                        {
                            childIndentation += alignment.IndentationDelta;
                        }
                        else if (_elementIndentations.TryGetValue(alignment.RelativeToElement, out var elementIndentation))
                        {
                            childIndentation = elementIndentation + alignment.IndentationDelta;
                        }
                    }

                    if (child is SyntaxNode n)
                    {
                        WriteFormattedText(n, childIndentation);
                    }
                    else if (child is SyntaxToken t2)
                    {
                        WriteToken(t2, childIndentation, spacingKind);
                        _elementIndentations.Add(t2, childIndentation);
                    }

                    if (i == 0)
                    {
                        // the indentation of the parent node is the same as the indentation of the first element
                        _elementIndentations.Add(node, childIndentation);
                    }
                }
            }
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
                    lineEnd = nextLineStart >= 0 ? nextLineStart : whitespaceEnd;
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
                for (int i = 0; i < node.ChildCount; i++)
                {
                    var child = node.GetChild(i);
                    if (child is SyntaxNode n)
                    {
                        IdentifyFormattingRules(n);
                    }
                    else if (child is SyntaxToken t)
                    {
                        IdentifyTokenSpacing(t);
                    }
                }

                IdentifyNodeSpacing(node);
            }
        }

        private void IdentifyTokenSpacing(SyntaxToken token)
        {
            // don't adjust spacing in genernal if there are already line breaks
            if (TextFacts.HasLineBreaks(token.Trivia))
                return;

            // if no previous token then leave as is
            var prev = token.GetPreviousToken();
            if (prev == null)
                return;

            if (IsWildcardPart(token))
            {
                // wildcard parts should have no space between them (though grammatically legal)
                if (token.Trivia != "")
                {
                    AddRule(token, SpacingRule.From(SpacingKind.NoSpace));
                }
            }
            else if (IsIdentifierOrKeyword(token) && IsIdentifierOrKeyword(prev))
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

        private void IdentifyNodeSpacing(SyntaxNode n)
        {
            if ((n.Parent is Statement && !(n.Parent is ExpressionStatement))
                || n.Parent is QueryOperator
                || n.Parent is Command)
            {
                // sub elements of statements/commands/query-operators are all indented.
                if (n.Parent.GetChildIndex(n) > 0) // except for first token
                {
                    AddRule(n, IndentRule());
                }

                // start all clauses on new line if there are any new lines in statement/query-operator/command
                if (n is Clause)
                {
                    AddRule(n.GetFirstToken(),
                        new SpacingRule(SpacingKind.NewLine, () => DidOrWillSpanMultipleLines(n.Parent)));
                }
            }

            switch (n)
            {
                case PipeExpression pe:
                    switch (_options.PipeOperatorStyle)
                    {
                        case PlacementStyle.Smart:
                            var entirePipeExpression = pe.GetFirstAncestorOrSelf<SyntaxNode>(a => !(a.Parent is PipeExpression));

                            // place this pipe expression's | on a new line if there are any new lines in the whole expression
                            AddRule(pe.Bar, new SpacingRule(SpacingKind.NewLine, () => DidOrWillSpanMultipleLines(entirePipeExpression, excluded: pe.Bar)));

                            // Adjust the spacing of the operator's first token so it snaps to the |'s placement.
                            var opToken = pe.Operator.GetFirstToken();
                            AddRule(opToken, SpacingRule.From(SpacingKind.SingleSpace));

                            // adjust first token to new line if query is part of a parenthesized expression
                            if (entirePipeExpression == pe && pe.Parent is ParenthesizedExpression)
                            {
                                var firstToken = pe.GetFirstToken();
                                AddRule(firstToken, new SpacingRule(SpacingKind.NewLine, () => DidOrWillSpanMultipleLines(entirePipeExpression, excluded: firstToken)));
                            }
                            break;

                        case PlacementStyle.NewLine:
                            // place this pipe expression's | on a new line if there are any new lines in the whole expression
                            AddRule(pe.Bar, SpacingRule.From(SpacingKind.NewLine));

                            // Adjust the spacing of the operator's first token so it snaps to the |'s placement.
                            opToken = pe.Operator.GetFirstToken();
                            AddRule(opToken, SpacingRule.From(SpacingKind.SingleSpace));
                            break;
                    }
                    break;

                case FunctionDeclaration fd:
                    var letStatement = fd.Parent;

                    AddBrackettingStyleRules(fd.Body.OpenBrace, fd.Body.CloseBrace, letStatement);

                    foreach (var statement in fd.Body.Statements)
                    {
                        var firstToken = statement.GetFirstToken();
                        AddRule(firstToken, new SpacingRule(SpacingKind.NewLine, () => DidOrWillSpanMultipleLines(fd.Body, inclusive: true, excluded: firstToken)));
                    }

                    AddRule(fd.Body.Statements, IndentRule(letStatement));

                    if (fd.Body.Expression != null)
                    {
                        var firstToken = fd.Body.Expression.GetFirstToken();
                        AddRule(firstToken, new SpacingRule(SpacingKind.NewLine, () => DidOrWillSpanMultipleLines(fd.Body, inclusive: true, excluded: firstToken)));
                        AddRule(fd.Body.Expression, IndentRule(letStatement));
                    }
                    break;

                case SeparatedElement se:
                    // semicolons?
                    if (se.Separator?.Kind == SyntaxKind.SemicolonToken
                        && se.Element is LetStatement)
                    {
                        switch (_options.SemicolonStyle)
                        {
                            case PlacementStyle.Smart:
                                AddRule(se.Separator, new SpacingRule(SpacingKind.NewLine, () => DidOrWillSpanMultipleLines((SyntaxNode)se.Element)));
                                AddRule(se.Separator, IndentRule(se));
                                break;

                            case PlacementStyle.NewLine:
                                AddRule(se.Separator, SpacingRule.From(SpacingKind.NewLine));
                                AddRule(se.Separator, IndentRule(se));
                                break;
                        }
                    }
                    break;

                case BracketedExpression be:
                    if (be.Parent is ElementExpression)
                    {
                        // no space between expression and open bracket
                        AddRule(be.OpenBracket, SpacingRule.From(SpacingKind.NoSpaceIfOnSameLine));
                    }
                    break;
            }
        }

        private void AddBrackettingStyleRules(SyntaxToken open, SyntaxToken close, SyntaxNode alignedTo)
        {
            switch (_options.BrackettingStyle)
            {
                case BrackettingStyle.Vertical:
                    AddRule(open,
                        new SpacingRule(SpacingKind.NewLine, () => DidOrWillSpanMultipleLines(open, close, inclusive: true, excluded: open))
                            .Otherwise(SpacingRule.From(SpacingKind.SingleSpaceIfOnSameLine)));

                    var next = open.GetNextToken();
                    if (next != null && next != close)
                    {
                        AddRule(next, SpacingRule.From(SpacingKind.SingleSpaceIfOnSameLine));
                    }

                    AddRule(open, new AlignmentRule(alignedTo));
                    AddRule(close,
                        new SpacingRule(SpacingKind.NewLine, () => DidOrWillSpanMultipleLines(open, close, inclusive: true, excluded: close))
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
                        new SpacingRule(SpacingKind.NewLine, () => DidOrWillSpanMultipleLines(open, close, inclusive: true, excluded: close))
                            .Otherwise(SpacingRule.From(SpacingKind.SingleSpaceIfOnSameLine)));
                    AddRule(close, new AlignmentRule(alignedTo));
                    break;
            }
        }

#if false
        private void AddListElementRules<TElement>(SyntaxList<SeparatedElement<TElement>> list, SyntaxToken relativeTo)
            where TElement : SyntaxNode
        {
            AddRule(list, IndentRule(relativeTo));

            for (int i = 0; i < list.ChildCount; i++)
            {
                var se = list[i];
                AddRule(se.Element, new SpacingRule(SpacingKind.NewLine, () => DidOrWillSpanMultipleLines(list, inclusive: true)));
            }
        }
#endif

        private static bool IsIdentifierOrKeyword(SyntaxToken token)
        {
            return token.Kind == SyntaxKind.IdentifierToken
                || SyntaxFacts.IsKeyword(token.Kind);
        }

        private static bool IsWildcardPart(SyntaxToken token)
        {
            return token.Parent is WildcardedName;
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
        private static bool DidSpanMultipleLines(SyntaxToken first, SyntaxToken last, bool inclusive = false, SyntaxToken excluded = null)
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
        /// Returns true if the tokens of the node were originally on different lines.
        /// </summary>
        private static bool DidSpanMultipleLines(SyntaxNode node, bool inclusive = false, SyntaxToken excluded = null)
        {
            if (node != null)
            {
                return DidSpanMultipleLines(node.GetFirstToken(), node.GetLastToken(), inclusive, excluded);
            }

            return false;
        }

        /// <summary>
        /// Returns true if the tokens were originally or will end up on different lines.
        /// Returns null if the outcome is unknown due to cyclic dependencies between formatting rules.
        /// </summary>
        private bool DidOrWillSpanMultipleLines(SyntaxToken first, SyntaxToken last, bool inclusive = false, SyntaxToken excluded = null)
        {
            if (DidSpanMultipleLines(first, last, inclusive, excluded))
            {
                return true;
            }

            return WillSpanMultipleLines(first, last, inclusive, excluded);
        }

        /// <summary>
        /// Returns true if the tokens of the node were originally or will end up on different lines.
        /// </summary>
        private bool DidOrWillSpanMultipleLines(SyntaxNode node, bool inclusive = false, SyntaxToken excluded = null)
        {
            if (DidSpanMultipleLines(node, inclusive, excluded))
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
