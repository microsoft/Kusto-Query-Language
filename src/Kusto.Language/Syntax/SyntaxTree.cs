using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kusto.Language.Editor;

namespace Kusto.Language.Syntax
{
    using Utils;

    internal class SyntaxTree
    {
        /// <summary>
        /// The root <see cref="SyntaxNode"/> of the <see cref="SyntaxTree"/>
        /// </summary>
        public SyntaxNode Root { get; }

        public SyntaxTree(SyntaxNode root)
        {
            this.Root = root;
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
        public bool IsSafeToRecurse => Depth <= KustoCode.MaxAnalyzableSyntaxDepth;

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