using System;
using System.Collections.Generic;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol representing a graph
    /// </summary>
    public class GraphSymbol : TypeSymbol
    {
        /// <summary>
        /// The shape an edge in the graph.
        /// </summary>
        public TableSymbol EdgeShape { get; }

        /// <summary>
        /// The shape of an node in the graph.
        /// This may be null.
        /// </summary>
        public TableSymbol NodeShape { get; }

        public GraphSymbol(string name, TableSymbol edgeShape, TableSymbol nodeShape = null)
            : base(name)
        {
            this.EdgeShape = edgeShape ?? TableSymbol.Empty;
            this.NodeShape = nodeShape;
        }

        public GraphSymbol(string name, TableSymbol edgeShape, IReadOnlyList<TableSymbol> nodeShapes)
            : base(name)
        {
            this.EdgeShape = edgeShape ?? TableSymbol.Empty;
            this.NodeShape = nodeShapes != null && nodeShapes.Count > 0 ? TableSymbol.Combine(CombineKind.UnifySameName, nodeShapes) : null;
        }

        public GraphSymbol(string name, IReadOnlyList<TableSymbol> edgeShapes, IReadOnlyList<TableSymbol> nodeShapes)
            : base(name)
        {
            this.EdgeShape = edgeShapes != null && edgeShapes.Count > 0 ? TableSymbol.Combine(CombineKind.UnifySameName, edgeShapes) : null;
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

        public GraphSymbol(IReadOnlyList<TableSymbol> edgeShapes, IReadOnlyList<TableSymbol> nodeShapes)
            : this("", edgeShapes, nodeShapes)
        {
        }

        public GraphSymbol(string name, string edgeSchema, string nodeSchema = null)
            : this(name, 
                  edgeSchema != null ? TableSymbol.From(edgeSchema) : null, 
                  nodeSchema != null ? TableSymbol.From(nodeSchema) : null)
        {
        }

        public override SymbolKind Kind => SymbolKind.Graph;
        public override Tabularity Tabularity => Tabularity.Other;

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

        /// <summary>
        /// Create a graph symbol from edge and node schemas only.
        /// </summary>
        public static GraphSymbol From(string edgeSchema, string nodeSchema = null)
        {
            return new GraphSymbol("", edgeSchema, nodeSchema);
        }

        public static readonly GraphSymbol Empty =
            new GraphSymbol("", TableSymbol.Empty, TableSymbol.Empty);
    }
}