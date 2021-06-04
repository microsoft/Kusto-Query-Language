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
        public SyntaxNode Parent { get; private set; }

        /// <summary>
        /// State flags that combine up the tree.
        /// </summary>
        private Flags flags;

        /// <summary>
        /// Addition information (diagnostics, semantic info) associated with this element
        /// </summary>
        private ExtendedData extendedData;

        /// <summary>
        /// Kind of token
        /// </summary>
        public virtual SyntaxKind Kind => SyntaxKind.None;

        /// <summary>
        /// For debugger display only.
        /// </summary>
        private string DebugText => this.ToString(IncludeTrivia.Minimal, maxLength: 100);

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

            for (int i = 0, n = this.ChildCount; i < n; i++)
            {
                var child = this.GetChild(i);
                if (child != null)
                {
                    child.OffsetInParent = offset;
                    child.IndexInParent = i;
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
            //public SyntaxNode Parent;
            public IReadOnlyList<Diagnostic> SyntaxDiagnostics;
            public SemanticInfo SemanticInfo;
        }

        internal ExtendedData GetExtendedData(bool create)
        {
            var data = this.extendedData;
            if (data == null && create)
            {
                var tmp = new ExtendedData();
                Interlocked.CompareExchange(ref this.extendedData, tmp, null);
                data = this.extendedData;
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
        public int IndexInParent { get; private set; }

        /// <summary>
        /// The number of immediate child elements this element has.
        /// </summary>
        public virtual int ChildCount => 0;

        /// <summary>
        /// Get the child element of this node at the specified index.
        /// </summary>
        public virtual SyntaxElement GetChild(int index) => throw new IndexOutOfRangeException();

#if DEBUG
        /// <summary>
        /// Property for debugging
        /// </summary>
        private SyntaxElement[] Children
        {
            get
            {
                var children = new SyntaxElement[this.ChildCount];
                for (int i = 0; i < this.ChildCount; i++)
                    children[i] = this.GetChild(i);
                return children;
            }
        }
#endif

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

                    // TODO: redo this to not be recursive
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
            if (child.Parent == this)
            {
                return child.IndexInParent;
            }
            else
            {
                return -1;
            }

#if false
            for (int i = 0, n = this.ChildCount; i < n; i++)
            {
                if (this.GetChild(i) == child)
                    return i;
            }

            return -1;
#endif
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
        /// Gets the common ancestor between two elements a and b.
        /// </summary>
        public static SyntaxNode GetCommonAncestor(SyntaxElement a, SyntaxElement b)
        {
            if (a == null || b == null)
                return null;

            var da = a.Depth;
            var db = b.Depth;

            while (da > db && da > 0)
            {
                a = a.Parent;
                da--;
            }

            while (db > da && db > 0)
            {
                b = b.Parent;
                db--;
            }

            if (da > 0 && a.Parent == b.Parent)
            {
                return a.Parent;
            }

            return null;
        }

        /// <summary>
        /// Gets the child node index for the subtree that the descendant is part of
        /// </summary>
        public int GetDescendantIndex(SyntaxElement descendant)
        {
            if (descendant == null)
                return -1;

            if (descendant.Parent != this)
            {
                var d = this.Depth;
                var dd = descendant.Depth;

                while (dd > d + 1)
                {
                    descendant = descendant.Parent;
                    dd--;
                }
            }

            if (descendant.Parent == this)
            {
                return descendant.IndexInParent;
            }
            else
            {
                return -1;
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
            return GetFirstDescendant(this, predicate, includeSelf: false);
        }

        /// <summary>
        /// Gets the first descendant of this element (including itself) that matches the specified type and predicate.
        /// </summary>
        public TElement GetFirstDescendantOrSelf<TElement>(Func<TElement, bool> predicate = null)
            where TElement : SyntaxElement
        {
            return GetFirstDescendant(this, predicate, includeSelf: true);
        }

        private static TElement GetFirstDescendant<TElement>(SyntaxElement element, Func<TElement, bool> predicate, bool includeSelf)
            where TElement : SyntaxElement
        {
            if (includeSelf && element is TElement telem && (predicate == null || predicate(telem)))
            {
                return telem;
            }

            var root = element;
            var childIndex = 0;

            while (element != null)
            {
                if (childIndex < element.ChildCount && childIndex >= 0)
                {
                    // walk down
                    var child = element.GetChild(childIndex);
                    if (child != null)
                    {
                        element = child;
                        childIndex = 0;

                        if (element is TElement telem2 && (predicate == null || predicate(telem2)))
                        {
                            return telem2;
                        }
                    }
                    else
                    {
                        childIndex++;
                    }
                }
                else if (element == root)
                {
                    break;
                }
                else
                {
                    // walk up
                    childIndex = element.IndexInParent + 1;
                    element = element.Parent;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all descendants of this element that match the specified type and predicate.
        /// </summary>
        public IReadOnlyList<TElement> GetDescendants<TElement>(Func<TElement, bool> predicate = null)
            where TElement : SyntaxElement
        {
            return GetDescendants(this, predicate, includeSelf: false);
        }

        /// <summary>
        /// Gets all descendants of this element (including itself) that match the specified type and predicate.
        /// </summary>
        public IReadOnlyList<TElement> GetDescendantsOrSelf<TElement>(Func<TElement, bool> predicate = null)
            where TElement : SyntaxElement
        {
            return GetDescendants(this, predicate, includeSelf: true);
        }

        /// <summary>
        /// Gets the descendants of the specified element that match the specified type and predicate.
        /// </summary>
        private static IReadOnlyList<TElement> GetDescendants<TElement>(
            SyntaxElement element, 
            Func<TElement, bool> predicate, 
            bool includeSelf)
            where TElement : SyntaxElement
        {
            List<TElement> list = null;

            if (includeSelf && element is TElement telem && (predicate == null || predicate(telem)))
            {
                list = list ?? new List<TElement>();
                list.Add(telem);
            }

            var root = element;
            var childIndex = 0;

            while (element != null)
            {
                if (childIndex < element.ChildCount && childIndex >= 0)
                {
                    // walk down
                    var child = element.GetChild(childIndex);
                    if (child != null)
                    {
                        element = child;
                        childIndex = 0;

                        if (element is TElement telem2 && (predicate == null || predicate(telem2)))
                        {
                            list = list ?? new List<TElement>();
                            list.Add(telem2);
                        }
                    }
                    else
                    {
                        childIndex++;
                    }
                }
                else if (element == root)
                {
                    break;
                }
                else
                {
                    // walk up
                    childIndex = element.IndexInParent + 1;
                    element = element.Parent;
                }
            }

            return list != null ? list.ToReadOnly() : EmptyReadOnlyList<TElement>.Instance;
        }

        /// <summary>
        /// Gets all the tokens contained by this <see cref="SyntaxElement"/> in lexical order.
        /// </summary>
        public IReadOnlyList<SyntaxToken> GetTokens(bool includeZeroWidthTokens = false)
        {
            var tokens = new List<SyntaxToken>();

            SyntaxToken token = null;
            while ((token = GetNextToken(this, token, includeZeroWidthTokens)) != null)
            {
                tokens.Add(token);
            }

            return tokens.ToReadOnly();
        }

        /// <summary>
        /// Invokes the action for each token contained by this <see cref="SyntaxElement"/>
        /// </summary>
        public void WalkTokens(Action<SyntaxToken> action)
        {
            WalkTokens(this.TriviaStart, this.End, action);
        }

        /// <summary>
        /// Invokes the action for each token contained by this <see cref="SyntaxElement"/>
        /// between the <see cref="p:start"/> and <see cref="p:end"/> text position.
        /// </summary>
        public void WalkTokens(int start, int end, Action<SyntaxToken> action)
        {
            start = Math.Max(start, this.TriviaStart);
            end = Math.Min(end, this.End);

            if (start < end)
            {
                for (var token = this.GetTokenAt(start);
                    token != null && token.TriviaStart < end;
                    token = GetNextToken(this, token, includeZeroWidthTokens: false))
                {
                    action(token);
                }
            }
        }

        /// <summary>
        /// Invokes the action for the element and its descendants, in lexical order, top down.
        /// </summary>
        /// <param name="action">The action that is invoked for each <see cref="SyntaxElement"/></param>
        public void WalkElements(Action<SyntaxElement> action)
        {
            WalkElements(this, action);
        }

        /// <summary>
        /// Walks this element and its descendants in lexical order, invoking the actions for each <see cref="SyntaxElement"/> including the root element.
        /// </summary>
        /// <param name="root">The root element of the walk. The walk includes this element and any descendant elements.</param>
        /// <param name="fnBefore">An optional function that is invoked for each element before any child elements are visited.</param>
        /// <param name="fnAfter">An optional function that is invoked for each element after any child elements have been visited.</param>
        /// <param name="fnDescend">An optional function that determines whether the children of an element are visited.</param>
        public static void WalkElements(
            SyntaxElement root, 
            Action<SyntaxElement> fnBefore = null, 
            Action<SyntaxElement> fnAfter = null,
            Func<SyntaxElement, bool> fnDescend = null)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            var node = root;
            var childIndex = 0;

            // the root before walking children
            fnBefore?.Invoke(root);

            while (node != null)
            {
                if (childIndex < node.ChildCount && childIndex >= 0 && (fnDescend == null || fnDescend(node)))
                {
                    // walk down
                    var child = node.GetChild(childIndex);
                    if (child != null)
                    {
                        node = child;
                        childIndex = 0;

                        // before walking children
                        fnBefore?.Invoke(node);
                    }
                    else
                    {
                        childIndex++;
                    }
                }
                else
                {
                    // after walking children
                    fnAfter?.Invoke(node);

                    // stop if we are done with root node
                    if (node == root)
                        break;

                    // walk up
                    childIndex = node.IndexInParent + 1;
                    node = node.Parent;
                }
            }
        }

        /// <summary>
        /// Walks this node and its descendants in lexical order, invoking the actions for each <see cref="SyntaxElement"/> including the root node.
        /// </summary>
        /// <param name="root">The root node of the walk. The walk includes this node and any descendant nodes.</param>
        /// <param name="fnBefore">An optional function that is invoked for each node before any child nodes are visited.</param>
        /// <param name="fnAfter">An optional function that is invoked for each node after any child nodes have been visited.</param>
        /// <param name="fnDescend">An optional function that determines whether the child nodes of an node are visited.</param>
        public static void WalkNodes(
            SyntaxNode root,
            Action<SyntaxNode> fnBefore = null,
            Action<SyntaxNode> fnAfter = null,
            Func<SyntaxNode, bool> fnDescend = null)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));

            var node = root;
            var childIndex = 0;

            // the root before walking children
            fnBefore?.Invoke(root);

            while (node != null)
            {
                if (childIndex < node.ChildCount && childIndex >= 0 && (fnDescend == null || fnDescend(node)))
                {
                    // walk down
                    var child = node.GetChild(childIndex) as SyntaxNode;
                    if (child != null)
                    {
                        node = child;
                        childIndex = 0;

                        // before walking children
                        fnBefore?.Invoke(node);
                    }
                    else
                    {
                        childIndex++;
                    }
                }
                else
                {
                    // after walking children
                    fnAfter?.Invoke(node);

                    // stop if we are done with root node
                    if (node == root)
                        break;

                    // walk up
                    childIndex = node.IndexInParent + 1;
                    node = node.Parent;
                }
            }
        }

        /// <summary>
        /// Gets the next <see cref="SyntaxElement"/> sibling of this element or null if there is no next sibling.
        /// </summary>
        public SyntaxElement GetNextSibling(bool includeZeroWidthElements = false)
        {
            if (this.Parent != null)
            {
                for (int i = this.IndexInParent + 1, n = this.Parent.ChildCount; i < n && i >= 0; i++)
                {
                    var sibling = this.Parent.GetChild(i);
                    if (sibling != null && (includeZeroWidthElements || sibling.FullWidth > 0))
                        return sibling;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the previous <see cref="SyntaxElement"/> sibling of this element or null if there is no previous sibling.
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
            return GetNextToken(this, null, includeZeroWidthTokens);
        }

        /// <summary>
        /// Gets the last descendant token of this <see cref="SyntaxElement"/> in lexical order.
        /// </summary>
        public SyntaxToken GetLastToken(bool includeZeroWidthTokens = false)
        {
            return GetPreviousToken(this, null, includeZeroWidthTokens);
        }

        protected static SyntaxToken GetNextToken(SyntaxElement root, SyntaxToken token, bool includeZeroWidthTokens)
        {
            var node = token != null ? token.Parent : root;
            var childIndex = token != null ? token.IndexInParent + 1: 0;

            while (node != null)
            {
                if (childIndex < node.ChildCount && childIndex >= 0)
                {
                    var child = node.GetChild(childIndex);
                    if (child != null)
                    {
                        node = child;
                        childIndex = 0;

                        if (node is SyntaxToken t && (includeZeroWidthTokens || t.FullWidth > 0))
                        {
                            return t;
                        }
                    }
                    else
                    {
                        childIndex++;
                    }
                }
                else if (node == root)
                {
                    return null;
                }
                else
                {
                    childIndex = node.IndexInParent + 1;
                    node = node.Parent;
                }
            }

            return null;
        }

        protected static SyntaxToken GetPreviousToken(SyntaxElement root, SyntaxToken token, bool includeZeroWidthTokens)
        {
            var node = token != null ? token.Parent : root;
            var childIndex = token != null ? token.IndexInParent - 1 : root.ChildCount - 1;

            while (node != null)
            {
                if (childIndex < node.ChildCount && childIndex >= 0)
                {
                    var child = node.GetChild(childIndex);
                    if (child != null)
                    {
                        node = child;
                        childIndex = node.ChildCount - 1;

                        if (node is SyntaxToken t && (includeZeroWidthTokens || t.FullWidth > 0))
                        {
                            return t;
                        }
                    }
                    else
                    {
                        childIndex--;
                    }
                }
                else if (node == root)
                {
                    return null;
                }
                else
                {
                    childIndex = node.IndexInParent - 1;
                    node = node.Parent;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the token at the specified position in the source text.
        /// If the position is within trivia, it will find the next token after the trivia.
        /// If the position is past the end of the tree, it will return the last token.
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
        public int TriviaStart => _triviaStart >= 0 ? _triviaStart : ComputeTriviaStart();

        /// <summary>
        /// The position in the source of the start of the leading trivia.
        /// </summary>
        private int _triviaStart = -1;

        internal void InitializeTriviaStarts()
        {
            SyntaxElement.WalkElements(
                this.Root,
                fnBefore: element =>
                {
                    System.Diagnostics.Debug.Assert(element.Parent == null || element.Parent._triviaStart >= 0);
                    element._triviaStart = (element.Parent?._triviaStart ?? 0) + element.OffsetInParent;
                });
        }

        protected int ComputeTriviaStart()
        {
            if (this.Parent == null)
            {
                return 0;
            }
            else if (this.Parent.Parent == null)
            {
                return this.OffsetInParent;
            }
            else
            {
                var totalOffset = 0;

                for (var node = this; node != null; node = node.Parent)
                {
                    totalOffset += node.OffsetInParent;
                }

                return totalOffset;
            }
        }

        /// <summary>
        /// The full width (in characters) of this element including leading trivia.
        /// </summary>
        public virtual int FullWidth => 0;

        protected int ComputeFullWidth()
        {
            int width = 0;

            for (int i = 0, n = this.ChildCount; i < n; i++)
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
            return ToString(includeTrivia, Int32.MaxValue);
        }

        public string ToString(IncludeTrivia includeTrivia, int maxLength)
        {
            var builder = new StringBuilder();
            var start = this.TriviaStart;

            this.WalkTokens(token =>
            {
                if (builder.Length < maxLength)
                {
                    token.Write(builder, includeTrivia, start);
                }
            });

            if (builder.Length > maxLength)
            {
                builder.Length = maxLength;
            }
            
            return builder.ToString();
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

        /// <summary>
        /// Same as <see cref="Minimal"/> except no line breaks are preserved.
        /// </summary>
        SingleLine
    }
}