using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Symbols
{
    using Syntax;
    using Utils;

    public abstract class ParameterLayout
    {
        /// <summary>
        /// Gets the parameters that correspond to the set of arguments for the specified signature.
        /// </summary>
        public abstract void GetArgumentParameters(Signature signature, IReadOnlyList<Expression> arguments, List<Parameter> argumentParameters);

        /// <summary>
        /// Gets the parameters corresponding to the set of argument types for the specified signature.
        /// </summary>
        public virtual void GetArgumentParameters(Signature signature, IReadOnlyList<TypeSymbol> argumentTypes, List<Parameter> argumentParameters)
        {
            // default implementation makes a fake set of arguments corresponding to the specified types
            var arguments = s_expressionListPool.AllocateFromPool();
            try
            {
                GetFakeExpressions(argumentTypes, arguments);
                GetArgumentParameters(signature, arguments, argumentParameters);
            }
            finally
            {
                s_expressionListPool.ReturnToPool(arguments);
            }
        }

        /// <summary>
        /// Gets the set of possible parameters for an argument that would follow after the specified set of existing arguments.
        /// </summary>
        public virtual void GetNextPossibleParameters(Signature signature, IReadOnlyList<Expression> arguments, List<Parameter> possibleParameters)
        {
            // default implementation asks for the layout given an additional parameter with type Unknown
            var newArguments = s_expressionListPool.AllocateFromPool();
            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                // add fake argument w/ unknown type to end of arguments
                newArguments.AddRange(arguments);
                newArguments.Add(FakeExpression.Create(ScalarTypes.Unknown));

                // try to get parameter layout for this extended list of arguments
                GetArgumentParameters(signature, newArguments, argumentParameters);

                // use last parameter in this layout
                possibleParameters.Add(argumentParameters[argumentParameters.Count - 1]);
            }
            finally
            {
                s_expressionListPool.ReturnToPool(newArguments);
                s_parameterListPool.ReturnToPool(argumentParameters);
            }
        }

        public virtual bool IsValidArgumentCount(Signature signature, int argumentCount)
        {
            return argumentCount >= signature.MinArgumentCount && argumentCount <= signature.MaxArgumentCount;
        }

        private static readonly ObjectPool<List<Parameter>> s_parameterListPool =
            new ObjectPool<List<Parameter>>(() => new List<Parameter>(), list => list.Clear());

        private static readonly ObjectPool<List<Expression>> s_expressionListPool =
            new ObjectPool<List<Expression>>(() => new List<Expression>(), list => list.Clear());

        private static void GetFakeExpressions(IReadOnlyList<TypeSymbol> types, List<Expression> expressions)
        {
            foreach (var type in types)
            {
                expressions.Add(FakeExpression.Create(type));
            }
        }
    }
}