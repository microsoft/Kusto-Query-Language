using System.Collections.Generic;
using System.Text;

namespace Kusto.Language.Symbols
{
    using Syntax;
    using System;
    using Utils;


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

    /// <summary>
    /// The symbol for a function.
    /// </summary>
    public sealed class FunctionSymbol : TypeSymbol
    {
        public override SymbolKind Kind => SymbolKind.Function;

        /// <summary>
        /// A function can have one or more signatures.
        /// </summary>
        public IReadOnlyList<Signature> Signatures { get; }

        /// <summary>
        /// The description of the function.
        /// </summary>
        public string Description { get; }

        private readonly bool hidden;
        private readonly bool constantFoldable;
        private readonly ResultNameKind resultNameKind;
        private readonly string resultNamePrefix;

        private FunctionSymbol(
            string name, 
            IEnumerable<Signature> signatures, 
            bool hidden, 
            bool constantFoldable, 
            ResultNameKind resultNameKind, 
            string resultNamePrefix,
            string description)
            : base(name)
        {
            this.Signatures = signatures.ToReadOnly();
            this.Description = description ?? "";

            foreach (var signature in this.Signatures)
            {
                signature.Symbol = this;
            }

            this.hidden = hidden;
            this.constantFoldable = constantFoldable;
            this.resultNameKind = resultNameKind;
            this.resultNamePrefix = resultNamePrefix;
        }

        public FunctionSymbol(string name, IEnumerable<Signature> signatures, string description = null)
            : this(name, signatures, hidden: false, constantFoldable: false, resultNameKind: ResultNameKind.Default, resultNamePrefix: null, description: description)
        {
        }

        public FunctionSymbol(string name, params Signature[] signatures)
            : this(name, (IEnumerable<Signature>)signatures)
        {
        }

        public FunctionSymbol(string name, TypeSymbol returnType, IReadOnlyList<Parameter> parameters, string description = null)
            : this(name, new[] { new Signature(returnType, parameters) }, description)
        {
        }

        public FunctionSymbol(string name, TypeSymbol returnType, params Parameter[] parameters)
            : this(name, new[] { new Signature(returnType, parameters) })
        {
        }

        public FunctionSymbol(string name, ReturnTypeKind returnTypeKind, IReadOnlyList<Parameter> parameters)
            : this(name, new[] { new Signature(returnTypeKind, parameters) })
        {
        }

        public FunctionSymbol(string name, ReturnTypeKind returnTypeKind, params Parameter[] parameters)
            : this(name, new[] { new Signature(returnTypeKind, parameters) })
        {
        }

        public FunctionSymbol(string name, CustomReturnType customReturnType, Tabularity tabularity, IReadOnlyList<Parameter> parameters)
            : this(name, new[] { new Signature(customReturnType, tabularity, parameters) })
        {
        }

        public FunctionSymbol(string name, CustomReturnType customReturnType, Tabularity tabularity, params Parameter[] parameters)
            : this(name, new[] { new Signature(customReturnType, tabularity, parameters) })
        {
        }

        public FunctionSymbol(string name, CustomReturnTypeShort customReturnType, Tabularity tabularity, IReadOnlyList<Parameter> parameters)
            : this(name, (table, args, signature) => customReturnType(table, args), tabularity, parameters)
        {
        }

        public FunctionSymbol(string name, CustomReturnTypeShort customReturnType, Tabularity tabularity, params Parameter[] parameters)
            : this(name, (table, args, signature) => customReturnType(table, args), tabularity, parameters)
        {
        }

        public FunctionSymbol(string name, string body, Tabularity tabularity, IReadOnlyList<Parameter> parameters, string description = null)
            : this(name, new[] { new Signature(body, tabularity, parameters) }, description)
        {
        }

        public FunctionSymbol(string name, string body, Tabularity tabularity, params Parameter[] parameters)
            : this(name, new[] { new Signature(body, tabularity, parameters) })
        {
        }

        public FunctionSymbol(string name, string body, IReadOnlyList<Parameter> parameters, string description = null)
            : this(name, new[] { new Signature(body, Tabularity.Unknown, parameters) }, description)
        {
        }

        public FunctionSymbol(string name, string body, params Parameter[] parameters)
            : this(name, new[] { new Signature(body, Tabularity.Unknown, parameters) })
        {
        }

        public FunctionSymbol(string name, FunctionDeclaration declaration, IReadOnlyList<Parameter> parameters)
            : this(name, new[] { new Signature(declaration, parameters) })
        {
        }

        public FunctionSymbol(string name, FunctionDeclaration declaration, params Parameter[] parameters)
            : this(name, new[] { new Signature(declaration, parameters) })
        {
        }

        /// <summary>
        /// Creates a new <see cref="FunctionSymbol"/> that is hidden from completion lists.
        /// </summary>
        public FunctionSymbol Hide()
        {
            return new FunctionSymbol(
                this.Name, 
                this.Signatures, 
                hidden: true, 
                constantFoldable: this.constantFoldable, 
                resultNameKind: this.resultNameKind, 
                resultNamePrefix: this.resultNamePrefix,
                description: this.Description);
        }

        public override bool IsHidden => this.hidden || base.IsHidden;

        /// <summary>
        /// Creates a new <see cref="FunctionSymbol"/> that is a constant if all its arguments are constant.
        /// </summary>
        public FunctionSymbol ConstantFoldable()
        {
            return new FunctionSymbol(
                this.Name, 
                this.Signatures, 
                hidden: this.hidden, 
                constantFoldable: true, 
                resultNameKind: this.resultNameKind, 
                resultNamePrefix: this.resultNamePrefix,
                description: this.Description);
        }

