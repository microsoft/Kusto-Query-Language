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

        public static readonly FormattingOptions Default = new FormattingOptions(
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
            semicolonStyle: PlacementStyle.None
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
            PlacementStyle semicolonStyle)
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
            PlacementStyle? semicolonStyle = null)
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
            var newExpressionStyle = expressionStyle.HasValue ? expressionStyle.Value : this.ExpressionStyle;
            var newStatementStyle = statementStyle.HasValue ? statementStyle.Value : this.StatementStyle;
            var newSemicolonStyle = semicolonStyle.HasValue ? semicolonStyle.Value : this.SemicolonStyle;

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
                || newSemicolonStyle != this.SemicolonStyle)
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
                    newSemicolonStyle);
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
    }
}