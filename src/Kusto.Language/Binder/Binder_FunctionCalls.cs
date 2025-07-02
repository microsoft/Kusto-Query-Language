using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Binding
{
    using Kusto.Language;
    using Parsing;
    using Symbols;
    using Syntax;
    using Utils;
    using static Symbols.TypeFacts;

    internal sealed partial class Binder
    {
        /// <summary>
        /// Binds a function call or pattern invocation expression
        /// </summary>
        private SemanticInfo BindFunctionCallOrPattern(FunctionCallExpression functionCall)
        {
            // the result type of the name should be bound to the function/pattern
            var symbol = GetResultTypeOrError(functionCall.Name);

            if (symbol is FunctionSymbol fn)
            {
                return BindFunctionCall(functionCall, fn);
            }
            else if (symbol is PatternSymbol ps)
            {
                return BindPattern(functionCall, ps);
            }
            else if (!symbol.IsError)
            {
                // the name was not a known function or pattern, but we decided to give it a result type, so let's use it
                return functionCall.Name.GetSemanticInfo();
            }
            else if (_isFuzzy)
            {
                return new SemanticInfo(
                    new TableSymbol().WithIsOpen(true),
                    DiagnosticFacts.GetFuzzyFunctionNotDefined(functionCall.Name.SimpleName).WithLocation(functionCall));
            }
            else
            {
                return ErrorInfo;
            }
        }

        private static bool IsInvokeOperatorFunctionCall(FunctionCallExpression functionCall)
        {
            return functionCall.Parent is InvokeOperator
                || (functionCall.Parent is PathExpression p && p.Selector == functionCall && p.Parent is InvokeOperator);
        }

        /// <summary>
        /// Binds a function call
        /// </summary>
        private SemanticInfo BindFunctionCall(
            FunctionCallExpression functionCall, FunctionSymbol fn)
        {
            var arguments = s_expressionListPool.AllocateFromPool();
            var argumentTypes = s_typeListPool.AllocateFromPool();

            try
            {
                GetArgumentsAndTypes(functionCall, arguments, argumentTypes);
                return BindFunctionCall(functionCall, fn, arguments, argumentTypes);
            }
            finally
            {
                s_expressionListPool.ReturnToPool(arguments);
                s_typeListPool.ReturnToPool(argumentTypes);
            }
        }

        /// <summary>
        /// Binds a function call
        /// </summary>
        private SemanticInfo BindFunctionCall(
            FunctionCallExpression functionCall, 
            FunctionSymbol fn, 
            IReadOnlyList<Expression> arguments,
            IReadOnlyList<TypeSymbol> argumentTypes)
        {
            var diagnostics = s_diagnosticListPool.AllocateFromPool();
            var matchingSignatures = s_signatureListPool.AllocateFromPool();

            try
            {
                GetBestMatchingSignatures(fn.Signatures, arguments, argumentTypes, matchingSignatures);

                if (matchingSignatures.Count == 1)
                {
                    CheckSignature(matchingSignatures[0], arguments, argumentTypes, functionCall.Name, diagnostics);
                    var funResult = GetFunctionCallResult(matchingSignatures[0], arguments, argumentTypes, functionCall.Name, diagnostics);
                    var resultType = funResult.Type;

                    // check for possible better dynamic result
                    if (funResult.Type == ScalarTypes.Dynamic
                        && HasDynamicPrimitives(argumentTypes))
                    {
                        var unwrappedArgumentTypes = s_typeListPool.AllocateFromPool();
                        try
                        {
                            GetUnwrappedDynamicPrimitives(argumentTypes, unwrappedArgumentTypes);
                            var unwrappedResultType = BindFunctionCall(functionCall, fn, arguments, unwrappedArgumentTypes).ResultType;
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

                    return new SemanticInfo(
                        matchingSignatures[0],
                        resultType,
                        diagnostics,
                        isConstant: fn.IsConstantFoldable && AllAreConstant(arguments),
                        calledFunctionInfo: funResult.Info);
                }
                else
                {
                    if (arguments.Count == 0 && fn.MinArgumentCount > 0)
                    {
                        diagnostics.Add(DiagnosticFacts.GetFunctionExpectsArgumentCountRange(fn.Name, fn.MinArgumentCount, fn.MaxArgumentCount).WithLocation(functionCall.Name));
                    }
                    else if (!ArgumentsHaveErrorsOrUnknown(argumentTypes))
                    {
                        var types = arguments.Select(e => GetResultTypeOrError(e)).ToList();
                        diagnostics.Add(DiagnosticFacts.GetFunctionNotDefinedWithMatchingParameters(functionCall.Name.SimpleName, types).WithLocation(functionCall.Name));
                    }

                    var returnType = GetCommonReturnType(matchingSignatures, arguments, argumentTypes, functionCall.Name);

                    return new SemanticInfo(fn, returnType, diagnostics, isConstant: fn.IsConstantFoldable && AllAreConstant(arguments));
                }
            }
            finally
            {
                s_diagnosticListPool.ReturnToPool(diagnostics);
                s_signatureListPool.ReturnToPool(matchingSignatures);
            }
        }

        /// <summary>
        /// Binds a pattern
        /// </summary>
        private SemanticInfo BindPattern(FunctionCallExpression functionCall, PatternSymbol pattern)
        {
            var diagnostics = s_diagnosticListPool.AllocateFromPool();
            var matchingSignatures = s_patternListPool.AllocateFromPool();
            var arguments = s_expressionListPool.AllocateFromPool();
            try
            {
                // check argument count
                if (pattern.Parameters.Count != functionCall.ArgumentList.Expressions.Count)
                {
                    diagnostics.Add(DiagnosticFacts.GetArgumentCountExpected(pattern.Parameters.Count).WithLocation(functionCall.Name));
                }

                // check actual arguments
                for (int i = 0, n = functionCall.ArgumentList.Expressions.Count; i < n; i++)
                {
                    var argument = functionCall.ArgumentList.Expressions[i].Element;
                    arguments.Add(argument);

                    if (i < pattern.Parameters.Count)
                    {
                        var type = pattern.Parameters[i].DeclaredTypes[0];
                        if (CheckIsExactType(argument, type, diagnostics))
                        {
                            CheckIsLiteral(argument, diagnostics);
                        }
                    }
                }

                GetMatchingPatterns(pattern.Signatures, arguments, matchingSignatures);

                if (matchingSignatures.Count == 0)
                {
                    diagnostics.Add(DiagnosticFacts.GetNoPatternMatchesArguments().WithLocation(functionCall.Name));
                    return new SemanticInfo(pattern, ErrorSymbol.Instance, diagnostics);
                }
                else
                {
                    var result = GetPatternReturnType(matchingSignatures);
                    return new SemanticInfo(pattern, result, diagnostics);
                }
            }
            finally
            {
                s_diagnosticListPool.ReturnToPool(diagnostics);
                s_patternListPool.ReturnToPool(matchingSignatures);
                s_expressionListPool.ReturnToPool(arguments);
            }
        }

        /// <summary>
        /// Gets the set of pattern signatures that match the arguments.
        /// </summary>
        private void GetMatchingPatterns(IReadOnlyList<PatternSignature> signatures, IReadOnlyList<Expression> arguments, List<PatternSignature> matchingSignatures)
        {
            // look for exact match
            foreach (var sig in signatures)
            {
                if (PatternMatches(sig, arguments, exact: true))
                {
                    matchingSignatures.Add(sig);
                }
            }

            // if no exact matches, look for partial matches
            if (matchingSignatures.Count == 0)
            {
                foreach (var sig in signatures)
                {
                    if (PatternMatches(sig, arguments, exact: false))
                    {
                        matchingSignatures.Add(sig);
                    }
                }
            }
        }

        /// <summary>
        /// Determines if the pattern signature matches the arguments.
        /// </summary>
        private bool PatternMatches(PatternSignature signature, IReadOnlyList<Expression> arguments, bool exact)
        {
            if (exact && signature.ArgumentValues.Count != arguments.Count)
                return false;

            for (int i = 0; i < arguments.Count; i++)
            {
                if (i < signature.ArgumentValues.Count)
                {
                    string matchValue = signature.ArgumentValues[i];
                    string argValue = arguments[i].LiteralValue?.ToString() ?? "";
                    if (matchValue != argValue)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the return type of the set of pattern signatures.
        /// The return type is either the type of the signature body if there is no path,
        /// or a database symbol containing variables named for each path.
        /// </summary>
        private TypeSymbol GetPatternReturnType(IReadOnlyList<PatternSignature> signatures)
        {
            if (signatures.Count == 1 && signatures[0].PathValue == null)
                return signatures[0].Signature.GetReturnType(_globals);

            var paths = s_symbolListPool.AllocateFromPool();
            var types = s_typeListPool.AllocateFromPool();
            try
            {
                foreach (var sig in signatures)
                {
                    var type = sig.Signature.GetReturnType(_globals);

                    if (sig.PathValue == null)
                    {
                        if (!types.Contains(type))
                        {
                            types.Add(type);
                        }
                    }
                    else
                    {
                        paths.Add(new VariableSymbol(sig.PathValue.ToString(), type));
                    }
                }

                if (paths.Count > 0)
                {
                    if (types.Count > 0)
                    {
                        // this should not happen, but in case it does
                        return new GroupSymbol(paths.Concat(types));
                    }
                    else
                    {
                        return new GroupSymbol(paths);
                    }
                }
                else if (types.Count == 1)
                {
                    return types[0];
                }
                else
                {
                    return new GroupSymbol(types);
                }
            }
            finally
            {
                s_symbolListPool.ReturnToPool(paths);
                s_typeListPool.ReturnToPool(types);
            }
        }

        /// <summary>
        /// Extracts the argument expressions and their result types into two separate lists.
        /// Handles the special case of the implicit invoke operator argument
        /// </summary>
        private void GetArgumentsAndTypes(
            FunctionCallExpression functionCall,
            List<Expression> arguments,
            List<TypeSymbol> argumentTypes)
        {
            var expressions = functionCall.ArgumentList.Expressions;

            for (int i = 0, n = expressions.Count; i < n; i++)
            {
                var arg = expressions[i].Element;
                arguments.Add(arg);
                argumentTypes.Add(GetResultTypeOrError(arg));
            }

            if (IsInvokeOperatorFunctionCall(functionCall))
            {
                // add fake argument to represent the implicit value
                arguments.Insert(0, functionCall.Name);
                argumentTypes.Insert(0, _implicitArgumentType ?? TableSymbol.Empty);
            }
        }

        /// <summary>
        /// Gets the parameters that correspond to the arguments.
        /// </summary>
        private void GetArgumentParameters(
            FunctionCallExpression functionCall,
            List<Parameter> parameters)
        {
            if (functionCall.ReferencedSignature is Signature signature)
            {
                var arguments = s_expressionListPool.AllocateFromPool();
                var argumentTypes = s_typeListPool.AllocateFromPool();

                try
                {
                    GetArgumentsAndTypes(functionCall, arguments, argumentTypes);
                    signature.GetArgumentParameters(arguments, parameters);
                }
                finally
                {
                    s_expressionListPool.ReturnToPool(arguments);
                    s_typeListPool.ReturnToPool(argumentTypes);
                }
            }
        }

        /// <summary>
        /// Gets the result information for the function call or operator invocation when invoked with the specified arguments.
        /// </summary>
        private FunctionCallResult GetFunctionCallResult(
            Signature signature,
            IReadOnlyList<Expression> arguments,
            IReadOnlyList<TypeSymbol> argumentTypes,
            SyntaxElement location,
            List<Diagnostic> diagnostics = null)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            if (argumentTypes == null)
                throw new ArgumentNullException(nameof(argumentTypes));

            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                signature.GetArgumentParameters(arguments, argumentParameters);
                return GetFunctionCallResult(signature, arguments, argumentTypes, argumentParameters, location, diagnostics);
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters);
            }
        }

        /// <summary>
        /// Gets the result information of the function call or operator invocation when invoked with the specified arguments.
        /// </summary>
        private FunctionCallResult GetFunctionCallResult(
            Signature signature,
            IReadOnlyList<Expression> arguments,
            IReadOnlyList<TypeSymbol> argumentTypes,
            IReadOnlyList<Parameter> argumentParameters,
            SyntaxElement location,
            List<Diagnostic> diagnostics = null)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            if (argumentTypes == null)
                throw new ArgumentNullException(nameof(argumentTypes));

            if (argumentParameters == null)
                throw new ArgumentNullException(nameof(argumentParameters));

            switch (signature.ReturnKind)
            {
                case ReturnTypeKind.Declared:
                    return signature.DeclaredReturnType;

                case ReturnTypeKind.Computed:
                    return this.GetComputedFunctionCallResult(signature, arguments, argumentTypes);

                case ReturnTypeKind.Parameter0:
                    var iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? argumentTypes[iArg] : ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter0Array:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? (TypeSymbol)ScalarTypes.GetDynamicArray(argumentTypes[iArg]) : ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter1:
                    iArg = argumentParameters.IndexOf(signature.Parameters[1]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? argumentTypes[iArg] : ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter2:
                    iArg = argumentParameters.IndexOf(signature.Parameters[2]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? argumentTypes[iArg] : ErrorSymbol.Instance;

                case ReturnTypeKind.ParameterN:
                    iArg = argumentParameters.IndexOf(signature.Parameters[signature.Parameters.Count - 1]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? argumentTypes[iArg] : ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter0Literal:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    return iArg >= 0 && iArg < arguments.Count ? GetTypeOfType(arguments[iArg]) : ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter1Literal:
                    iArg = argumentParameters.IndexOf(signature.Parameters[1]);
                    return iArg >= 0 && iArg < arguments.Count ? GetTypeOfType(arguments[iArg]) : ErrorSymbol.Instance;

                case ReturnTypeKind.ParameterNLiteral:
                    iArg = argumentParameters.IndexOf(signature.Parameters[signature.Parameters.Count - 1]);
                    return iArg >= 0 && iArg < arguments.Count ? GetTypeOfType(arguments[iArg]) : ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter0Promoted:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? TypeFacts.PromoteToLong(argumentTypes[iArg]) : ErrorSymbol.Instance;

                case ReturnTypeKind.Common:
                    return TypeFacts.GetCommonArgumentType(argumentParameters, argumentTypes) ?? ErrorSymbol.Instance;

                case ReturnTypeKind.CommonNonDynamic:
                    return TypeFacts.GetCommonArgumentType(argumentParameters, argumentTypes, ignoreDynamic: true) ?? ErrorSymbol.Instance;

                case ReturnTypeKind.Widest:
                    return TypeFacts.GetWidestScalarType(argumentTypes).PromoteToLong() ?? ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter0Cluster:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count
                        && TryGetLiteralStringValue(arguments[iArg], out var clusterName))
                    {
                        return GetClusterFunctionResult(clusterName, arguments[iArg], diagnostics);
                    }
                    else
                    {
                        return new ClusterSymbol("", null, isOpen: true);
                    }

                case ReturnTypeKind.Parameter0Database:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count
                        && TryGetLiteralStringValue(arguments[iArg], out var databaseName))
                    {
                        return GetDatabaseFunctionResult(databaseName, arguments[iArg], diagnostics);
                    }
                    else if (arguments.Count == 0)
                    {
                        // database() refers to current database
                        return _currentDatabase;
                    }
                    else
                    {
                        return new DatabaseSymbol("", null, isOpen: true);
                    }

                case ReturnTypeKind.Parameter0Table:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count
                        && TryGetLiteralStringValue(arguments[iArg], out var tableName))
                    {
                        return GetTableFunctionResult(tableName, arguments[iArg], diagnostics);
                    }
                    else
                    {
                        return TableSymbol.Empty.WithIsOpen(true);
                    }

                case ReturnTypeKind.Parameter0ExternalTable:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count
                        && TryGetLiteralStringValue(arguments[iArg], out var externalTableName))
                    {
                        return GetExternalTableFunctionResult(externalTableName, arguments[iArg], diagnostics);
                    }
                    else
                    {
                        return TableSymbol.Empty.WithIsOpen(true);
                    }
                case ReturnTypeKind.Parameter0MaterializedView:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count
                        && TryGetLiteralStringValue(arguments[iArg], out var materializedViewName))
                    {
                        return GetMaterializedViewFunctionResult(materializedViewName, arguments[iArg], diagnostics);
                    }
                    else
                    {
                        return TableSymbol.Empty.WithIsOpen(true);
                    }
                case ReturnTypeKind.Parameter0EntityGroup:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count
                        && TryGetLiteralStringValue(arguments[iArg], out var entityGroupName))
                    {
                        return GetEntityGroupFunctionResult(entityGroupName, arguments[iArg], diagnostics);
                    }
                    else
                    {
                        return new EntityGroupSymbol();
                    }
                case ReturnTypeKind.Parameter0StoredQueryResult:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count
                        && TryGetLiteralStringValue(arguments[iArg], out var sqrName))
                    {
                        return GetStoredQueryResultFunctionResult(sqrName, arguments[iArg], diagnostics);
                    }
                    else
                    {
                        return StoredQueryResultSymbol.Empty;
                    }
                case ReturnTypeKind.Parameter0Graph:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count
                        && TryGetLiteralStringValue(arguments[iArg], out var graphModelName))
                    {
                        iArg = argumentParameters.IndexOf(signature.Parameters[1]);
                        if (iArg >= 0 && iArg < arguments.Count)
                        {
                            if (TryGetLiteralStringValue(arguments[iArg], out var snapshotName))
                            {
                                return GetGraphFunctionResult(graphModelName, snapshotName, null, location, diagnostics);
                            }
                            else if (TryGetLiteralValue<bool>(arguments[iArg], out var isVolatile))
                            {
                                return GetGraphFunctionResult(graphModelName, null, isVolatile, location, diagnostics);
                            }
                        }
                        else
                        {
                            return GetGraphFunctionResult(graphModelName, null, null, location, diagnostics);
                        }
                    }
                    return GraphSymbol.Empty;

                case ReturnTypeKind.Custom:
                    var context = new BinderCallContext(this, location as SyntaxNode, arguments, argumentTypes, argumentParameters, signature);
                    return signature.CustomReturnType(context) ?? ErrorSymbol.Instance;

                default:
                    throw new NotImplementedException();
            }
        }

        private sealed class BinderCallContext : CustomReturnTypeContext
        {
            private readonly Binder _binder;
            private readonly SyntaxNode _location;
            private readonly IReadOnlyList<Expression> _arguments;
            private readonly IReadOnlyList<TypeSymbol> _argumentTypes;
            private readonly IReadOnlyList<Parameter> _argumentParameters;
            private readonly Signature _signature;

            public BinderCallContext(
                Binder binder, 
                SyntaxNode location, 
                IReadOnlyList<Expression> arguments, 
                IReadOnlyList<TypeSymbol> argumentTypes,
                IReadOnlyList<Parameter> argumentParameters,
                Signature signature)
            {
                _binder = binder;
                _location = location;
                _arguments = arguments;
                _argumentTypes = argumentTypes;
                _argumentParameters = argumentParameters;
                _signature = signature;
            }

            public override SyntaxNode Location => _location;
            public override IReadOnlyList<Expression> Arguments => _arguments;
            public override IReadOnlyList<TypeSymbol> ArgumentTypes => _argumentTypes;
            public override IReadOnlyList<Parameter> ArgumentParameters => _argumentParameters;
            public override Signature Signature => _signature;
            public override TableSymbol RowScope => _binder.RowScopeOrEmpty;
            public override GlobalState Globals => _binder._globals;
            public override ClusterSymbol CurrentCluster => _binder._currentCluster;
            public override DatabaseSymbol CurrentDatabase => _binder._currentDatabase;
            public override FunctionSymbol CurrentFunction => _binder._currentFunction;

            private static readonly SyntaxNode Nowhere =
                new NameReference(SyntaxToken.Missing(SyntaxKind.IdentifierToken));

            public override Symbol GetReferencedSymbol(string name)
            {
                var info = _binder.BindName(name, SymbolMatch.Default, Nowhere);
                return info?.ReferencedSymbol as Symbol;
            }

            public override TypeSymbol GetResultType(string name)
            {
                var info = _binder.BindName(name, SymbolMatch.Default, Nowhere);
                return info?.ResultType;
            }

            public override string GetResultName(Expression expr, string defaultName = "")
            {
                return Binder.GetExpressionResultName(expr, defaultName, this.RowScope);
            }
        }

        internal static bool TryGetLiteralStringValue(Expression expression, out string value)
        {
            if (TryGetLiteralValueInfo(expression, out var valueInfo))
            {
                value = valueInfo?.Value as string;
                return value != null;
            }
            else
            {
                value = null;
                return false;
            }
        }

        internal static bool TryGetLiteralValue<T>(Expression expression, out T value)
        {
            if (TryGetLiteralValueInfo(expression, out var valueInfo)
                && valueInfo?.Value is T tvalue)
            {
                value = tvalue;
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        /// <summary>
        /// Gets the value of the literal if the expression is a literal or refers to literal
        /// </summary>
        internal static bool TryGetLiteralValueInfo(Expression expression, out ValueInfo value)
        {
            expression = GetUnderlyingExpression(expression);

            if (expression.IsLiteral)
            {
                value = expression.LiteralValueInfo;
                return value != null;
            }
            else if (expression is NameReference nr && nr.ReferencedSymbol is VariableSymbol vs && vs.IsConstant)
            {
                value = vs.ConstantValueInfo;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// Determines if the name is a pattern (contains a *)
        /// </summary>
        private static bool IsPattern(string name)
        {
            return name.Contains("*");
        }

        /// <summary>
        /// Gets the cluster for the specified name, or an empty open cluster.
        /// </summary>
        private ClusterSymbol GetClusterFunctionResult(string name, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            var cluster = _globals.GetCluster(name);
            if (cluster == null)
            {
                if (diagnostics != null && location != null)
                {
                    if (_isFuzzy)
                    {
                        diagnostics.Add(DiagnosticFacts.GetFuzzyClusterNotDefined(name).WithLocation(location));
                    }
                    else
                    {
                        diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownCluster(name).WithLocation(location));
                    }
                }

                cluster = GetOpenCluster(name);
            }

            return cluster;
        }

        /// <summary>
        /// Gets the result for an invocation of the database() function
        /// </summary>
        private TypeSymbol GetDatabaseFunctionResult(string nameOrPattern, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            var db = GetMatchingDatabase(nameOrPattern, _pathScope);

            if (db == null)
            {
                if (diagnostics != null && location != null)
                {
                    if (_isFuzzy)
                    {
                        diagnostics.Add(DiagnosticFacts.GetFuzzyDatabaseNotDefined(nameOrPattern).WithLocation(location));
                    }
                    else
                    {
                        diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownDatabase(nameOrPattern).WithLocation(location));
                    }
                }

                if (!IsPattern(nameOrPattern))
                {
                    // return open database regardless of container's open state to reduce cascading errors
                    if (_pathScope == null)
                    {
                        db = GetOpenDatabase(nameOrPattern, _currentCluster);
                    }
                    else if (_pathScope is ClusterSymbol cluster)
                    {
                        db = GetOpenDatabase(nameOrPattern, cluster);
                    }
                    else if (_pathScope is GroupSymbol g)
                    {
                        // use the first cluster in the group 
                        var gc = g.Members.OfType<ClusterSymbol>().FirstOrDefault();
                        if (gc != null)
                        {
                            db = GetOpenDatabase(nameOrPattern, gc);
                        }
                    }
                }
            }

            return db;
        }

        /// <summary>
        /// Gets the named database or group of databases
        /// </summary>
        private TypeSymbol GetMatchingDatabase(string nameOrPattern, Symbol clusterOrGroup)
        {
            if ((clusterOrGroup == null 
                || clusterOrGroup == _currentCluster)
                && (string.IsNullOrEmpty(nameOrPattern)
                    || string.Compare(_currentDatabase.Name, nameOrPattern, ignoreCase: true) == 0
                    || string.Compare(_currentDatabase.AlternateName, nameOrPattern, ignoreCase: true) == 0))
            {
                return _currentDatabase;
            }

            if (_aliasedDatabases.TryGetValue(nameOrPattern, out var db))
            {
                return db;
            }

            var matching = s_symbolListPool.AllocateFromPool();
            try
            {
                if (clusterOrGroup == null)
                {
                    GetMatchingDatabases(nameOrPattern, _currentCluster, matching);
                }
                else if (clusterOrGroup is ClusterSymbol cluster)
                {
                    GetMatchingDatabases(nameOrPattern, cluster, matching);
                }
                else if (clusterOrGroup is GroupSymbol group)
                {
                    foreach (var s in group.Members)
                    {
                        if (s is ClusterSymbol c)
                        {
                            GetMatchingDatabases(nameOrPattern, c, matching);
                        }
                    }
                }

                if (matching.Count == 1)
                {
                    return (TypeSymbol)matching[0];
                }
                else if (matching.Count > 1)
                {
                    return new GroupSymbol(matching);
                }
                else if (!IsPattern(nameOrPattern))
                {
                    if (clusterOrGroup == null && _currentCluster.IsOpen)
                    {
                        return GetOpenDatabase(nameOrPattern, _currentCluster);
                    }
                    else if (clusterOrGroup is ClusterSymbol c && c.IsOpen)
                    {
                        return GetOpenDatabase(nameOrPattern, c);
                    }
                    else if (clusterOrGroup is GroupSymbol g)
                    {
                        // if any cluster in the group is open, return an open database corresponding to that cluster
                        foreach (var m in g.Members)
                        {
                            if (m is ClusterSymbol cs && cs.IsOpen)
                            {
                                return GetOpenDatabase(nameOrPattern, cs);
                            }
                        }
                    }
                }

                // no matching name and cannot be open database
                return null;
            }
            finally
            {
                s_symbolListPool.ReturnToPool(matching);
            }
        }

        /// <summary>
        /// Gets the matching databases in the specified cluster
        /// </summary>
        private static void GetMatchingDatabases(string nameOrPattern, ClusterSymbol cluster, List<Symbol> matches)
        {
            if (!string.IsNullOrEmpty(nameOrPattern))
            {
                foreach (var cdb in cluster.Databases)
                {
                    if (KustoFacts.Matches(nameOrPattern, cdb.Name, ignoreCase: true)
                        || KustoFacts.Matches(nameOrPattern, cdb.AlternateName, ignoreCase: true))
                    {
                        matches.Add(cdb);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the result of calling the table() function in the current context.
        /// </summary>
        private TypeSymbol GetTableFunctionResult(string nameOrPattern, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            // check for local table first
            if (_pathScope == null && !IsPattern(nameOrPattern))
            {
                var match = SymbolMatch.Table | SymbolMatch.Local | SymbolMatch.View;

                var symbols = s_symbolListPool.AllocateFromPool();
                try
                {
                    // check scope for variables, etc
                    _localScope.GetSymbols(nameOrPattern, match, symbols);

                    if (symbols.Count > 0)
                    {
                        var result = symbols[0];

                        if (result is FunctionSymbol fs 
                            && fs.IsView 
                            && fs.MinArgumentCount == 0)
                        {
                            result = fs.GetReturnType(_globals);
                        }
                        else
                        {
                            result = GetResultType(result);
                        }

                        return result as TableSymbol ?? (TypeSymbol)ErrorSymbol.Instance;
                    }
                }
                finally
                {
                    s_symbolListPool.ReturnToPool(symbols);
                }
            }

            var table = GetMatchingDatabaseTable(nameOrPattern, _pathScope);

            if (table == null)
            {
                if (diagnostics != null && location != null)
                {
                    if (_isFuzzy)
                    {
                        diagnostics.Add(DiagnosticFacts.GetFuzzyTableNotDefined(nameOrPattern).WithLocation(location));
                    }
                    else
                    {
                        diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownTable(nameOrPattern).WithLocation(location));
                    }
                }

                // return open table regardless of containing tables's open state to reduce cascading errors
                if (!IsPattern(nameOrPattern))
                {
                    if (_pathScope == null)
                    {
                        table = GetOpenTable(nameOrPattern, _currentDatabase);
                    }
                    else if (_pathScope is DatabaseSymbol db)
                    {
                        table = GetOpenTable(nameOrPattern, db);
                    }
                    else if (_pathScope is GroupSymbol g)
                    {
                        // make an open table based on the first database
                        var gdb = g.Members.OfType<DatabaseSymbol>().FirstOrDefault();
                        if (gdb != null)
                        {
                            table = GetOpenTable(nameOrPattern, gdb);
                        }
                    }
                }
            }

            return table;
        }

        /// <summary>
        /// Gets the matching table or group of tables
        /// </summary>
        private TypeSymbol GetMatchingDatabaseTable(string nameOrPattern, Symbol databaseOrGroup)
        {
            var matches = s_symbolListPool.AllocateFromPool();
            try
            {
                if (databaseOrGroup == null)
                {
                    GetMatchingDatabaseTables(nameOrPattern, _currentDatabase, matches);
                }
                else if (databaseOrGroup is DatabaseSymbol database)
                {
                    GetMatchingDatabaseTables(nameOrPattern, database, matches);
                }
                else if (databaseOrGroup is GroupSymbol group)
                {
                    foreach (var s in group.Members)
                    {
                        if (s is DatabaseSymbol db)
                        {
                            GetMatchingDatabaseTables(nameOrPattern, db, matches);
                        }
                    }
                }

                if (matches.Count == 1)
                {
                    return (TableSymbol)matches[0];
                }
                else if (matches.Count > 1)
                {
                    return new GroupSymbol(matches);
                }
                else if (!IsPattern(nameOrPattern))
                {
                    if (databaseOrGroup == null && _currentDatabase.IsOpen)
                    {
                        return GetOpenTable(nameOrPattern, _currentDatabase);
                    }
                    if (databaseOrGroup is DatabaseSymbol db && db.IsOpen)
                    {
                        return GetOpenTable(nameOrPattern, db);
                    }
                    else if (databaseOrGroup is GroupSymbol g)
                    {
                        // in any database in group is open, then return an open table corresponding to that database
                        foreach (var m in g.Members)
                        {
                            if (m is DatabaseSymbol gdb && gdb.IsOpen)
                            {
                                return GetOpenTable(nameOrPattern, gdb);
                            }
                        }
                    }
                }

                // nothing matched and could not invent an OpenTable
                return null;
            }
            finally
            {
                s_symbolListPool.ReturnToPool(matches);
            }
        }

        /// <summary>
        /// Gets all matching tables
        /// </summary>
        private void GetMatchingDatabaseTables(string nameOrPattern, DatabaseSymbol database, List<Symbol> matches)
        {
            foreach (var table in database.Tables)
            {
                if (KustoFacts.Matches(nameOrPattern, table.Name))
                {
                    matches.Add(table);
                }
            }
        }

        /// <summary>
        /// Gets the result of calling the external_table() function in the current context.
        /// </summary>
        private TypeSymbol GetExternalTableFunctionResult(string name, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            var db = _pathScope as DatabaseSymbol ?? _currentDatabase;
            var table = db.GetExternalTable(name);

            if (table == null)
            {
                if (diagnostics != null && location != null)
                {
                    if (_isFuzzy)
                    {
                        diagnostics.Add(DiagnosticFacts.GetFuzzyExternalTableNotDefined(name).WithLocation(location));
                    }
                    else
                    {
                        diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownExternalTable(name).WithLocation(location));
                    }
                }

                table = new TableSymbol(name).WithIsExternal(true).WithIsOpen(true);
            }

            return table;
        }

        /// <summary>
        /// Gets the result of calling the materialized_view() function in the current context.
        /// </summary>
        private TypeSymbol GetMaterializedViewFunctionResult(string name, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            var db = _pathScope as DatabaseSymbol ?? _currentDatabase;
            TableSymbol table = db.GetMaterializedView(name);

            if (table == null)
            {
                if (diagnostics != null && location != null)
                {
                    if (_isFuzzy)
                    {
                        diagnostics.Add(DiagnosticFacts.GetFuzzyMaterializedViewNotDefined(name).WithLocation(location));
                    }
                    else
                    {
                        diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownMaterializedView(name).WithLocation(location));
                    }
                }

                table = new TableSymbol(name).WithIsMaterializedView(true).WithIsOpen(true);
            }

            return table;
        }

        /// <summary>
        /// Gets the result of calling the entity_group() function in the current context.
        /// </summary>
        private FunctionCallResult GetEntityGroupFunctionResult(string name, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            var db = _pathScope as DatabaseSymbol ?? _currentDatabase;
            var entityGroup = db.GetEntityGroup(name);

            if (entityGroup == null)
            {
                if (diagnostics != null && location != null)
                {
                    if (_isFuzzy)
                    {
                        diagnostics.Add(DiagnosticFacts.GetFuzzyEntityGroupNotDefined(name).WithLocation(location));
                    }
                    else
                    {
                        diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownEntityGroup(name).WithLocation(location));
                    }
                }

                return new EntityGroupSymbol();
            }

            // return the computed result of the signature that refers to the entity group declaration.
            // This will allow the facts about the entities in the group to be visible to analysis.
            if (entityGroup.Signature != null)
                return this.GetComputedFunctionCallResult(entityGroup.Signature);

            return entityGroup;
        }

        /// <summary>
        /// Gets the result of calling the stored_query_result() function in the current context.
        /// </summary>
        private TypeSymbol GetStoredQueryResultFunctionResult(string name, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            var db = _pathScope as DatabaseSymbol ?? _currentDatabase;
            StoredQueryResultSymbol sqr = db.GetStoredQueryResult(name);

            if (sqr == null)
            {
                if (diagnostics != null && location != null)
                {
                    if (_isFuzzy)
                    {
                        diagnostics.Add(DiagnosticFacts.GetFuzzyStoredQueryResultNotDefined(name).WithLocation(location));
                    }
                    else
                    {
                        diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownStoredQueryResult(name).WithLocation(location));
                    }
                }

                sqr = new StoredQueryResultSymbol(name, EmptyReadOnlyList<ColumnSymbol>.Instance);
            }

            return sqr;
        }

        private GraphSymbol GetGraphFunctionResult(
            string modelName, string snapshotName, bool? isVolatile, 
            SyntaxElement location, List<Diagnostic> diagnostics)
        {
            var db = _pathScope as DatabaseSymbol ?? _currentDatabase;
            var model = db.GetGraphModel(modelName);
            if (model != null)
            {
                if (snapshotName != null)
                {
                    if (!model.TryGetSnapshot(snapshotName, out _))
                    {
                        diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownGraphSnapshot(snapshotName, modelName).WithLocation(location));
                    }
                }

                if (model.ComputedGraphSymbol == null)
                {
                    var prevCluster = _currentCluster;
                    var prevDatabase = _currentDatabase;

                    _currentCluster = _globals.GetCluster(db);
                    _currentDatabase = db;

                    var edgeShape = model.Edges.Count > 0 ? GetCombinedGraphResults(model.Edges) : null;
                    var nodeShape = model.Nodes.Count > 0 ? GetCombinedGraphResults(model.Nodes) : null;
                    var symbol = new GraphSymbol(edgeShape, nodeShape);

                    model.ComputedGraphSymbol = symbol;

                    _currentCluster = prevCluster;
                    _currentDatabase = prevDatabase;
                }

                return model.ComputedGraphSymbol;
            }
            else
            {
                diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownGraphModel(modelName).WithLocation(location));
            }

            return GraphSymbol.Empty;
        }

        private TableSymbol GetCombinedGraphResults(IReadOnlyList<Signature> signatures)
        {
            var tables = s_tableListPool.AllocateFromPool();
            try
            {
                foreach (var signature in signatures)
                {
                    FunctionCallResult fcResult = this.GetComputedFunctionCallResult(signature);
                    if (fcResult.Type is TableSymbol table)
                        tables.Add(table);
                }

                var resultTable = TableSymbol.Combine(CombineKind.UnifySameName, tables);
                return resultTable;
            }
            finally
            {
                s_tableListPool.ReturnToPool(tables);
            }
        }

        /// <summary>
        /// Determines if <see cref="P:type1"/> can be promoted to <see cref="P:type2"/>
        /// </summary>
        public static bool IsPromotable(TypeSymbol type1, TypeSymbol type2)
        {
            return type1 is ScalarSymbol type1Scalar && type2 is ScalarSymbol type2Scalar && type2Scalar.IsWiderThan(type1Scalar);
        }

        /// <summary>
        /// Promotes int to long
        /// </summary>
        public static TypeSymbol Promote(TypeSymbol symbol)
        {
            if (symbol == ScalarTypes.Int)
            {
                return ScalarTypes.Long;
            }
            else
            {
                return symbol;
            }
        }


        /// <summary>
        /// Gets the common return type across a set of signatures, or error if there is no common type.
        /// The common return type is the return type all the signatures share, or the error type if the return types differ.
        /// </summary>
        private TypeSymbol GetCommonReturnType(IReadOnlyList<Signature> signatures, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes, SyntaxElement location)
        {
            if (signatures.Count == 0)
            {
                return ErrorSymbol.Instance;
            }
            else if (signatures.Count == 1)
            {
                return GetFunctionCallResult(signatures[0], arguments, argumentTypes, location).Type;
            }
            else
            {
                var firstType = GetFunctionCallResult(signatures[0], arguments, argumentTypes, location).Type;

                for (int i = 1; i < signatures.Count; i++)
                {
                    var type = GetFunctionCallResult(signatures[i], arguments, argumentTypes, location).Type;
                    if (!SymbolsAssignable(firstType, type))
                    {
                        if (ArgumentsHaveErrorsOrUnknown(argumentTypes))
                        {
                            return ScalarTypes.Unknown;
                        }
                        else
                        {
                            return ErrorSymbol.Instance;
                        }
                    }
                }

                return firstType;
            }
        }

        /// <summary>
        /// Gets the signatures that best match the specified arguments.
        /// If there is no best match, then multiple signatures will be returned.
        /// </summary>
        private void GetBestMatchingSignatures(
            IReadOnlyList<Signature> signatures, 
            IReadOnlyList<Expression> arguments, 
            IReadOnlyList<TypeSymbol> argumentTypes, 
            List<Signature> result,
            bool requireAllArgumentsMatch = false)
        {
            var argCount = argumentTypes.Count;

            if (signatures.Count == 0)
            {
                return;
            }
            else if (signatures.Count == 1)
            {
                result.Add(signatures[0]);
                return;
            }

            // determine candidates
            if (signatures.Count > 1)
            {
                var closestCount = 0;
                var maxCount = 0;

                foreach (var s in signatures)
                {
                    if (argCount >= s.MinArgumentCount && argCount <= s.MaxArgumentCount)
                    {
                        result.Add(s);
                    }
                    else if (argCount < s.MinArgumentCount && closestCount > s.MinArgumentCount)
                    {
                        closestCount = s.MinArgumentCount;
                    }

                    if (s.MaxArgumentCount > maxCount)
                    {
                        maxCount = s.MaxArgumentCount;
                    }
                }

                // if we didn't already find candidates, pick all with closest count
                if (result.Count == 0)
                {
                    if (closestCount == 0)
                    {
                        closestCount = maxCount;
                    }

                    foreach (var s in signatures)
                    {
                        if (closestCount >= s.MinArgumentCount && closestCount <= s.MaxArgumentCount)
                        {
                            result.Add(s);
                        }
                    }
                }
            }

            // reduce results to best matching functions
            if (result.Count > 1)
            {
                int mostMatchingParameterCount = 0;

                // determine the most matching parameter count
                foreach (var s in result)
                {
                    var count = GetParameterMatchCount(s, arguments, argumentTypes);
                    if (count > mostMatchingParameterCount)
                    {
                        mostMatchingParameterCount = count;
                    }
                }

                if (requireAllArgumentsMatch && mostMatchingParameterCount < arguments.Count)
                {
                    mostMatchingParameterCount = arguments.Count;
                }

                // remove all candidates that do not have the most matching parameters
                for (int i = result.Count - 1; i >= 0; i--)
                {
                    var sig = result[i];
                    if (GetParameterMatchCount(sig, arguments, argumentTypes) != mostMatchingParameterCount)
                    {
                        result.RemoveAt(i);
                    }
                }

                // still more than one?  Try to find best match
                if (result.Count > 1)
                {
                    var best = result[0];
                    for (int i = 1; i < result.Count; i++)
                    {
                        var signatureCompare = CompareSignatureMatch(result[i], best, arguments, argumentTypes);
                        if (signatureCompare > 0)
                        {
                            best = result[i];
                        }
                        else if (signatureCompare == 0)
                        {
                            // these two signatures are ambiguous
                            return;
                        }
                    }

                    // go through again looking for signatures that somehow now compare
                    // as better or equal to the prevously determined best.
                    for (int i = 0; i < result.Count; i++)
                    {
                        if (result[i] != best)
                        {
                            var signatureCompare = CompareSignatureMatch(best, result[i], arguments, argumentTypes);
                            if (signatureCompare <= 0)
                            {
                                // now a different signature is better? This is ambiguous.
                                return;
                            }
                        }
                    }

                    // one was clearly the best
                    result.Clear();
                    result.Add(best);
                }
            }
        }

        /// <summary>
        /// Determines if <see cref="P:signature1"/> is a better match than <see cref="P:signature2"/> for the specified arguments.
        /// </summary>
        private int CompareSignatureMatch(Signature signature1, Signature signature2, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            var argCount = argumentTypes.Count;
            var matchCount1 = GetParameterMatchCount(signature1, arguments, argumentTypes);
            var matchCount2 = GetParameterMatchCount(signature2, arguments, argumentTypes);

            // if signature1 matches all arguments but signature2 does not, signature1 is better
            if (matchCount1 == argCount && matchCount2 < argCount)
                return matchCount1 - matchCount2;

            // signature with better worst overall parameter match wins
            var worstMatch1 = GetWorstParameterMatch(signature1, arguments, argumentTypes);
            var worstMatch2 = GetWorstParameterMatch(signature2, arguments, argumentTypes);

            var matchCompare = CompareParameterMatch(worstMatch1, worstMatch2);
            if (matchCompare != 0)
                return matchCompare;

            // signature with the better best overall parameter match wins
            var bestMatch1 = GetBestParameterMatch(signature1, arguments, argumentTypes);
            var bestMatch2 = GetBestParameterMatch(signature2, arguments, argumentTypes);

            matchCompare = CompareParameterMatch(bestMatch1, bestMatch2);
            if (matchCompare != 0)
                return matchCompare;

            // ambigous on betterness of parameter matches
            // signature with the most matching parameters wins
            return matchCount1 - matchCount2;
        }

        private static int CompareParameterMatch(ParameterMatchKind match1, ParameterMatchKind match2)
        {
            return match1 - match2;
        }

        /// <summary>
        /// Determines the number of arguments that match their corresponding signature parameter.
        /// </summary>
        private int GetParameterMatchCount(Signature signature, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            var argCount = argumentTypes.Count;
            int matches = 0;

            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                signature.GetArgumentParameters(arguments, argumentParameters);

                for (int i = 0; i < argCount; i++)
                {
                    if (GetParameterMatchKind(
                        signature,
                        argumentParameters, argumentTypes,
                        argumentParameters[i], arguments[i], argumentTypes[i]) != ParameterMatchKind.None)
                    {
                        matches++;
                    }
                }
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters);
            }


            return matches;
        }

        private static bool IsDefaultValueIndicator(Parameter parameter, Expression argument)
        {
            return parameter.DefaultValueIndicator != null
                && argument.ResultType == ScalarTypes.String
                && argument is LiteralExpression lit
                && lit.LiteralValue is string value
                && value == parameter.DefaultValueIndicator;
        }

        private ParameterMatchKind GetWorstParameterMatch(
            Signature signature,
            IReadOnlyList<Expression> arguments,
            IReadOnlyList<TypeSymbol> argumentTypes)
        {
            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                signature.GetArgumentParameters(arguments, argumentParameters);

                var worstMatchKind = ParameterMatchKind.Exact;

                for (int argumentIndex = 0; argumentIndex < arguments.Count; argumentIndex++)
                {
                    var matchKind = GetParameterMatchKind(signature, argumentParameters, argumentTypes, argumentParameters[argumentIndex], arguments[argumentIndex], argumentTypes[argumentIndex]);
                    if (matchKind < worstMatchKind)
                        worstMatchKind = matchKind;
                }

                return worstMatchKind;
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters);
            }
        }

        private ParameterMatchKind GetBestParameterMatch(
            Signature signature,
            IReadOnlyList<Expression> arguments,
            IReadOnlyList<TypeSymbol> argumentTypes)
        {
            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                signature.GetArgumentParameters(arguments, argumentParameters);

                var bestMatchKind = ParameterMatchKind.None;

                for (int argumentIndex = 0; argumentIndex < arguments.Count; argumentIndex++)
                {
                    var matchKind = GetParameterMatchKind(signature, argumentParameters, argumentTypes, argumentParameters[argumentIndex], arguments[argumentIndex], argumentTypes[argumentIndex]);
                    if (matchKind > bestMatchKind)
                        bestMatchKind = matchKind;
                }

                return bestMatchKind;
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters);
            }
        }

        private ParameterMatchKind GetParameterMatchKind(
            Signature signature,
            IReadOnlyList<Parameter> argumentParameters,
            IReadOnlyList<TypeSymbol> argumentTypes,
            Parameter parameter,
            Expression argument,
            TypeSymbol argumentType)
        {
            return GetParameterMatchKind(signature, argumentParameters, argumentTypes, parameter, argument, argumentType, AllowImplicitArgumentCoercion(signature));
        }

        /// <summary>
        /// Determines the kind of match that the argument has with its corresponding signature parameter.
        /// </summary>
        public static ParameterMatchKind GetParameterMatchKind(
            Signature signature,
            IReadOnlyList<Parameter> argumentParameters,
            IReadOnlyList<TypeSymbol> argumentTypes,
            Parameter parameter,
            Expression argument,
            TypeSymbol argumentType,
            bool allowImplicitArgumentCoercion)
        {
            if (parameter == null)
                return ParameterMatchKind.None;

            if (argumentType == ScalarTypes.Unknown)
                return ParameterMatchKind.Unknown;

            if (IsDefaultValueIndicator(parameter, argument))
            {
                return ParameterMatchKind.Exact;
            }

            if (argument is StarExpression)
            {
                return (parameter.ArgumentKind == ArgumentKind.StarOnly
                    || parameter.ArgumentKind == ArgumentKind.StarAllowed)
                        ? ParameterMatchKind.Exact
                        : ParameterMatchKind.None;
            }
            else if (parameter.ArgumentKind == ArgumentKind.StarOnly)
            {
                return ParameterMatchKind.None;
            }

            switch (parameter.TypeKind)
            {
                case ParameterTypeKind.Any:
                    return ParameterMatchKind.Unknown;

                case ParameterTypeKind.Declared:
                    if (SymbolsAssignable(parameter.DeclaredTypes, argumentType, Conversion.None))
                    {
                        if (parameter.DeclaredTypes.Count == 1)
                        {
                            return ParameterMatchKind.Exact;
                        }
                        else
                        {
                            return ParameterMatchKind.OneOfMany;
                        }
                    }
                    else if (SymbolsAssignable(parameter.DeclaredTypes, argumentType, Conversion.Promotable))
                    {
                        return ParameterMatchKind.Promoted;
                    }
                    else if (allowImplicitArgumentCoercion
                        && SymbolsAssignable(parameter.DeclaredTypes, argumentType, Conversion.Dynamic))
                    {
                        return ParameterMatchKind.Dynamic;
                    }
                    else if (allowImplicitArgumentCoercion
                        && SymbolsAssignable(parameter.DeclaredTypes, argumentType, Conversion.Compatible))
                    {
                        return ParameterMatchKind.Compatible;
                    }
                    break;

                case ParameterTypeKind.Scalar:
                    if (argumentType.IsScalar)
                        return ParameterMatchKind.Scalar;
                    break;

                case ParameterTypeKind.Integer:
                    if (TypeFacts.IsInteger(argumentType))
                        return ParameterMatchKind.Integer;
                    break;

                case ParameterTypeKind.RealOrDecimal:
                    if (TypeFacts.IsRealOrDecimal(argumentType))
                        return ParameterMatchKind.OneOfMany;
                    break;

                case ParameterTypeKind.StringOrDynamic:
                    if (TypeFacts.IsStringOrDynamic(argumentType))
                        return ParameterMatchKind.OneOfMany;
                    break;

                case ParameterTypeKind.StringOrArray:
                    if (TypeFacts.IsStringOrArray(argumentType))
                        return ParameterMatchKind.OneOfMany;
                    break;

                case ParameterTypeKind.IntegerOrArray:
                    if (TypeFacts.IsIntegerOrArray(argumentType))
                        return ParameterMatchKind.OneOfMany;
                    break;

                case ParameterTypeKind.DynamicArray:
                    if (TypeFacts.IsDynamicArray(argumentType))
                        return ParameterMatchKind.OneOfMany;
                    break;

                case ParameterTypeKind.DynamicBag:
                    if (TypeFacts.IsDynamicBag(argumentType))
                        return ParameterMatchKind.OneOfMany;
                    break;

                case ParameterTypeKind.Number:
                    if (TypeFacts.IsNumeric(argumentType))
                        return ParameterMatchKind.Number;
                    break;

                case ParameterTypeKind.NumberOrBool:
                    if (TypeFacts.IsNumeric(argumentType) || argumentType == ScalarTypes.Bool)
                        return ParameterMatchKind.Number;
                    break;

                case ParameterTypeKind.Summable:
                    if (TypeFacts.IsSummable(argumentType))
                        return ParameterMatchKind.Summable;
                    break;
                case ParameterTypeKind.Orderable:
                    if (TypeFacts.IsOrderable(argumentType))
                        return ParameterMatchKind.Orderable;
                    break;
                case ParameterTypeKind.Tabular:
                    if (IsTabular(argumentType))
                        return ParameterMatchKind.Tabular;
                    break;

                case ParameterTypeKind.Database:
                    if (IsDatabase(argumentType))
                        return ParameterMatchKind.Database;
                    break;

                case ParameterTypeKind.Cluster:
                    if (IsCluster(argumentType))
                        return ParameterMatchKind.Cluster;
                    break;

                case ParameterTypeKind.NotBool:
                    if (!SymbolsAssignable(argumentType, ScalarTypes.Bool))
                        return ParameterMatchKind.NotType;
                    break;

                case ParameterTypeKind.NotRealOrBool:
                    if (!SymbolsAssignable(argumentType, ScalarTypes.Real)
                        && !SymbolsAssignable(argumentType, ScalarTypes.Bool))
                        return ParameterMatchKind.NotType;
                    break;

                case ParameterTypeKind.NotDynamic:
                    if (argumentType.IsAnyScalarExceptDynamic())
                        return ParameterMatchKind.NotType;
                    break;

                case ParameterTypeKind.Parameter0:
                    return GetParameterMatchKind(signature, argumentParameters, argumentTypes, argumentParameters[0], argument, argumentType, allowImplicitArgumentCoercion);

                case ParameterTypeKind.Parameter1:
                    return GetParameterMatchKind(signature, argumentParameters, argumentTypes, argumentParameters[1], argument, argumentType, allowImplicitArgumentCoercion);

                case ParameterTypeKind.Parameter2:
                    return GetParameterMatchKind(signature, argumentParameters, argumentTypes, argumentParameters[2], argument, argumentType, allowImplicitArgumentCoercion);

                case ParameterTypeKind.CommonScalar:
                case ParameterTypeKind.CommonNumber:
                case ParameterTypeKind.CommonSummable:
                case ParameterTypeKind.CommonOrderable:
                case ParameterTypeKind.CommonScalarOrDynamic:
                    var commonType = TypeFacts.GetCommonArgumentType(argumentParameters, argumentTypes);
                    if (commonType != null)
                    {
                        if (SymbolsAssignable(commonType, argumentType, Conversion.None))
                        {
                            return ParameterMatchKind.Exact;
                        }
                        else if (SymbolsAssignable(commonType, argumentType, Conversion.Promotable))
                        {
                            return ParameterMatchKind.Promoted;
                        }
                        else if (SymbolsAssignable(commonType, argumentType, Conversion.Dynamic))
                        {
                            return ParameterMatchKind.Dynamic;
                        }
                        else if (allowImplicitArgumentCoercion
                            && SymbolsAssignable(commonType, argumentType, Conversion.Compatible))
                        {
                            return ParameterMatchKind.Compatible;
                        }
                        else if (parameter.TypeKind == ParameterTypeKind.CommonScalarOrDynamic
                                 && SymbolsAssignable(argumentType, ScalarTypes.Dynamic))
                        {
                            return ParameterMatchKind.Exact;
                        }
                    }
                    break;
            }

            return ParameterMatchKind.None;
        }

        /// <summary>
        /// Gets <see cref="FunctionCallResult"/> for computed functions that have bodies that must be parsed and bound before understanding the result type.
        /// </summary>
        private FunctionCallResult GetComputedFunctionCallResult(Signature signature, IReadOnlyList<Expression> arguments = null, IReadOnlyList<TypeSymbol> argumentTypes = null)
        {
            TryGetFunctionBodyFacts(signature, out var funFacts);

            // if the function is not yet analyzed or is known to have a variable return type
            // then compute the function body facts and return type for this location by getting the expansion.
            if (funFacts == null || funFacts.HasVariableReturnType)
            {
                CallSiteInfo callSite = null;

                if (funFacts != null
                    && arguments != null
                    && TryGetResultTypeCallSite(signature, funFacts.DependentParameters, arguments, out callSite)
                    && CanCacheResultType(callSite)
                    && TryGetResultTypeFromCache(callSite, out var resultType))
                {
                    // return known result type w/ deferred expansion
                    return new FunctionCallResult(
                        resultType,
                        new FunctionCallInfo(GetDeferredFunctionCallExpansion(signature, arguments, argumentTypes), funFacts));
                }

                // use expansion at this call site to determine correct return type
                // if signature facts was not yet known, it will be computed by calling GetCallSiteExpansion
                var expansion = this.GetFunctionCallExpansion(signature, arguments, argumentTypes);
                
                // get computed fun facts
                TryGetFunctionBodyFacts(signature, out funFacts);

                var returnType = GetBodyResultType(expansion?.Root);

                if (returnType == null || returnType.IsError)
                    returnType = ScalarTypes.Unknown;

                if (funFacts != null
                    && arguments != null
                    && (callSite != null || TryGetResultTypeCallSite(signature, funFacts.DependentParameters, arguments, out callSite))
                    && CanCacheResultType(callSite))
                {
                    // add result type to cache for these arguments
                    // so we don't have to recompute/re-expand it when referenced again.
                    AddResultTypeToCache(callSite, returnType);
                }

                return new FunctionCallResult(returnType, new FunctionCallInfo(expansion, funFacts));
            }
            else
            {
                // body has non-variable (fixed) return type.
                return new FunctionCallResult(
                    funFacts.NonVariableReturnType,
                    new FunctionCallInfo(GetDeferredFunctionCallExpansion(signature, arguments, argumentTypes), funFacts));
            }
        }

        /// <summary>
        /// Returns true if the function call at this callsite allows caching of result type.
        /// </summary>
        private bool CanCacheResultType(CallSiteInfo callSite)
        {
            TryGetFunctionBodyFacts(callSite.Signature, out var funFacts);

            if (funFacts == null)
                return false;

            if (!funFacts.HasVariableReturnType)
                return false;

            if (funFacts.HasUnqualifiedTableCall)
            {
                if (IsLocalTabularVariable(funFacts.UnqualifiedTableNames))
                    return false;

                foreach (var value in callSite.Values)
                {
                    if (value is string possibleTableName
                        && IsLocalTabularVariable(possibleTableName))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets a 'result type' callsite for a function call.
        /// </summary>
        private static bool TryGetResultTypeCallSite(
            Signature signature,
            IReadOnlyList<Parameter> dependentParameters,
            IReadOnlyList<Expression> arguments,
            out CallSiteInfo callSiteInfo
            )
        {
            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                signature.GetArgumentParameters(arguments, argumentParameters);

                var values = new object[dependentParameters.Count];
                for (int i = 0; i < dependentParameters.Count; i++)
                {
                    var p = dependentParameters[i];
                    if (p != null)
                    {
                        var index = argumentParameters.IndexOf(p);
                        if (index < 0 || index >= arguments.Count)
                        {
                            callSiteInfo = null;
                            return false;
                        }

                        var arg = arguments[index];
                        if (TryGetCallSiteArgumentValue(arguments[index], out var value))
                        {
                            values[i] = value;
                        }
                        else
                        {
                            callSiteInfo = null;
                            return false;
                        }
                    }
                }

                callSiteInfo = new CallSiteInfo(signature, dependentParameters, values);
                return true;
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters);
            }
        }

        /// <summary>
        /// Gets the value of a function call argument 
        /// that can be used as a callsite value.
        /// </summary>
        private static bool TryGetCallSiteArgumentValue(Expression arg, out object value)
        {
            if (arg.ConstantValueInfo != null)
            {
                value = arg.ConstantValueInfo.Value;
                return true;
            }
            else if (arg.ResultType is TableSymbol table)
            {
                value = table;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the result type for the callsite, if cached.
        /// </summary>
        private bool TryGetResultTypeFromCache(CallSiteInfo callSiteInfo, out TypeSymbol type)
        {
            if (_localBindingCache.CallSiteToResultTypeMap.TryGetValue(callSiteInfo, out type))
                return true;

            if (_globalBindingCache.CallSiteToResultTypeMap.TryGetValue(callSiteInfo.Signature, out var globalMru)
                && globalMru.TryGetValue(callSiteInfo, out type))
                return true;

            type = null;
            return false;
        }

        /// <summary>
        /// Adds the result type for the call site to the cache
        /// </summary>
        private void AddResultTypeToCache(CallSiteInfo callsite, TypeSymbol resultType)
        {
            var shouldCacheGlobally = IsDatabaseSymbolSignature(callsite.Signature);
            if (shouldCacheGlobally)
            {
                if (!_globalBindingCache.CallSiteToResultTypeMap.TryGetValue(callsite.Signature, out var globalMru))
                {
                    globalMru = _globalBindingCache.CallSiteToResultTypeMap.GetOrAdd(
                        callsite.Signature, _ => new MostRecentlyUsedCache<CallSiteInfo, TypeSymbol>(_globals.GetProperty(Properties.MaxCachedResultTypes))
                        );
                }

                globalMru.AddOrUpdate(callsite, resultType);
            }
            else
            {
                _localBindingCache.CallSiteToResultTypeMap.Add(callsite, resultType);
            }
        }

        private static TypeSymbol GetBodyResultType(SyntaxNode body)
        {
            return 
                body is Expression exprBody ? exprBody.ResultType
                : body is FunctionBody functionBody ? functionBody.Expression?.ResultType
                : null;
        }

        private static bool HasSyntaxErrors(SyntaxNode syntax)
        {
            return syntax != null
                && syntax.ContainsSyntaxDiagnostics
                && syntax.GetContainedSyntaxDiagnostics().Any(d => d.Severity == DiagnosticSeverity.Error);
        }

        private Func<FunctionCallExpansion> GetDeferredFunctionCallExpansion(Signature signature, IReadOnlyList<Expression> arguments = null, IReadOnlyList<TypeSymbol> argumentTypes = null)
        {
            FunctionCallExpansion expansion = null;
            var args = arguments.ToReadOnly(); // force copy
            var types = argumentTypes.ToReadOnly(); // force copy

            return () =>
            {
                if (expansion == null)
                {
                    // re-introduce binding lock since deferred function can be called outside the current binding lock
                    lock (this._globalBindingCache)
                    {
                        expansion = this.GetFunctionCallExpansion(signature, args, types);
                    }
                }

                return expansion;
            };
        }

        /// <summary>
        /// Gets the inline expansion of a function call
        /// </summary>
        internal FunctionCallExpansion GetFunctionCallExpansion(
            Signature signature,
            IReadOnlyList<Expression> arguments = null,
            IReadOnlyList<TypeSymbol> argumentTypes = null)
        {
            if (signature.ReturnKind != ReturnTypeKind.Computed)
                return null;

            // block cycles in computation
            if (_localBindingCache.SignaturesComputingExpansion.Contains(signature))
                return null;

            _localBindingCache.SignaturesComputingExpansion.Add(signature);
            try
            {
                FunctionCallExpansion expansion = null;

                TryGetExpansionCallSiteInfo(signature, arguments, out var callSiteInfo);

                if (callSiteInfo != null 
                    && CanCacheExpansion(callSiteInfo)
                    && TryGetExpansionFromCache(callSiteInfo, out expansion))
                {
                    return expansion;
                }

                try
                { 
                    var body = GetUnboundBody(signature);
                    if (body != null)
                    {
                        var isInDatabase = IsDatabaseSymbolSignature(signature);
                        var currentDatabase = isInDatabase ? _globals.GetDatabase(signature.Symbol) : null;
                        var currentCluster = isInDatabase ? _globals.GetCluster(currentDatabase) : null;

                        if (signature.Declaration != null)
                        {
                            // associate new tree with tree it originated from
                            expansion = new FunctionCallExpansion(body, signature.Declaration.Tree, signature.Declaration.TriviaStart);
                        }
                        else
                        {
                            expansion = new FunctionCallExpansion(body);
                        }

                        var staticScope = GetOuterScope(signature);

                        var locals = GetCallSiteArgumentsAsVariables(signature, arguments, argumentTypes);
                        if (TryBindCalledFunctionBody(expansion, this, currentCluster, currentDatabase, signature.Symbol as FunctionSymbol, staticScope, locals))
                        {
                            // compute function body facts as side effect
                            var _ = GetOrComputeFunctionBodyFacts(signature, expansion.Root);
                        }
                        else
                        {
                            // don't return expansion that did not bind
                            expansion = null;
                        }
                    }
                }
                catch (Exception)
                {
                    // don't return expansion that failed in binding
                    expansion = null;
                }

                // record number of times each signature is expanded
                _localBindingCache.FunctionExpansionCounts.AddOrUpdate(signature, k => 1, (k, v) => v + 1);

                if (expansion != null 
                    && callSiteInfo != null
                    && CanCacheExpansion(callSiteInfo))
                {
                    AddExpansionToCache(callSiteInfo, expansion);
                }

                return expansion;
            }
            finally
            {
                _localBindingCache.SignaturesComputingExpansion.Remove(signature);
            }
        }

        /// <summary>
        /// Gets the cached expansion for the function at the call site.
        /// </summary>
        bool TryGetExpansionFromCache(CallSiteInfo callsite, out FunctionCallExpansion expansion)
        {
            return _localBindingCache.CallSiteToExpansionMap.TryGetValue(callsite, out expansion)
                || (_globalBindingCache.CallSiteToExpansionMap.TryGetValue(callsite.Signature, out var globalMru)
                    && globalMru.TryGetValue(callsite, out expansion));
        }

        /// <summary>
        /// Returns true if the function can have its expansions cached.
        /// </summary>
        private bool CanCacheExpansion(CallSiteInfo callSite)
        {
            TryGetFunctionBodyFacts(callSite.Signature, out var funFacts);

            if (funFacts == null)
                return false;

            if (!funFacts.HasUnqualifiedTableCall)
                return true;

            if (IsLocalTabularVariable(funFacts.UnqualifiedTableNames))
                return false;

            // check any argument value that might end up as argument to unqualified table call
            foreach (var dp in funFacts.DependentParameters)
            {
                var index = callSite.Parameters.IndexOf(dp);
                if (index >= 0 && index < callSite.Values.Count
                    && callSite.Values[index] is string possibleTableName)
                {
                    if (IsLocalTabularVariable(possibleTableName))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Return true if the name can refer to a local tabular variable in dynamic scope.
        /// </summary>
        private bool IsLocalTabularVariable(string name)
        {
            return _localScope.ContainsSymbol(name, SymbolMatch.Tabular | SymbolMatch.Local);
        }

        /// <summary>
        /// Returns true if any of the names can refer to a local tabular variable in dynamic scope.
        /// </summary>
        private bool IsLocalTabularVariable(IReadOnlyList<string> names)
        {
            if (names.Count == 0)
                return false;

            if (names.Count == 1)
                return IsLocalTabularVariable(names[0]);

            return names.Any(name => IsLocalTabularVariable(name));
        }

        /// <summary>
        /// Adds expansion to either global or local cache.
        /// </summary>
        void AddExpansionToCache(CallSiteInfo callsite, FunctionCallExpansion expansion)
        {
            var shouldCacheGlobally = IsDatabaseSymbolSignature(callsite.Signature);
            if (shouldCacheGlobally)
            {
                if (!_globalBindingCache.CallSiteToExpansionMap.TryGetValue(callsite.Signature, out var globalMru))
                {
                    globalMru = _globalBindingCache.CallSiteToExpansionMap.GetOrAdd(
                        callsite.Signature,
                        _ => new MostRecentlyUsedCache<CallSiteInfo, FunctionCallExpansion>(_globals.GetProperty(Properties.MaxCachedExpansions))
                        );
                }

                globalMru.AddOrUpdate(callsite, expansion);
            }
            else
            {
                _localBindingCache.CallSiteToExpansionMap.Add(callsite, expansion);
            }
        }

        /// <summary>
        /// True if the signature is declared by a symbol that is part of database known to the current <see cref="GlobalState"/>
        /// </summary>
        private bool IsDatabaseSymbolSignature(Signature signature)
        {
            return signature != null
                && signature.Symbol != null
                && signature.Declaration == null   // they don't have syntax trees (yet)
                && signature.Body != null          // they do have a body as text
                && _globals.IsDatabaseSymbol(signature.Symbol);       // and they are known by the global state
        }

        /// <summary>
        /// Gets the 'expansion' call site info for a function call.
        /// </summary>
        private bool TryGetExpansionCallSiteInfo(
            Signature signature, 
            IReadOnlyList<Expression> arguments, 
            out CallSiteInfo callSite)
        {
            if (arguments != null)
            {
                var parameters = new List<Parameter>(arguments.Count);
                var values = new List<object>(arguments.Count);
                if (TryGetCallSiteParametersAndValues(signature, arguments, parameters, values))
                {
                    callSite = new CallSiteInfo(signature, parameters, values);
                    return true;
                }
            }

            callSite = null;
            return false;
        }

        /// <summary>
        /// Gets the correpsonding set of parameters and argument values
        /// for the function call.
        /// </summary>
        private bool TryGetCallSiteParametersAndValues(
            Signature signature, 
            IReadOnlyList<Expression> arguments, 
            List<Parameter> parameters,
            List<object> values)
        {
            if (arguments == null)
                return false;

            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                signature.GetArgumentParameters(arguments, argumentParameters);

                foreach (var p in signature.Parameters)
                {
                    var argIndex = argumentParameters != null ? argumentParameters.IndexOf(p) : -1;

                    if (argIndex >= 0 
                        && argIndex < arguments.Count)
                    {
                        if (TryGetCallSiteArgumentValue(arguments[argIndex], out var value))
                        {
                            parameters.Add(argumentParameters[argIndex]);
                            values.Add(value);
                        }
                    }
                    else if (p.DefaultValue != null)
                    {
                        if (TryGetCallSiteArgumentValue(p.DefaultValue, out var value))
                        {
                            parameters.Add(p);
                            values.Add(value);
                        }
                    }
                }

                return true;
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters);
            }
        }

        /// <summary>
        /// Gets the set of local variables to use instead of argument parameters for the expansion of the function call.
        /// </summary>
        private IReadOnlyList<VariableSymbol> GetCallSiteArgumentsAsVariables(Signature signature, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            var locals = new List<VariableSymbol>();

            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                if (arguments != null)
                {
                    signature.GetArgumentParameters(arguments, argumentParameters);
                }
                else if (argumentTypes != null)
                {
                    signature.GetArgumentParameters(argumentTypes, argumentParameters);
                }

                foreach (var p in signature.Parameters)
                {
                    var argIndex = argumentParameters != null ? argumentParameters.IndexOf(p) : -1;

                    if (argIndex >= 0 && arguments != null & argIndex < arguments.Count)
                    {
                        var arg = arguments[argIndex];

                        var argType = argumentTypes != null && argIndex < argumentTypes.Count
                            ? argumentTypes[argIndex]
                            : arg.ResultType;

                        // use parameter type as variable type if scalar, to avoid analyzing function bodies with incorrect types.
                        var localType = p.IsScalar && p.TypeKind == ParameterTypeKind.Declared
                            ? p.DeclaredTypes[0]
                            : argType;

                        var isLiteral = Binder.TryGetLiteralValueInfo(arg, out var valueInfo);
                        locals.Add(new VariableSymbol(p.Name, localType, isLiteral, valueInfo, source: arg));
                    }
                    else
                    {
                        var type = GetRepresentativeType(p);

                        var isConstant = p.IsOptional && p.DefaultValue != null;
                        ValueInfo valueInfo = null;
                        if (isConstant)
                        {
                            TryGetLiteralValueInfo(p.DefaultValue, out valueInfo);
                        }

                        locals.Add(new VariableSymbol(p.Name, type, isConstant, valueInfo));
                    }
                }

                return locals.ToReadOnly();
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters);
            }
        }

        /// <summary>
        /// Builds an expanded declaration of the function customized given the arguments used at the call site.
        /// </summary>
        private static string GetFunctionBodyText(Signature signature)
        {
            var body = signature.Body.Trim();

            if (!body.StartsWith("{", StringComparison.Ordinal))
                body = "{" + body;

            if (!body.EndsWith("}", StringComparison.Ordinal))
                body += "\n}";

            return body;
        }

        private static string GetEntityGroupBodyText(Signature signature)
        {
            // translate to correct body form already handled by EntityGroupSymbol
            return signature.Body;
        }

        private static SyntaxNode GetUnboundBody(Signature signature)
        {
            if (signature.Declaration != null)
            {
                return signature.Declaration.Clone();
            }
            else if (signature.Symbol is EntityGroupSymbol)
            {
                var text = GetEntityGroupBodyText(signature);
                return QueryParser.ParseEntityGroup(text);
            }
            else
            {
                var text = GetFunctionBodyText(signature);
                return QueryParser.ParseFunctionBody(text);
            }
        }

        private LocalScope GetOuterScope(Signature signature)
        {
            if (signature.Declaration != null
                && signature.Declaration.Parent is FunctionDeclaration fd
                && _staticScopes.TryGetValue(fd, out var scope))
            {
                return scope;
            }

            return null;
        }

        private static TypeSymbol GetRepresentativeType(Parameter parameter)
        {
            switch (parameter.TypeKind)
            {
                case ParameterTypeKind.Declared:
                    return parameter.DeclaredTypes[0];
                case ParameterTypeKind.Tabular:
                    return TableSymbol.Empty;
                default:
                    return ScalarTypes.Dynamic;
            }
        }

        internal FunctionBodyFacts GetOrComputeFunctionBodyFacts(Signature signature, SyntaxNode body)
        {
            if (!TryGetFunctionBodyFacts(signature, out var facts))
            {
                facts = ComputeFunctionBodyFacts(signature, body);
                SetFunctionBodyFacts(signature, facts);
            }

            return facts;
        }

        internal bool TryGetFunctionBodyFacts(Signature signature, out FunctionBodyFacts facts)
        {
            if (_globalBindingCache.DatabaseFunctionBodyFacts.TryGetValue(signature, out facts))
                return true;

            if (_localBindingCache.NonDatabaseFunctionBodyFacts.TryGetValue(signature, out facts))
                return true;

            facts = null;
            return false;
        }

        internal void SetFunctionBodyFacts(Signature signature, FunctionBodyFacts facts)
        {
            if (IsDatabaseSymbolSignature(signature))
            {
                _globalBindingCache.DatabaseFunctionBodyFacts.AddOrUpdate(signature, facts);
            }
            else
            {
                _localBindingCache.NonDatabaseFunctionBodyFacts[signature] = facts;
            }
        }

        private FunctionBodyFacts ComputeFunctionBodyFacts(Signature signature, SyntaxNode body)
        {
            var result = FunctionBodyFacts.Default;
            var isTabular = GetBodyResultType(body) is TableSymbol;

            // if the function returns a table, mark all tabular parameters as dependent
            if (isTabular && signature.Parameters.Any(p => p.IsTabular))
            {
                result = result.AddDependentParameters(signature.Parameters.Where(p => p.IsTabular));
            }

            var argParams = s_parameterListPool.AllocateFromPool();

            // identify dependent parameters and other function body facts
            SyntaxElement.WalkNodes(
                body,
                fnBefore: node =>
                {
                    if (node is FunctionCallExpression fc
                        && IsSymbolLookupFunction(fc.ReferencedSymbol))
                    {
                        var isUnqualifiedTableCall = false;

                        if (fc.ReferencedSymbol == Functions.Table)
                        {
                            // distinguish between database(d).table(t) vs just table(t)
                            // since table(t) can see local variables from dynamic scope
                            if (fc.Parent is PathExpression p && p.Selector == fc)
                            {
                                result = result.WithHasQualifiedTableCall(true);
                            }
                            else
                            {
                                // unqualified table calls (even with literal arguments) can be dependent on the call site since
                                // the names can reference tabular variables in outer scopes
                                result = result.WithHasUnqualifiedTableCall(true);
                                isUnqualifiedTableCall = true;
                            }
                        }
                        else if (fc.ReferencedSymbol == Functions.ExternalTable)
                        {
                            result = result.WithHasExternalTableCall(true);
                        }
                        else if (fc.ReferencedSymbol == Functions.MaterializedView)
                        {
                            result = result.WithHasMaterializedViewCall(true);
                        }
                        else if (fc.ReferencedSymbol == Functions.Database)
                        {
                            result = result.WithHasDatabaseCall(true);
                        }
                        else if (fc.ReferencedSymbol == Functions.Cluster)
                        {
                            result = result.WithHasClusterCall(true);
                        }
                        else if (fc.ReferencedSymbol == Functions.EntityGroup
                            && fc.GetCalledFunctionFacts() is FunctionBodyFacts egCallFacts)
                        {
                            // get facts from analysis of the entity-group definition
                            result = result.CombineCalledFunction(egCallFacts);
                        }
                        else if (fc.ReferencedSymbol == Functions.StoredQueryResult)
                        {
                            result = result.WithHasStoredQueryResultCall(true);
                        }
                        else if (fc.ReferencedSymbol == Functions.Graph)
                        {
                            result = result.WithHasGraphCall(true);
                        }

                        // Any reference to an enclosing function's parameter in arguments is a dependent parameter.
                        // Some arguments may not affect the result type, but err on the side of caution.
                        for (int i = 0; i < fc.ArgumentList.Expressions.Count; i++)
                        {
                            var arg = fc.ArgumentList.Expressions[i].Element;
                            result = AddReferencedParametersAsDependentParameters(result, signature, arg);
                        }

                        // Unqualified table calls are additionally problematic, since they may refer to local table variables too
                        // and we are not doing analysis of all local variable source expressions and how they enter the enclosing function.
                        if (isUnqualifiedTableCall
                            && isTabular
                            && fc.ArgumentList.Expressions.Count > 0
                            && fc.ArgumentList.Expressions[0].Element.ConstantValueInfo?.Value is string tableName)
                        {
                            result = result.AddUnqualifiedTableName(tableName);
                        }
                    }
                    else if (node is ProjectByNamesOperator proj)
                    {
                        // expressions of project-by-names operator alter the result schema,
                        // so any dependency of one of these on a function parameter means the result schema
                        // might be dependent on that parameter too.
                        result = AddReferencedParametersAsDependentParameters(result, signature, proj);
                    }
                    else if (
                        node is Expression ex
                        && ex.ReferencedSignature is Signature sig 
                        && !IsSymbolLookupFunction(sig.Symbol))
                    {
                        var facts = GetFunctionBodyFacts(ex);
                        result = result.CombineCalledFunction(facts);

                        // translate dependent parameters from the called function to parameters of the calling function
                        if (facts.DependentParameters.Count > 0
                            && ex is FunctionCallExpression fcall
                            && isTabular)
                        {
                            argParams.Clear();
                            GetArgumentParameters(fcall, argParams);

                            for (int i = 0; i < fcall.ArgumentList.Expressions.Count; i++)
                            {
                                // if this argument corresponds to a dependent parameter of the called function
                                // then any parameter referenced in the argument must also be dependent
                                var arg = fcall.ArgumentList.Expressions[i].Element;
                                if (facts.DependentParameters.Contains(argParams[i]))
                                {
                                    // note: these arguments should be constrained to only constant expressions due to requirements of symbol lookup functions.
                                    result = AddReferencedParametersAsDependentParameters(result, signature, arg);
                                }
                            }
                        }
                    }
                },
                fnAfter: node =>
                {
                    if (node.Alternates != null)
                    {
                        foreach (var alt in node.Alternates)
                        {
                            var facts = ComputeFunctionBodyFacts(signature, alt);
                            result = result.CombineCalledFunction(facts);
                        }
                    }
                },
                fnDescend: node => 
                    node == body 
                    || (!(node is FunctionDeclaration) && !(node is FunctionBody))
                );

            s_parameterListPool.ReturnToPool(argParams);

            var nonVariableReturnType = !result.HasVariableReturnType
                ? GetBodyResultType(body) ?? ErrorSymbol.Instance
                : null;

            result = result.WithNonVariableReturnType(nonVariableReturnType);

            var hasSyntaxErrors = HasSyntaxErrors(body);
            result = result.WithHasSyntaxErrors(hasSyntaxErrors);

            return result;
        }

        /// <summary>
        /// Adds all referenced parameters of the specified signature found in the node sub-tree to 
        /// the <see cref="FunctionBodyFacts"/> dependent parameters list.
        /// </summary>
        private static FunctionBodyFacts AddReferencedParametersAsDependentParameters(FunctionBodyFacts facts, Signature signature, SyntaxNode root)
        {
            var referencedParams = s_parameterListPool.AllocateFromPool();
            try
            {
                GetReferencedParameters(signature, root, referencedParams);
                facts = facts.AddDependentParameters(referencedParams);
            }
            finally
            {
                s_parameterListPool.ReturnToPool(referencedParams);
            }

            return facts;
        }


        /// <summary>
        /// Gets all referenced parameters in the expression sub-tree
        /// </summary>
        private static void GetReferencedParameters(Signature signature, SyntaxNode root, List<Parameter> parameters)
        {
            // look for all expressions that refer to a parameter or variable
            SyntaxElement.WalkNodes(
                root,
                node =>
                {
                    if (node is SimpleNamedExpression sne)
                        node = sne.Expression;

                    if (node is Expression expression)
                    {
                        switch (expression.ReferencedSymbol)
                        {
                            case ParameterSymbol p:
                                if (signature.GetParameter(p.Name) is Parameter sparam)
                                {
                                    parameters.Add(sparam);
                                }
                                break;
                            case VariableSymbol v:
                                if (v.Source != null)
                                {
                                    if (v.Source.Tree != expression.Tree)
                                    {
                                        // variable is declared in different tree than its source expression,
                                        // this indicates this is a fake variable being used in place of a parameter symbol
                                        // for expansion binding to carry constant values.
                                        if (signature.GetParameter(v.Name) is Parameter vparam)
                                        {
                                            parameters.Add(vparam);
                                        }
                                    }
                                    else
                                    {
                                        // follow reference w/in same tree 
                                        GetReferencedParameters(signature, v.Source, parameters);
                                    }
                                }
                                break;
                        }
                    }
                });
        }

#if false
        /// <summary>
        /// Gets the parameter referenced by the expression.
        /// Sees through simple let-variable reassignments.
        /// </summary>
        private static Parameter GetReferencedParameter(Signature signature, Expression expression)
        {
            if (expression is SimpleNamedExpression sne)
                expression = sne.Expression;

            switch (expression.ReferencedSymbol)
            {
                case ParameterSymbol p:
                    return signature.GetParameter(p.Name);
                case VariableSymbol v:
                    if (v.Source != null
                        && v.Source.Tree != expression.Tree)
                    {
                        // this is a variable in place of a parameter symbol used for expansion binding to carry constant values.
                        return signature.GetParameter(v.Name);
                    }

                    return v.Source != null 
                        ? GetReferencedParameter(signature, v.Source) 
                        : null;
            }

            return null;
        }
#endif

        private static bool IsSymbolLookupFunction(Symbol symbol) =>
            symbol == Functions.Table
            || symbol == Functions.ExternalTable
            || symbol == Functions.MaterializedView
            || symbol == Functions.EntityGroup
            || symbol == Functions.StoredQueryResult
            || symbol == Functions.Database
            || symbol == Functions.Cluster
            || symbol == Functions.Graph;

        /// <summary>
        /// Gets the <see cref="FunctionBodyFacts"/> for the function invocation
        /// </summary>
        private FunctionBodyFacts GetFunctionBodyFacts(Expression expr)
        {
            if (expr.ReferencedSignature is Signature signature)
            {
                if (!TryGetFunctionBodyFacts(signature, out var funFacts)
                    && signature.ReturnKind == ReturnTypeKind.Computed)
                {
                    if (expr is FunctionCallExpression functionCall)
                    {
                        var arguments = s_expressionListPool.AllocateFromPool();
                        var argumentTypes = s_typeListPool.AllocateFromPool();

                        try
                        {
                            GetArgumentsAndTypes(functionCall, arguments, argumentTypes);
                            GetComputedFunctionCallResult(signature, arguments, argumentTypes);
                        }
                        finally
                        {
                            s_expressionListPool.ReturnToPool(arguments);
                            s_typeListPool.ReturnToPool(argumentTypes);
                        }
                    }
                    else if (expr is NameReference)
                    {
                        GetComputedFunctionCallResult(signature, EmptyReadOnlyList<Expression>.Instance, EmptyReadOnlyList<TypeSymbol>.Instance);
                    }

                    // try again
                    TryGetFunctionBodyFacts(signature, out funFacts);
                }

                return funFacts ?? FunctionBodyFacts.Default;
            }

            return FunctionBodyFacts.Default;
        }
    }
}