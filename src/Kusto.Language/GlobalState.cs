using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Symbols;
    using Utils;

    /// <summary>
    /// The global state that a kusto query is associated with.
    /// </summary>
    public sealed class GlobalState
    {
        /// <summary>
        /// Known clusters
        /// </summary>
        public IReadOnlyList<ClusterSymbol> Clusters { get; }

        /// <summary>
        /// The default cluster.
        /// </summary>
        public ClusterSymbol Cluster { get; }

        /// <summary>
        /// The default database.
        /// </summary>
        public DatabaseSymbol Database { get; }

        /// <summary>
        /// Known functions.
        /// </summary>
        public IReadOnlyList<FunctionSymbol> Functions { get; }

        /// <summary>
        /// Known aggregates.
        /// </summary>
        public IReadOnlyList<FunctionSymbol> Aggregates { get; }

        /// <summary>
        /// Known plug-ins.
        /// </summary>
        public IReadOnlyList<FunctionSymbol> PlugIns { get; }

        /// <summary>
        /// Scalar operators
        /// </summary>
        public IReadOnlyList<OperatorSymbol> Operators { get; }

        /// <summary>
        /// Supported commands
        /// </summary>
        public IReadOnlyList<CommandSymbol> Commands { get; }

        /// <summary>
        /// Ambient parameters
        /// </summary>
        public IReadOnlyList<ParameterSymbol> Parameters { get; }

        /// <summary>
        /// Known query options
        /// </summary>
        public IReadOnlyList<OptionSymbol> Options { get; }

        /// <summary>
        /// The <see cref="KustoCache"/> used to store additional accumulated global state.
        /// If caching is not enabled for this <see cref="GlobalState"/> this property will return null.
        /// </summary>
        public KustoCache Cache { get; }

        /// <summary>
        /// Name to aggregate lookup map
        /// </summary>
        private Dictionary<string, FunctionSymbol> aggregatesMap;

        /// <summary>
        /// Name to function lookup map
        /// </summary>
        private Dictionary<string, FunctionSymbol> functionsMap;

        /// <summary>
        /// Name to plugin lookup map
        /// </summary>
        private Dictionary<string, FunctionSymbol> pluginMap;

        /// <summary>
        /// Name to operator lookup map
        /// </summary>
        private Dictionary<OperatorKind, OperatorSymbol> operatorMap;

        /// <summary>
        /// Name to command lookup map
        /// </summary>
        private Dictionary<string, CommandSymbol> commandMap;

        /// <summary>
        /// Name to <see cref="OptionSymbol"/> lookup map
        /// </summary>
        private Dictionary<string, OptionSymbol> optionMap;

        /// <summary>
        /// Symbol (database) to <see cref="ClusterSymbol"/> reverse lookup map
        /// </summary>
        private Dictionary<Symbol, ClusterSymbol> reverseClusterMap;

        /// <summary>
        /// Symbol to <see cref="DatabaseSymbol"/> reverse lookup map
        /// </summary>
        private Dictionary<Symbol, DatabaseSymbol> reverseDatabaseMap;

        /// <summary>
        /// Column to <see cref="TableSymbol"/> reverse lookup map
        /// </summary>
        private Dictionary<Symbol, TableSymbol> reverseTableMap;

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> instance.
        /// </summary>
        private GlobalState(
            IReadOnlyList<ClusterSymbol> clusters,
            ClusterSymbol cluster,
            DatabaseSymbol database,
            IReadOnlyList<FunctionSymbol> functions,
            IReadOnlyList<FunctionSymbol> aggregates,
            IReadOnlyList<FunctionSymbol> plugins,
            IReadOnlyList<OperatorSymbol> operators,
            IReadOnlyList<CommandSymbol> commands,
            IReadOnlyList<ParameterSymbol> parameters,
            IReadOnlyList<OptionSymbol> options,
            KustoCache cache,
            Dictionary<Symbol, ClusterSymbol> reverseClusterMap,
            Dictionary<Symbol, DatabaseSymbol> reverseDatabaseMap,
            Dictionary<Symbol, TableSymbol> reverseTableMap,
            Dictionary<string, FunctionSymbol> functionsMap,
            Dictionary<string, FunctionSymbol> aggregatesMap,
            Dictionary<string, FunctionSymbol> pluginMap,
            Dictionary<OperatorKind, OperatorSymbol> operatorMap,
            Dictionary<string, CommandSymbol> commandMap,
            Dictionary<string, OptionSymbol> optionMap)
        {
            this.Clusters = clusters ?? EmptyReadOnlyList<ClusterSymbol>.Instance;
            this.Cluster = cluster ?? ClusterSymbol.Unknown;
            this.Database = database ?? DatabaseSymbol.Unknown;
            this.Functions = functions ?? EmptyReadOnlyList<FunctionSymbol>.Instance;
            this.Aggregates = aggregates ?? EmptyReadOnlyList<FunctionSymbol>.Instance;
            this.PlugIns = plugins ?? EmptyReadOnlyList<FunctionSymbol>.Instance;
            this.Operators = operators ?? EmptyReadOnlyList<OperatorSymbol>.Instance;
            this.Commands = commands ?? EmptyReadOnlyList<CommandSymbol>.Instance;
            this.Parameters = parameters ?? EmptyReadOnlyList<ParameterSymbol>.Instance;
            this.Options = options ?? EmptyReadOnlyList<OptionSymbol>.Instance;
            this.Cache = cache != null ? cache.WithGlobals(this) : null;
            this.reverseClusterMap = reverseClusterMap;
            this.reverseDatabaseMap = reverseDatabaseMap;
            this.reverseTableMap = reverseTableMap;
            this.functionsMap = functionsMap;
            this.aggregatesMap = aggregatesMap;
            this.pluginMap = pluginMap;
            this.operatorMap = operatorMap;
            this.commandMap = commandMap;
            this.optionMap = optionMap;
        }

        /// <summary>
        /// Makes a new instance of this <see cref="GlobalState"/> that contains the same content,
        /// but possibly clears the cache.
        /// </summary>
        public GlobalState Copy()
        {
            return new GlobalState(
                this.Clusters,
                this.Cluster,
                this.Database,
                this.Functions,
                this.Aggregates,
                this.PlugIns,
                this.Operators,
                this.Commands,
                this.Parameters,
                this.Options,
                this.Cache,
                this.reverseClusterMap,
                this.reverseDatabaseMap,
                this.reverseTableMap,
                this.functionsMap,
                this.aggregatesMap,
                this.pluginMap,
                this.operatorMap,
                this.commandMap,
                this.optionMap);
        }

        /// <summary>
        /// Conditionally creates a new instance of a <see cref="GlobalState"/> if one of the 
        /// optional arguments is different than the current corresponding value.
        /// </summary>
        private GlobalState With(
            Optional<IReadOnlyList<ClusterSymbol>> clusters = default(Optional<IReadOnlyList<ClusterSymbol>>),
            Optional<ClusterSymbol> cluster = default(Optional<ClusterSymbol>),
            Optional<DatabaseSymbol> database = default(Optional<DatabaseSymbol>),
            Optional<IReadOnlyList<FunctionSymbol>> functions = default(Optional<IReadOnlyList<FunctionSymbol>>),
            Optional<IReadOnlyList<FunctionSymbol>> aggregates = default(Optional<IReadOnlyList<FunctionSymbol>>),
            Optional<IReadOnlyList<FunctionSymbol>> plugins = default(Optional<IReadOnlyList<FunctionSymbol>>),
            Optional<IReadOnlyList<OperatorSymbol>> operators = default(Optional<IReadOnlyList<OperatorSymbol>>),
            Optional<IReadOnlyList<CommandSymbol>> commands = default(Optional<IReadOnlyList<CommandSymbol>>),
            Optional<IReadOnlyList<ParameterSymbol>> parameters = default(Optional<IReadOnlyList<ParameterSymbol>>),
            Optional<IReadOnlyList<OptionSymbol>> options = default(Optional<IReadOnlyList<OptionSymbol>>),
            Optional<KustoCache> cache = default(Optional<KustoCache>))
        {
            var useClusters = clusters.HasValue ? clusters.Value : this.Clusters;
            var useCluster = cluster.HasValue ? cluster.Value : this.Cluster;
            var useDatabase = database.HasValue ? database.Value : this.Database;
            var useFunctions = functions.HasValue ? functions.Value : this.Functions;
            var useAggregates = aggregates.HasValue ? aggregates.Value : this.Aggregates;
            var usePlugins = plugins.HasValue ? plugins.Value : this.PlugIns;
            var useOperators = operators.HasValue ? operators.Value : this.Operators;
            var useCommands = commands.HasValue ? commands.Value : this.Commands;
            var useParameters = parameters.HasValue ? parameters.Value : this.Parameters;
            var useOptions = options.HasValue ? options.Value : this.Options;
            var useCache = cache.HasValue ? cache.Value : this.Cache;

            if (useClusters != this.Clusters
                || useCluster != this.Cluster
                || useDatabase != this.Database
                || useFunctions != this.Functions
                || useAggregates != this.Aggregates
                || usePlugins != this.PlugIns
                || useOperators != this.Operators
                || useCommands != this.Commands
                || useParameters != this.Parameters
                || useOptions != this.Options
                || useCache != this.Cache)
            {
                return new GlobalState(
                    useClusters,
                    useCluster,
                    useDatabase,
                    useFunctions,
                    useAggregates,
                    usePlugins,
                    useOperators,
                    useCommands,
                    useParameters,
                    useOptions,
                    useCache,
                    useClusters == this.Clusters ? this.reverseClusterMap : null,
                    useClusters == this.Clusters ? this.reverseDatabaseMap : null,
                    useClusters == this.Clusters ? this.reverseTableMap : null,
                    useFunctions == this.Functions ? this.functionsMap : null,
                    useAggregates == this.Aggregates ? this.aggregatesMap : null,
                    usePlugins == this.PlugIns ? this.pluginMap : null,
                    useOperators == this.Operators ? this.operatorMap : null,
                    useCommands == this.Commands ? this.commandMap : null,
                    useOptions == this.Options ? this.optionMap : null);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with caching enabled.
        /// </summary>
        public GlobalState WithCache()
        {
            if (this.Cache != null)
            {
                return this;
            }
            else
            {
                return With(cache: new KustoCache(this));
            }
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified cluster list.
        /// </summary>
        public GlobalState WithClusterList(IReadOnlyList<ClusterSymbol> clusters)
        {
            if (this.Clusters == clusters)
            {
                return this;
            }
            else
            {
                // change the set of clusters and update current cluster in case its symbol was updated
                var newCluster = clusters.FirstOrDefault(c => c.Name == this.Cluster.Name) ?? ClusterSymbol.Unknown;
                return With(clusters: Optional(clusters)).WithCluster(newCluster);
            }
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with either
        /// the cluster with the same name replaced with the new cluster
        /// or the new cluster added.
        /// </summary>
        public GlobalState AddOrReplaceCluster(ClusterSymbol cluster)
        {
            if (cluster == ClusterSymbol.Unknown
                || cluster == this.Cluster
                || this.Clusters.Contains(cluster))
            {
                return this;
            }
            else
            {
                var newClusters = AddOrReplace(this.Clusters, cluster);
                return WithClusterList(newClusters);
            }
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified default cluster.
        /// </summary>
        public GlobalState WithCluster(ClusterSymbol cluster)
        {
            if (this.Cluster == cluster)
            {
                return this;
            }
            else if (cluster == ClusterSymbol.Unknown)
            {
                return With(cluster: ClusterSymbol.Unknown, database: DatabaseSymbol.Unknown);
            }
            else if (this.Clusters.Contains(cluster))
            {
                // this is a known cluster, so change current and try to set current database to one with same name
                var newDb = cluster.GetDatabase(this.Database.Name) ?? DatabaseSymbol.Unknown;
                return With(cluster: cluster, database: newDb);
            }
            else
            {
                // add new cluster or replace existing cluster with same name
                var newClusters = AddOrReplace(this.Clusters, cluster);
                return WithClusterList(newClusters).WithCluster(cluster);
            }
        }

        private static IReadOnlyList<T> AddOrReplace<T>(IReadOnlyList<T> list, T newElement)
            where T: Symbol
        {
            var existingElement = list.FirstOrDefault(s => s.Name == newElement.Name);
            if (existingElement == newElement)
            {
                return list;
            }
            else
            {
                var newList = new List<T>(list);
                if (existingElement != null)
                {
                    var index = newList.IndexOf(existingElement);
                    if (index >= 0)
                    {
                        newList[index] = newElement;
                    }
                    else
                    {
                        newList.Add(newElement);
                    }
                }
                else
                {
                    newList.Add(newElement);
                }

                return newList;
            }
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified default database.
        /// </summary>
        public GlobalState WithDatabase(DatabaseSymbol database)
        {
            database = database ?? DatabaseSymbol.Unknown;

            if (this.Database == database)
            {
                return this;
            }
            else if (database == DatabaseSymbol.Unknown
                || this.Cluster.Databases.Contains(database))
            {
                // same cluster, just change database
                return With(database: database);
            }
            else
            {
                // check if it is a database of some other known cluster
                var knownCluster = GetCluster(database);
                if (knownCluster != null)
                {
                    // changing the current database changes the current cluster too
                    return With(cluster: knownCluster, database: database);
                }
                else
                {
                    // the database must be part of a known cluster, so add a cluster for it to be part of
                    var cluster = new ClusterSymbol(database.Name + ":cluster", database);
                    return WithCluster(cluster).WithDatabase(database);
                }
            }
        }

        /// <summary>
        /// True if the <see cref="TableSymbol"/> is part of one of the known databases.
        /// </summary>
        public bool IsDatabaseTable(TableSymbol table)
        {
            return GetDatabase(table) != null;
        }

        /// <summary>
        /// True if the <see cref="FunctionSymbol"/> is part of one of the known databases.
        /// </summary>
        public bool IsDatabaseFunction(FunctionSymbol function)
        {
            return GetDatabase(function) != null;
        }

        /// <summary>
        /// True if the <see cref="Symbol"/> is contained by one of the known databases.
        /// </summary>
        public bool IsDatabaseSymbol(Symbol symbol)
        {
            return GetDatabase(symbol) != null;
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified functions.
        /// </summary>
        public GlobalState WithFunctions(IReadOnlyList<FunctionSymbol> functions)
        {
            return With(functions: Optional(functions));
        }

        /// <summary>
        /// Gets the cluster given the simple name or host name.
        /// </summary>
        public ClusterSymbol GetCluster(string name)
        {
            name = KustoFacts.GetHostName(name) ?? name;

            if (this.Cluster != ClusterSymbol.Unknown
                && (KustoFacts.IsClusterHostName(name, this.Cluster.Name)
                    || (KustoFacts.IsClusterShortName(name, this.Cluster.Name) && KustoFacts.IsKustoWindowsNet(this.Cluster.Name))))
            {
                return this.Cluster;
            }

            return this.Clusters.FirstOrDefault(c => KustoFacts.IsClusterHostName(name, c.Name))
                ?? this.Clusters.FirstOrDefault(c => KustoFacts.IsClusterShortName(name, c.Name) && KustoFacts.IsKustoWindowsNet(c.Name))
                ?? this.Clusters.FirstOrDefault(c => KustoFacts.IsClusterShortName(name, c.Name));
        }

        /// <summary>
        /// Gets the <see cref="ClusterSymbol"/> that contains the <see cref="DatabaseSymbol"/>.
        /// </summary>
        public ClusterSymbol GetCluster(DatabaseSymbol database)
        {
            if (database == null)
                return null;

            if (this.reverseClusterMap == null)
            {
                var map = new Dictionary<Symbol, ClusterSymbol>();

                foreach (var cluster in this.Clusters)
                {
                    foreach (var member in cluster.Members)
                    {
                        map[member] = cluster;
                    }
                }

                Interlocked.CompareExchange(ref this.reverseClusterMap, map, null);
            }

            this.reverseClusterMap.TryGetValue(database, out var result);
            return result;
        }

        /// <summary>
        /// Gets the <see cref="DatabaseSymbol"/> that contains the <see cref="TableSymbol"/>.
        /// </summary>
        public DatabaseSymbol GetDatabase(TableSymbol table)
        {
            return GetDatabase((Symbol)table);
        }

        /// <summary>
        /// Gets the <see cref="DatabaseSymbol"/> that contains the <see cref="FunctionSymbol"/>.
        /// </summary>
        public DatabaseSymbol GetDatabase(FunctionSymbol function)
        {
            return GetDatabase((Symbol)function);
        }

        /// <summary>
        /// Gets the <see cref="DatabaseSymbol"/> that contains this <see cref="Symbol"/>
        /// </summary>
        public DatabaseSymbol GetDatabase(Symbol symbol)
        {
            if (symbol == null)
                return null;

            if (this.reverseDatabaseMap == null)
            {
                var map = new Dictionary<Symbol, DatabaseSymbol>();

                foreach (var database in this.Clusters.SelectMany(c => c.Databases))
                {
                    foreach (var member in database.Members)
                    {
                        map[member] = database;
                    }
                }

                Interlocked.CompareExchange(ref this.reverseDatabaseMap, map, null);
            }

            this.reverseDatabaseMap.TryGetValue(symbol, out var result);
            return result;
        }

        /// <summary>
        /// Gets the known database's <see cref="TableSymbol"/> that contains the <see cref="ColumnSymbol"/>.
        /// </summary>
        public TableSymbol GetTable(ColumnSymbol column)
        {
            if (column == null)
                return null;

            if (this.reverseTableMap == null)
            {
                var map = new Dictionary<Symbol, TableSymbol>();

                foreach(var table in this.Clusters.SelectMany(c => c.Databases).SelectMany(d => d.Tables))
                {
                    foreach (var col in table.Columns)
                    {
                        map[col] = table;
                    }
                }

                Interlocked.CompareExchange(ref this.reverseTableMap, map, null);
            }

            this.reverseTableMap.TryGetValue(column, out var result);
            return result;
        }

        /// <summary>
        /// Gets the function with the specified name, or null
        /// </summary>
        public FunctionSymbol GetFunction(string name)
        {
            if (this.functionsMap == null)
            {
                var map = this.Functions.ToDictionary(f => f.Name);
                Interlocked.CompareExchange(ref functionsMap, map, null);
            }

            this.functionsMap.TryGetValue(name, out var fn);
            return fn;
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified aggregates.
        /// </summary>
        public GlobalState WithAggregates(IReadOnlyList<FunctionSymbol> aggregates)
        {
            return With(aggregates: Optional(aggregates));
        }

        /// <summary>
        /// Gets the aggregate with the specified name, or null.
        /// </summary>
        public FunctionSymbol GetAggregate(string name)
        {
            if (this.aggregatesMap == null)
            {
                var map = this.Aggregates.ToDictionary(f => f.Name);
                Interlocked.CompareExchange(ref this.aggregatesMap, map, null);
            }

            this.aggregatesMap.TryGetValue(name, out var fn);
            return fn;
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified plug-ins.
        /// </summary>
        public GlobalState WithPlugIns(IReadOnlyList<FunctionSymbol> plugins)
        {
            return With(plugins: Optional(plugins));
        }

        /// <summary>
        /// Gets the plug-in with the specified name, or null.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FunctionSymbol GetPlugIn(string name)
        {
            if (this.pluginMap == null)
            {
                var map = this.PlugIns.ToDictionary(f => f.Name);
                Interlocked.CompareExchange(ref this.pluginMap, map, null);
            }

            this.pluginMap.TryGetValue(name, out var fn);
            return fn;
        }

        /// <summary>
        /// True if the function is a known aggregate.
        /// </summary>
        public bool IsAggregateFunction(FunctionSymbol fn)
        {
            return GetAggregate(fn.Name) == fn;
        }

        /// <summary>
        /// True if the function is a known built-in function.
        /// </summary>
        public bool IsBuiltInFunction(FunctionSymbol fn)
        {
            return GetFunction(fn.Name) == fn
                || GetAggregate(fn.Name) == fn
                || GetPlugIn(fn.Name) == fn;
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified operators.
        /// </summary>
        public GlobalState WithOperators(IReadOnlyList<OperatorSymbol> operators)
        {
            return With(operators: Optional(operators));
        }

        /// <summary>
        /// Gets the built-in operator symbol for the corresponding argument types.
        /// </summary>
        public OperatorSymbol GetOperator(OperatorKind kind)
        {
            if (this.operatorMap == null)
            {
                this.operatorMap = this.Operators.ToDictionary(o => o.OperatorKind);
            }

            if (this.operatorMap.TryGetValue(kind, out var op))
            {
                return op;
            }

            return null;
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified commands.
        /// </summary>
        public GlobalState WithCommands(IReadOnlyList<CommandSymbol> commands)
        {
            return With(commands: Optional(commands));
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the additional commands.
        /// </summary>
        public GlobalState AddCommands(IReadOnlyList<CommandSymbol> additionalCommands)
        {
            return With(commands: this.Commands.Concat(additionalCommands).ToList());
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the additional commands.
        /// </summary>
        public GlobalState AddCommands(params CommandSymbol[] additionalCommands)
        {
            return AddCommands((IReadOnlyList<CommandSymbol>) additionalCommands);
        }

        /// <summary>
        /// Gets a <see cref="CommandSymbol"/> given its name.
        /// </summary>
        public CommandSymbol GetCommand(string name)
        {
            if (this.commandMap == null)
            {
                var map = new Dictionary<string, CommandSymbol>(this.Commands.Count);
                foreach (var c in this.Commands)
                {
                    map[c.Name] = c;
                }
                Interlocked.CompareExchange(ref this.commandMap, map, null);
            }

            this.commandMap.TryGetValue(name, out var command);
            return command;
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified parameters.
        /// </summary>
        public GlobalState WithParameters(IReadOnlyList<ParameterSymbol> parameters)
        {
            return With(parameters: Optional(parameters));
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the additional parameters.
        /// </summary>
        public GlobalState AddParameters(IReadOnlyList<ParameterSymbol> parameters)
        {
            return WithParameters(this.Parameters.Concat(parameters).ToList());
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the additional parameters.
        /// </summary>
        public GlobalState AddParameters(params ParameterSymbol[] parameters)
        {
            return WithParameters((IReadOnlyList<ParameterSymbol>)parameters);
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified options.
        /// </summary>
        public GlobalState WithOptions(IReadOnlyList<OptionSymbol> options)
        {
            return With(options: Optional(options));
        }

        /// <summary>
        /// Gets the <see cref="OptionSymbol"/> with the specified name, or null if none match.
        /// </summary>
        public OptionSymbol GetOption(string name)
        {
            if (this.optionMap == null)
            {
                var map = new Dictionary<string, OptionSymbol>();
                foreach (var opt in this.Options)
                {
                    map[opt.Name] = opt;
                }

                Interlocked.CompareExchange(ref this.optionMap, map, null);
            }

            this.optionMap.TryGetValue(name, out var option);
            return option;
        }

        private static Optional<T> Optional<T>(T value) => new Optional<T>(value);

        private static GlobalState s_default;

        /// <summary>
        /// The default <see cref="GlobalState"/>
        /// </summary>
        public static GlobalState Default
        {
            get
            {
                // initialize lazy, because other symbols may reference this default instance
                if (s_default == null)
                {
                    var globals =
                        new GlobalState(
                            EmptyReadOnlyList<ClusterSymbol>.Instance,
                            ClusterSymbol.Unknown,
                            DatabaseSymbol.Unknown,
                            Language.Functions.All,
                            Language.Aggregates.All,
                            Language.PlugIns.All,
                            Language.Operators.All,
                            Language.EngineCommands.All,
                            EmptyReadOnlyList<ParameterSymbol>.Instance,
                            Language.Options.All,
                            cache: null,
                            reverseClusterMap: null,
                            reverseDatabaseMap: null,
                            reverseTableMap: null,
                            functionsMap: null,
                            aggregatesMap: null,
                            pluginMap: null,
                            operatorMap: null,
                            commandMap: null,
                            optionMap: null);
                    Interlocked.CompareExchange(ref s_default, globals, null);
                }

                return s_default;
            }
        }
    }
}