namespace Kusto.Language.Symbols
{
    public enum ResultNameKind
    {
        /// <summary>
        /// The name is not inferred from the function invocation.
        /// Typically the column is given the name Column1 or Column2, etc.
        /// </summary>
        None,

        /// <summary>
        /// The name is combination of the name of function and the name inferred from the first argument.
        /// </summary>
        NameAndFirstArgument,

        /// <summary>
        /// The name is a combination of the function name and the name is inferred from the the first argument,
        /// but only if there is only one argument, otherwise it acts as <see cref="None"/>.
        /// </summary>
        /// <remarks>This option exists because of odd name inference behavior of a few functions that differ depending on the number of arguments.</remarks>
        NameAndOnlyArgument,

        /// <summary>
        /// The name is a combination of the specified prefix and the name inferred from the first argument.
        /// </summary>
        PrefixAndFirstArgument,

        /// <summary>
        /// The name if a combination of the specified prefix and the name is inferred from the the first argument,
        /// but only if there is only one argument, otherwise it acts as <see cref="None"/>.
        /// </summary>
        /// <remarks>This option exists because of odd name inference behavior of a few functions that differ depending on the number of arguments.</remarks>
        PrefixAndOnlyArgument,

        /// <summary>
        /// The name of the column is the value the first parameter's string literal argument
        /// if a column of that name exists in the current row scope.
        /// </summary>
        /// <remarks>This option exists to support columnifexists() function.</remarks>
        FirstArgumentValueIfColumn,

        /// <summary>
        /// The name is the prefix name as specified (no underscores added.)
        /// </summary>
        PrefixOnly,

        /// <summary>
        /// The name is the name inferred from the first argument.
        /// </summary>
        FirstArgument,

        /// <summary>
        /// The name is the name inferred from the first argument if there is only one argument, otherwise it acts the same as <see cref="None"/>
        /// </summary>
        OnlyArgument,

        Default = None
    }
}