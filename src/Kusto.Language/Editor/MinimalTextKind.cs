namespace Kusto.Language.Editor
{
    public enum MinimalTextKind
    {
        /// <summary>
        /// Removes only whitespace and comments before the start of the query
        /// </summary>
        RemoveLeadingWhitespaceAndComments,

        /// <summary>
        /// Removes all whitespace before the query, removes all comments and reduces
        /// remaining whitespace to a single space or line break between tokens.
        /// </summary>
        MinimizeWhitespaceAndRemoveComments,

        /// <summary>
        /// Removes all whitespace before and after the query, removes all comments,
        /// and reduces any remaining whitespace to a single space between tokens.
        /// </summary>
        SingleLine,
    }
}