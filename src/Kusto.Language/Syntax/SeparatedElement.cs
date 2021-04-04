using System;
using System.Collections;
using System.Collections.Generic;

namespace Kusto.Language.Syntax
{
    using Editor;

    public abstract class SeparatedElement : SyntaxNode
    {
        /// <summary>
        /// The element in a list
        /// </summary>
        public SyntaxElement Element { get; }

        /// <summary>
        /// An optional separator token.
        /// </summary>
        public SyntaxToken Separator { get; }

        protected SeparatedElement(SyntaxElement element, SyntaxToken separator = null)
            : base(null)
        {
            this.Element = Attach(element);
            this.Separator = Attach(separator, optional: true);
            this.Init();
        }

        public override SyntaxKind Kind => SyntaxKind.SeparatedElement;

        public override int ChildCount => 2;

        public override SyntaxElement GetChild(int index)
        {
            switch (index)
            {
                case 0: return this.Element;
                case 1: return this.Separator;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public override CompletionHint GetCompletionHint(int index)
        {
            switch (index)
            {
                case 0: return CompletionHint.Inherit;
                case 1: return CompletionHint.Syntax; // seperator is punctuation, no symbols
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitSeparatedElement(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitSeparatedElement(this);
        }
    }

    /// <summary>
    /// An element in a list with an optional separator token.
    /// </summary>
    public sealed class SeparatedElement<TElement> : SeparatedElement
        where TElement : SyntaxElement
    {
        /// <summary>
        /// The element in a list
        /// </summary>
        public new TElement Element => (TElement)base.Element;

        public SeparatedElement(TElement element, SyntaxToken separator = null)
            : base(element, separator)
        {
        }

        protected override SyntaxElement CloneCore()
        {
            return new SeparatedElement<TElement>((TElement)this.Element.Clone(), this.Separator?.Clone());
        }

        public static SeparatedElement<TElement> Empty() => new SeparatedElement<TElement>(null, null);
    }
}