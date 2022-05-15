using System;

namespace Kusto.Language
{
    using Symbols;

    /// <summary>
    /// Useful facts about the function body identified during analysis.
    /// </summary>
    public class FunctionBodyFacts
    {
        internal FunctionBodyFlags Flags { get; }

        /// <summary>
        /// The return type of the function body when it is not dependent on arguments.
        /// </summary>
        public TypeSymbol NonVariableComputedReturnType { get; }

        /// <summary>
        /// True if the function body had syntax or semantic errors.
        /// </summary>
        public bool HasErrors { get; }

        internal FunctionBodyFacts(
            FunctionBodyFlags flags,
            TypeSymbol nonVariableReturnType,
            bool hasErrors)
        {
            Flags = flags;
            NonVariableComputedReturnType = nonVariableReturnType;
            HasErrors = hasErrors;
        }

        /// <summary>
        /// True if the function has a variable return type (it is dependent on argument values or call site)
        /// </summary>
        public bool HasVariableReturnType =>
            (Flags & FunctionBodyFlags.VariableReturn) != 0;

        /// <summary>
        /// True if the function body has a call to the cluster method, or a function invoked within it does
        /// </summary>
        public bool HasClusterCall =>
            (Flags & FunctionBodyFlags.Cluster) != 0;

        /// <summary>
        /// True if the function body has a call to the database method, or a function invoked within it does
        /// </summary>
        public bool HasDatabaseCall =>
            (Flags & FunctionBodyFlags.Database) != 0;

        /// <summary>
        /// True if the function body has a call to the unqualified table method, or a function invoked within it does
        /// </summary>
        public bool HasUnqualifiedTableCall =>
            (Flags & FunctionBodyFlags.UnqualifiedTable) != 0;

        /// <summary>
        /// True if the function body has a call to the qualified database().table method, or a function invoked within it does
        /// </summary>
        public bool HasQualifiedTableCall =>
            (Flags & FunctionBodyFlags.QualifiedTable) != 0;

        /// <summary>
        /// True if the function body has a call to the externaltable method, or a function invoked within it does
        /// </summary>
        public bool HasExternalTableCall =>
            (Flags & FunctionBodyFlags.ExternalTable) != 0;

        /// <summary>
        /// True if the function body has a call to the materializedview method, or a function invoked within it does
        /// </summary>
        public bool HasMaterializedViewCall =>
            (Flags & FunctionBodyFlags.MaterializedView) != 0;

        /// <summary>
        /// True if the function body has any interesting aspects.
        /// </summary>
        public bool IsInteresting =>
            Flags != FunctionBodyFlags.None;

        /// <summary>
        /// Gets the <see cref="FunctionBodyFacts"/> associated with this <see cref="FunctionSymbol"/>
        /// that may have been cached with the <see cref="GlobalState"/> during analysis.
        /// </summary>
        public static bool TryGetFacts(FunctionSymbol function, GlobalState globals, out FunctionBodyFacts facts)
        {
            if (globals.IsDatabaseFunction(function))
            {
                return Binding.Binder.TryGetDatabaseFunctionBodyFacts(function, globals, out facts);
            }
            else
            {
                // non-database function symbols get their facts cached on the symbol itself.
                facts = function.NonDatabaseFunctionBodyFacts;
                return facts != null;
            }
        }
    }

    [Flags]
    internal enum FunctionBodyFlags
    {
        /// <summary>
        /// The function body does not have any known special conditions.
        /// </summary>
        None = 0b_0000_0000,

        /// <summary>
        /// The function body or any of its dependencies includes a call to the cluster() function.
        /// </summary>
        Cluster = 0b_0000_0001,

        /// <summary>
        /// The function body or any of its dependencies includes a call to the database() function.
        /// </summary>
        Database = 0b_0000_0010,

        /// <summary>
        /// The function body or any of its dependencies includes an unqualified call to the table() function.
        /// </summary>
        UnqualifiedTable = 0b_0000_0100,

        /// <summary>
        /// The function body or any of its dependencies includes a qualified call to the table() function.
        /// </summary>
        QualifiedTable = 0b_0000_1000,

        /// <summary>
        /// The function body or any of its dependencies includes a call to the external_table() function.
        /// </summary>
        ExternalTable = 0b_0001_0000,

        /// <summary>
        /// The function body may have a variable return type (due to variable tabular input)
        /// </summary>
        VariableReturn = 0b_0010_0000,

        /// <summary>
        /// The function body or any of its dependencies includes a call to the materialized_view() function.
        /// </summary>
        MaterializedView = 0b_0100_0000,
    }
}