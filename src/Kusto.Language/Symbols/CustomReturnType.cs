namespace Kusto.Language.Symbols
{
    /// <summary>
    /// A function that determines the a function's return type given binding info.
    /// </summary>
    public delegate TypeSymbol CustomReturnType(CustomReturnTypeContext info);
}
