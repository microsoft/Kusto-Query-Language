using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;

namespace Kusto.Language.Binding
{
    internal partial class Binder
    {
        /// <summary>
        /// The <see cref="TreeBinder"/> is a <see cref="SyntaxVisitor"/> that orchestrates binding the entire syntax tree.
        /// From the top down, it adjusts the <see cref="Binder"/>'s state to determine symbols in scope for each node and its descendants, etc.
        /// From bottom up, it invokes the <see cref="NodeBinder"/> on each <see cref="SyntaxNode"/> to evalute its <see cref="SemanticInfo"/> if any.
        /// All child nodes are thus bound before any parent nodes.
        /// </summary>
        private class TreeBinder : DefaultSyntaxVisitor
        {
            private readonly Binder _binder;
            private readonly NodeBinder _nodeBinder;

            public TreeBinder(Binder binder)
            {
                _binder = binder;
                _nodeBinder = new NodeBinder(binder);
            }

            protected override void DefaultVisit(SyntaxNode node)
            {
                // first bind child nodes
                this.VisitChildren(node);

                // bind this node
                this.BindNode(node);
            }

            private void VisitChildren(SyntaxNode node)
            {
                if (node != null)
                {
                    for (int i = 0, n = node.ChildCount; i < n; i++)
                    {
                        var child = node.GetChild(i) as SyntaxNode;
                        if (child != null)
                        {
                            child.Accept(this);
                        }
                    }
                }
            }

            private void BindNode(SyntaxNode node)
            {
                _binder._cancellationToken.ThrowIfCancellationRequested();

                // use NodeBinder to determine semantic info for this node.
                var info = node.Accept(_nodeBinder);

                // remember semantic info
                _binder.SetSemanticInfo(node, info);
            }

            public override void VisitPathExpression(PathExpression node)
            {
                // bracketed expressions are not evaluated in scope of the left-hand side
                if (node.Selector is BracketedExpression)
                {
                    base.VisitPathExpression(node);
                    return;
                }
                else
                {
                    node.Expression.Accept(this);

                    // result type of left-side expression is in scope after the dot.
                    var oldPathScope = _binder._pathScope;
                    _binder._pathScope = GetResultTypeOrError(node.Expression);
                    try
                    {
                        node.Selector.Accept(this);
                    }
                    finally
                    {
                        _binder._pathScope = oldPathScope;
                    }

                    BindNode(node);
                }
            }

            public override void VisitDynamicExpression(DynamicExpression node)
            {
                _binder._dynamicDepth++;
                try
                {
                    base.VisitDynamicExpression(node);
                }
                finally
                {
                    _binder._dynamicDepth--;
                }
            }

            public override void VisitPipeExpression(PipeExpression node)
            {
                if (node.Operator is UnionOperator union)
                {
                    // set fuzziness of left side expression if operator is fuzzy union
                    var oldIsFuzzy = _binder._isFuzzy;
                    var isFuzzy = union.Parameters.GetParameterLiteralValue<bool?>(QueryOperatorParameters.IsFuzzy);
                    if (isFuzzy != null)
                        _binder._isFuzzy = isFuzzy.Value;
                    node.Expression.Accept(this);
                    _binder._isFuzzy = oldIsFuzzy;
                }
                else
                {
                    node.Expression.Accept(this);
                }

                // result of left-side expression is in scope for right-side query operator
                var oldRowScope = _binder._rowScope;
                var oldScopeKind = _binder._scopeKind;
                _binder._rowScope = GetResultType(node.Expression) as TableSymbol;
                _binder._scopeKind = ScopeKind.Normal;
                try
                {
                    node.Operator.Accept(this);
                }
                finally
                {
                    _binder._rowScope = oldRowScope;
                    _binder._scopeKind = oldScopeKind;
                }

                BindNode(node);
            }

            public override void VisitLookupOperator(LookupOperator node)
            {
                node.Parameters.Accept(this);

                // table expression should not see row scope...
                var oldRowScope = _binder._rowScope;
                _binder._rowScope = null;
                try
                {
                    node.Expression.Accept(this);
                }
                finally
                {
                    _binder._rowScope = oldRowScope;
                }

                // condition clause should see both left & right row scopes.
                _binder._rightRowScope = GetResultType(node.Expression) as TableSymbol;

                try
                {
                    node.LookupClause.Accept(this);

                    // allow right scope to stay for binding of lookup operator node too.
                    BindNode(node);
                }
                finally
                {
                    _binder._rightRowScope = null;
                }
            }

