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
        private readonly SafeList<Symbol> _members;

        /// <summary>
        /// If true, then the definition of the cluster is not fully known.
        /// </summary>
        public bool IsOpen { get; }

        /// <summary>
        /// Creates a new instance of a <see cref="ClusterSymbol"/>.
        /// </summary>
        private ClusterSymbol(string name, IEnumerable<Symbol> members, bool isOpen = false)
            : base(name)
        {
            _members = members.ToSafeList();
            _members.CheckArgumentNullOrElementNull(nameof(members));
            this.IsOpen = isOpen;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="ClusterSymbol"/>.
        /// </summary>
        public ClusterSymbol(string name, IEnumerable<DatabaseSymbol> databases, bool isOpen = false)
            : this(name, (IReadOnlyList<Symbol>)databases, isOpen)
        {
        }

        /// <summary>
        /// Creates a new instance of a <see cref="ClusterSymbol"/>.
        /// </summary>
        public ClusterSymbol(string name, params DatabaseSymbol[] databases)
            : this(name, databases, false)
        {
        }

        public override IReadOnlyList<Symbol> Members => _members;

        public override SymbolKind Kind => SymbolKind.Cluster;

        public override Tabularity Tabularity => Tabularity.Tabular;

        /// <summary>
        /// Creates a new <see cref="ClusterSymbol"/> with the specified members.
        /// </summary>
        public ClusterSymbol AddMembers(IEnumerable<Symbol> members)
        {
            var newMembers = _members.Concat(members);
            return new ClusterSymbol(this.Name, newMembers, this.IsOpen);
        }

        /// <summary>
        /// Creates a new <see cref="ClusterSymbol"/> with the specified members.
        /// </summary>
        public ClusterSymbol AddMembers(params Symbol[] members)
        {
            return AddMembers((IReadOnlyList<Symbol>)members);
        }

        /// <summary>
        /// The databases associated with this cluster.
        /// </summary>
        public IReadOnlyList<DatabaseSymbol> Databases
        {
            get
            {
                if (_databases == null)
                {
                    _databases = _members.OfType<DatabaseSymbol>().ToReadOnly();
                }

                return _databases;
            }
        }

        private IReadOnlyList<DatabaseSymbol> _databases;
        private Dictionary<string, DatabaseSymbol> _nameToDatabaseMap;

        /// <summary>
        /// Gets the database with the specified name or returns null.
        /// </summary>
        public DatabaseSymbol GetDatabase(string databaseName)
        {
            if (string.IsNullOrEmpty(databaseName))
                return null;

            if (_nameToDatabaseMap == null)
            {
                var tmp = new Dictionary<string, DatabaseSymbol>();
                
                foreach (var db in this.Databases)
                {
                    tmp[db.Name] = db;
                    if (!string.IsNullOrEmpty(db.AlternateName))
                        tmp[db.AlternateName] = db;
                }

                Interlocked.CompareExchange(ref _nameToDatabaseMap, tmp, null);
            }

            _nameToDatabaseMap.TryGetValue(databaseName, out var databaseSymbol);
            return databaseSymbol;
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
            var newMembers = _members.AddItem(database);
            return new ClusterSymbol(this.Name, newMembers, this.IsOpen);
        }

        /// <summary>
        /// Creates a new <see cref="ClusterSymbol"/> with existing database replaced with the new database.
        /// </summary>
        public ClusterSymbol UpdateDatabase(DatabaseSymbol existingDatabase, DatabaseSymbol newDatabase)
        {
            var newMembers = _members.Select(d => d == existingDatabase ? newDatabase : d);
            return new ClusterSymbol(this.Name, newMembers, this.IsOpen);
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
            var newMembers = _members.Where(m => m != symbolToRemove).ToReadOnly();
            return new ClusterSymbol(this.Name, newMembers, this.IsOpen);
        }

        /// <summary>
        /// Creates a new <see cref="ClusterSymbol"/> with the specified databases removed.
        /// </summary>
        public ClusterSymbol RemoveDatabases(IEnumerable<DatabaseSymbol> symbolsToRemove)
        {
            var newMembers = _members.Except(symbolsToRemove).ToReadOnly();
            return new ClusterSymbol(this.Name, newMembers, this.IsOpen);
        }

        /// <summary>
        /// The symbol used to represent unknown clusters.
        /// </summary>
        public static readonly ClusterSymbol Unknown = 
            new ClusterSymbol("", members: null, isOpen: true);
    }
}