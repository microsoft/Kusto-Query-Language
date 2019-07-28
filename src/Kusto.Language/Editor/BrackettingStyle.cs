namespace Kusto.Language.Editor
{
    public enum BrackettingStyle
    {
        /// <summary>
        /// Do not adjust bracketting.
        /// </summary>
        None,

        /// <summary>
        /// Bracketting pairs are aligned horizontally when they are place on new lines.
        /// </summary>
        Vertical,

        /// <summary>
        /// Bracketting pairs are aligned diagonally, KR style, when placed on new lines.
        /// </summary>
        Diagonal
    }
}