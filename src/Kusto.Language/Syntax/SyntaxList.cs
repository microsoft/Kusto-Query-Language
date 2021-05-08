using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Syntax
{
    using Utils;

    /// <summary>
    /// A list of <see cref="SyntaxElement"/>'s.
    /// </summary>
    public abstract class SyntaxList : SyntaxNode //, IReadOnlyList<SyntaxElement>
    {
        private readonly SyntaxElement[] elements;

        protected SyntaxList(SyntaxElement[] elements, IReadOnlyList<Diagnostic> diagnostics)
            : base(diagnostics)
        {
            this.elements = elements;

            for (int i = 0; i < elements.Length; i++)
            {
                this.elements[i] = Attach(elements[i]);
            }

            this.Init();
        }

        public override SyntaxKind Kind => SyntaxKind.List;

        public virtual Type ElementType => null;

        /// <summary>
        /// Gets the element at the index.
        /// </summary>
        public SyntaxElement this[int index] => elements[index];

        /// <summary>
        /// The number of elements in the list.
        /// </summary>
        public int Count => elements.Length;

        public IEnumerator<SyntaxElement> GetEnumerator()
        {
            return ((IEnumerable<SyntaxElement>)this.elements).GetEnumerator();
        }

        /// <summary>
        /// The number of child elements this element has.
        /// </summary>
        public override int ChildCount => this.Count;

        /// <summary>
        /// Get the child of this element at the specified index.
        /// </summary>
        public override SyntaxElement GetChild(int index) => this.elements[index];

        protected IReadOnlyList<SyntaxElement> GetElements()
        {
            return this.elements;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitList(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitList(this);
        }
    }

    /// <summary>
    /// A list of <see cref="SyntaxElement"/>'s.
    /// </summary>
    public sealed class SyntaxList<TElement> : SyntaxList, IReadOnlyList<TElement>
        where TElement : SyntaxElement
    {
        public SyntaxList(IEnumerable<TElement> elements, IReadOnlyList<Diagnostic> diagnostics = null)
            : base(elements.ToArray(), diagnostics)
        {
        }

        public SyntaxList(params TElement[] elements)
            : base(elements, null)
        {
        }

        public override SyntaxKind Kind => SyntaxKind.List;

        public override Type ElementType => typeof(TElement);

        private static TElement[] Copy(IReadOnlyList<TElement> list)
        {
            var newArray = new TElement[list.Count];

            for (int i = 0; i < newArray.Length; i++)
            {
                newArray[i] = list[i];
            }

            return newArray;
        }

        /// <summary>
        /// Gets the element at the index.
        /// </summary>
        public new TElement this[int index] => (TElement)base[index];

        public new IEnumerator<TElement> GetEnumerator()
        {
            return ((IEnumerable<TElement>)this.GetElements()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Creates a copy of this <see cref="SyntaxList{TElement}"/>
        /// </summary>
        public new SyntaxList<TElement> Clone() => (SyntaxList<TElement>)this.CloneCore();

        protected override SyntaxElement CloneCore()
        {
            var oldElements = this.GetElements();
            var newElements = new TElement[oldElements.Count];

            for (int i = 0; i < newElements.Length; i++)
            {
                newElements[i] = (TElement)oldElements[i].Clone();
            }

            return new SyntaxList<TElement>(newElements);
        }

        public static SyntaxList<TElement> Empty() => new SyntaxList<TElement>(new TElement[0]);
    }

    public static class SyntaxListExtensions
    {
        /// <summary>
        /// Gets the first <see cref="NamedParameter"/> with the specified name or null if none match.
        /// </summary>
        public static NamedParameter GetByName(this SyntaxList<NamedParameter> list, string name) =>
            list.FirstOrDefault(np => np.Name.SimpleName == name);

        /// <summary>
        /// Gets the first <see cref="NamedParameter"/> with one of the specified names or null if none match any of the names.
        /// </summary>
        public static NamedParameter GetByName(this SyntaxList<NamedParameter> list, IReadOnlyList<string> names) =>
            list.FirstOrDefault(np => names.Contains(np.Name.SimpleName));

        /// <summary>
        /// Gets the first <see cref="NamedParameter"/> that matches the <see cref="QueryOperatorParameter"/> definition
        /// </summary>
        public static NamedParameter GetParameter(this SyntaxList<NamedParameter> list, QueryOperatorParameter parameter) =>
            list.FirstOrDefault(np => np.Name.SimpleName == parameter.Name || (parameter.Aliases.Count > 0 && parameter.Aliases.Contains(np.Name.SimpleName)));
    }
}