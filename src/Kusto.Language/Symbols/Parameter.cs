using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;
    using Syntax;

    /// <summary>
    /// A parameter declaration for a <see cref="Signature"/>.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Parameter: {DebugText}")]
    public class Parameter
    {
        private string DebugText =>
            DebugDisplay.GetText(this);

        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The kind of parameter.
        /// </summary>
        public ParameterTypeKind TypeKind { get; }

        /// <summary>
        /// The possible declared types of the parameter.
        /// </summary>
        public IReadOnlyList<TypeSymbol> DeclaredTypes { get; }

        /// <summary>
        /// The kind of argument this parameter is constrained to.
        /// </summary>
        public ArgumentKind ArgumentKind { get; }

        /// <summary>
        /// The minimum number of times the parameter can occur in the signature.
        /// </summary>
        public int MinOccurring { get; }

        /// <summary>
        /// The maximum number of times the parameter can occur in the signature.
        /// </summary>
        public int MaxOccurring { get; }

        /// <summary>
        /// True if the parameter is optional.
        /// </summary>
        public bool IsOptional => this.MinOccurring == 0;

        /// <summary>
        /// True if the parameter can be repeated.
        /// </summary>
        public bool IsRepeatable => this.MaxOccurring > 1;

        /// <summary>
        /// The specific values that this parameter is constrained to.
        /// </summary>
        public IReadOnlyList<object> Values { get; }

        /// <summary>
        /// Completion list examples.
        /// </summary>
        public IReadOnlyList<string> Examples { get; }

        /// <summary>
        /// If true, then text values (string literal or tokens) must be matched case sensitive.
        /// </summary>
        public bool IsCaseSensitive { get; }

        /// <summary>
        /// The string literal that can be used to indicate use of the default value.
        /// </summary>
        public string DefaultValueIndicator { get; }

        /// <summary>
        /// The default value specified for user defined functions.
        /// </summary>
        public Expression DefaultValue { get; }

        /// <summary>
        /// The descripton of the parameter.
        /// </summary>
        public string Description { get; }

        private static readonly IReadOnlyList<object> NoValues = EmptyReadOnlyList<object>.Instance;

        private static readonly IReadOnlyList<string> NoExamples = EmptyReadOnlyList<string>.Instance;

        private Parameter(
            string name, 
            ParameterTypeKind typeKind, 
            TypeSymbol[] types, 
            ArgumentKind argumentKind, 
            IEnumerable<object> values, 
            IEnumerable<string> examples,
            bool isCaseSensitive,
            string defaultValueIndicator,
            int minOccurring,
            int maxOccurring,
            Expression defaultValue,
            string description)
        {
            if (typeKind == ParameterTypeKind.Declared)
            {
                this.DeclaredTypes = types.CheckArgumentNullOrEmptyOrElementNull(nameof(types)).ToReadOnly();
            }
            else
            {
                this.DeclaredTypes = EmptyReadOnlyList<TypeSymbol>.Instance;
            }

            this.Name = name;
            this.TypeKind = typeKind;
            this.ArgumentKind = argumentKind;
            this.Values = values != null ? values.ToReadOnly() : NoValues;
            this.Examples = examples != null ? examples.ToReadOnly() : NoExamples;
            this.IsCaseSensitive = isCaseSensitive;
            this.MinOccurring = defaultValue != null ? 0 : minOccurring;
            this.MaxOccurring = defaultValue != null ? 1 : maxOccurring;
            this.DefaultValueIndicator = defaultValueIndicator;
            this.DefaultValue = defaultValue;
            this.Description = description ?? "";
        }

        public Parameter(
            string name, 
            ParameterTypeKind typeKind, 
            ArgumentKind argumentKind = ArgumentKind.Expression, 
            IReadOnlyList<object> values = null, 
            IReadOnlyList<string> examples = null,
            bool isCaseSensitive = false, 
            string defaultValueIndicator = null,
            int minOccurring = 1, 
            int maxOccurring = 1,
            Expression defaultValue = null,
            string description = null)
            : this(
                  name, 
                  typeKind, 
                  null, 
                  argumentKind, 
                  values, 
                  examples, 
                  isCaseSensitive, 
                  defaultValueIndicator, 
                  minOccurring, 
                  maxOccurring, 
                  defaultValue,
                  description)
        {
        }

        public Parameter(
            string name, 
            TypeSymbol type, 
            ArgumentKind argumentKind = ArgumentKind.Expression, 
            IReadOnlyList<object> values = null,
            IReadOnlyList<string> examples = null,
            bool isCaseSensitive = false,
            string defaultValueIndicator = null,
            int minOccurring = 1,
            int maxOccurring = 1,
            Expression defaultValue = null,
            string description = null)
            : this(
                  name, 
                  ParameterTypeKind.Declared, 
                  new[] { type ?? ScalarTypes.Unknown }, 
                  argumentKind, 
                  values, 
                  examples, 
                  isCaseSensitive, 
                  defaultValueIndicator, 
                  minOccurring, 
                  maxOccurring, 
                  defaultValue,
                  description)
        {
        }

        public Parameter(
            string name,
            TypeSymbol[] types,
            ArgumentKind argumentKind = ArgumentKind.Expression,
            IReadOnlyList<object> values = null,
            IReadOnlyList<string> examples = null,
            bool isCaseSensitive = false,
            string defaultValueIndicator = null,
            int minOccurring = 1,
            int maxOccurring = 1,
            Expression defaultValue = null,
            string description = null)
            : this(
                  name, 
                  ParameterTypeKind.Declared, 
                  types, 
                  argumentKind, 
                  values, 
                  examples, 
                  isCaseSensitive, 
                  defaultValueIndicator, 
                  minOccurring, 
                  maxOccurring, 
                  defaultValue,
                  description)
        {
        }

        public Parameter(string name, TypeSymbol type)
            : this(name, ParameterTypeKind.Declared, new[] { type ?? ScalarTypes.Unknown }, ArgumentKind.Expression, null, null, false, null, 1, 1, null, null)
        {
        }

        /// <summary>
        /// Convert a <see cref="ParameterSymbol"/> into a <see cref="Parameter"/> instance.
        /// </summary>
        public static Parameter From(ParameterSymbol parameter, bool isOptional = false, Expression defaultValue = null)
        {
            return new Parameter(parameter.Name, parameter.Type, minOccurring: isOptional ? 0 : 1, defaultValue: defaultValue, description: parameter.Description);
        }

        /// <summary>
        /// Convert a parsed <see cref="FunctionParameter"/> syntax into a <see cref="Parameter"/>.
        /// </summary>
        public static Parameter From(FunctionParameter declaration)
        {
            var name = declaration.NameAndType.Name.SimpleName;
            var type = Binding.Binder.GetDeclaredType(declaration.NameAndType.Type);
            var defvalue = declaration.DefaultValue?.Value ?? null;
            return new Parameter(name, type, defaultValue: defvalue);
        }

        /// <summary>
        /// Convert parsed <see cref="FunctionParameters"/> syntax into a list of <see cref="Parameter"/>
        /// </summary>
        public static IReadOnlyList<Parameter> From(FunctionParameters declaration)
        {
            var list = new List<Parameter>();
            
            foreach (var pelem in declaration.Parameters)
            {
                list.Add(From(pelem.Element));
            }

            return list;
        }

        /// <summary>
        /// Parse the parameter list of a function declaration
        /// </summary>
        public static IReadOnlyList<Parameter> ParseList(string parameters)
        {
            var fp = Parsing.QueryParser.ParseFunctionParameters(parameters);
            return (fp != null)
                ? From(fp)
                : EmptyReadOnlyList<Parameter>.Instance;
        }

        /// <summary>
        /// Gets the text of the parameter as it would appear in a function declaration.
        /// </summary>
        public static string GetDeclaration(Parameter parameter)
        {
            var name = KustoFacts.BracketNameIfNecessary(parameter.Name);

            if (parameter.TypeKind == ParameterTypeKind.Declared && parameter.DeclaredTypes.Count == 1)
            {
                var type = parameter.DeclaredTypes[0];
                if (type is TableSymbol table)
                {
                    return $"{name}: {GetTableSchemaDeclaration(table)}";
                }
                else if (parameter.DefaultValue != null)
                {
                    return $"{name}: {parameter.DeclaredTypes[0].Name} = {parameter.DefaultValue.ToString(IncludeTrivia.Interior)}";
                }
                else
                {
                    return $"{KustoFacts.BracketNameIfNecessary(parameter.Name)}: {parameter.DeclaredTypes[0].Name}";
                }
            }
            else if (parameter.TypeKind == ParameterTypeKind.Tabular)
            {
                return $"{name}: (*)";
            }
            else
            {
                // not sure what to do here if this is called for other kinds of parameters
                return $"{name}: dynamic";
            }
        }

        /// <summary>
        /// Gets the text of a parameter list of a function declaration for the specified parameters.
        /// </summary>
        public static string GetParameterListDeclaration(IReadOnlyList<Parameter> parameters)
        {
            return $"({string.Join(", ", parameters.Select(p => GetDeclaration(p)))})";
        }

        /// <summary>
        /// Gets the text of a table schema as it would appear as a type of a parameter in a function declaration.
        /// </summary>
        private static string GetTableSchemaDeclaration(TableSymbol table)
        {
            if (table.Columns.Count == 0)
            {
                return "(*)";
            }
            else
            {
                return $"({string.Join(", ", table.Columns.Select(c => GetColumnDeclaration(c)))})";
            }
        }

        /// <summary>
        /// Gets the text of a column as it would appear in the table type of a parameter of a function declaration.
        /// </summary>
        private static string GetColumnDeclaration(ColumnSymbol column)
        {
            return $"{KustoFacts.BracketNameIfNecessary(column.Name)}: {column.Type.Name}";
        }

        /// <summary>
        /// True if the type of the parameter is dependant on arguments.
        /// </summary>
        public bool TypeDependsOnArguments =>
            this.TypeKind != ParameterTypeKind.Declared || this.DeclaredTypes[0].IsTabular;

        public Tabularity Tabularity
        {
            get
            {
                switch (this.TypeKind)
                {
                    case ParameterTypeKind.Declared:
                        return this.DeclaredTypes[0].Tabularity;
                    case ParameterTypeKind.Tabular:
                    case ParameterTypeKind.Database:
                    case ParameterTypeKind.Cluster:
                        return Tabularity.Tabular;
                    default:
                        return Tabularity.Scalar;
                }
            }
        }

        public bool IsScalar => this.Tabularity == Tabularity.Scalar;

        public bool IsTabular => this.Tabularity == Tabularity.Tabular;
    }
}