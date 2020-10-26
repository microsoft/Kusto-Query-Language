using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Symbols
{
    using Syntax;
    using Utils;

    /// <summary>
    /// A function that builds a list of parameters associated with each argument.
    /// </summary>
    public delegate void ParameterLayoutBuilder(Signature signature, IReadOnlyList<Expression> arguments, List<Parameter> argumentParameters);

    /// <summary>
    /// The known set of <see cref="ParameterLayout"/>'s
    /// </summary>
    public static class ParameterLayouts
    {
        /// <summary>
        ///  Does not allow parameters to repeat.
        ///  Optional parameters (minOccurring=0) cannot be skipped.
        ///  Named arguments allowed.
        /// </summary>
        public static readonly ParameterLayout Fixed = new NonRepeatingParameterLayout();

        /// <summary>
        /// Allows for individual repeating parameters, transitioning based on type matching.
        /// Optional parameters (minOccurring=0) cannot be skipped.
        /// Named argument not allowed.
        /// </summary>
        public static readonly ParameterLayout Repeating = new RepeatingParameterLayout(false);

        /// <summary>
        /// Allows for individual repeating parameters, transitioning based on type matching.
        /// Optional parameters (minOccurring=0) may be skipped.
        /// Named arguments not allowed.
        /// </summary>
        public static readonly ParameterLayout RepeatingSkipping = new RepeatingParameterLayout(true);

        /// <summary>
        /// Allows for a single group of parameters to repeat together.
        /// Optional parameters (minOccurring=0) cannot be skipped and cannot be part of the repeating block.
        /// Named arguments not allowed.
        /// </summary>
        public static readonly ParameterLayout BlockRepeating = new BlockRepeatingParameterLayout();

        /// <summary>
        /// A custom layout supplied by a builder function.
        /// </summary>
        public static ParameterLayout Custom(ParameterLayoutBuilder builder) => new CustomParameterLayout(builder);
    }

    internal class NonRepeatingParameterLayout : ParameterLayout
    {
        public override void GetArgumentParameters(Signature signature, IReadOnlyList<Expression> arguments, List<Parameter> argumentParameters)
        {
            LayoutParameters(signature, arguments.Count, arguments, argumentParameters);
        }

        public override void GetArgumentParameters(Signature signature, IReadOnlyList<TypeSymbol> argumentTypes, List<Parameter> argumentParameters)
        {
            LayoutParameters(signature, argumentTypes.Count, null, argumentParameters);
        }

        internal static void LayoutParameters(Signature signature, int nArguments, IReadOnlyList<Expression> arguments, List<Parameter> argumentParameters)
        {
            for (int i = 0; i < nArguments; i++)
            {
                if (signature.AllowsNamedArguments && arguments != null && arguments[i] is SimpleNamedExpression sn)
                {
                    argumentParameters.Add(signature.GetParameter(sn.Name.SimpleName));
                }
                else if (i < signature.Parameters.Count)
                {
                    argumentParameters.Add(signature.Parameters[i]);
                }
                else
                {
                    argumentParameters.Add(Signature.UnknownParameter);
                }
            }
        }

        public override void GetNextPossibleParameters(Signature signature, IReadOnlyList<Expression> existingArguments, List<Parameter> possibleParameters)
        {
            var iParameter = existingArguments.Count;
            if (iParameter < signature.Parameters.Count)
            {
                possibleParameters.Add(signature.Parameters[iParameter]);
            }
        }
    }

    internal class RepeatingParameterLayout : ParameterLayout
    {
        private readonly bool _allowSkippingOptionalParameters;

        public RepeatingParameterLayout(bool allowSkippingOptionalParameters)
        {
            _allowSkippingOptionalParameters = allowSkippingOptionalParameters;
        }

        public override void GetArgumentParameters(Signature signature, IReadOnlyList<Expression> arguments, List<Parameter> argumentParameters)
        {
            var argTypes = s_typeListPool.AllocateFromPool();
            try
            {
                GetArgumentTypes(arguments, argTypes);

                int iCurrentParameter = 0;
                int iCurrentParameterCount = 0;
                GetArgumentParameters(signature, arguments, argTypes, argumentParameters, ref iCurrentParameter, ref iCurrentParameterCount);
            }
            finally
            {
                s_typeListPool.ReturnToPool(argTypes);
            }
        }

        public override void GetArgumentParameters(Signature signature, IReadOnlyList<TypeSymbol> argumentTypes, List<Parameter> argumentParameters)
        {
            int iCurrentParameter = 0;
            int iCurrentParameterCount = 0;
            GetArgumentParameters(signature, null, argumentTypes, argumentParameters, ref iCurrentParameter, ref iCurrentParameterCount);
        }

        private void GetArgumentParameters(Signature signature, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes, List<Parameter> argumentParameters,
            ref int iCurrentParameter, ref int iCurrentParameterCount)
        {
            for (int iArg = 0; iArg < argumentTypes.Count; iArg++)
            {
                if (iArg >= signature.MaxArgumentCount)
                {
                    argumentParameters.Add(Signature.UnknownParameter);
                    continue;
                }

                var currentParam = signature.Parameters[iCurrentParameter];

                // if we already have the max number of current parameters, use next parameter
                if (iCurrentParameter < signature.Parameters.Count - 1
                    && iCurrentParameterCount >= currentParam.MaxOccurring)
                {
                    iCurrentParameter++;
                    iCurrentParameterCount = 0;
                    currentParam = signature.Parameters[iCurrentParameter];
                }

                if (iCurrentParameterCount < currentParam.MinOccurring
                    || (iCurrentParameterCount == 0 && currentParam.MinOccurring == 0 && !_allowSkippingOptionalParameters))
                {
                    // based on min occurring, current parameter is still required
                    argumentParameters.Add(currentParam);
                    iCurrentParameterCount++;
                }
                else if (iCurrentParameter == signature.Parameters.Count - 1)
                {
                    if (iCurrentParameterCount < currentParam.MaxOccurring)
                    {
                        argumentParameters.Add(currentParam);
                        iCurrentParameterCount++;
                    }
                    else
                    {
                        argumentParameters.Add(Signature.UnknownParameter);
                    }
                }
                else
                {
                    // otherwise compare to next parameter to see which is better
                    var arg = arguments != null ? arguments[iArg] : null;
                    var argType = argumentTypes[iArg];
                    var currentMatch = Binding.Binder.GetParameterMatchKind(
                        signature, argumentParameters, argumentTypes, currentParam, arg, argType, allowLooseParameterMatching: false);

                    var iNextParameter = iCurrentParameter + 1;
                    while (iNextParameter < signature.Parameters.Count)
                    {
                        var nextParam = signature.Parameters[iNextParameter];
                        var nextMatch = Binding.Binder.GetParameterMatchKind(
                            signature, argumentParameters, argumentTypes, nextParam, arg, argType, allowLooseParameterMatching: false);

                        if (currentMatch >= nextMatch && currentMatch != Binding.ParameterMatchKind.None)
                        {
                            // current is better
                            argumentParameters.Add(currentParam);
                            iCurrentParameterCount++;
                            break;
                        }
                        else if (nextMatch != Binding.ParameterMatchKind.None)
                        {
                            // next is better
                            argumentParameters.Add(nextParam);
                            iCurrentParameter = iNextParameter;
                            iCurrentParameterCount = 1;
                            break;
                        }
                        else if (nextParam.MinOccurring == 0
                            && _allowSkippingOptionalParameters
                            && iNextParameter < signature.Parameters.Count - 1)
                        {
                            // next parameter and its optional, so try one after that
                            iNextParameter++;
                            continue;
                        }
                        else
                        {
                            // no parameter is a match.. use the current one
                            argumentParameters.Add(currentParam);
                            iCurrentParameterCount++;
                            break;
                        }
                    }
                }
            }
        }

        private static void GetArgumentTypes(IReadOnlyList<Expression> arguments, List<TypeSymbol> types)
        {
            foreach (var arg in arguments)
            {
                types.Add(arg.ResultType);
            }
        }

        private static readonly ObjectPool<List<TypeSymbol>> s_typeListPool =
            new ObjectPool<List<TypeSymbol>>(() => new List<TypeSymbol>(), list => list.Clear());

        private static readonly ObjectPool<List<Parameter>> s_parameterListPool =
            new ObjectPool<List<Parameter>>(() => new List<Parameter>(), list => list.Clear());

        public override void GetNextPossibleParameters(Signature signature, IReadOnlyList<Expression> arguments, List<Parameter> possibleParameters)
        {
            var existingArgumentParameters = s_parameterListPool.AllocateFromPool();
            var argumentTypes = s_typeListPool.AllocateFromPool();
            try
            {
                GetArgumentTypes(arguments, argumentTypes);

                int iCurrentParameter = 0;
                int iCurrentParameterCount = 0;
                GetArgumentParameters(signature, arguments, argumentTypes, existingArgumentParameters, ref iCurrentParameter, ref iCurrentParameterCount);

                if (arguments.Count >= signature.MaxArgumentCount)
                {
                    // beyond all possible args, nothing to suggest!
                    return;
                }

                var currentParam = signature.Parameters[iCurrentParameter];

                // if we already have the max number of current parameters, use next parameter
                if (iCurrentParameter < signature.Parameters.Count - 1
                    && iCurrentParameterCount >= currentParam.MaxOccurring)
                {
                    iCurrentParameter++;
                    iCurrentParameterCount = 0;
                    currentParam = signature.Parameters[iCurrentParameter];
                }

                if (iCurrentParameterCount < currentParam.MinOccurring)
                {
                    // based on min occurring, current parameter is still required
                    possibleParameters.Add(currentParam);
                    iCurrentParameterCount++;
                }
                else if (iCurrentParameter == signature.Parameters.Count - 1)
                {
                    // last possible parameter, so this must be it (if we've not already used it up)
                    if (iCurrentParameterCount < currentParam.MaxOccurring)
                    {
                        possibleParameters.Add(currentParam);
                        iCurrentParameterCount++;
                    }
                }
                else
                {
                    possibleParameters.Add(currentParam);

                    if (iCurrentParameterCount > 0 || _allowSkippingOptionalParameters)
                    {
                        // add all parameters that might occur next
                        var iNextParameter = iCurrentParameter + 1;
                        while (iNextParameter < signature.Parameters.Count)
                        {
                            var nextParam = signature.Parameters[iNextParameter];
                            possibleParameters.Add(nextParam);

                            if (nextParam.MinOccurring == 0
                                && _allowSkippingOptionalParameters
                                && iNextParameter < signature.Parameters.Count - 1)
                            {
                                // next parameter and its optional, so try one after that
                                iNextParameter++;
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            finally
            {
                s_typeListPool.ReturnToPool(argumentTypes);
                s_parameterListPool.ReturnToPool(existingArgumentParameters);
            }
        }
    }

    internal class BlockRepeatingParameterLayout : ParameterLayout
    {
        public override void GetArgumentParameters(Signature signature, IReadOnlyList<Expression> arguments, List<Parameter> argumentParameters)
        {
            if (signature.HasRepeatableParameters)
            {
                GetArgumentParameters(signature, arguments.Count, argumentParameters);
            }
            else
            {
                NonRepeatingParameterLayout.LayoutParameters(signature, arguments.Count, arguments, argumentParameters);
            }
        }

        public override void GetArgumentParameters(Signature signature, IReadOnlyList<TypeSymbol> argumentTypes, List<Parameter> argumentParameters)
        {
            if (signature.HasRepeatableParameters)
            {
                GetArgumentParameters(signature, argumentTypes.Count, argumentParameters);
            }
            else
            {
                NonRepeatingParameterLayout.LayoutParameters(signature, argumentTypes.Count, null, argumentParameters);
            }
        }

        private void GetArgumentParameters(Signature signature, int nArguments, List<Parameter> argumentParameters)
        {
            var firstRepeatableParameter = signature.Parameters.FirstIndex(p => p.IsRepeatable);
            var lastRepeatableParameter = signature.Parameters.LastIndex(p => p.IsRepeatable);

            var firstRepeatingArgument = firstRepeatableParameter;
            var numberOfRepeatingParameters = (lastRepeatableParameter - firstRepeatableParameter + 1);

            var minRepeats = signature.Parameters[firstRepeatableParameter].MinOccurring;
            var maxRepeats = signature.Parameters[firstRepeatableParameter].MaxOccurring;

            var minRepeatingArguments = numberOfRepeatingParameters * minRepeats;
            var maxRepeatingArguments = numberOfRepeatingParameters * maxRepeats;

            var parametersAfterLastRepeatingParameter = signature.Parameters.Count - lastRepeatableParameter - 1;
            var possibleRepeatingArguments = (nArguments - firstRepeatingArgument) - parametersAfterLastRepeatingParameter;
            var expectedRepeatingArguments = Math.Min(maxRepeatingArguments, Math.Max(minRepeatingArguments, possibleRepeatingArguments));

            var repeatingArgumentGroups = (expectedRepeatingArguments + numberOfRepeatingParameters - 1) / numberOfRepeatingParameters;
            var totalRepeatingArguments = repeatingArgumentGroups * numberOfRepeatingParameters;
            var lastRepeatingArgument = firstRepeatableParameter + totalRepeatingArguments - 1;

            for (int i = 0; i < nArguments; i++)
            {
                if (i < firstRepeatingArgument)
                {
                    argumentParameters.Add(signature.Parameters[i]);
                }
                else if (i >= firstRepeatingArgument && i <= lastRepeatingArgument)
                {
                    argumentParameters.Add(signature.Parameters[firstRepeatableParameter + ((i - firstRepeatingArgument) % numberOfRepeatingParameters)]);
                }
                else
                {
                    var index = lastRepeatableParameter + (i - lastRepeatingArgument);
                    if (index < signature.Parameters.Count)
                    {
                        argumentParameters.Add(signature.Parameters[index]);
                    }
                    else
                    {
                        argumentParameters.Add(Signature.UnknownParameter);
                    }
                }
            }
        }

        public override void GetNextPossibleParameters(Signature signature, IReadOnlyList<Expression> arguments, List<Parameter> possibleParameters)
        {
            var argumentIndex = arguments.Count;

            if (signature.HasRepeatableParameters)
            {
                var firstRepeatableParameter = signature.Parameters.FirstIndex(p => p.IsRepeatable);
                var lastRepeatableParameter = signature.Parameters.LastIndex(p => p.IsRepeatable);

                if (argumentIndex < firstRepeatableParameter)
                {
                    // argument occurs within the fixed parameters before the start of the repeating block
                    possibleParameters.Add(signature.Parameters[argumentIndex]);
                }
                else
                {
                    var nRepeatable = lastRepeatableParameter - firstRepeatableParameter + 1;
                    var minOccurring = signature.Parameters[firstRepeatableParameter].MinOccurring;
                    var maxOccurring = signature.Parameters[firstRepeatableParameter].MaxOccurring;

                    var repeatGroup = (argumentIndex - firstRepeatableParameter) / nRepeatable;
                    var repeatOffset = ((argumentIndex - firstRepeatableParameter) % nRepeatable);
                    if (repeatGroup < maxOccurring)
                    {
                        // if less than maxOccurring groups have repeated, then this argument position
                        // may either belong to one of the repeating parameters or one of the fixed parameters that follow
                        var iRepeatableParam = repeatOffset + firstRepeatableParameter;
                        possibleParameters.Add(signature.Parameters[iRepeatableParam]);

                        // only show possible following fixed paramter if we've satisfied at least the minimum repeats
                        if (repeatGroup >= minOccurring)
                        {
                            var iEndParam = lastRepeatableParameter + repeatOffset + 1;
                            if (iEndParam < signature.Parameters.Count)
                            {
                                possibleParameters.Add(signature.Parameters[iEndParam]);
                            }
                        }
                    }
                    else
                    {
                        // maximum repeat blocks have occurred, only fixed parameters after block are possible
                        var lastRepeatingArgument = firstRepeatableParameter + nRepeatable * maxOccurring;
                        var iEndParam = (argumentIndex - lastRepeatingArgument) + lastRepeatableParameter + 1;
                        if (iEndParam < signature.Parameters.Count)
                        {
                            possibleParameters.Add(signature.Parameters[iEndParam]);
                        }
                    }
                }
            }
            else if (argumentIndex < signature.Parameters.Count)
            {
                // no repeatable parameters (so just use next parameter only)
                possibleParameters.Add(signature.Parameters[argumentIndex]);
            }
        }

        public override bool IsValidArgumentCount(Signature signature, int argumentCount)
        {
            var isValid = base.IsValidArgumentCount(signature, argumentCount);

            if (isValid && signature.HasRepeatableParameters)
            {
                var firstRepeatableParameter = signature.Parameters.FirstIndex(p => p.IsRepeatable);
                var lastRepeatableParameter = signature.Parameters.LastIndex(p => p.IsRepeatable);

                var nVariable = (lastRepeatableParameter - firstRepeatableParameter + 1);
                var nBefore = firstRepeatableParameter;
                var nAfter = signature.Parameters.Count - (nBefore + nVariable);
                var nFixed = nBefore + nAfter;

                // argument count must include an even multiple of the repeating parameters
                isValid = (argumentCount - nFixed) % nVariable == 0;
            }

            return isValid;
        }
    }

    internal class CustomParameterLayout : ParameterLayout
    {
        private readonly ParameterLayoutBuilder _builder;

        public CustomParameterLayout(ParameterLayoutBuilder builder)
        {
            _builder = builder;
        }

        public override void GetArgumentParameters(Signature signature, IReadOnlyList<Expression> arguments, List<Parameter> argumentParameters)
        {
            _builder(signature, arguments, argumentParameters);

            // fill out any parameters left unspecified
            while (argumentParameters.Count < arguments.Count)
            {
                argumentParameters.Add(Signature.UnknownParameter);
            }
        }
    }
}