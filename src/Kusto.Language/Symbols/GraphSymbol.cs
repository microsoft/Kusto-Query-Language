using System;
using System.Collections.Generic;

namespace Kusto.Language.Symbols
{
    /// <summary>
    /// A symbol representing a graph
    /// </summary>
    public class GraphSymbol : TypeSymbol
    {
        public TableSymbol EdgeShape { get; }
        public TableSymbol NodeShape { get; }

        public GraphSymbol(string name, TableSymbol edgeShape, TableSymbol nodeShape = null)
            : base(name)
        {
            if (edgeShape == null)
                throw new ArgumentNullException(nameof(edgeShape));

            this.EdgeShape = edgeShape;
            this.NodeShape = nodeShape;
        }

        public GraphSymbol(string name, TableSymbol edgeShape, IReadOnlyList<TableSymbol> nodeShapes)
            : base(name)
        {
            if (edgeShape == null)
                throw new ArgumentNullException(nameof(edgeShape));

            this.EdgeShape = edgeShape;
            this.NodeShape = nodeShapes != null && nodeShapes.Count > 0 ? TableSymbol.Combine(CombineKind.UnifySameName, nodeShapes) : null;
        }

        public GraphSymbol(TableSymbol edgeShape, TableSymbol nodeShape = null)
            : this("", edgeShape, nodeShape)
        {
        }

        public GraphSymbol(TableSymbol edgeShape, IReadOnlyList<TableSymbol> nodeShapes)
            : this("", edgeShape, nodeShapes)
        {
        }

        public override SymbolKind Kind => SymbolKind.Graph;
        public override Tabularity Tabularity => Tabularity.Graph;

        public GraphSymbol WithName(string name)
        {
            if (name != null)
            {
                return new GraphSymbol(name, this.EdgeShape, this.NodeShape);
            }
            else
            {
                return this;
            }
        }

        public GraphSymbol WithEdgeShape(TableSymbol edgeShape)
        {
            if (this.EdgeShape != edgeShape)
            {
                return new GraphSymbol(this.Name, edgeShape, this.NodeShape);
            }
            else
            {
                return this;
            }
        }

        public GraphSymbol WithNodeShape(TableSymbol nodeShape)
        {
            if (this.NodeShape != null)
            {
                return new GraphSymbol(this.Name, this.EdgeShape, nodeShape);
            }
            else
            {
                return this;
            }
        }

        protected override string GetDisplay()
        {
            if (this.Name.Length > 0)
            {
                if (this.NodeShape != null)
                {
                    return $"{this.Name}: [Edge{this.EdgeShape.Display}, Node{this.NodeShape.Display}]";
                }
                else
                {
                    return $"{this.Name}: [Edge{this.EdgeShape.Display}]";
                }
            }
            else
            {
                if (this.NodeShape != null)
                {
                    return $"[Edge{this.EdgeShape.Display}, Node{this.NodeShape.Display}]";
                }
                else
                {
                    return $"[Edge{this.EdgeShape.Display}]";
                }
            }
        }

        /// <summary>
        /// Merges two graph definitions together by merging the edge and node shapes.
        /// </summary>
        public static GraphSymbol Merge(GraphSymbol leftGraph, GraphSymbol rightGraph)
        {
            if (leftGraph != null && rightGraph != null)
            {
                var newEdgeShape = TableSymbol.Combine(CombineKind.UnifySameName, leftGraph.EdgeShape, rightGraph.EdgeShape);

                TableSymbol newNodeShape = null;
                if (leftGraph.NodeShape != null && rightGraph.NodeShape != null)
                {
                    newNodeShape = TableSymbol.Combine(CombineKind.UnifySameName, leftGraph.NodeShape, rightGraph.NodeShape);
                }
                else if (leftGraph.NodeShape != null)
                {
                    newNodeShape = leftGraph.NodeShape;
                }
                else if (rightGraph.NodeShape != null)
                {
                    newNodeShape = rightGraph.NodeShape;
                }

                return new GraphSymbol(newEdgeShape, newNodeShape);
            }
            else if (leftGraph != null)
            {
                return leftGraph;
            }
            else if (rightGraph != null)
            {
                return rightGraph;
            }
            else
            {
                return null;
            }
        }
    }
}