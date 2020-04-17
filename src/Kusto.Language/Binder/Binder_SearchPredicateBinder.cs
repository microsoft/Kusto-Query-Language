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
        /// The <see cref="SearchPredicateBinder"/> handles special binding logic for predicates used by search and find operators.
        /// </summary>
        private class SearchPredicateBinder : DefaultSyntaxVisitor
        {
            private readonly Binder _binder;
            private readonly TreeBinder _treeBinder;

            public SearchPredicateBinder(
                Binder binder,
                TreeBinder treeBinder)
            {
                _binder = binder;
                _treeBinder = treeBinder;
            }

            public TableSymbol RowScopeOrEmpty => _binder._rowScope ?? TableSymbol.Empty;

            protected override void DefaultVisit(SyntaxNode node)
            {
                // if we get here, fall back to normal expression binding.
                node.Accept(_treeBinder);
            }

            public override void VisitStarExpression(StarExpression node)
            {
                if (node.Parent is BinaryExpression b)
                {
                    // use dynamic type as union of all column types
                    _binder.SetSemanticInfo(node, new SemanticInfo(ScalarTypes.Dynamic));
                }
                else
                {
                    // stand alone asterisk equiv to 'true'
                    _binder.SetSemanticInfo(node, new SemanticInfo(ScalarTypes.Bool));
                }
            }

            public override void VisitLiteralExpression(LiteralExpression node)
            {
                if (node.Kind == SyntaxKind.StringLiteralExpression)
                {
                    // string literals in predicate positions are abbreviations of:  * has <literal>
                    _binder.SetSemanticInfo(node, new SemanticInfo(ScalarTypes.Bool));
                }
                else
                {
                    node.Accept(_treeBinder);
                }
            }

            public override void VisitParenthesizedExpression(ParenthesizedExpression node)
            {
                // stay in special rules for parenthesized expression
                node.Expression.Accept(this);
                _binder.SetSemanticInfo(node, new SemanticInfo(node.Expression.ResultType));
            }

            public override void VisitBinaryExpression(BinaryExpression node)
            {
                var opKind = GetOperatorKind(node.Kind);
                switch (opKind)
                {
                    case OperatorKind.And:
                    case OperatorKind.Or:
                        // stay in special rules for and/or
                        node.Left.Accept(this);
                        node.Right.Accept(this);
                        var info = _binder.GetBinaryOperatorInfo(opKind, node.Left, node.Right, node.Operator);
                        _binder.SetSemanticInfo(node, info);
                        break;

                    default:
                        // drop back to normal expression binding for anything else
                        node.Accept(_treeBinder);
                        break;
                }
            }
        }
    }
}