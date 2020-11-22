namespace Kusto.Language.Editor
{
    public class FormattingOptions
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
        /// Bracketting style: (), [], {}
        /// </summary>
        public BrackettingStyle BrackettingStyle { get; }

        /// <summary>
        /// Placement style of pipe operator.
        /// </summary>
        public PlacementStyle PipeOperatorStyle { get; }

        /// <summary>
        /// Placement style of query statements.
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
            pipeOperatorStyle: PlacementStyle.Smart,
            statementStyle: PlacementStyle.NewLine,
            semicolonStyle: PlacementStyle.None
            );

        private FormattingOptions(
            int indentationSize,
            bool insertMissingTokens,
            BrackettingStyle brackettingStyle,
            PlacementStyle pipeOperatorStyle,
            PlacementStyle statementStyle,
            PlacementStyle semicolonStyle)
        {
            this.IndentationSize = indentationSize;
            this.InsertMissingTokens = insertMissingTokens;
            this.BrackettingStyle = brackettingStyle;
            this.PipeOperatorStyle = pipeOperatorStyle;
            this.StatementStyle = statementStyle;
            this.SemicolonStyle = semicolonStyle;
        }

        private FormattingOptions With(
            int? indentationSize = null,
            bool? insertMissingTokens = null,
            BrackettingStyle? brackettingStyle = null,
            PlacementStyle? pipeOperatorStyle = null,
            PlacementStyle? statementStyle = null,
            PlacementStyle? semicolonStyle = null)
        {
            var newIndentationSize = indentationSize.HasValue ? indentationSize.Value : this.IndentationSize;
            var newInsertMissingTokens = insertMissingTokens.HasValue ? insertMissingTokens.Value : this.InsertMissingTokens;
            var newBrackettingStyle = brackettingStyle.HasValue ? brackettingStyle.Value : this.BrackettingStyle;
            var newPipeOperatorStyle = pipeOperatorStyle.HasValue ? pipeOperatorStyle.Value : this.PipeOperatorStyle;
            var newStatementStyle = statementStyle.HasValue ? statementStyle.Value : this.StatementStyle;
            var newSemicolonStyle = semicolonStyle.HasValue ? semicolonStyle.Value : this.SemicolonStyle;

            if (newIndentationSize != this.IndentationSize
                || newInsertMissingTokens != this.InsertMissingTokens
                || newBrackettingStyle != this.BrackettingStyle
                || newPipeOperatorStyle != this.PipeOperatorStyle
                || newStatementStyle != this.StatementStyle
                || newSemicolonStyle != this.SemicolonStyle)
            {
                return new FormattingOptions(
                    newIndentationSize,
                    newInsertMissingTokens,
                    newBrackettingStyle,
                    newPipeOperatorStyle,
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

        public FormattingOptions WithPipeOperatorStyle(PlacementStyle style)
        {
            return With(pipeOperatorStyle: style);
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