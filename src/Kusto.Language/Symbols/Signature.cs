using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Symbols
{
    using Parsing;
    using Syntax;
    using Utils;

    public delegate TypeSymbol CustomReturnType(TableSymbol table, IReadOnlyList<Expression> arguments, Signature signature);
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
        /// The declaration of the function.
        /// </summary>
        public FunctionDeclaration Declaration { get; }

        /// <summary>
        /// A custom function that evaluates the return type of this signature.
        /// </summary>
        public CustomReturnType CustomReturnType { get; }

        /// <summary>
        /// If true, this signature is hidden from intellisense
        /// </summary>
        public bool IsHidden { get; }

        /// <summary>
        /// If true, this signature can have a variable number of arguments.
        /// </summary>
        public bool HasRepeatableParameters => _firstRepeatableParameter >= 0;

        private readonly sbyte _firstRepeatableParameter;
        private readonly sbyte _lastRepeatableParameter;

        /// <summary>
        /// True if any parameter type can vary depending on the argument types in use.
        /// </summary>
        private bool ReturnTypeDependsOnArguments { get; }

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
                && tabularity == Tabularity.Unknown)
            {
                this._tabularity = returnType.Tabularity;
            }

            this.ReturnTypeDependsOnArguments =
                returnKind != ReturnTypeKind.Declared
                && this.Parameters.Count > 0
                && this.Parameters.Any(p => p.TypeDependsOnArguments);

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
        /// Creates a signature just like this one, but is hidden from intellisense.
        /// </summary>
        public Signature Hide()
        {
            return new Signature(this.ReturnKind, this._returnType, this._body, this.Declaration, this.CustomReturnType, this._tabularity, this.Parameters, isHidden: true);
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
        /// Gets the parameter to use with an argument at the specified index.
        /// If the function has a variable number of parameters, this function can return the same symbol for multiple argument indices.
        /// </summary>
        public Parameter GetParameter(int argumentIndex, int argumentCount)
        {
            if (this.HasRepeatableParameters)
            {
                if (argumentIndex < this._firstRepeatableParameter)
                {
                    // before repeatable parameters
                    return this.Parameters[argumentIndex];
                }
                else if (argumentIndex > this._lastRepeatableParameter
                    && this._lastRepeatableParameter < this.Parameters.Count - 1
                    && (argumentCount - argumentIndex) < (this.Parameters.Count - this._lastRepeatableParameter))
                {
                    // after the repeatable parameters
                    var iAfterRepeatableParam = this.Parameters.Count - (argumentCount - argumentIndex);
                    return this.Parameters[iAfterRepeatableParam];
                }
                else
                {
                    // within repeatable parameters
                    var nRepeatable = this._lastRepeatableParameter - this._firstRepeatableParameter + 1;
                    var iparam = ((argumentIndex - this._firstRepeatableParameter) % nRepeatable) + this._firstRepeatableParameter;
                    return this.Parameters[iparam];
                }
            }
            else
            {
                return argumentIndex < this.Parameters.Count
                    ? this.Parameters[argumentIndex]
                    : null;
            }
        }

        /// <summary>
        /// Gets the set of possible parameters for signatures with repeatable parameter blocks and
        /// with possibly incomplete argument lists.
        /// </summary>
        public void GetPossibleParameters(int argumentIndex, int argumentCount, List<Parameter> possible)
        {
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

        /// <summary>
        /// Gets the parameter given the parameter name.
        /// </summary>
        public Parameter GetParameter(string name)
        {
            foreach (var p in this.Parameters)
            {
                if (p.Name == name)
                {
                    return p;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the parameter corresponding to the specific argument.
        /// </summary>
        public Parameter GetParameter(Expression argument, int argumentIndex, int argumentCount)
        {
            if (argument is SimpleNamedExpression n)
            {
                return GetParameter(n.Name.SimpleName);
            }

            return GetParameter(argumentIndex, argumentCount);
        }

        /// <summary>
        /// Gets the first argument index for the corresponding parameter.
        /// </summary>
        public int GetArgumentIndex(Parameter parameter, IReadOnlyList<Expression> arguments)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                var arg = arguments[i];
                var p = GetParameter(arg, i, arguments.Count);
                if (p == parameter)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Gets the range of argument indices for the corresponding parameter.
        /// </summary>
        public void GetArgumentRange(Parameter parameter, IReadOnlyList<Expression> arguments, out int start, out int length)
        {
            start = GetArgumentIndex(parameter, arguments);

            if (start >= 0)
            {
                length = 1;

                for (int i = start + 1; i < arguments.Count; i++, length++)
                {
                    var p = GetParameter(i, arguments.Count);
                    if (p != parameter)
                        break;
                }
            }
            else
            {
                length = 0;
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
                            return this.GetParameter(0, this.Parameters.Count)?.Tabularity ?? Tabularity.Unknown;
                        case ReturnTypeKind.Parameter1:
                            return this.GetParameter(1, this.Parameters.Count)?.Tabularity ?? Tabularity.Unknown;
                        case ReturnTypeKind.Parameter2:
                            return this.GetParameter(2, this.Parameters.Count)?.Tabularity ?? Tabularity.Unknown;
                        case ReturnTypeKind.ParameterN:
                            return this.GetParameter(this.Parameters.Count - 1, this.Parameters.Count)?.Tabularity ?? Tabularity.Unknown;
                        case ReturnTypeKind.Custom:
                            return Tabularity.Unknown;
                        case ReturnTypeKind.Parameter0Table:
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

            return $"{Symbol.Name}({builder.ToString()}) => {this.ReturnTypeDisplay}";
        }

        /// <summary>
        /// Determines the tabularity of signatures with computed return types.
        /// </summary>
        public void ComputeTabularity(GlobalState globals)
        {
            if (this.ReturnKind != ReturnTypeKind.Custom || _tabularity != Tabularity.Unknown)
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

        internal FunctionBodyFacts? FunctionBodyFacts { get; set; }
        internal TypeSymbol NonVariableComputedReturnType { get; set; }

        internal bool HasVariableReturnType =>
            FunctionBodyFacts != null && (FunctionBodyFacts & Symbols.FunctionBodyFacts.VariableReturn) != 0;

        internal bool HasClusterCall =>
            FunctionBodyFacts != null && (FunctionBodyFacts & Symbols.FunctionBodyFacts.Cluster) != 0;

        internal bool HasDatabaseCall =>
            FunctionBodyFacts != null && (FunctionBodyFacts & Symbols.FunctionBodyFacts.Database) != 0;
    }

    [Flags]
    internal enum FunctionBodyFacts
    {
        None                = 0b_0000_0000,

        Cluster             = 0b_0000_0001,
        Database            = 0b_0000_0010,
        Table               = 0b_0000_0100,

        VariableReturn      = 0b_0001_0000,
    }
}