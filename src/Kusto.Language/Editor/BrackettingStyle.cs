namespace Kusto.Language.Editor
{
    /// <summary>
    /// Bracketting style determines how bracket pairs are aligned when on separate lines.
    /// </summary>
    public enum BrackettingStyle
    {
        /// <summary>
        /// Use the default style.
        /// </summary>
        Default,

        /// <summary>
        /// Bracket alignment is not adjusted.
        /// </summary>
        None,

        /// <summary>
        /// Bracket pairs are aligned vertically when on separate lines.
        /// </summary>
        Vertical,

        /// <summary>
        /// Bracket pairs are aligned diagonally when on separate lines.
        /// </summary>
        Diagonal
    }
}