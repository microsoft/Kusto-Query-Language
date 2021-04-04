using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Editor;

namespace Kusto.Language.Syntax
{
    using Utils;

    /// <summary>
    /// A <see cref="SyntaxNode"/> with variable shape.
    /// </summary>
    public class CustomNode : SyntaxNode
    {
        private readonly IReadOnlyList<CustomElementDescriptor> shape;
        private readonly IReadOnlyList<SyntaxElement> elements;

        public CustomNode(IReadOnlyList<CustomElementDescriptor> shape, IReadOnlyList<SyntaxElement> elements, IReadOnlyList<Diagnostic> diagnostics)
            : base(diagnostics)
        {
            this.shape = shape ?? EmptyReadOnlyList<CustomElementDescriptor>.Instance;

            if (elements != null)
            {
                var elist = new List<SyntaxElement>(elements.Count);

                for (int i = 0; i < elements.Count; i++)
                {
                    var e = elements[i];
                    var optional = this.shape[i].IsOptional;
                    var attached = Attach(e, optional);
                    elist.Add(attached);
                }

                this.elements = elist.AsReadOnly();
            }
            else
            {
                this.elements = EmptyReadOnlyList<SyntaxElement>.Instance;
            }

            this.Init();
        }

        public CustomNode(IReadOnlyList<CustomElementDescriptor> shape, params SyntaxElement[] elements)
            : this(shape, elements, null)
        {
        }

        public override SyntaxKind Kind => SyntaxKind.CustomNode;

        public override int ChildCount => this.shape.Count;

        public override SyntaxElement GetChild(int index)
        {
            if (index < 0 || index >= this.shape.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index < this.elements.Count)
            {
                return this.elements[index];
            }
            else
            {
                return null;
            }
        }

        public override string GetName(int index)
        {
            if (index < 0 || index >= this.shape.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.shape[index].Name;
        }

        public override CompletionHint GetCompletionHint(int index)
        {
            if (index < 0 || index >= this.shape.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.shape[index].CompletionHint;
        }

        public override bool IsOptional(int index)
        {
            if (index < 0 || index >= this.shape.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.shape[index].IsOptional;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCustom(this);
        }

        public override TResult Accept<TResult>(SyntaxVisitor<TResult> visitor)
        {
            return visitor.VisitCustom(this);
        }

        protected override SyntaxElement CloneCore()
        {
            var clonedElements = this.elements.Select(e => e.Clone()).ToArray();
            return new CustomNode(this.shape, clonedElements);
        }
    }

    /// <summary>
    /// Describes facts about a child element of a <see cref="CustomNode"/>.
    /// </summary>
    public class CustomElementDescriptor
    {
        /// <summary>
        /// The name of the element.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// If true, then the element is optional (can be null)
        /// </summary>
        public bool IsOptional { get; }

        /// <summary>
        /// The <see cref="CompletionHint"/> associated with this element.
        /// </summary>
        public CompletionHint CompletionHint { get; }

        public CustomElementDescriptor(string name, CompletionHint hint, bool isOptional = false)
        {
            this.Name = name ?? "";
            this.IsOptional = isOptional;
            this.CompletionHint = hint;
        }

        public CustomElementDescriptor(CompletionHint hint, bool isOptional = false)
            : this("", hint, isOptional)
        {
        }

        public CustomElementDescriptor WithName(string name)
        {
            if (this.Name == name)
                return this;
            return new CustomElementDescriptor(name, this.CompletionHint, this.IsOptional);
        }

        public CustomElementDescriptor WithHint(CompletionHint hint)
        {
            if (this.CompletionHint == hint)
                return this;
            return new CustomElementDescriptor(this.Name, hint, this.IsOptional);
        }

        public CustomElementDescriptor WithIsOptional(bool isOptional)
        {
            if (this.IsOptional == isOptional)
                return this;
            return new CustomElementDescriptor(this.Name, this.CompletionHint, this.IsOptional);
        }

        public static CustomElementDescriptor Default =
            new CustomElementDescriptor("", CompletionHint.None, false);
    }
}
