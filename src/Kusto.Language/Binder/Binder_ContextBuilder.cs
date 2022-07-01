using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Binding
{
    using Symbols;
    using Syntax;
    using Utils;

    internal partial class Binder
    {
        /// <summary>
        /// The <see cref="ContextBuilder"/> is a <see cref="SyntaxVisitor"/> that puts
        /// a <see cref="Binder"/> into the same state that existed for a given <see cref="SyntaxNode"/> during 
        /// the full semantic analysis.
        /// </summary>
        /// <remarks>
        /// The <see cref="ContextBuilder"/> works by walking the syntax tree upwards from a node at
        /// a given position, recreating the state for the ancestor nodes first and refining that state
        /// on the walk back to down the original node.
        /// </remarks>
        internal class ContextBuilder : DefaultSyntaxVisitor
        {
            private readonly Binder _binder;
            private readonly int _position;
            private readonly AsContextBuilder _asBuilder;

            public ContextBuilder(Binder binder, int position)
            {
                _binder = binder;
                _position = position;
                _asBuilder = new AsContextBuilder(position, _binder);
            }

            protected override void DefaultVisit(SyntaxNode node)
            {
                // if you can, ask your parent instead
                for (var parent = node.Parent; parent != null; parent = parent.Parent)
                {
                    if (parent is SyntaxNode parentNode)
                    {
                        parentNode.Accept(this);
                        break;  // okay, done now
                    }
                }

                // reached the top?  Look for as-operators too
                if (node.Parent == null)
                {
                    node.Accept(_asBuilder);
                }
            }

            public override void VisitPathExpression(PathExpression node)
            {
                base.VisitPathExpression(node);

                // expressions on right-hand side of a dot have path scope
                if (_position >= node.Selector.TriviaStart)
                {
                    _binder._pathScope = node.Expression.ResultType;
                }
            }

            public override void VisitElementExpression(ElementExpression node)
            {
                base.VisitElementExpression(node);

                // expressions within the element selector have path scope?
                if (_position >= node.Selector.TriviaStart)
                {
                    _binder._pathScope = node.Expression.ResultType;
                }
            }

            public override void VisitParenthesizedExpression(ParenthesizedExpression node)
            {
                base.VisitParenthesizedExpression(node);

                // nested expressions should not see outer path scope
                _binder._pathScope = null;
                _binder._implicitArgumentType = null;
            }

            public override void VisitFunctionCallExpression(FunctionCallExpression node)
            {
                base.VisitFunctionCallExpression(node);

                // function call arguments do not have path scope or special scope kinds like aggregate
                if (_position > node.Name.End)
                {
                    _binder._pathScope = null;
                    _binder._scopeKind = _binder.GetArgumentScope(node, _binder._scopeKind);
                    _binder._implicitArgumentType = null;
                }
            }

            public override void VisitPipeExpression(PipeExpression node)
            {
                base.VisitPipeExpression(node);

                // operators on right-hand side of pipe have row scope
                if (_position >= node.Operator.TriviaStart)
                {
                    _binder._rowScope = node.Expression.ResultType as TableSymbol;
                }
            }

            public override void VisitEvaluateOperator(EvaluateOperator node)
            {
                base.VisitEvaluateOperator(node);
                _binder._scopeKind = ScopeKind.PlugIn;
            }

            public override void VisitSummarizeOperator(SummarizeOperator node)
            {
                base.VisitSummarizeOperator(node);

                if (node.ByClause == null || _position < node.ByClause.TextStart)
                {
                    _binder._scopeKind = ScopeKind.Aggregate;
                }
            }

            public override void VisitNamedParameter(NamedParameter node)
            {
                base.VisitNamedParameter(node);
                _binder._scopeKind = ScopeKind.Normal;
            }

            public override void VisitMakeSeriesOperator(MakeSeriesOperator node)
            {
                base.VisitMakeSeriesOperator(node);

                if (_position < node.OnClause.TextStart ||
                    (node.OnClause.IsMissing && node.OnClause.End == node.End && _position >= node.End))
                {
                    _binder._scopeKind = ScopeKind.Aggregate;
                }
            }

            public override void VisitTopNestedClause(TopNestedClause node)
            {
                base.VisitTopNestedClause(node);

                if (node.ByKeyword.Width > 0 && _position > node.ByKeyword.End)
                {
                    _binder._scopeKind = ScopeKind.Aggregate;
                }
            }

            /// <summary>
            /// The node ends in a list or optional element
            /// </summary>
            private static bool CanHoldMore(SyntaxNode node)
            {
                // walk up tree looking for lists
                var lastToken = node.GetLastToken(includeZeroWidthTokens: true);
                
                for (var subNode = lastToken.Parent;
                    subNode != node && subNode.TextStart > node.TextStart;
                    subNode = subNode.Parent)
                {
                    for (int i = subNode.ChildCount - 1; i >= 0; i--)
                    {
                        var child = subNode.GetChild(i);
                        // missing optional element could be here
                        if (child == null && subNode.IsOptional(i))
                            return true;
                        // lists can always have more
                        if (child is SyntaxList)
                            return true;
                        // see-through zero width elements
                        if (child != null && child.Width > 0)
                            break;
                    }
                }

                return false;
            }

            private static bool IsInTriviaAfter(SyntaxNode node, int position)
            {
                return IsInTriviaAfter(node.GetLastToken(), position);
            }

            private static bool IsInTriviaAfter(SyntaxToken token, int position)
            {
                var nextToken = token.GetNextToken();
                return nextToken == null 
                    || position < nextToken.TextStart 
                    || nextToken.Kind == SyntaxKind.EndOfTextToken;
            }

            public override void VisitFunctionDeclaration(FunctionDeclaration node)
            {
                base.VisitFunctionDeclaration(node);

                if (_position >= node.Body.TextStart && 
                    (_position < node.Body.End
                     || node.Body.CloseBrace.IsMissing && _position <= node.Body.End + 1))
                {
                    _binder.AddDeclarationsToLocalScope(node.Parameters.Parameters);
                }
            }

            public override void VisitFunctionBody(FunctionBody node)
            {
                base.VisitFunctionBody(node);

                if (node.CloseBrace.IsMissing || _position < node.CloseBrace.TextStart)
                {
                    AddStatementDeclarationsToScope(node.Statements);
                }
            }

            public override void VisitQueryBlock(QueryBlock node)
            {
                base.VisitQueryBlock(node);

                AddStatementDeclarationsToScope(node.Statements);
            }

            public override void VisitCommandBlock(CommandBlock node)
            {
                base.VisitCommandBlock(node);

                if (node.Statements.Count > 0
                    && node.Statements[0].Separator != null
                    && _position > node.Statements[0].End)
                {
                    var command = node.Statements[0].Element.GetFirstDescendant<Command>();
                    if (command != null)
                    {
                        var commandResults = new VariableSymbol("$command_results", _binder.GetResultTypeOrError(command));
                        _binder._localScope.AddSymbol(commandResults);
                    }
                }
                else
                {
                    AddStatementDeclarationsToScope(node.Statements);
                }
            }

            private void AddStatementDeclarationsToScope(SyntaxList<SeparatedElement<Statement>> statementList)
            {
                for (int i = 0, n = statementList.Count; i < n; i++)
                {
                    var se = statementList[i];

                    // don't include declarations not fully defined before position
                    if (_position < se.End ||
                        IsInTriviaAfter(se.Element, _position)
                        && (IsIncomplete(se.Element) || CanHoldMore(se.Element)))
                        break;

                    if (se.Element is LetStatement ls)
                    {
                        _binder.AddLetDeclarationToScope(_binder._localScope, ls);
                    }
                    else if (se.Element is QueryParametersStatement qps)
                    {
                        _binder.AddDeclarationsToLocalScope(qps.Parameters);
                    }
                    else if (se.Element is PatternStatement ps)
                    {
                        _binder._localScope.AddSymbol(_binder.GetReferencedSymbol(ps.Name));
                    }
                }
            }

            /// <summary>
            /// The node has a missing element as its last child.
            /// </summary>
            private static bool IsIncomplete(SyntaxNode node)
            {
                var last = node.GetLastToken(includeZeroWidthTokens: true);
                return last?.IsMissing ?? false;
            }


            public override void VisitPatternDeclaration(PatternDeclaration node)
            {
                base.VisitPatternDeclaration(node);

                if (_position > node.Patterns.TextStart)
                {
                    _binder.AddDeclarationsToLocalScope(node.Parameters);

                    if (node.PathParameter != null)
                    {
                        _binder.AddDeclarationToLocalScope(node.PathParameter.Parameter.Name);
                    }
                }
            }

            public override void VisitJoinOperator(JoinOperator node)
            {
                base.VisitJoinOperator(node);

                if (node.ConditionClause == null || _position < node.ConditionClause.TextStart)
                {
                    // no row scope
                    _binder._rowScope = null;
                }
                else if (node.ConditionClause != null && _position >= node.ConditionClause.TextStart)
                {
                    _binder._rightRowScope = node.Expression.ResultType as TableSymbol;
                }
            }

            public override void VisitLookupOperator(LookupOperator node)
            {
                base.VisitLookupOperator(node);

                if (node.LookupClause.IsMissing || _position < node.LookupClause.TextStart)
                {
                    // no row scope
                    _binder._rowScope = null;
                }
                else if (_position >= node.LookupClause.TextStart)
                {
                    // this.position >= node.LookupClause.TextStart
                    _binder._rightRowScope = node.Expression.ResultType as TableSymbol;
                }
            }

            public override void VisitUnionOperator(UnionOperator node)
            {
                base.VisitUnionOperator(node);

                // union operator expressions are all tables.. they don't refer to row scope columns
                _binder._rowScope = null;
            }

            public override void VisitFindOperator(FindOperator node)
            {
                base.VisitFindOperator(node);

                if (node.InClause == null || _position >= node.InClause.End)
                {
                    _binder._rowScope = _binder.GetFindColumnsTable(node);
                }
            }

            public override void VisitSearchOperator(SearchOperator node)
            {
                base.VisitSearchOperator(node);

                if (_position >= node.Condition.TextStart)
                {
                    _binder._rowScope = _binder.GetSearchColumnsTable(node);
                }

                if (node.InClause != null && _position >= node.InClause.TextStart)
                {
                    // in clause arguments are all tables, no columns visible
                    _binder._rowScope = null;
                }
            }

            public override void VisitMvApplyOperator(MvApplyOperator node)
            {
                base.VisitMvApplyOperator(node);

                if (!node.OnKeyword.IsMissing && _position >= node.OnKeyword.TextStart)
                {
                    var info = new NodeBinder(_binder).VisitMvApplyOperator(node);
                    _binder._rowScope = info?.ResultType as TableSymbol;
                }
            }

            public override void VisitInvokeOperator(InvokeOperator node)
            {
                base.VisitInvokeOperator(node);

                if (node.Function != null && !node.Function.IsMissing && _position > node.Function.TextStart)
                {
                    _binder._rowScope = null;
                }
            }

            public override void VisitPartitionOperator(PartitionOperator node)
            {
                base.VisitPartitionOperator(node);

                if (_position >= node.Operand.TextStart && node.Operand is PartitionQuery)
                {
                    var column = node.ByExpression.ReferencedSymbol as ColumnSymbol;
                    if (column != null)
                    {
                        _binder._localScope.AddSymbol(column);
                    }
                }
            }

            public override void VisitScanOperator(ScanOperator node)
            {
                base.VisitScanOperator(node);

                if (_position > node.WithKeyword.TextStart && node.DeclareClause != null)
                {
                    _binder.AddDeclarationsToLocalScope(node.DeclareClause.Declarations);
                    _binder.AddStepDeclarationsToLocalScope(node);
                }
            }

            public override void VisitCustomCommand(CustomCommand node)
            {
                var nearestTableRef = node.GetDescendants<NameReference>(nr => nr.ReferencedSymbol is TableSymbol)
                    .Where(nr => nr.End <= _position)
                    .LastOrDefault();

                if (nearestTableRef != null)
                {
                    _binder._rowScope = (TableSymbol)nearestTableRef.ReferencedSymbol;
                }
            }

            public override void VisitInExpression(InExpression node)
            {
                base.VisitInExpression(node);

                if (_position >= node.Operator.End)
                {
                    _binder._rowScope = null;
                }
            }

            public override void VisitToScalarExpression(ToScalarExpression node)
            {
                base.VisitToScalarExpression(node);
                _binder._rowScope = null;
            }

            public override void VisitToTableExpression(ToTableExpression node)
            {
                base.VisitToTableExpression(node);
                _binder._rowScope = null;
            }

            public override void VisitSetOptionStatement(SetOptionStatement node)
            {
                base.VisitSetOptionStatement(node);

                if (_position >= node.SetKeyword.End && (node.ValueClause == null || _position <= node.ValueClause.TextStart))
                {
                    _binder._scopeKind = ScopeKind.Option;
                }
            }

            public override void VisitMaterializedViewCombineExpression(MaterializedViewCombineExpression node)
            {
                base.VisitMaterializedViewCombineExpression(node);

                if (_position > node.AggregationsClause.OpenParen.TextStart)
                {
                    _binder._rowScope = _binder.GetResultType(node.DeltaClause.Expression) as TableSymbol;
                }
            }

            public override void VisitMakeGraphTableAndKeyClause(MakeGraphTableAndKeyClause node)
            {
                base.VisitMakeGraphTableAndKeyClause(node);

                if (_position >= node.OnKeyword.TextStart)
                {
                    _binder._rowScope = node.Table.ResultType as TableSymbol;
                }
            }

            public override void VisitGraphMatchOperator(GraphMatchOperator node)
            {
                base.VisitGraphMatchOperator(node);

                if ((node.WhereClause != null && _position >= node.WhereClause.TextStart)
                    || (node.ProjectClause != null && _position >= node.ProjectClause.TextStart))
                {
                    _binder._localScope = new LocalScope(_binder._localScope);
                    _binder.AddGraphMatchPatternDeclarationsToLocalScope(node);
                }
            }

            public override void VisitGraphMergeOperator(GraphMergeOperator node)
            {
                base.VisitGraphMergeOperator(node);

                if (node.OnClause == null || _position < node.OnClause.TextStart)
                {
                    // no row scope
                    _binder._rowScope = null;
                }
                else if (node.OnClause != null && _position >= node.OnClause.TextStart)
                {
                    var leftShape = node.Parent is PipeExpression pe && pe.Expression.ResultType is GraphSymbol lgs ? lgs.EdgeShape : null;
                    var rightShape = node.Graph.ResultType is GraphSymbol rgs ? rgs.EdgeShape : null;
                    _binder._rowScope = leftShape ?? rightShape;
                    _binder._rightRowScope = rightShape ?? leftShape;
                }
            }
        }
    }
}
