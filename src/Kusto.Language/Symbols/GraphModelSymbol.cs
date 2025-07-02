using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol representing a graph model.
    /// </summary>
    public sealed class GraphModelSymbol : TypeSymbol
    {
        /// <summary>
        /// All queries defining edge tables.
        /// </summary>
        public IReadOnlyList<Signature> Edges { get; }

        /// <summary>
        /// All queries defining node tables.
        /// </summary>
        public IReadOnlyList<Signature> Nodes { get; }

        /// <summary>
        /// All named snapshots.
        /// </summary>
        public IReadOnlyList<GraphSnapshotSymbol> Snapshots { get; }

        public GraphModelSymbol(
            string name,
            IEnumerable<Signature> edges,
            IEnumerable<Signature> nodes,
            IEnumerable<GraphSnapshotSymbol> snapshots)      
            : base(name)
        {
            this.Snapshots = snapshots.ToReadOnly();
            this.Edges = edges.ToReadOnly();
            this.Nodes = nodes.ToReadOnly();
            
            foreach (var edge in this.Edges)
            {
                edge.Symbol = this;
            }

            foreach (var node in this.Nodes)
            {
                node.Symbol = this;
            }

            foreach (var snapshot in this.Snapshots)
            {
                snapshot.Model = this;
            }
        }

        public GraphModelSymbol(
            string name,
            IEnumerable<string> edges = null,
            IEnumerable<string> nodes = null,
            IEnumerable<string> snapshots = null)
            : this(
                  name,
                  edges != null ? edges.Select(e => new Signature(e, Tabularity.Unknown)) : null,
                  nodes != null ? nodes.Select(n => new Signature(n, Tabularity.Unknown)) : null,
                  snapshots != null ? snapshots.Select(sn => new GraphSnapshotSymbol(sn)) : null
                  )
        {
        }

        public GraphModelSymbol(
            string name,
            string edge,
            string node = null,
            IEnumerable<string> snapshots = null)
            : this(
                  name,
                  edge != null ? new[] { edge } : null,
                  node != null ? new[] { node } : null,
                  snapshots
                  )
        {
        }

        public override Tabularity Tabularity => Tabularity.None;
        public override SymbolKind Kind => SymbolKind.GraphModel;
        public override IReadOnlyList<Symbol> Members => this.Snapshots;

        private Dictionary<string, GraphSnapshotSymbol> _snapshotMap;

        /// <summary>
        /// Returns the snapshot of the specified name or null if it does not exist.
        /// </summary>
        public bool TryGetSnapshot(string name, out GraphSnapshotSymbol snapshot)
        {
            if (_snapshotMap == null)
            {
                var tmp = this.Snapshots.ToDictionaryLast(sn => sn.Name);
                Interlocked.CompareExchange(ref _snapshotMap, tmp, null);
            }

            return _snapshotMap.TryGetValue(name, out snapshot);
        }

        internal GraphSymbol ComputedGraphSymbol { get; set; }
    }

    public class GraphSnapshotSymbol : Symbol
    {
        public GraphModelSymbol Model { get; internal set; }

        public GraphSnapshotSymbol(string name)
            : base(name)
        {
        }

        public override Tabularity Tabularity => Tabularity.None;
        public override SymbolKind Kind => SymbolKind.GraphSnapshot;
    }
}