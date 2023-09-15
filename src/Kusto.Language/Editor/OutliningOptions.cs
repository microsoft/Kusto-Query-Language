namespace Kusto.Language.Editor
{
    public sealed class OutliningOptions
    {
        /// <summary>
        /// The maximum number of characters in collapsed text.
        /// </summary>
        public int MaxCollapsedTextLength { get; }

        /// <summary>
        /// Outline the entire query block
        /// </summary>
        public bool Queries { get; }

        /// <summary>
        /// Outline each statement that spans multiple lines
        /// </summary>
        public bool Statements { get; }

        private OutliningOptions(
            int maxLength,
            bool queries,
            bool statements
            )
        {
            this.MaxCollapsedTextLength = maxLength;
            this.Queries = queries;
            this.Statements = statements;
        }

        public static readonly OutliningOptions Default =
            new OutliningOptions(
                maxLength: 120,
                queries: true,
                statements: false
                );

        private OutliningOptions With(
            int? maxLength = null,
            bool? queries = null,
            bool? statements = null)
        {
            var newMaxLength = maxLength.HasValue ? maxLength.Value : this.MaxCollapsedTextLength;
            var newQueries = queries.HasValue ? queries.Value : this.Queries;
            var newStatements = statements.HasValue ? statements.Value : this.Statements;

            if (maxLength != this.MaxCollapsedTextLength
                || newQueries != this.Queries
                || newStatements != this.Statements)
            {
                return new OutliningOptions(
                    newMaxLength,
                    newQueries,
                    newStatements
                    );
            }
            else
            {
                return this;
            }
        }

        public OutliningOptions WithMaxCollapesedTextlength(int length)
        {
            return With(maxLength: length);
        }

        public OutliningOptions WithQueries(bool enable)
        {
            return With(queries: enable);
        }

        public OutliningOptions WithStatements(bool enable)
        {
            return With(statements: enable);
        }
    }
}