        /// <summary>
        /// How the name of the result of this function is determined.
        /// </summary>
        public ResultNameKind ResultNameKind => this.resultNameKind;

        /// <summary>
        /// The prefix to column names generated by this function.
        /// </summary>
        public string ResultNamePrefix => this.resultNamePrefix;

        /// <summary>
        /// Creates a new <see cref="FunctionSymbol"/> that has the property <see cref="ResultNamePrefix"/> set.
        /// </summary>
        public FunctionSymbol WithResultNamePrefix(string resultNamePrefix)
        {
            if (this.ResultNamePrefix != resultNamePrefix)
            {
                return new FunctionSymbol(
                    this.Name, 
                    this.Signatures, 
                    hidden: this.hidden, 
                    constantFoldable: this.constantFoldable, 
                    resultNameKind: this.resultNameKind, 
                    resultNamePrefix: resultNamePrefix,
                    description: this.Description);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Creates a new <see cref="FunctionSymbol"/> with the property <see cref="ResultNameKind"/> set.
        /// </summary>
        public FunctionSymbol WithResultNameKind(ResultNameKind kind)
        {
            if (this.ResultNameKind != kind)
            {
                return new FunctionSymbol(
                    this.Name, 
                    this.Signatures, 
                    hidden: this.hidden, 
                    constantFoldable: this.constantFoldable, 
                    resultNameKind: kind, 
                    resultNamePrefix: this.resultNamePrefix,
                    description: this.Description);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Creates a new <see cref="FunctionSymbol"/> with the specified description.
        /// </summary>
        public FunctionSymbol WithDescription(string description)
        {
            if (this.Description != description)
            {
                return new FunctionSymbol(
                    this.Name,
                    this.Signatures,
                    hidden: this.hidden,
                    constantFoldable: this.constantFoldable,
                    resultNameKind: this.resultNameKind,
                    resultNamePrefix: this.resultNamePrefix,
                    description: description);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// This function is considered a constant if all its arguments are constants.
        /// </summary>
        public bool IsConstantFoldable => this.constantFoldable;

        public override Tabularity Tabularity => this.Signatures[0].Tabularity;

        /// <summary>
        /// Gets the return type for the function as best as can be determined without specific call site arguments.
        /// </summary>
        public TypeSymbol GetReturnType(GlobalState globals)
        {
            return this.Signatures[0].GetReturnType(globals);
        }

        /// <summary>
        /// The minimum number of arguments that this function requires.
        /// </summary>
        public int MinArgumentCount
        {
            get
            {
                var min = -1;

                foreach (var s in this.Signatures)
                {
                    min = (min == -1)
                        ? s.MinArgumentCount
                        : Math.Min(min, s.MinArgumentCount);
                }

                return min;
            }
        }

        /// <summary>
        /// The maximum number of arguments this function can take.
        /// </summary>
        public int MaxArgumentCount
        {
            get
            {
                var max = -1;

                foreach (var s in this.Signatures)
                {
                    max = (max == -1)
                        ? s.MaxArgumentCount
                        : Math.Max(max, s.MaxArgumentCount);
                }

                return max;
            }
        }

        protected override string GetDisplay()
        {
            return GetDisplay(verbose: false);
        }

        public string GetDisplay(bool verbose)
        {
            var sig = this.Signatures[0];
            var builder = new StringBuilder();

            for (int i = 0; i < sig.Parameters.Count; i++)
            {
                var p = sig.Parameters[i];

                if (i > 0)
                {
                    builder.Append(", ");
                }

                if (p.IsOptional)
                {
                    // everything after this must be optional too, so just denote the entire section as optional.
                    builder.Append("[");
                    builder.Append(GetParameterDisplay(p, verbose));
                    builder.Append("]");
                }
                else
                {
                    builder.Append(GetParameterDisplay(p, verbose));
                }
            }

            if (sig.HasRepeatableParameters)
            {
                builder.Append(", ...");
            }

            var prms = builder.ToString();

            return $"{this.Name}({prms})";
        }

        private string GetParameterDisplay(Parameter parameter, bool verbose)
        {
            if (verbose)
            {
                var typeDisplay = GetTypeDisplay(parameter);
                if (!string.IsNullOrEmpty(typeDisplay))
                {
                    return $"{parameter.Name}: {typeDisplay}";
                }
            }

            return parameter.Name;
        }

        private string GetTypeDisplay(Parameter parameter)
        {
            switch (parameter.TypeKind)
            {
                case ParameterTypeKind.Declared:
                    return parameter.TypeDisplay;
                case ParameterTypeKind.Integer:
                    return "integer";
                case ParameterTypeKind.IntegerOrDynamic:
                    return "integer|dynamic";
                case ParameterTypeKind.Number:
                    return "number";
                case ParameterTypeKind.RealOrDecimal:
                    return "real|decimal";
                case ParameterTypeKind.Summable:
                    return "summable";
                case ParameterTypeKind.StringOrDynamic:
                    return "string|dynamic";
                case ParameterTypeKind.Parameter0:
                    return GetTypeDisplay(this.Signatures[0].Parameters[0]);
                case ParameterTypeKind.Parameter1:
                    return GetTypeDisplay(this.Signatures[0].Parameters[1]);
                case ParameterTypeKind.Parameter2:
                    return GetTypeDisplay(this.Signatures[0].Parameters[2]);
                case ParameterTypeKind.SingleColumnTable:
                case ParameterTypeKind.Tabular:
                    return "()";
                case ParameterTypeKind.Cluster:
                    return "cluster";
                case ParameterTypeKind.Database:
                    return "database";
                case ParameterTypeKind.Scalar:
                default:
                    return "scalar";
            }
        }
    }
}