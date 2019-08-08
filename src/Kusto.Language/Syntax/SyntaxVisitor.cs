using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Syntax
{
    /// <summary>
    /// A base class for visiting any <see cref="SyntaxNode"/> sub-type.
    /// </summary>
    public abstract partial class SyntaxVisitor
    {
        public abstract void VisitCustom(CustomNode node);
        public abstract void VisitList(SyntaxList list);
        public abstract void VisitSeparatedElement(SeparatedElement separatedElement);
    }

    /// <summary>
    /// A base class for visiting any <see cref="SyntaxNode"/> sub-type.
    /// Visitors that are not overridden automatically forward to the <see cref="DefaultVisit(SyntaxNode)"/> method.
    /// </summary>
    public abstract partial class DefaultSyntaxVisitor : SyntaxVisitor
    {
        public override void VisitCustom(CustomNode node)
        {
            this.DefaultVisit(node);
        }

        public override void VisitList(SyntaxList list)
        {
            this.DefaultVisit(list);
        }

        public override void VisitSeparatedElement(SeparatedElement separatedElement)
        {
            this.DefaultVisit(separatedElement);
        }
    }

    public abstract partial class SyntaxVisitor<TResult>
    {
        public abstract TResult VisitCustom(CustomNode node);
        public abstract TResult VisitList(SyntaxList list);
        public abstract TResult VisitSeparatedElement(SeparatedElement separatedElement);
    }

    public abstract partial class DefaultSyntaxVisitor<TResult>
    {
        public override TResult VisitCustom(CustomNode node)
        {
            return this.DefaultVisit(node);
        }

        public override TResult VisitList(SyntaxList list)
        {
            return this.DefaultVisit(list);
        }

        public override TResult VisitSeparatedElement(SeparatedElement separatedElement)
        {
            return this.DefaultVisit(separatedElement);
        }
    }
}