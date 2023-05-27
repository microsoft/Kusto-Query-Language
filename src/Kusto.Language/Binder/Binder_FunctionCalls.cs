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
            else if (IsFuzzyUnionOperand(functionCall))
            {
                return new SemanticInfo(
                    new TableSymbol().WithIsOpen(true),
                    DiagnosticFacts.GetFuzzyUnionOperandNotDefined(functionCall.Name.SimpleName).WithLocation(functionCall));
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

                    return new SemanticInfo(matchingSignatures[0], resultType, diagnostics, isConstant: fn.IsConstantFoldable && AllAreConstant(arguments), calledFunctionInfo: funResult.Info);
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
                argumentTypes.Insert(0, _implicitArgumentType);
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
            if (TryGetLiteralValue(expression, out var objValue))
            {
                value = objValue as string;
                return value != null;
            }
            else
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the value of the literal if the expression is a literal or refers to literal
        /// </summary>
        internal static bool TryGetLiteralValue(Expression expression, out object value)
        {
            expression = GetUnderlyingExpression(expression);

            if (expression.IsLiteral)
            {
                value = expression.LiteralValue;
                return value != null;
            }
            else if (expression is NameReference nr && nr.ReferencedSymbol is VariableSymbol vs && vs.IsConstant)
            {
                value = vs.ConstantValue;
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
                    diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownCluster(name).WithLocation(location));
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
                    diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownDatabase(nameOrPattern).WithLocation(location));
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
            if (clusterOrGroup == _currentCluster
                && !string.IsNullOrEmpty(nameOrPattern)
                && (string.Compare(_currentDatabase.Name, nameOrPattern, ignoreCase: true) == 0
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
                var match = SymbolMatch.Table | SymbolMatch.Local;

                var symbols = s_symbolListPool.AllocateFromPool();
                try
                {
                    // check scope for variables, etc
                    _localScope.GetSymbols(nameOrPattern, match, symbols);

                    if (symbols.Count > 0)
                    {
                        var result = GetResultType(symbols[0]);
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
                    diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownTable(nameOrPattern).WithLocation(location));
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
                    diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownExternalTable(name).WithLocation(location));
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
                    diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownMaterializedView(name).WithLocation(location));
                }

                table = new TableSymbol(name).WithIsMaterializedView(true).WithIsOpen(true);
            }

            return table;
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
            return GetParameterMatchKind(signature, argumentParameters, argumentTypes, parameter, argument, argumentType, AllowLooseParameterMatching(signature));
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
            bool allowLooseParameterMatching)
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
                    else if (allowLooseParameterMatching
                        && SymbolsAssignable(parameter.DeclaredTypes, argumentType, Conversion.Dynamic))
                    {
                        return ParameterMatchKind.Dynamic;
                    }
                    else if (allowLooseParameterMatching
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
                    return GetParameterMatchKind(signature, argumentParameters, argumentTypes, argumentParameters[0], argument, argumentType, allowLooseParameterMatching);

                case ParameterTypeKind.Parameter1:
                    return GetParameterMatchKind(signature, argumentParameters, argumentTypes, argumentParameters[1], argument, argumentType, allowLooseParameterMatching);

                case ParameterTypeKind.Parameter2:
                    return GetParameterMatchKind(signature, argumentParameters, argumentTypes, argumentParameters[2], argument, argumentType, allowLooseParameterMatching);

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
                        else if (allowLooseParameterMatching
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
            var outerScope = _localScope.Copy();

            TryGetFunctionBodyFacts(signature, out var funFacts);

            // if the function is not yet analyzed or is known to have a variable return type
            // then compute the function body facts and return type for this location by calling GetCallSiteExpansion.
            if (funFacts == null || funFacts.HasVariableReturnType)
            {
                // use expansion at this call site to determine correct return type
                // if signature facts was not yet known, it will be computed by calling GetCallSiteExpansion
                var expansion = this.GetFunctionCallExpansion(signature, arguments, argumentTypes, outerScope);
                
                // try again after evaluating expansion
                TryGetFunctionBodyFacts(signature, out funFacts);

                var returnType = GetBodyResultType(expansion?.Root);

                var hasErrors = funFacts != null ? funFacts.HasErrors
                    : returnType != null && returnType.IsError ? true
                    : HasErrors(expansion?.Root);

                if (returnType == null || returnType.IsError)
                    returnType = ScalarTypes.Unknown;

                return new FunctionCallResult(returnType, new FunctionCallInfo(expansion, funFacts, hasErrors));
            }
            else
            {
                // body has non-variable (fixed) return type.
                return new FunctionCallResult(
                    funFacts.NonVariableComputedReturnType,
                    new FunctionCallInfo(GetDeferredFunctionCallExpansion(signature, arguments, argumentTypes, outerScope), funFacts, funFacts.HasErrors));
            }
        }

        private static TypeSymbol GetBodyResultType(SyntaxNode body)
        {
            return 
                body is Expression exprBody ? exprBody.ResultType
                : body is FunctionBody functionBody ? functionBody.Expression?.ResultType
                : null;
        }

        private static bool HasErrors(SyntaxNode syntax)
        {
            return syntax != null
                && ((syntax.ContainsSyntaxDiagnostics
                     && syntax.GetContainedSyntaxDiagnostics().Any(d => d.Severity == DiagnosticSeverity.Error))
                    || (syntax.GetContainedDiagnostics().Any(d => d.Severity == DiagnosticSeverity.Error)));
        }

        private Func<FunctionCallExpansion> GetDeferredFunctionCallExpansion(Signature signature, IReadOnlyList<Expression> arguments = null, IReadOnlyList<TypeSymbol> argumentTypes = null, LocalScope outerScope = null)
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
                        expansion = this.GetFunctionCallExpansion(signature, args, types, outerScope);
                    }
                }

                return expansion;
            };
        }

        /// <summary>
        /// Gets the inline expansion of a function call
        /// </summary>
        internal FunctionCallExpansion GetFunctionCallExpansion(Signature signature, IReadOnlyList<Expression> arguments = null, IReadOnlyList<TypeSymbol> argumentTypes = null, LocalScope outerScope = null)
        {
            if (signature.ReturnKind != ReturnTypeKind.Computed)
                return null;

            // block cycles in computation
            if (_localBindingCache.SignaturesComputingExpansion.Contains(signature))
                return null;

            _localBindingCache.SignaturesComputingExpansion.Add(signature);
            try
            {
                var callSiteInfo = GetCallSiteInfo(signature, arguments, argumentTypes);

                if (!TryGetExpansionFromCache(callSiteInfo, out var expansion))
                {
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

                            if (TryBindCalledFunctionBody(expansion, this, currentCluster, currentDatabase, signature.Symbol as FunctionSymbol, outerScope, callSiteInfo.Locals))
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

                    AddExpansionToCache(callSiteInfo, expansion);
                }

                return expansion;
            }
            finally
            {
                _localBindingCache.SignaturesComputingExpansion.Remove(signature);
            }

            // Tries to get the expansion from global or local cache.
            bool TryGetExpansionFromCache(CallSiteInfo callsite, out FunctionCallExpansion expansion)
            {
                return _localBindingCache.CallSiteToExpansionMap.TryGetValue(callsite, out expansion)
                    || _globalBindingCache.CallSiteToExpansionMap.TryGetValue(callsite, out expansion);
            }

            // Adds expansion to global or local cache.
            void AddExpansionToCache(CallSiteInfo callsite, FunctionCallExpansion expansion)
            {
                TryGetFunctionBodyFacts(callsite.Signature, out var funFacts);

                // if there is a call to unqualified table(t) then it may require resolving using dynamic scope, so don't cache anywhere
                if (funFacts != null && funFacts.HasUnqualifiedTableCall)
                    return;

                // only add database functions that are not variable in nature to global cache
                // might need to rethink this if memory consumption is shown to be an issue
                var shouldCacheGlobally = IsDatabaseSymbolSignature(callsite.Signature)
                    && (funFacts != null && !funFacts.HasVariableReturnType);

                if (shouldCacheGlobally)
                {
                    _globalBindingCache.CallSiteToExpansionMap.Add(callsite, expansion);
                }
                else
                {
                    _localBindingCache.CallSiteToExpansionMap.Add(callsite, expansion);
                }
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

        private CallSiteInfo GetCallSiteInfo(Signature signature, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            var locals = GetArgumentsAsLocals(signature, arguments, argumentTypes);
            return new CallSiteInfo(signature, locals);
        }

        private IReadOnlyList<VariableSymbol> GetArgumentsAsLocals(Signature signature, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes)
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

                        var isLiteral = Binding.Binder.TryGetLiteralValue(arg, out var literalValue);
                        locals.Add(new VariableSymbol(p.Name, localType, isLiteral, literalValue, source: arg));
                    }
                    else
                    {
                        var type = GetRepresentativeType(p);

                        var isConstant = p.IsOptional && p.DefaultValue != null;
                        object constantValue = null;
                        if (isConstant)
                        {
                            TryGetLiteralValue(p.DefaultValue, out constantValue);
                        }

                        locals.Add(new VariableSymbol(p.Name, type, isConstant, constantValue));
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
                var bodyFacts = ComputeFunctionBodyFlags(signature, body);

                var nonVariableReturnType = (bodyFacts & FunctionBodyFlags.VariableReturn) == 0
                    ? GetBodyResultType(body) ?? ErrorSymbol.Instance
                    : null;

                var hasErrors = HasErrors(body);

                facts = new FunctionBodyFacts(bodyFacts, nonVariableReturnType, hasErrors);
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
                _globalBindingCache.DatabaseFunctionBodyFacts[signature] = facts;
            }
            else
            {
                _localBindingCache.NonDatabaseFunctionBodyFacts[signature] = facts;
            }
        }

        private static IEnumerable<TElement> GetMainBodyOnlyDescendants<TElement>(SyntaxNode body, Func<TElement, bool> predicate)
            where TElement : SyntaxElement
        {
            List<TElement> list = null;

            SyntaxElement.WalkElements(body,
                fnAfter: element =>
                {
                    if (element is TElement te && predicate(te))
                    {
                        if (list == null)
                            list = new List<TElement>();
                        list.Add(te);
                    }
                },

                // do not consider nested function declaration elements
                fnDescend:
                    element => element == body
                            || (!(element is FunctionDeclaration) && !(element is FunctionBody))
                );

            return list ?? EmptyReadOnlyList<TElement>.Instance;
        }

        private FunctionBodyFlags ComputeFunctionBodyFlags(Signature signature, SyntaxNode body)
        {
            var result = FunctionBodyFlags.None;
            var isTabular = GetBodyResultType(body) is TableSymbol;

            // look for explicit calls to table(), database() or cluster() like functions
            foreach (var fc in GetMainBodyOnlyDescendants<FunctionCallExpression>(body,
                _fc => IsSymbolLookupFunction(_fc.ReferencedSymbol)))
            {
                if (fc.ReferencedSymbol == Functions.Table)
                {
                    // distinguish between database(d).table(t) vs just table(t)
                    // since table(t) can see variables in dynamic scope
                    if (fc.Parent is PathExpression p && p.Selector == fc)
                    {
                        result |= FunctionBodyFlags.QualifiedTable;
                    }
                    else
                    {
                        // unqualified table calls (even with literal arguments) can be dependent on the call site since
                        // the names can reference local tabular variables in outer scopes
                        result |= FunctionBodyFlags.UnqualifiedTable | FunctionBodyFlags.VariableReturn;
                    }
                }
                else if (fc.ReferencedSymbol == Functions.ExternalTable)
                {
                    result |= FunctionBodyFlags.ExternalTable;
                }
                else if (fc.ReferencedSymbol == Functions.MaterializedView)
                {
                    result |= FunctionBodyFlags.MaterializedView;
                }
                else if (fc.ReferencedSymbol == Functions.Database)
                {
                    result |= FunctionBodyFlags.Database;
                }
                else if (fc.ReferencedSymbol == Functions.Cluster)
                {
                    result |= FunctionBodyFlags.Cluster;
                }

                // if the argument is not a literal, then the function likely has a variable return schema
                // note: it might not, but that would require full flow analysis of result type back to inputs.
                var isLiteral = fc.ArgumentList.Expressions.Count > 0 && fc.ArgumentList.Expressions[0].Element.IsLiteral;
                if (!isLiteral && isTabular)
                {
                    result |= FunctionBodyFlags.VariableReturn;
                }
            }

            // the function returns a table and at least one parameter is a tabular
            if (isTabular && signature.Parameters.Any(p => p.IsTabular))
            {
                result |= FunctionBodyFlags.VariableReturn;
            }

            // also consider any facts from other calls to user functions
            foreach (var fce in GetMainBodyOnlyDescendants<Expression>(body, ex =>
                ex.ReferencedSymbol is FunctionSymbol fs
                && !IsSymbolLookupFunction(fs)
                && (ex is FunctionCallExpression
                    || (ex is NameReference && !(ex.Parent is FunctionCallExpression)))))
            {
                var flags = GetFunctionBodyFlags(fce);

                // if the calling function has no parameters and the called function does not contain unqualified calls to table() function, then don't considered it having a variable return
                if (signature.Parameters.Count == 0 && (flags & FunctionBodyFlags.UnqualifiedTable) == 0)
                {
                    flags &= ~FunctionBodyFlags.VariableReturn;
                }

                result |= flags;
            }

            return result;
        }

        private static bool IsSymbolLookupFunction(Symbol symbol) =>
            symbol == Functions.Table
            || symbol == Functions.ExternalTable
            || symbol == Functions.MaterializedView
            || symbol == Functions.Database
            || symbol == Functions.Cluster;

        /// <summary>
        /// Gets the <see cref="FunctionBodyFlags"/> for the function invocation
        /// </summary>
        private FunctionBodyFlags GetFunctionBodyFlags(Expression expr)
        {
            if (expr.ReferencedSymbol is FunctionSymbol fs)
            {
                var signature = fs.Signatures[0];

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

                return funFacts?.Flags ?? FunctionBodyFlags.None;
            }

            return FunctionBodyFlags.None;
        }
    }
}