using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Binding
{
    using Symbols;
    using Syntax;
    using Utils;

    internal partial class Binder
    {
        /// <summary>
        /// The <see cref="NodeBinder"/> is a <see cref="SyntaxVisitor"/> that computes
        /// the <see cref="SemanticInfo"/> foreach each kind of <see cref="SyntaxNode"/>.
        /// </summary>
        private class NodeBinder : SyntaxVisitor<SemanticInfo>
        {
            private readonly Binder _binder;

            public NodeBinder(Binder binder)
            {
                _binder = binder;
            }

            public TableSymbol RowScopeOrEmpty => _binder.RowScopeOrEmpty;
            public TableSymbol RightRowScopeOrEmpty => _binder.RightRowScopeOrEmpty;

            #region declarations
            public override SemanticInfo VisitNameAndTypeDeclaration(NameAndTypeDeclaration node)
            {
                // this declaration does not have a type on its own
                return VoidInfo;
            }

            public override SemanticInfo VisitFunctionDeclaration(FunctionDeclaration node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    var parameters = new List<Parameter>();

                    // first series of parameters can be tabular
                    var canBeTabular = true;

                    // get parameter symbols already defined
                    for (int i = 0; i < node.Parameters.Parameters.Count; i++)
                    {
                        var fp = node.Parameters.Parameters[i].Element;

                        bool isOptional = fp.DefaultValue != null;

                        if (fp.NameAndType.Name.ReferencedSymbol is ParameterSymbol p)
                        {
                            if (p.IsTabular && !canBeTabular)
                            {
                                diagnostics.Add(DiagnosticFacts.GetTabularParametersMustBeDeclaredFirst().WithLocation(fp));
                            }
                            canBeTabular = p.IsTabular;

                            parameters.Add(Parameter.From(p, isOptional, fp.DefaultValue?.Value));
                        }
                    }

                    var name = GetNameFromContext(node);
                    var fs = new FunctionSymbol(name, node.Body, parameters).WithIsView(node.ViewKeyword != null);

                    return new SemanticInfo(fs, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitFunctionParameter(FunctionParameter node)
            {
                if (node.DefaultValue != null)
                {
                    var diagnostics = s_diagnosticListPool.AllocateFromPool();
                    try
                    {
                        var type = _binder.GetTypeFromTypeExpression(node.NameAndType.Type);
                        if (type != null && !type.IsError)
                        {
                            if (_binder.CheckIsLiteralNotToken(node.DefaultValue.Value, diagnostics))
                            {
                                _binder.CheckIsType(node.DefaultValue.Value, type, Conversion.Compatible, diagnostics);
                            }
                        }

                        if (diagnostics.Count > 0)
                        {
                            return new SemanticInfo(diagnostics);
                        }
                    }
                    finally
                    {
                        s_diagnosticListPool.ReturnToPool(diagnostics);
                    }
                }

                return null;
            }

            public override SemanticInfo VisitDefaultValueDeclaration(DefaultValueDeclaration node)
            {
                return null;
            }

            public override SemanticInfo VisitFunctionBody(FunctionBody node)
            {
                return null;
            }

            public override SemanticInfo VisitFunctionParameters(FunctionParameters node)
            {
                return null;
            }

            /// <summary>
            /// Get the name the expression will be assigned by a let statement
            /// it is part of, or empty string.
            /// </summary>
            private static string GetNameFromContext(Expression expr)
            {
                return (expr.Parent is LetStatement ls) ? ls.Name.SimpleName : "";
            }

            public override SemanticInfo VisitPatternStatement(PatternStatement node)
            {
                return null;
            }

            public override SemanticInfo VisitPatternDeclaration(PatternDeclaration node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    var patternsigs = new List<PatternSignature>();

                    var parameters = new List<Parameter>();
                    for (int i = 0, n = node.Parameters.Count; i < n; i++)
                    {
                        var parameter = node.Parameters[0].Element;
                        var name = parameter.Name.SimpleName;
                        var type = _binder.GetTypeFromTypeExpression(parameter.Type, diagnostics);
                        parameters.Add(new Parameter(name, type));
                    }

                    Parameter pathParameter = null;
                    if (node.PathParameter != null)
                    {
                        var name = node.PathParameter.Parameter.Name.SimpleName;
                        var type = _binder.GetTypeFromTypeExpression(node.PathParameter.Parameter.Type);
                        pathParameter = new Parameter(name, type);
                    }

                    for (int i = 0, n = node.Patterns.Count; i < n; i++)
                    {
                        var pattern = node.Patterns[i];

                        // check match values
                        if (pattern.ParameterValues.Expressions.Count != node.Parameters.Count)
                        {
                            diagnostics.Add(DiagnosticFacts.GetValueCountMustEqualParameterCount().WithLocation(pattern.ParameterValues));
                        }

                        // check all match values are literals of the correct type
                        var values = new List<string>();

                        for (int v = 0, vn = pattern.ParameterValues.Expressions.Count; v < vn; v++)
                        {
                            var parameter = node.Parameters[v].Element;
                            var value = pattern.ParameterValues.Expressions[v].Element;

                            // all values must be string literals
                            if (_binder.CheckIsExactType(value, ScalarTypes.String, diagnostics))
                            {
                                _binder.CheckIsLiteral(value, diagnostics);
                            }

                            values.Add(value.LiteralValue?.ToString() ?? "");
                        }

                        // check path value
                        string pathValue = null;

                        if (pattern.PathValue == null && node.PathParameter != null)
                        {
                            diagnostics.Add(DiagnosticFacts.GetPathValueExpected().WithLocation(pattern.EqualToken));
                        }
                        else if (pattern.PathValue != null && node.PathParameter == null)
                        {
                            diagnostics.Add(DiagnosticFacts.GetPathValueWithNoPathParameter().WithLocation(pattern.PathValue));
                        }
                        else if (pattern.PathValue != null && node.PathParameter != null)
                        {
                            // path value must be string literal
                            if (_binder.CheckIsExactType(pattern.PathValue.Value, ScalarTypes.String, diagnostics))
                            {
                                _binder.CheckIsLiteral(pattern.PathValue.Value, diagnostics);
                            }

                            pathValue = pattern.PathValue.Value.LiteralValue?.ToString() ?? "";
                        }

                        patternsigs.Add(new PatternSignature(values, pathValue, pattern.Body));
                    }

                    var patternName = (node.Parent as PatternStatement)?.Name.SimpleName;
                    var resultType = new PatternSymbol(patternName, parameters, pathParameter, patternsigs);

                    return new SemanticInfo(resultType, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitMaterializeExpression(MaterializeExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    _binder.CheckIsTabularOrGraph(node.Expression, diagnostics);
                    return new SemanticInfo(GetResultTypeOrError(node.Expression), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitNameDeclaration(NameDeclaration node)
            {
                return null;
            }

            #endregion

            #region literals
            public override SemanticInfo VisitLiteralExpression(LiteralExpression node)
            {
                switch (node.Kind)
                {
                    case SyntaxKind.BooleanLiteralExpression:
                        return LiteralBoolInfo;
                    case SyntaxKind.IntLiteralExpression:
                        return LiteralIntInfo;
                    case SyntaxKind.LongLiteralExpression:
                        return LiteralLongInfo;
                    case SyntaxKind.RealLiteralExpression:
                        return LiteralRealInfo;
                    case SyntaxKind.DecimalLiteralExpression:
                        if (_binder._dynamicDepth != 0)
                        {
                            return new SemanticInfo(ScalarTypes.Decimal, DiagnosticFacts.GetDecimalInDynamic().WithLocation(node));
                        }
                        return LiteralDecimalInfo;
                    case SyntaxKind.StringLiteralExpression:
                        return LiteralStringInfo;
                    case SyntaxKind.DateTimeLiteralExpression:
                        return LiteralDateTimeInfo;
                    case SyntaxKind.TimespanLiteralExpression:
                        return LiteralTimeSpanInfo;
                    case SyntaxKind.GuidLiteralExpression:
                        return LiteralGuidInfo;
                    case SyntaxKind.TokenLiteralExpression:
                        return VoidInfo;
                    case SyntaxKind.NullLiteralExpression:
                        return LiteralNullInfo;
                    default:
                        throw new InvalidOperationException($"Unknown literal kind: {node.Kind}");
                }
            }

            public override SemanticInfo VisitTypeOfLiteralExpression(TypeOfLiteralExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    TypeSymbol type;

                    if (node.Types.Count == 1 && node.Types[0].Element is PrimitiveTypeExpression pt)
                    {
                        type = Binder.GetType(pt, diagnostics);
                    }
                    else
                    {
                        var columns = s_columnListPool.AllocateFromPool();
                        try
                        {
                            for (int i = 0; i < node.Types.Count; i++)
                            {
                                var element = node.Types[i].Element;
                                switch (element)
                                {
                                    case StarExpression s:
                                        if (_binder._rowScope != null)
                                        {
                                            // include all columns in scope
                                            columns.AddRange(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty));
                                        }
                                        else
                                        {
                                            diagnostics.Add(DiagnosticFacts.GetNoColumnsInScope().WithLocation(s));
                                        }
                                        break;

                                    case NameAndTypeDeclaration nat:
                                        var declaredType = _binder.GetTypeFromTypeExpression(nat.Type, diagnostics);
                                        var newColumn = new ColumnSymbol(nat.Name.SimpleName, declaredType);
                                        columns.Add(newColumn);
                                        break;

                                    default:
                                        diagnostics.Add(DiagnosticFacts.GetInvalidColumnDeclaration().WithLocation(element));
                                        break;
                                }
                            }

                            type = new TableSymbol(columns);
                        }
                        finally
                        {
                            s_columnListPool.ReturnToPool(columns);
                        }
                    }

                    return new SemanticInfo(type, ScalarTypes.Type, diagnostics, isConstant: true);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitDynamicExpression(DynamicExpression node)
            {
                var info = node.Expression.GetSemanticInfo();
                return new SemanticInfo(ScalarTypes.GetDynamic(info.ResultType), isConstant: true);
            }

            public override SemanticInfo VisitCompoundStringLiteralExpression(CompoundStringLiteralExpression node)
            {
                return LiteralStringInfo;
            }
            #endregion

            #region scalar operators
            public override SemanticInfo VisitBinaryExpression(BinaryExpression node)
            {
                var opKind = GetOperatorKind(node.Kind);
                if (opKind != OperatorKind.None)
                {
                    return _binder.GetBinaryOperatorInfo(opKind, node.Left, node.Right, node.Operator);
                }
                else
                {
                    throw new InvalidOperationException($"Unknown binary operator kind: {node.Kind}");
                }
            }

            public override SemanticInfo VisitPrefixUnaryExpression(PrefixUnaryExpression node)
            {
                var opKind = GetOperatorKind(node.Kind);
                if (opKind != OperatorKind.None)
                {
                    return _binder.GetUnaryOperatorInfo(opKind, node.Expression, node.Operator);
                }
                else
                {
                    throw new InvalidOperationException($"Unknown unary operator kind: {node.Kind}");
                }
            }

            public override SemanticInfo VisitInExpression(InExpression node)
            {
                var dx = s_diagnosticListPool.AllocateFromPool();
                var args = s_expressionListPool.AllocateFromPool();
                try
                {
                    args.Add(node.Left);

                    for (int i = 0; i < node.Right.Expressions.Count; i++)
                    {
                        args.Add(node.Right.Expressions[i].Element);
                    }

                    var op = SyntaxFacts.GetOperatorKind(node.Operator.Kind);
                    return _binder.GetOperatorInfo(op, args, node.Operator);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(dx);
                    s_expressionListPool.ReturnToPool(args);
                }
            }

            public override SemanticInfo VisitHasAnyExpression(HasAnyExpression node)
            {
                var dx = s_diagnosticListPool.AllocateFromPool();
                var args = s_expressionListPool.AllocateFromPool();
                try
                {
                    args.Add(node.Left);

                    for (int i = 0; i < node.Right.Expressions.Count; i++)
                    {
                        args.Add(node.Right.Expressions[i].Element);
                    }

                    var op = SyntaxFacts.GetOperatorKind(node.Operator.Kind);
                    return _binder.GetOperatorInfo(op, args, node.Operator);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(dx);
                    s_expressionListPool.ReturnToPool(args);
                }
            }

            public override SemanticInfo VisitHasAllExpression(HasAllExpression node)
            {
                var dx = s_diagnosticListPool.AllocateFromPool();
                var args = s_expressionListPool.AllocateFromPool();
                try
                {
                    args.Add(node.Left);

                    for (int i = 0; i < node.Right.Expressions.Count; i++)
                    {
                        args.Add(node.Right.Expressions[i].Element);
                    }

                    var op = SyntaxFacts.GetOperatorKind(node.Operator.Kind);
                    return _binder.GetOperatorInfo(op, args, node.Operator);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(dx);
                    s_expressionListPool.ReturnToPool(args);
                }
            }

            public override SemanticInfo VisitBetweenExpression(BetweenExpression node)
            {
                var dx = s_diagnosticListPool.AllocateFromPool();
                var args = s_expressionListPool.AllocateFromPool();
                try
                {
                    args.Add(node.Left);
                    args.Add(node.Right.First);
                    args.Add(node.Right.Second);

                    var op = SyntaxFacts.GetOperatorKind(node.Operator.Kind);
                    return _binder.GetOperatorInfo(op, args, node.Operator);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(dx);
                    s_expressionListPool.ReturnToPool(args);
                }
            }

            public override SemanticInfo VisitToScalarExpression(ToScalarExpression node)
            {
                var dx = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    if (node.KindParameter != null)
                    {
                        _binder.CheckQueryOperatorParameter(node.KindParameter, QueryOperatorParameters.ToScalarKindParameter, dx);
                    }

                    var resultType = GetResultType(node.Expression);
                    if (resultType is TableSymbol table)
                    {
                        if (table.Columns.Count > 0)
                        {
                            var col = table.Columns[0];
                            return new SemanticInfo(col.Type, dx, isConstant: true);
                        }
                        else
                        {
                            dx.Add(DiagnosticFacts.GetTableHasNoColumns().WithLocation(node.Expression));
                            return new SemanticInfo(ErrorSymbol.Instance, dx, isConstant: true);
                        }
                    }
                    else if (resultType is ScalarSymbol)
                    {
                        return new SemanticInfo(resultType, dx, isConstant: true);
                    }
                    else
                    {
                        dx.Add(DiagnosticFacts.GetTableOrScalarExpected().WithLocation(node.Expression));
                        return new SemanticInfo(ErrorSymbol.Instance, dx, isConstant: true);
                    }
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(dx);
                }
            }

            public override SemanticInfo VisitToTableExpression(ToTableExpression node)
            {
                var dx = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    if (node.KindParameter != null)
                    {
                        _binder.CheckQueryOperatorParameter(node.KindParameter, QueryOperatorParameters.ToTableKindParameter, dx);
                    }

                    if (_binder.CheckIsTabular(node.Expression, dx))
                    {
                        var table = (TableSymbol)GetResultType(node.Expression);
                        return new SemanticInfo(table, dx);
                    }

                    return new SemanticInfo(ErrorSymbol.Instance, dx);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(dx);
                }
            }

            #endregion

            #region names and paths
            public override SemanticInfo VisitTokenName(TokenName node)
            {
                // handled by parent node
                return null;
            }

            public override SemanticInfo VisitBracketedName(BracketedName node)
            {
                // handled by parent node
                return null;
            }

            public override SemanticInfo VisitBracedName(BracedName node)
            {
                // handled by parent node
                return null;
            }

            public override SemanticInfo VisitWildcardedName(WildcardedName node)
            {
                // handled by parent node
                return null;
            }

            public override SemanticInfo VisitBracketedWildcardedName(BracketedWildcardedName node)
            {
                // handled by parent node
                return null;
            }

            public override SemanticInfo VisitNameReference(NameReference node)
            {
                switch (node.Name)
                {
                    case TokenName _:
                        return _binder.BindName(node.SimpleName, node.Match, node);
                    case BracketedName _:
                        return _binder.BindName(node.SimpleName, node.Match, node);
                    case WildcardedName wc:
                        return VisitWildcardedNameReference(node, wc.SimpleName);
                    case BracketedWildcardedName bwc:
                        return VisitWildcardedNameReference(node, bwc.SimpleName);
                    case BracedName _:
                        return VisitClientParameterReference(node);
                    default:
                        throw new NotImplementedException();
                }
            }

            private SemanticInfo VisitClientParameterReference(NameReference node)
            {
                if (_binder._globals.GetProperty(Properties.AllowClientParameters))
                {
                    // check for supplied symbol for client parameter
                    if (_binder._globals.GetClientSymbol(node.SimpleName) is Symbol symbol)
                    {
                        return GetSemanticInfo(symbol, null);
                    }
                    else
                    {
                        // client parameter does not have a known type
                        if (IsInTabularContext(node))
                        {
                            // but it is probably tabular
                            var table = new TableSymbol(node.SimpleName).WithIsOpen(true);
                            return new SemanticInfo(table);
                        }
                        else
                        {
                            // otherwise its an unknown scalar
                            return new SemanticInfo(ScalarTypes.Unknown);
                        }
                    }
                }
                else
                {
                    // error if client parameters not supported
                    return new SemanticInfo(ScalarTypes.Unknown, DiagnosticFacts.GetClientParametersNotSupported().WithLocation(node));
                }
            }

            private SemanticInfo VisitWildcardedNameReference(NameReference node, string pattern)
            {
                var list = s_symbolListPool.AllocateFromPool();
                var filteredList = s_symbolListPool.AllocateFromPool();
                var matchingList = s_symbolListPool.AllocateFromPool();

                try
                {
                    var match = IsInTabularContext(node)
                        ? SymbolMatch.Table | SymbolMatch.Function | SymbolMatch.View | SymbolMatch.Local | SymbolMatch.Tabular
                        : SymbolMatch.Column | SymbolMatch.Function | SymbolMatch.View | SymbolMatch.Local | SymbolMatch.Scalar;

                    _binder.GetSymbolsInContext(node, match, IncludeFunctionKind.LocalViews | IncludeFunctionKind.DatabaseFunctions, list);

                    FilterVisibleSymbols(node, list, filteredList);

                    GetWildcardSymbols(pattern, filteredList, matchingList);

                    if (matchingList.Count == 1)
                    {
                        return CreateSemanticInfo(matchingList[0]);
                    }
                    else if (matchingList.Count > 1)
                    {
                        return CreateSemanticInfo(new GroupSymbol(matchingList));
                    }
                    else if (_binder._rowScope != null && _binder._rowScope.IsOpen)
                    {
                        // this might match zero or more columns from the row scope if it is open
                        return CreateSemanticInfo(new GroupSymbol());
                    }
                    else
                    {
                        return new SemanticInfo(ErrorSymbol.Instance, DiagnosticFacts.GetNameDoesNotReferToAnyKnownItem(pattern).WithLocation(node));
                    }
                }
                finally
                {
                    s_symbolListPool.ReturnToPool(list);
                    s_symbolListPool.ReturnToPool(filteredList);
                    s_symbolListPool.ReturnToPool(matchingList);
                }
            }

            private static readonly ObjectPool<Dictionary<string, Symbol>> s_symbolMapPool =
                new ObjectPool<Dictionary<string, Symbol>>(() => new Dictionary<string, Symbol>(), d => d.Clear());

            private void FilterVisibleSymbols(SyntaxNode location, IReadOnlyList<Symbol> symbols, List<Symbol> filteredSymbols)
            {
                bool IsInsideCurrentDatabaseFunction = _binder.IsInsideCurrentDatabaseFunctionBody(location);

                var map = s_symbolMapPool.AllocateFromPool();
                try
                {
                    foreach (var symbol in symbols)
                    {
                        // pick between for tables and functions with same name
                        if (map.TryGetValue(symbol.Name, out var existingSymbol))
                        {
                            if (symbol is TableSymbol tab
                                && existingSymbol is FunctionSymbol fs
                                && fs.MinArgumentCount == 0
                                && _binder._currentDatabase.GetAnyTable(symbol.Name) != null
                                && IsInsideCurrentDatabaseFunction)
                            {
                                // if inside database function declaration choose the table over the function
                                map[symbol.Name] = tab;
                            }
                            else if (symbol is FunctionSymbol fs2
                                && fs2.MinArgumentCount == 0
                                && existingSymbol is TableSymbol tab2
                                && _binder._currentDatabase.GetAnyTable(symbol.Name) != null
                                && !IsInsideCurrentDatabaseFunction)
                            {
                                // otherwise choose the function over the table
                                map[symbol.Name] = fs2;
                            }
                        }
                        else if (symbol is FunctionSymbol fs3
                            && fs3.MinArgumentCount > 0)
                        {
                            // do not add functions that require arguments
                        }
                        else
                        {
                            map.Add(symbol.Name, symbol);
                        }
                    }

                    filteredSymbols.AddRange(map.Values);
                }
                finally
                {
                    s_symbolMapPool.ReturnToPool(map);
                }
            }

            public override SemanticInfo VisitBracketedExpression(BracketedExpression node)
            {
                if (node.Parent is ElementExpression ee && ee.Selector == node)
                {
                    // element selector: container[indexer]
                    return GetElementExpressionInfo(ee.Expression, node);
                }
                else if (node.Parent is PathExpression pe && pe.Selector == node)
                {
                    // path selector: container.[indexer] 
                    // treat same as element selector
                    return GetElementExpressionInfo(pe.Expression, node);
                }
                else
                {
                    // unqualified bracketed expression -- this might happen in a partially typed case
                    // or an incorrectly typed case:  [foo]  when they meant to type ['foo']
                    var indexerType = GetResultTypeOrError(node.Expression);

                    if (indexerType.IsError)
                    {
                        return ErrorInfo;
                    }

                    if (indexerType != ScalarTypes.String)
                    {
                        return new SemanticInfo(ErrorSymbol.Instance, DiagnosticFacts.GetExpressionMustHaveType(ScalarTypes.String).WithLocation(node.Expression));
                    }
                    else if (!node.Expression.IsLiteral)
                    {
                        // computed name lookup?? Is this valid here?
                        return new SemanticInfo(ErrorSymbol.Instance, DiagnosticFacts.GetExpressionMustBeLiteral().WithLocation(node.Expression));
                    }
                    else
                    {
                        // we should never reach here, but if we do then treat it like a valid name reference
                        return _binder.BindName((string)node.Expression.LiteralValue, SymbolMatch.Default, node.Expression);
                    }
                }
            }

            private SemanticInfo GetElementExpressionInfo(Expression collection, BracketedExpression selector)
            {
                var indexerType = GetResultTypeOrError(selector.Expression);
                if (indexerType.IsError)
                {
                    return ErrorInfo;
                }

                var collectionType = collection.ResultType;
                if (collectionType == null || collectionType.IsError)
                {
                    return ErrorInfo;
                }
                else if (collectionType is DynamicArraySymbol arrayType)
                {
                    var elementType = ScalarTypes.GetDynamic(arrayType.ElementType);

                    if (!TypeFacts.IsIntegerOrDynamic(indexerType))
                    {
                        // must be a integer array index (dynamic okay)
                        return new SemanticInfo(elementType, DiagnosticFacts.GetExpressionMustHaveType(ScalarTypes.Int, ScalarTypes.Long).WithLocation(selector.Expression));
                    }
                    else
                    {
                        // you've successfully accessed an element of the array.
                        return new SemanticInfo(elementType);
                    }
                }
                else if (collectionType is DynamicBagSymbol bagType)
                {
                    if (!TypeFacts.IsStringOrDynamic(indexerType))
                    {
                        // must be a string member name index (dynamic okay)
                        return new SemanticInfo(ScalarTypes.Dynamic, DiagnosticFacts.GetExpressionMustHaveType(ScalarTypes.String).WithLocation(selector.Expression));
                    }
                    else if (selector.Expression.ConstantValue is string name
                            && bagType.TryGetProperty(name, out var prop))
                    {
                        // you've successfully accessed a known property of the dynamic bag
                        // you get a dynamic version of whatever type the property is.
                        return new SemanticInfo(ScalarTypes.GetDynamic(prop.Type));
                    }
                    else
                    {
                        // you've successfully accessed an element of a dynamic value
                        // you get another dynamic value.
                        return new SemanticInfo(ScalarTypes.Dynamic);
                    }
                }
                else if (collectionType == ScalarTypes.Dynamic)
                {
                    if (!TypeFacts.IsStringOrDynamic(indexerType) && !TypeFacts.IsIntegerOrDynamic(indexerType))
                    {
                        // must be a integer array index or a string member name index (dynamic okay)
                        return new SemanticInfo(ScalarTypes.Dynamic, DiagnosticFacts.GetExpressionMustHaveType(ScalarTypes.Int, ScalarTypes.Long, ScalarTypes.String).WithLocation(selector.Expression));
                    }
                    else
                    {
                        // you've successfully accessed an element of a dynamic value: you get another dynamic value.
                        return new SemanticInfo(ScalarTypes.Dynamic);
                    }
                }
                else if (collectionType is TupleSymbol ts)
                {
                    if (TypeFacts.IsInteger(indexerType)
                        && selector.Expression.IsConstant
                        && TryGetIntValue(selector.Expression.ConstantValue, out var index))
                    {
                        if (index >= 0 && index < ts.Columns.Count)
                        {
                            var col = ts.Columns[index];
                            return new SemanticInfo(col, col.Type);
                        }
                    }

                    return new SemanticInfo(ScalarTypes.Unknown);
                }
                else if (collectionType == ScalarTypes.Unknown)
                {
                    // unknown is unknown
                    return new SemanticInfo(ScalarTypes.Unknown);
                }
                else
                {
                    // element access only works for dynamic values
                    return new SemanticInfo(ErrorSymbol.Instance, DiagnosticFacts.GetTheElementAccessOperatorIsNotAllowedInThisContext().WithLocation(selector));
                }
            }

            private static bool TryGetIntValue(object value, out int intValue)
            {
                if (value is int iVal)
                {
                    intValue = iVal;
                    return true;
                }
                else if (value is long longVal)
                {

                    intValue = (int)longVal;
                    return true;
                }
                else
                {
                    intValue = 0;
                    return false;
                }
            }

            public override SemanticInfo VisitPathExpression(PathExpression node)
            {
                // same as selector (without repeating diagnositcs)
                return new SemanticInfo(GetReferencedSymbol(node.Selector), GetResultTypeOrError(node.Selector));
            }

            public override SemanticInfo VisitElementExpression(ElementExpression node)
            {
                // same as selector (without repeating diagnostics)
                return new SemanticInfo(GetReferencedSymbol(node.Selector), GetResultTypeOrError(node.Selector));
            }
            #endregion

            #region function calls
            public override SemanticInfo VisitFunctionCallExpression(FunctionCallExpression node)
            {
                return _binder.BindFunctionCallOrPattern(node);
            }

            #endregion

            #region other nodes
            public override SemanticInfo VisitParenthesizedExpression(ParenthesizedExpression node)
            {
                return new SemanticInfo(GetResultTypeOrError(node.Expression));
            }

            public override SemanticInfo VisitEntityGroup(EntityGroup node)
            {
                var dxs = s_diagnosticListPool.AllocateFromPool();
                var symbols = s_symbolListPool.AllocateFromPool();
                try
                {
                    if (node.Entities.Count > 0)
                    {
                        foreach (var se in node.Entities)
                        {
                            var expectedKind = symbols.Count > 0 ? symbols[0].Kind : (SymbolKind?)null;
                            if (CheckEntityGroupElementKind(expectedKind, se.Element, dxs))
                            {
                                symbols.Add(se.Element.ResultType);
                            }
                        }

                        var name = GetNameFromContext(node);
                        var type = new EntityGroupSymbol(name, symbols);
                        return new SemanticInfo(type, dxs);
                    }
                    else
                    {
                        dxs.Add(DiagnosticFacts.GetClusterDatabaseOrTableExpected().WithLocation(node.OpenBracket));
                        return new SemanticInfo(ErrorSymbol.Instance, dxs);
                    }
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(dxs);
                    s_symbolListPool.ReturnToPool(symbols);
                }
            }

            private bool CheckEntityGroupElementKind(SymbolKind? kind, Expression expr, List<Diagnostic> diagnostics)
            {
                var resultType = GetResultTypeOrError(expr);

                if (kind is SymbolKind expectedKind)
                {
                    if (resultType.Kind == expectedKind)
                    {
                        return true;
                    }
                    else
                    {
                        switch (expectedKind)
                        {
                            case SymbolKind.Table:
                                diagnostics.Add(DiagnosticFacts.GetTableExpected().WithLocation(expr));
                                break;
                            case SymbolKind.Database:
                                diagnostics.Add(DiagnosticFacts.GetDatabaseExpected().WithLocation(expr));
                                break;
                            case SymbolKind.Cluster:
                                diagnostics.Add(DiagnosticFacts.GetClusterExpected().WithLocation(expr));
                                break;
                        }

                        return false;
                    }
                }
                else if (resultType is ClusterSymbol
                    || resultType is DatabaseSymbol
                    || resultType is TableSymbol)
                {
                    return true;
                }
                else
                {
                    diagnostics.Add(DiagnosticFacts.GetClusterDatabaseOrTableExpected().WithLocation(expr));
                    return false;
                }
            }

            public override SemanticInfo VisitOrderedExpression(OrderedExpression node)
            {
                return new SemanticInfo(GetReferencedSymbol(node.Expression), GetResultTypeOrError(node.Expression));
            }

            public override SemanticInfo VisitSimpleNamedExpression(SimpleNamedExpression node)
            {
                return new SemanticInfo(GetResultTypeOrError(node.Expression));
            }

            public override SemanticInfo VisitCompoundNamedExpression(CompoundNamedExpression node)
            {
                return new SemanticInfo(GetResultTypeOrError(node.Expression));
            }

            public override SemanticInfo VisitPipeExpression(PipeExpression node)
            {
                return new SemanticInfo(GetResultTypeOrError(node.Operator));
            }

            public override SemanticInfo VisitAtExpression(AtExpression node)
            {
                // TODO:
                return null;
            }

            public override SemanticInfo VisitDataScopeExpression(DataScopeExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    _binder.CheckIsTabular(node.Expression, diagnostics);

                    return new SemanticInfo(GetResultTypeOrError(node.Expression), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitExpressionCouple(ExpressionCouple node)
            {
                // this is used in between operator.. its not an expression itself.
                return null;
            }

            public override SemanticInfo VisitExpressionList(ExpressionList node)
            {
                return null;
            }

            public override SemanticInfo VisitForkExpression(ForkExpression node)
            {
                var resultType = GetResultTypeOrError(node.Expression);

                if (node.NameEquals != null)
                {
                    if (resultType is TableSymbol table)
                    {
                        resultType = new TableSymbol(_binder.GetDeclaredAndInferredColumns(table))
                            .WithInheritableProperties(table)
                            .WithName(node.NameEquals.Name.SimpleName);
                    }
                }

                return new SemanticInfo(resultType);
            }

            public override SemanticInfo VisitPartitionSubquery(PartitionSubquery node)
            {
                var resultType = GetResultTypeOrError(node.Subquery);

                if (resultType is TableSymbol table)
                {
                    resultType = new TableSymbol(_binder.GetDeclaredAndInferredColumns(table))
                        .WithInheritableProperties(table);
                }

                return new SemanticInfo(resultType);
            }

            public override SemanticInfo VisitPartitionQuery(PartitionQuery node)
            {
                var resultType = GetResultTypeOrError(node.Query);

                if (resultType is TableSymbol table)
                {
                    resultType = new TableSymbol(_binder.GetDeclaredAndInferredColumns(table))
                        .WithInheritableProperties(table);
                }

                return new SemanticInfo(resultType);
            }

            public override SemanticInfo VisitPartitionScope(PartitionScope node)
            {
                return null;
            }

            public override SemanticInfo VisitJsonArrayExpression(JsonArrayExpression node)
            {
                var commonType = TypeFacts.GetCommonResultType(node.Values, Conversion.None);
                var arrayType = ScalarTypes.GetDynamicArray(commonType);
                return new SemanticInfo(arrayType);
            }

            public override SemanticInfo VisitJsonObjectExpression(JsonObjectExpression node)
            {
                var columns = s_columnListPool.AllocateFromPool();
                try
                {
                    for (int i = 0; i < node.Pairs.Count; i++)
                    {
                        var pair = node.Pairs[i].Element;
                        var column = new ColumnSymbol(pair.Name.ValueText, pair.Value.ResultType ?? ScalarTypes.Unknown);
                        columns.Add(column);
                    }

                    var bagType = ScalarTypes.GetDynamicBag(columns);
                    return new SemanticInfo(bagType);
                }
                finally
                {
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            public override SemanticInfo VisitJsonPair(JsonPair node)
            {
                return null;
            }

            public override SemanticInfo VisitList(SyntaxList list)
            {
                return null;
            }

            public override SemanticInfo VisitSeparatedElement(SeparatedElement separatedElement)
            {
                return null;
            }

            public override SemanticInfo VisitMakeSeriesExpression(MakeSeriesExpression node)
            {
                return new SemanticInfo(GetResultTypeOrError(node.Expression));
            }

            public override SemanticInfo VisitNamedParameter(NamedParameter node)
            {
                return null;
            }

            public override SemanticInfo VisitPackExpression(PackExpression node)
            {
                return null;
            }

            public override SemanticInfo VisitPatternMatch(PatternMatch node)
            {
                return null;
            }

            public override SemanticInfo VisitPatternPathValue(PatternPathValue node)
            {
                return null;
            }

            public override SemanticInfo VisitPatternPathParameter(PatternPathParameter node)
            {
                return null;
            }

            public override SemanticInfo VisitPrimitiveTypeExpression(PrimitiveTypeExpression node)
            {
                var dx = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    var type = _binder.GetTypeFromTypeExpression(node, dx);
                    return new SemanticInfo(type, VoidSymbol.Instance, dx);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(dx);
                }
            }

            public override SemanticInfo VisitSchemaTypeExpression(SchemaTypeExpression node)
            {
                var dx = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    var type = _binder.GetTypeFromTypeExpression(node, dx);
                    return new SemanticInfo(type, VoidSymbol.Instance, dx);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(dx);
                }
            }

            public override SemanticInfo VisitQueryBlock(QueryBlock node)
            {
                return null;
            }

            public override SemanticInfo VisitSkippedTokens(SkippedTokens node)
            {
                return null;
            }

            public override SemanticInfo VisitRenameList(RenameList node)
            {
                return null;
            }

            public override SemanticInfo VisitNameReferenceList(NameReferenceList node)
            {
                return null;
            }

            private static bool IsArgument(Expression e) =>
                 e.Parent is SeparatedElement<Expression> se
                    && se.Parent is SyntaxList list
                    && list.Parent is ExpressionList el
                    && el.Parent is FunctionCallExpression;

            private static bool IsLeftOperand(Expression e) =>
                e.Parent is BinaryExpression be && be.Left == e;

            public override SemanticInfo VisitStarExpression(StarExpression node)
            {
                if (IsArgument(node))
                {
                    var columns = _binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty);
                    var refSymbol = new GroupSymbol(columns);
                    var resultSymbol = new TupleSymbol(columns);
                    return new SemanticInfo(refSymbol, resultSymbol);
                }
                else if (IsLeftOperand(node))
                {
                    return new SemanticInfo(ScalarTypes.Dynamic);
                }
                else
                {
                    return null;
                }
            }

            public override SemanticInfo VisitTypedColumnReference(TypedColumnReference node)
            {
                return new SemanticInfo(GetResultTypeOrError(node.Column));
            }

            public override SemanticInfo VisitCustom(CustomNode node)
            {
                return null;
            }

            public override SemanticInfo VisitMaterializedViewCombineExpression(MaterializedViewCombineExpression node)
            {
                var resultType = GetResultTypeOrError(node.AggregationsClause.Expression);
                return new SemanticInfo(resultType);
            }

            public override SemanticInfo VisitMaterializedViewCombineNameClause(MaterializedViewCombineNameClause node)
            {
                // verify string literal 
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    if (!_binder.CheckIsExactType(node.Value, ScalarTypes.String, diagnostics) ||
                    !_binder.CheckIsLiteral(node.Value, diagnostics))
                    {
                        return new SemanticInfo(diagnostics);
                    }

                    return null;

                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitMaterializedViewCombineClause(MaterializedViewCombineClause node)
            {
                // handled by VisitMaterializedViewCombineExpression
                return null;
            }
            #endregion

            #region query operators
            private void CheckFirstInPipe(QueryOperator queryOp, List<Diagnostic> diagnostics)
            {
                if (KustoFacts.HasPipedInput(queryOp))
                {
                    diagnostics.Add(DiagnosticFacts.GetQueryOperatorMustBeFirst().WithLocation(queryOp.GetChild(0) ?? queryOp));
                }
            }

            private void CheckNotFirstInPipe(QueryOperator queryOp, List<Diagnostic> diagnostics)
            {
                if (!KustoFacts.HasPipedInput(queryOp))
                {
                    diagnostics.Add(DiagnosticFacts.GetQueryOperatorCannotBeFirst().WithLocation(queryOp.GetChild(0) ?? queryOp));
                }
            }

            public override SemanticInfo VisitBadQueryOperator(BadQueryOperator node)
            {
                return ErrorInfo;
            }

            public override SemanticInfo VisitFilterOperator(FilterOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.FilterParameters, diagnostics);
                    _binder.CheckIsExactType(node.Condition, ScalarTypes.Bool, diagnostics);

                    var resultTable = new TableSymbol(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty))
                        .WithInheritableProperties(RowScopeOrEmpty);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitTakeOperator(TakeOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.TakeParameters, diagnostics);
                    _binder.CheckIsInteger(node.Expression, diagnostics);

                    var resultTable = new TableSymbol(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty))
                        .WithInheritableProperties(RowScopeOrEmpty)
                        .WithIsSorted(false);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitSampleOperator(SampleOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.SampleParameters, diagnostics);
                    _binder.CheckIsInteger(node.Expression, diagnostics);

                    var resultTable = new TableSymbol(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty))
                        .WithInheritableProperties(RowScopeOrEmpty)
                        .WithIsSorted(false);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitSampleDistinctOperator(SampleDistinctOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.SampleDistinctParameters, diagnostics);
                    _binder.CheckIsInteger(node.Expression, diagnostics);
                    var _ = _binder.CheckIsColumn(node.OfExpression, diagnostics)
                        && _binder.CheckIsNotType(node.OfExpression, ScalarTypes.Dynamic, diagnostics);

                    var ofCol = GetOrDeclareColumnForExpression(node.OfExpression, defaultName: "Column1");

                    var result = new TableSymbol(ofCol)
                            .WithInheritableProperties(RowScopeOrEmpty)
                            .WithIsSorted(false);

                    return new SemanticInfo(result, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitCountOperator(CountOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    var name = node.AsIdentifier != null ? node.AsIdentifier.Identifier.ValueText : "Count";
                    return new SemanticInfo(new TableSymbol(new ColumnSymbol(name, ScalarTypes.Long)), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitProjectOperator(ProjectOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckIsScalar(node.Expressions, diagnostics);

                    _binder.CreateProjectionColumns(node.Expressions, builder, diagnostics);

                    var resultTable = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(RowScopeOrEmpty);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitProjectAwayOperator(ProjectAwayOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.GetColumnsInColumnList(node.Expressions, columns, diagnostics);
                    var namesToRemove = new HashSet<string>(columns.Select(c => c.Name));
                    columns.Clear();

                    // only include columns from the original table that are not included in the list
                    foreach (ColumnSymbol column in RowScopeOrEmpty.Members)
                    {
                        if (!namesToRemove.Contains(column.Name))
                        {
                            columns.Add(column);
                        }
                    }

                    var resultType = new TableSymbol(columns).WithInheritableProperties(RowScopeOrEmpty);
                    var info = new SemanticInfo(resultType, diagnostics);
                    return info;
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            public override SemanticInfo VisitProjectKeepOperator(ProjectKeepOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.GetColumnsInColumnList(node.Expressions, columns, diagnostics);
                    var namesToKeep = new HashSet<string>(columns.Select(c => c.Name));
                    columns.Clear();

                    // only include columns from the original table that are not included in the list
                    foreach (ColumnSymbol column in RowScopeOrEmpty.Members)
                    {
                        if (namesToKeep.Contains(column.Name))
                        {
                            columns.Add(column);
                        }
                    }

                    var resultType = new TableSymbol(columns).WithInheritableProperties(RowScopeOrEmpty);
                    var info = new SemanticInfo(resultType, diagnostics);
                    return info;
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            public override SemanticInfo VisitProjectRenameOperator(ProjectRenameOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    builder.AddRange(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty), declare: true, doNotRepeat: true);
                    _binder.CreateProjectionColumns(node.Expressions, builder, diagnostics, style: ProjectionStyle.Rename);

                    var resultTable = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(RowScopeOrEmpty);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitProjectReorderOperator(ProjectReorderOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CreateProjectionColumns(node.Expressions, builder, diagnostics, style: ProjectionStyle.Reorder, doNotRepeat: true);

                    // add any remaining columns not explicit in projection
                    foreach (var col in RowScopeOrEmpty.Columns)
                    {
                        builder.Add(col);
                    }

                    var resultTable = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(RowScopeOrEmpty);

                    var info = new SemanticInfo(resultTable, diagnostics);
                    return info;
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitProjectByNamesOperator(ProjectByNamesOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CreateProjectionColumns(node.Expressions, builder, diagnostics, style: ProjectionStyle.ByNames, doNotRepeat: true);

                    var resultTable = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(RowScopeOrEmpty);

                    var info = new SemanticInfo(resultTable, diagnostics);
                    return info;
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitExtendOperator(ExtendOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckIsScalar(node.Expressions, diagnostics);

                    builder.AddRange(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty), doNotRepeat: true);
                    _binder.CreateProjectionColumns(node.Expressions, builder, diagnostics, style: ProjectionStyle.Extend);

                    var resultType = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(RowScopeOrEmpty);

                    var info = new SemanticInfo(resultType, diagnostics);
                    return info;
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitSummarizeOperator(SummarizeOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.SummarizeParameters, diagnostics);

                    _binder.CheckIsScalar(node.Aggregates, diagnostics);

                    if (node.ByClause != null)
                    {
                        var _ = _binder.CheckIsScalar(node.ByClause.Expressions, diagnostics)
                            && _binder.CheckIsNotDynamic(node.ByClause.Expressions, diagnostics);

                        // all columns corresponding to by-clause expressions
                        _binder.CreateProjectionColumns(node.ByClause.Expressions, builder, diagnostics);

                        // don't re-add any columns already added from by-clause
                        builder.DoNotAddAny(builder.GetProjection());
                    }

                    // all columns corresponding to aggregate expressions
                    _binder.CreateProjectionColumns(node.Aggregates, builder, diagnostics, style: ProjectionStyle.Summarize);

                    var resultTable = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(RowScopeOrEmpty)
                        .WithIsSorted(false);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitDistinctOperator(DistinctOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.DistinctParameters, diagnostics);

                    _binder.CreateProjectionColumns(node.Expressions, builder, diagnostics);

                    var star = node.Expressions.FirstOrDefault(e => e.Element is StarExpression)?.Element;
                    var projection = builder.GetProjection();

                    var _ = _binder.CheckIsScalar(node.Expressions, diagnostics)
                        && _binder.CheckIsNotDynamic(node.Expressions, diagnostics)
                        && (star == null || _binder.CheckIsNotDynamic(projection, star, diagnostics));

                    var resultTable = new TableSymbol(projection)
                        .WithInheritableProperties(RowScopeOrEmpty)
                        .WithIsSorted(false);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitAssertSchemaOperator(AssertSchemaOperator node)
            {
                var resultTable = new TableSymbol(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty))
                        .WithInheritableProperties(RowScopeOrEmpty);

                return new SemanticInfo(resultTable);
            }

            public override SemanticInfo VisitTopOperator(TopOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckIsInteger(node.Expression, diagnostics);
                    _binder.CheckIsScalar(node.ByExpression, diagnostics);

                    // does not change table shape
                    var resultTable = new TableSymbol(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty))
                        .WithInheritableProperties(RowScopeOrEmpty)
                        .WithIsSorted(true);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitTopHittersOperator(TopHittersOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckIsInteger(node.Expression, diagnostics);
                    _binder.CheckIsColumn(node.OfExpression, diagnostics);

                    _binder.CreateProjectionColumns(node.OfExpression, builder, diagnostics);

                    if (node.ByClause != null)
                    {
                        _binder.CheckIsNumber(node.ByClause.Expression, diagnostics);
                        var approxSumCol = GetOrDeclareColumnForExpression(node.ByClause.Expression, "approximate_sum_" + GetExpressionResultName(node.ByClause.Expression));
                        builder.Add(approxSumCol);
                    }
                    else
                    {
                        var approxCountCol = GetOrDeclareColumnForExpression(node.OfExpression, "approximate_count_" + GetExpressionResultName(node.OfExpression), ScalarTypes.Long);
                        builder.Add(approxCountCol);
                    }

                    var resultTable = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(RowScopeOrEmpty)
                        .WithIsSorted(true);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitTopNestedOperator(TopNestedOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                var uniqueNames = s_uniqueNameTablePool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    for (int i = 0, n = node.Clauses.Count; i < n; i++)
                    {
                        var clause = node.Clauses[i].Element;

                        if (clause.Expression != null)
                        {
                            _binder.CheckIsInteger(clause.Expression, diagnostics);
                        }

                        _binder.CheckIsScalar(clause.OfExpression, diagnostics);

                        if (clause.WithOthersClause != null)
                        {
                            _binder.CheckIsScalar(clause.WithOthersClause.Expression, diagnostics);
                        }

                        _binder.CheckIsScalar(clause.ByExpression, diagnostics);

                        var ofName = uniqueNames.GetOrAddName(GetExpressionResultName(clause.OfExpression));
                        var ofCol = GetOrDeclareColumnForExpression(clause.OfExpression, ofName);
                        columns.Add(ofCol);

                        var byName = uniqueNames.GetOrAddName(GetExpressionDeclaredName(clause.ByExpression) ?? "aggregated_" + ofName);
                        var byCol = GetOrDeclareColumnForExpression(clause.ByExpression, byName);
                        columns.Add(byCol);
                    }

                    var resultTable = new TableSymbol(columns)
                        .WithInheritableProperties(RowScopeOrEmpty)
                        .WithIsSorted(true);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                    s_uniqueNameTablePool.ReturnToPool(uniqueNames);
                }
            }

            public override SemanticInfo VisitConsumeOperator(ConsumeOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.ConsumeParameters, diagnostics);

                    // consume doesn't produce anything
                    return new SemanticInfo(VoidSymbol.Instance, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitExecuteAndCacheOperator(ExecuteAndCacheOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    // execute and cache doesn't change anything?
                    var resultTable = new TableSymbol(RowScopeOrEmpty.Columns)
                        .WithInheritableProperties(RowScopeOrEmpty);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitRowSchema(RowSchema node)
            {
                // handled by parent node
                return null;
            }

            public override SemanticInfo VisitEvaluateRowSchema(EvaluateRowSchema node)
            {
                // handled by parent node
                return null;
            }

            public override SemanticInfo VisitDataTableExpression(DataTableExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                try
                {
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.DataTableParameters, diagnostics);
                    CreateColumnsFromRowSchema(node.Schema.Columns, columns, diagnostics);
                    _binder.CheckDataValueTypes(node.Values, columns, diagnostics);
                    return new SemanticInfo(new TableSymbol(columns), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            public override SemanticInfo VisitContextualDataTableExpression(ContextualDataTableExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                try
                {
                    CreateColumnsFromRowSchema(node.Schema.Columns, columns, diagnostics);

                    if (node.Id != null)
                    {
                        var _ = _binder.CheckIsExactType(node.Id, ScalarTypes.Guid, diagnostics)
                            && _binder.CheckIsLiteral(node.Id, diagnostics);
                    }

                    return new SemanticInfo(new TableSymbol(columns), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            public override SemanticInfo VisitExternalDataExpression(ExternalDataExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                try
                {
                    CreateColumnsFromRowSchema(node.Schema.Columns, columns, diagnostics);

                    node.URIs.Select(item => _binder.CheckIsExactType(item.Element, ScalarTypes.String, diagnostics));

                    if (node.WithClause != null)
                    {
                        // Does not check properties in with clause. Any property name is legal?
                    }

                    return new SemanticInfo(new TableSymbol(columns), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            public override SemanticInfo VisitInlineExternalTableExpression(InlineExternalTableExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                var partitionColumnNames = s_stringSetPool.AllocateFromPool();
                var partitionColumnsDirectlyUsed = s_stringSetPool.AllocateFromPool();
                var partitionColumnsFunctionUsed = s_expressionListPool.AllocateFromPool();
                try
                {
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.SortParameters, diagnostics);

                    CreateColumnsFromRowSchema(node.Schema.Columns, columns, diagnostics);

                    if (node.PartitionClause != null)
                    {
                        foreach (var partitionColumn in node.PartitionClause.PartitionColumns)
                        {
                            // Check Partition column names uniqueness
                            if (!DeclareColumnName(partitionColumnNames, partitionColumn.Element.Name.SimpleName, diagnostics, partitionColumn.Element.Name.Name))
                            {
                                diagnostics.Add(DiagnosticFacts.GetDuplicateColumnDeclaration(partitionColumn.Element.Name.SimpleName).WithLocation(partitionColumn.Element));
                                break;
                            }

                            TypeSymbol type = null;
                            switch (partitionColumn.Element.Type)
                            {
                                case PrimitiveTypeExpression p:
                                    type = Binder.GetType(p);
                                    break;
                                default:
                                    diagnostics?.Add(DiagnosticFacts.GetWrongPartitionColumnType().WithLocation(partitionColumn.Element));
                                    break;
                            }

                            if (type == null)
                            {
                                break;
                            }
                            if (partitionColumn.Element.Expr == null)
                            {
                                if (columns.Any(item => item.Name == partitionColumn.Element.Name.SimpleName))
                                {
                                    diagnostics.Add(DiagnosticFacts.GetDuplicateColumnDeclaration(partitionColumn.Element.Name.SimpleName).WithLocation(partitionColumn.Element));
                                    break;
                                }

                                if (SymbolsAssignable(type, ScalarTypes.Long, Conversion.None))
                                {
                                    diagnostics?.Add(DiagnosticFacts.GetWrongVirtualPartitionColumnType().WithLocation(partitionColumn.Element));
                                    break;
                                }
                                // Virtual Column need to be added to the list of columns
                                columns.Add(new ColumnSymbol(partitionColumn.Element.Name.SimpleName, type, source: partitionColumn.Element.Name));
                            }
                            else
                            {
                                _binder.CheckIsExactType(partitionColumn.Element.Expr, type, diagnostics);
                                // Validate that only closed list of functions is allowed for partition column
                                if (partitionColumn.Element.Expr.Kind == SyntaxKind.FunctionCallExpression)
                                {
                                    if (!KustoFacts.InlineExternalTablePartitionColumnFunctions.Contains(((FunctionCallExpression)partitionColumn.Element.Expr).Name.SimpleName))
                                    {
                                        diagnostics?.Add(DiagnosticFacts.GetWrongPartitionColumnFunction().WithLocation(partitionColumn.Element));
                                    }
                                }
                            }
                        }
                    }
                    if (node.PathFormat != null)
                    {
                        foreach (var pathFormatElement in node.PathFormat.PathExpressions)
                        {
                            if (pathFormatElement.PartitionColumnExpression.Kind == SyntaxKind.NameReference)
                            {
                                partitionColumnsDirectlyUsed.Add(((NameReference)pathFormatElement.PartitionColumnExpression).SimpleName);
                            }
                            else if (pathFormatElement.PartitionColumnExpression.Kind == SyntaxKind.DateTimePattern)
                            {
                                var dateTimePattern = (DateTimePattern)pathFormatElement.PartitionColumnExpression;
                                if (!CheckDateTimePatternAllowed(partitionColumnsFunctionUsed, dateTimePattern))
                                {
                                    diagnostics.Add(DiagnosticFacts.GetPartitionColumnCanNotBeUsedBothDirectlyAndPattern(((NameReference)dateTimePattern.PartitionColumn).SimpleName).WithLocation(node.PathFormat));
                                    break;
                                }
                                partitionColumnsFunctionUsed.Add(dateTimePattern);
                            }
                        }

                        // Find all partition column names not present in either set
                        var unusedPartitionColumns = partitionColumnNames
                            .Where(name => !partitionColumnsDirectlyUsed.Contains(name) && !partitionColumnsFunctionUsed.Any(item => ((DateTimePattern)item).PartitionColumn.SimpleName == name))
                            .ToList();

                        // Add diagnostics for each unused column
                        foreach (var name in unusedPartitionColumns)
                        {
                            diagnostics.Add(DiagnosticFacts.GetPartitionColumnNotUsedInPathFormat(name).WithLocation(node.PathFormat));
                        }

                        // Find partitions that have both direct and function usage
                        var conflictingPartitionColumns = partitionColumnsFunctionUsed
                            .Where(expr => partitionColumnsDirectlyUsed.Contains(((DateTimePattern)expr).PartitionColumn.SimpleName))
                            .ToList();

                        // Add diagnostics for each conflicting partition
                        foreach (var expr in conflictingPartitionColumns)
                        {
                            diagnostics.Add(DiagnosticFacts
                                .GetPartitionColumnCanNotBeUsedBothDirectlyAndPattern(((DateTimePattern)expr).PartitionColumn.SimpleName)
                                .WithLocation(node.PathFormat));
                        }
                    }

                    node.ConnectionStrings.ConnectionStrings.Select(item => _binder.CheckIsExactType(item.Element, ScalarTypes.String, diagnostics));

                    if (!KustoFacts.InlineExternalTableDataFormats.Contains(node.DataFormatParameter.Value.ValueText))
                    {
                        diagnostics.Add(DiagnosticFacts.GetWrongDataStreamType(node.DataFormatParameter.Value.ValueText).WithLocation(node.DataFormatParameter));
                    }

                    return new SemanticInfo(new TableSymbol(columns), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                    s_stringSetPool.ReturnToPool(partitionColumnNames);
                    s_stringSetPool.ReturnToPool(partitionColumnsDirectlyUsed);
                    s_expressionListPool.ReturnToPool(partitionColumnsFunctionUsed);
                }
            }

            private bool CheckDateTimePatternAllowed(List<Expression> existingExpressions, DateTimePattern current)
            {
                foreach (var existingExpression in existingExpressions)
                {
                    var existingDateTimePattern = existingExpression as DateTimePattern;
                    if (existingDateTimePattern.PartitionColumn.SimpleName == current.PartitionColumn.SimpleName
                        && existingDateTimePattern.StringLiteral.Token.ValueText != current.StringLiteral.Token.ValueText)
                    {
                        return false;
                    }
                }
                return true;
            }

            public override SemanticInfo VisitDateTimePattern(DateTimePattern node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    _binder.CheckIsExactType(node.PartitionColumn, ScalarTypes.DateTime, diagnostics);
                    return new SemanticInfo(ScalarTypes.DateTime, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitSortOperator(SortOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.SortParameters, diagnostics);

                    var _ = _binder.CheckIsScalar(node.Expressions, diagnostics)
                        && _binder.CheckIsNotDynamic(node.Expressions, diagnostics);

                    var resultTable = new TableSymbol(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty))
                        .WithInheritableProperties(RowScopeOrEmpty)
                        .WithIsSorted(true);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitSerializeOperator(SerializeOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.SerializedParameters, diagnostics);

                    _binder.CheckIsScalar(node.Expressions, diagnostics);

                    builder.AddRange(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty), doNotRepeat: true);
                    _binder.CreateProjectionColumns(node.Expressions, builder, diagnostics);

                    var resultType = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(RowScopeOrEmpty)
                        .WithIsSerialized(true);

                    var info = new SemanticInfo(resultType, diagnostics);
                    return info;
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitAsOperator(AsOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.AsParameters, diagnostics);

                    var resultTable = new TableSymbol(RowScopeOrEmpty.Columns)
                        .WithInheritableProperties(RowScopeOrEmpty);

                    return new SemanticInfo(resultTable);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitForkOperator(ForkOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    var tables = new List<TableSymbol>();
                    for (int i = 0, n = node.Expressions.Count; i < n; i++)
                    {
                        var expr = node.Expressions[i].Expression;
                        _binder.CheckIsTabular(expr, diagnostics);
                        CheckQueryOperators(expr, KustoFacts.ForkOperatorKinds, diagnostics);

                        var tableType = GetResultType(expr) as TableSymbol;
                        if (tableType != null)
                        {
                            var name = tables.Count == 0 ? "Results" : "Results_" + (tables.Count + 1);
                            var table = new TableSymbol(name, _binder.GetDeclaredAndInferredColumns(tableType)).WithInheritableProperties(tableType);
                            tables.Add(table);
                        }
                    }

                    return new SemanticInfo(new GroupSymbol(tables), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            private static void CheckQueryOperators(
                Expression expr,
                IReadOnlyList<SyntaxKind> validQueryOperators,
                List<Diagnostic> diagnostics,
                bool operatorRequired = true,
                bool allowContextualRoot = false)
            {
                while (expr is PipeExpression pe)
                {
                    CheckQueryOperator(pe.Operator, validQueryOperators, diagnostics);
                    expr = pe.Expression;
                }

                if (expr is QueryOperator q)
                {
                    CheckQueryOperator(q, validQueryOperators, diagnostics);
                }
                else if (operatorRequired && !(allowContextualRoot && expr is ContextualDataTableExpression))
                {
                    diagnostics.Add(DiagnosticFacts.GetQueryOperatorExpected().WithLocation(expr));
                }
            }

            private static void CheckQueryOperator(QueryOperator queryOperator, IReadOnlyList<SyntaxKind> validQueryOperators, List<Diagnostic> diagnostics)
            {
                var keyword = queryOperator.GetFirstToken();
                if (keyword != null
                    && !validQueryOperators.Contains(queryOperator.Kind)
                    && !queryOperator.ContainsSyntaxDiagnostics)
                {
                    diagnostics.Add(DiagnosticFacts.GetQueryOperatorNotAllowedInContext(keyword.Text).WithLocation(keyword));
                }
            }

            public override SemanticInfo VisitPartitionByOperator(PartitionByOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    // todo: check parameters when the correct set is known
                    //_binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.PartitionParameters, diagnostics);

                    var _ = _binder.CheckIsColumn(node.Entity, diagnostics)
                        && _binder.CheckIsNotType(node.Entity, ScalarTypes.Dynamic, diagnostics);

                    CheckQueryOperators(node.Subquery, KustoFacts.PostPipeOperatorKinds, diagnostics, allowContextualRoot: true);

                    var tableType = node.Subquery.ResultType as TableSymbol;

                    var result = new TableSymbol(_binder.GetDeclaredAndInferredColumns(tableType))
                                    .WithInheritableProperties(RowScopeOrEmpty)
                                    .WithIsSorted(false);

                    return new SemanticInfo(result, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitPartitionByIdClause(PartitionByIdClause node)
            {
                return null;
            }

            public override SemanticInfo VisitPartitionOperator(PartitionOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.PartitionParameters, diagnostics);

                    var operand = node.Operand;
                    var _ = _binder.CheckIsColumn(node.ByExpression, diagnostics)
                        && _binder.CheckIsNotType(node.ByExpression, ScalarTypes.Dynamic, diagnostics);
                    _binder.CheckIsTabular(operand, diagnostics);

                    if (operand is PartitionSubquery ps)
                    {
                        CheckQueryOperators(ps.Subquery, KustoFacts.PostPipeOperatorKinds, diagnostics);
                    }

                    var tableType = GetResultType(operand) as TableSymbol;
                    if (tableType == null)
                    {
                        // Failed to resolve operand as table operator
                        return new SemanticInfo(RowScopeOrEmpty, diagnostics);
                    }

                    var result = new TableSymbol(_binder.GetDeclaredAndInferredColumns(tableType))
                        .WithInheritableProperties(RowScopeOrEmpty)
                        .WithIsSorted(false);

                    return new SemanticInfo(result, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitSearchOperator(SearchOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                var tables = s_tableListPool.AllocateFromPool();
                try
                {
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.SearchParameters, diagnostics);
                    _binder.CheckIsExactType(node.Condition, ScalarTypes.Bool, diagnostics);

                    columns.Add(new ColumnSymbol("$table", ScalarTypes.String));

                    TableSymbol result;
                    if (node.InClause != null)
                    {
                        CheckFirstInPipe(node, diagnostics);

                        for (int i = 0, n = node.InClause.Expressions.Count; i < n; i++)
                        {
                            var expr = node.InClause.Expressions[i].Element;
                            _binder.CheckIsTabular(expr, diagnostics);
                        }
                    }

                    var searchColumnsTable = _binder.GetSearchColumnsTable(node);
                    _binder.GetDeclaredAndInferredColumns(searchColumnsTable, columns);

                    if (_binder._rowScope != null)
                    {
                        // if no in-clause can be any position in pipe
                        result = new TableSymbol(columns)
                            .WithInheritableProperties(_binder._rowScope)
                            .WithIsSorted(false);
                    }
                    else
                    {
                        result = new TableSymbol(columns)
                            .WithIsOpen(searchColumnsTable.IsOpen);
                    }

                    return new SemanticInfo(result, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_tableListPool.ReturnToPool(tables);
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            public override SemanticInfo VisitFindOperator(FindOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                var refColumns = s_columnListPool.AllocateFromPool();
                var packColumns = s_columnListPool.AllocateFromPool();
                var refTables = s_tableListPool.AllocateFromPool();

                try
                {
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.FindParameters, diagnostics);
                    _binder.CheckIsExactType(node.Condition, ScalarTypes.Bool, diagnostics);

                    var sourceColumnName = node.Parameters.GetParameterNameValue(QueryOperatorParameters.WithSource) ?? "source_";
                    columns.Add(new ColumnSymbol(sourceColumnName, ScalarTypes.String));

                    var tables = _binder.GetFindTables(node);
                    var resultIsOpen = tables.Any(t => t.IsOpen);
                    var explicitPack = false;

                    // only consider tables that have column references
                    _binder.GetReferencedColumnsInTree(node.Condition, refColumns);

                    if (tables.Count == 1)
                    {
                        // only one table
                        refTables.AddRange(tables);
                    }
                    else if (ReferencesAllTablesImplicitly(node))
                    {
                        // references all columns from all tables
                        refTables.AddRange(tables);
                    }
                    else if (tables.Count > 0 && refColumns.Count > 0)
                    {
                        // only include tables that have a column the same name as one
                        // referenced in the condition
                        foreach (var t in tables)
                        {
                            foreach (var c in refColumns)
                            {
                                // if the table has a column of the same name
                                if (t.IsOpen || t.TryGetColumn(c.Name, out _))
                                {
                                    refTables.Add(t);
                                    break;
                                }
                            }
                        }
                    }

                    if (node.Project == null || node.Project.ProjectKeyword.Kind == SyntaxKind.ProjectSmartKeyword)
                    {
                        // project-smart

                        // any column that is common to all referenced tables
                        var commonColumnsTable = _binder.GetTableOfCommonColumns(refTables);
                        columns.AddRange(commonColumnsTable.Columns);

                        // any columns referenced explicitly in the predicate
                        foreach (var c in refColumns)
                        {
                            if (!columns.Contains(c))
                            {
                                columns.Add(c);
                            }
                        }

                        GetPackColumns(refTables, columns, packColumns);

                        if (packColumns.Count > 0)
                        {
                            columns.Add(new ColumnSymbol("pack_", ScalarTypes.Dynamic, originalColumns: packColumns));
                        }

                        UnifyColumnsWithSameNameAndType(columns);
                    }
                    else
                    {
                        // explicit projection
                        resultIsOpen = false;

                        // regular project
                        for (int i = 0; i < node.Project.Columns.Count; i++)
                        {
                            var exp = node.Project.Columns[i].Element;

                            switch (exp)
                            {
                                case PackExpression _:
                                    explicitPack = true;
                                    if (i == node.Project.Columns.Count - 1)
                                    {
                                        GetPackColumns(refTables, columns, packColumns);
                                        columns.Add(new ColumnSymbol("pack_", ScalarTypes.Dynamic, originalColumns: packColumns));
                                    }
                                    else
                                    {
                                        diagnostics.Add(DiagnosticFacts.GetPackMustBeLastItemInList().WithLocation(exp));
                                    }
                                    break;

                                case TypedColumnReference tc:
                                    _binder.CheckIsColumn(tc.Column, diagnostics);
                                    if (GetReferencedSymbol(tc.Column) is ColumnSymbol c)
                                    {
                                        var type = _binder.GetTypeFromTypeExpression(tc.Type, diagnostics);
                                        columns.Add(new ColumnSymbol(c.Name, type, originalColumns: new[] { c }));
                                    }
                                    break;

                                case NameReference nr:
                                    _binder.CheckIsColumn(nr, diagnostics);
                                    if (GetReferencedSymbol(nr) is ColumnSymbol c2)
                                    {
                                        columns.Add(c2);
                                    }
                                    break;
                            }
                        }
                    }

                    if (node.ProjectAway != null && node.ProjectAway.Columns.Count > 0)
                    {
                        if (node.ProjectAway.Columns[0].Element is StarExpression)
                        {
                            columns.RemoveAll(c => c.Name != sourceColumnName && (c.Name != "pack_" || !explicitPack));
                        }
                        else
                        {
                            // remove specified columns
                            var columnNamesToRemove = s_stringSetPool.AllocateFromPool();
                            try
                            {
                                for (int i = 0; i < node.ProjectAway.Columns.Count; i++)
                                {
                                    var columnExp = node.ProjectAway.Columns[i].Element;
                                    if (GetReferencedSymbol(columnExp) is ColumnSymbol col)
                                    {
                                        columnNamesToRemove.Add(col.Name);
                                    }
                                }

                                columns.RemoveAll(c => columnNamesToRemove.Contains(c.Name));
                            }
                            finally
                            {
                                s_stringSetPool.ReturnToPool(columnNamesToRemove);
                            }
                        }
                    }

                    var resultTable = new TableSymbol(columns).WithIsOpen(resultIsOpen);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                    s_columnListPool.ReturnToPool(refColumns);
                    s_columnListPool.ReturnToPool(packColumns);
                    s_tableListPool.ReturnToPool(refTables);
                }
            }

            private static void GetPackColumns(IReadOnlyList<TableSymbol> tables, IReadOnlyList<ColumnSymbol> projectedColumns, List<ColumnSymbol> packColumns)
            {
                var colNameMap = s_stringSetPool.AllocateFromPool();
                try
                {
                    // check to see if there is a table that has extra columns that need to be packed
                    foreach (var c in projectedColumns)
                    {
                        colNameMap.Add(c.Name);
                    }

                    foreach (var t in tables)
                    {
                        // if table is open, then we don't know what ends up in projected set at execution time
                        if (t.IsOpen)
                            continue;

                        // if the table has more columns than end up in the projected set
                        // then the rest will appear in a packed_ column
                        foreach (var c in t.Columns)
                        {
                            if (!colNameMap.Contains(c.Name))
                            {
                                packColumns.Add(c);
                                break;
                            }
                        }
                    }
                }
                finally
                {
                    s_stringSetPool.ReturnToPool(colNameMap);
                }
            }

            private static bool ReferencesAllTablesImplicitly(FindOperator node)
            {
                // contains '*' expression as in '* has xxx', so this condition references all columns
                if (node.Condition.GetFirstDescendantOrSelf<StarExpression>() != null)
                    return true;

                // has a stand alone search term, which is an abbreviation of '* has xxx'
                return node.Condition.GetFirstDescendantOrSelf<Expression>(e => IsStandAloneSearchTerm(e)) != null;
            }

            private static bool IsStandAloneSearchTerm(Expression expr)
            {
                // a stand-alone search term is a constant string that was adjusted to bool by SearchAndPredicateBinder
                return expr.IsConstant && expr.ConstantValue is string && expr.ResultType == ScalarTypes.Bool;
            }

            public override SemanticInfo VisitUnionOperator(UnionOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                var tables = s_tableListPool.AllocateFromPool();
                try
                {
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.UnionParameters, diagnostics);

                    if (node.Parameters.GetParameterNameValue(QueryOperatorParameters.WithSource) is string name)
                    {
                        columns.Add(new ColumnSymbol(name, ScalarTypes.String));
                    }

                    if (RowScopeOrEmpty != null)
                    {
                        tables.Add(RowScopeOrEmpty);
                    }

                    for (int i = 0, n = node.Expressions.Count; i < n; i++)
                    {
                        var expr = node.Expressions[i].Element;
                        _binder.CheckIsTabular(expr, diagnostics);
                        _binder.AddTables(GetResultType(expr), tables);
                    }

                    var unifiedTable = _binder.GetTableOfColumnsUnifiedByNameAndType(tables);

                    var resultTable = unifiedTable;

                    if (columns.Count > 0)
                    {
                        columns.AddRange(unifiedTable.Columns);
                        resultTable = new TableSymbol(columns);
                    }

                    if (tables.Any(t => t.IsOpen))
                    {
                        resultTable = resultTable.WithIsOpen(true);
                    }

                    return new SemanticInfo(resultTable.WithIsSorted(false), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                    s_tableListPool.ReturnToPool(tables);
                }
            }

            private static readonly ObjectPool<List<JoinColumnPair>> s_joinColumnsPool =
                new ObjectPool<List<JoinColumnPair>>(() => new List<JoinColumnPair>(), list => list.Clear());

            public override SemanticInfo VisitLookupOperator(LookupOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                var exprColumns = s_columnListPool.AllocateFromPool();
                var joinColumns = s_joinColumnsPool.AllocateFromPool();

                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.LookupParameters, diagnostics);

                    _binder.CheckIsTabular(node.Expression, diagnostics);

                    // check the lookup clause(s)
                    if (node.LookupClause is JoinOnClause joc)
                    {
                        CheckJoinOnClause(joc, diagnostics, joinColumns);
                    }

                    // figure out the result columns
                    _binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty, exprColumns);

                    // substitute common column for any left-side column used in join condition
                    foreach (var col in exprColumns)
                    {
                        var index = joinColumns.FirstIndex(jc => jc.Left == col);
                        if (index >= 0 && index < joinColumns.Count)
                        {
                            var jc = joinColumns[index];
                            columns.Add(new ColumnSymbol(jc.Left.Name, jc.Left.Type, jc.Left.Description, originalColumns: new[] { jc.Left, jc.Right }));
                        }
                        else
                        {
                            columns.Add(col);
                        }
                    }

                    var resultIsOpen = RowScopeOrEmpty.IsOpen;

                    var exprTable = GetResultType(node.Expression) as TableSymbol;
                    if (exprTable != null)
                    {
                        exprColumns.Clear();
                        _binder.GetDeclaredAndInferredColumns(exprTable, exprColumns);

                        // do not include any right-side columns that were used in join condition
                        // since they are already represented by the common column
                        exprColumns.RemoveAll(c => joinColumns.Any(jc => jc.Right == c));

                        columns.AddRange(exprColumns);
                        resultIsOpen |= exprTable.IsOpen;
                    }

                    MakeColumnNamesUnique(columns);

                    var resultTable = new TableSymbol(columns).WithIsOpen(resultIsOpen);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                    s_columnListPool.ReturnToPool(exprColumns);
                    s_joinColumnsPool.ReturnToPool(joinColumns);
                }
            }

            private static bool IsAntiOrSemiJoin(string joinKind)
            {
                return IsLeftAntiOrSemiJoin(joinKind)
                    || IsRightAntiOrSemiJoin(joinKind);
            }

            private static bool IsLeftAntiOrSemiJoin(string joinKind)
            {
                switch (joinKind)
                {
                    case "anti":
                    case "leftanti":
                    case "leftsemi":
                    case "leftantisemi":
                        return true;
                    default:
                        return false;
                }
            }

            private static bool IsRightAntiOrSemiJoin(string joinKind)
            {
                switch (joinKind)
                {
                    case "rightanti":
                    case "rightsemi":
                    case "rightantisemi":
                        return true;
                    default:
                        return false;
                }
            }

            public override SemanticInfo VisitJoinOperator(JoinOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.JoinParameters, diagnostics);

                    _binder.CheckIsTabular(node.Expression, diagnostics);

                    // check the join condition(s)
                    switch (node.ConditionClause)
                    {
                        case JoinOnClause c:
                            CheckJoinOnClause(c, diagnostics);
                            break;
                        case JoinWhereClause c:
                            _binder.CheckIsExactType(c.Expression, ScalarTypes.Bool, diagnostics);
                            break;
                        default:
                            diagnostics.Add(DiagnosticFacts.GetMissingJoinOnClause().WithLocation(node));
                            break;
                    }

                    var joinKind = node.Parameters.GetParameterLiteralValue<string>(QueryOperatorParameters.Kind);
                    var resultIsOpen = false;

                    // if not explicitly a right-anti/semi join, then add left-side columns
                    if (!IsRightAntiOrSemiJoin(joinKind))
                    {
                        // add left-side columns
                        columns.AddRange(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty));
                        resultIsOpen |= RowScopeOrEmpty.IsOpen;
                    }

                    var exprTable = GetResultType(node.Expression) as TableSymbol;
                    if (exprTable != null && !IsLeftAntiOrSemiJoin(joinKind))
                    {
                        // add right-side columns
                        _binder.GetDeclaredAndInferredColumns(exprTable, columns);
                        resultIsOpen |= exprTable.IsOpen;
                    }

                    MakeColumnNamesUnique(columns);

                    var resultTable = new TableSymbol(columns).WithIsOpen(resultIsOpen);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            private class JoinColumnPair
            {
                public ColumnSymbol Left { get; }
                public ColumnSymbol Right { get; }
                public JoinColumnPair(ColumnSymbol left, ColumnSymbol right) { this.Left = left; this.Right = right; }
            }

            private void CheckJoinOnClause(
                JoinOnClause clause,
                List<Diagnostic> diagnostics,
                List<JoinColumnPair> joinColumns = null)
            {
                for (int i = 0, n = clause.Expressions.Count; i < n; i++)
                {
                    var expr = clause.Expressions[i].Element;
                    CheckJoinOnExpression(expr, diagnostics, joinColumns);
                }
            }

            private void CheckJoinOnExpression(
                Expression condition,
                List<Diagnostic> diagnostics,
                List<JoinColumnPair> joinColumns = null)
            {
                condition = RemoveParenthesis(condition);

                if (condition is BinaryExpression be)
                {
                    if (be.Kind == SyntaxKind.EqualExpression)
                    {
                        if (CheckJoinOnEquality(be, diagnostics, out var leftMatchingColumn, out var rightMatchingColumn))
                        {
                            if (leftMatchingColumn != null && rightMatchingColumn != null && joinColumns != null)
                            {
                                joinColumns.Add(new JoinColumnPair(leftMatchingColumn, rightMatchingColumn));
                            }
                        }
                    }
                    else if (be.Kind == SyntaxKind.AndExpression)
                    {
                        CheckJoinOnExpression(be.Left, diagnostics, joinColumns);
                        CheckJoinOnExpression(be.Right, diagnostics, joinColumns);
                    }
                    else
                    {
                        diagnostics.Add(DiagnosticFacts.GetInvalidJoinCondition().WithLocation(condition));
                    }
                }
                else if (condition is NameReference nr)
                {
                    if (_binder.CheckIsColumn(nr, diagnostics))
                    {
                        if (CheckCommonColumn(nr, diagnostics, out var leftColumn, out var rightColumn))
                        {
                            if (leftColumn != null && rightColumn != null && joinColumns != null)
                            {
                                joinColumns.Add(new JoinColumnPair(leftColumn, rightColumn));
                            }
                        }

                        _binder.CheckIsScalar(condition, diagnostics); // are there non-scalar columns?
                    }
                }
                else
                {
                    diagnostics.Add(DiagnosticFacts.GetInvalidJoinCondition().WithLocation(condition));
                }
            }

            private bool CheckCommonColumn(
                NameReference name,
                List<Diagnostic> diagnostics,
                out ColumnSymbol leftColumn,
                out ColumnSymbol rightColumn)
            {
                rightColumn = null;

                if (_binder.TryGetDeclaredOrInferredColumn(RowScopeOrEmpty, name.SimpleName, out leftColumn)
                    && _binder.TryGetDeclaredOrInferredColumn(RightRowScopeOrEmpty, name.SimpleName, out rightColumn))
                {
                    if (leftColumn.Type != rightColumn.Type
                        && leftColumn.Type != ScalarTypes.Unknown
                        && rightColumn.Type != ScalarTypes.Unknown)
                    {
                        diagnostics.Add(DiagnosticFacts.GetCommonJoinColumnsMustHaveSameType(name.SimpleName).WithLocation(name));
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    diagnostics.Add(DiagnosticFacts.GetColumnMustExistOnBothSidesOfJoin(name.SimpleName).WithLocation(name));
                    return false;
                }
            }

            private bool CheckJoinOnEquality(
                Expression condition,
                List<Diagnostic> diagnostics,
                out ColumnSymbol leftColumn,
                out ColumnSymbol rightColumn)
            {
                leftColumn = null;
                rightColumn = null;
                condition = RemoveParenthesis(condition);

                if (!(condition is BinaryExpression be
                    && condition.Kind == SyntaxKind.EqualExpression))
                {
                    diagnostics.Add(DiagnosticFacts.GetInvalidJoinCondition().WithLocation(condition));
                    return false;
                }

                if (CheckJoinOnEqualityOperand(be.Left, "$left", diagnostics, out leftColumn)
                    & CheckJoinOnEqualityOperand(be.Right, "$right", diagnostics, out rightColumn))
                {
                    return CheckJoinOnKeysComparable(be.Left, be.Right, diagnostics);
                }

                return false;
            }

            private static bool CheckJoinOnKeysComparable(Expression left, Expression right, List<Diagnostic> diagnostics)
            {
                var leftType = left.ResultType as ScalarSymbol ?? ScalarTypes.Unknown;
                var rightType = right.ResultType as ScalarSymbol ?? ScalarTypes.Unknown;

                // join keys cannot be dynamic
                if (leftType is DynamicSymbol)
                {
                    if (diagnostics != null)
                        diagnostics.Add(DiagnosticFacts.GetJoinKeyCannotBeDynamic().WithLocation(left));
                    return false;
                }
                else if (rightType is DynamicSymbol)
                {
                    if (diagnostics != null)
                        diagnostics.Add(DiagnosticFacts.GetJoinKeyCannotBeDynamic().WithLocation(right));
                    return false;
                }

                // unknown or error so ignore
                if (leftType == ScalarTypes.Unknown || leftType.IsError
                    || rightType == ScalarTypes.Unknown || rightType.IsError)
                {
                    return true;
                }

                // join keys must be consistent data type
                if (Promote(leftType) != Promote(rightType))
                {
                    if (diagnostics != null)
                        diagnostics.Add(DiagnosticFacts.GetJoinKeysNotComparable(leftType.Name, rightType.Name));
                    return false;
                }

                return true;
            }

            private bool CheckJoinOnEqualityOperand(Expression operand, string prefix, List<Diagnostic> diagnostics, out ColumnSymbol column)
            {
                column = null;
                operand = RemoveParenthesis(operand);

                if (!(operand is PathExpression path
                    && GetReferencedName(path.Expression) == prefix)) // look for $left.c or $right.c
                {
                    diagnostics.Add(DiagnosticFacts.GetInvalidJoinConditionOperand(prefix).WithLocation(operand));
                    return false;
                }

                // look for $left.c or $right.c
                if (_binder.CheckIsColumn(operand, diagnostics))
                {
                    column = GetReferencedSymbol(operand) as ColumnSymbol;
                    return _binder.CheckIsScalar(operand, diagnostics); // are there non-scalar columns?
                }

                return false;
            }

            private static Expression RemoveParenthesis(Expression expression)
            {
                while (expression is ParenthesizedExpression pe)
                    expression = pe.Expression;

                return expression;
            }

            private static string GetReferencedName(Expression expression)
            {
                switch (expression)
                {
                    case NameReference nr:
                        return nr.SimpleName;

                    case BracketedExpression br:
                        if (br.Expression.Kind == SyntaxKind.StringLiteralExpression
                            || br.Expression.Kind == SyntaxKind.CompoundStringLiteralExpression)
                        {
                            return (string)br.Expression.LiteralValue;
                        }
                        break;
                }

                return null;
            }

            public override SemanticInfo VisitRangeOperator(RangeOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckFirstInPipe(node, diagnostics);

                    _binder.CheckIsSummable(node.From, diagnostics);
                    _binder.CheckIsSummable(node.To, diagnostics);
                    _binder.CheckIsSummable(node.Step, diagnostics);
                    _binder.CheckIsNotConstantValue(node.Step, 0L, false, diagnostics);

                    var fromType = GetResultTypeOrError(node.From);
                    var toType = GetResultTypeOrError(node.To);
                    var stepType = GetResultTypeOrError(node.Step);
                    var fromToType = TypeFacts.GetCommonScalarType(fromType, toType) ?? ScalarTypes.Unknown;
                    var rangeType = (fromToType == stepType && stepType == ScalarTypes.Int)
                        ? ScalarTypes.Int // does not match add semantics here
                        : _binder.GetBinaryOperatorResultType(OperatorKind.Add, fromToType, stepType, node.Step, diagnostics);

                    var rangeName = node.Name.SimpleName;
                    var result = new TableSymbol(new ColumnSymbol(rangeName, rangeType));

                    return new SemanticInfo(result, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitFacetOperator(FacetOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    var tables = new List<TableSymbol>();

                    // add with table type to combined output if there is a with clause
                    if (node.WithClause != null)
                    {
                        TableSymbol tableType;
                        switch (node.WithClause)
                        {
                            case FacetWithOperatorClause c:
                                _binder.CheckIsTabular(c.Operator, diagnostics);
                                CheckQueryOperators(c.Operator, KustoFacts.ForkOperatorKinds, diagnostics);
                                tableType = GetResultType(c.Operator) as TableSymbol;
                                if (tableType != null)
                                {
                                    tables.Add(tableType);
                                }
                                break;

                            case FacetWithExpressionClause c:
                                _binder.CheckIsTabular(c.Expression, diagnostics);
                                tableType = GetResultType(c.Expression) as TableSymbol;
                                if (tableType != null)
                                {
                                    tables.Add(tableType);
                                }
                                break;
                        }
                    }

                    // create a separate facet table for each column specified
                    for (int i = 0, n = node.Expressions.Count; i < n; i++)
                    {
                        var expr = node.Expressions[i].Element;
                        var _ = _binder.CheckIsColumn(expr, diagnostics)
                            && _binder.CheckIsNotType(expr, ScalarTypes.Dynamic, diagnostics);

                        var name = GetExpressionResultName(expr);
                        var tableName = i == 0 ? "Facet" : "Facet_" + (i + 1);

                        var table = new TableSymbol(
                                tableName,
                                GetOrDeclareColumnForExpression(expr, name),
                                GetOrDeclareColumnForExpression(expr, "count_" + name, ScalarTypes.Long))
                            .WithInheritableProperties(RowScopeOrEmpty)
                            .WithIsSorted(false);

                        tables.Add(table);
                    }

                    return new SemanticInfo(new GroupSymbol(tables), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitMakeSeriesOperator(MakeSeriesOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.MakeSeriesParameters, diagnostics);

                    // check by clause first, because these columns are first in the result
                    if (node.ByClause != null)
                    {
                        for (int i = 0, n = node.ByClause.Expressions.Count; i < n; i++)
                        {
                            var expr = node.ByClause.Expressions[i].Element;
                            _binder.CheckIsScalar(expr, diagnostics);
                            _binder.CreateProjectionColumns(expr, builder, diagnostics);
                        }
                    }

                    for (int i = 0, n = node.Aggregates.Count; i < n; i++)
                    {
                        var agg = node.Aggregates[i].Element;

                        _binder.CheckIsScalar(agg.Expression, diagnostics);

                        if (agg.DefaultExpression != null)
                        {
                            _binder.CheckIsLiteral(agg.DefaultExpression, diagnostics);
                            _binder.CheckIsType(agg.DefaultExpression, GetResultTypeOrError(agg.Expression), Conversion.Promotable, diagnostics);
                        }

                        _binder.CreateProjectionColumns(agg.Expression, builder, diagnostics, style: ProjectionStyle.Summarize, columnType: ScalarTypes.Dynamic);
                    }

                    if (node.OnClause != null)
                    {
                        _binder.CheckIsScalar(node.OnClause.Expression, diagnostics);
                        _binder.CreateProjectionColumns(node.OnClause.Expression, builder, diagnostics, columnType: ScalarTypes.Dynamic);

                        if (node.RangeClause is MakeSeriesInRangeClause inRangeClause)
                        {
                            if (!inRangeClause.ContainsSyntaxDiagnostics)
                            {
                                CheckArgumentCount(inRangeClause.Arguments.Expressions, 3, diagnostics);
                            }

                            if (inRangeClause.Arguments.Expressions.Count == 3)
                            {
                                _binder.CheckIsType(inRangeClause.Arguments.Expressions[0].Element, GetResultTypeOrError(node.OnClause.Expression), Conversion.Promotable, diagnostics);
                                _binder.CheckIsType(inRangeClause.Arguments.Expressions[1].Element, GetResultTypeOrError(node.OnClause.Expression), Conversion.Promotable, diagnostics);
                                _binder.CheckIsIntervalType(inRangeClause.Arguments.Expressions[2].Element, GetResultTypeOrError(node.OnClause.Expression), diagnostics);
                            }
                        }
                        else if (node.RangeClause is MakeSeriesFromToStepClause fromToClause)
                        {
                            if (fromToClause.MakeSeriesFromClause?.Expression != null)
                            {
                                _binder.CheckIsType(fromToClause.MakeSeriesFromClause.Expression, GetResultTypeOrError(node.OnClause.Expression), Conversion.Promotable, diagnostics);
                            }

                            if (fromToClause.MakeSeriesToClause?.Expression != null)
                            {
                                _binder.CheckIsType(fromToClause.MakeSeriesToClause.Expression, GetResultTypeOrError(node.OnClause.Expression), Conversion.Promotable, diagnostics);
                            }
                            _binder.CheckIsIntervalType(fromToClause.MakeSeriesStepClause.Expression, GetResultTypeOrError(node.OnClause.Expression), diagnostics);
                        }
                    }

                    var resultType = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(RowScopeOrEmpty)
                        .WithIsSorted(false);

                    return new SemanticInfo(resultType, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitMvExpandOperator(MvExpandOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    builder.AddRange(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty), doNotRepeat: true);

                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.MvExpandParameters, diagnostics);

                    for (int i = 0, n = node.Expressions.Count; i < n; i++)
                    {
                        var expr = node.Expressions[i].Element;

                        _binder.CheckIsDynamic(expr.Expression, diagnostics);

                        var newType = GetMvExpandResultType(node.Parameters, expr.Expression, expr.ToTypeOf);
                        _binder.CreateProjectionColumns(expr.Expression, builder, diagnostics, style: ProjectionStyle.Replace, columnType: newType);
                    }

                    if (node.Parameters.GetParameterNameValue(QueryOperatorParameters.WithItemIndex) is string indexName)
                    {
                        builder.Add(new ColumnSymbol(indexName, ScalarTypes.Long));
                    }

                    if (node.RowLimitClause != null)
                    {
                        _binder.CheckIsInteger(node.RowLimitClause.RowLimit, diagnostics);
                    }

                    var result = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(RowScopeOrEmpty);

                    return new SemanticInfo(result, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            private static TypeSymbol GetMvExpandResultType(SyntaxList<NamedParameter> parameters, Expression expression, ToTypeOfClause toTypeOf)
            {
                TypeSymbol newType;

                if (toTypeOf?.TypeOf?.ReferencedSymbol is TypeSymbol toTypeOfType)
                {
                    // to type of clause gives result type
                    newType = toTypeOfType;
                }
                else if (expression.ResultType is DynamicArraySymbol arrayType)
                {
                    // initial expression is known to be an array, so each item will be an element of the array
                    newType = TypeFacts.GetElementType(expression.ResultType);
                }
                else if (expression.ResultType is DynamicBagSymbol)
                {
                    var bagexpKind = parameters.GetParameterNameValue(QueryOperatorParameters.BagExpansion)
                                  ?? parameters.GetParameterNameValue(QueryOperatorParameters.Kind);

                    // initial expression is known to be a bag, so give better result type than just 'dynamic'.
                    if (bagexpKind == "array")
                    {
                        // each row of expansion of bag gets an array for each name:value pair containing the name and value.
                        newType = ScalarTypes.DynamicArray;
                    }
                    else
                    {
                        // each row of expansion of bag gets a small bag with one name:value
                        newType = ScalarTypes.DynamicBag;
                    }
                }
                else if (expression.ResultType is DynamicSymbol)
                {
                    // we don't actually know if this column is a bag or array,
                    // so must return type as dynamic.
                    newType = ScalarTypes.Dynamic;
                }
                else
                {
                    // not even dynamic?  Error case, just return column's type.
                    newType = expression.ResultType;
                }

                return newType;
            }

            public override SemanticInfo VisitMvExpandExpression(MvExpandExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    TypeSymbol colType = GetResultTypeOrError(node.Expression);

                    if (node.ToTypeOf != null)
                    {
                        colType = GetReferencedSymbol(node.ToTypeOf.TypeOf) as TypeSymbol ?? ErrorSymbol.Instance;
                    }

                    return new SemanticInfo(colType, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitMvApplyOperator(MvApplyOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    builder.AddRange(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty), doNotRepeat: true);

                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.MvApplyParameters, diagnostics);

                    for (int i = 0, n = node.Expressions.Count; i < n; i++)
                    {
                        var expr = node.Expressions[i].Element;

                        _binder.CheckIsDynamic(expr.Expression, diagnostics);

                        var newType = GetMvExpandResultType(node.Parameters, expr.Expression, expr.ToTypeOf);

                        _binder.CreateProjectionColumns(expr.Expression, builder, diagnostics, columnType: newType, style: ProjectionStyle.Replace);
                    }

                    if (node.Parameters.GetParameterNameValue(QueryOperatorParameters.WithItemIndex) is string indexName)
                    {
                        builder.Add(new ColumnSymbol(indexName, ScalarTypes.Long));
                    }

                    if (node.RowLimitClause != null)
                    {
                        _binder.CheckIsInteger(node.RowLimitClause.RowLimit, diagnostics);
                    }

                    // ignore Subquery value here (see TreeBinder)
                    // the schema returned here is used for the type flowing into the subquery
                    var result = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(RowScopeOrEmpty);

                    return new SemanticInfo(result, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitMvApplyExpression(MvApplyExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    TypeSymbol colType = node.Expression?.ResultType ?? ErrorSymbol.Instance;

                    if (node.ToTypeOf != null)
                    {
                        colType = node.ToTypeOf?.TypeOf?.ResultType as TypeSymbol ?? ErrorSymbol.Instance;
                    }

                    return new SemanticInfo(colType, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitMvApplySubqueryExpression(MvApplySubqueryExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckQueryOperators(node.Expression, KustoFacts.PostPipeOperatorKinds, diagnostics, allowContextualRoot: true);

                    var resultType = GetResultTypeOrError(node.Expression);
                    if (resultType is TableSymbol table)
                    {
                        resultType = new TableSymbol(_binder.GetDeclaredAndInferredColumns(table))
                            .WithInheritableProperties(table);
                    }

                    return new SemanticInfo(resultType, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitPrintOperator(PrintOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckFirstInPipe(node, diagnostics);

                    for (int i = 0, n = node.Expressions.Count; i < n; i++)
                    {
                        var expr = node.Expressions[i].Element;
                        _binder.CheckIsScalar(expr, diagnostics);

                        _binder.CreateProjectionColumns(expr, builder, diagnostics, style: ProjectionStyle.Print, columnName: "print_" + i);
                    }

                    var resultType = new TableSymbol(builder.GetProjection());
                    return new SemanticInfo(resultType, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitReduceByOperator(ReduceByOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.ReduceParameters, diagnostics);

                    _binder.CheckIsExactType(node.Expression, ScalarTypes.String, diagnostics);

                    if (node.With != null)
                    {
                        _binder.CheckQueryOperatorParameters(node.With.Parameters, QueryOperatorParameters.ReduceWithParameters, diagnostics);
                    }

                    TableSymbol resultType;

                    var kind = node.Parameters.GetParameterLiteralValue<string>(QueryOperatorParameters.Kind);
                    if (kind == "source")
                    {
                        _binder.GetDeclaredAndInferredColumns(this.RowScopeOrEmpty, columns);
                    }

                    columns.Add(new ColumnSymbol("Pattern", ScalarTypes.String, source: node.Expression));
                    columns.Add(new ColumnSymbol("Count", ScalarTypes.Long, source: node.Expression));
                    columns.Add(new ColumnSymbol("Representative", ScalarTypes.String, source: node.Expression));
                    resultType = new TableSymbol(columns);

                    return new SemanticInfo(resultType, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            public override SemanticInfo VisitRenderOperator(RenderOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckIsToken(node.ChartType, KustoFacts.ChartTypes, true, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.RenderParameters, diagnostics);

                    if (node.WithClause != null)
                    {
                        _binder.CheckQueryOperatorParameters(node.WithClause.Properties, QueryOperatorParameters.RenderWithProperties, diagnostics);
                    }

                    _binder.GetDeclaredAndInferredColumns(this.RowScopeOrEmpty, columns);

                    var resultTable = new TableSymbol(columns)
                        .WithInheritableProperties(RowScopeOrEmpty);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            private SemanticInfo ParseVisitCommon(QueryOperator node, Expression expression, SyntaxList<SyntaxNode> patterns, SyntaxList<NamedParameter> parameters)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                var declaredNames = s_stringSetPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty, columns);

                    _binder.CheckQueryOperatorParameters(parameters, QueryOperatorParameters.ParseParameters, diagnostics);
                    _binder.CheckIsScalar(expression, diagnostics);

                    for (int i = 0, n = patterns.Count; i < n; i++)
                    {
                        var part = patterns[i];

                        // check for legal pattern arrangment
                        switch (part.Kind)
                        {
                            case SyntaxKind.StringLiteralExpression:
                            case SyntaxKind.CompoundStringLiteralExpression:
                                break;

                            case SyntaxKind.StarExpression:
                                if (i < patterns.Count - 1)
                                {
                                    var nextPart = patterns[i + 1];
                                    if (nextPart.Kind != SyntaxKind.StringLiteralExpression && nextPart.Kind != SyntaxKind.CompoundStringLiteralExpression)
                                    {
                                        diagnostics.Add(DiagnosticFacts.GetParsePatternStringLiteralMustFollowStar().WithLocation(part));
                                    }
                                }

                                if (i > 0)
                                {
                                    var prevPart = patterns[i - 1];
                                    if (prevPart.Kind == SyntaxKind.NameDeclaration
                                        || prevPart.Kind == SyntaxKind.BracketedExpression
                                        || (prevPart is NameAndTypeDeclaration nat
                                            && nat.Type is PrimitiveTypeExpression pt
                                            && Binder.GetType(pt) == ScalarTypes.String))
                                    {
                                        diagnostics.Add(DiagnosticFacts.GetParsePatternUsingStarAfterStringColumnIsAmbiguous().WithLocation(part));
                                    }
                                }
                                break;

                            case SyntaxKind.NameDeclaration:
                            case SyntaxKind.NameAndTypeDeclaration:
                                if (i > 0)
                                {
                                    var prevPart = patterns[i - 1];
                                    if (prevPart.Kind != SyntaxKind.StringLiteralExpression && prevPart.Kind != SyntaxKind.CompoundStringLiteralExpression)
                                    {
                                        diagnostics.Add(DiagnosticFacts.GetParsePatternNameDoesNotFollowStringLiteral().WithLocation(part));
                                    }
                                }
                                break;

                            default:
                                // TODO: does this ever happen?
                                diagnostics.Add(DiagnosticFacts.GetInvalidPatternPart().WithLocation(part));
                                break;
                        }

                        // gather column declarations
                        switch (part)
                        {
                            case NameDeclaration nd:
                                if (DeclareColumnName(declaredNames, nd.SimpleName, diagnostics, nd))
                                {
                                    var col = new ColumnSymbol(nd.SimpleName, ScalarTypes.String, source: expression);
                                    columns.Add(col);
                                    _binder.SetSemanticInfo(nd, GetSemanticInfo(col));
                                }
                                break;

                            case NameAndTypeDeclaration nat:
                                if (nat.Type is PrimitiveTypeExpression pt
                                    && DeclareColumnName(declaredNames, nat.Name.SimpleName, diagnostics, nat.Name))
                                {
                                    var type = Binder.GetType(pt);
                                    var col = new ColumnSymbol(nat.Name.SimpleName, type, source: expression);
                                    columns.Add(col);
                                    _binder.SetSemanticInfo(nat.Name, GetSemanticInfo(col));
                                }
                                break;
                        }
                    }

                    var result = new TableSymbol(columns)
                        .WithInheritableProperties(RowScopeOrEmpty);

                    return new SemanticInfo(result, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                    s_stringSetPool.ReturnToPool(declaredNames);
                }
            }

            public override SemanticInfo VisitParseWhereOperator(ParseWhereOperator node)
            {
                return ParseVisitCommon(node, node.Expression, node.Patterns, node.Parameters);
            }

            public override SemanticInfo VisitParseOperator(ParseOperator node)
            {
                return ParseVisitCommon(node, node.Expression, node.Patterns, node.Parameters);
            }

            public override SemanticInfo VisitParseKvWithClause(ParseKvWithClause node)
            {
                return null;
            }

            public override SemanticInfo VisitParseKvOperator(ParseKvOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty, columns);
                    _binder.CheckIsScalar(node.Expression, diagnostics);

                    CreateColumnsFromRowSchema(node.Keys.Columns, columns, diagnostics);

                    if (node.WithClause != null)
                    {
                        _binder.CheckQueryOperatorParameters(node.WithClause.Properties, QueryOperatorParameters.ParseKvWithProperties, diagnostics);
                    }

                    var result = new TableSymbol(columns)
                        .WithInheritableProperties(RowScopeOrEmpty);

                    return new SemanticInfo(result, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            public override SemanticInfo VisitInvokeOperator(InvokeOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    return new SemanticInfo(GetResultTypeOrError(node.Function), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitEvaluateOperator(EvaluateOperator node)
            {
                var call = node.FunctionCall;
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();

                try
                {
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.EvaluateParameters, diagnostics);

                    if (node.Schema != null)
                    {
                        CreateColumnsFromRowSchema(node.Schema.Schema.Columns, columns);
                        return new SemanticInfo(new TableSymbol(columns), diagnostics);
                    }
                    else
                    {
                        return new SemanticInfo(GetResultTypeOrError(node.FunctionCall), diagnostics);
                    }
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            public override SemanticInfo VisitGetSchemaOperator(GetSchemaOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    if ((string)node.KindParameter?.Expression?.LiteralValue == "csl")
                    {
                        return s_GetSchemaAsCslInfo;
                    }
                    return s_GetSchemaInfo;
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            private static readonly TableSymbol s_GetSchemaSchema = new TableSymbol(
                new ColumnSymbol("ColumnName", ScalarTypes.String),
                new ColumnSymbol("ColumnOrdinal", ScalarTypes.Long),
                new ColumnSymbol("DataType", ScalarTypes.String),
                new ColumnSymbol("ColumnType", ScalarTypes.String));

            private static readonly SemanticInfo s_GetSchemaInfo = new SemanticInfo(s_GetSchemaSchema);

            private static readonly TableSymbol s_GetSchemaAsCslSchema = new TableSymbol(
                new ColumnSymbol("Schema", ScalarTypes.String));

            private static readonly SemanticInfo s_GetSchemaAsCslInfo = new SemanticInfo(s_GetSchemaAsCslSchema);


            public override SemanticInfo VisitScanOperator(ScanOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.ScanParameters, diagnostics);

                    // TODO: check other clauses here

                    _binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty, columns);

                    if (node.DeclareClause != null)
                    {
                        foreach (var element in node.DeclareClause.Declarations)
                        {
                            var decl = element.Element;
                            columns.Add(new ColumnSymbol(decl.NameAndType.Name.SimpleName, GetDeclaredType(decl.NameAndType.Type), source: decl.NameAndType.Name));
                        }
                    }

                    var matchIdParam = node.Parameters.FirstOrDefault(np => np.Name.SimpleName == QueryOperatorParameters.WithMatchId.Name);
                    var matchIdColumnName = (matchIdParam != null && matchIdParam.Expression is NameDeclaration matchNd) ? matchNd.SimpleName : "match_id";
                    columns.Add(new ColumnSymbol(matchIdColumnName, ScalarTypes.Long));

                    var resultTable = new TableSymbol(columns)
                        .WithInheritableProperties(RowScopeOrEmpty);

                    return new SemanticInfo(resultTable, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                }
            }

            public override SemanticInfo VisitScanOrderByClause(ScanOrderByClause node)
            {
                return null;
            }

            public override SemanticInfo VisitScanPartitionByClause(ScanPartitionByClause node)
            {
                return null;
            }

            public override SemanticInfo VisitScanDeclareClause(ScanDeclareClause node)
            {
                return null;
            }

            public override SemanticInfo VisitScanAssignment(ScanAssignment node)
            {
                return null;
            }

            public override SemanticInfo VisitScanStep(ScanStep node)
            {
                return null;
            }

            public override SemanticInfo VisitScanStepOutput(ScanStepOutput node)
            {
                return null;
            }

            public override SemanticInfo VisitScanComputationClause(ScanComputationClause node)
            {
                return null;
            }

            public override SemanticInfo VisitMacroExpandScopeReferenceName(MacroExpandScopeReferenceName node)
            {
                // set symbol for scope reference declaration
                if (node.Parent is MacroExpandOperator macroExpand
                    && macroExpand.EntityGroup?.ResultType is EntityGroupSymbol entityGroup)
                {
                    var scopeSymbol = new EntityGroupElementSymbol(
                        node.EntityGroupReferenceName.SimpleName,
                        entityGroup);
                    _binder.SetSemanticInfo(node.EntityGroupReferenceName, new SemanticInfo(scopeSymbol, scopeSymbol));
                }

                return null;
            }

            public override SemanticInfo VisitMacroExpandOperator(MacroExpandOperator node)
            {
                // handled in TreeBinder
                return null;
            }

            public override SemanticInfo VisitMakeGraphOperator(MakeGraphOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                List<TableSymbol> nodesShape = null;
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckIsColumn(node.SourceColumn, diagnostics);
                    _binder.CheckIsColumn(node.TargetColumn, diagnostics);

                    if (!node.SourceColumn.ResultType.IsAnyScalarExceptDynamic())
                    {
                        diagnostics.Add(DiagnosticFacts.GetMakeGraphDynamicNodeIdColumnNotSupported().WithLocation(node.SourceColumn));
                    }

                    if (!node.TargetColumn.ResultType.IsAnyScalarExceptDynamic())
                    {
                        diagnostics.Add(DiagnosticFacts.GetMakeGraphDynamicNodeIdColumnNotSupported().WithLocation(node.TargetColumn));
                    }

                    if (node.WithClause is MakeGraphWithTablesAndKeysClause tablesAndKeysClause)
                    {
                        nodesShape = s_tableListPool.AllocateFromPool();
                        for (int i = 0; i < tablesAndKeysClause.TablesAndKeys.Count; i++)
                        {
                            var tableAndKey = tablesAndKeysClause.TablesAndKeys[i].Element;
                            _binder.CheckIsTabular(tableAndKey.Table, diagnostics);
                            if (!tableAndKey.Column.ResultType.IsAnyScalarExceptDynamic())
                            {
                                diagnostics.Add(DiagnosticFacts.GetMakeGraphDynamicNodeIdColumnNotSupported().WithLocation(tableAndKey.Column));
                            }

                            if (tableAndKey.Table.ResultType is TableSymbol table)
                            {
                                nodesShape.Add(table);
                            }
                        }
                    }

                    else if (node.WithClause is MakeGraphWithImplicitIdClause implicitIdClause)
                    {
                        nodesShape = s_tableListPool.AllocateFromPool();
                        if (string.IsNullOrEmpty(implicitIdClause.Name.SimpleName))
                        {
                            diagnostics.Add(DiagnosticFacts.GetMakeGraphImplicityIdShouldNotBeEmpty().WithLocation(implicitIdClause.Name));
                        }
                        // Create a symbol representing a "table" with only the implicit node id.
                        var table = new TableSymbol(new ColumnSymbol(implicitIdClause.Name.SimpleName, node.SourceColumn.ResultType));
                        nodesShape.Add(table);
                    }
                    
                    // Note that we don't handled the PartitionedByClause here but rather in the dedicated Visit* method.

                    var symbol = new GraphSymbol(this.RowScopeOrEmpty, nodesShape);
                    return new SemanticInfo(symbol, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    if (nodesShape != null)
                    {
                        s_tableListPool.ReturnToPool(nodesShape);
                    }
                }
            }

            public override SemanticInfo VisitMakeGraphWithTablesAndKeysClause(MakeGraphWithTablesAndKeysClause node)
            {
                // handled by VisitMakeGraphOperator
                return null;
            }

            public override SemanticInfo VisitMakeGraphWithImplicitIdClause(MakeGraphWithImplicitIdClause node)
            {
                // handled by VisitMakeGraphOperator
                return null;
            }

            public override SemanticInfo VisitMakeGraphTableAndKeyClause(MakeGraphTableAndKeyClause node)
            {
                // handled by VisitMakeGraphOperator
                return null;
            }

            public override SemanticInfo VisitMakeGraphPartitionedByClause(MakeGraphPartitionedByClause node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    _binder.CheckIsColumn(node.Entity, diagnostics);
                    CheckQueryOperators(node.Subquery, KustoFacts.PostPipeOperatorKinds, diagnostics, allowContextualRoot: true);

                    var tableType = node.Subquery.ResultType as TableSymbol;

                    var result = new TableSymbol(_binder.GetDeclaredAndInferredColumns(tableType))
                                    .WithInheritableProperties(RowScopeOrEmpty)
                                    .WithIsSorted(false);

                    return new SemanticInfo(result, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitGraphMatchOperator(GraphMatchOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    TypeSymbol symbol = null;

                    var leftGraph = GetGraphSymbol(node);
                    if (leftGraph == null)
                    {
                        diagnostics.Add(DiagnosticFacts.GetQueryOperatorExpectsGraph().WithLocation(node.GraphMatchKeyword));
                    }

                    if (node.Patterns == null || node.Patterns.Count == 0)
                    {
                        diagnostics.Add(DiagnosticFacts.GetMissingGraphMatchPattern().WithLocation(node.Patterns));
                    }
                    else
                    {
                        foreach (var pattern in node.Patterns)
                        {
                            CheckGraphMatchPattern(pattern.Element, diagnostics);
                        }
                    }

                    if (node.WhereClause != null)
                    {
                        _binder.CheckIsExactType(node.WhereClause.Condition, ScalarTypes.Bool, diagnostics);
                    }

                    if (node.ProjectClause != null)
                    {
                        // Getting all edges that are variable edges and has name
                        var variableEdges = new HashSet<string>();
                        node.Patterns.WalkElements(element =>
                        {
                            if (element is GraphMatchPatternEdge edge && edge.Range != null && edge.Name != null)
                            {
                                variableEdges.Add(edge.Name.SimpleName);
                            }
                        });

                        foreach (var expr in node.ProjectClause.Expressions)
                        {
                            TypeSymbol columnType = null;
                            var referencedElements = new HashSet<string>();
                            expr.Element.WalkNodes(elementNode =>
                            {
                                if (elementNode is NameReference nameRef)
                                {
                                    referencedElements.Add(nameRef.SimpleName);
                                }
                            });
                            if (variableEdges.Any(e => referencedElements.Contains(e)))
                            {
                                var colType = GetResultTypeOrError(expr.Element);
                                columnType = ScalarTypes.GetDynamicArray(colType);
                            }

                            _binder.CreateProjectionColumns(expr.Element, builder, diagnostics, ProjectionStyle.GraphMatch, columnType: columnType);
                        }

                        symbol = new TableSymbol(builder.GetProjection());
                    }
                    else
                    {
                        symbol = (TypeSymbol)leftGraph ?? ErrorSymbol.Instance;
                    }

                    return new SemanticInfo(symbol, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override SemanticInfo VisitGraphShortestPathsOperator(GraphShortestPathsOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    TypeSymbol symbol = null;

                    var leftGraph = GetGraphSymbol(node);
                    if (leftGraph == null)
                    {
                        diagnostics.Add(DiagnosticFacts.GetQueryOperatorExpectsGraph().WithLocation(node.GraphShortestPathsKeyword));
                    }

                    if (node.Patterns == null || node.Patterns.Count == 0)
                    {
                        diagnostics.Add(DiagnosticFacts.GetMissingGraphMatchPattern().WithLocation(node.Patterns));
                    }
                    else
                    {
                        foreach (var pattern in node.Patterns)
                        {
                            CheckGraphMatchPattern(pattern.Element, diagnostics);
                        }
                    }

                    if (node.WhereClause != null)
                    {
                        _binder.CheckIsExactType(node.WhereClause.Condition, ScalarTypes.Bool, diagnostics);
                    }

                    if (node.ProjectClause != null)
                    {
                        // Getting all edges that are variable edges and has name
                        var variableEdges = new HashSet<string>();
                        node.Patterns.WalkElements(element =>
                        {
                            if (element is GraphMatchPatternEdge edge && edge.Range != null && edge.Name != null)
                            {
                                variableEdges.Add(edge.Name.SimpleName);
                            }
                        });

                        foreach (var expr in node.ProjectClause.Expressions)
                        {
                            TypeSymbol columnType = null;
                            var referencedElements = new HashSet<string>();
                            expr.Element.WalkNodes(elementNode =>
                            {
                                if (elementNode is NameReference nameRef)
                                {
                                    referencedElements.Add(nameRef.SimpleName);
                                }
                            });
                            if (variableEdges.Any(e => referencedElements.Contains(e)))
                            {
                                var colType = GetResultTypeOrError(expr.Element);
                                columnType = ScalarTypes.GetDynamicArray(colType);
                            }

                            _binder.CreateProjectionColumns(expr.Element, builder, diagnostics, ProjectionStyle.GraphMatch, columnType: columnType);
                        }

                        symbol = new TableSymbol(builder.GetProjection());
                    }
                    else
                    {
                        symbol = (TypeSymbol)leftGraph ?? ErrorSymbol.Instance;
                    }

                    return new SemanticInfo(symbol, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            private void CheckGraphMatchPattern(GraphMatchPattern node, List<Diagnostic> diagnostics)
            {
                if (node.PatternElements == null || node.PatternElements.Count == 0)
                {
                    diagnostics.Add(DiagnosticFacts.GetMissingGraphMatchPatternElement().WithLocation(node));
                    return;
                }

                for (int i = 0; i < node.PatternElements.Count; i++)
                {
                    var element = node.PatternElements[i];
                    var expectNode = i % 2 == 0;

                    // Validate every element is the expected type
                    if (expectNode && !(element is GraphMatchPatternNode))
                    {
                        diagnostics.Add(DiagnosticFacts.GetGraphMatchPatternSyntaxError("node", "edge").WithLocation(element));
                    }
                    else if (!expectNode && !(element is GraphMatchPatternEdge))
                    {
                        diagnostics.Add(DiagnosticFacts.GetGraphMatchPatternSyntaxError("edge", "node").WithLocation(element));
                    }
                }

                // Validating pattern ends with node
                var lastElement = node.PatternElements[node.PatternElements.Count - 1];
                if (!(lastElement is GraphMatchPatternNode))
                {
                    diagnostics.Add(DiagnosticFacts.GetMissingGraphMatchPatternElement().WithLocation(lastElement));
                }
            }

            public override SemanticInfo VisitGraphMatchPattern(GraphMatchPattern node)
            {
                // handled by VisitGraphMatchOperator
                return null;
            }

            public override SemanticInfo VisitGraphMatchPatternNode(GraphMatchPatternNode node)
            {
                // handled by VisitGraphMatchOperator
                return null;
            }

            public override SemanticInfo VisitGraphMatchPatternEdge(GraphMatchPatternEdge node)
            {
                // handled by VisitGraphMatchOperator
                return null;
            }

            public override SemanticInfo VisitGraphMatchPatternEdgeRange(GraphMatchPatternEdgeRange node)
            {
                // handled by VisitGraphMatchOperator
                return null;
            }

            public override SemanticInfo VisitWhereClause(WhereClause node)
            {
                // handled by containing node
                return null;
            }

            public override SemanticInfo VisitProjectClause(ProjectClause node)
            {
                // handled by containing node
                return null;
            }

            public override SemanticInfo VisitGraphMarkComponentsOperator(GraphMarkComponentsOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    if (node.Parameters != null)
                    {
                        _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.GraphMarkComponentsParameters, diagnostics);
                    }

                    // get existing graph symbol from left-side of parent pipe operator
                    var graphSymbol = (node.Parent as PipeExpression)?.Expression?.ResultType is GraphSymbol g
                        ? new GraphSymbol(g.EdgeShape, g.NodeShape)
                        : new GraphSymbol(this.RowScopeOrEmpty);

                    // add component-id column to node shape
                    var componentIdName = node.Parameters.GetParameterNameValue(QueryOperatorParameters.WithComponentId) as string ?? "ComponentId";
                    var componentIdColumn = new ColumnSymbol(componentIdName, ScalarTypes.Long);
                    var newNodeShape = graphSymbol.NodeShape != null
                        ? graphSymbol.NodeShape.AddColumns(componentIdColumn)
                        : new TableSymbol(new[] { componentIdColumn });
                    graphSymbol = graphSymbol.WithNodeShape(newNodeShape);

                    return new SemanticInfo(graphSymbol, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitGraphWhereNodesOperator(GraphWhereNodesOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckIsExactType(node.Condition, ScalarTypes.Bool, diagnostics);

                    // get existing graph symbol from left-side of parent pipe operator
                    var graphSymbol = (node.Parent as PipeExpression)?.Expression?.ResultType is GraphSymbol g
                        ? new GraphSymbol(g.EdgeShape, g.NodeShape)
                        : new GraphSymbol(this.RowScopeOrEmpty);

                    return new SemanticInfo(graphSymbol, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitGraphWhereEdgesOperator(GraphWhereEdgesOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckIsExactType(node.Condition, ScalarTypes.Bool, diagnostics);

                    // get existing graph symbol from left-side of parent pipe operator
                    var graphSymbol = (node.Parent as PipeExpression)?.Expression?.ResultType is GraphSymbol g
                        ? new GraphSymbol(g.EdgeShape, g.NodeShape)
                        : new GraphSymbol(this.RowScopeOrEmpty);

                    return new SemanticInfo(graphSymbol, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitGraphToTableOperator(GraphToTableOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    TypeSymbol symbol = null;

                    var leftGraph = GetGraphSymbol(node);
                    if (leftGraph == null)
                    {
                        diagnostics.Add(DiagnosticFacts.GetQueryOperatorExpectsGraph().WithLocation(node.GraphToTableKeyword));
                    }
                    else if (node.OutputClause.Count == 1)
                    {
                        var clause = node.OutputClause[0].Element;
                        if (clause.EntityKeyword.Kind == SyntaxKind.GraphEdgesKeyword)
                        {
                            symbol = VisitGraphToTableEdgesClause(clause, leftGraph);
                        }
                        else if (clause.EntityKeyword.Kind == SyntaxKind.NodesKeyword)
                        {
                            symbol = VisitGraphToTableNodesClause(clause, leftGraph);
                        }
                    }
                    else if (node.OutputClause.Count == 2)
                    {

                        var nodesTableClause = node.OutputClause.FirstOrDefault(oc => oc.Element.EntityKeyword.Kind == SyntaxKind.NodesKeyword)?.Element;
                        var edgesTableClause = node.OutputClause.FirstOrDefault(oc => oc.Element.EntityKeyword.Kind == SyntaxKind.GraphEdgesKeyword)?.Element;
                        if (nodesTableClause == null || edgesTableClause == null)
                        {
                            diagnostics.Add(DiagnosticFacts.GetMissingGraphEntityType().WithLocation(node.OutputClause));
                            return new SemanticInfo(diagnostics);
                        }

                        var nodesTable = VisitGraphToTableNodesClause(nodesTableClause, leftGraph);
                        var edgesTable = VisitGraphToTableEdgesClause(edgesTableClause, leftGraph);
                        symbol = new GroupSymbol(nodesTable, edgesTable);
                    }
                    else
                    {
                        diagnostics.Add(DiagnosticFacts.GetIncorrectNumberOfOutputGraphEntities().WithLocation(node.OutputClause));
                    }

                    return new SemanticInfo(symbol, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            private TableSymbol VisitGraphToTableEdgesClause(GraphToTableOutputClause node, GraphSymbol graph)
            {
                var edges = graph?.EdgeShape ?? this.RowScopeOrEmpty;
                var name = node.AsClause?.Name?.SimpleName;
                var columns = new List<ColumnSymbol>(edges.Columns.Count);
                AddGraphToTableHashColumn(node, columns, QueryOperatorParameters.WithSourceId);
                AddGraphToTableHashColumn(node, columns, QueryOperatorParameters.WithTargetId);
                columns.AddRange(edges.Columns);

                return new TableSymbol(name, columns);
            }

            private TableSymbol VisitGraphToTableNodesClause(GraphToTableOutputClause node, GraphSymbol graph)
            {
                var nodes = graph?.NodeShape ?? this.RowScopeOrEmpty;
                var name = node.AsClause?.Name?.SimpleName;
                var columns = new List<ColumnSymbol>(nodes.Columns.Count);
                AddGraphToTableHashColumn(node, columns, QueryOperatorParameters.WithNodeId);
                columns.AddRange(nodes.Columns);

                return new TableSymbol(name, columns);
            }

            private void AddGraphToTableHashColumn(GraphToTableOutputClause node, List<ColumnSymbol> columns, QueryOperatorParameter parameter)
            {
                var hashColumn = node.Parameters.GetParameterNameValue(parameter);
                if (!string.IsNullOrEmpty(hashColumn))
                {
                    columns.Add(new ColumnSymbol(hashColumn, ScalarTypes.Long));
                }
            }

            public override SemanticInfo VisitGraphToTableOutputClause(GraphToTableOutputClause node)
            {
                // handled by containing node
                return null;
            }

            public override SemanticInfo VisitGraphToTableAsClause(GraphToTableAsClause node)
            {
                // handled by containing node
                return null;
            }
            #endregion

            #region clauses 
            // Clauses don't have semantics on their own but may influence their parent node's semantics
            // typically handled by the parent node's visit method.

            public override SemanticInfo VisitCountAsIdentifierClause(CountAsIdentifierClause node)
            {
                return null;
            }

            public override SemanticInfo VisitDataScopeClause(DataScopeClause node)
            {
                return null;
            }

            public override SemanticInfo VisitDefaultExpressionClause(DefaultExpressionClause node)
            {
                return null;
            }

            public override SemanticInfo VisitExternalDataWithClause(ExternalDataWithClause node)
            {
                return null;
            }

            public override SemanticInfo VisitInlineExternalTableKindClause(InlineExternalTableKindClause node)
            {
                return null;
            }

            public override SemanticInfo VisitInlineExternalTablePathFormatPartitionColumnReference(InlineExternalTablePathFormatPartitionColumnReference node)
            {
                return null;
            }

            public override SemanticInfo VisitInlineExternalTableDataFormatClause(InlineExternalTableDataFormatClause node)
            {
                return null;
            }

            public override SemanticInfo VisitInlineExternalTablePathFormatClause(InlineExternalTablePathFormatClause node)
            {
                return null;
            }

            public override SemanticInfo VisitPartitionColumnDeclaration(PartitionColumnDeclaration node)
            {
                return null;
            }

            public override SemanticInfo VisitInlineExternalTablePartitionClause(InlineExternalTablePartitionClause node)
            {
                return null;
            }

            public override SemanticInfo VisitInlineExternalTableConnectionStringsClause(InlineExternalTableConnectionStringsClause node)
            {
                return null;
            }

            public override SemanticInfo VisitFacetWithOperatorClause(FacetWithOperatorClause node)
            {
                return null;
            }

            public override SemanticInfo VisitFacetWithExpressionClause(FacetWithExpressionClause node)
            {
                return null;
            }

            public override SemanticInfo VisitFindInClause(FindInClause node)
            {
                return null;
            }

            public override SemanticInfo VisitFindProjectClause(FindProjectClause node)
            {
                return null;
            }

            public override SemanticInfo VisitJoinOnClause(JoinOnClause node)
            {
                return null;
            }

            public override SemanticInfo VisitJoinWhereClause(JoinWhereClause node)
            {
                return null;
            }

            public override SemanticInfo VisitMakeSeriesByClause(MakeSeriesByClause node)
            {
                return null;
            }

            public override SemanticInfo VisitMakeSeriesInRangeClause(MakeSeriesInRangeClause node)
            {
                return null;
            }
            public override SemanticInfo VisitMakeSeriesFromClause(MakeSeriesFromClause node)
            {
                return null;
            }

            public override SemanticInfo VisitMakeSeriesToClause(MakeSeriesToClause node)
            {
                return null;
            }

            public override SemanticInfo VisitMakeSeriesStepClause(MakeSeriesStepClause node)
            {
                return null;
            }

            public override SemanticInfo VisitMakeSeriesFromToStepClause(MakeSeriesFromToStepClause node)
            {
                return null;
            }

            public override SemanticInfo VisitMakeSeriesOnClause(MakeSeriesOnClause node)
            {
                return null;
            }

            public override SemanticInfo VisitMvExpandRowLimitClause(MvExpandRowLimitClause node)
            {
                return null;
            }

            public override SemanticInfo VisitMvApplyRowLimitClause(MvApplyRowLimitClause node)
            {
                return null;
            }

            public override SemanticInfo VisitMvApplyContextIdClause(MvApplyContextIdClause node)
            {
                return null;
            }

            public override SemanticInfo VisitNameEqualsClause(NameEqualsClause node)
            {
                return null;
            }

            public override SemanticInfo VisitOrderingClause(OrderingClause node)
            {
                return null;
            }

            public override SemanticInfo VisitOrderingNullsClause(OrderingNullsClause node)
            {
                return null;
            }

            public override SemanticInfo VisitReduceByWithClause(ReduceByWithClause node)
            {
                return null;
            }

            public override SemanticInfo VisitRenderWithClause(RenderWithClause node)
            {
                return null;
            }

            public override SemanticInfo VisitSummarizeByClause(SummarizeByClause node)
            {
                return null;
            }

            public override SemanticInfo VisitTopHittersByClause(TopHittersByClause node)
            {
                return null;
            }

            public override SemanticInfo VisitTopNestedClause(TopNestedClause node)
            {
                return null;
            }

            public override SemanticInfo VisitTopNestedWithOthersClause(TopNestedWithOthersClause node)
            {
                return null;
            }

            public override SemanticInfo VisitToTypeOfClause(ToTypeOfClause node)
            {
                return null;
            }

            public override SemanticInfo VisitEvaluateSchemaClause(EvaluateSchemaClause node)
            {
                return null;
            }
            #endregion

            #region statements
            public override SemanticInfo VisitAliasStatement(AliasStatement node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    if (!GetResultTypeOrError(node.Expression).IsError)
                    {
                        _binder.CheckIsDatabase(node.Expression, diagnostics);
                    }
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }

                return null;
            }

            public override SemanticInfo VisitExpressionStatement(ExpressionStatement node)
            {
                return null;
            }

            public override SemanticInfo VisitLetStatement(LetStatement node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    var name = node.Name.Name.SimpleName;

                    if (_binder._localScope.HasSymbol(name))
                    {
                        diagnostics.Add(DiagnosticFacts.GetVariableAlreadyDeclared(name).WithLocation(node.Name));
                    }

                    if (GetResultType(node.Expression) is TupleSymbol ts)
                    {
                        diagnostics.Add(DiagnosticFacts.GetMultiValuedExpressionCannotBeAssignedToVariable().WithLocation(node.Expression));
                    }

                    if (diagnostics.Count > 0)
                    {
                        return new SemanticInfo(diagnostics);
                    }

                    return null;
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitQueryParametersStatement(QueryParametersStatement node)
            {
                return null;
            }

            public override SemanticInfo VisitRestrictStatement(RestrictStatement node)
            {
                return null;
            }

            public override SemanticInfo VisitSetOptionStatement(SetOptionStatement node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    _binder.CheckIsIdentifierNameDeclaration(node.Name, diagnostics);

                    var option = _binder._globals.GetOption(node.Name.SimpleName);
                    if (option != null)
                    {
                        _binder.SetSemanticInfo(node.Name, new SemanticInfo(option, (TypeSymbol)null));
                    }

                    if (node.ValueClause != null)
                    {
                        if (_binder.CheckIsLiteralOrName(node.ValueClause.Expression, diagnostics))
                        {
                            if (option != null && option.Types.Count > 0)
                            {
                                _binder.CheckIsAnyType(node.ValueClause.Expression, option.Types, Conversion.Compatible, diagnostics);
                            }
                        }
                    }

                    if (diagnostics.Count > 0)
                    {
                        return new SemanticInfo(diagnostics);
                    }
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }

                return null;
            }

            public override SemanticInfo VisitOptionValueClause(OptionValueClause node)
            {
                // handled by VisitSetOptionStatement
                return null;
            }
            #endregion

            #region commands
            public override SemanticInfo VisitCommandWithValueClause(CommandWithValueClause node)
            {
                return null;
            }

            public override SemanticInfo VisitCommandWithPropertyListClause(CommandWithPropertyListClause node)
            {
                return null;
            }

            public override SemanticInfo VisitBadCommand(BadCommand node)
            {
                return null;
            }

            public override SemanticInfo VisitCommandAndSkippedTokens(CommandAndSkippedTokens node)
            {
                return null;
            }

            public override SemanticInfo VisitCommandBlock(CommandBlock node)
            {
                return null;
            }

            public override SemanticInfo VisitCustomCommand(CustomCommand node)
            {
                var commandSymbol = _binder._globals.GetCommand(node.CommandKind);
                if (commandSymbol != null)
                {
                    return new SemanticInfo(commandSymbol, commandSymbol.ResultType);
                }

                return null;
            }

            public override SemanticInfo VisitPartialCommand(PartialCommand node)
            {
                return null;
            }

            public override SemanticInfo VisitUnknownCommand(UnknownCommand node)
            {
                return null;
            }
            #endregion

            #region Directives
            public override SemanticInfo VisitDirectiveBlock(DirectiveBlock node)
            {
                // no longer used
                return null;
            }

            public override SemanticInfo VisitDirective(Directive node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    _binder.ApplyDirective(node, diagnostics);
                    return new SemanticInfo(diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitRestrictStatementWithClause(RestrictStatementWithClause node)
            {
                return null;
            }
            #endregion
        }
    }
}