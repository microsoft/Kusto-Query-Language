namespace Kusto.Language.Symbols
{
    /// <summary>
    /// A function that determines if a <see cref="FunctionSymbol"/> is available in a given context.
    /// </summary>
    public delegate bool CustomAvailability(CustomAvailabilityContext context);
}
