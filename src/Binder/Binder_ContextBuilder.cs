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
        /// The <see cref="ContextBuilder"/> is a <see cref="SyntaxVisitor"/> that recreates
        /// the <see cref="Binder"/> state that existed for a <see cref="SyntaxNode"/> during the full semantic analysis
        /// of the <see cref="TreeBinder"/> without actually doing the full analysis.
        /// </summary>
        private class ContextBuilder : DefaultSyntaxVisitor
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
                        parentNode.Visit(this);
                        break;  // okay, done now
                    }
                }

                // reached the top?  Look for as-operators too
                if (node.Parent == null)
                {
                    node.Visit(_asBuilder);
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
                if (_position >= node.ArgumentList.TextStart)
                {
                    _binder._pathScope = null;
                    _binder._scopeKind = ScopeKind.Normal;
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

            public override void VisitList(SyntaxList list)
            {
                base.VisitList(list);

                // add declarations to local scope that occur before the position
                if (list.ElementType == typeof(SeparatedElement<Statement>))
                {
                    for (int i = 0, n = list.Count; i < n; i++)
                    {
                        var se = (SeparatedElement<Statement>)list[i];

                        // don't include declarations not fully defined before position
                        if (_position < se.End || IsIncomplete(se))
                            break;

                        if (se.Element is LetStatement ls)
                        {
                            _binder.AddLetDeclarationToScope(_binder._localScope, ls);
                        }
                        else if (se.Element is QueryParametersStatement qps)
                        {
                            _binder.AddDeclarationsToLocalScope(_binder._localScope, qps.Parameters);
                        }
                        else if (se.Element is PatternStatement ps)
                        {
                            _binder._localScope.AddDeclaration(_binder.GetReferencedSymbol(ps.Name));
                        }
                    }
                }
            }

            private static bool IsIncomplete(SyntaxNode node)
            {
                var last = node.GetLastToken(includeZeroWidthTokens: true);
                return last?.IsMissing ?? false;
            }

            public override void VisitFunctionDeclaration(FunctionDeclaration node)
            {
                base.VisitFunctionDeclaration(node);

                if (_position >= node.Body.TextStart && 
                    (_position < node.Body.End
                     || node.Body.CloseBrace.IsMissing && _position <= node.Body.End + 1))
                {
                    _binder.AddDeclarationsToLocalScope(_binder._localScope, node.Parameters.Parameters);
                }
            }

            public override void VisitPatternDeclaration(PatternDeclaration node)
            {
                base.VisitPatternDeclaration(node);

                if (_position > node.Patterns.TextStart)
                {
                    _binder.AddDeclarationsToLocalScope(_binder._localScope, node.Parameters);

                    if (node.PathParameter != null)
                    {
                        _binder.AddDeclarationToLocalScope(_binder._localScope, node.PathParameter.Parameter);
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

                if (_position < node.LookupClause.TextStart)
                {
                    // no row scope
                    _binder._rowScope = null;
                }
                else
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

                if (_position >= node.OnKeyword.TextStart)
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
        }

        class AsContextBuilder : DefaultSyntaxVisitor
        {
            private int _position;
            private readonly Binder _binder;

            protected override void DefaultVisit(SyntaxNode node)
            {
                // visit children
                if (node != null)
                {
                    for (int i = 0, n = node.ChildCount; i < n; i++)
                    {
                        var child = node.GetChild(i) as SyntaxNode;
                        if (child != null)
                        {
                            child.Visit(this);
                        }
                    }
                }
            }

            public AsContextBuilder(int position, Binder binder)
            {
                _position = position;
                _binder = binder;
            }

            public override void VisitAsOperator(AsOperator node)
            {
                base.VisitAsOperator(node);

                var name = node.Name.SimpleName;
                var type = node.ResultType;
                if (!string.IsNullOrEmpty(name) && _position > node.End && type != null)
                {
                    var declaration = new VariableSymbol(node.Name.SimpleName, type);
                    _binder._localScope.AddDeclaration(declaration);
                }
            }

            public override void VisitFunctionBody(FunctionBody node)
            {
                // only include as-operators inside function bodies if the position is also within the body
                if (_position > node.TextStart && _position < node.End)
                {
                    base.VisitFunctionBody(node);
                }
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
                        _binder._localScope.AddDeclaration(commandResults);
                    }
                }
            }
        }
    }
}
