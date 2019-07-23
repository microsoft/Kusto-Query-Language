namespace Kusto.Language.Editor
{
    /// <summary>
    /// The location of a reference to a cluster in the text of the code.
    /// </summary>
    public class ClusterReference : SyntaxReference
    {
        /// <summary>
        /// The name of the cluster referenced.
        /// </summary>
        public string Cluster { get; }

        public ClusterReference(string cluster, int start, int length)
            : base(start, length)
        {
            this.Cluster = cluster;
        }
    }
}