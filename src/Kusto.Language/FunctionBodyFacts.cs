using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Symbols;
    using Utils;

    /// <summary>
    /// Useful facts about the function body identified during analysis.
    /// </summary>
    public class FunctionBodyFacts
    {
        private readonly Flags _flags;

        /// <summary>
        /// The return type of the function body when it is not dependent on parameters.
        /// </summary>
        public TypeSymbol NonVariableReturnType { get; }

        /// <summary>
        /// The parameters of the function that causes the return type to be variable.
        /// </summary>
        public IReadOnlyList<Parameter> DependentParameters { get; }

        /// <summary>
        /// The names used in unqualified table calls.
        /// </summary>
        public IReadOnlyList<string> UnqualifiedTableNames { get; }

        /// <summary>
        /// True if the function body had syntax or semantic errors.
        /// </summary>
        public bool HasSyntaxErrors { get; }

        /// <summary>
        /// The default <see cref="FunctionBodyFacts"/>.
        /// </summary>
        public static readonly FunctionBodyFacts Default =
            new FunctionBodyFacts(Flags.None, null, false, null, null);

        private FunctionBodyFacts(
            Flags flags,
            TypeSymbol nonVariableReturnType,
            bool hasSyntaxErrors,
            IEnumerable<Parameter> dependentParameters,
            IEnumerable<string> unqualifiedTableNames)
        {
            _flags = flags;
            NonVariableReturnType = nonVariableReturnType;
            HasSyntaxErrors = hasSyntaxErrors;
            DependentParameters = dependentParameters.ToReadOnly();
            UnqualifiedTableNames = unqualifiedTableNames.ToReadOnly();
        }

        /// <summary>
        /// Returns a new <see cref="FunctionBodyFacts"/> with the specified values,
        /// if the specified values differ from the current values.
        /// </summary>
        private FunctionBodyFacts With(
            Flags? flags = null,
            Optional<TypeSymbol> nonVariableReturnType = default(Optional<TypeSymbol>),
            bool? hasSyntaxErrors = null,
            Optional<IEnumerable<Parameter>> dependentParameters = default(Optional<IEnumerable<Parameter>>),
            Optional<IEnumerable<string>> unqualifiedTableNames = default(Optional<IEnumerable<string>>)
            )
        {
            var newFlags = flags.HasValue ? flags.Value : _flags;
            var newNonVariableReturnType = nonVariableReturnType.HasValue ? nonVariableReturnType.Value : this.NonVariableReturnType;
            var newHasSyntaxErrors = hasSyntaxErrors.HasValue ? hasSyntaxErrors.Value : this.HasSyntaxErrors;
            var newDependentParameters = dependentParameters.HasValue ? dependentParameters.Value : this.DependentParameters;
            var newUnqualifiedTableNames = unqualifiedTableNames.HasValue ? unqualifiedTableNames.Value : this.UnqualifiedTableNames;

            if (newFlags != _flags
                || newNonVariableReturnType != this.NonVariableReturnType
                || newHasSyntaxErrors != this.HasSyntaxErrors
                || newDependentParameters != this.DependentParameters
                || newUnqualifiedTableNames != this.UnqualifiedTableNames)
            {
                return new FunctionBodyFacts(
                    newFlags, 
                    newNonVariableReturnType, 
                    newHasSyntaxErrors, 
                    newDependentParameters,
                    newUnqualifiedTableNames
                    );
            }

            return this;
        }

        /// <summary>
        /// Returns a <see cref="FunctionBodyFacts"/> with <see cref="NonVariableReturnType"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithNonVariableReturnType(TypeSymbol type) =>
            With(nonVariableReturnType: type);

        /// <summary>
        /// Returns a <see cref="FunctionBodyFacts"/> with <see cref="HasSyntaxErrors"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithHasSyntaxErrors(bool value) =>
            With(hasSyntaxErrors: value);

        /// <summary>
        /// Returns a <see cref="FunctionBodyFacts"/> with <see cref="DependentParameters"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithDependentParameters(IEnumerable<Parameter> list)
        {
            if (this.DependentParameters == list)
            {
                return this;
            }
            else
            {
                return With(dependentParameters: new Optional<IEnumerable<Parameter>>(list.Distinct()));
            }
        }

        /// <summary>
        /// Returns a <see cref="FunctionBodyFacts"/> with an additional dependent parameter.
        /// </summary>
        public FunctionBodyFacts AddDependentParameter(Parameter parameter)
        {
            if (!this.DependentParameters.Contains(parameter))
            {
                return With(dependentParameters: this.DependentParameters.ToSafeList().AddItem(parameter));
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Returns a <see cref="FunctionBodyFacts"/> with an additional dependent parameter.
        /// </summary>
        public FunctionBodyFacts AddDependentParameters(IEnumerable<Parameter> parameters)
        {
            var adding = parameters.Where(p => !this.DependentParameters.Contains(p));
            if (adding.Any())
            {
                return With(dependentParameters: this.DependentParameters.ToSafeList().AddItems(adding));
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Returns a <see cref="FunctionBodyFacts"/> with <see cref="UnqualifiedTableNames"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithUnqualifiedTableNames(IEnumerable<string> names)
        {
            if (this.UnqualifiedTableNames == names)
            {
                return this;
            }
            else
            {
                return With(unqualifiedTableNames: new Optional<IEnumerable<string>>(names.Distinct()));
            }
        }

        /// <summary>
        /// Returns a <see cref="FunctionBodyFacts"/> with an additional unqualified table name.
        /// </summary>
        public FunctionBodyFacts AddUnqualifiedTableName(string name)
        {
            if (!this.UnqualifiedTableNames.Contains(name))
            {
                return With(unqualifiedTableNames: this.UnqualifiedTableNames.ToSafeList().AddItem(name));
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// True if the function's return type is dependent on argument values at call site.
        /// </summary>
        public bool HasVariableReturnType =>
            this.DependentParameters.Count > 0
            || this.HasUnqualifiedTableCall;

        /// <summary>
        /// True if the function body has a call to the cluster method, or a function invoked within it does
        /// </summary>
        public bool HasClusterCall =>
            (_flags & Flags.Cluster) != 0;

        /// <summary>
        /// Returns a new <see cref="FunctionBodyFacts"/> with <see cref="HasClusterCall"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithHasClusterCall(bool value) =>
            With(flags: value ? _flags | Flags.Cluster : _flags & ~Flags.Cluster);

        /// <summary>
        /// True if the function body has a call to the database method, or a function invoked within it does
        /// </summary>
        public bool HasDatabaseCall =>
            (_flags & Flags.Database) != 0;

        /// <summary>
        /// Returns a new <see cref="FunctionBodyFacts"/> with <see cref="HasDatabaseCall"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithHasDatabaseCall(bool value) =>
            With(flags: value ? _flags | Flags.Database : _flags & ~Flags.Database);

        /// <summary>
        /// True if the function body has a call to the unqualified table method, or a function invoked within it does.
        /// </summary>
        public bool HasUnqualifiedTableCall =>
            (_flags & Flags.UnqualifiedTable) != 0;

        /// <summary>
        /// Returns a new <see cref="FunctionBodyFacts"/> with <see cref="HasUnqualifiedTableCall"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithHasUnqualifiedTableCall(bool value) =>
            With(flags: value ? _flags | Flags.UnqualifiedTable : _flags & ~Flags.UnqualifiedTable);

        /// <summary>
        /// True if the function body has a call to the qualified database().table method, or a function invoked within it does.
        /// </summary>
        public bool HasQualifiedTableCall =>
            (_flags & Flags.QualifiedTable) != 0;

        /// <summary>
        /// Returns a new <see cref="FunctionBodyFacts"/> with <see cref="HasQualifiedTableCall"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithHasQualifiedTableCall(bool value) =>
            With(flags: value ? _flags | Flags.QualifiedTable : _flags & ~Flags.QualifiedTable);

        /// <summary>
        /// True if the function body has a call to the externaltable method, or a function invoked within it does.
        /// </summary>
        public bool HasExternalTableCall =>
            (_flags & Flags.ExternalTable) != 0;

        /// <summary>
        /// Returns a new <see cref="FunctionBodyFacts"/> with <see cref="HasExternalTableCall"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithHasExternalTableCall(bool value) =>
            With(flags: value ? _flags | Flags.ExternalTable : _flags & ~Flags.ExternalTable);

        /// <summary>
        /// True if the function body has a call to the materializedview method, or a function invoked within it does.
        /// </summary>
        public bool HasMaterializedViewCall =>
            (_flags & Flags.MaterializedView) != 0;

        /// <summary>
        /// Returns a new <see cref="FunctionBodyFacts"/> with <see cref="HasMaterializedViewCall"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithHasMaterializedViewCall(bool value) =>
            With(flags: value ? _flags | Flags.MaterializedView : _flags & ~Flags.MaterializedView);

        /// <summary>
        /// True if the function body has a call to the stored_query_result() method, or a function invoked within it does.
        /// </summary>
        public bool HasStoredQueryResultCall =>
            (_flags & Flags.StoredQueryResult) != 0;

        /// <summary>
        /// Returns a new <see cref="FunctionBodyFacts"/> with <see cref="HasStoredQueryResultCall"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithHasStoredQueryResultCall(bool value) =>
            With(flags: value ? _flags | Flags.StoredQueryResult : _flags & ~Flags.StoredQueryResult);

        /// <summary>
        /// True if the function body has a call to the graph() method, or a function invoked within it does.
        /// </summary>
        public bool HasGraphCall =>
            (_flags & Flags.Graph) != 0;

        /// <summary>
        /// Returns a new <see cref="FunctionBodyFacts"/> with <see cref="HasGraphCall"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithHasGraphCall(bool value) =>
            With(flags: value ? _flags | Flags.Graph : _flags & ~Flags.Graph);

        /// <summary>
        /// True if the function body has any interesting aspects.
        /// </summary>
        public bool IsInteresting =>
            _flags != Flags.None;

        /// <summary>
        /// Returns a new <see cref="FunctionBodyFacts"/> with <see cref="IsInteresting"/> assigned.
        /// </summary>
        public FunctionBodyFacts WithIsInteresting(bool value) =>
            With(flags: value ? _flags | Flags.None : _flags & ~Flags.None);

        /// <summary>
        /// Returns a <see cref="FunctionBodyFacts"/> with the properties of the called function's facts combined with one.
        /// </summary>
        public FunctionBodyFacts CombineCalledFunction(FunctionBodyFacts facts)
        {
            var newFlags = _flags | facts._flags;
            var newUnqualifiedTableNames =
                this.UnqualifiedTableNames.Count == 0 && facts.UnqualifiedTableNames.Count == 0 ? (IEnumerable<string>)this.UnqualifiedTableNames
                        : this.UnqualifiedTableNames.Count > 0 && facts.UnqualifiedTableNames.Count == 0 ? (IEnumerable<string>)this.UnqualifiedTableNames
                        : this.UnqualifiedTableNames.Count == 0 && facts.UnqualifiedTableNames.Count > 0 ? (IEnumerable<string>)facts.UnqualifiedTableNames
                        : this.UnqualifiedTableNames.Concat(facts.UnqualifiedTableNames).Distinct();

            return With(
                flags: newFlags,
                unqualifiedTableNames: new Optional<IEnumerable<string>>(newUnqualifiedTableNames)
                );
        }

        [Flags]
        private enum Flags
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
            /// The function body or any of its dependencies includes a call to the materialized_view() function.
            /// </summary>
            MaterializedView = 0b_0100_0000,

            /// <summary>
            /// The function body or any of its dependencies includes a call to the stored_query_result() function
            /// </summary>
            StoredQueryResult = 0b_1000_0000,

            /// <summary>
            /// The function body or any of its dependencies includes a call to the graph() function
            /// </summary>
            Graph = 0b_0001_0000_0000
        }
    }
}