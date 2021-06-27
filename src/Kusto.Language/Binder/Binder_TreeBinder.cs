using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kusto.Language.Symbols;
using Kusto.Language.Syntax;
using Kusto.Language.Utils;

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
                    _binder._pathScope = _binder.GetResultTypeOrError(node.Expression);
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

            public override void VisitPipeExpression(PipeExpression node)
            {
                node.Expression.Accept(this); 

                // result of left-side expression is in scope for right-side query operator
                var oldRowScope = _binder._rowScope;
                _binder._rowScope = _binder.GetResultType(node.Expression) as TableSymbol;
                try
                {
                    node.Operator.Accept(this);
                }
                finally
                {
                    _binder._rowScope = oldRowScope;
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
                _binder._rightRowScope = _binder.GetResultType(node.Expression) as TableSymbol;

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
                _binder._rightRowScope = _binder.GetResultType(node.Expression) as TableSymbol;
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
                    expr.Accept(this);
                }
            }

            public override void VisitUnionOperator(UnionOperator node)
            {
                var oldRowScope = _binder._rowScope;
                _binder._rowScope = null;
                try
                {
                    // union operator expressions do not bind to row scope columns (they are only tables)
                    VisitChildren(node);
                }
                finally
                {
                    _binder._rowScope = oldRowScope;
                }

                BindNode(node);
            }

            public override void VisitSummarizeOperator(SummarizeOperator node)
            {
                node.Parameters.Accept(this);

                // visit by clause before aggregates so by expressions are already bound
                // when resolving aggregate expression result types.
                node.ByClause?.Accept(this);

                VisitInScope(node.Aggregates, ScopeKind.Aggregate);

                BindNode(node);
            }

            public override void VisitMakeSeriesOperator(MakeSeriesOperator node)
            {
                node.Parameters.Accept(this);
                VisitInScope(node.Aggregates, ScopeKind.Aggregate);
                node.OnClause?.Accept(this);
                node.RangeClause?.Accept(this);
                node.ByClause?.Accept(this);

                BindNode(node);
            }

            private void VisitInScope(SyntaxNode node, ScopeKind kind)
            {
                if (node == null)
                    return;

                var oldScopeKind = _binder._scopeKind;
                _binder._scopeKind = kind;
                try
                {
                    node.Accept(this);
                }
                finally
                {
                    _binder._scopeKind = oldScopeKind;
                }
            }

            public override void VisitTopNestedClause(TopNestedClause node)
            {
                node.Expression?.Accept(this);
                node.OfExpression.Accept(this);
                node.WithOthersClause?.Accept(this);
                VisitInScope(node.ByExpression, ScopeKind.Aggregate);

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

                if (node.Operand is PartitionQuery)
                {
                    // partition-expressions { xxx } don't have an implied row-scope, since you 
                    // are required to specify a complete query expression.
                    var oldRowScope = _binder._rowScope;
                    _binder._rowScope = null;

                    var oldLocalScope = _binder._localScope;
                    try
                    {
                        // put column referenced in by-expression into scope during evaluation of partition expression
                        var column = _binder.GetReferencedSymbol(node.ByExpression) as ColumnSymbol;
                        if (column != null)
                        {
                            _binder._localScope = new LocalScope(_binder._localScope);
                            _binder._localScope.AddSymbol(column);
                        }

                        node.Operand.Accept(this);
                    }
                    finally
                    {
                        _binder._rowScope = oldRowScope;
                        _binder._localScope = oldLocalScope;
                    }
                }
                else
                {
                    // do nothing here, this sub expression assumes same row-scope as partition operator has
                    node.Operand.Accept(this);
                }

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
                    _binder._rowScope = _binder.GetResultType(node.DeltaClause.Expression) as TableSymbol;
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

                    if (_binder.GetReferencedSymbol(node.Name) is FunctionSymbol fn && fn.Signatures.Count == 1)
                    {
                        // handle arguments from a known signature specially
                        this.VisitArgumentList(node.ArgumentList, fn.Signatures[0], argumentScope);
                    }
                    else
                    {
                        this.VisitInScope(node.ArgumentList, argumentScope);
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
                for (int i = 0, n = list.Expressions.Count; i < n; i++)
                {
                    var arg = list.Expressions[i].Element;
                    var p = i < signature.Parameters.Count ? signature.Parameters[i] : null;

                    if (p != null && signature.HasAggregateParameters)
                    {
                        if (p.ArgumentKind == ArgumentKind.Aggregate)
                        {
                            // switch to aggregate scope for arguments that need to be aggregate expressions
                            this.VisitInScope(arg, ScopeKind.Aggregate);
                        }
                        else
                        {
                            this.VisitInScope(arg, argumentScope);
                        }
                    }
                    else
                    {
                        this.VisitInScope(arg, argumentScope);
                    }
                }
            }

            public override void VisitInvokeOperator(InvokeOperator node)
            {
                var oldRowScope = _binder._rowScope;
                _binder._implicitArgumentType = _binder._rowScope;
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

                TryGetLiteralValue(node.Expression, out var literalValue);

                var exprType = _binder.GetResultTypeOrError(node.Expression);
                Symbol local = (exprType is FunctionSymbol)
                    ? exprType
                    : (Symbol)new VariableSymbol(node.Name.SimpleName, exprType, _binder.GetIsConstant(node.Expression), literalValue);

                // put local symbol definition on name
                _binder.SetSemanticInfo(node.Name, new SemanticInfo(local, null));

                // add to local scope
                _binder._localScope.AddSymbol(local);
            }

            public override void VisitFunctionDeclaration(FunctionDeclaration node)
            {
                var oldLocalScope = _binder._localScope;
                try
                {
                    _binder._localScope = new LocalScope(oldLocalScope);
                    base.VisitFunctionDeclaration(node);
                }
                finally
                {
                    _binder._localScope = oldLocalScope;
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
                _binder.BindParameterDeclarations(node.Parameters);
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

            public override void VisitPatternStatement(PatternStatement node)
            {
                base.VisitPatternStatement(node);

                var type = node.Pattern != null
                    ? _binder.GetResultTypeOrError(node.Pattern)
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
                var db = _binder.GetResultTypeOrError(node.Expression) as DatabaseSymbol;

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
                bool isCommand = node.GetFirstAncestor<CustomCommand>() != null;
                if (isCommand && node.ReferencedSymbol is TableSymbol ts)
                {
                    _binder._rowScope = ts;
                }
            }

            public override void VisitCommandBlock(CommandBlock node)
            {
                if (node.Statements.Count > 0)
                {
                    var commandStatement = node.Statements[0].Element;
                    commandStatement.Accept(this);

                    var command = commandStatement.GetFirstDescendant<Command>();
                    if (command != null)
                    {
                        var commandResults = new VariableSymbol("$command_results", _binder.GetResultTypeOrError(command));
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

            public override void VisitInExpression(InExpression node)
            {
                node?.Left.Accept(this);

                var oldScope = _binder._rowScope;
                _binder._rowScope = null;
                try
                {
                    node?.Right.Accept(this);
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