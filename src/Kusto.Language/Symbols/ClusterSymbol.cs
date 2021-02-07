using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Symbols
{
    using Utils;

    /// <summary>
    /// A symbol representing a cluster.
    /// </summary>
    public sealed class ClusterSymbol : TypeSymbol
    {
        public IReadOnlyList<DatabaseSymbol> Databases { get; }

        public override IReadOnlyList<Symbol> Members => this.Databases;

        public override SymbolKind Kind => SymbolKind.Cluster;

        /// <summary>
        /// If true, then the definition of the cluster is not fully known.
        /// </summary>
        public bool IsOpen { get; }

        /// <summary>
        /// Creates a new instance of a <see cref="ClusterSymbol"/>.
        /// </summary>
        public ClusterSymbol(string name, IEnumerable<DatabaseSymbol> databases, bool isOpen = false)
            : base(name)
        {
            this.Databases = databases.ToReadOnly();
            this.IsOpen = isOpen;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="ClusterSymbol"/>.
        /// </summary>
        public ClusterSymbol(string name, params DatabaseSymbol[] databases)
            : this(name, databases, false)
        {
        }

        public override Tabularity Tabularity => Tabularity.Tabular;

        /// <summary>
        /// Returns true if name matches this cluster's name.
        /// </summary>
        public bool IsCluster(string name)
        {
            name = KustoFacts.GetHostName(name) ?? name;
            return KustoFacts.IsClusterHostName(name, this.Name)
                || KustoFacts.IsClusterShortName(name, this.Name);
        }

        /// <summary>
        /// Gets the database with the specified name or returns null.
        /// </summary>
        public DatabaseSymbol GetDatabase(string databaseName)
        {
            return this.Databases.FirstOrDefault(d => d.Name == databaseName);
        }

        /// <summary>
        /// Creates a new <see cref="ClusterSymbol"/> with the specified list of databases.
        /// </summary>
        public ClusterSymbol WithDatabases(IEnumerable<DatabaseSymbol> databases)
        {
            return new ClusterSymbol(this.Name, databases, this.IsOpen);
        }

        /// <summary>
        /// Creates a new <see cref="ClusterSymbol"/> with the specified database added.
        /// </summary>
        public ClusterSymbol AddDatabase(DatabaseSymbol database)
        {
            var newDatabases = this.Databases.Concat(new[] { database });
            return new ClusterSymbol(this.Name, newDatabases, this.IsOpen);
        }

        /// <summary>
        /// Creates a new <see cref="ClusterSymbol"/> with existing database replaced with the new database.
        /// </summary>
        public ClusterSymbol UpdateDatabase(DatabaseSymbol existingDatabase, DatabaseSymbol newDatabase)
        {
            var newDatabases = this.Databases.Select(d => d == existingDatabase ? newDatabase : d);
            return new ClusterSymbol(this.Name, newDatabases, this.IsOpen);
        }

        /// <summary>
        /// Creates a new <see cref="ClusterSymbol"/> with database added or replacing an existing database with the same name.
        /// </summary>
        public ClusterSymbol AddOrUpdateDatabase(DatabaseSymbol newDatabase)
        {
            var existingDatabase = this.GetDatabase(newDatabase.Name);
            if (existingDatabase != null)
            {
                return this.UpdateDatabase(existingDatabase, newDatabase);
            }
            else
            {
                return this.AddDatabase(newDatabase);
            }
        }

        /// <summary>
        /// Creates a new <see cref="ClusterSymbol"/> with the specified database removed.
        /// </summary>
        public ClusterSymbol RemoveDatabase(DatabaseSymbol symbolToRemove)
        {
            var newDatabases = this.Databases.Where(d => d != symbolToRemove).ToReadOnly();
            return new ClusterSymbol(this.Name, newDatabases, this.IsOpen);
        }

        /// <summary>
        /// Creates a new <see cref="ClusterSymbol"/> with the specified databases removed.
        /// </summary>
        public ClusterSymbol RemoveDatabases(IEnumerable<DatabaseSymbol> symbolsToRemove)
        {
            var newDatabases = this.Databases.Except(symbolsToRemove).ToReadOnly();
            return new ClusterSymbol(this.Name, newDatabases, this.IsOpen);
        }

        protected override string GetDisplay() =>
            $"cluster({this.Name})";


        public static readonly ClusterSymbol Unknown = new ClusterSymbol("", databases: null, isOpen: true);
    }
}