            public override void VisitJoinOperator(JoinOperator node)
            {
                node.Parameters.Accept(this);

                // table expression should not see row scope...
                var oldRowScope = _binder._rowScope;
                _binder._rowScope = null;
                try
                {
                    node.Expression.Accept(this);
                }
                finally
                {
                    _binder._rowScope = oldRowScope;
                }

                // condition clause should see both left & right row scopes.
                _binder._rightRowScope = GetResultType(node.Expression) as TableSymbol;
                try
                {
                    node.ConditionClause?.Accept(this);

                    // allow right scope to stay for binding of join operator node too.
                    BindNode(node);
                }
                finally
                {
                    _binder._rightRowScope = null;
                }
            }

            public override void VisitJoinOnClause(JoinOnClause node)
            {
                for (int i = 0; i < node.Expressions.Count; i++)
                {
                    VisitJoinOnExpression(node.Expressions[i].Element);
                }
            }

            private void VisitJoinOnExpression(Expression expr)
            {
                if (expr is BinaryExpression be && be.Kind == SyntaxKind.AndExpression)
                {
                    VisitJoinOnExpression(be.Left);
                    VisitJoinOnExpression(be.Right);
                    _binder.SetSemanticInfo(be, new SemanticInfo(ScalarTypes.Bool));
                }
                else
                {
                    _binder._commonColumnsOnly = true;
                    expr.Accept(this);
                    _binder._commonColumnsOnly = false;
                }
            }

            public override void VisitUnionOperator(UnionOperator node)
            {
                // union operator expressions do not bind to row scope columns (they are only tables)
                var oldRowScope = _binder._rowScope;
                _binder._rowScope = null;

                // set fuzziness of input evaluation
                var oldIsFuzzy = _binder._isFuzzy;
                var isFuzzy = node.Parameters.GetParameterLiteralValue<bool?>(QueryOperatorParameters.IsFuzzy);
                if (isFuzzy != null)
                    _binder._isFuzzy = isFuzzy.Value;

                VisitChildren(node);

                _binder._rowScope = oldRowScope;
                _binder._isFuzzy = oldIsFuzzy;

                BindNode(node);
            }

            public override void VisitSummarizeOperator(SummarizeOperator node)
            {
                node.Parameters.Accept(this);

                // visit by clause before aggregates so by expressions are already bound
                // when resolving aggregate expression result types.
                node.ByClause?.Accept(this);

                VisitInScopeKind(node.Aggregates, ScopeKind.Aggregate);

                BindNode(node);
            }

