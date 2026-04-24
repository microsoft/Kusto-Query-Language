namespace Kusto.Language.Editor
{
    public sealed class FormattingOptions
    {
        /// <summary>
        /// The number of spaces used per indentation level.
        /// </summary>
        public int IndentationSize { get; }

        /// <summary>
        /// Automatically add any missing tokens.
        /// </summary>
        public bool InsertMissingTokens { get; }

        /// <summary>
        /// The default bracketting style.
        /// </summary>
        public BrackettingStyle BrackettingStyle { get; }

        /// <summary>
        /// Schema list bracketting style
        /// </summary>
        public BrackettingStyle SchemaStyle { get; }

        /// <summary>
        /// Data table values bracket style
        /// </summary>
        public BrackettingStyle DataTableValueStyle { get; }

        /// <summary>
        /// Function body brace style
        /// </summary>
        public BrackettingStyle FunctionBodyStyle { get; }

        /// <summary>
        /// Function parameter list parentheses style
        /// </summary>
        public BrackettingStyle FunctionParameterStyle { get; }

        /// <summary>
        /// Function call argument list parentheses style
        /// </summary>
        public BrackettingStyle FunctionArgumentStyle { get; }

        /// <summary>
        /// Placement style of pipe operator.
        /// </summary>
        public PlacementStyle PipeOperatorStyle { get; }

        /// <summary>
        /// Placement style of expressions in a list.
        /// </summary>
        public PlacementStyle ExpressionStyle { get; }

        /// <summary>
        /// Placement style of statements.
        /// </summary>
        public PlacementStyle StatementStyle { get; }

        /// <summary>
        /// Placement style of semicolons between statements.
        /// </summary>
        public PlacementStyle SemicolonStyle { get; }

        /// <summary>
        /// The spacing between tokens not otherwise covered by other options.
        /// </summary>
        public SpacingStyle GeneralSpacing { get; }

        /// <summary>
        /// The spacing before and after a prefix unary operator
        /// </summary>
        public SpacingStyle PrefixOperatorSpacing { get; }

        /// <summary>
        /// The spacing before and after an infix binary operator
        /// </summary>
        public DualSpacingStyle InfixOperatorSpacing { get; }

        /// <summary>
        /// The spacing before and after a pipe operator.
        /// </summary>
        public DualSpacingStyle PipeOperatorSpacing { get; }

        /// <summary>
        /// The spacing before and after a comma
        /// </summary>
        public DualSpacingStyle CommaSpacing { get; }

        /// <summary>
        /// The spacing before and after a colon (such as in a column or parameter declaration)
        /// </summary>
        public DualSpacingStyle ColonSpacing { get; }

        /// <summary>
        /// The spacing before and after an assignment operator (=)
        /// </summary>
        public DualSpacingStyle AssignmentSpacing { get; }

        /// <summary>
        /// The spacing before and after the range operator (..)
        /// </summary>
        public DualSpacingStyle RangeOperatorSpacing { get; }

        /// <summary>
        /// The spacing before and after a semicolon
        /// </summary>
        public DualSpacingStyle SemicolonSpacing { get; }

        /// <summary>
        /// The spacing before and after the expression in a parenthesized expression: (x + 1) vs ( x + 1 )
        /// </summary>
        public SpacingStyle ParenthesizedExpressionSpacing { get; }

        /// <summary>
        /// The spacing before and after an argument lists: fn(a, b) vs fn( a, b )
        /// </summary>
        public SpacingStyle ArgumentListSpacing { get; }

        /// <summary>
        /// The spacing inside empty argument lists: fn() vs fn( )
        /// </summary>
        public SpacingStyle EmptyArgumentListSpacing { get; }

        /// <summary>
        /// The spacing before and after a parameter list: (x: int) vs ( x: int )
        /// </summary>
        public SpacingStyle ParameterListSpacing { get; }

        /// <summary>
        /// The spacing inside empty parameter lists: () vs ( )
        /// </summary>
        public SpacingStyle EmptyParameterListSpacing { get; }

        /// <summary>
        /// The spacing before or after the expressions inside a json array: [0] vs [ 0 ]
        /// </summary>
        public SpacingStyle JsonArraySpacing { get; }

        /// <summary>
        /// The spacing inside empty json arrays: [] vs [ ]
        /// </summary>
        public SpacingStyle EmptyJsonArraySpacing { get; }

        /// <summary>
        /// The spacing inside json objects: {a: 1} vs { a: 1 }
        /// </summary>
        public SpacingStyle JsonObjectSpacing { get; }

        /// <summary>
        /// The spacing inside empty json objects: {} vs { }
        /// </summary>
        public SpacingStyle EmptyJsonObjectSpacing { get; }

        /// <summary>
        /// The spacing inside data table value brackets: [1, 2] vs [ 1, 2 ]
        /// </summary>
        public SpacingStyle DataTableValueSpacing { get; }

        /// <summary>
        /// The spacing inside empty data table value brackets: [] vs [ ]
        /// </summary>
        public SpacingStyle EmptyDataTableValueSpacing { get; }

        /// <summary>
        /// The spacing inside function body braces: { x } vs {x}
        /// </summary>
        public SpacingStyle FunctionBodySpacing { get; }

        /// <summary>
        /// The spacing inside empty function body braces: { } vs {}
        /// </summary>
        public SpacingStyle EmptyFunctionBodySpacing { get; }

        /// <summary>
        /// The spacing before a function body open brace: () { x } vs (){ x }
        /// </summary>
        public SpacingStyle BeforeFunctionBodySpacing { get; }

        /// <summary>
        /// The spacing before a parameter list open paren: (x: int) vs (x: int)
        /// </summary>
        public SpacingStyle BeforeParameterListSpacing { get; }

        /// <summary>
        /// The spacing before an argument list open paren: fn(x) vs fn (x)
        /// </summary>
        public SpacingStyle BeforeArgumentListSpacing { get; }

        /// <summary>
        /// The spacing before a data table value open bracket: datatable [1, 2] vs datatable[1, 2]
        /// </summary>
        public SpacingStyle BeforeDataTableValueSpacing { get; }

        /// <summary>
        /// The default formatting options.
        /// </summary>
        public static readonly FormattingOptions Default = 
            new FormattingOptions(
                indentationSize: 4,
                insertMissingTokens: true,
                brackettingStyle: BrackettingStyle.Vertical,
                schemaStyle: BrackettingStyle.Default,
                dataTableValueStyle: BrackettingStyle.Default,
                functionBodyStyle: BrackettingStyle.Default,
                functionParameterStyle: BrackettingStyle.Default,
                functionArgumentStyle: BrackettingStyle.Diagonal,
                pipeOperatorStyle: PlacementStyle.Smart,
                expressionStyle: PlacementStyle.Smart,
                statementStyle: PlacementStyle.NewLine,
                semicolonStyle: PlacementStyle.None,
                generalSpacing: SpacingStyle.One,
                prefixOperatorSpacing: SpacingStyle.Minimal,
                infixOperatorSpacing: DualSpacingStyle.Both,
                pipeOperatorSpacing: DualSpacingStyle.Both,
                commaSpacing: DualSpacingStyle.After,
                colonSpacing: DualSpacingStyle.After,
                assignmentSpacing: DualSpacingStyle.Both,
                rangeOperatorSpacing: DualSpacingStyle.Both,
                semicolonSpacing: DualSpacingStyle.After,
                expressionParenSpacing: SpacingStyle.Minimal,
                argumentListSpacing: SpacingStyle.Minimal,
                emptyArgumentListSpacing: SpacingStyle.Minimal,
                parameterListSpacing: SpacingStyle.Minimal,
                emptyParameterListSpacing: SpacingStyle.Minimal,
                jsonArraySpacing: SpacingStyle.Minimal,
                emptyJsonArraySpacing: SpacingStyle.Minimal,
                jsonObjectSpacing: SpacingStyle.Minimal,
                emptyJsonObjectSpacing: SpacingStyle.Minimal,
                dataTableValueSpacing: SpacingStyle.Minimal,
                emptyDataTableValueSpacing: SpacingStyle.Minimal,
                functionBodySpacing: SpacingStyle.One,
                emptyFunctionBodySpacing: SpacingStyle.One,
                beforeFunctionBodySpacing: SpacingStyle.One,
                beforeParameterListSpacing: SpacingStyle.Minimal,
                beforeArgumentListSpacing: SpacingStyle.Minimal,
                beforeDataTableValueSpacing: SpacingStyle.One
                );

        private FormattingOptions(
            int indentationSize,
            bool insertMissingTokens,
            BrackettingStyle brackettingStyle,
            BrackettingStyle schemaStyle,
            BrackettingStyle dataTableValueStyle,
            BrackettingStyle functionBodyStyle,
            BrackettingStyle functionParameterStyle,
            BrackettingStyle functionArgumentStyle,
            PlacementStyle pipeOperatorStyle,
            PlacementStyle expressionStyle,
            PlacementStyle statementStyle,
            PlacementStyle semicolonStyle,
            SpacingStyle generalSpacing,
            SpacingStyle prefixOperatorSpacing,
            DualSpacingStyle infixOperatorSpacing,
            DualSpacingStyle pipeOperatorSpacing,
            DualSpacingStyle commaSpacing,
            DualSpacingStyle colonSpacing,
            DualSpacingStyle assignmentSpacing,
            DualSpacingStyle rangeOperatorSpacing,
            DualSpacingStyle semicolonSpacing,
            SpacingStyle expressionParenSpacing,
            SpacingStyle argumentListSpacing,
            SpacingStyle emptyArgumentListSpacing,
            SpacingStyle parameterListSpacing,
            SpacingStyle emptyParameterListSpacing,
            SpacingStyle jsonArraySpacing,
            SpacingStyle emptyJsonArraySpacing,
            SpacingStyle jsonObjectSpacing,
            SpacingStyle emptyJsonObjectSpacing,
            SpacingStyle dataTableValueSpacing,
            SpacingStyle emptyDataTableValueSpacing,
            SpacingStyle functionBodySpacing,
            SpacingStyle emptyFunctionBodySpacing,
            SpacingStyle beforeFunctionBodySpacing,
            SpacingStyle beforeParameterListSpacing,
            SpacingStyle beforeArgumentListSpacing,
            SpacingStyle beforeDataTableValueSpacing)
        {
            this.IndentationSize = indentationSize;
            this.InsertMissingTokens = insertMissingTokens;
            this.BrackettingStyle = brackettingStyle;
            this.SchemaStyle = schemaStyle;
            this.DataTableValueStyle = dataTableValueStyle;
            this.FunctionBodyStyle = functionBodyStyle;
            this.FunctionParameterStyle = functionParameterStyle;
            this.FunctionArgumentStyle = functionArgumentStyle;
            this.PipeOperatorStyle = pipeOperatorStyle;
            this.ExpressionStyle = expressionStyle;
            this.StatementStyle = statementStyle;
            this.SemicolonStyle = semicolonStyle;
            this.GeneralSpacing = generalSpacing;
            this.PrefixOperatorSpacing = prefixOperatorSpacing;
            this.InfixOperatorSpacing = infixOperatorSpacing;
            this.PipeOperatorSpacing = pipeOperatorSpacing;
            this.CommaSpacing = commaSpacing;
            this.ColonSpacing = colonSpacing;
            this.AssignmentSpacing = assignmentSpacing;
            this.RangeOperatorSpacing = rangeOperatorSpacing;
            this.SemicolonSpacing = semicolonSpacing;
            this.ParenthesizedExpressionSpacing = expressionParenSpacing;
            this.ArgumentListSpacing = argumentListSpacing;
            this.EmptyArgumentListSpacing = emptyArgumentListSpacing;
            this.ParameterListSpacing = parameterListSpacing;
            this.EmptyParameterListSpacing = emptyParameterListSpacing;
            this.JsonArraySpacing = jsonArraySpacing;
            this.EmptyJsonArraySpacing = emptyJsonArraySpacing;
            this.JsonObjectSpacing = jsonObjectSpacing;
            this.EmptyJsonObjectSpacing = emptyJsonObjectSpacing;
            this.DataTableValueSpacing = dataTableValueSpacing;
            this.EmptyDataTableValueSpacing = emptyDataTableValueSpacing;
            this.FunctionBodySpacing = functionBodySpacing;
            this.EmptyFunctionBodySpacing = emptyFunctionBodySpacing;
            this.BeforeFunctionBodySpacing = beforeFunctionBodySpacing;
            this.BeforeParameterListSpacing = beforeParameterListSpacing;
            this.BeforeArgumentListSpacing = beforeArgumentListSpacing;
            this.BeforeDataTableValueSpacing = beforeDataTableValueSpacing;
        }

        private FormattingOptions With(
            int? indentationSize = null,
            bool? insertMissingTokens = null,
            BrackettingStyle? brackettingStyle = null,
            BrackettingStyle? schemaStyle = null,
            BrackettingStyle? dataTableValueStyle = null,
            BrackettingStyle? functionBodyStyle = null,
            BrackettingStyle? functionParameterStyle = null,
            BrackettingStyle? functionArgumentStyle = null,
            PlacementStyle? pipeOperatorStyle = null,
            PlacementStyle? expressionStyle = null,
            PlacementStyle? statementStyle = null,
            PlacementStyle? semicolonStyle = null,
            SpacingStyle? generalSpacing = null,
            SpacingStyle? prefixOperatorSpacing = null,
            DualSpacingStyle? infixOperatorSpacing = null,
            DualSpacingStyle? pipeOperatorSpacing = null,
            DualSpacingStyle? commaSpacing = null,
            DualSpacingStyle? colonSpacing = null,
            DualSpacingStyle? assignmentSpacing = null,
            DualSpacingStyle? rangeOperatorSpacing = null,
            DualSpacingStyle? semicolonSpacing = null,
            SpacingStyle? expressionParenSpacing = null,
            SpacingStyle? argumentListSpacing = null,
            SpacingStyle? emptyArgumentListSpacing = null,
            SpacingStyle? parameterListSpacing = null,
            SpacingStyle? emptyParameterListSpacing = null,
            SpacingStyle? jsonArraySpacing = null,
            SpacingStyle? emptyJsonArraySpacing = null,
            SpacingStyle? jsonObjectSpacing = null,
            SpacingStyle? emptyJsonObjectSpacing = null,
            SpacingStyle? dataTableValueSpacing = null,
            SpacingStyle? emptyDataTableValueSpacing = null,
            SpacingStyle? functionBodySpacing = null,
            SpacingStyle? emptyFunctionBodySpacing = null,
            SpacingStyle? beforeFunctionBodySpacing = null,
            SpacingStyle? beforeParameterListSpacing = null,
            SpacingStyle? beforeArgumentListSpacing = null,
            SpacingStyle? beforeDataTableValueSpacing = null)
        {
            var newIndentationSize = indentationSize.HasValue ? indentationSize.Value : this.IndentationSize;
            var newInsertMissingTokens = insertMissingTokens.HasValue ? insertMissingTokens.Value : this.InsertMissingTokens;
            var newBrackettingStyle = brackettingStyle.HasValue ? brackettingStyle.Value : this.BrackettingStyle;
            var newSchemaStyle = schemaStyle.HasValue ? schemaStyle.Value : this.SchemaStyle;
            var newDataTableValueStyle = dataTableValueStyle.HasValue ? dataTableValueStyle.Value : this.DataTableValueStyle;
            var newFunctionBodyStyle = functionBodyStyle.HasValue ? functionBodyStyle.Value : this.FunctionBodyStyle;
            var newFunctionParameterStyle = functionParameterStyle.HasValue ? functionParameterStyle.Value : this.FunctionParameterStyle;
            var newFunctionArgumentStyle = functionArgumentStyle.HasValue ? functionArgumentStyle.Value : this.FunctionArgumentStyle;
            var newPipeOperatorStyle = pipeOperatorStyle.HasValue ? pipeOperatorStyle.Value : this.PipeOperatorStyle;
            var newPipeOperatorSpacing = pipeOperatorSpacing.HasValue ? pipeOperatorSpacing.Value : this.PipeOperatorSpacing;
            var newExpressionStyle = expressionStyle.HasValue ? expressionStyle.Value : this.ExpressionStyle;
            var newStatementStyle = statementStyle.HasValue ? statementStyle.Value : this.StatementStyle;
            var newSemicolonStyle = semicolonStyle.HasValue ? semicolonStyle.Value : this.SemicolonStyle;
            var newGeneralSpacing = generalSpacing.HasValue ? generalSpacing.Value : this.GeneralSpacing;
            var newPrefixOperatorSpacing = prefixOperatorSpacing.HasValue ? prefixOperatorSpacing.Value : this.PrefixOperatorSpacing;
            var newInfixOperatorSpacing = infixOperatorSpacing.HasValue ? infixOperatorSpacing.Value : this.InfixOperatorSpacing;
            var newCommaSpacing = commaSpacing.HasValue ? commaSpacing.Value : this.CommaSpacing;
            var newColonSpacing = colonSpacing.HasValue ? colonSpacing.Value : this.ColonSpacing;
            var newAssignmentSpacing = assignmentSpacing.HasValue ? assignmentSpacing.Value : this.AssignmentSpacing;
            var newRangeOperatorSpacing = rangeOperatorSpacing.HasValue ? rangeOperatorSpacing.Value : this.RangeOperatorSpacing;
            var newSemicolonSpacing = semicolonSpacing.HasValue ? semicolonSpacing.Value : this.SemicolonSpacing;
            var newExpressionParenSpacing = expressionParenSpacing.HasValue ? expressionParenSpacing.Value : this.ParenthesizedExpressionSpacing;
            var newArgumentListSpacing = argumentListSpacing.HasValue ? argumentListSpacing.Value : this.ArgumentListSpacing;
            var newEmptyArgumentListSpacing = emptyArgumentListSpacing.HasValue ? emptyArgumentListSpacing.Value : this.EmptyArgumentListSpacing;
            var newParameterListSpacing = parameterListSpacing.HasValue ? parameterListSpacing.Value : this.ParameterListSpacing;
            var newEmptyParameterListSpacing = emptyParameterListSpacing.HasValue ? emptyParameterListSpacing.Value : this.EmptyParameterListSpacing;
            var newJsonArraySpacing = jsonArraySpacing.HasValue ? jsonArraySpacing.Value : this.JsonArraySpacing;
            var newEmptyJsonArraySpacing = emptyJsonArraySpacing.HasValue ? emptyJsonArraySpacing.Value : this.EmptyJsonArraySpacing;
            var newJsonObjectSpacing = jsonObjectSpacing.HasValue ? jsonObjectSpacing.Value : this.JsonObjectSpacing;
            var newEmptyJsonObjectSpacing = emptyJsonObjectSpacing.HasValue ? emptyJsonObjectSpacing.Value : this.EmptyJsonObjectSpacing;
            var newDataTableValueSpacing = dataTableValueSpacing.HasValue ? dataTableValueSpacing.Value : this.DataTableValueSpacing;
            var newEmptyDataTableValueSpacing = emptyDataTableValueSpacing.HasValue ? emptyDataTableValueSpacing.Value : this.EmptyDataTableValueSpacing;
            var newFunctionBodySpacing = functionBodySpacing.HasValue ? functionBodySpacing.Value : this.FunctionBodySpacing;
            var newEmptyFunctionBodySpacing = emptyFunctionBodySpacing.HasValue ? emptyFunctionBodySpacing.Value : this.EmptyFunctionBodySpacing;
            var newBeforeFunctionBodySpacing = beforeFunctionBodySpacing.HasValue ? beforeFunctionBodySpacing.Value : this.BeforeFunctionBodySpacing;
            var newBeforeParameterListSpacing = beforeParameterListSpacing.HasValue ? beforeParameterListSpacing.Value : this.BeforeParameterListSpacing;
            var newBeforeArgumentListSpacing = beforeArgumentListSpacing.HasValue ? beforeArgumentListSpacing.Value : this.BeforeArgumentListSpacing;
            var newBeforeDataTableValueSpacing = beforeDataTableValueSpacing.HasValue ? beforeDataTableValueSpacing.Value : this.BeforeDataTableValueSpacing;

            if (newIndentationSize != this.IndentationSize
                || newInsertMissingTokens != this.InsertMissingTokens
                || newBrackettingStyle != this.BrackettingStyle
                || newSchemaStyle != this.SchemaStyle
                || newDataTableValueStyle != this.DataTableValueStyle
                || newFunctionBodyStyle != this.FunctionBodyStyle
                || newFunctionParameterStyle != this.FunctionParameterStyle
                || newFunctionArgumentStyle != this.FunctionArgumentStyle
                || newPipeOperatorStyle != this.PipeOperatorStyle
                || newExpressionStyle != this.ExpressionStyle
                || newStatementStyle != this.StatementStyle
                || newSemicolonStyle != this.SemicolonStyle
                || newGeneralSpacing != this.GeneralSpacing
                || newPrefixOperatorSpacing != this.PrefixOperatorSpacing
                || newInfixOperatorSpacing != this.InfixOperatorSpacing
                || newPipeOperatorSpacing != this.PipeOperatorSpacing
                || newCommaSpacing != this.CommaSpacing
                || newColonSpacing != this.ColonSpacing
                || newAssignmentSpacing != this.AssignmentSpacing
                || newRangeOperatorSpacing != this.RangeOperatorSpacing
                || newSemicolonSpacing != this.SemicolonSpacing
                || newExpressionParenSpacing != this.ParenthesizedExpressionSpacing
                || newArgumentListSpacing != this.ArgumentListSpacing
                || newEmptyArgumentListSpacing != this.EmptyArgumentListSpacing
                || newParameterListSpacing != this.ParameterListSpacing
                || newEmptyParameterListSpacing != this.EmptyParameterListSpacing
                || newJsonArraySpacing != this.JsonArraySpacing
                || newEmptyJsonArraySpacing != this.EmptyJsonArraySpacing
                || newJsonObjectSpacing != this.JsonObjectSpacing
                || newEmptyJsonObjectSpacing != this.EmptyJsonObjectSpacing
                || newDataTableValueSpacing != this.DataTableValueSpacing
                || newEmptyDataTableValueSpacing != this.EmptyDataTableValueSpacing
                || newFunctionBodySpacing != this.FunctionBodySpacing
                || newEmptyFunctionBodySpacing != this.EmptyFunctionBodySpacing
                || newBeforeFunctionBodySpacing != this.BeforeFunctionBodySpacing
                || newBeforeParameterListSpacing != this.BeforeParameterListSpacing
                || newBeforeArgumentListSpacing != this.BeforeArgumentListSpacing
                || newBeforeDataTableValueSpacing != this.BeforeDataTableValueSpacing)
            {
                return new FormattingOptions(
                    newIndentationSize,
                    newInsertMissingTokens,
                    newBrackettingStyle,
                    newSchemaStyle,
                    newDataTableValueStyle,
                    newFunctionBodyStyle,
                    newFunctionParameterStyle,
                    newFunctionArgumentStyle,
                    newPipeOperatorStyle,
                    newExpressionStyle,
                    newStatementStyle,
                    newSemicolonStyle,
                    newGeneralSpacing,
                    newPrefixOperatorSpacing,
                    newInfixOperatorSpacing,
                    newPipeOperatorSpacing,
                    newCommaSpacing,
                    newColonSpacing,
                    newAssignmentSpacing,
                    newRangeOperatorSpacing,
                    newSemicolonSpacing,
                    newExpressionParenSpacing,
                    newArgumentListSpacing,
                    newEmptyArgumentListSpacing,
                    newParameterListSpacing,
                    newEmptyParameterListSpacing,
                    newJsonArraySpacing,
                    newEmptyJsonArraySpacing,
                    newJsonObjectSpacing,
                    newEmptyJsonObjectSpacing,
                    newDataTableValueSpacing,
                    newEmptyDataTableValueSpacing,
                    newFunctionBodySpacing,
                    newEmptyFunctionBodySpacing,
                    newBeforeFunctionBodySpacing,
                    newBeforeParameterListSpacing,
                    newBeforeArgumentListSpacing,
                    newBeforeDataTableValueSpacing
                    );
            }
            else
            {
                return this;
            }
        }

        public FormattingOptions WithIndentationSize(int size)
        {
            return With(indentationSize: size);
        }

        public FormattingOptions WithInsertMissingTokens(bool enable)
        {
            return With(insertMissingTokens: enable);
        }

        public FormattingOptions WithBrackettingStyle(BrackettingStyle style)
        {
            return With(brackettingStyle: style);
        }

        public FormattingOptions WithSchemaStyle(BrackettingStyle style)
        {
            return With(schemaStyle: style);
        }

        public FormattingOptions WithDataTableValueStyle(BrackettingStyle style)
        {
            return With(dataTableValueStyle: style);
        }

        public FormattingOptions WithFunctionBodyStyle(BrackettingStyle style)
        {
            return With(functionBodyStyle: style);
        }

        public FormattingOptions WithFunctionParameterStyle(BrackettingStyle style)
        {
            return With(functionParameterStyle: style);
        }

        public FormattingOptions WithFunctionArgumentStyle(BrackettingStyle style)
        {
            return With(functionArgumentStyle: style);
        }

        public FormattingOptions WithPipeOperatorStyle(PlacementStyle style)
        {
            return With(pipeOperatorStyle: style);
        }

        public FormattingOptions WithExpressionStyle(PlacementStyle style)
        {
            return With(expressionStyle: style);
        }

        public FormattingOptions WithStatementStyle(PlacementStyle style)
        {
            return With(statementStyle: style);
        }

        public FormattingOptions WithSemicolonStyle(PlacementStyle style)
        {
            return With(semicolonStyle: style);
        }

        public FormattingOptions WithGeneralSpacing(SpacingStyle style)
        {
            return With(generalSpacing: style);
        }

        public FormattingOptions WithPrefixOperatorSpacing(SpacingStyle style)
        {
            return With(prefixOperatorSpacing: style);
        }

        public FormattingOptions WithInfixOperatorSpacing(DualSpacingStyle style)
        {
            return With(infixOperatorSpacing: style);
        }

        public FormattingOptions WithPipeOperatorSpacing(DualSpacingStyle style)
        {
            return With(pipeOperatorSpacing: style);
        }

        public FormattingOptions WithCommaSpacing(DualSpacingStyle style)
        {
            return With(commaSpacing: style);
        }

        public FormattingOptions WithColonSpacing(DualSpacingStyle style)
        {
            return With(colonSpacing: style);
        }

        public FormattingOptions WithAssignmentSpacing(DualSpacingStyle style)
        {
            return With(assignmentSpacing: style);
        }

        public FormattingOptions WithRangeOperatorSpacing(DualSpacingStyle style)
        {
            return With(rangeOperatorSpacing: style);
        }

        public FormattingOptions WithSemicolonSpacing(DualSpacingStyle style)
        {
            return With(semicolonSpacing: style);
        }

        public FormattingOptions WithExpressionParenSpacing(SpacingStyle style)
        {
            return With(expressionParenSpacing: style);
        }

        public FormattingOptions WithArgumentListSpacing(SpacingStyle style)
        {
            return With(argumentListSpacing: style);
        }

        public FormattingOptions WithEmptyArgumentListSpacing(SpacingStyle style)
        {
            return With(emptyArgumentListSpacing: style);
        }

        public FormattingOptions WithParameterListSpacing(SpacingStyle style)
        {
            return With(parameterListSpacing: style);
        }

        public FormattingOptions WithEmptyParameterListSpacing(SpacingStyle style)
        {
            return With(emptyParameterListSpacing: style);
        }

        public FormattingOptions WithJsonArraySpacing(SpacingStyle style)
        {
            return With(jsonArraySpacing: style);
        }

        public FormattingOptions WithEmptyJsonArraySpacing(SpacingStyle style)
        {
            return With(emptyJsonArraySpacing: style);
        }

        public FormattingOptions WithJsonObjectSpacing(SpacingStyle style)
        {
            return With(jsonObjectSpacing: style);
        }

        public FormattingOptions WithEmptyJsonObjectSpacing(SpacingStyle style)
        {
            return With(emptyJsonObjectSpacing: style);
        }

        public FormattingOptions WithDataTableValueSpacing(SpacingStyle style)
        {
            return With(dataTableValueSpacing: style);
        }

        public FormattingOptions WithEmptyDataTableValueSpacing(SpacingStyle style)
        {
            return With(emptyDataTableValueSpacing: style);
        }

        public FormattingOptions WithFunctionBodySpacing(SpacingStyle style)
        {
            return With(functionBodySpacing: style);
        }

        public FormattingOptions WithEmptyFunctionBodySpacing(SpacingStyle style)
        {
            return With(emptyFunctionBodySpacing: style);
        }

        public FormattingOptions WithBeforeFunctionBodySpacing(SpacingStyle style)
        {
            return With(beforeFunctionBodySpacing: style);
        }

        public FormattingOptions WithBeforeParameterListSpacing(SpacingStyle style)
        {
            return With(beforeParameterListSpacing: style);
        }

        public FormattingOptions WithBeforeArgumentListSpacing(SpacingStyle style)
        {
            return With(beforeArgumentListSpacing: style);
        }

        public FormattingOptions WithBeforeDataTableValueSpacing(SpacingStyle style)
        {
            return With(beforeDataTableValueSpacing: style);
        }
    }
}