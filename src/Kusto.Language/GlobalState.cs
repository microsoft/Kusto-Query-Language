using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Kusto.Language.Editor;
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
        /// The default domain suffix
        /// </summary>
        public string Domain { get; }

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
        /// The kind of server that determines what set of control commands are available.
        /// </summary>
        public string ServerKind { get; }

        /// <summary>
        /// Symbols that are in scope but defined outside the query.
        /// </summary>
        public IReadOnlyList<Symbol> AmbientSymbols { get; }

        /// <summary>
        /// Symbols for client parameters that appear as braced names in a query
        /// and are textually substituted by the client before execution.
        /// Client parameters do not require declared symbols to function.
        /// These symbols provide information for better semantic analysis.
        /// </summary>
        public IReadOnlyList<Symbol> ClientSymbols { get; }

        /// <summary>
        /// Known query options
        /// </summary>
        public IReadOnlyList<OptionSymbol> Options { get; }

        /// <summary>
        /// All the properties and their values
        /// </summary>
        private IReadOnlyList<PropertyAndValue> Properties { get; }

        /// <summary>
        /// The <see cref="KustoCache"/> used to store additional accumulated global state.
        /// If caching is not enabled for this <see cref="GlobalState"/> this property will return null.
        /// </summary>
        public KustoCache Cache { get; }

        /// <summary>
        /// Options to determine parsing behavior.
        /// </summary>
        public ParseOptions ParseOptions { get; }

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
        /// Name to ambient <see cref="Symbol"/> lookup map.
        /// </summary>
        private Dictionary<string, Symbol> ambientSymbolsMap;

        /// <summary>
        /// Name to client <see cref="Symbol"/> lookup map.
        /// </summary>
        private Dictionary<string, Symbol> clientSymbolsMap;

        /// <summary>
        /// Name to <see cref="GlobalState"/> lookup map
        /// </summary>
        private Dictionary<GlobalStateProperty, object> propertyMap;

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
            string domain,
            IReadOnlyList<ClusterSymbol> clusters,
            ClusterSymbol cluster,
            DatabaseSymbol database,
            IReadOnlyList<FunctionSymbol> functions,
            IReadOnlyList<FunctionSymbol> aggregates,
            IReadOnlyList<FunctionSymbol> plugins,
            IReadOnlyList<OperatorSymbol> operators,
            string serverKind,
            IReadOnlyList<Symbol> ambientSymbols,
            IReadOnlyList<Symbol> clientSymbols,
            IReadOnlyList<OptionSymbol> options,
            IReadOnlyList<PropertyAndValue> properties,
            KustoCache cache,
            ParseOptions parseOptions,
            Dictionary<Symbol, ClusterSymbol> reverseClusterMap,
            Dictionary<Symbol, DatabaseSymbol> reverseDatabaseMap,
            Dictionary<Symbol, TableSymbol> reverseTableMap,
            Dictionary<string, FunctionSymbol> functionsMap,
            Dictionary<string, FunctionSymbol> aggregatesMap,
            Dictionary<string, FunctionSymbol> pluginMap,
            Dictionary<OperatorKind, OperatorSymbol> operatorMap,
            Dictionary<string, CommandSymbol> commandMap,
            Dictionary<string, Symbol> ambientSymbolsMap,
            Dictionary<string, Symbol> clientSymbolsMap,
            Dictionary<string, OptionSymbol> optionMap,
            Dictionary<GlobalStateProperty, object> propertyMap)
        {
            this.Domain = domain ?? KustoFacts.KustoWindowsNet;
            this.Clusters = clusters ?? EmptyReadOnlyList<ClusterSymbol>.Instance;
            this.Cluster = cluster ?? ClusterSymbol.Unknown;
            this.Database = database ?? DatabaseSymbol.Unknown;
            this.Functions = functions ?? EmptyReadOnlyList<FunctionSymbol>.Instance;
            this.Aggregates = aggregates ?? EmptyReadOnlyList<FunctionSymbol>.Instance;
            this.PlugIns = plugins ?? EmptyReadOnlyList<FunctionSymbol>.Instance;
            this.Operators = operators ?? EmptyReadOnlyList<OperatorSymbol>.Instance;
            this.ServerKind = serverKind ?? ServerKinds.Engine;
            this.AmbientSymbols = ambientSymbols ?? EmptyReadOnlyList<Symbol>.Instance;
            this.ClientSymbols = clientSymbols ?? EmptyReadOnlyList<Symbol>.Instance;
            this.Options = options ?? EmptyReadOnlyList<OptionSymbol>.Instance;
            this.Properties = properties ?? EmptyReadOnlyList<PropertyAndValue>.Instance;
            this.Cache = cache != null ? cache.WithGlobals(this) : null;
            this.ParseOptions = parseOptions ?? ParseOptions.Default;
            this.reverseClusterMap = reverseClusterMap;
            this.reverseDatabaseMap = reverseDatabaseMap;
            this.reverseTableMap = reverseTableMap;
            this.functionsMap = functionsMap;
            this.aggregatesMap = aggregatesMap;
            this.pluginMap = pluginMap;
            this.operatorMap = operatorMap;
            this.commandMap = commandMap;
            this.ambientSymbolsMap = ambientSymbolsMap;
            this.clientSymbolsMap = clientSymbolsMap;
            this.optionMap = optionMap;
            this.propertyMap = propertyMap;
        }

        /// <summary>
        /// Makes a new instance of this <see cref="GlobalState"/> that contains the same content,
        /// but possibly clears the cache.
        /// </summary>
        public GlobalState Copy()
        {
            return new GlobalState(
                this.Domain,
                this.Clusters,
                this.Cluster,
                this.Database,
                this.Functions,
                this.Aggregates,
                this.PlugIns,
                this.Operators,
                this.ServerKind,
                this.AmbientSymbols,
                this.ClientSymbols,
                this.Options,
                this.Properties,
                this.Cache,
                this.ParseOptions,
                this.reverseClusterMap,
                this.reverseDatabaseMap,
                this.reverseTableMap,
                this.functionsMap,
                this.aggregatesMap,
                this.pluginMap,
                this.operatorMap,
                this.commandMap,
                this.ambientSymbolsMap,
                this.clientSymbolsMap,
                this.optionMap,
                this.propertyMap);
        }

        /// <summary>
        /// Conditionally creates a new instance of a <see cref="GlobalState"/> if one of the 
        /// optional arguments is different than the current corresponding value.
        /// </summary>
        private GlobalState With(
            Optional<string> domain = default(Optional<string>),
            Optional<IReadOnlyList<ClusterSymbol>> clusters = default(Optional<IReadOnlyList<ClusterSymbol>>),
            Optional<ClusterSymbol> cluster = default(Optional<ClusterSymbol>),
            Optional<DatabaseSymbol> database = default(Optional<DatabaseSymbol>),
            Optional<IReadOnlyList<FunctionSymbol>> functions = default(Optional<IReadOnlyList<FunctionSymbol>>),
            Optional<IReadOnlyList<FunctionSymbol>> aggregates = default(Optional<IReadOnlyList<FunctionSymbol>>),
            Optional<IReadOnlyList<FunctionSymbol>> plugins = default(Optional<IReadOnlyList<FunctionSymbol>>),
            Optional<IReadOnlyList<OperatorSymbol>> operators = default(Optional<IReadOnlyList<OperatorSymbol>>),
            Optional<string> serverKind = default(Optional<string>),
            Optional<IReadOnlyList<Symbol>> ambientSymbols = default(Optional<IReadOnlyList<Symbol>>),
            Optional<IReadOnlyList<Symbol>> clientSymbols = default(Optional<IReadOnlyList<Symbol>>),
            Optional<IReadOnlyList<OptionSymbol>> options = default(Optional<IReadOnlyList<OptionSymbol>>),
            Optional<IReadOnlyList<PropertyAndValue>> properties = default(Optional<IReadOnlyList<PropertyAndValue>>),
            Optional<KustoCache> cache = default(Optional<KustoCache>),
            Optional<ParseOptions> parseOptions = default(Optional<ParseOptions>))
        {
            var useDomain = domain.HasValue ? domain.Value : this.Domain;
            var useClusters = clusters.HasValue ? clusters.Value : this.Clusters;
            var useCluster = cluster.HasValue ? cluster.Value : this.Cluster;
            var useDatabase = database.HasValue ? database.Value : this.Database;
            var useFunctions = functions.HasValue ? functions.Value : this.Functions;
            var useAggregates = aggregates.HasValue ? aggregates.Value : this.Aggregates;
            var usePlugins = plugins.HasValue ? plugins.Value : this.PlugIns;
            var useOperators = operators.HasValue ? operators.Value : this.Operators;
            var useServerKind = serverKind.HasValue ? serverKind.Value : this.ServerKind;
            var useAmbientSymbols = ambientSymbols.HasValue ? ambientSymbols.Value : this.AmbientSymbols;
            var useClientSymbols = clientSymbols.HasValue ? clientSymbols.Value : this.ClientSymbols;
            var useOptions = options.HasValue ? options.Value : this.Options;
            var useProperties = properties.HasValue ? properties.Value : this.Properties;
            var useCache = cache.HasValue ? cache.Value : this.Cache;
            var useParseOptions = parseOptions.HasValue ? parseOptions.Value : this.ParseOptions;

            if (useDomain != this.Domain
                || useClusters != this.Clusters
                || useCluster != this.Cluster
                || useDatabase != this.Database
                || useFunctions != this.Functions
                || useAggregates != this.Aggregates
                || usePlugins != this.PlugIns
                || useOperators != this.Operators
                || useServerKind != this.ServerKind
                || useAmbientSymbols != this.AmbientSymbols
                || useClientSymbols != this.ClientSymbols
                || useOptions != this.Options
                || useProperties != this.Properties
                || useCache != this.Cache
                || useParseOptions != this.ParseOptions)
            {
                return new GlobalState(
                    useDomain,
                    useClusters,
                    useCluster,
                    useDatabase,
                    useFunctions,
                    useAggregates,
                    usePlugins,
                    useOperators,
                    useServerKind,
                    useAmbientSymbols,
                    useClientSymbols,
                    useOptions,
                    useProperties,
                    useCache,
                    useParseOptions,
                    useClusters == this.Clusters ? this.reverseClusterMap : null,
                    useClusters == this.Clusters ? this.reverseDatabaseMap : null,
                    useClusters == this.Clusters ? this.reverseTableMap : null,
                    useFunctions == this.Functions ? this.functionsMap : null,
                    useAggregates == this.Aggregates ? this.aggregatesMap : null,
                    usePlugins == this.PlugIns ? this.pluginMap : null,
                    useOperators == this.Operators ? this.operatorMap : null,
                    useServerKind == this.ServerKind ? this.commandMap : null,
                    useAmbientSymbols == this.AmbientSymbols ? this.ambientSymbolsMap : null,
                    useClientSymbols == this.ClientSymbols ? this.clientSymbolsMap : null,
                    useOptions == this.Options ? this.optionMap : null,
                    useProperties == this.Properties ? this.propertyMap : null);
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
        /// Constructs a new <see cref="GlobalState"/> with the specified <see cref="ParseOptions"/>.
        /// </summary>
        public GlobalState WithParseOptions(ParseOptions parseOptions)
        {
            if (this.ParseOptions == parseOptions)
            {
                return this;
            }
            else
            {
                return With(parseOptions: parseOptions);
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
            else if (clusters == null)
            {
                return With(clusters: Optional(clusters)).WithCluster(ClusterSymbol.Unknown);
            }
            else
            {
                // change the set of clusters and update current cluster in case its symbol was updated
                var newCluster = clusters.FirstOrDefault(c => c.Name == this.Cluster.Name) ?? ClusterSymbol.Unknown;
                return With(clusters: Optional(clusters)).WithCluster(newCluster);
            }
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified cluster list.
        /// </summary>
        public GlobalState WithClusterList(params ClusterSymbol[] clusters)
        {
            return WithClusterList((IReadOnlyList<ClusterSymbol>)clusters);
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
            else if (cluster == ClusterSymbol.Unknown || cluster == null)
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

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified default cluster.
        /// </summary>
        public GlobalState WithCluster(string clusterName)
        {
            return WithCluster(GetCluster(clusterName) ?? ClusterSymbol.Unknown);
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified default domain suffix.
        /// </summary>
        public GlobalState WithDomain(string domain)
        {
            if (!string.IsNullOrEmpty(domain)
                && domain[0] != '.')
            {
                domain = "." + domain;
            }

            return With(domain: domain);
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
        /// Constructs a new <see cref="GlobalState"/> with the specified default database.
        /// </summary>
        public GlobalState WithDatabase(string databaseName)
        {
            return WithDatabase(this.Cluster.GetDatabase(databaseName) ?? DatabaseSymbol.Unknown);
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
        /// Gets the cluster given the short name or host name.
        /// </summary>
        public ClusterSymbol GetCluster(string name)
        {
            if (name == null)
                return null;

            name = KustoFacts.GetHostName(name) ?? name;

            if (this.Cluster != ClusterSymbol.Unknown
                && (KustoFacts.IsHostName(name, this.Cluster.Name)
                    || (KustoFacts.IsShortHostName(name, this.Cluster.Name, this.Domain))))
            {
                return this.Cluster;
            }

            return this.Clusters.FirstOrDefault(c => KustoFacts.IsHostName(name, c.Name))
                ?? this.Clusters.FirstOrDefault(c => KustoFacts.IsShortHostName(name, c.Name, this.Domain));
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
        /// Gets the <see cref="DatabaseSymbol"/> that contains the <see cref="EntityGroupSymbol"/>.
        /// </summary>
        public DatabaseSymbol GetDatabase(EntityGroupSymbol entityGroup)
        {
            return GetDatabase((Symbol)entityGroup);
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
            if (name == null)
                return null;

            if (this.Functions.Count == 0)
                return null;

            if (this.functionsMap == null)
            {
                var map = this.Functions.ToDictionaryLast(f => f.Name);
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
            if (name == null)
                return null;

            if (this.Aggregates.Count == 0)
                return null;

            if (this.aggregatesMap == null)
            {
                var map = this.Aggregates.ToDictionaryLast(f => f.Name);
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
            if (name == null || this.PlugIns.Count == 0)
                return null;

            if (this.pluginMap == null)
            {
                var map = this.PlugIns.ToDictionaryLast(f => f.Name);
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
            return fn != null 
                && GetAggregate(fn.Name) == fn;
        }

        /// <summary>
        /// True if the function is a known built-in function.
        /// </summary>
        public bool IsBuiltInFunction(FunctionSymbol fn)
        {
            if (fn == null)
                return false;

            return GetFunction(fn.Name) == fn
                || GetAggregate(fn.Name) == fn
                || GetPlugIn(fn.Name) == fn;
        }

        public bool IsBuiltInFunctionName(string functionName)
        {
            if (string.IsNullOrEmpty(functionName))
                return false;

            return GetFunction(functionName)?.Name == functionName
                || GetAggregate(functionName)?.Name == functionName
                || GetPlugIn(functionName)?.Name == functionName;
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
            if (this.Operators.Count == 0)
                return null;

            if (this.operatorMap == null)
            {
                this.operatorMap = this.Operators.ToDictionaryLast(o => o.OperatorKind);
            }

            this.operatorMap.TryGetValue(kind, out var op);
            return op;
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified server kind <see cref="ServerKinds"/>.
        /// </summary>
        public GlobalState WithServerKind(string serverKind)
        {
            return With(serverKind: serverKind);
        }

        /// <summary>
        /// Gets a <see cref="CommandSymbol"/> given its name.
        /// </summary>
        public CommandSymbol GetCommand(string name)
        {
            if (name == null)
                return null;

            if (this.commandMap == null)
            {
                var commands = GetCommands(this.ServerKind);

                var map = new Dictionary<string, CommandSymbol>(commands.Count);
                foreach (var c in commands)
                {
                    map[c.Name] = c;
                }

                Interlocked.CompareExchange(ref this.commandMap, map, null);
            }

            this.commandMap.TryGetValue(name, out var command);
            return command;
        }

        private static IReadOnlyList<CommandSymbol> GetCommands(string serverKind)
        {
            switch (serverKind)
            {
                case ServerKinds.Engine:
                    return EngineCommands.All;
                case ServerKinds.DataManager:
                    return DataManagerCommands.All;
                case ServerKinds.ClusterManager:
                    return ClusterManagerCommands.All;
                case ServerKinds.AriaBridge:
                    return AriaBridgeCommands.All;
                default:
                    return EmptyReadOnlyList<CommandSymbol>.Instance;
            }
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified ambient symbols.
        /// </summary>
        public GlobalState WithAmbientSymbols(IReadOnlyList<Symbol> symbols)
        {
            return With(ambientSymbols: Optional(symbols));
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the additional ambient symbols.
        /// </summary>
        public GlobalState AddOrUpdateAmbientSymbols(IReadOnlyList<Symbol> symbols)
        {
            return WithAmbientSymbols(this.AmbientSymbols.AddOrUpdate(symbols, s => s.Name));
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the additional ambient symbols.
        /// </summary>
        public GlobalState AddOrUpdateAmbientSymbols(params Symbol[] symbols)
        {
            return AddOrUpdateAmbientSymbols((IReadOnlyList<Symbol>)symbols);
        }

        /// <summary>
        /// Gets the ambient <see cref="Symbol"/> given its name.
        /// </summary>
        public Symbol GetAmbientSymbol(string name)
        {
            if (name == null || this.AmbientSymbols.Count == 0)
                return null;

            if (this.ambientSymbolsMap == null)
            {
                var map = this.AmbientSymbols.ToDictionaryLast(p => p.Name);
                Interlocked.CompareExchange(ref this.ambientSymbolsMap, map, null);
            }

            this.ambientSymbolsMap.TryGetValue(name, out var symbol);
            return symbol;
        }

        /// <summary>
        /// Ambient parameters
        /// </summary>
        [Obsolete("Use AmbientSymbols")]
        public IReadOnlyList<ParameterSymbol> Parameters
        {
            get
            {
                if (this.ambientParameters == null)
                {
                    var parameters = this.AmbientSymbols.OfType<ParameterSymbol>().ToReadOnly();
                    Interlocked.CompareExchange(ref this.ambientParameters, parameters, null);
                }

                return this.ambientParameters;
            }
        }

        private IReadOnlyList<ParameterSymbol> ambientParameters;

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified ambient parameters.
        /// </summary>
        [Obsolete("Use WithAmbientSymbols")]
        public GlobalState WithParameters(IReadOnlyList<ParameterSymbol> parameters)
        {
            var ambientWithoutParameters = this.AmbientSymbols.Where(s => !(s is ParameterSymbol));
            var newList = ambientWithoutParameters.Concat(parameters).Where(s => s != null).ToReadOnly();
            return WithAmbientSymbols(newList);
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the additional ambient parameters.
        /// </summary>
        [Obsolete("Use AddOrUpdateAmbientSymbols")]
        public GlobalState AddParameters(IReadOnlyList<ParameterSymbol> parameters)
        {
            return AddOrUpdateAmbientSymbols(parameters);
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the additional ambient parameters.
        /// </summary>
        [Obsolete("Use AddOrUpdateAmbientSymbols")]
        public GlobalState AddParameters(params ParameterSymbol[] parameters)
        {
            return AddOrUpdateAmbientSymbols(parameters);
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified client parameter symbols.
        /// </summary>
        public GlobalState WithClientSymbols(IReadOnlyList<Symbol> symbols)
        {
            return With(clientSymbols: Optional(symbols));
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified client symbols added or updated.
        /// </summary>
        public GlobalState AddOrUpdateClientSymbols(IReadOnlyList<Symbol> symbols)
        {
            return WithClientSymbols(this.ClientSymbols.AddOrUpdate(symbols, s => s.Name));
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> with the specified client symbols added or updated.
        /// </summary>
        public GlobalState AddOrUpdateClientSymbols(params Symbol[] symbols)
        {
            return AddOrUpdateClientSymbols((IReadOnlyList<Symbol>)symbols);
        }

        /// <summary>
        /// Gets the client parameter <see cref="Symbol"/> given its name.
        /// </summary>
        public Symbol GetClientSymbol(string name)
        {
            if (name == null || this.ClientSymbols.Count == 0)
                return null;

            if (this.clientSymbolsMap == null)
            {
                var map = this.ClientSymbols.ToDictionaryLast(p => p.Name);
                Interlocked.CompareExchange(ref this.clientSymbolsMap, map, null);
            }

            this.clientSymbolsMap.TryGetValue(name, out var symbol);
            return symbol;
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
            if (name == null)
                return null;

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

        private class PropertyAndValue
        {
            public GlobalStateProperty Property { get; }
            public object Value { get; }

            public PropertyAndValue(GlobalStateProperty property, object value)
            {
                this.Property = property;
                this.Value = value;
            }
        }

        /// <summary>
        /// Gets the value for the specified property
        /// </summary>
        public T GetProperty<T>(GlobalStateProperty<T> property)
        {
            if (property == null)
                return default(T);            

            if (this.Properties.Count == 0)
            {
                return property.DefaultValue;
            }

            if (this.propertyMap == null)
            {
                var map = this.Properties.ToDictionaryLast(pv => pv.Property, pv => pv.Value);
                Interlocked.CompareExchange(ref this.propertyMap, map, null);
            }

            if (this.propertyMap.TryGetValue(property, out var value))
            {
                return (T)value;
            }
            else
            {
                return property.DefaultValue;
            }
        }

        /// <summary>
        /// Constructs a new <see cref="GlobalState"/> instance with the property added or replaced.
        /// </summary>
        public GlobalState WithProperty<T>(GlobalStateProperty<T> property, T value)
        {
            if (property == null)
                return this;

            List<PropertyAndValue> list = null;

            bool hasCurrentValue = false;
            object currentValue = default;

            // look for existing property (w/o forcing map to be populated)
            if (this.propertyMap != null)
            {
                hasCurrentValue = this.propertyMap.TryGetValue(property, out currentValue);
            }
            else
            {
                var currentPropAndValue = this.Properties.FirstOrDefault(p => p.Property == property);
                if (currentPropAndValue != null)
                {
                    hasCurrentValue = true;
                    currentValue = currentPropAndValue.Value;
                }
            }

            // if it already exists, replace it
            if (hasCurrentValue)
            {
                if (object.Equals(currentValue, value))
                {
                    // the same value already exists
                    return this;
                }

                list = this.Properties.ToList();
                var index = list.FindIndex(p => p.Property == property);
                if (index >= 0)
                {
                    list[index] = new PropertyAndValue(property, value);
                    return With(properties: list.AsReadOnly());
                }
            }

            // otherwise add it to the end
            if (list == null)
            {
                list = this.Properties.ToList();
            }

            list.Add(new PropertyAndValue(property, value));
            return With(properties: list.AsReadOnly());
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
                            KustoFacts.KustoWindowsNet,
                            EmptyReadOnlyList<ClusterSymbol>.Instance,
                            ClusterSymbol.Unknown,
                            DatabaseSymbol.Unknown,
                            Language.Functions.All,
                            Language.Aggregates.All,
                            Language.PlugIns.All,
                            Language.Operators.All,
                            ServerKinds.Engine,
                            EmptyReadOnlyList<Symbol>.Instance, // ambient parameters
                            EmptyReadOnlyList<Symbol>.Instance, // client parameters
                            Language.Options.All,
                            EmptyReadOnlyList<PropertyAndValue>.Instance,
                            cache: null,
                            parseOptions: null,
                            reverseClusterMap: null,
                            reverseDatabaseMap: null,
                            reverseTableMap: null,
                            functionsMap: null,
                            aggregatesMap: null,
                            pluginMap: null,
                            operatorMap: null,
                            commandMap: null,
                            ambientSymbolsMap: null,
                            clientSymbolsMap: null,
                            optionMap: null,
                            propertyMap: null);
                    Interlocked.CompareExchange(ref s_default, globals, null);
                }

                return s_default;
            }
        }
    }

    public abstract class GlobalStateProperty
    {
        public string Name { get; }

        protected GlobalStateProperty(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }

    public class GlobalStateProperty<T> : GlobalStateProperty
    {
        public T DefaultValue { get; }

        public GlobalStateProperty(string name, T defaultValue = default)
            : base(name)
        {
            this.DefaultValue = defaultValue;
        }
    }
}