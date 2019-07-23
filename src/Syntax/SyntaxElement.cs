using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Syntax
{
    using Editor;
    using Symbols;
    using Utils;

    /// <summary>
    /// A basic element of syntax.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{Kind}: {DebugText}")]
    public abstract partial class SyntaxElement
    {
        /// <summary>
        /// The parent node of this element, or extended data. Use property to access this value.
        /// </summary>
        private object parent;

        /// <summary>
        /// State flags that combine up the tree.
        /// </summary>
        private Flags flags;

        /// <summary>
        /// Kind of token
        /// </summary>
        public abstract SyntaxKind Kind { get; }

        /// <summary>
        /// For debugger display only.
        /// </summary>
        private string DebugText => this.ToString(IncludeTrivia.Minimal);

        #region initialization
        protected SyntaxElement(IReadOnlyList<Diagnostic> diagnostics)
        {
            SetDiagnostics(diagnostics);
        }

        /// <summary>
        /// Initializes the element (does one-time computations)
        /// </summary>
        protected virtual void Init()
        {
            int offset = 0;

            for (int i = 0; i < this.ChildCount; i++)
            {
                var child = this.GetChild(i);
                if (child != null)
                {
                    child.OffsetInParent = offset;
                    child.IndexInParent = (short)i;
                    offset += child.FullWidth;
                }
            }
        }

        /// <summary>
        /// Attaches the <see cref="SyntaxElement"/> as a child of this <see cref="SyntaxNode"/>.
        /// </summary>
        protected TElement Attach<TElement>(TElement element, bool optional = false)
            where TElement : SyntaxElement
        {
            if (element != null)
            {
                if (element.Parent != null)
                {
                    throw new InvalidOperationException("The syntax element already has a parent.");
                }

                element.Parent = (SyntaxNode)this;
                this.flags |= element.flags;
            }
            else if (!optional)
            {
                throw new ArgumentNullException("The element is not optional");
            }

            return element;
        }
        #endregion

        #region diagnostics
        /// <summary>
        /// True if this <see cref="SyntaxElement"/> or any child element has syntax diagnostics.
        /// </summary>
        public bool ContainsSyntaxDiagnostics => (this.flags & Flags.ContainsDiagnostics) != 0;

        /// <summary>
        /// True if this element has syntax diagnostics.
        /// </summary>
        public bool HasSyntaxDiagnostics => this.ContainsSyntaxDiagnostics && this.SyntaxDiagnostics.Count > 0;

        /// <summary>
        /// All syntax diagnostics located at this element.
        /// </summary>
        public IReadOnlyList<Diagnostic> SyntaxDiagnostics => this.GetExtendedData(create: false)?.SyntaxDiagnostics ?? Diagnostic.NoDiagnostics;

        /// <summary>
        /// Gets syntax diagnostics for this <see cref="SyntaxElement"/> an all child elements.
        /// </summary>
        public IReadOnlyList<Diagnostic> GetContainedSyntaxDiagnostics()
        {
            if (this.ContainsSyntaxDiagnostics)
            {
                var diagnostics = new List<Diagnostic>();
                GatherDiagnostics(this, diagnostics, DiagnosticsInclude.Syntactic);
                return diagnostics.AsReadOnly();
            }
            else
            {
                return Diagnostic.NoDiagnostics;
            }
        }

        private void SetDiagnostics(IReadOnlyList<Diagnostic> diagnostics)
        {
            // bind diagnostics to this element
            // this function should only be called once during construction of this node.
            if (diagnostics != null && diagnostics.Count > 0)
            {
                this.GetExtendedData(true).SyntaxDiagnostics = diagnostics.ToReadOnly();
                this.flags |= Flags.ContainsDiagnostics;
            }
        }

        /// <summary>
        /// Creates a copy of this <see cref="SyntaxElement"/> with the specified diagnostics.
        /// </summary>
        public SyntaxElement WithDiagnostics(IEnumerable<Diagnostic> diagnostics)
        {
            var clone = this.Clone();
            clone.SetDiagnostics(diagnostics.ToList().AsReadOnly());
            return clone;
        }

        /// <summary>
        /// Creates a copy of this <see cref="SyntaxElement"/> with the specified diagnostics added.
        /// </summary>
        public SyntaxElement WithAdditionalDiagnostics(IEnumerable<Diagnostic> diagnostics)
        {
            return this.WithDiagnostics(this.SyntaxDiagnostics.Concat(diagnostics));
        }

        /// <summary>
        /// Creates a copy of this <see cref="SyntaxElement"/> with the specified diagnostics added.
        /// </summary>
        public SyntaxElement WithAdditionalDiagnostics(params Diagnostic[] diagnostics)
        {
            return this.WithDiagnostics(this.SyntaxDiagnostics.Concat(diagnostics));
        }

        [Flags]
        private enum Flags : short
        {
            ContainsDiagnostics = 0x1
        }

        internal class ExtendedData
        {
            public SyntaxNode Parent;
            public IReadOnlyList<Diagnostic> SyntaxDiagnostics;
            public SemanticInfo SemanticInfo;
        }

        internal ExtendedData GetExtendedData(bool create)
        {
            var data = this.parent as ExtendedData;
            if (data == null && create)
            {
                data = new ExtendedData { Parent = this.parent as SyntaxNode };
                Interlocked.CompareExchange(ref this.parent, data, this.parent);
                data = this.parent as ExtendedData;
            }

            return data;
        }
        #endregion

        /// <summary>
        /// True if the <see cref="SyntaxElement"/> is a <see cref="SyntaxToken"/>.
        /// </summary>
        public virtual bool IsToken => false;

        /// <summary>
        /// True if the element is taking the place of a missing element.
        /// </summary>
        public virtual bool IsMissing => this.Width == 0 && this.ContainsSyntaxDiagnostics;

        #region navigation
        /// <summary>
        /// The parent node of this element.
        /// </summary>
        public SyntaxNode Parent
        {
            get
            {
                var node = this.parent as SyntaxNode;
                if (node != null)
                    return node;

                var data = this.parent as ExtendedData;
                if (data != null)
                    return data.Parent;

                return null;
            }

            private set
            {
                // can only be set once
                Interlocked.CompareExchange(ref this.parent, value, null);

                var data = this.parent as ExtendedData;
                if (data != null)
                {
                    Interlocked.CompareExchange(ref data.Parent, value, null);
                }
            }
        }

        /// <summary>
        /// The root element
        /// </summary>
        public SyntaxElement Root
        {
            get
            {
                var element = this;
                var parent = element.Parent;

                while (parent != null)
                {
                    element = parent;
                    parent = element.Parent;
                }

                return element;
            }
        }

        /// <summary>
        /// Child Index in parent.
        /// </summary>
        protected short IndexInParent { get; private set; }

        /// <summary>
        /// The number of immediate child elements this element has.
        /// </summary>
        public abstract int ChildCount { get; }

        /// <summary>
        /// Get the child element of this node at the specified index.
        /// </summary>
        public virtual SyntaxElement GetChild(int index) => throw new IndexOutOfRangeException();

        /// <summary>
        /// True if the child element at the specified index is optional and may contain a null value.
        /// </summary>
        public virtual bool IsOptional(int index) => false;

        /// <summary>
        /// Gets the name of the child element at the specified index.
        /// </summary>
        public virtual string GetName(int index) => "";

        /// <summary>
        /// The <see cref="CompletionHint"/> to use for this child element index in the syntax tree.
        /// </summary>
        public virtual CompletionHint GetCompletionHint(int index) => this.GetCompletionHintCore(index);

        /// <summary>
        /// The generated <see cref="CompletionHint"/> to use for this child element index in the syntax tree.
        /// </summary>
        protected virtual CompletionHint GetCompletionHintCore(int index) => CompletionHint.Inherit;

        /// <summary>
        /// True if the element or any of its descendants have missing children
        /// </summary>
        public bool HasMissingChildren()
        {
            if (this.ContainsSyntaxDiagnostics)
            {
                for (int n = this.ChildCount - 1; n >= 0; n--)
                {
                    var child = this.GetChild(n);

                    if (child != null && (child.IsMissing || child.HasMissingChildren()))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the index of the child node, or -1 if the node is not a child.
        /// </summary>
        public int GetChildIndex(SyntaxElement child)
        {
            for (int i = 0, n = this.ChildCount; i < n; i++)
            {
                if (this.GetChild(i) == child)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// The depth of this node below the root.
        /// </summary>
        public int Depth
        {
            get
            {
                int depth = 0;

                for (var element = this; element.Parent != null; element = element.Parent)
                {
                    depth++;
                }

                return depth;
            }
        }

        /// <summary>
        /// Returns true if this element is the ancestor of the specified element.
        /// </summary>
        public bool IsAncestorOf(SyntaxElement element)
        {
            while (element != null)
            {
                if (element.Parent == this)
                    return true;

                element = element.Parent;
            }

            return false;
        }

        /// <summary>
        /// The name of the element given by the parent.
        /// </summary>
        public string NameInParent
        {
            get
            {
                if (this.Parent != null)
                {
                    return this.Parent.GetName(this.Parent.GetChildIndex(this));
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets the first ancestor of this element that matches the specified type and predicate.
        /// </summary>
        public TElement GetFirstAncestor<TElement>(Func<TElement, bool> predicate = null)
            where TElement : SyntaxElement
        {
            for (SyntaxElement elem = this.Parent; elem != null; elem = elem.Parent)
            {
                if (elem is TElement e && (predicate == null || predicate(e)))
                {
                    return e;
                }
            }

            return default(TElement);
        }

        /// <summary>
        /// Gets the first ancestor of this element (including itself) that matches the specified type and predicate.
        /// </summary>
        public TElement GetFirstAncestorOrSelf<TElement>(Func<TElement, bool> predicate = null)
            where TElement : SyntaxElement
        {
            for (var elem = this; elem != null; elem = elem.Parent)
            {
                if (elem is TElement e && (predicate == null || predicate(e)))
                {
                    return e;
                }
            }

            return default(TElement);
        }

        /// <summary>
        /// Gets the all ancestors of this element (including itself) that match the specified type and predicate.
        /// </summary>
        public IReadOnlyList<TElement> GetAncestors<TElement>(Func<TElement, bool> predicate = null)
            where TElement : SyntaxElement
        {
            List<TElement> list = null;

            for (SyntaxElement elem = this.Parent; elem != null; elem = elem.Parent)
            {
                if (elem is TElement e && (predicate == null || predicate(e)))
                {
                    if (list == null)
                    {
                        list = new List<TElement>();
                    }

                    list.Add(e);
                }
            }

            return list != null ? list.ToReadOnly() : EmptyReadOnlyList<TElement>.Instance;
        }

        /// <summary>
        /// Gets the all ancestors of this element (including itself) that match the specified type and predicate.
        /// </summary>
        public IReadOnlyList<TElement> GetAncestorsOrSelf<TElement>(Func<TElement, bool> predicate = null)
            where TElement : SyntaxElement
        {
            List<TElement> list = null;

            for (var elem = this; elem != null; elem = elem.Parent)
            {
                if (elem is TElement e && (predicate == null || predicate(e)))
                {
                    if (list == null)
                    {
                        list = new List<TElement>();
                    }

                    list.Add(e);
                }
            }

            return list != null ? list.ToReadOnly() : EmptyReadOnlyList<TElement>.Instance;
        }

        /// <summary>
        /// Gets the first descendant of this element that matches the specified type and predicate.
        /// </summary>
        public TElement GetFirstDescendant<TElement>(Func<TElement, bool> predicate = null)
            where TElement : SyntaxElement
        {
            for (int i = 0; i < this.ChildCount; i++)
            {
                var child = this.GetChild(i);
                if (child != null)
                {
                    if (child is TElement ce)
                    {
                        if (predicate == null || predicate(ce))
                            return ce;
                    }

                    if (child is SyntaxNode cn)
                    {
                        var result = child.GetFirstDescendant(predicate);
                        if (result != null)
                            return result;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the first descendant of this element (including itself) that matches the specified type and predicate.
        /// </summary>
        public TElement GetFirstDescendantOrSelf<TElement>(Func<TElement, bool> predicate = null)
            where TElement : SyntaxElement
        {
            if (this is TElement te && (predicate == null || predicate(te)))
            {
                return te;
            }

            return GetFirstDescendant(predicate);
        }

        /// <summary>
        /// Gets all descendants of this element that match the specified type and predicate.
        /// </summary>
        public IReadOnlyList<TElement> GetDescendants<TElement>(Func<TElement, bool> predicate = null)
            where TElement : SyntaxElement
        {
            var list = GetDescendants(this, predicate, null);
            return list != null ? list.ToReadOnly() : EmptyReadOnlyList<TElement>.Instance;
        }

        /// <summary>
        /// Gets the descendants of the specified element that match the specified type and predicate.
        /// </summary>
        private static List<TElement> GetDescendants<TElement>(SyntaxElement element, Func<TElement, bool> predicate, List<TElement> list)
            where TElement : SyntaxElement
        {
            for (int i = 0; i < element.ChildCount; i++)
            {
                var child = element.GetChild(i);
                if (child != null)
                {
                    if (child is TElement ce && (predicate == null || predicate(ce)))
                    {
                        if (list == null)
                        {
                            list = new List<TElement>();
                        }

                        list.Add(ce);
                    }

                    if (child is SyntaxNode cn)
                    {
                        list = GetDescendants(child, predicate, list);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Gets all descendants of this element (including itself) that match the specified type and predicate.
        /// </summary>
        public IReadOnlyList<TElement> GetDescendantsOrSelf<TElement>(Func<TElement, bool> predicate = null)
            where TElement : SyntaxElement
        {
            List<TElement> list = null;

            if (this is TElement te && (predicate == null || predicate(te)))
            {
                list = new List<TElement>() { te };
            }

            list = GetDescendants(this, predicate, list);

            return list != null ? list.ToReadOnly() : EmptyReadOnlyList<TElement>.Instance;
        }

        /// <summary>
        /// Gets all the tokens contained by this <see cref="SyntaxElement"/> in lexical order.
        /// </summary>
        public IReadOnlyList<SyntaxToken> GetTokens(bool includeZeroWidthTokens = false)
        {
            var tokens = new List<SyntaxToken>();

            this.WalkTokens(this.TriviaStart, this.End, t =>
            {
                if (t.FullWidth > 0 || includeZeroWidthTokens)
                {
                    tokens.Add(t);
                }
            });

            return tokens.ToReadOnly();
        }

        /// <summary>
        /// Enumerates the tokens in lexical order and invokes the action for all the tokens 
        /// between the <see cref="p:start"/> and <see cref="p:end"/> text position.
        /// </summary>
        public void WalkTokens(int start, int end, Action<SyntaxToken> action)
        {
            for (int i = 0, n = this.ChildCount; i < n; i++)
            {
                var child = this.GetChild(i);
                if (child != null && Overlaps(start, end, child.TriviaStart, child.End))
                {
                    if (child.IsToken)
                    {
                        action((SyntaxToken)child);
                    }
                    else
                    {
                        child.WalkTokens(start, end, action);
                    }
                }
            }
        }

        /// <summary>
        /// Enumerates the sub-elements in lexical order, bottom up, invoking the action for each element including this element.
        /// </summary>
        public void WalkElements(Action<SyntaxElement> action)
        {
            for (int i = 0, n = this.ChildCount; i < n; i++)
            {
                var child = this.GetChild(i);
                if (child != null)
                {
                    child.WalkElements(action);
                }
            }

            action(this);
        }

        /// <summary>
        /// Returns true if the range (startA, endA) overlaps (startB, endB)
        /// </summary>
        private static bool Overlaps(int startA, int endA, int startB, int endB)
        {
            return Math.Max(startA, startB) < Math.Min(endA, endB);
        }

        /// <summary>
        /// Gets the next <see cref="SyntaxElement"/> sibling of this element.
        /// </summary>
        public SyntaxElement GetNextSibling(bool includeZeroWidthElements = false)
        {
            if (this.Parent != null)
            {
                for (int i = this.IndexInParent + 1; i < this.Parent.ChildCount; i++)
                {
                    var sibling = this.Parent.GetChild(i);
                    if (sibling != null && (includeZeroWidthElements || sibling.FullWidth > 0))
                        return sibling;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the previous <see cref="SyntaxElement"/> sibling of this element.
        /// </summary>
        public SyntaxElement GetPreviousSibling(bool includeZeroWidthElements = false)
        {
            if (this.Parent != null)
            {
                for (int i = this.IndexInParent - 1; i >= 0; i--)
                {
                    var sibling = this.Parent.GetChild(i);
                    if (sibling != null && (includeZeroWidthElements || sibling.FullWidth > 0))
                        return sibling;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the first descendant token of this <see cref="SyntaxElement"/> in lexical order.
        /// </summary>
        public SyntaxToken GetFirstToken(bool includeZeroWidthTokens = false)
        {
            for (int i = 0, n = this.ChildCount; i < n; i++)
            {
                var child = this.GetChild(i);
                if (child != null)
                {
                    if (child.IsToken)
                    {
                        if (includeZeroWidthTokens || child.FullWidth > 0)
                            return (SyntaxToken)child;
                    }
                    else
                    {
                        var first = child.GetFirstToken(includeZeroWidthTokens);
                        if (first != null)
                            return first;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the last descendant token of this <see cref="SyntaxElement"/> in lexical order.
        /// </summary>
        public SyntaxToken GetLastToken(bool includeZeroWidthTokens = false)
        {
            for (int i = this.ChildCount - 1; i >= 0; i--)
            {
                var child = this.GetChild(i);
                if (child != null)
                {
                    if (child.IsToken)
                    {
                        if (includeZeroWidthTokens || child.FullWidth > 0)
                            return (SyntaxToken)child;
                    }
                    else
                    {
                        var first = child.GetLastToken(includeZeroWidthTokens);
                        if (first != null)
                            return first;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the token at the specified position in the source text.
        /// If the position is within trivia, it will find the next token after the trivia.
        /// If the position is past the width of the tree, it will find the last token.
        /// </summary>
        public SyntaxToken GetTokenAt(int position)
        {
            var element = this;

            if (this.IsToken)
            {
                if (this.TriviaStart <= position && position < this.End)
                    return (SyntaxToken)this;
            }
            else
            {
                element = this.Root;
            }

            if (position >= element.FullWidth)
            {
                return element.GetLastToken(includeZeroWidthTokens: true);
            }

            // drill down until we find the token that covers this position.
        retry:
            if (element != null && element.ChildCount > 0)
            {
                for (int i = 0, n = element.ChildCount; i < n; i++)
                {
                    var child = element.GetChild(i);
                    if (child != null)
                    {
                        if (child.TriviaStart <= position && position < child.End)
                        {
                            if (child.IsToken)
                                return (SyntaxToken)child;

                            element = child;
                            goto retry;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the node that spans the specified range in the source text.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="length"></param>
        public SyntaxNode GetNodeAt(int position, int length)
        {
            var token = GetTokenAt(position);
            var parent = token.Parent;

            while (parent != null && (parent.End < position + length || !(parent is SyntaxNode)))
            {
                parent = parent.Parent;
            }

            return parent as SyntaxNode;
        }
        #endregion

        #region bounds
        /// <summary>
        /// The position in the source of the start of the leading trivia.
        /// </summary>
        public int TriviaStart => 
            this.Parent == null ? this.OffsetInParent : this.Parent.TriviaStart + this.OffsetInParent;

        /// <summary>
        /// The full width (in characters) of this element including leading trivia.
        /// </summary>
        public abstract int FullWidth { get; }

        protected int ComputeFullWidth()
        {
            int width = 0;

            for (int i = 0; i < this.ChildCount; i++)
            {
                var child = this.GetChild(i);
                if (child != null)
                {
                    width += child.FullWidth;
                }
            }

            return width;
        }

        /// <summary>
        /// The width (in characters) of the leading trivia.
        /// </summary>
        public virtual int TriviaWidth =>
            this.GetFirstToken()?.TriviaWidth ?? 0;

        /// <summary>
        /// The position in the source where the element's first token text starts.
        /// </summary>
        public int TextStart => this.TriviaStart + this.TriviaWidth;

        /// <summary>
        /// The position in the source immediately after this element.
        /// </summary>
        public int End => this.TriviaStart + this.FullWidth;

        /// <summary>
        /// The width (in characters) of this element, not including trivia.
        /// </summary>
        public virtual int Width => this.FullWidth - this.TriviaWidth;

        /// <summary>
        /// The offset in characters of this element from the start of the parent element.
        /// </summary>
        protected int OffsetInParent { get; private set; }
        #endregion

        #region clone
        /// <summary>
        /// Creates a copy of this <see cref="SyntaxElement"/>
        /// </summary>
        public SyntaxElement Clone() => CloneCore();

        protected abstract SyntaxElement CloneCore();
        #endregion

        #region ToString
        public override string ToString()
        {
            return this.ToString(IncludeTrivia.All);
        }

        public virtual string ToString(IncludeTrivia includeTrivia)
        {
            var builder = new StringBuilder();
            this.Write(builder, includeTrivia, this.TriviaStart);
            return builder.ToString();
        }

        protected virtual void Write(StringBuilder builder, IncludeTrivia includeTrivia, int initialTriviaStart)
        {
            for (int i = 0, n = this.ChildCount; i < n; i++)
            {
                var child = this.GetChild(i);
                if (child != null)
                {
                    child.Write(builder, includeTrivia, initialTriviaStart);
                }
            }
        }
        #endregion
    }

    public enum IncludeTrivia
    {
        /// <summary>
        /// All trivia is included.
        /// </summary>
        All,

        /// <summary>
        /// Only interior trivia is included.
        /// Trivia before the first token is not included.
        /// </summary>
        Interior,

        /// <summary>
        /// Only minimal trivia on the interior is included. 
        /// Trivia before the first token is not included, all other trivia becomes a single space or line feed.
        /// </summary>
        Minimal,
    }
}