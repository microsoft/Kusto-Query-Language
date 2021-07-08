using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Binding
{
    using Parsing;
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
                return new SemanticInfo(CreateFunctionSymbol(node));
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
                            if (_binder.CheckIsLiteralValue(node.DefaultValue.Value, diagnostics))
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

            private FunctionSymbol CreateFunctionSymbol(FunctionDeclaration decl)
            {
                // borrow name from parent variable (for debugging)
                string name;
                switch (decl.Parent)
                {
                    case LetStatement ls:
                        name = ls.Name.SimpleName;
                        break;
                    default:
                        name = "";
                        break;
                }

                var parameters = new List<Parameter>();

                // get parameter symbols already defined
                for (int i = 0; i < decl.Parameters.Parameters.Count; i++)
                {
                    var fp = decl.Parameters.Parameters[i].Element;

                    bool isOptional = fp.DefaultValue != null;

                    if (_binder.GetReferencedSymbol(fp.NameAndType.Name) is ParameterSymbol p)
                    {
                        parameters.Add(Parameter.From(p, isOptional, fp.DefaultValue?.Value));
                    }
                }

                var fs = new FunctionSymbol(name, decl.Body, parameters);

#if false  // TODO: check if we can add this back if we know it is invariant
                // add exiting declaration as default expansion
                var cs = _binder.GetCallSiteInfo(fs.Signatures[0], EmptyReadOnlyList<Expression>.Instance, EmptyReadOnlyList<TypeSymbol>.Instance);
                _binder._localBindingCache.CallSiteToExpansionMap.Add(cs, decl.Body);
                _binder.SetSignatureBindingInfo(fs.Signatures[0], decl.Body);
#endif
                return fs;
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
                    _binder.CheckIsTabular(node.Expression, diagnostics);
                    return new SemanticInfo(_binder.GetResultTypeOrError(node.Expression), diagnostics);
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
                    case SyntaxKind.NullLiteralExpression:
                        return VoidInfo;
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
                return LiteralDynamicInfo;
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

                    var resultType = _binder.GetResultType(node.Expression);
                    if (resultType is TableSymbol table)
                    {
                        if (table.Columns.Count > 0)
                        {
                            var col = table.Columns[0];
                            return new SemanticInfo(col.Type, dx);
                        }
                        else
                        {
                            dx.Add(DiagnosticFacts.GetTableHasNoColumns().WithLocation(node.Expression));
                            return new SemanticInfo(ErrorSymbol.Instance, dx);
                        }
                    }
                    else if (resultType is ScalarSymbol)
                    {
                        return new SemanticInfo(resultType, dx);
                    }
                    else
                    {
                        dx.Add(DiagnosticFacts.GetTableOrScalarExpected().WithLocation(node.Expression));
                        return new SemanticInfo(ErrorSymbol.Instance, dx);
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
                        var table = (TableSymbol)_binder.GetResultType(node.Expression);
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
                        return VisitWildcardedNameReference(node, wc.Pattern);
                    case BracketedWildcardedName bwc:
                        return VisitWildcardedNameReference(node, bwc.Pattern);
                    case BracedName _:
                        // client parameter does not bind to anything
                        return new SemanticInfo(ScalarTypes.Unknown);
                    default:
                        throw new NotImplementedException();
                }
            }

            private SemanticInfo VisitWildcardedNameReference(NameReference node, SyntaxToken pattern)
            {
                var list = s_symbolListPool.AllocateFromPool();
                var filteredList = s_symbolListPool.AllocateFromPool();
                var matchingList = s_symbolListPool.AllocateFromPool();

                try
                {
                    var match = IsInTabularContext(node)
                        ? SymbolMatch.Table | SymbolMatch.Function | SymbolMatch.Local | SymbolMatch.Tabular
                        : SymbolMatch.Column | SymbolMatch.Function | SymbolMatch.Local | SymbolMatch.Scalar;

                    _binder.GetSymbolsInContext(node, match, IncludeFunctionKind.LocalFunctions | IncludeFunctionKind.DatabaseFunctions, list);

                    FilterVisibleSymbols(node, list, filteredList);

                    GetWildcardSymbols(pattern.Text, filteredList, matchingList);

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
                        return new SemanticInfo(ErrorSymbol.Instance, DiagnosticFacts.GetNameDoesNotReferToAnyKnownItem(pattern.Text).WithLocation(node));
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
                bool isInsideDatabaseFunctionDeclaration = _binder.IsInsideDatabaseFunctionDeclaration(location);

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
                                && isInsideDatabaseFunctionDeclaration)
                            {
                                // if inside database function declaration choose the table over the function
                                map[symbol.Name] = tab;
                            }
                            else if (symbol is FunctionSymbol fs2
                                && fs2.MinArgumentCount == 0
                                && existingSymbol is TableSymbol tab2
                                && _binder._currentDatabase.GetAnyTable(symbol.Name) != null
                                && !isInsideDatabaseFunctionDeclaration)
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
                    var indexerType = _binder.GetResultTypeOrError(node.Expression);

                    if (indexerType.IsError)
                    {
                        return ErrorInfo;
                    }

                    if (indexerType != ScalarTypes.String)
                    {
                        return new SemanticInfo(null, ErrorSymbol.Instance, DiagnosticFacts.GetExpressionMustHaveType(ScalarTypes.String).WithLocation(node.Expression));
                    }
                    else if (!node.Expression.IsLiteral)
                    {
                        // computed name lookup?? Is this valid here?
                        return new SemanticInfo(null, ErrorSymbol.Instance, DiagnosticFacts.GetExpressionMustBeLiteral().WithLocation(node.Expression));
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
                var indexerType = _binder.GetResultTypeOrError(selector.Expression);
                if (indexerType.IsError)
                {
                    return ErrorInfo;
                }

                var collectionType = collection.ResultType;
                if (collectionType == null || collectionType.IsError)
                {
                    return ErrorInfo;
                }
                else if (collectionType == ScalarTypes.Dynamic)
                {
                    if (!IsInteger(indexerType) && !IsStringOrDynamic(indexerType))
                    {
                        // must be a integer array index or a string member name index (dynamic okay?)
                        return new SemanticInfo(null, ScalarTypes.Dynamic, DiagnosticFacts.GetExpressionMustHaveType(ScalarTypes.Int, ScalarTypes.Long, ScalarTypes.String).WithLocation(selector.Expression));
                    }
                    else
                    {
                        // you've successfully accessed an element of a dynamic value: you get another dynamic value.
                        return new SemanticInfo(ScalarTypes.Dynamic);
                    }
                }
                else if (collectionType is TupleSymbol ts)
                {
                    if (IsInteger(indexerType) 
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
                    return new SemanticInfo(null, ErrorSymbol.Instance, DiagnosticFacts.GetTheElementAccessOperatorIsNotAllowedInThisContext().WithLocation(selector));
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
                return new SemanticInfo(_binder.GetReferencedSymbol(node.Selector), _binder.GetResultTypeOrError(node.Selector));
            }

            public override SemanticInfo VisitElementExpression(ElementExpression node)
            {
                // same as selector (without repeating diagnostics)
                return new SemanticInfo(_binder.GetReferencedSymbol(node.Selector), _binder.GetResultTypeOrError(node.Selector));
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
                return new SemanticInfo(null, _binder.GetResultTypeOrError(node.Expression));
            }

            public override SemanticInfo VisitOrderedExpression(OrderedExpression node)
            {
                return new SemanticInfo(_binder.GetReferencedSymbol(node.Expression), _binder.GetResultTypeOrError(node.Expression));
            }

            public override SemanticInfo VisitSimpleNamedExpression(SimpleNamedExpression node)
            {
                return new SemanticInfo(null, _binder.GetResultTypeOrError(node.Expression));
            }

            public override SemanticInfo VisitCompoundNamedExpression(CompoundNamedExpression node)
            {
                return new SemanticInfo(null, _binder.GetResultTypeOrError(node.Expression));
            }

            public override SemanticInfo VisitPipeExpression(PipeExpression node)
            {
                return new SemanticInfo(null, _binder.GetResultTypeOrError(node.Operator));
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

                    return new SemanticInfo(_binder.GetResultTypeOrError(node.Expression), diagnostics);
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
                var resultType = _binder.GetResultTypeOrError(node.Expression);

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
                var resultType = _binder.GetResultTypeOrError(node.Subquery);

                if (resultType is TableSymbol table)
                {
                    resultType = new TableSymbol(_binder.GetDeclaredAndInferredColumns(table))
                        .WithInheritableProperties(table);
                }

                return new SemanticInfo(resultType);
            }

            public override SemanticInfo VisitPartitionQuery(PartitionQuery node)
            {
                var resultType = _binder.GetResultTypeOrError(node.Query);

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
                return LiteralDynamicInfo;
            }

            public override SemanticInfo VisitJsonObjectExpression(JsonObjectExpression node)
            {
                return LiteralDynamicInfo;
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
                return new SemanticInfo(_binder.GetResultTypeOrError(node.Expression));
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
                return new SemanticInfo(_binder.GetResultTypeOrError(node.Column));
            }

            public override SemanticInfo VisitCustom(CustomNode node)
            {
                return null;
            }

            public override SemanticInfo VisitMaterializedViewCombineExpression(MaterializedViewCombineExpression node)
            {
                var resultType = _binder.GetResultTypeOrError(node.AggregationsClause.Expression);


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
            /// <summary>
            /// True if the query operator is on the right hand side of a pipe expression.
            /// </summary>
            private static bool IsSecondaryPipeOperator(QueryOperator queryOp)
            {
                return (queryOp.Parent is PipeExpression pe && pe.Operator == queryOp)
                    || IsChildOfPipeStartingExpression(queryOp);
            }

            private static bool IsChildOfPipeStartingExpression(Expression expr)
            {
                return (expr.Parent is ForkExpression fce && fce.Expression == expr)
                    || (expr.Parent is PartitionSubquery ps && ps.Subquery == expr)
                    || (expr.Parent is MvApplySubqueryExpression mvas && mvas.Expression == expr)
                    || (expr.Parent is FacetWithExpressionClause fwce && fwce.Expression == expr)
                    || (expr.Parent is Expression pe && IsChildOfPipeStartingExpression(pe))
                    || (expr.Parent is MaterializedViewCombineClause mvc && mvc.Parent is MaterializedViewCombineExpression mve && mve.AggregationsClause == mvc);
            }

            private void CheckFirstInPipe(QueryOperator queryOp, List<Diagnostic> diagnostics)
            {
                if (IsSecondaryPipeOperator(queryOp))
                {
                    diagnostics.Add(DiagnosticFacts.GetQueryOperatorMustBeFirst().WithLocation(queryOp.GetChild(0) ?? queryOp));
                }
            }

            private void CheckNotFirstInPipe(QueryOperator queryOp, List<Diagnostic> diagnostics)
            {
                if (!IsSecondaryPipeOperator(queryOp))
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
                    _binder.CheckIsColumn(node.OfExpression, diagnostics);

                    var name = GetExpressionResultName(node.OfExpression, "Column1");

                    var result = new TableSymbol(new ColumnSymbol(name, _binder.GetResultTypeOrError(node.OfExpression)))
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

                    // produces a table of one column
                    if (node.AsIdentifier != null)
                    {
                        var name = node.AsIdentifier.Identifier.Text;
                        return new SemanticInfo(new TableSymbol(new ColumnSymbol(name, ScalarTypes.Long)), diagnostics);
                    }
                    else
                    {
                        return new SemanticInfo(new TableSymbol(new ColumnSymbol("Count", ScalarTypes.Long)), diagnostics);
                    }
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

                    // Even that column order changes - it doesn't really matter right now
                    var resultTable = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(RowScopeOrEmpty)
                        .WithIsOpen(true);

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

                    if (node.ByClause != null)
                    {
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
                        builder.Add(new ColumnSymbol("approximate_sum_" + GetExpressionResultName(node.ByClause.Expression), _binder.GetResultTypeOrError(node.ByClause.Expression)));
                    }
                    else
                    {
                        builder.Add(new ColumnSymbol("approximate_count_" + GetExpressionResultName(node.OfExpression), ScalarTypes.Long));
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

                        var ofName = GetExpressionResultName(clause.OfExpression);
                        columns.Add(new ColumnSymbol(uniqueNames.GetOrAddName(ofName), _binder.GetResultTypeOrError(clause.OfExpression)));

                        var byName = GetExpressionDeclaredName(clause.ByExpression)
                            ?? "aggregated_" + ofName;

                        GetExpressionResultName(clause.ByExpression, null);

                        columns.Add(new ColumnSymbol(uniqueNames.GetOrAddName(byName), _binder.GetResultTypeOrError(clause.ByExpression)));
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

            private static readonly TableSymbol s_ConsumeStatsSchema = new TableSymbol(new ColumnSymbol("Stats", ScalarTypes.Dynamic));

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

            public override SemanticInfo VisitDataTableExpression(DataTableExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                var declaredNames = s_stringSetPool.AllocateFromPool();
                try
                {
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.DataTableParameters, diagnostics);
                    CreateColumnsFromSchema(node.Schema, columns, declaredNames, diagnostics);
                    _binder.CheckDataValueTypes(node.Values, columns, diagnostics);
                    return new SemanticInfo(new TableSymbol(columns), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                    s_columnListPool.ReturnToPool(columns);
                    s_stringSetPool.ReturnToPool(declaredNames);
                }
            }

            public override SemanticInfo VisitContextualDataTableExpression(ContextualDataTableExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                var declaredNames = s_stringSetPool.AllocateFromPool();
                try
                {
                    CreateColumnsFromSchema(node.Schema, columns, declaredNames, diagnostics);

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
                    s_stringSetPool.ReturnToPool(declaredNames);
                }
            }

            public override SemanticInfo VisitExternalDataExpression(ExternalDataExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                var declaredNames = s_stringSetPool.AllocateFromPool();
                try
                {
                    CreateColumnsFromSchema(node.Schema, columns, declaredNames, diagnostics);

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
                    s_stringSetPool.ReturnToPool(declaredNames);
                }
            }

            public override SemanticInfo VisitSortOperator(SortOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.SortParameters, diagnostics);

                    for (int i = 0, n = node.Expressions.Count; i < n; i++)
                    {
                        var expr = node.Expressions[i].Element;
                        _binder.CheckIsScalar(expr, diagnostics);
                    }

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

                        var tableType = _binder.GetResultType(expr) as TableSymbol;
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
                if (keyword != null && !validQueryOperators.Contains(queryOperator.Kind))
                {
                    diagnostics.Add(DiagnosticFacts.GetQueryOperatorNotAllowedInContext(keyword.Text).WithLocation(keyword));
                }
            }

            public override SemanticInfo VisitPartitionOperator(PartitionOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.PartitionParameters, diagnostics);

                    var operand = node.Operand;
                    _binder.CheckIsColumn(node.ByExpression, diagnostics);
                    _binder.CheckIsTabular(operand, diagnostics);

                    if (operand is PartitionSubquery ps)
                    {
                        CheckQueryOperators(ps.Subquery, KustoFacts.PostPipeOperatorKinds, diagnostics);
                    }

                    var tableType = _binder.GetResultType(operand) as TableSymbol;
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
                var refTables = s_tableListPool.AllocateFromPool();
                var colNameMap = s_stringSetPool.AllocateFromPool();

                try
                {
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.FindParameters, diagnostics);
                    _binder.CheckIsExactType(node.Condition, ScalarTypes.Bool, diagnostics);

                    var withSource = node.Parameters.GetParameter(QueryOperatorParameters.WithSource);
                    string sourceColumnName = (withSource != null) ? GetNameDeclarationName(withSource.Expression) ?? "source_" : "source_";
                    columns.Add(new ColumnSymbol(sourceColumnName, ScalarTypes.String));

                    var tables = _binder.GetFindTables(node);
                    var resultIsOpen = tables.Any(t => t.IsOpen);
                    var explicitPack = false;

                    if (node.Project == null || node.Project.ProjectKeyword.Kind == SyntaxKind.ProjectSmartKeyword)
                    {
                        // project-smart

                        // only consider tables that have column references
                        _binder.GetReferencedColumnsInTree(node.Condition, refColumns);

                        if (tables.Count == 1)
                        {
                            // only one table
                            refTables.AddRange(tables);
                        }
                        else if (ReferencesAllTables(node))
                        {
                            // * references all columns from all tables
                            refTables.AddRange(tables);
                        }
                        else
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

                        // check to see if there is a table that has extra columns that need to be packed
                        foreach (var c in columns)
                        {
                            colNameMap.Add(c.Name);
                        }

                        var packExtraColumns = false;

                        foreach (var t in refTables)
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
                                    packExtraColumns = true;
                                    break;
                                }
                            }
                        }

                        if (packExtraColumns)
                        {
                            columns.Add(new ColumnSymbol("pack_", ScalarTypes.Dynamic));
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
                                        columns.Add(new ColumnSymbol("pack_", ScalarTypes.Dynamic));
                                    }
                                    else
                                    {
                                        diagnostics.Add(DiagnosticFacts.GetPackMustBeLastItemInList().WithLocation(exp));
                                    }
                                    break;

                                case TypedColumnReference tc:
                                    _binder.CheckIsColumn(tc.Column, diagnostics);
                                    if (_binder.GetReferencedSymbol(tc.Column) is ColumnSymbol c)
                                    {
                                        var type = _binder.GetTypeFromTypeExpression(tc.Type, diagnostics);
                                        columns.Add(new ColumnSymbol(c.Name, type));
                                    }
                                    break;

                                case NameReference nr:
                                    _binder.CheckIsColumn(nr, diagnostics);
                                    if (_binder.GetReferencedSymbol(nr) is ColumnSymbol c2)
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
                                    if (_binder.GetReferencedSymbol(columnExp) is ColumnSymbol col)
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
                    s_tableListPool.ReturnToPool(refTables);
                    s_stringSetPool.ReturnToPool(colNameMap);
                }
            }

            private static bool ReferencesAllTables(FindOperator node)
            {
                if (node.Condition.GetFirstDescendantOrSelf<StarExpression>() != null)
                    return true;

                // any string literal at root or left/right of and/or is abbreviation of: * has <string-literal>
                return node.Condition.GetFirstDescendantOrSelf<LiteralExpression>(lt =>
                    lt.Kind == SyntaxKind.StringLiteralExpression
                    && (lt.Parent == node
                        || lt.Parent is BinaryExpression be &&
                           (be.Kind == SyntaxKind.AndExpression || be.Kind == SyntaxKind.OrExpression))) != null;
            }

            public override SemanticInfo VisitUnionOperator(UnionOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                var tables = s_tableListPool.AllocateFromPool();
                try
                {
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.UnionParameters, diagnostics);

                    var withSourceParameter = node.Parameters.GetParameter(QueryOperatorParameters.WithSource);
                    if (withSourceParameter != null)
                    {
                        var name = GetNameDeclarationName(withSourceParameter.Expression);
                        if (name != null)
                        {
                            columns.Add(new ColumnSymbol(name, ScalarTypes.String));
                        }
                    }

                    if (RowScopeOrEmpty != null)
                    {
                        tables.Add(RowScopeOrEmpty);
                    }

                    for (int i = 0, n = node.Expressions.Count; i < n; i++)
                    {
                        var expr = node.Expressions[i].Element;
                        _binder.CheckIsTabular(expr, diagnostics);
                        _binder.AddTables(_binder.GetResultType(expr), tables);
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

            public override SemanticInfo VisitLookupOperator(LookupOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                var columns = s_columnListPool.AllocateFromPool();
                var exprColumns = s_columnListPool.AllocateFromPool();
                var rightJoinColumns = s_columnListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.LookupParameters, diagnostics);

                    _binder.CheckIsTabular(node.Expression, diagnostics);

                    // check the lookup clause(s)
                    if (node.LookupClause is JoinOnClause joc)
                    {
                        CheckJoinOnClause(joc, diagnostics, null, rightJoinColumns);
                    }

                    // figure out the result type
                    columns.AddRange(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty));
                    var resultIsOpen = RowScopeOrEmpty.IsOpen;

                    var exprTable = _binder.GetResultType(node.Expression) as TableSymbol;
                    if (exprTable != null)
                    {
                        _binder.GetDeclaredAndInferredColumns(exprTable, exprColumns);
                        // only add expr columns that were not equated to a source column via the join-on expression
                        exprColumns.RemoveAll(c => rightJoinColumns.Contains(c));
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
                    s_columnListPool.ReturnToPool(rightJoinColumns);
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
                    }

                    var joinKindNode = node.Parameters.GetParameter(QueryOperatorParameters.Kind);
                    var joinKind = joinKindNode?.Expression is LiteralExpression lit ? lit.Token.ValueText : "";

                    var resultIsOpen = false;

                    // if not explicitly a right-anti/semi join, then add left-side columns
                    if (!IsRightAntiOrSemiJoin(joinKind))
                    {
                        // add left-side columns
                        columns.AddRange(_binder.GetDeclaredAndInferredColumns(RowScopeOrEmpty));
                        resultIsOpen |= RowScopeOrEmpty.IsOpen;
                    }

                    var exprTable = _binder.GetResultType(node.Expression) as TableSymbol;
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

            private void CheckJoinOnClause(
                JoinOnClause clause,
                List<Diagnostic> diagnostics,
                List<ColumnSymbol> leftColumns = null,
                List<ColumnSymbol> rightColumns = null)
            {
                for (int i = 0, n = clause.Expressions.Count; i < n; i++)
                {
                    var expr = clause.Expressions[i].Element;
                    CheckJoinOnExpression(expr, diagnostics, leftColumns, rightColumns);
                }
            }

            private void CheckJoinOnExpression(
                Expression condition, 
                List<Diagnostic> diagnostics,
                List<ColumnSymbol> leftColumns = null,
                List<ColumnSymbol> rightColumns = null)
            {
                condition = RemoveParenthesis(condition);

                if (condition is BinaryExpression be)
                {
                    if (be.Kind == SyntaxKind.EqualExpression)
                    {
                        if (CheckJoinEquality(be, diagnostics, out var leftMatchingColumn, out var rightMatchingColumn))
                        {
                            if (leftMatchingColumn != null && rightMatchingColumn != null)
                            {
                                leftColumns?.Add(leftMatchingColumn);
                                rightColumns?.Add(rightMatchingColumn);
                            }
                        }
                    }
                    else if (be.Kind == SyntaxKind.AndExpression)
                    {
                        CheckJoinOnExpression(be.Left, diagnostics, leftColumns, rightColumns);
                        CheckJoinOnExpression(be.Right, diagnostics, leftColumns, rightColumns);
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
                            if (leftColumn != null && rightColumn != null)
                            {
                                leftColumns?.Add(leftColumn);
                                rightColumns?.Add(rightColumn);
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
                    return true;
                }
                else
                {
                    diagnostics.Add(DiagnosticFacts.GetColumnMustExistOnBothSidesOfJoin(name.SimpleName).WithLocation(name));
                    return false;
                }
            }

            private bool CheckJoinEquality(
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

                if (CheckJoinEqualityOperand(be.Left, "$left", diagnostics, out leftColumn)
                    & CheckJoinEqualityOperand(be.Right, "$right", diagnostics, out rightColumn))
                {
                    return true;
                }

                return false;
            }

            private bool CheckJoinEqualityOperand(Expression operand, string prefix, List<Diagnostic> diagnostics, out ColumnSymbol column)
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
                    column = _binder.GetReferencedSymbol(operand) as ColumnSymbol;
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

                    var commonType = GetCommonScalarType(_binder.GetResultTypeOrError(node.From), _binder.GetResultTypeOrError(node.To), _binder.GetResultTypeOrError(node.Step)) ?? ErrorSymbol.Instance;

                    var name = node.Name.SimpleName;

                    var result = new TableSymbol(new ColumnSymbol(name, commonType));

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
                                tableType = _binder.GetResultType(c.Operator) as TableSymbol;
                                if (tableType != null)
                                {
                                    tables.Add(tableType);
                                }
                                break;

                            case FacetWithExpressionClause c:
                                _binder.CheckIsTabular(c.Expression, diagnostics);
                                tableType = _binder.GetResultType(c.Expression) as TableSymbol;
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
                        _binder.CheckIsColumn(expr, diagnostics);

                        var name = GetExpressionResultName(expr);
                        var tableName = i == 0 ? "Facet" : "Facet_" + (i + 1);

                        var table = new TableSymbol(
                                tableName,
                                new ColumnSymbol(name, _binder.GetResultTypeOrError(expr)),
                                new ColumnSymbol("count_" + name, ScalarTypes.Long))
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
                            _binder.CheckIsType(agg.DefaultExpression, _binder.GetResultTypeOrError(agg.Expression), Conversion.Promotable, diagnostics);
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
                                _binder.CheckIsType(inRangeClause.Arguments.Expressions[0].Element, _binder.GetResultTypeOrError(node.OnClause.Expression), Conversion.Promotable, diagnostics);
                                _binder.CheckIsType(inRangeClause.Arguments.Expressions[1].Element, _binder.GetResultTypeOrError(node.OnClause.Expression), Conversion.Promotable, diagnostics);
                                _binder.CheckIsIntervalType(inRangeClause.Arguments.Expressions[2].Element, _binder.GetResultTypeOrError(node.OnClause.Expression), diagnostics);
                            }
                        }
                        else if (node.RangeClause is MakeSeriesFromToStepClause fromToClause)
                        {
                            if (fromToClause.MakeSeriesFromClause?.Expression != null)
                            {
                                _binder.CheckIsType(fromToClause.MakeSeriesFromClause.Expression, _binder.GetResultTypeOrError(node.OnClause.Expression), Conversion.Promotable, diagnostics);
                            }

                            if (fromToClause.MakeSeriesToClause?.Expression != null)
                            {
                                _binder.CheckIsType(fromToClause.MakeSeriesToClause.Expression, _binder.GetResultTypeOrError(node.OnClause.Expression), Conversion.Promotable, diagnostics);
                            }
                            _binder.CheckIsIntervalType(fromToClause.MakeSeriesStepClause.Expression, _binder.GetResultTypeOrError(node.OnClause.Expression), diagnostics);
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

                        _binder.CheckIsExactType(expr.Expression, ScalarTypes.Dynamic, diagnostics);
                        TypeSymbol type = expr.ToTypeOf?.TypeOf?.ReferencedSymbol as TypeSymbol ?? expr.Expression.ResultType; 
                        _binder.CreateProjectionColumns(expr.Expression, builder, diagnostics, style: ProjectionStyle.Replace, columnType: type);
                    }

                    var itemIndex = node.Parameters.GetParameter(QueryOperatorParameters.WithItemIndex);
                    if (itemIndex != null)
                    {
                        var indexName = GetNameDeclarationName(itemIndex.Expression);
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

            public override SemanticInfo VisitMvExpandExpression(MvExpandExpression node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    TypeSymbol colType = _binder.GetResultTypeOrError(node.Expression);

                    if (node.ToTypeOf != null)
                    {
                        colType = _binder.GetReferencedSymbol(node.ToTypeOf.TypeOf) as TypeSymbol ?? ErrorSymbol.Instance;
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

                        _binder.CheckIsExactType(expr.Expression, ScalarTypes.Dynamic, diagnostics);
                        TypeSymbol type = expr.ToTypeOf?.TypeOf?.ReferencedSymbol as TypeSymbol ?? expr.Expression.ResultType;
                        _binder.CreateProjectionColumns(expr.Expression, builder, diagnostics, columnType: type, style: ProjectionStyle.Replace);
                    }

                    var itemIndex = node.Parameters.GetParameter(QueryOperatorParameters.WithItemIndex);
                    if (itemIndex != null)
                    {
                        var indexName = GetNameDeclarationName(itemIndex.Expression);
                        builder.Add(new ColumnSymbol(indexName, ScalarTypes.Long));
                    }

                    if (node.RowLimitClause != null)
                    {
                        _binder.CheckIsInteger(node.RowLimitClause.RowLimit, diagnostics);
                    }

                    // ignore Subquery value here (see TreeBinder)
                    // return info for type flowing into subquery
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

                    var resultType = _binder.GetResultTypeOrError(node.Expression);
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
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.ReduceParameters, diagnostics);

                    _binder.CheckIsExactType(node.Expression, ScalarTypes.String, diagnostics);

                    if (node.With != null)
                    {
                        _binder.CheckQueryOperatorParameters(node.With.Parameters, QueryOperatorParameters.ReduceWithParameters, diagnostics);
                    }

                    var resultType = new TableSymbol(s_ReduceColumns);
                    return new SemanticInfo(resultType, diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            private static readonly IReadOnlyList<ColumnSymbol> s_ReduceColumns = new[]
            {
                new ColumnSymbol("Pattern", ScalarTypes.String),
                new ColumnSymbol("Count", ScalarTypes.Long),
                new ColumnSymbol("Representative", ScalarTypes.String)
            };

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
                                    var col = new ColumnSymbol(nd.SimpleName, ScalarTypes.String);
                                    columns.Add(col);
                                    _binder.SetSemanticInfo(nd, GetSemanticInfo(col));
                                }
                                break;

                            case NameAndTypeDeclaration nat:
                                if (nat.Type is PrimitiveTypeExpression pt
                                    && DeclareColumnName(declaredNames, nat.Name.SimpleName, diagnostics, nat.Name))
                                {
                                    var type = Binder.GetType(pt);
                                    var col = new ColumnSymbol(nat.Name.SimpleName, type);
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

            public override SemanticInfo VisitInvokeOperator(InvokeOperator node)
            {
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    CheckNotFirstInPipe(node, diagnostics);

                    return new SemanticInfo(_binder.GetResultTypeOrError(node.Function), diagnostics);
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

                try
                {
                    _binder.CheckQueryOperatorParameters(node.Parameters, QueryOperatorParameters.EvaluateParameters, diagnostics);
                    return new SemanticInfo(_binder.GetResultTypeOrError(node.FunctionCall), diagnostics);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override SemanticInfo VisitGetSchemaOperator(GetSchemaOperator node)
            {
                return s_GetSchemaInfo;
            }

            private static readonly TableSymbol s_GetSchemaSchema = new TableSymbol(
                new ColumnSymbol("ColumnName", ScalarTypes.String),
                new ColumnSymbol("ColumnOrdinal", ScalarTypes.Long),
                new ColumnSymbol("DataType", ScalarTypes.String),
                new ColumnSymbol("ColumnType", ScalarTypes.String));

            private static readonly SemanticInfo s_GetSchemaInfo = new SemanticInfo(s_GetSchemaSchema);


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
                            columns.Add(new ColumnSymbol(decl.NameAndType.Name.SimpleName, GetDeclaredType(decl.NameAndType.Type)));
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

            public override SemanticInfo VisitScanComputationClause(ScanComputationClause node)
            {
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
                    if (!_binder.GetResultTypeOrError(node.Expression).IsError)
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

                    if (_binder._localScope.ContainsSymbol(name))
                    {
                        diagnostics.Add(DiagnosticFacts.GetVariableAlreadyDeclared(name).WithLocation(node.Name));
                    }

                    if (_binder.GetResultType(node.Expression) is TupleSymbol ts)
                    {
                        diagnostics.Add(DiagnosticFacts.GetMultiValuedExpressionCannotBeAssignedToVariable().WithLocation(node.Expression));
                    }

                    if (diagnostics.Count > 0)
                    {
                        return new SemanticInfo(null, null, diagnostics);
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
                        return new SemanticInfo(null, null, diagnostics);
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

            public override SemanticInfo VisitCommandBlock(CommandBlock node)
            {
                return null;
            }

            public override SemanticInfo VisitCustomCommand(CustomCommand node)
            {
                var commandSymbol = _binder._globals.GetCommand(node.CommandKind);
                return new SemanticInfo(commandSymbol, commandSymbol.ResultType);
            }

            public override SemanticInfo VisitUnknownCommand(UnknownCommand node)
            {
                return null;
            }
#endregion

#region Directives
            public override SemanticInfo VisitDirectiveBlock(DirectiveBlock node)
            {
                return null;
            }
#endregion
        }
    }
}