            public override void VisitMacroExpandOperator(MacroExpandOperator node)
            {
                // analyze parameters
                node.Parameters.Accept(this);

                // set fuzziness of entity evaluation
                var oldIsFuzzy = _binder._isFuzzy;
                var isFuzzy = node.Parameters.GetParameterLiteralValue<bool?>(QueryOperatorParameters.IsFuzzy);
                if (isFuzzy != null)
                {
                    _binder._isFuzzy = isFuzzy.Value;
                }

                // analyze entity group
                node.EntityGroup.Accept(this);
                _binder._isFuzzy = false;

                // define scope symbol
                node.ScopeReferenceName?.Accept(this);

                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    var resultTables = new List<TableSymbol>();
                    var alternateStatementLists = new List<SyntaxNode>();

                    var egSymbol = node.EntityGroup.ResultType as EntityGroupSymbol;
                    var declaredScope = node.ScopeReferenceName?.EntityGroupReferenceName?.ReferencedSymbol as EntityGroupElementSymbol;
                    var entityGroupNameReference = node.EntityGroup as NameReference;
                    var scopeName = 
                        node.ScopeReferenceName?.EntityGroupReferenceName?.SimpleName
                        ?? entityGroupNameReference?.SimpleName
                        ?? egSymbol?.Name
                        ?? "$scope";

                    var oldScope = _binder._localScope;

                    if (egSymbol != null)
                    {
                        // evaluate statement list once per entity group member
                        foreach (var entitySymbol in egSymbol.Members.OfType<TypeSymbol>())
                        {
                            _binder._localScope = new LocalScope(oldScope);

                            SyntaxList<SeparatedElement<Statement>> statements;

                            // the primary entity is the one associated with the declared scope
                            var isPrimaryEntity = declaredScope?.UnderlyingSymbol == entitySymbol;
                            if (isPrimaryEntity)
                            {
                                // associate declared scope with original statement list
                                _binder._localScope.AddSymbol(declaredScope);
                                statements = node.StatementList;
                            }
                            else
                            {
                                // use alternate scopes for other entity group members 
                                var alternateScope = new EntityGroupElementSymbol(scopeName, egSymbol, entitySymbol);
                                _binder._localScope.AddSymbol(alternateScope);

                                // make copy of statement list to hold alternate semantic evaluation
                                statements = node.StatementList.CopyAsFragment();

                                // remember alternate evaluated statement lists
                                alternateStatementLists.Add(statements);
                            }

                            statements.Accept(this);

                            if (GetFirstExpressionStatement(statements) is ExpressionStatement es)
                            {
                                _binder.CheckIsTabular(es.Expression, diagnostics);
                                if (GetResultType(es.Expression) is TableSymbol ts)
                                {
                                    resultTables.Add(ts);
                                }
                            }
                        }
                    }
                    else
                    {
                        // no valid entity group, so evaluate statements just once with temp scope.
                        if (declaredScope != null)
                        {
                            _binder._localScope.AddSymbol(declaredScope);
                        }
                        else
                        {
                            var tempScopeSymbol = new EntityGroupElementSymbol(scopeName);
                            _binder._localScope.AddSymbol(tempScopeSymbol);
                        }

                        node.StatementList.Accept(this);
                    }

                    _binder._localScope = oldScope;
                    _binder._isFuzzy = oldIsFuzzy;

                    var resultType = TableSymbol.Combine(CombineKind.UnifySameNameAndType, resultTables);
                    var info = new SemanticInfo(resultType, diagnostics);
                    _binder.SetSemanticInfo(node, info);

                    if (alternateStatementLists.Count > 0)
                    {
                        var statementsInfo = new SemanticInfo(null).WithAlternates(alternateStatementLists);
                        _binder.SetSemanticInfo(node.StatementList, statementsInfo);
                    }
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            private static ExpressionStatement GetFirstExpressionStatement(SyntaxList<SeparatedElement<Statement>> statements)
            {
                for (int i = 0; i < statements.Count; i++)
                {
                    if (statements[i].Element is ExpressionStatement es)
                    {
                        return es;
                    }
                }

                return null;
            }

            public override void VisitMakeSeriesOperator(MakeSeriesOperator node)
            {
                node.Parameters.Accept(this);
                VisitInScopeKind(node.Aggregates, ScopeKind.Aggregate);
                node.OnClause?.Accept(this);
                node.RangeClause?.Accept(this);
                node.ByClause?.Accept(this);

                BindNode(node);
            }

            private void VisitInRowScope(SyntaxNode node, TableSymbol rowScope)
            {
                if (node == null)
                    return;

                var oldRowScope = _binder._rowScope;
                _binder._rowScope = rowScope;
                node.Accept(this);
                _binder._rowScope = oldRowScope;
            }

            private void VisitInScopeKind(SyntaxNode node, ScopeKind kind)
            {
                if (node == null)
                    return;

                var oldScopeKind = _binder._scopeKind;
                _binder._scopeKind = kind;
                node.Accept(this);
                _binder._scopeKind = oldScopeKind;
            }

            public override void VisitTopNestedClause(TopNestedClause node)
            {
                node.Expression?.Accept(this);
                node.OfExpression.Accept(this);
                node.WithOthersClause?.Accept(this);
                VisitInScopeKind(node.ByExpression, ScopeKind.Aggregate);

                BindNode(node);
            }

            public override void VisitAsOperator(AsOperator node)
            {
                base.VisitAsOperator(node);

                if (_binder._rowScope != null)
                {
                    var name = node.Name.SimpleName;
                    var symbol = new VariableSymbol(name, _binder._rowScope);
                    _binder.SetSemanticInfo(node.Name, new SemanticInfo(symbol, null));
                    _binder._localScope.AddSymbol(symbol);
                }

                BindNode(node);
            }

            public override void VisitPartitionOperator(PartitionOperator node)
            {
                node.ByExpression.Accept(this);

                var oldLocalScope = _binder._localScope;

                // put column referenced in by-expression into scope during evaluation of partition expression
                var column = GetReferencedSymbol(node.ByExpression) as ColumnSymbol;
                if (column != null)
                {
                    _binder._localScope = new LocalScope(_binder._localScope);
                    _binder._localScope.AddSymbol(column);
                }

                if (node.Operand is PartitionQuery)
                {
                    // partition-expressions { xxx } don't have an implied row-scope, since you 
                    // are required to specify a complete query expression.
                    var oldRowScope = _binder._rowScope;
                    _binder._rowScope = null;
                    node.Operand.Accept(this);
                    _binder._rowScope = oldRowScope;
                }
                else
                {
                    // do nothing here, this sub expression assumes same row-scope as partition operator has
                    node.Operand.Accept(this);
                }

                _binder._localScope = oldLocalScope;

                BindNode(node);
            }

            public override void VisitForkOperator(ForkOperator node)
            {
                var oldRowScope = _binder._rowScope;
                try
                {
                    // reset back to the original row scope for each fork
                    foreach (var expr in node.Expressions)
                    {
                        _binder._rowScope = oldRowScope;
                        expr.Accept(this);
                    }
                }
                finally
                {
                    _binder._rowScope = oldRowScope;
                }

                BindNode(node);
            }

            public override void VisitMaterializedViewCombineExpression(MaterializedViewCombineExpression node)
            {
                node.ViewName.Accept(this);
                node.BaseClause.Accept(this);
                node.DeltaClause.Accept(this);

                var oldScope = _binder._rowScope;
                try
                {
                    _binder._rowScope = GetResultType(node.DeltaClause.Expression) as TableSymbol;
                    node.AggregationsClause.Accept(this);
                }
                finally
                {
                    _binder._rowScope = oldScope;

                }

                BindNode(node);
            }

            public override void VisitParenthesizedExpression(ParenthesizedExpression node)
            {
                // nested expressions should not see any existing path scope
                var oldPathScope = _binder._pathScope;
                _binder._pathScope = null;
                try
                {
                    base.VisitParenthesizedExpression(node);
                }
                finally
                {
                    _binder._pathScope = oldPathScope;
                }
            }

            public override void VisitFunctionCallExpression(FunctionCallExpression node)
            {
                // first bind name to determine the function
                node.Name.Accept(this);

                // function call arguments should not see any existing path scope
                var oldPathScope = _binder._pathScope;
                _binder._pathScope = null; 
                try
                {
                    var argumentScope = _binder.GetArgumentScope(node, _binder._scopeKind);

                    if (GetReferencedSymbol(node.Name) is FunctionSymbol fn && fn.Signatures.Count == 1)
                    {
                        // handle arguments from a known signature specially
                        this.VisitArgumentList(node.ArgumentList, fn.Signatures[0], argumentScope);
                    }
                    else
                    {
                        this.VisitInScopeKind(node.ArgumentList, argumentScope);
                    }
                }
                finally
                {
                    _binder._pathScope = oldPathScope;
                }

                BindNode(node);

                // copy final semantic info of function call to name node, unless binding the name was an error
                if (node.Name.ResultType == null || !node.Name.ResultType.IsError)
                {
                    var fcInfo = node.GetSemanticInfo();
                    _binder.SetSemanticInfo(node.Name, new SemanticInfo(fcInfo?.ReferencedSymbol, fcInfo?.ResultType));
                }
            }

            private void VisitArgumentList(ExpressionList list, Signature signature, ScopeKind argumentScope)
            {
                var arguments = s_expressionListPool.AllocateFromPool();
                var argumentParameters = s_parameterListPool.AllocateFromPool();

                for (int i = 0, n = list.Expressions.Count; i < n; i++)
                {
                    arguments.Add(list.Expressions[i].Element);
                }

                signature.GetArgumentParameters(arguments, argumentParameters);

                for (int i = 0, n = arguments.Count; i < n; i++)
                {
                    var arg = arguments[i];
                    var p = argumentParameters[i];

                    if (p != null)
                    {
                        switch (p.ArgumentKind)
                        {
                            case ArgumentKind.Aggregate:
                                // switch to aggregate scope for arguments that need to be aggregate expressions
                                this.VisitInScopeKind(arg, ScopeKind.Aggregate);
                                break;

                            case ArgumentKind.Column_Parameter0:
                            case ArgumentKind.Column_Parameter0_Common:
                                if (i > 0 && arguments[0]?.ResultType is TableSymbol p0Table)
                                {
                                    var oldRowScope = _binder._rowScope;

                                    if (p.ArgumentKind == ArgumentKind.Column_Parameter0_Common)
                                    {
                                        var commonColumns = new List<ColumnSymbol>();
                                        GetCommonColumns(_binder._rowScope.Columns, p0Table.Columns, commonColumns);
                                        _binder._rowScope = new TableSymbol(commonColumns);
                                    }
                                    else
                                    {
                                        _binder._rowScope = p0Table;
                                    }

                                    this.VisitInScopeKind(arg, argumentScope);
                                    _binder._rowScope = oldRowScope;
                                }
                                else
                                {
                                    this.VisitInScopeKind(arg, ScopeKind.Aggregate);
                                }
                                break;

                            case ArgumentKind.Expression_Parameter0_Element:
                                if (i > 0 && arguments[0].ResultType is TupleSymbol tuple)
                                {
                                    this.VisitInRowScope(arg, new TableSymbol(tuple.Columns));
                                }
                                else
                                {
                                    this.VisitInScopeKind(arg, argumentScope);
                                }
                                break;

                            default:
                                this.VisitInScopeKind(arg, argumentScope);
                                break;
                        }
                    }
                    else
                    {
                        this.VisitInScopeKind(arg, argumentScope);
                    }
                }

                s_expressionListPool.ReturnToPool(arguments);
                s_parameterListPool.ReturnToPool(argumentParameters);
            }

            public override void VisitInvokeOperator(InvokeOperator node)
            {
                var oldRowScope = _binder._rowScope;
                _binder._implicitArgumentType = _binder.RowScopeOrEmpty;
                _binder._rowScope = null;
                try
                {
                    node.Function.Accept(this);
                }
                finally
                {
                    _binder._rowScope = oldRowScope;
                    _binder._implicitArgumentType = null;
                }

                BindNode(node);
            }

            public override void VisitEvaluateOperator(EvaluateOperator node)
            {
                var oldScopeKind = _binder._scopeKind;
                _binder._scopeKind = ScopeKind.PlugIn;

                try
                {
                    VisitChildren(node);
                }
                finally
                {
                    _binder._scopeKind = oldScopeKind;
                }

                BindNode(node);
            }

            public override void VisitLetStatement(LetStatement node)
            {
                base.VisitLetStatement(node);

                TryGetLiteralValueInfo(node.Expression, out var valueInfo);

                var exprType = GetResultTypeOrError(node.Expression);
                Symbol local = (exprType is FunctionSymbol || exprType is EntityGroupSymbol)
                    ? exprType
                    : (Symbol)new VariableSymbol(node.Name.SimpleName, exprType, GetIsConstant(node.Expression), valueInfo, node.Expression);

                // put local symbol definition on name
                _binder.SetSemanticInfo(node.Name, new SemanticInfo(local, null));

                // add to local scope
                _binder._localScope.AddSymbol(local);
            }

            public override void VisitFunctionDeclaration(FunctionDeclaration node)
            {
                var oldLocalScope = _binder._localScope;
                var oldDefaultColumnNameSuffix = _binder._defaultColumnNameSuffix;
                try
                {
                    // remember scope before function declaration
                    // to use when evaluating function expansions
                    _binder._staticScopes[node] = oldLocalScope.Copy();

                    _binder._localScope = new LocalScope(oldLocalScope);
                    _binder._defaultColumnNameSuffix = 1;
                    base.VisitFunctionDeclaration(node);
                }
                finally
                {
                    _binder._localScope = oldLocalScope;
                    _binder._defaultColumnNameSuffix = oldDefaultColumnNameSuffix;
                }
            }

            public override void VisitFunctionParameters(FunctionParameters node)
            {
                base.VisitFunctionParameters(node);

                // declare all parameters in the local scope
                var diagnostics = s_diagnosticListPool.AllocateFromPool();
                try
                {
                    _binder.BindParameterDeclarations(node.Parameters);
                    _binder.AddDeclarationsToLocalScope(node.Parameters);
                }
                finally
                {
                    s_diagnosticListPool.ReturnToPool(diagnostics);
                }
            }

            public override void VisitQueryParametersStatement(QueryParametersStatement node)
            {
                base.VisitQueryParametersStatement(node);

                // declare all query parameters in the local scope
                _binder.BindParameterDeclarationsAsVariables(node.Parameters);
                _binder.AddDeclarationsToLocalScope(node.Parameters);
            }

            public override void VisitScanOperator(ScanOperator node)
            {
                node.Parameters.Accept(this);
                node.OrderByClause?.Accept(this);
                node.PartitionByClause?.Accept(this);

                var oldLocalScope = _binder._localScope;
                _binder._localScope = new LocalScope(oldLocalScope);
                try
                {
                    if (node.DeclareClause != null)
                    {
                        _binder.BindColumnDeclarations(node.DeclareClause.Declarations);
                        _binder.AddDeclarationsToLocalScope(node.DeclareClause.Declarations);
                    }

                    _binder.BindStepDeclarations(node);
                    _binder.AddStepDeclarationsToLocalScope(node);

                    node.Steps.Accept(this);
                }
                finally
                {
                    _binder._localScope = oldLocalScope;
                }

                BindNode(node);
            }

            public override void VisitInlineExternalTableExpression(InlineExternalTableExpression node)
            {
                node.Parameters.Accept(this);

                var oldLocalScope = _binder._localScope;

                try
                {
                    _binder._localScope = new LocalScope(oldLocalScope);
                    if (node.Schema != null)
                    {
                        _binder.BindColumnDeclarations(node.Schema.Columns);
                        _binder.AddDeclarationsToLocalScope(node.Schema.Columns);
                    }
                    node.PartitionClause?.Accept(this);

                    _binder._localScope = new LocalScope(oldLocalScope);
                    if (node.PartitionClause != null)
                    {
                        for (int i = 0; i < node.PartitionClause.PartitionColumns.Count; i++)
                        {
                            var p = node.PartitionClause.PartitionColumns[i].Element;
                            var name = p.Name.SimpleName;
                            var type = _binder.GetTypeFromTypeExpression(p.Type);

                            if (!string.IsNullOrEmpty(name))
                            {
                                var symbol = new ColumnSymbol(name, type);
                                _binder.SetSemanticInfo(p.Name, new SemanticInfo(symbol, type));

                                _binder.AddDeclarationToLocalScope(p.Name);
                            }
                        }
                    }
                    node.PathFormat?.Accept(this);
                }
                finally
                {
                    _binder._localScope = oldLocalScope;
                }

                BindNode(node);
            }

            public override void VisitPatternStatement(PatternStatement node)
            {
                base.VisitPatternStatement(node);

                var type = node.Pattern != null
                    ? GetResultTypeOrError(node.Pattern)
                    : new PatternSymbol(node.Name.SimpleName);

                var local = new VariableSymbol(node.Name.SimpleName, type);

                // put local symbol definition on name
                _binder.SetSemanticInfo(node.Name, new SemanticInfo(local, null));

                // add to local scope
                _binder._localScope.AddSymbol(local);
            }

            public override void VisitPatternDeclaration(PatternDeclaration node)
            {
                // pre-bind parameters
                _binder.BindParameterDeclarations(node.Parameters);
                if (node.PathParameter != null)
                {
                    _binder.BindParameterDeclaration(node.PathParameter.Parameter);
                };

                var oldLocalScope = _binder._localScope;
                try
                {
                    _binder._localScope = new LocalScope(oldLocalScope);

                    // add bound parameter symbols to scope
                    _binder.AddDeclarationsToLocalScope(node.Parameters);

                    if (node.PathParameter != null)
                    {
                        _binder.AddDeclarationToLocalScope(node.PathParameter.Parameter.Name);
                    }

                    // parameters in scope while bindng pattern match bodies
                    base.VisitPatternDeclaration(node);
                }
                finally
                {
                    _binder._localScope = oldLocalScope;
                }
            }

            public override void VisitAliasStatement(AliasStatement node)
            {
                base.VisitAliasStatement(node);

                // remember database as aliased name.
                var name = node.Name.SimpleName;
                var db = GetResultTypeOrError(node.Expression) as DatabaseSymbol;

                if (name != null && db != null)
                {
                    _binder._aliasedDatabases[name] = db;
                }
            }

            public override void VisitFindOperator(FindOperator node)
            {
                node.DataScope?.Accept(this);
                node.Parameters?.Accept(this);
                node.InClause?.Accept(this);

                var oldRowScope = _binder._rowScope;
                try
                {
                    // gather all columns to put into scope for condition
                    _binder._rowScope = _binder.GetFindColumnsTable(node);

                    if (this.predicateBinder == null)
                    {
                        this.predicateBinder = new SearchPredicateBinder(_binder, this);
                    }

                    node.Condition.Accept(this.predicateBinder);

                    node.Project?.Accept(this);
                    node.ProjectAway?.Accept(this);
                }
                finally
                {
                    _binder._rowScope = oldRowScope;
                }

                BindNode(node);
            }

            private SearchPredicateBinder predicateBinder;

            public override void VisitSearchOperator(SearchOperator node)
            {
                node.Parameters?.Accept(this);
                node.DataScope?.Accept(this);

                var oldRowScope = _binder._rowScope;
                _binder._rowScope = null;

                node.InClause?.Accept(this);

                // gather all columns to put in scope for condition
                _binder._rowScope = oldRowScope;
                _binder._rowScope = _binder.GetSearchColumnsTable(node);

                if (this.predicateBinder == null)
                {
                    this.predicateBinder = new SearchPredicateBinder(_binder, this);
                }

                node.Condition.Accept(this.predicateBinder);

                _binder._rowScope = oldRowScope;

                BindNode(node);
            }

            public override void VisitMvApplyOperator(MvApplyOperator node)
            {
                node.Expressions?.Accept(this);
                node.RowLimitClause?.Accept(this);
                node.ContextIdClause?.Accept(this);

                var info = node.Accept(_nodeBinder);

                // now that we know the result schema (table) put it in scope and evaluate the subquery
                var oldRowScope = _binder._rowScope;
                var builder = s_projectionBuilderPool.AllocateFromPool();
                try
                {
                    _binder._rowScope = info.ResultType as TableSymbol;
                    node.Subquery.Accept(this);

                    // apply sub-query's semantic info back to overall apply operator
                    var subqueryInfo = node.Subquery.GetSemanticInfo();

                    if (oldRowScope != null)
                    {
                        // add all columns not applied/iterated over
                        var appliedColumns = new HashSet<ColumnSymbol>(node.Expressions.Select(e => e.Element.Expression?.ReferencedSymbol as ColumnSymbol).Where(e => e != null));

                        foreach (var col in oldRowScope.Columns)
                        {
                            if (!appliedColumns.Contains(col))
                            {
                                builder.Add(col, doNotRepeat: true);
                            }
                        }
                    }

                    if (subqueryInfo.ResultType is TableSymbol subqueryTable)
                    {
                        foreach (var col in subqueryTable.Columns)
                        {
                            builder.Add(col, replace: true);
                        }
                    }
                   
                    var resultTable = new TableSymbol(builder.GetProjection())
                        .WithInheritableProperties(_binder.RowScopeOrEmpty);

                    var applyInfo = new SemanticInfo(resultTable, info.Diagnostics);

                    _binder.SetSemanticInfo(node, applyInfo);
                }
                finally
                {
                    _binder._rowScope = oldRowScope;
                    s_projectionBuilderPool.ReturnToPool(builder);
                }
            }

            public override void VisitNameReference(NameReference node)
            {
                base.VisitNameReference(node);

                // some commands have unqualified column reference relative to a previous table reference
                if (node.ReferencedSymbol is TableSymbol ts && IsCommandButNotQueryPart(node))
                {
                    _binder._rowScope = ts;
                }
            }

            /// <summary>
            /// Returns true if the node is part of a command syntax but not in the input/output query.
            /// </summary>
            private static bool IsCommandButNotQueryPart(SyntaxNode node)
            {
                // if the name/path is a part of a command expression
                while (node.Parent is PathExpression
                    || node.Parent is SeparatedElement
                    || node.Parent is SyntaxList
                    )
                {
                    node = node.Parent;
                }

                return node.Parent is Command
                    || node.Parent is CustomNode;
            }

            public override void VisitCommandBlock(CommandBlock node)
            {
                this.VisitList(node.Directives);

                if (node.Statements.Count > 0)
                {
                    var commandStatement = node.Statements[0].Element;
                    commandStatement.Accept(this);

                    var command = commandStatement.GetFirstDescendant<Command>();
                    if (command != null)
                    {
                        var commandResults = new VariableSymbol("$command_results", GetResultTypeOrError(command));
                        _binder._localScope.AddSymbol(commandResults);
                    }

                    // all other statements
                    for (int i = 1; i < node.Statements.Count; i++)
                    {
                        node.Statements[i].Element.Accept(this);
                    }
                }
            }

            public override void VisitToScalarExpression(ToScalarExpression node)
            {
                node.KindParameter?.Accept(this);

                var oldScope = _binder._rowScope;
                _binder._rowScope = null;
                try
                {
                    node.Expression?.Accept(this);
                }
                finally
                {
                    _binder._rowScope = oldScope;
                }

                BindNode(node);
            }

            public override void VisitToTableExpression(ToTableExpression node)
            {
                node.KindParameter?.Accept(this);

                var oldScope = _binder._rowScope;
                _binder._rowScope = null;
                try
                {
                    node.Expression?.Accept(this);
                }
                finally
                {
                    _binder._rowScope = oldScope;
                }

                BindNode(node);
            }

            public override void VisitMakeGraphOperator(MakeGraphOperator node)
            {
                // We want to visit (and bind) the partitioned-by subquery last.
                for (int i = 0, n = node.ChildCount; i < n; i++)
                {
                    if (node.GetChild(i) is SyntaxNode child && node.GetName(i) != nameof(node.PartitionedByClause))
                    {
                        child.Accept(this);
                    }
                }
                BindNode(node);

                // In case we have a partitioned-by subquery, we want the result symbol to be that of the 
                // subquery.
                if (node.PartitionedByClause != null)
                {
                    node.PartitionedByClause.Accept(this);

                    var mgInfo = node.GetSemanticInfo();
                    var info = node.PartitionedByClause.Subquery.GetSemanticInfo().WithDiagnostics(mgInfo.Diagnostics);
                    _binder.SetSemanticInfo(node, info);
                }
                
            }

            public override void VisitMakeGraphTableAndKeyClause(MakeGraphTableAndKeyClause node)
            {
                node.Table?.Accept(this);

                var oldScope = _binder._rowScope;
                _binder._rowScope = node.Table.ResultType as TableSymbol;
                try
                {
                    node.Column?.Accept(this);
                }
                finally
                {
                    _binder._rowScope = oldScope;
                }

                BindNode(node);
            }

            public override void VisitGraphMatchOperator(GraphMatchOperator node)
            {
                var oldScope = _binder._rowScope;
                var oldLocalScope = _binder._localScope;
                _binder._rowScope = null;
                _binder._localScope = new LocalScope(oldLocalScope);
                try
                {
                    node.Parameters.Accept(this);

                    _binder.BindGraphMatchPatternDeclarations(node, node.Patterns);
                    _binder.AddGraphMatchPatternDeclarationsToLocalScope(node.Patterns);

                    node.WhereClause?.Accept(this);
                    node.ProjectClause?.Accept(this);
                }
                finally
                {
                    _binder._rowScope = oldScope;
                    _binder._localScope = oldLocalScope;
                }

                BindNode(node);
            }

            public override void VisitGraphShortestPathsOperator(GraphShortestPathsOperator node)
            {
                var oldScope = _binder._rowScope;
                var oldLocalScope = _binder._localScope;
                _binder._rowScope = null;
                _binder._localScope = new LocalScope(oldLocalScope);
                try
                {
                    node.Parameters.Accept(this);

                    _binder.BindGraphMatchPatternDeclarations(node, node.Patterns);
                    _binder.AddGraphMatchPatternDeclarationsToLocalScope(node.Patterns);

                    node.WhereClause?.Accept(this);
                    node.ProjectClause?.Accept(this);
                }
                finally
                {
                    _binder._rowScope = oldScope;
                    _binder._localScope = oldLocalScope;
                }

                BindNode(node);
            }

            public override void VisitGraphToTableOperator(GraphToTableOperator node)
            {
                base.VisitGraphToTableOperator(node);
                var oldScope = _binder._rowScope;
                _binder._rowScope = null;

                try
                {                                        
                    if (node.OutputClause.Count > 1 && node.ResultType is GroupSymbol group)
                    {
                        var tables = group.Members.Where(m => m is TableSymbol);
                        _binder._localScope.AddSymbols(tables);
                    }
                }
                finally
                {
                    _binder._rowScope = oldScope;
                }

                BindNode(node);
            }
        }
    }
}