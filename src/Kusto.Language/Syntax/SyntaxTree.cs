using System;

namespace Kusto.Language.Syntax
{
    public class SyntaxTree
    {
        /// <summary>
        /// The root <see cref="SyntaxNode"/> of the <see cref="SyntaxTree"/>
        /// </summary>
        public SyntaxNode Root { get; }

        /// <summary>
        /// If not null, then the syntax tree this tree fragment was copied from.
        /// </summary>
        public SyntaxTree Original { get; }

        /// <summary>
        /// The position that this syntax tree fragment starts within the original tree it was copied from.
        /// </summary>
        public int OffsetInOriginal { get; }

        public SyntaxTree(SyntaxNode root, SyntaxTree original = null, int offsetInOriginal = 0)
        {
            this.Root = root;
            this.Original = original;
            this.OffsetInOriginal = offsetInOriginal;
            root.SetTree(this);
            root.InitializeTriviaStarts();
        }

        private int _depth = -1;

        /// <summary>
        /// The maximal depth of the nodes in the tree.
        /// </summary>
        public int Depth
        {
            get
            {
                if (_depth == -1)
                {
                    _depth = ComputeMaxDepth(this.Root);
                }

                return _depth;
            }
        }

        /// <summary>
        /// True if the tree depth is shallow enough to allow stack recursion
        /// to walk the nodes of this tree.
        /// </summary>
        internal bool IsSafeToRecurse(GlobalState state) => 
            Depth <= state.GetProperty(Properties.MaxAnalysisDepth);

        /// <summary>
        /// Walks the entire syntax tree and evaluates the maximum depth of all the nodes.
        /// </summary>
        private static int ComputeMaxDepth(SyntaxNode root)
        {
            var maxDepth = 0;
            var depth = 0;

            SyntaxElement.WalkNodes(
                root,
                fnBefore: e =>
                {
                    depth++;
                    if (depth > maxDepth)
                        maxDepth = depth;
                },
                fnAfter: e => depth--);

            return maxDepth;
        }
    }
}