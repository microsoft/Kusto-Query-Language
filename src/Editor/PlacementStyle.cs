namespace Kusto.Language.Editor
{
    public enum PlacementStyle
    {
        /// <summary>
        /// Do not adjust placement
        /// </summary>
        None,

        /// <summary>
        /// Place on a new line always
        /// </summary>
        NewLine,

        /// <summary>
        /// Place on a new line if other parts are already span multiple lines
        /// </summary>
        Smart,
    }
}