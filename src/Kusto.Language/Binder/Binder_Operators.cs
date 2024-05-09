using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Binding
{
    using Parsing;
    using Symbols;
    using Syntax;
    using Utils;

    internal sealed partial class Binder
    {
        private SemanticInfo GetBinaryOperatorInfo(OperatorKind kind, Expression left, Expression right, SyntaxElement location)
        {
            return GetBinaryOperatorInfo(kind, left, GetResultTypeOrError(left), right, GetResultTypeOrError(right), location);
        }

        private SemanticInfo GetBinaryOperatorInfo(OperatorKind kind, Expression left, TypeSymbol leftType, Expression right, TypeSymbol rightType, SyntaxElement location)
        {
            var arguments = s_expressionListPool.AllocateFromPool();
            var argumentTypes = s_typeListPool.AllocateFromPool();

            try
            {
                arguments.Add(left);
                arguments.Add(right);

                argumentTypes.Add(leftType);
                argumentTypes.Add(rightType);

                return GetOperatorInfo(kind, arguments, argumentTypes, location, requireAllArgumentsMatch: true);
            }
            finally
            {
                s_expressionListPool.ReturnToPool(arguments);
                s_typeListPool.ReturnToPool(argumentTypes);
            }
        }

        private SemanticInfo GetUnaryOperatorInfo(OperatorKind kind, Expression operand, SyntaxElement location)
        {
            var arguments = s_expressionListPool.AllocateFromPool();

            try
            {
                arguments.Add(operand);

                return GetOperatorInfo(kind, arguments, location, requireAllArgumentsMatch: true);
            }
            finally
            {
                s_expressionListPool.ReturnToPool(arguments);
            }
        }

        private SemanticInfo GetOperatorInfo(
            OperatorKind kind, 
            IReadOnlyList<Expression> arguments, 
            SyntaxElement location,
            bool requireAllArgumentsMatch = false)
        {
            var argumentTypes = s_typeListPool.AllocateFromPool();

            try
            {
                for (int i = 0; i < arguments.Count; i++)
                {
                    argumentTypes.Add(GetResultTypeOrError(arguments[i]));
                }

                return GetOperatorInfo(kind, arguments, argumentTypes, location, requireAllArgumentsMatch);
            }
            finally
            {
                s_typeListPool.ReturnToPool(argumentTypes);
            }
        }

        private SemanticInfo GetOperatorInfo(
            OperatorKind kind, 
            IReadOnlyList<Expression> arguments, 
            IReadOnlyList<TypeSymbol> argumentTypes,
            SyntaxElement location,
            bool requireAllArgumentsMatch)
        {
            var matchingSignatures = s_signatureListPool.AllocateFromPool();
            var diagnostics = s_diagnosticListPool.AllocateFromPool();

            try
            {
                var op = _globals.GetOperator(kind);

                GetBestMatchingSignatures(op.Signatures, arguments, argumentTypes, matchingSignatures, requireAllArgumentsMatch);

                if (matchingSignatures.Count == 1)
                {
                    CheckSignature(matchingSignatures[0], arguments, argumentTypes, location, diagnostics);
                    var funResult = GetFunctionCallResult(matchingSignatures[0], arguments, argumentTypes, location);
                    var resultType = funResult.Type;

                    // check for possible better dynamic result
                    if (funResult.Type == ScalarTypes.Dynamic
                        && HasDynamicPrimitives(argumentTypes))
                    {
                        var unwrappedArgumentTypes = s_typeListPool.AllocateFromPool();
                        try
                        {
                            GetUnwrappedDynamicPrimitives(argumentTypes, unwrappedArgumentTypes);
                            var unwrappedResultType = GetOperatorInfo(kind, arguments, unwrappedArgumentTypes, location, requireAllArgumentsMatch).ResultType;
                            if (unwrappedResultType is ScalarSymbol
                                && !(unwrappedResultType is DynamicSymbol)
                                && unwrappedResultType != ScalarTypes.Unknown)
                            {
                                resultType = ScalarTypes.GetDynamic(unwrappedResultType);
                            }
                        }
                        finally
                        {
                            s_typeListPool.ReturnToPool(unwrappedArgumentTypes);
                        }
                    }

                    return new SemanticInfo(matchingSignatures[0], resultType, diagnostics, isConstant: AllAreConstant(arguments));
                }
                else
                {
                    if (matchingSignatures.Count == 0 && requireAllArgumentsMatch)
                    {
                        // try again to get better return type
                        GetBestMatchingSignatures(op.Signatures, arguments, argumentTypes, matchingSignatures, requireAllArgumentsMatch: false);
                    }

                    if (!ArgumentsHaveErrorsOrUnknown(argumentTypes))
                    {
                        diagnostics.Add(DiagnosticFacts.GetOperatorNotDefined(location.ToString(IncludeTrivia.Interior), argumentTypes).WithLocation(location));
                    }

                    var returnType = GetCommonReturnType(matchingSignatures, arguments, argumentTypes, location);
                    return new SemanticInfo(op, returnType, diagnostics);
                }
            }
            finally
            {
                s_signatureListPool.ReturnToPool(matchingSignatures);
                s_diagnosticListPool.ReturnToPool(diagnostics);
            }
        }


        private static bool AllAreConstant(IReadOnlyList<Expression> expressions)
        {
            for (int i = 0; i < expressions.Count; i++)
            {
                if (!GetIsConstant(expressions[i]))
                    return false;
            }

            return true;
        }

        private static OperatorKind GetOperatorKind(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.AddExpression:
                    return OperatorKind.Add;
                case SyntaxKind.SubtractExpression:
                    return OperatorKind.Subtract;
                case SyntaxKind.MultiplyExpression:
                    return OperatorKind.Multiply;
                case SyntaxKind.DivideExpression:
                    return OperatorKind.Divide;
                case SyntaxKind.ModuloExpression:
                    return OperatorKind.Modulo;
                case SyntaxKind.UnaryMinusExpression:
                    return OperatorKind.UnaryMinus;
                case SyntaxKind.UnaryPlusExpression:
                    return OperatorKind.UnaryPlus;
                case SyntaxKind.EqualExpression:
                    return OperatorKind.Equal;
                case SyntaxKind.NotEqualExpression:
                    return OperatorKind.NotEqual;
                case SyntaxKind.LessThanExpression:
                    return OperatorKind.LessThan;
                case SyntaxKind.LessThanOrEqualExpression:
                    return OperatorKind.LessThanOrEqual;
                case SyntaxKind.GreaterThanExpression:
                    return OperatorKind.GreaterThan;
                case SyntaxKind.GreaterThanOrEqualExpression:
                    return OperatorKind.GreaterThanOrEqual;
                case SyntaxKind.EqualTildeExpression:
                    return OperatorKind.EqualTilde;
                case SyntaxKind.BangTildeExpression:
                    return OperatorKind.BangTilde;
                case SyntaxKind.HasExpression:
                    return OperatorKind.Has;
                case SyntaxKind.HasCsExpression:
                    return OperatorKind.HasCs;
                case SyntaxKind.NotHasExpression:
                    return OperatorKind.NotHas;
                case SyntaxKind.NotHasCsExpression:
                    return OperatorKind.NotHasCs;
                case SyntaxKind.HasPrefixExpression:
                    return OperatorKind.HasPrefix;
                case SyntaxKind.HasPrefixCsExpression:
                    return OperatorKind.HasPrefixCs;
                case SyntaxKind.NotHasPrefixExpression:
                    return OperatorKind.NotHasPrefix;
                case SyntaxKind.NotHasPrefixCsExpression:
                    return OperatorKind.NotHasPrefixCs;
                case SyntaxKind.HasSuffixExpression:
                    return OperatorKind.HasSuffix;
                case SyntaxKind.HasSuffixCsExpression:
                    return OperatorKind.HasSuffixCs;
                case SyntaxKind.NotHasSuffixExpression:
                    return OperatorKind.NotHasSuffix;
                case SyntaxKind.NotHasSuffixCsExpression:
                    return OperatorKind.NotHasSuffixCs;
                case SyntaxKind.LikeExpression:
                    return OperatorKind.Like;
                case SyntaxKind.LikeCsExpression:
                    return OperatorKind.LikeCs;
                case SyntaxKind.NotLikeExpression:
                    return OperatorKind.NotLike;
                case SyntaxKind.NotLikeCsExpression:
                    return OperatorKind.NotLikeCs;
                case SyntaxKind.ContainsExpression:
                    return OperatorKind.Contains;
                case SyntaxKind.ContainsCsExpression:
                    return OperatorKind.ContainsCs;
                case SyntaxKind.NotContainsExpression:
                    return OperatorKind.NotContains;
                case SyntaxKind.NotContainsCsExpression:
                    return OperatorKind.NotContainsCs;
                case SyntaxKind.StartsWithExpression:
                    return OperatorKind.StartsWith;
                case SyntaxKind.StartsWithCsExpression:
                    return OperatorKind.StartsWithCs;
                case SyntaxKind.NotStartsWithExpression:
                    return OperatorKind.NotStartsWith;
                case SyntaxKind.NotStartsWithCsExpression:
                    return OperatorKind.NotStartsWithCs;
                case SyntaxKind.EndsWithExpression:
                    return OperatorKind.EndsWith;
                case SyntaxKind.EndsWithCsExpression:
                    return OperatorKind.EndsWithCs;
                case SyntaxKind.NotEndsWithExpression:
                    return OperatorKind.NotEndsWith;
                case SyntaxKind.NotEndsWithCsExpression:
                    return OperatorKind.NotEndsWithCs;
                case SyntaxKind.MatchesRegexExpression:
                    return OperatorKind.MatchRegex;
                case SyntaxKind.InExpression:
                    return OperatorKind.In;
                case SyntaxKind.InCsExpression:
                    return OperatorKind.InCs;
                case SyntaxKind.NotInExpression:
                    return OperatorKind.NotIn;
                case SyntaxKind.NotInCsExpression:
                    return OperatorKind.NotInCs;
                case SyntaxKind.BetweenExpression:
                    return OperatorKind.Between;
                case SyntaxKind.NotBetweenExpression:
                    return OperatorKind.NotBetween;
                case SyntaxKind.AndExpression:
                    return OperatorKind.And;
                case SyntaxKind.OrExpression:
                    return OperatorKind.Or;
                case SyntaxKind.SearchExpression:
                    return OperatorKind.Search;
                case SyntaxKind.HasAnyExpression:
                    return OperatorKind.HasAny;
                case SyntaxKind.HasAllExpression:
                    return OperatorKind.HasAll;
                default:
                    return OperatorKind.None;
            }
        }

        /// <summary>
        /// Returns the result type for the binary operator given the two argument types.
        /// </summary>
        private TypeSymbol GetBinaryOperatorResultType(OperatorKind kind, TypeSymbol leftType, TypeSymbol rightType, SyntaxElement location, List<Diagnostic> diagnostics = null)
        {
            var fakeLeftArg = FakeExpression.Create(leftType);
            var fakeRightArg = FakeExpression.Create(rightType);
            return GetBinaryOperatorResultType(kind, fakeLeftArg, fakeRightArg, location ?? fakeLeftArg, diagnostics);
        }

        /// <summary>
        /// Returns the result type for the binary operator given the two argument expressions.
        /// </summary>
        private TypeSymbol GetBinaryOperatorResultType(OperatorKind kind, Expression left, Expression right, SyntaxElement location, List<Diagnostic> diagnostics = null)
        {
            var info = GetBinaryOperatorInfo(kind, left, GetResultTypeOrError(left), right, GetResultTypeOrError(right), location ?? left);
            if (diagnostics != null)
                diagnostics.AddRange(info.Diagnostics);
            return info.ResultType;
        }
    }
}