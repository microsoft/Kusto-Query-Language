namespace Kusto.Language.Editor
{
    /// <summary>
    /// The location of a reference to a database in the text of the code.
    /// </summary>
    public class DatabaseReference : SyntaxReference
    {
        /// <summary>
        /// The name of the database that is explicitly referenced.
        /// </summary>
        public string Database { get; }

        /// <summary>
        /// The cluster that the database is associated with, either implied or explicitly referenced.
        /// </summary>
        public string Cluster { get; }

        public DatabaseReference(string database, string cluster, int start, int length)
            : base(start, length)
        {
            this.Database = database;
            this.Cluster = cluster;
        }
    }
}