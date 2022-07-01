using System;

namespace Kusto.Language.Binding
{
    using Symbols;
    using Syntax;

    internal partial class Binder
    {
        /// <summary>
        /// A context builder that searches for 'as' operator definitions and adds them to the local scope
        /// </summary>
        internal class AsContextBuilder : DefaultSyntaxVisitor
        {
            private readonly int _position;
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
                            child.Accept(this);
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
                    _binder._localScope.AddSymbol(declaration);
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
        }
    }
}