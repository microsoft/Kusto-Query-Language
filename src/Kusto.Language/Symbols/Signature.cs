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
        /// The declaration of the body of the function in source.
        /// </summary>
        public FunctionBody Declaration { get; }

        /// <summary>
        /// A custom function that evaluates the return type of this signature.
        /// </summary>
        public CustomReturnType CustomReturnType { get; }

        /// <summary>
        /// The rule that describes how parameters are associated with arguments
        /// </summary>
        public ParameterLayout Layout { get; }

        /// <summary>
        /// If true, this signature is hidden from intellisense
        /// </summary>
        public bool IsHidden { get; }

        /// <summary>
        /// If true, this signature can have a variable number of arguments.
        /// </summary>
        public bool HasRepeatableParameters { get; }

        /// <summary>
        /// If true, this signature has optional parameters.
        /// </summary>
        public bool HasOptionalParameters { get; }

        /// <summary>
        /// If true, this signature has parameters that are expected to be aggregates.
        /// </summary>
        public bool HasAggregateParameters { get; }

        private TypeSymbol _returnType;
        private string _body;
        private Tabularity _tabularity;

        private Signature(
            ReturnTypeKind returnKind,
            TypeSymbol returnType,
            string body,
            FunctionBody declaration,
            CustomReturnType customReturnType,
            Tabularity tabularity,
            IReadOnlyList<Parameter> parameters,
            ParameterLayout layout = null,
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
            this.IsHidden = isHidden;

            if (returnKind == ReturnTypeKind.Computed
                && returnType != null
                && tabularity == Tabularity.Unspecified)
            {
                this._tabularity = returnType.Tabularity;
            }

            int minArgumentCount = 0;
            int maxArgumentCount = 0;

            foreach (var p in this.Parameters)
            {
                if (p.IsRepeatable)
                {
                    this.HasRepeatableParameters = true;
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

            if (layout != null)
            {
                this.Layout = layout;
            }
            else if (this.HasRepeatableParameters)
            {
                this.Layout = ParameterLayouts.Repeating;
            }
            else
            {
                this.Layout = ParameterLayouts.Fixed;
            }
        }

        public Signature(ReturnTypeKind returnKind, IReadOnlyList<Parameter> parameters)
             : this(returnKind, null, null, null, null, Tabularity.Unspecified, parameters)
        {
        }

        public Signature(ReturnTypeKind returnKind, params Parameter[] parameters)
             : this(returnKind, null, null, null, null, Tabularity.Unspecified, parameters)
        {
        }

        public Signature(TypeSymbol returnType, IReadOnlyList<Parameter> parameters)
            : this(ReturnTypeKind.Declared, returnType, null, null, null, Tabularity.Unspecified, parameters)
        {
            if (returnType == null)
                throw new ArgumentNullException(nameof(returnType));
        }

        public Signature(TypeSymbol returnType, params Parameter[] parameters)
            : this(ReturnTypeKind.Declared, returnType, null, null, null, Tabularity.Unspecified, parameters)
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

        public Signature(FunctionBody declaration, IReadOnlyList<Parameter> parameters)
            : this(ReturnTypeKind.Computed, declaration.Expression?.ResultType as TypeSymbol, null, declaration, null, Tabularity.Unspecified, parameters)
        {
        }

        public Signature(FunctionBody declaration, params Parameter[] parameters)
            : this(ReturnTypeKind.Computed, declaration.Expression?.ResultType as TypeSymbol, null, declaration, null, Tabularity.Unspecified, parameters)
        {
        }

        /// <summary>
        /// Creates a new <see cref="Signature"/> just like this one, but with a custom function that 
        /// builds a list of parameter associated with each argument.
        /// </summary>
        public Signature WithLayout(ParameterLayoutBuilder customBuilder)
        {
            return WithLayout(ParameterLayouts.Custom(customBuilder));
        }

        /// <summary>
        /// Creates a new <see cref="Signature"/> just like this one, but with a custom function that 
        /// builds a list of parameter associated with each argument.
        /// </summary>
        public Signature WithLayout(ParameterLayout layout)
        {
            return new Signature(this.ReturnKind, this._returnType, this._body, this.Declaration, this.CustomReturnType, this._tabularity, this.Parameters, layout, this.IsHidden);
        }

        /// <summary>
        /// Creates a <see cref="Signature"/> just like this one, but is hidden from intellisense.
        /// </summary>
        public Signature Hide()
        {
            return new Signature(this.ReturnKind, this._returnType, this._body, this.Declaration, this.CustomReturnType, this._tabularity, this.Parameters, this.Layout, isHidden: true);
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
                    this._body = this.Declaration.ToString(IncludeTrivia.Interior);
                }

                return this._body;
            }
        }

        /// <summary>
        /// The return type if specified as part of the signature.
        /// </summary>
        public TypeSymbol DeclaredReturnType =>
            this.ReturnKind == ReturnTypeKind.Declared ? this._returnType : null;

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

        /// <summary>
        /// A parameter used to associate with an argument when no declared parameter matches.
        /// </summary>
        public static readonly Parameter UnknownParameter = new Parameter("", ScalarTypes.Unknown);

        /// <summary>
        /// True if the function allows named arguments
        /// </summary>
        public bool AllowsNamedArguments => !(this.Symbol is FunctionSymbol fn && GlobalState.Default.IsBuiltInFunction(fn));

        /// <summary>
        /// Builds a list of parameters as associated with the specified arguments.
        /// </summary>
        public List<Parameter> GetArgumentParameters(IReadOnlyList<Expression> arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            var argumentParameters = new List<Parameter>();
            GetArgumentParameters(arguments, argumentParameters);
            return argumentParameters;
        }

        /// <summary>
        /// Builds a list of parameters associated with the specified arguments.
        /// </summary>
        public void GetArgumentParameters(IReadOnlyList<Expression> arguments, List<Parameter> argumentParameters)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            if (argumentParameters == null)
                throw new ArgumentNullException(nameof(argumentParameters));

            this.Layout.GetArgumentParameters(this, arguments, argumentParameters);
        }

        /// <summary>
        /// Builds a list of parameters associated with the specified argument types.
        /// </summary>
        public void GetArgumentParameters(IReadOnlyList<TypeSymbol> argumentTypes, List<Parameter> argumentParameters)
        {
            if (argumentTypes == null)
                throw new ArgumentNullException(nameof(argumentTypes));

            if (argumentParameters == null)
                throw new ArgumentNullException(nameof(argumentParameters));

            this.Layout.GetArgumentParameters(this, argumentTypes, argumentParameters);
        }

        /// <summary>
        /// Gets the set of possible parameters that could occur after the specified arguments
        /// </summary>
        public void GetNextPossibleParameters(IReadOnlyList<Expression> arguments, List<Parameter> possibleParameters)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            if (possibleParameters == null)
                throw new ArgumentNullException(nameof(possibleParameters));

            this.Layout.GetNextPossibleParameters(this, arguments, possibleParameters);
        }

        /// <summary>
        /// True if the argument count represents a valid number of arguments.
        /// </summary>
        public bool IsValidArgumentCount(int argumentCount)
        {
            return this.Layout.IsValidArgumentCount(this, argumentCount);
        }

        public Tabularity Tabularity
        {
            get
            {
                if (this._tabularity == Tabularity.Unspecified)
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

            return $"{Symbol?.Name ?? ""}({builder}) => {this.ReturnTypeDisplay}";
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
                this.GetArgumentParameters(argumentTypes, argumentParameters);
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
            if (this.ReturnKind == ReturnTypeKind.Computed
                && _tabularity == Tabularity.Unspecified)
            {
                // pre-assign to hopefully avoid duplicate computation work
                _tabularity = Tabularity.Unknown;

                // first try to deduce tabularity from syntax alone
                var syntaxTabularity = GetSyntaxTabularity(globals);
                if (syntaxTabularity != Tabularity.Unknown)
                {
                    _tabularity = syntaxTabularity;
                }
                else
                {
                    // otherwise try to fully bind and base tabularity on return type
                    var type = Binding.Binder.GetComputedReturnType(this, globals);
                    if (type != null)
                    {
                        if (type.IsTabular)
                        {
                            _tabularity = Tabularity.Tabular;
                        }
                        else if (type.IsScalar)
                        {
                            _tabularity = Tabularity.Scalar;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determine's the tabularity of this function signature from the syntax of the body.
        /// </summary>
        private Tabularity GetSyntaxTabularity(GlobalState globals)
        {
            SyntaxList<SeparatedElement<Statement>> statements = null;

            if (this.Declaration != null)
            {
                statements = this.Declaration.Statements;
            }
            else if (_body != null)
            {
                var body = _body.Trim();
                if (body.StartsWith("{"))
                {
                    body = body.Substring(1);
                    if (body.EndsWith("}"))
                        body = body.Substring(0, body.Length - 1);
                }
                var code = KustoCode.Parse(body);
                statements = code.Syntax.GetFirstDescendantOrSelf<SyntaxList<SeparatedElement<Statement>>>();
            }

            if (statements != null 
                && statements.Count > 0
                && statements[statements.Count - 1].Element is ExpressionStatement es)
            {
                switch (es.Expression)
                {
                    case QueryOperator _:  // querys operator are always tabular
                    case PipeExpression _: // pipes are always queries
                    case Command _:        // commands always have tabular output
                    case DataTableExpression _:
                    case ToTableExpression _:
                        return Tabularity.Tabular;
                    case BinaryExpression _:
                    case PrefixUnaryExpression _:
                    case InExpression _:
                    case HasAnyExpression _:
                    case HasAllExpression _:
                    case BetweenExpression _:
                    case ElementExpression _:
                    case BracketedExpression _:
                    case LiteralExpression _:
                    case CompoundStringLiteralExpression _:
                    case DynamicExpression _:
                    case ToScalarExpression _:
                        return Tabularity.Scalar;
                    case FunctionCallExpression fc:
                        var fn = globals.GetFunction(fc.Name.SimpleName);
                        if (fn != null)
                            return fn.Tabularity;
                        var dbFn = globals.Database?.GetFunction(fc.Name.SimpleName);
                        if (dbFn != null)
                            return dbFn.Tabularity;
                        return Tabularity.Unknown;
                    case Expression _:
                    default:
                        return Tabularity.Unknown;
                }
            }
            else
            {
                return Tabularity.None;
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