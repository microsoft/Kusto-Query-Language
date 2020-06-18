using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Symbols
{
    using Parsing;
    using Syntax;
    using Utils;

    /// <summary>
    /// A function that determines the a function's return type given the list of arguments.
    /// </summary>
    public delegate TypeSymbol CustomReturnType(TableSymbol table, IReadOnlyList<Expression> arguments, Signature signature);

    /// <summary>
    /// A function that determines the a function's return type given the list of arguments.
    /// </summary>
    public delegate TypeSymbol CustomReturnTypeShort(TableSymbol table, IReadOnlyList<Expression> arguments);

    /// <summary>
    /// A function that builds a list of parameters associated with each argument.
    /// </summary>
    public delegate void CustomArgumentParametersBuilder(Signature signature, IReadOnlyList<Expression> arguments, List<Parameter> argumentParameters);

    /// <summary>
    /// The parameters and return type of a function-like symbol.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Display}")]
    public class Signature
    {
        /// <summary>
        /// The symbol this is a signature for.
        /// </summary>
        public Symbol Symbol { get; internal set; }

        /// <summary>
        /// The approach by which the return type is determined.
        /// </summary>
        public ReturnTypeKind ReturnKind { get; }

        /// <summary>
        /// The declared parameters.
        /// </summary>
        public IReadOnlyList<Parameter> Parameters { get; }

        /// <summary>
        /// The minimum number of arguments an invocation of this signature can have.
        /// </summary>
        public int MinArgumentCount { get; }

        /// <summary>
        /// The maximum number of arguments an invocation of this signature can have.
        /// </summary>
        public int MaxArgumentCount { get; }

        /// <summary>
        /// The declaration of the function.
        /// </summary>
        public FunctionDeclaration Declaration { get; }

        /// <summary>
        /// A custom function that evaluates the return type of this signature.
        /// </summary>
        public CustomReturnType CustomReturnType { get; }

        /// <summary>
        /// A custom function that associates parameters to input arguments.
        /// </summary>
        public CustomArgumentParametersBuilder CustomArgumentParametersBuilder { get; }

        /// <summary>
        /// If true, this signature is hidden from intellisense
        /// </summary>
        public bool IsHidden { get; }

        /// <summary>
        /// If true, this signature can have a variable number of arguments.
        /// </summary>
        public bool HasRepeatableParameters => _firstRepeatableParameter >= 0;

        public bool HasOptionalParameters { get; }
        public bool HasAggregateParameters { get; }

        private readonly sbyte _firstRepeatableParameter;
        private readonly sbyte _lastRepeatableParameter;

        private TypeSymbol _returnType;
        private string _body;
        private Tabularity _tabularity;

        private Signature(
            ReturnTypeKind returnKind,
            TypeSymbol returnType,
            string body,
            FunctionDeclaration declaration,
            CustomReturnType customReturnType,
            Tabularity tabularity,
            IReadOnlyList<Parameter> parameters,
            CustomArgumentParametersBuilder customParameterListBuilder = null,
            bool isHidden = false)
        {
            if (returnKind == ReturnTypeKind.Declared && returnType == null)
                throw new ArgumentNullException(nameof(returnType));

            if (returnKind == ReturnTypeKind.Computed && !(body != null | declaration != null))
                throw new ArgumentNullException(nameof(body));

            if (returnKind == ReturnTypeKind.Custom && customReturnType == null)
                throw new ArgumentNullException(nameof(customReturnType));

            this.ReturnKind = returnKind;
            this._returnType = returnType;
            this._body = body;
            this.Declaration = declaration;
            this.CustomReturnType = customReturnType;
            this._tabularity = tabularity;
            this.Parameters = parameters.ToReadOnly();
            this.CustomArgumentParametersBuilder = customParameterListBuilder;
            this.IsHidden = isHidden;

            if (returnKind == ReturnTypeKind.Computed
                && returnType != null
                && tabularity == Tabularity.Unknown)
            {
                this._tabularity = returnType.Tabularity;
            }

            this._firstRepeatableParameter = -1;
            this._lastRepeatableParameter = -1;

            int minArgumentCount = 0;
            int maxArgumentCount = 0;

            for (sbyte i = 0, n = (sbyte)this.Parameters.Count; i < n; i++)
            {
                var p = parameters[i];

                if (p.IsRepeatable)
                {
                    if (this._firstRepeatableParameter == -1)
                        this._firstRepeatableParameter = i;
                    this._lastRepeatableParameter = i;
                }

                if (p.IsOptional)
                {
                    this.HasOptionalParameters = true;
                }

                if (p.ArgumentKind == ArgumentKind.Aggregate)
                {
                    this.HasAggregateParameters = true;
                }

                minArgumentCount += p.MinOccurring;
                maxArgumentCount += p.MaxOccurring;
            }

            this.MinArgumentCount = minArgumentCount;
            this.MaxArgumentCount = maxArgumentCount;
        }

        public Signature(ReturnTypeKind returnKind, IReadOnlyList<Parameter> parameters)
             : this(returnKind, null, null, null, null, Tabularity.Unknown, parameters)
        {
        }

        public Signature(ReturnTypeKind returnKind, params Parameter[] parameters)
             : this(returnKind, null, null, null, null, Tabularity.Unknown, parameters)
        {
        }

        public Signature(TypeSymbol returnType, IReadOnlyList<Parameter> parameters)
            : this(ReturnTypeKind.Declared, returnType, null, null, null, Tabularity.Unknown, parameters)
        {
            if (returnType == null)
                throw new ArgumentNullException(nameof(returnType));
        }

        public Signature(TypeSymbol returnType, params Parameter[] parameters)
            : this(ReturnTypeKind.Declared, returnType, null, null, null, Tabularity.Unknown, parameters)
        {
            if (returnType == null)
                throw new ArgumentNullException(nameof(returnType));
        }

        public Signature(CustomReturnType customReturnType, Tabularity tabularity, IReadOnlyList<Parameter> parameters)
            : this(ReturnTypeKind.Custom, null, null, null, customReturnType, tabularity, parameters)
        {
            if (customReturnType == null)
                throw new ArgumentNullException(nameof(customReturnType));
        }

        public Signature(CustomReturnType customReturnType, Tabularity tabularity, params Parameter[] parameters)
            : this(ReturnTypeKind.Custom, null, null, null, customReturnType, tabularity, parameters)
        {
            if (customReturnType == null)
                throw new ArgumentNullException(nameof(customReturnType));
        }

        public Signature(CustomReturnTypeShort customReturnType, Tabularity tabularity, IReadOnlyList<Parameter> parameters)
            : this((table, args, signature) => customReturnType(table, args), tabularity, parameters)
        {
        }

        public Signature(CustomReturnTypeShort customReturnType, Tabularity tabularity, params Parameter[] parameters)
            : this((table, args, signature) => customReturnType(table, args), tabularity, parameters)
        {
        }

        public Signature(string body, Tabularity tabularity, IReadOnlyList<Parameter> parameters)
            : this(ReturnTypeKind.Computed, null, body, null, null, tabularity, parameters)
        {
        }

        public Signature(string body, Tabularity tabularity, params Parameter[] parameters)
            : this(ReturnTypeKind.Computed, null, body, null, null, tabularity, parameters)
        {
        }

        public Signature(FunctionDeclaration declaration, IReadOnlyList<Parameter> parameters)
            : this(ReturnTypeKind.Computed, declaration.Body.Expression?.ResultType as TypeSymbol, null, declaration, null, Tabularity.Unknown, parameters)
        {
        }

        public Signature(FunctionDeclaration declaration, params Parameter[] parameters)
            : this(ReturnTypeKind.Computed, declaration.Body.Expression?.ResultType as TypeSymbol, null, declaration, null, Tabularity.Unknown, parameters)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Signature"/> just like this one, but with a custom function that 
        /// builds a list of parameter associated with each argument.
        /// </summary>
        public Signature WithArgumentParametersBuilder(CustomArgumentParametersBuilder customBuilder)
        {
            return new Signature(this.ReturnKind, this._returnType, this._body, this.Declaration, this.CustomReturnType, this._tabularity, this.Parameters, customBuilder, this.IsHidden);
        }

        /// <summary>
        /// Creates a <see cref="Signature"/> just like this one, but is hidden from intellisense.
        /// </summary>
        public Signature Hide()
        {
            return new Signature(this.ReturnKind, this._returnType, this._body, this.Declaration, this.CustomReturnType, this._tabularity, this.Parameters, this.CustomArgumentParametersBuilder, isHidden: true);
        }

        /// <summary>
        /// The body of the function, if declared within the query or database.
        /// </summary>
        public string Body
        {
            get
            {
                if (this._body == null && this.Declaration != null)
                {
                    this._body = this.Declaration.Body.ToString(IncludeTrivia.Interior);
                }

                return this._body;
            }
        }

        /// <summary>
        /// The return type if specified as part of the signature.
        /// </summary>
        public TypeSymbol DeclaredReturnType =>
            this.ReturnKind == ReturnTypeKind.Declared ? this._returnType : null;

        /// <summary>
        /// Gets the set of possible parameters for signatures with repeatable parameter blocks and
        /// with possibly incomplete argument lists.
        /// </summary>
        public void GetPossibleParameters(int argumentIndex, int argumentCount, List<Parameter> possible)
        {
            // TODO: update this to consider custom argument parameter rules/layouts

            if (this.HasRepeatableParameters)
            {
                if (argumentIndex < this._firstRepeatableParameter)
                {
                    possible.Add(this.Parameters[argumentIndex]);
                }
                else
                {
                    var nRepeatable = this._lastRepeatableParameter - this._firstRepeatableParameter + 1;
                    var iRepeatableParam = ((argumentIndex - this._firstRepeatableParameter) % nRepeatable) + this._firstRepeatableParameter;
                    possible.Add(this.Parameters[iRepeatableParam]);

                    // if there exists at least one full block of repeatable parameters
                    // and there are non-repeatable parameters following the repeatable parameters
                    // determine the after parameter this could be
                    if (argumentIndex > this._lastRepeatableParameter
                        && this._lastRepeatableParameter < this.Parameters.Count - 1)
                    {
                        var remainder = (argumentIndex - this._firstRepeatableParameter) % nRepeatable;

                        // include all possible after-repeatable parameters
                        // if repeating size < number of after-repeatable parameters then the number of repeats can be variable
                        // assuming that the argumentCount is potentially a partial set of arguments (during typing)
                        for (int iAfterParam = this._lastRepeatableParameter + remainder; iAfterParam < this.Parameters.Count; iAfterParam += nRepeatable)
                        {
                            possible.Add(this.Parameters[iAfterParam]);
                        }
                   }
                }
            }
            else if (argumentIndex < this.Parameters.Count)
            {
                possible.Add(this.Parameters[argumentIndex]);
            }
        }

        private Dictionary<string, Parameter> nameToParameterMap;

        /// <summary>
        /// Gets the parameter given the parameter name.
        /// </summary>
        public Parameter GetParameter(string name)
        {
            if (this.nameToParameterMap == null)
            {
                var map = new Dictionary<string, Parameter>(this.Parameters.Count);

                foreach (var p in this.Parameters)
                {
                    map[p.Name] = p;
                }

                this.nameToParameterMap = map;
            }

            this.nameToParameterMap.TryGetValue(name, out var parameter);
            return parameter;
        }

        private static readonly Parameter UnknownParameter = new Parameter("", ScalarTypes.Unknown);

        /// <summary>
        /// True if the function allows named arguments
        /// </summary>
        public bool AllowsNamedArguments => !(this.Symbol is FunctionSymbol fn && GlobalState.Default.IsBuiltInFunction(fn));

        /// <summary>
        /// Builds a list of parameters as associated with the specified arguments.
        /// </summary>
        public List<Parameter> GetArgumentParameters(IReadOnlyList<Expression> arguments, bool? respectNamedArguments = null)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            var argumentParameters = new List<Parameter>();
            GetArgumentParameters(arguments, argumentParameters, respectNamedArguments);
            return argumentParameters;
        }

        /// <summary>
        /// Builds a list of parameters as associated with the specified arguments.
        /// </summary>
        public void GetArgumentParameters(IReadOnlyList<Expression> arguments, List<Parameter> argumentParameters, bool? respectNamedArguments = null)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            if (argumentParameters == null)
                throw new ArgumentNullException(nameof(argumentParameters));

            this.GetArgumentParameters(arguments.Count, arguments, argumentParameters, respectNamedArguments);
        }

        /// <summary>
        /// Builds a list of parameters as associated with the specified number of arguments.
        /// </summary>
        public void GetArgumentParameters(int nArguments, List<Parameter> argumentParameters)
        {
            if (argumentParameters == null)
                throw new ArgumentNullException(nameof(argumentParameters));

            this.GetArgumentParameters(nArguments, null, argumentParameters);
        }

        /// <summary>
        /// Builds a list of parameters as associated with the specified arguments.
        /// </summary>
        private void GetArgumentParameters(int nArguments, IReadOnlyList<Expression> arguments, List<Parameter> argumentParameters, bool? respectNamedArguments = null)
        {
            bool namedArgumentsAllowed = respectNamedArguments ?? AllowsNamedArguments;

            if (this.CustomArgumentParametersBuilder != null && arguments != null)
            {
                this.CustomArgumentParametersBuilder(this, arguments, argumentParameters);
            }
            else if (this.HasRepeatableParameters)
            {
                int firstRepeatingArgument = this._firstRepeatableParameter;
                var numberOfRepeatingParameters = (this._lastRepeatableParameter - this._firstRepeatableParameter + 1);
                var minRepeats = this.Parameters[this._firstRepeatableParameter].MinOccurring;
                var possibleRepeatingArguments = (arguments.Count - firstRepeatingArgument) - (this.Parameters.Count - this._lastRepeatableParameter);
                var minRepeatingArguments = numberOfRepeatingParameters * minRepeats;
                var repeatingArgumentGroups = (possibleRepeatingArguments + numberOfRepeatingParameters - 1) / numberOfRepeatingParameters;
                var totalRepeatingArguments = Math.Max(repeatingArgumentGroups * numberOfRepeatingParameters, minRepeatingArguments);
                var lastRepeatingArgument = firstRepeatingArgument + totalRepeatingArguments;

                for (int i = 0; i < nArguments; i++)
                {
                    if (namedArgumentsAllowed && arguments != null && arguments[i] is SimpleNamedExpression sn)
                    {
                        argumentParameters.Add(GetParameter(sn.Name.SimpleName));
                    }
                    else if (i < firstRepeatingArgument)
                    {
                        argumentParameters.Add(this.Parameters[i]);
                    }
                    else if (i >= firstRepeatingArgument && i < lastRepeatingArgument)
                    {
                        argumentParameters.Add(this.Parameters[this._firstRepeatableParameter + (i % numberOfRepeatingParameters)]);
                    }
                    else if (i >= lastRepeatingArgument && i < arguments.Count)
                    {
                        argumentParameters.Add(this.Parameters[this._lastRepeatableParameter + (i - lastRepeatingArgument)]);
                    }
                    else
                    {
                        argumentParameters.Add(UnknownParameter);
                    }
                }
            }
            else
            {
                for (int i = 0; i < nArguments; i++)
                {
                    if (namedArgumentsAllowed && arguments != null && arguments[i] is SimpleNamedExpression sn)
                    {
                        argumentParameters.Add(GetParameter(sn.Name.SimpleName));
                    }
                    else if (i < this.Parameters.Count)
                    {
                        argumentParameters.Add(this.Parameters[i]);
                    }
                    else
                    {
                        argumentParameters.Add(UnknownParameter);
                    }
                }
            }
        }

        public Tabularity Tabularity
        {
            get
            {
                if (this._tabularity == Tabularity.Unknown)
                {
                    switch (this.ReturnKind)
                    {
                        case ReturnTypeKind.Declared:
                            return this._returnType.Tabularity;
                        case ReturnTypeKind.Computed:
                            return Tabularity.Unknown;
                        case ReturnTypeKind.Parameter0:
                            return this.Parameters.Count > 0 ? this.Parameters[0].Tabularity : Tabularity.Unknown;
                        case ReturnTypeKind.Parameter1:
                            return this.Parameters.Count > 1 ? this.Parameters[1].Tabularity : Tabularity.Unknown;
                        case ReturnTypeKind.Parameter2:
                            return this.Parameters.Count > 2 ? this.Parameters[2].Tabularity : Tabularity.Unknown;
                        case ReturnTypeKind.ParameterN:
                            return this.Parameters.Count > 0 ? this.Parameters[this.Parameters.Count - 1].Tabularity : Tabularity.Unknown;
                        case ReturnTypeKind.Custom:
                            return Tabularity.Unknown;
                        case ReturnTypeKind.Parameter0Table:
                        case ReturnTypeKind.Parameter0ExternalTable:
                        case ReturnTypeKind.Parameter0MaterializedView:
                        case ReturnTypeKind.Parameter0Database:
                        case ReturnTypeKind.Parameter0Cluster:
                            return Tabularity.Tabular;
                        default:
                            return Tabularity.Scalar;
                    }
                }

                return this._tabularity;
            }
        }

        public bool IsScalar
        {
            get
            {
                switch (this.Tabularity)
                {
                    case Tabularity.Scalar:
                    case Tabularity.Unknown:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsTabular
        {
            get
            {
                switch (this.Tabularity)
                {
                    case Tabularity.Tabular:
                    case Tabularity.Unknown:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool IsValidArgumentCount(int argumentCount)
        {
            if (argumentCount < this.MinArgumentCount || argumentCount > this.MaxArgumentCount)
                return false;

            if (this.HasRepeatableParameters)
            {
                // argument count must include an even multiple of the repeating parameters
                var nVariable = (this._lastRepeatableParameter - this._firstRepeatableParameter + 1);
                return (argumentCount - this.Parameters.Count) % nVariable == 0;
            }

            return true;
        }

        public string ReturnTypeDisplay
        {
            get
            {
                switch (this.ReturnKind)
                {
                    case ReturnTypeKind.Declared:
                        return this._returnType.Display;
                    default:
                        return "<" + this.ReturnKind.ToString() + ">";
                }
            }
        }

        private string _display;

        public string Display
        {
            get
            {
                if (this._display == null)
                {
                    this._display = this.GetDisplay();
                }

                return this._display;
            }
        }

        private string GetDisplay()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < this.Parameters.Count; i++)
            {
                var p = this.Parameters[i];

                if (i > 0)
                {
                    builder.Append(", ");
                }

                if (p.IsOptional)
                {
                    builder.Append("[");
                    builder.Append(p.Display);
                    builder.Append("]");
                }
                else
                {
                    builder.Append(p.Display);
                }
            }

            if (this.HasRepeatableParameters)
            {
                builder.Append(", ...");
            }

            return $"{Symbol.Name}({builder}) => {this.ReturnTypeDisplay}";
        }

        /// <summary>
        /// Gets the return type for the function as best as can be determined without specific call site arguments.
        /// </summary>
        /// <param name="globals">The <see cref="GlobalState"/> in context for any computations made in determining the return type.</param>
        public TypeSymbol GetReturnType(GlobalState globals)
        {
            if (globals == null)
                throw new ArgumentNullException(nameof(globals));

            switch (this.ReturnKind)
            {
                case ReturnTypeKind.Declared:
                    return this.DeclaredReturnType;

                case ReturnTypeKind.Computed:
                    return Binding.Binder.GetComputedReturnType(this, globals);

                case ReturnTypeKind.Parameter0Cluster:
                    return new ClusterSymbol("", null, isOpen: true);

                case ReturnTypeKind.Parameter0Database:
                    return new DatabaseSymbol("", null, isOpen: true);

                case ReturnTypeKind.Parameter0Table:
                    return TableSymbol.Empty.WithIsOpen(true);

                case ReturnTypeKind.Parameter0ExternalTable:
                    return TableSymbol.Empty.WithIsOpen(true);

                case ReturnTypeKind.Parameter0MaterializedView:
                    return TableSymbol.Empty.WithIsOpen(true);

                default:
                    return this.Tabularity == Tabularity.Tabular
                        ? TableSymbol.Empty.WithIsOpen(true)
                        : (TypeSymbol)ScalarTypes.Unknown;
            }
        }

        private static readonly ObjectPool<List<Parameter>> s_parameterListPool =
            new ObjectPool<List<Parameter>>(() => new List<Parameter>(), list => list.Clear());

        /// <summary>
        /// Gets the return type for the function as best as can be determined with a set of hypothetical argument types.
        /// </summary>
        /// <param name="globals">The <see cref="GlobalState"/> in context for any computations made in determining the return type.</param>
        /// <param name="argumentTypes">A list of types for hypothetical arguments.</param>
        public TypeSymbol GetReturnType(GlobalState globals, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            if (globals == null)
                throw new ArgumentNullException(nameof(globals));

            if (argumentTypes == null)
                throw new ArgumentNullException(nameof(argumentTypes));

            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                this.GetArgumentParameters(argumentTypes.Count, argumentParameters);
                return GetReturnType(globals, argumentTypes, argumentParameters);
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters);
            }
        }

        /// <summary>
        /// Gets the return type for the function as best as can be determined with a set of hypothetical argument types
        /// and corresponding their parameters.
        /// </summary>
        /// <param name="globals">The <see cref="GlobalState"/> in context for any computations made in determining the return type.</param>
        /// <param name="argumentTypes">A list of types for hypothetical arguments.</param>
        /// <param name="argumentParameters">A list of the parameters associated with each hypothetical argument.</param>
        public TypeSymbol GetReturnType(
            GlobalState globals,
            IReadOnlyList<TypeSymbol> argumentTypes,
            IReadOnlyList<Parameter> argumentParameters)
        {
            if (globals == null)
                throw new ArgumentNullException(nameof(globals));

            if (argumentParameters == null)
                throw new ArgumentNullException(nameof(argumentParameters));

            if (argumentTypes == null)
                throw new ArgumentNullException(nameof(argumentTypes));

            switch (this.ReturnKind)
            {
                case ReturnTypeKind.Declared:
                    return this.DeclaredReturnType;

                case ReturnTypeKind.Computed:
                    return Binding.Binder.GetComputedReturnType(this, globals, argumentTypes);

                case ReturnTypeKind.Parameter0:
                    var iArg = argumentParameters.IndexOf(this.Parameters[0]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? argumentTypes[iArg] : ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter1:
                    iArg = argumentParameters.IndexOf(this.Parameters[1]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? argumentTypes[iArg] : ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter2:
                    iArg = argumentParameters.IndexOf(this.Parameters[2]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? argumentTypes[iArg] : ErrorSymbol.Instance;

                case ReturnTypeKind.ParameterN:
                    iArg = argumentParameters.IndexOf(this.Parameters[this.Parameters.Count - 1]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? argumentTypes[iArg] : ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter0Promoted:
                    iArg = argumentParameters.IndexOf(this.Parameters[0]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? Binding.Binder.Promote(argumentTypes[iArg]) : ErrorSymbol.Instance;

                case ReturnTypeKind.Common:
                    return Binding.Binder.GetCommonArgumentType(argumentParameters, argumentTypes) ?? ErrorSymbol.Instance;

                case ReturnTypeKind.Widest:
                    return Binding.Binder.GetWidestArgumentType(this, argumentTypes) ?? ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter0Cluster:
                    return new ClusterSymbol("", null, isOpen: true);

                case ReturnTypeKind.Parameter0Database:
                    return new DatabaseSymbol("", null, isOpen: true);

                case ReturnTypeKind.Parameter0Table:
                    return TableSymbol.Empty.WithIsOpen(true);

                case ReturnTypeKind.Parameter0ExternalTable:
                    return TableSymbol.Empty.WithIsOpen(true);

                case ReturnTypeKind.Parameter0MaterializedView:
                    return TableSymbol.Empty.WithIsOpen(true);

                default:
                    return this.Tabularity == Tabularity.Tabular
                        ? TableSymbol.Empty.WithIsOpen(true)
                        : (TypeSymbol)ScalarTypes.Unknown;
            }
        }

        /// <summary>
        /// Determines the tabularity of signatures with computed return types.
        /// </summary>
        /// <param name="globals">The <see cref="GlobalState"/> in context for any computations made in determining the tabularity.</param>
        public void ComputeTabularity(GlobalState globals)
        {
            if (this.ReturnKind != ReturnTypeKind.Computed || _tabularity != Tabularity.Unknown)
                return;

            var type = Binding.Binder.GetComputedReturnType(this, globals);

            if (this._tabularity == Tabularity.Unknown && type != null)
            {
                if (type.IsTabular)
                {
                    this._tabularity = Tabularity.Tabular;
                }
                else if (type.IsScalar)
                {
                    this._tabularity = Tabularity.Scalar;
                }
            }
        }

        /// <summary>
        /// Facts determined about the function body when analyzed
        /// </summary>
        internal FunctionBodyFacts? FunctionBodyFacts { get; set; }

        /// <summary>
        /// The computed return type is saved here when it is determined to be fixed relative to argument values.
        /// </summary>
        internal TypeSymbol NonVariableComputedReturnType { get; set; }

        /// <summary>
        /// True if the signature has a variable return type (is dependant on argument values)
        /// </summary>
        internal bool HasVariableReturnType =>
            FunctionBodyFacts != null && (FunctionBodyFacts & Symbols.FunctionBodyFacts.VariableReturn) != 0;

        /// <summary>
        /// True if the function body has a call to the cluster method, or a function invoked within that does
        /// </summary>
        internal bool HasClusterCall =>
            FunctionBodyFacts != null && (FunctionBodyFacts & Symbols.FunctionBodyFacts.Cluster) != 0;

        /// <summary>
        /// True if the function body has a call to the database method, or a function invoked within that does
        /// </summary>
        internal bool HasDatabaseCall =>
            FunctionBodyFacts != null && (FunctionBodyFacts & Symbols.FunctionBodyFacts.Database) != 0;
    }

    [Flags]
    internal enum FunctionBodyFacts
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
        Table = 0b_0000_0100,

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