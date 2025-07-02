using System.Collections.Generic;

namespace Kusto.Language.Symbols
{
    using Syntax;
    using Utils;

    /// <summary>
    /// A binding context for a function/operator call.
    /// </summary>
    public abstract class CustomReturnTypeContext
    {
        /// <summary>
        /// The location related to the function/operator call.
        /// </summary>
        public virtual SyntaxNode Location => null;

        /// <summary>
        /// The arguments provided to the function call.
        /// </summary>
        public virtual IReadOnlyList<Expression> Arguments => null;

        /// <summary>
        /// The types of the arguments provided to the function call.
        /// </summary>
        public virtual IReadOnlyList<TypeSymbol> ArgumentTypes => null;

        /// <summary>
        /// The <see cref="Parameter"/> associated with each argument provided to the function call.
        /// </summary>
        public virtual IReadOnlyList<Parameter> ArgumentParameters => null;

        /// <summary>
        /// The input table/schema from the left of a pipe operator.
        /// </summary>
        public virtual TableSymbol RowScope => null;

        /// <summary>
        /// The signature of the function being called.
        /// </summary>
        public virtual Signature Signature => null;

        /// <summary>
        /// The <see cref="GlobalState"/> in use.
        /// </summary>
        public virtual GlobalState Globals => null;

        /// <summary>
        /// The current cluster at the call site.
        /// </summary>
        public virtual ClusterSymbol CurrentCluster => null;

        /// <summary>
        /// The current database as the call site.
        /// </summary>
        public virtual DatabaseSymbol CurrentDatabase => null;

        /// <summary>
        /// The function the call site is within.
        /// </summary>
        public virtual FunctionSymbol CurrentFunction => null;

        /// <summary>
        /// Returns the symbol referenced by the name or null if no such symbol exists in scope.
        /// </summary>
        public abstract Symbol GetReferencedSymbol(string name);

        /// <summary>
        /// Returns the result type of the symbol referenced by the name or null if no such symbol exists in scope.
        /// </summary>
        public abstract TypeSymbol GetResultType(string name);

        /// <summary>
        /// Gets the column name the expression would have in a projection list.
        /// </summary>
        public abstract string GetResultName(Expression expr, string defaultName = "");

        /// <summary>
        /// Gets the first argument associated with the named parameter, or null if no argument is associated with the specified parameter.
        /// </summary>
        public Expression GetArgument(string parameterName)
        {
            var p = this.Signature.GetParameter(parameterName);
            if (p != null)
            {
                return GetArgument(p);
            }

            return null;
        }

        /// <summary>
        /// Gets the first argument associated with the named parameter, or null if no argument is associated with the specified parameter.
        /// </summary>
        public Expression GetArgument(Parameter parameter)
        {
            var argIndex = this.ArgumentParameters.IndexOf(parameter);
            if (argIndex >= 0 && argIndex < this.Arguments.Count)
            {
                return this.Arguments[argIndex];
            }

            return null;
        }

        /// <summary>
        /// Gets the arguments for the specified parameter.
        /// </summary>
        public IReadOnlyList<Expression> GetArguments(string parameterName)
        {
            var parameter = this.Signature.GetParameter(parameterName);
            if (parameter != null)
            {
                return GetArguments(parameter);
            }
            else
            {
                return EmptyReadOnlyList<Expression>.Instance;
            }
        }

        /// <summary>
        /// Gets the arguments for the specified parameter.
        /// </summary>
        public IReadOnlyList<Expression> GetArguments(Parameter parameter)
        {
            List<Expression> arguments = null;

            for (int i = 0; i < this.ArgumentParameters.Count; i++)
            {
                if (this.ArgumentParameters[i] == parameter)
                {
                    if (arguments == null)
                        arguments = new List<Expression>();
                    arguments.Add(this.Arguments[i]);
                }
            }

            return arguments ?? EmptyReadOnlyList<Expression>.Instance;
        }
    }
}