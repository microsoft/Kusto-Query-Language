namespace Kusto.Language.Symbols
{
    using Syntax;

    /// <summary>
    /// A binding context for a function/operator call.
    /// </summary>
    public abstract class CustomAvailabilityContext
    {
        /// <summary>
        /// The location related to the function/operator call.
        /// </summary>
        public virtual SyntaxNode Location => null;
    }
}