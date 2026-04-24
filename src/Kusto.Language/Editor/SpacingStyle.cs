namespace Kusto.Language.Editor
{
    public enum SpacingStyle
    {
        /// <summary>
        /// The spacing is not adjusted.
        /// </summary>
        AsIs,

        /// <summary>
        /// The minimal amount of spacing between adjacent tokens that allows successful parsing,
        /// so either no space or a single space if required
        /// </summary>
        Minimal, 

        /// <summary>
        /// One space between adjacent tokens
        /// </summary>
        One   
    }


    /// <summary>
    /// The spacing style of an operator or token that has two sides, such as a binary operator or a comma.
    /// </summary>
    public enum DualSpacingStyle
    {
        /// <summary>
        /// The spacing is not adjusted
        /// </summary>
        AsIs,

        /// <summary>
        /// No space before or after (minimal spacing)
        /// </summary>
        Neither,

        /// <summary>
        /// There is a single space before the operator and no space (minimal) after the operator.
        /// </summary>
        Before,

        /// <summary>
        /// There is a single space after the operator and no space (minimal) before the operator.
        /// </summary>
        After,

        /// <summary>
        /// There is a single space both before and after the operator.
        /// </summary>
        Both
    }
}