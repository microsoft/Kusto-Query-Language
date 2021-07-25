using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Binding
{
    using Parsing;
    using Symbols;
    using Syntax;
    using Utils;

    /// <summary>
    /// The kinds of conversions allowed between values of two different types.
    /// </summary>
    internal enum Conversion
    {
        /// <summary>
        /// No conversion allowed between different scalar types (strict)
        /// </summary>
        None,

        /// <summary>
        /// Type promotion (widening) allowed.
        /// </summary>
        Promotable,

        /// <summary>
        /// Conversions between compatible types allowed (widening or narrowing)
        /// </summary>
        Compatible,

        /// <summary>
        /// All conversions allowed (no checking)
        /// </summary>
        Any
    }

    /// <summary>
    /// The kind of match that an argument can have with its corresponding signature parameter.
    /// </summary>
    internal enum ParameterMatchKind
    {
        // These are in order of which is better.. a better match is one that is more specific.

        /// <summary>
        /// There is no match between the argument and the parameter.
        /// </summary>
        None,

        /// <summary>
        /// The argument had an unknown type.
        /// </summary>
        Unknown,

        /// <summary>
        /// The argument's type is not the excluded type
        /// </summary>
        NotType,

        /// <summary>
        /// The argument's type is a scalar type
        /// </summary>
        Scalar,

        /// <summary>
        /// The argument's type is a summable scalar type
        /// </summary>
        Summable,

        /// <summary>
        /// The argument's type is an orderable scalar type
        /// </summary>
        Orderable,

        /// <summary>
        /// The argumet's type is a number
        /// </summary>
        Number,

        /// <summary>
        /// The argument type is compatible with the parameter type
        /// </summary>
        Compatible,

        /// <summary>
        /// The arguments type can be promoted to the parameter type
        /// </summary>
        Promoted,  // smaller set than all numbers

        /// <summary>
        /// The argument's type is tabular.
        /// </summary>
        Tabular,

        /// <summary>
        /// The argument's type is a table.
        /// </summary>
        Table,

        /// <summary>
        /// The argument's type is a database
        /// </summary>
        Database,

        /// <summary>
        /// The argument's type is a cluster
        /// </summary>
        Cluster,

        /// <summary>
        /// The argument's type is one of two possible parameter types
        /// </summary>
        OneOfTwo,  // one of two explicit types?

        /// <summary>
        /// The argument's type is an exact match for the parameter type
        /// </summary>
        Exact
    }

    /// <summary>
    /// A <see cref="SyntaxTree"/> that represents the expansion of a function call.
    /// </summary>
    internal class Expansion : SyntaxTree
    {
        public FunctionBody Body => (FunctionBody)this.Root;

        public Expansion(FunctionBody body)
            : base(body)
        {
        }
    }

    /// <summary>
    /// Binding state that persists across multiple bindings (lifetime of <see cref="KustoCache"/>)
    /// </summary>
    internal class GlobalBindingCache
    {
        internal readonly Dictionary<IReadOnlyList<TableSymbol>, TableSymbol> UnifiedNameColumnsMap =
            new Dictionary<IReadOnlyList<TableSymbol>, TableSymbol>(ReadOnlyListComparer<TableSymbol>.Default);

        internal readonly Dictionary<IReadOnlyList<TableSymbol>, TableSymbol> UnifiedNameAndTypeColumnsMap =
            new Dictionary<IReadOnlyList<TableSymbol>, TableSymbol>(ReadOnlyListComparer<TableSymbol>.Default);

        internal readonly Dictionary<IReadOnlyList<TableSymbol>, TableSymbol> CommonColumnsMap =
            new Dictionary<IReadOnlyList<TableSymbol>, TableSymbol>(ReadOnlyListComparer<TableSymbol>.Default);

        internal Dictionary<CallSiteInfo, Expansion> CallSiteToExpansionMap =
            new Dictionary<CallSiteInfo, Expansion>(CallSiteInfo.Comparer.Instance);
    }

    /// <summary>
    /// Binding state that exists for the duration of the binder.
    /// </summary>
    internal class LocalBindingCache
    {
        internal readonly HashSet<Signature> SignaturesComputingExpansion
            = new HashSet<Signature>();

        internal Dictionary<CallSiteInfo, Expansion> CallSiteToExpansionMap =
            new Dictionary<CallSiteInfo, Expansion>(CallSiteInfo.Comparer.Instance);
    }

    internal class CallSiteInfo
    {
        public Signature Signature { get; }

        public IReadOnlyList<VariableSymbol> Locals { get; }

        public CallSiteInfo(Signature signature, IReadOnlyList<VariableSymbol> locals)
        {
            this.Signature = signature;
            this.Locals = locals;
        }

        public override string ToString()
        {
            return Signature.Symbol.Name + "("
                + string.Join(",", this.Locals.Select(v => v.IsConstant && v.ConstantValue != null ? $"{v.Name}={v.ConstantValue}" : v.Name))
                + ")";
        }

        internal class Comparer : IEqualityComparer<CallSiteInfo>
        {
            public static readonly Comparer Instance = new Comparer();

            public bool Equals(CallSiteInfo x, CallSiteInfo y)
            {
                if (x.Signature != y.Signature)
                    return false;

                if (x.Locals.Count != y.Locals.Count)
                    return false;

                for (int i = 0; i < x.Locals.Count; i++)
                {
                    var lx = x.Locals[i];
                    var ly = y.Locals[i];

                    if (lx.Name != ly.Name
                        || lx.Type != ly.Type
                        || lx.IsConstant != ly.IsConstant
                        || !object.Equals(lx.ConstantValue, ly.ConstantValue))
                        return false;
                }

                return true;
            }

            public int GetHashCode(CallSiteInfo obj)
            {
                return obj.Signature.GetHashCode();
            }
        }
    }

    /// <summary>
    /// The binder performs general semantic analysis of the syntax tree.
    /// </summary>
    internal sealed partial class Binder
    {
        /// <summary>
        /// Global state including symbols declared in ambient database.
        /// </summary>
        private readonly GlobalState _globals;

        /// <summary>
        /// The cluster assumed when resolveing unqualified calls to database() 
        /// </summary>
        private ClusterSymbol _currentCluster;

        /// <summary>
        /// The database assumed when resolving unqualified references table/function names or calls to table()
        /// </summary>
        private DatabaseSymbol _currentDatabase;

        /// <summary>
        /// The function being declared.
        /// </summary>
        private FunctionSymbol _currentFunction;

        /// <summary>
        /// All symbol declared locally within the query appear in the local scope.
        /// These are symbols declared by let statements or the as query operator.
        /// Local scopes may be nested within other local scopes.
        /// </summary>
        private LocalScope _localScope;

        /// <summary>
        /// Columns accessible in piped query operators
        /// </summary>
        private TableSymbol _rowScope;

        /// <summary>
        /// Columns accessible from right side of join operator
        /// </summary>
        private TableSymbol _rightRowScope;

        /// <summary>
        /// Members accessible from left side of path/element expression
        /// </summary>
        private Symbol _pathScope;

        /// <summary>
        /// Implicit argument type used for invoke binding.
        /// </summary>
        private TypeSymbol _implicitArgumentType;

        /// <summary>
        /// The kind of scope in effect.
        /// </summary>
        private ScopeKind _scopeKind;

        /// <summary>
        /// Any aliased databases.
        /// </summary>
        private readonly Dictionary<string, DatabaseSymbol> _aliasedDatabases =
            new Dictionary<string, DatabaseSymbol>();

        /// <summary>
        /// Binding state that is shared across many binders/bindings
        /// </summary>
        private readonly GlobalBindingCache _globalBindingCache;

        /// <summary>
        /// Binding state that is private to one binding (including nested bindings)
        /// </summary>
        private readonly LocalBindingCache _localBindingCache;

        /// <summary>
        /// An optional function that assigns <see cref="SemanticInfo"/> to a <see cref="SyntaxNode"/>
        /// </summary>
        private readonly Action<SyntaxNode, SemanticInfo> _semanticInfoSetter;

        /// <summary>
        /// An optional <see cref="CancellationToken"/> specified for use during binding.
        /// </summary>
        private readonly CancellationToken _cancellationToken;

        private Binder(
            GlobalState globals,
            ClusterSymbol currentCluster,
            DatabaseSymbol currentDatabase,
            FunctionSymbol currentFunction,
            LocalScope outerScope,
            GlobalBindingCache globalBindingCache,
            LocalBindingCache localBindingCache,
            Action<SyntaxNode, SemanticInfo> semanticInfoSetter,
            CancellationToken cancellationToken)
        {
            _globals = globals;
            _currentCluster = currentCluster ?? globals.Cluster;
            _currentDatabase = currentDatabase ?? globals.Database;
            _currentFunction = currentFunction;
            _globalBindingCache = globalBindingCache ?? new GlobalBindingCache();
            _localBindingCache = localBindingCache ?? new LocalBindingCache();
            _localScope = new LocalScope(outerScope);
            _semanticInfoSetter = semanticInfoSetter ?? DefaultSetSemanticInfo;
            _cancellationToken = cancellationToken;
        }

        public TableSymbol RowScopeOrEmpty => _rowScope ?? TableSymbol.Empty;

        public TableSymbol RightRowScopeOrEmpty => _rightRowScope ?? TableSymbol.Empty;

        /// <summary>
        /// Do semantic analysis over the syntax tree.
        /// </summary>
        public static bool TryBind(
            SyntaxTree tree,
            GlobalState globals,
            LocalBindingCache localBindingCache = null,
            Action<SyntaxNode, SemanticInfo> semanticInfoSetter = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!tree.IsSafeToRecurse)
                return false;

            globals = globals.WithCache();
            var bindingCache = globals.Cache.GetOrCreate<GlobalBindingCache>();
            lock (bindingCache)
            {
                var binder = new Binder(
                    globals,
                    globals.Cluster,
                    globals.Database,
                    null, // currentFunction
                    GetDefaultOuterScope(globals),
                    bindingCache,
                    localBindingCache,
                    semanticInfoSetter: semanticInfoSetter,
                    cancellationToken: cancellationToken);

                var treeBinder = new TreeBinder(binder);

                tree.Root.Accept(treeBinder);
                return true;
            }
        }

        private static LocalScope GetDefaultOuterScope(GlobalState globals)
        {
            LocalScope outerScope = null;

            if (globals.Parameters.Count > 0)
            {
                outerScope = new LocalScope();
                outerScope.AddSymbols(globals.Parameters);
            }

            return outerScope;
        }

        internal static void DefaultSetSemanticInfo(SyntaxNode node, SemanticInfo info)
        {
            if (info != null)
            {
                var data = node.GetExtendedData(create: true);
                data.SemanticInfo = info;
            }
        }

        /// <summary>
        /// Do semantic analysis over an inline expansion of a function body.
        /// </summary>
        public static bool TryBindExpansion(
            SyntaxTree expansionTree,
            Binder outer,
            ClusterSymbol currentCluster,
            DatabaseSymbol currentDatabase,
            FunctionSymbol currentFunction,
            LocalScope outerScope,
            IEnumerable<Symbol> locals)
        {
            if (!expansionTree.IsSafeToRecurse)
                return false;

            var binder = new Binder(
                outer._globals,
                currentCluster ?? outer._currentCluster,
                currentDatabase ?? outer._currentDatabase,
                currentFunction,
                outerScope,
                outer._globalBindingCache,
                outer._localBindingCache,
                outer._semanticInfoSetter,
                outer._cancellationToken);

            if (locals != null)
            {
                binder.SetLocals(locals);
            }

            var treeBinder = new TreeBinder(binder);
            expansionTree.Root.Accept(treeBinder);

            return true;
        }

        private void SetLocals(IEnumerable<Symbol> locals)
        {
            foreach (var local in locals)
            {
                _localScope.AddSymbol(local);
            }
        }

        /// <summary>
        /// Sets the context of the binder to the specified node and text position.
        /// </summary>
        private void SetContext(SyntaxNode contextNode, int position = -1)
        {
            // note: assumes this API is only called at most once after constructor.
            if (contextNode != null)
            {
                var builder = new ContextBuilder(this, position >= 0 ? position : contextNode.TextStart);
                contextNode.Accept(builder);
            }
        }       

        /// <summary>
        /// Gets the computed return type for functions specified with a body or declaration.
        /// </summary>
        public static TypeSymbol GetComputedReturnType(
            Signature signature,
            GlobalState globals,
            IReadOnlyList<TypeSymbol> argumentTypes = null)
        {
            globals = globals.WithCache();

            var currentDatabase = globals.GetDatabase(signature.Symbol);
            var currentCluster = globals.GetCluster(currentDatabase);

            var bindingCache = globals.Cache.GetOrCreate<GlobalBindingCache>();
            lock (bindingCache)
            {
                var binder = new Binder(
                    globals,
                    currentCluster,
                    currentDatabase,
                    signature.Symbol as FunctionSymbol, // currentFunction
                    GetDefaultOuterScope(globals),
                    bindingCache,
                    localBindingCache: null,
                    semanticInfoSetter: null, 
                    cancellationToken: default(CancellationToken));

                return binder.GetComputedSignatureResult(signature, null, argumentTypes).Type;
            }
        }

        /// <summary>
        /// Scope kind.
        /// </summary>
        private enum ScopeKind
        {
            /// <summary>
            /// Normal lookup in <see cref="Binder"/>
            /// </summary>
            Normal,

            /// <summary>
            /// Only aggregate functions are visible
            /// </summary>
            Aggregate,

            /// <summary>
            /// Only plug-in funtions are visible
            /// </summary>
            PlugIn,

            /// <summary>
            /// Only query options are visible
            /// </summary>
            Option
        }

        /// <summary>
        /// Gets the <see cref="ScopeKind"/> in effect for a function's arguments.
        /// </summary>
        private ScopeKind GetArgumentScope(FunctionCallExpression fc, ScopeKind outerScope)
        {
            if (GetReferencedSymbol(fc.Name) is FunctionSymbol fs
                && _globals.IsAggregateFunction(fs))
            {
                // aggregate function arguments are always normal
                return ScopeKind.Normal;
            }
            else if (outerScope == ScopeKind.Aggregate)
            {
                // if the function is not a known aggregate then keep aggregate scope as there may be
                // aggregates nested in the function arguments
                return ScopeKind.Aggregate;
            }
            else
            {
                return ScopeKind.Normal;
            }
        }

#region Semantic Info accessors
        private void SetSemanticInfo(SyntaxNode node, SemanticInfo info)
        {
            if (node != null)
            {
                _semanticInfoSetter?.Invoke(node, info);
            }
        }

        private TypeSymbol GetResultTypeOrError(Expression expression) =>
            expression?.ResultType ?? ErrorSymbol.Instance;

        private TypeSymbol GetResultType(Expression expression) =>
            expression?.ResultType;

        private Symbol GetReferencedSymbol(Expression expression) =>
            expression?.ReferencedSymbol;

        private bool GetIsConstant(Expression expression) =>
            expression?.IsConstant ?? false;
#endregion

#region Symbol access/caching
        private Dictionary<string, ClusterSymbol> _openClusters;

        private ClusterSymbol GetOpenCluster(string name)
        {
            if (_openClusters == null)
            {
                _openClusters = new Dictionary<string, ClusterSymbol>();
            }

            if (!_openClusters.TryGetValue(name, out var cluster))
            {
                cluster = new ClusterSymbol(name, null, isOpen: true);
                _openClusters.Add(name, cluster);
            }

            return cluster;
        }

        /// <summary>
        /// Gets the named database.
        /// </summary>
        private DatabaseSymbol GetDatabase(string name, ClusterSymbol cluster = null)
        {
            cluster = cluster ?? _currentCluster;

            if (cluster == _currentCluster && string.Compare(_currentDatabase.Name, name, ignoreCase: true) == 0)
            {
                return _currentDatabase;
            }

            if (_aliasedDatabases.TryGetValue(name, out var db))
            {
                return db;
            }

            var list = s_symbolListPool.AllocateFromPool();
            try
            {

                cluster.GetMembers(name, SymbolMatch.Database, list, ignoreCase: true);

                if (list.Count >= 1)
                {
                    return (DatabaseSymbol)list[0];
                }
                else if (cluster.IsOpen)
                {
                    return GetOpenDatabase(name, cluster);
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                s_symbolListPool.ReturnToPool(list);
            }
        }

        private Dictionary<ClusterSymbol, Dictionary<string, DatabaseSymbol>> _openDatabases;

        private DatabaseSymbol GetOpenDatabase(string name, ClusterSymbol cluster)
        {
            cluster = cluster ?? _currentCluster;

            if (_openDatabases == null)
            {
                _openDatabases = new Dictionary<ClusterSymbol, Dictionary<string, DatabaseSymbol>>();
            }

            if (!_openDatabases.TryGetValue(cluster, out var map))
            {
                map = new Dictionary<string, DatabaseSymbol>();
                _openDatabases.Add(cluster, map);
            }

            if (!map.TryGetValue(name, out var database))
            {
                database = new DatabaseSymbol(name, null, isOpen: true);
                map.Add(name, database);
            }

            return database;
        }

        /// <summary>
        /// Gets the named table.
        /// </summary>
        private TableSymbol GetTable(string name, DatabaseSymbol database = null)
        {
            database = database ?? _currentDatabase;

            var list = s_symbolListPool.AllocateFromPool();
            try
            {
                database.GetMembers(name, SymbolMatch.Table, list);
                if (list.Count >= 1)
                {
                    return (TableSymbol)list[0];
                }
                else if (database.IsOpen)
                {
                    return GetOpenTable(name, database);
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                s_symbolListPool.ReturnToPool(list);
            }
        }

        private Dictionary<DatabaseSymbol, Dictionary<string, TableSymbol>> _openTables;

        private TableSymbol GetOpenTable(string name, DatabaseSymbol database)
        {
            if (_openTables == null)
            {
                _openTables = new Dictionary<DatabaseSymbol, Dictionary<string, TableSymbol>>();
            }

            if (!_openTables.TryGetValue(database, out var map))
            {
                map = new Dictionary<string, TableSymbol>();
                _openTables.Add(database, map);
            }

            if (!map.TryGetValue(name, out var table))
            {
                table = new TableSymbol(name).WithIsOpen(true);
                map.Add(name, table);
            }

            return table;
        }

        private Dictionary<TableSymbol, Dictionary<string, ColumnSymbol>> openColumns;

        private ColumnSymbol GetOpenColumn(string name, TableSymbol table)
        {
            if (openColumns == null)
            {
                openColumns = new Dictionary<TableSymbol, Dictionary<string, ColumnSymbol>>();
            }

            if (!openColumns.TryGetValue(table, out var columnMap))
            {
                columnMap = new Dictionary<string, ColumnSymbol>();
                openColumns.Add(table, columnMap);
            }

            if (!columnMap.TryGetValue(name, out var column))
            {
                column = new ColumnSymbol(name, ScalarTypes.Unknown);
                columnMap.Add(name, column);
            }

            return column;
        }

        private void GetDeclaredAndInferredColumns(TableSymbol table, List<ColumnSymbol> columns)
        {
            columns.AddRange(table.Columns);

            if (table.IsOpen && openColumns != null && openColumns.TryGetValue(table, out var columnMap))
            {
                columns.AddRange(columnMap.Values);
            }
        }

        public IReadOnlyList<ColumnSymbol> GetDeclaredAndInferredColumns(TableSymbol table)
        {
            if (table.IsOpen && openColumns != null && openColumns.ContainsKey(table))
            {
                var list = new List<ColumnSymbol>();
                GetDeclaredAndInferredColumns(table, list);
                return list;
            }
            else
            {
                return table.Columns;
            }
        }

        public bool TryGetDeclaredOrInferredColumn(TableSymbol table, string name, out ColumnSymbol column)
        {
            if (table.GetFirstMember(name, SymbolMatch.Column) is ColumnSymbol c)
            {
                column = c;
                return true;
            }
            else if (table.IsOpen)
            {
                column = GetOpenColumn(name, table);
                return true;
            }
            else
            {
                column = null;
                return false;
            }
        }

        private Dictionary<TableSymbol, TupleSymbol> tupleMap;

        /// <summary>
        /// Gets a tuple with the same columns (declared and inferred) as the table.
        /// </summary>
        private TupleSymbol GetTuple(TableSymbol table)
        {
            if (tupleMap == null)
            {
                tupleMap = new Dictionary<TableSymbol, TupleSymbol>();
            }

            if (!tupleMap.TryGetValue(table, out var tuple))
            {
                tuple = new TupleSymbol(table.Columns, table);
                tupleMap.Add(table, tuple);
            }

            return tuple;
        }

        private bool CanCache(IReadOnlyList<TableSymbol> tables)
        {
            return tables == _currentDatabase.Tables || tables.All(t => _globals.IsDatabaseTable(t));
        }

        /// <summary>
        /// A table that contains all the columns in the specified list of tables, unified on name.
        /// </summary>
        private TableSymbol GetTableOfColumnsUnifiedByName(IReadOnlyList<TableSymbol> tables)
        {
            // consider making this cache thread safe
            if (!_globalBindingCache.UnifiedNameColumnsMap.TryGetValue(tables, out var unifiedColumnsTable))
            {
                var cache = CanCache(tables);

                tables = tables.ToReadOnly();
                var columns = new List<ColumnSymbol>();

                foreach (var table in tables)
                {
                    columns.AddRange(table.Columns);
                }

                Binder.UnifyColumnsWithSameName(columns);

                unifiedColumnsTable = new TableSymbol(columns).WithIsOpen(tables.Any(t => t.IsOpen));

                if (cache)
                {
                    _globalBindingCache.UnifiedNameColumnsMap[tables] = unifiedColumnsTable;
                }
            }

            return unifiedColumnsTable;
        }

        /// <summary>
        /// A table that contains all the columns in the specified list of tables, unified on name and type.
        /// </summary>
        private TableSymbol GetTableOfColumnsUnifiedByNameAndType(IReadOnlyList<TableSymbol> tables)
        {
            // consider making this cache thread safe
            if (!_globalBindingCache.UnifiedNameAndTypeColumnsMap.TryGetValue(tables, out var unifiedColumnsTable))
            {
                var cache = CanCache(tables);

                tables = tables.ToReadOnly();
                var columns = new List<ColumnSymbol>();

                foreach (var table in tables)
                {
                    columns.AddRange(table.Columns);
                }

                Binder.UnifyColumnsWithSameNameAndType(columns);

                unifiedColumnsTable = new TableSymbol(columns).WithIsOpen(tables.Any(t => t.IsOpen));

                if (cache)
                {
                    _globalBindingCache.UnifiedNameAndTypeColumnsMap[tables] = unifiedColumnsTable;
                }
            }

            return unifiedColumnsTable;
        }

        /// <summary>
        /// A table that contains the common columns in the specified list of tables.
        /// </summary>
        private TableSymbol GetTableOfCommonColumns(IReadOnlyList<TableSymbol> tables)
        {
            // consider making this cache thread safe
            if (!_globalBindingCache.CommonColumnsMap.TryGetValue(tables, out var commonColumnsTable))
            {
                var cache = CanCache(tables);

                tables = tables.ToReadOnly();
                var columns = new List<ColumnSymbol>();

                Binder.GetCommonColumns(tables, columns);

                commonColumnsTable = new TableSymbol(columns);

                // since these are the common columns, open columns can only exist if all tables are open
                if (tables.Count > 0 && tables.All(t => t.IsOpen))
                {
                    commonColumnsTable = commonColumnsTable.WithIsOpen(true);
                }

                if (cache)
                {
                    _globalBindingCache.CommonColumnsMap[tables] = commonColumnsTable;
                }
            }

            return commonColumnsTable;
        }
#endregion

#region Symbols in scope
        /// <summary>
        /// Gets all the symbols that are in scope at the text position.
        /// </summary>
        public static void GetSymbolsInScope(SyntaxTree tree, int position, GlobalState globals, SymbolMatch match, IncludeFunctionKind include, List<Symbol> list, CancellationToken cancellationToken)
        {
            if (tree.IsSafeToRecurse)
            {
                globals = globals.WithCache();
                var bindingCache = globals.Cache.GetOrCreate<GlobalBindingCache>();
                lock (bindingCache)
                {
                    var binder = new Binder(
                        globals,
                        globals.Cluster,
                        globals.Database,
                        null, // currentFunction
                        GetDefaultOuterScope(globals),
                        bindingCache,
                        localBindingCache: null,
                        semanticInfoSetter: null,
                        cancellationToken: cancellationToken);
                    var startNode = GetStartNode(tree.Root, position);
                    binder.SetContext(startNode, position);
                    binder.GetSymbolsInContext(startNode, match, include, list);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="TableSymbol"/> that is in scope as the implicit set of columns accessible within a query.
        /// </summary>
        public static TableSymbol GetRowScope(SyntaxTree tree, int position, GlobalState globals, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (tree.IsSafeToRecurse)
            {
                globals = globals.WithCache();
                var bindingCache = globals.Cache.GetOrCreate<GlobalBindingCache>();
                lock (bindingCache)
                {
                    var binder = new Binder(
                        globals,
                        globals.Cluster,
                        globals.Database,
                        null, // currentFunction
                        GetDefaultOuterScope(globals),
                        bindingCache,
                        localBindingCache: null,
                        semanticInfoSetter: null,
                        cancellationToken: cancellationToken);
                    var startNode = GetStartNode(tree.Root, position);
                    binder.SetContext(startNode, position);
                    return binder._rowScope;
                }
            }
            else
            {
                return TableSymbol.Empty;
            }
        }

        private static SyntaxNode GetStartNode(SyntaxNode root, int position)
        {
            var token = root.GetTokenAt(position);

            if (token != null && position <= token.TextStart)
            {
                var prev = token.GetPreviousToken();
                if (prev != null && prev.Depth >= token.Depth)
                {
                    return prev.Parent;
                }

                return token.Parent;
            }

            return null;
        }

        private void GetSymbolsInContext(SyntaxNode contextNode, SymbolMatch match, IncludeFunctionKind include, List<Symbol> list)
        {
            if (_pathScope != null)
            {
                // so far only columns, tables and functions can be dot accessed.
                var memberMatch = match & (SymbolMatch.Column | SymbolMatch.Table | SymbolMatch.Function);

                // table.column only works in commands
                if (_pathScope is TableSymbol && !IsInsideControlCommandProper(contextNode))
                {
                    memberMatch &= ~SymbolMatch.Column;
                }

                // any columns or tables from left-hand side?
                if ((memberMatch & SymbolMatch.Column) != 0)
                {
                    if (_pathScope is TableSymbol table && table.IsOpen)
                    {
                        list.AddRange(GetDeclaredAndInferredColumns(table));
                    }
                    else if (_pathScope is TupleSymbol tuple && tuple.RelatedTable != null && tuple.RelatedTable.IsOpen)
                    {
                        list.AddRange(GetDeclaredAndInferredColumns(tuple.RelatedTable));
                    }
                    else
                    {
                        _pathScope.GetMembers(memberMatch, list);
                    }
                }
                else if (memberMatch != 0)
                {
                    _pathScope.GetMembers(memberMatch, list);
                }

                // any special functions from left-hand side?
                if ((match & SymbolMatch.Function) != 0)
                {
                    GetSpecialFunctions(null, list);
                }
            }
            else
            {
                switch (_scopeKind)
                {
                    case ScopeKind.Normal:
                        // row scope columns
                        if (_rowScope != null && (match & SymbolMatch.Column) != 0)
                        {
                            if (_rightRowScope != null)
                            {
                                // add $left and $right variables
                                list.Add(new VariableSymbol("$left", GetTuple(_rowScope)));
                                list.Add(new VariableSymbol("$right", GetTuple(_rightRowScope)));

                                // common columns
                                GetCommonColumns(GetDeclaredAndInferredColumns(_rowScope), GetDeclaredAndInferredColumns(_rightRowScope), list);
                            }
                            else
                            {
                                _rowScope.GetMembers(match, list);
                            }
                        }

                        var localMatch = match;
                        if ((include & IncludeFunctionKind.LocalFunctions) == 0)
                            localMatch &= ~SymbolMatch.Function;

                        // local symbols
                        _localScope.GetSymbols(localMatch, list);

                        // get any built-in functions
                        if ((match & SymbolMatch.Function) != 0 && (include & IncludeFunctionKind.BuiltInFunctions) != 0)
                        {
                            GetFunctionsInScope(match, null, IncludeFunctionKind.BuiltInFunctions, list);
                        }

                        // metadata symbols (tables, etc)
                        if (_currentDatabase != null)
                        {
                            var dbMatch = match;

                            if ((include & IncludeFunctionKind.DatabaseFunctions) == 0)
                                dbMatch &= ~SymbolMatch.Function;

                            _currentDatabase.GetMembers(dbMatch, list);
                        }

                        if ((match & SymbolMatch.Database) != 0)
                        {
                            _currentCluster.GetMembers(match, list);
                        }

                        if ((match & SymbolMatch.Cluster) != 0)
                        {
                            list.AddRange(_globals.Clusters);
                        }
                        break;

                    // aggregate scopes only see aggregate functions
                    case ScopeKind.Aggregate:
                        if ((match & SymbolMatch.Function) != 0)
                        {
                            GetFunctionsInScope(match, null, include, list);
                        }
                        break;

                    // plug-in scopes only see plug-in functions
                    case ScopeKind.PlugIn:
                        if ((match & SymbolMatch.Function) != 0)
                        {
                            GetFunctionsInScope(match, null, include, list);
                        }
                        break;

                    case ScopeKind.Option:
                        if ((match & SymbolMatch.Option) != 0)
                        {
                            list.AddRange(_globals.Options);
                        }
                        break;
                }
            }
        }

        private void GetSpecialFunctions(string name, List<Symbol> functions)
        {
            if (_pathScope != null)
            {
                // these special methods show up as dottable methods on their respective types
                switch (_pathScope.Kind)
                {
                    case SymbolKind.Cluster:
                        if (name == null || Functions.Database.Name == name)
                            functions.Add(Functions.Database);
                        break;
                    case SymbolKind.Database:
                        if (name == null || Functions.Database.Name == name)
                        {
                            functions.Add(Functions.Table);
                            functions.Add(Functions.ExternalTable);
                            functions.Add(Functions.MaterializedView);
                        }
                        break;
                }
            }
        }

        private void GetFunctionsInScope(
            SymbolMatch match,
            string name,
            IncludeFunctionKind include,
            List<Symbol> functions)
        {
            var allFunctions = s_symbolListPool.AllocateFromPool();
            try
            {
                GetFunctionsInScope(
                    _scopeKind,
                    name,
                    include,
                    allFunctions);

                foreach (var fn in allFunctions)
                {
                    if (fn.Matches(match))
                    {
                        functions.Add(fn);
                    }
                }
            }
            finally
            {
                s_symbolListPool.ReturnToPool(allFunctions);
            }
        }

        private void GetFunctionsInScope(
            ScopeKind kind,
            string name,
            IncludeFunctionKind include,
            List<Symbol> functions)
        {
            if (_pathScope != null)
            {
                GetSpecialFunctions(name, functions);
            }
            else
            {
                switch (kind)
                {
                    case ScopeKind.Aggregate:
                        if (name == null)
                        {
                            if ((include & IncludeFunctionKind.BuiltInFunctions) != 0)
                            {
                                functions.AddRange(_globals.Aggregates);
                            }

                            GetFunctionsInScope(ScopeKind.Normal, name, include, functions);
                        }
                        else
                        {
                            if ((include & IncludeFunctionKind.BuiltInFunctions) != 0)
                            {
                                var fn = _globals.GetAggregate(name);
                                if (fn != null)
                                {
                                    functions.Add(fn);
                                }
                            }

                            if (functions.Count == 0)
                            {
                                GetFunctionsInScope(ScopeKind.Normal, name, include, functions);
                            }
                        }
                        break;

                    case ScopeKind.PlugIn:
                        if ((include & IncludeFunctionKind.BuiltInFunctions) != 0)
                        {
                            if (name == null)
                            {
                                functions.AddRange(_globals.PlugIns);
                            }
                            else
                            {
                                var fn = _globals.GetPlugIn(name);
                                if (fn != null)
                                {
                                    functions.Add(fn);
                                }
                            }
                        }
                        break;

                    default:
                        if ((include & IncludeFunctionKind.BuiltInFunctions) != 0)
                        {
                            if (name == null)
                            {
                                functions.AddRange(_globals.Functions);
                            }
                            else if (functions.Count == 0)
                            {
                                var fn = _globals.GetFunction(name);
                                if (fn != null)
                                {
                                    functions.Add(fn);
                                }
                            }
                        }

                        if ((name == null || functions.Count == 0) && (include & IncludeFunctionKind.LocalFunctions) != 0)
                        {
                            GetLocalFunctionsInScope(name, functions);
                        }

                        if ((name == null || functions.Count == 0) && (include & IncludeFunctionKind.DatabaseFunctions) != 0 && _currentDatabase != null)
                        {
                            _currentDatabase.GetMembers(name, SymbolMatch.Function, functions);
                        }
                        break;
                }
            }
        }

        private void GetLocalFunctionsInScope(string name, List<Symbol> functions)
        {
            var locals = s_symbolListPool.AllocateFromPool();
            try
            {
                _localScope.GetSymbols(name, SymbolMatch.Local | SymbolMatch.Function, locals);

                foreach (Symbol local in locals)
                {
                    if (local is FunctionSymbol fs)
                    {
                        functions.Add(fs);
                    }
                    else if (local is PatternSymbol ps)
                    {
                        functions.Add(ps);
                    }
                    else if (local is VariableSymbol vs)
                    {
                        var resultType = GetResultType(local);
                        if (resultType is FunctionSymbol lfs)
                        {
                            functions.Add(lfs);
                        }
                        else if (resultType is PatternSymbol lps)
                        {
                            functions.Add(lps);
                        }
                    }
                }
            }
            finally
            {
                s_symbolListPool.ReturnToPool(locals);
            }
        }
#endregion

#region Common definitions
        private static ObjectPool<List<Symbol>> s_symbolListPool =
            new ObjectPool<List<Symbol>>(() => new List<Symbol>(), list => list.Clear());

        private static ObjectPool<List<Diagnostic>> s_diagnosticListPool =
            new ObjectPool<List<Diagnostic>>(() => new List<Diagnostic>(), list => list.Clear());

        private static ObjectPool<List<ColumnSymbol>> s_columnListPool =
            new ObjectPool<List<ColumnSymbol>>(() => new List<ColumnSymbol>(), list => list.Clear());

        private static ObjectPool<List<TableSymbol>> s_tableListPool =
            new ObjectPool<List<TableSymbol>>(() => new List<TableSymbol>(), list => list.Clear());

        private static ObjectPool<List<FunctionSymbol>> s_functionListPool =
            new ObjectPool<List<FunctionSymbol>>(() => new List<FunctionSymbol>(), list => list.Clear());

        private static ObjectPool<List<Signature>> s_signatureListPool =
            new ObjectPool<List<Signature>>(() => new List<Signature>(), list => list.Clear());

        private static ObjectPool<List<PatternSignature>> s_patternListPool =
            new ObjectPool<List<PatternSignature>>(() => new List<PatternSignature>(), list => list.Clear());

        private static ObjectPool<List<Expression>> s_expressionListPool =
            new ObjectPool<List<Expression>>(() => new List<Expression>(), list => list.Clear());

        private static ObjectPool<List<TypeSymbol>> s_typeListPool =
            new ObjectPool<List<TypeSymbol>>(() => new List<TypeSymbol>(), list => list.Clear());

        private static ObjectPool<HashSet<string>> s_stringSetPool =
            new ObjectPool<HashSet<string>>(() => new HashSet<string>(), s => s.Clear());

        private static ObjectPool<UniqueNameTable> s_uniqueNameTablePool =
            new ObjectPool<UniqueNameTable>(() => new UniqueNameTable(), t => t.Clear());

        private static ObjectPool<ProjectionBuilder> s_projectionBuilderPool =
            new ObjectPool<ProjectionBuilder>(() => new ProjectionBuilder(), b => b.Clear());

        private static ObjectPool<List<Parameter>> s_parameterListPool =
            new ObjectPool<List<Parameter>>(() => new List<Parameter>(), list => list.Clear());

        private static readonly SemanticInfo LiteralBoolInfo = new SemanticInfo(ScalarTypes.Bool, isConstant: true);
        private static readonly SemanticInfo LiteralIntInfo = new SemanticInfo(ScalarTypes.Int, isConstant: true);
        private static readonly SemanticInfo LiteralLongInfo = new SemanticInfo(ScalarTypes.Long, isConstant: true);
        private static readonly SemanticInfo LiteralRealInfo = new SemanticInfo(ScalarTypes.Real, isConstant: true);
        private static readonly SemanticInfo LiteralDecimalInfo = new SemanticInfo(ScalarTypes.Decimal, isConstant: true);
        private static readonly SemanticInfo LiteralStringInfo = new SemanticInfo(ScalarTypes.String, isConstant: true);
        private static readonly SemanticInfo LiteralDateTimeInfo = new SemanticInfo(ScalarTypes.DateTime, isConstant: true);
        private static readonly SemanticInfo LiteralTimeSpanInfo = new SemanticInfo(ScalarTypes.TimeSpan, isConstant: true);
        private static readonly SemanticInfo LiteralGuidInfo = new SemanticInfo(ScalarTypes.Guid, isConstant: true);
        private static readonly SemanticInfo LiteralDynamicInfo = new SemanticInfo(ScalarTypes.Dynamic, isConstant: true);
        private static readonly SemanticInfo UnknownInfo = new SemanticInfo(ScalarTypes.Unknown, isConstant: true);
        private static readonly SemanticInfo ErrorInfo = new SemanticInfo(ErrorSymbol.Instance);
        private static readonly SemanticInfo VoidInfo = new SemanticInfo(VoidSymbol.Instance);
#endregion

#region Name binding
        private static bool IsFunctionCallName(SyntaxNode name)
        {
            return name.Parent is FunctionCallExpression fn && fn.Name == name;
        }

        public bool IsInsideDatabaseFunctionDeclaration(SyntaxNode location)
        {
            // this is true when during expansion binding of database functions
            if (_currentFunction != null)
                return true;

            return IsInsideControlCommandFunctionDeclaration(location);
        }

        private static bool IsInsideControlCommandFunctionDeclaration(SyntaxNode location)
        {
            var functionDeclaration = location.GetFirstAncestor<FunctionDeclaration>();
            if (functionDeclaration != null && functionDeclaration.Parent is CustomNode)
                return true;

            var functionBody = location.GetFirstAncestor<FunctionBody>();
            if (functionBody != null && functionBody.Parent is CustomNode)
                return true;

            return false;
        }

        private static bool IsInsideControlCommand(SyntaxNode location)
        {
            // so far, only control commands use CustomNode
            return location.GetFirstAncestor<CustomNode>() != null;
        }

        private static bool IsInsideControlCommandProper(SyntaxNode location)
        {
            // its part of the control command but not part of any
            // function declaration or input query
            return IsInsideControlCommand(location)
                && !IsInsideControlCommandFunctionDeclaration(location)
                && !IsInsideControlCommandInputQuery(location);
        }

        private static bool IsInsideControlCommandInputQuery(SyntaxNode location)
        {
            // looking for statement list that is child of a CustomNode
            return location.GetFirstAncestor<SyntaxList<SeparatedElement<Statement>>>(
                s => s.Parent is CustomNode) != null;
        }

        private static bool IsInvocableFunctionName(SyntaxNode location)
        {
            // function names in non-executable parts of control commands are not invocable
            return !IsInsideControlCommandProper(location);
        }

        private static bool IsPossibleInvocableFunctionWithoutArgumentList(SyntaxNode location)
        {
            return !IsFunctionCallName(location)
                && IsInvocableFunctionName(location);
        }

        private static bool IsEvaluateFunctionName(SyntaxNode name)
        {
            return name.Parent is FunctionCallExpression fn
                && fn.Parent is EvaluateOperator;
        }

#if false
        private static bool IsInCommand(SyntaxNode location)
        {
            var command = location.GetFirstAncestor<Command>();
            var functionBody = location.GetFirstAncestor<FunctionBody>();
            return command != null && functionBody == null;
        }
#endif

        private SemanticInfo BindName(string name, SymbolMatch match, Expression location)
        {
            if (name == "")
                return ErrorInfo;

            if (_pathScope != null)
            {
                if (_pathScope == ScalarTypes.Dynamic)
                {
                    // any x.y where x is dynamic, is also dynamic
                    return LiteralDynamicInfo;
                }
                else if(_pathScope == ScalarTypes.Unknown)
                {
                    // any x.y where x is unknown, is also unknown (though probably dynamic)
                    return UnknownInfo;
                }
                else if (_pathScope == ErrorSymbol.Instance)
                {
                    // any x.y where x is an error, is also an error
                    return ErrorInfo;
                }
            }
            else if (name == "$left" && _rowScope != null && _rightRowScope != null)
            {
                var tuple = GetTuple(_rowScope);
                return new SemanticInfo(tuple, tuple);
            }
            else if (name == "$right" && _rightRowScope != null)
            {
                var tuple = GetTuple(_rightRowScope);
                return new SemanticInfo(tuple, tuple);
            }

            var list = s_symbolListPool.AllocateFromPool();
            try
            {
                bool allowZeroArgumentInvocation = false;

                if (IsFunctionCallName(location))
                {
                    if (_pathScope is DatabaseSymbol ds)
                    {
                        if (name == Functions.Table.Name)
                        {
                            list.Add(Functions.Table);
                        }
                        else if (name == Functions.ExternalTable.Name)
                        {
                            list.Add(Functions.ExternalTable);
                        }
                        else if (name == Functions.MaterializedView.Name)
                        {
                            list.Add(Functions.MaterializedView);
                        }
                        else
                        {
                            _pathScope.GetMembers(name, SymbolMatch.Function, list);
                        }
                    }
                    else if (_pathScope is ClusterSymbol cs && name == Functions.Database.Name)
                    {
                        list.Add(Functions.Database);
                    }
                    else
                    {
                        GetFunctionsInScope(_scopeKind, name, IncludeFunctionKind.All, list);
                    }
                }
                else
                {
                    // don't match the database functions that have same name as database tables
                    // if we are inside declaration of a database function
                    if (IsInsideDatabaseFunctionDeclaration(location) &&
                        _currentDatabase.GetAnyTable(name) != null)
                    {
                        match &= ~SymbolMatch.Function;
                    }

                    if (_pathScope != null)
                    {
                        if (_pathScope is TupleSymbol tuple
                            && tuple.RelatedTable != null
                            && tuple.RelatedTable.IsOpen
                            && TryGetDeclaredOrInferredColumn(tuple.RelatedTable, name, out var col))
                        {
                            list.Add(col);
                        }
                        else if (_pathScope is DatabaseSymbol ds)
                        {
                            // first look for functions
                            _pathScope.GetMembers(name, match & SymbolMatch.Function, list);
                            RemoveFunctionsThatCannotBeInvokedWithZeroArgs(list);

                            // database functions don't require argument lists to invoke
                            allowZeroArgumentInvocation = list.Count > 0;

                            if (list.Count == 0)
                            {
                                // next look for anything else (tables)
                                _pathScope.GetMembers(name, match & ~SymbolMatch.Function, list);
                            }

                            // otherwise this is possible an open table
                            if (list.Count == 0 && ds.IsOpen)
                            {
                                var table = GetOpenTable(name, ds);
                                return new SemanticInfo(table, table);
                            }
                        }
                        else if (!(_pathScope is TableSymbol) || IsInsideControlCommandProper(location))
                        {
                            _pathScope.GetMembers(name, match, list);
                        }
                    }

                    // check binding against any columns in the row scope
                    if (list.Count == 0 && _rowScope != null)
                    {
                        _rowScope.GetMembers(name, match, list);
                    }

                    // try secondary right-side row scope (from join operator)
                    if (list.Count == 0 && _rightRowScope != null)
                    {
                        _rightRowScope.GetMembers(name, match, list);
                    }

                    // try local variables (includes any user-defined functions)
                    if (list.Count == 0)
                    {
                        _localScope.GetSymbols(name, match, list);

                        // user defined functions do not require argument list if it has no arguments
                        allowZeroArgumentInvocation = list.Count > 0;
                    }

                    // look for zero-argument functions
                    if (list.Count == 0 && IsPossibleInvocableFunctionWithoutArgumentList(location) 
                        && (match & SymbolMatch.Function) != 0)
                    {
                        // database functions only (locally defined functions are already handled above)
                        GetFunctionsInScope(_scopeKind, name, IncludeFunctionKind.DatabaseFunctions, list);
                        RemoveFunctionsThatCannotBeInvokedWithZeroArgs(list);

                        // database functions do not require argument list if it has zero arguments.
                        allowZeroArgumentInvocation = list.Count > 0;
                    }

                    // other items in database (tables, etc)
                    if (list.Count == 0 && _currentDatabase != null)
                    {
                        _currentDatabase.GetMembers(name, match, list);
                    }

                    // databases can be directly referenced in commands
                    if (list.Count == 0 && _currentCluster != null && (match & SymbolMatch.Database) != 0)
                    {
                        _currentCluster.GetMembers(name, match, list);
                    }

                    // look for any built-in functions with matching name (even with those with parameters)
                    if (list.Count == 0 && (match & SymbolMatch.Function) != 0)
                    {
                        GetFunctionsInScope(_scopeKind, name, IncludeFunctionKind.BuiltInFunctions, list);
                    }

                    // infer column for this otherwise unbound reference?
                    if (list.Count == 0 && _rowScope != null && _rowScope.IsOpen && (match & SymbolMatch.Column) != 0)
                    {
                        // table is open, so create a dynamic column for the otherwise unbound name
                        list.Add(GetOpenColumn(name, _rowScope));
                    }
                }

                if (list.Count == 1)
                {
                    var item = list[0];
                    var resultType = GetResultType(item);

                    // check for zero-parameter function invocation not part of a function call node
                    if (resultType is FunctionSymbol fn && IsPossibleInvocableFunctionWithoutArgumentList(location))
                    {
                        var sig = fn.Signatures.FirstOrDefault(s => s.MinArgumentCount == 0);
                        if (sig != null && allowZeroArgumentInvocation)
                        {
                            var sigResult = GetSignatureResult(sig, EmptyReadOnlyList<Expression>.Instance, EmptyReadOnlyList<TypeSymbol>.Instance);
                            return new SemanticInfo(item, sigResult.Type, expander: sigResult.Expander);
                        }
                        else
                        {
                            var returnType = GetCommonReturnType(fn.Signatures, EmptyReadOnlyList<Expression>.Instance, EmptyReadOnlyList<TypeSymbol>.Instance);
                            return new SemanticInfo(item, returnType, DiagnosticFacts.GetFunctionRequiresArgumentList(name).WithLocation(location));
                        }
                    }
                    else
                    {
                        return CreateSemanticInfo(item);
                    }
                }
                else if (list.Count == 0)
                {
                    if (IsFunctionCallName(location))
                    {
                        if (_globals.GetAggregate(name) != null && _scopeKind != ScopeKind.Aggregate)
                        {
                            return new SemanticInfo(
                                ErrorSymbol.Instance,
                                DiagnosticFacts.GetAggregateNotAllowedInThisContext(name).WithLocation(location));
                        }
                        else if (_globals.GetPlugIn(name) != null && _scopeKind != ScopeKind.PlugIn)
                        {
                            return new SemanticInfo(
                                ErrorSymbol.Instance,
                                DiagnosticFacts.GetPluginNotAllowedInThisContext(name).WithLocation(location));
                        }
                        else if (IsEvaluateFunctionName(location))
                        {
                            if (PlugIns.GetPlugIn(name) != null)
                            {
                                return new SemanticInfo(
                                    ErrorSymbol.Instance,
                                    DiagnosticFacts.GetPlugInFunctionIsNotEnabled(name).WithLocation(location));
                            }
                            else
                            {
                                return new SemanticInfo(
                                    ErrorSymbol.Instance,
                                    DiagnosticFacts.GetPlugInFunctionNotDefined(name).WithLocation(location));
                            }
                        }
                        else if (IsFuzzyUnionOperand(location) || IsBestEffortUnionOperand(location))
                        {
                            return null;
                        }
                        else if (_pathScope is DatabaseSymbol ds)
                        {
                            if (ds != null && ds.IsOpen)
                            {
                                return new SemanticInfo(new TableSymbol("").WithIsOpen(true));
                            }
                            else
                            {
                                return new SemanticInfo(
                                    new TableSymbol("").WithIsOpen(true),
                                    DiagnosticFacts.GetNameDoesNotReferToAnyKnownFunction(name).WithLocation(location));
                            }
                        }
                        else if (IsInTabularContext(location))
                        {
                            return new SemanticInfo(
                                new TableSymbol("").WithIsOpen(true),
                                DiagnosticFacts.GetNameDoesNotReferToAnyKnownFunction(name).WithLocation(location));
                        }
                        else
                        {
                            return new SemanticInfo(
                                ErrorSymbol.Instance,
                                DiagnosticFacts.GetNameDoesNotReferToAnyKnownFunction(name).WithLocation(location));
                        }
                    }
                    else if (IsInTabularContext(location))
                    {
                        if (IsBestEffortUnionOperand(location))
                        {
                            return new SemanticInfo(
                               new TableSymbol().WithIsOpen(true),
                               DiagnosticFacts.GetBestEffortUnionOperandNotDefined(name).WithLocation(location));
                        }
                        else if(IsFuzzyUnionOperand(location))
                        {
                            return new SemanticInfo(
                                new TableSymbol().WithIsOpen(true),
                                DiagnosticFacts.GetFuzzyUnionOperandNotDefined(name).WithLocation(location));
                        }
                        else if (_pathScope is DatabaseSymbol ds
                            && ds.IsOpen)
                        {
                            return new SemanticInfo(new TableSymbol(name).WithIsOpen(true));
                        }
                        else
                        {
                            return new SemanticInfo(
                                new TableSymbol(name).WithIsOpen(true),
                                DiagnosticFacts.GetNameDoesNotReferToAnyKnownTable(name).WithLocation(location));
                        }
                    }
                    else
                    {
                        return new SemanticInfo(
                            ErrorSymbol.Instance,
                            DiagnosticFacts.GetNameDoesNotReferToAnyKnownItem(name).WithLocation(location));
                    }
                }
                else
                {
                    return new SemanticInfo(
                        new GroupSymbol(list.ToList()),
                        ErrorSymbol.Instance,
                        DiagnosticFacts.GetNameRefersToMoreThanOneItem(name).WithLocation(location));
                }
            }
            finally
            {
                s_symbolListPool.ReturnToPool(list);
            }
        }

        private static void RemoveFunctionsThatCannotBeInvokedWithZeroArgs(List<Symbol> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] is FunctionSymbol fn && fn.MinArgumentCount > 0)
                {
                    list.RemoveAt(i);
                }
            }
        }

        private static void GetWildcardSymbols(string pattern, IReadOnlyList<Symbol> symbols, List<Symbol> matchingSymbols)
        {
            foreach (var symbol in symbols)
            {
                if (KustoFacts.Matches(pattern, symbol.Name))
                {
                    matchingSymbols.Add(symbol);
                }
            }
        }

        /// <summary>
        /// Determine if the element (a name) is in a known tabular context
        /// </summary>
        private static bool IsInTabularContext(SyntaxElement element)
        {
            // if function call name, look further up to determine context
            if (element.Parent is FunctionCallExpression fc 
                && fc.Name == element)
            {
                element = element.Parent;
            }

            // if inside parenthesis or part of a list, look further up
            while (element.Parent is ParenthesizedExpression
                || element.Parent is SyntaxList
                || element.Parent is SeparatedElement)
            {
                element = element.Parent;
            }

            if (element.Parent != null)
            {
                // if x | op then x is expected to be tabular
                if (element.Parent is PipeExpression pe && pe.Expression == element)
                {
                    return true;
                }
                
                // if database().x then x is expected to be tabular
                if (element.Parent is PathExpression pt 
                    && pt.Selector == element
                    && pt.Expression.ResultType is DatabaseSymbol)
                {
                    return true;
                }

                // use completion hint to help us determine if context is tabular
                var hint = element.Parent.GetCompletionHint(element.IndexInParent);

                if (hint == Editor.CompletionHint.Table
                    || hint == Editor.CompletionHint.Tabular
                    || hint == Editor.CompletionHint.MaterializedView
                    || hint == Editor.CompletionHint.ExternalTable)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determine if the element is the operand of a fuzzy union
        /// </summary>
        private static bool IsFuzzyUnionOperand(SyntaxElement element)
        {
            return IsBoolParamUnionOperand(element, parameterKind: QueryOperatorParameters.IsFuzzy);
        }

        /// <summary>
        /// Determine if the element is the operand of a best effort union
        /// </summary>
        private static bool IsBestEffortUnionOperand(SyntaxElement element)
        {
            return IsBoolParamUnionOperand(element, parameterKind: QueryOperatorParameters.BestEffort);
        }

        /// <summary>
        /// Determine if the element is the operand of fuzzy/best effort union
        /// </summary>
        private static bool IsBoolParamUnionOperand(SyntaxElement element, QueryOperatorParameter parameterKind)
        {
            if (element.Parent is FunctionCallExpression fc && fc.Name == element)
            {
                element = element.Parent;
            }

            while (element.Parent is ParenthesizedExpression
                || element.Parent is SyntaxList
                || element.Parent is SeparatedElement)
            {
                element = element.Parent;
            }

            var context = element.Parent;

            if (context != null)
            {
                if (context is PipeExpression pe && pe.Expression == element)
                {
                    context = pe.Operator;
                }

                if (context is UnionOperator uo)
                {
                    var np = uo.Parameters.GetParameter(parameterKind);
                    return np != null && np.Expression.ConstantValue is bool b && b;
                }
            }

            return false;
        }
        #endregion

        #region Operator binding
        private SemanticInfo GetBinaryOperatorInfo(OperatorKind kind, Expression left, Expression right, SyntaxElement location)
        {
            return GetBinaryOperatorInfo(kind, left, GetResultTypeOrError(left), right, GetResultTypeOrError(right), location);
        }

        private SemanticInfo GetBinaryOperatorInfo(OperatorKind kind, Expression left, TypeSymbol leftType, Expression right, TypeSymbol rightType, SyntaxElement location)
        {
            var arguments = s_expressionListPool.AllocateFromPool();
            var argumentTypes = s_typeListPool.AllocateFromPool();

            try
            {
                arguments.Add(left);
                arguments.Add(right);

                argumentTypes.Add(leftType);
                argumentTypes.Add(rightType);

                return GetOperatorInfo(kind, arguments, argumentTypes, location);
            }
            finally
            {
                s_expressionListPool.ReturnToPool(arguments);
                s_typeListPool.ReturnToPool(argumentTypes);
            }
        }

        private SemanticInfo GetUnaryOperatorInfo(OperatorKind kind, Expression operand, SyntaxElement location)
        {
            var arguments = s_expressionListPool.AllocateFromPool();

            try
            {
                arguments.Add(operand);

                return GetOperatorInfo(kind, arguments, location);
            }
            finally
            {
                s_expressionListPool.ReturnToPool(arguments);
            }
        }

        private SemanticInfo GetOperatorInfo(OperatorKind kind, IReadOnlyList<Expression> arguments, SyntaxElement location)
        {
            var argumentTypes = s_typeListPool.AllocateFromPool();

            try
            {
                for (int i = 0; i < arguments.Count; i++)
                {
                    argumentTypes.Add(GetResultTypeOrError(arguments[i]));
                }

                return GetOperatorInfo(kind, arguments, argumentTypes, location);
            }
            finally
            {
                s_typeListPool.ReturnToPool(argumentTypes);
            }
        }

        private SemanticInfo GetOperatorInfo(OperatorKind kind, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes, SyntaxElement location)
        {
            var matchingSignatures = s_signatureListPool.AllocateFromPool();
            var diagnostics = s_diagnosticListPool.AllocateFromPool();

            try
            {
                var op = _globals.GetOperator(kind);

                GetBestMatchingSignatures(op.Signatures, arguments, argumentTypes, matchingSignatures);

                if (matchingSignatures.Count == 1)
                {
                    CheckSignature(matchingSignatures[0], arguments, argumentTypes, location, diagnostics);
                    var sigResult = GetSignatureResult(matchingSignatures[0], arguments, argumentTypes);
                    return new SemanticInfo(matchingSignatures[0].Symbol, sigResult.Type, diagnostics, isConstant: AllAreConstant(arguments));
                }
                else
                {
                    if (!ArgumentsHaveErrorsOrUnknown(argumentTypes))
                    {
                        diagnostics.Add(DiagnosticFacts.GetOperatorNotDefined(location.ToString(IncludeTrivia.Interior), argumentTypes).WithLocation(location));
                    }

                    var returnType = GetCommonReturnType(matchingSignatures, arguments, argumentTypes);
                    return new SemanticInfo(matchingSignatures[0].Symbol, returnType, diagnostics);
                }
            }
            finally
            {
                s_signatureListPool.ReturnToPool(matchingSignatures);
                s_diagnosticListPool.ReturnToPool(diagnostics);
            }
        }

        private bool AllAreConstant(IReadOnlyList<Expression> expressions)
        {
            for(int i = 0; i < expressions.Count; i++)
            {
                if (!GetIsConstant(expressions[i]))
                    return false;
            }

            return true;
        }

        private static OperatorKind GetOperatorKind(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.AddExpression:
                    return OperatorKind.Add;
                case SyntaxKind.SubtractExpression:
                    return OperatorKind.Subtract;
                case SyntaxKind.MultiplyExpression:
                    return OperatorKind.Multiply;
                case SyntaxKind.DivideExpression:
                    return OperatorKind.Divide;
                case SyntaxKind.ModuloExpression:
                    return OperatorKind.Modulo;
                case SyntaxKind.UnaryMinusExpression:
                    return OperatorKind.UnaryMinus;
                case SyntaxKind.UnaryPlusExpression:
                    return OperatorKind.UnaryPlus;
                case SyntaxKind.EqualExpression:
                    return OperatorKind.Equal;
                case SyntaxKind.NotEqualExpression:
                    return OperatorKind.NotEqual;
                case SyntaxKind.LessThanExpression:
                    return OperatorKind.LessThan;
                case SyntaxKind.LessThanOrEqualExpression:
                    return OperatorKind.LessThanOrEqual;
                case SyntaxKind.GreaterThanExpression:
                    return OperatorKind.GreaterThan;
                case SyntaxKind.GreaterThanOrEqualExpression:
                    return OperatorKind.GreaterThanOrEqual;
                case SyntaxKind.EqualTildeExpression:
                    return OperatorKind.EqualTilde;
                case SyntaxKind.BangTildeExpression:
                    return OperatorKind.BangTilde;
                case SyntaxKind.HasExpression:
                    return OperatorKind.Has;
                case SyntaxKind.HasCsExpression:
                    return OperatorKind.HasCs;
                case SyntaxKind.NotHasExpression:
                    return OperatorKind.NotHas;
                case SyntaxKind.NotHasCsExpression:
                    return OperatorKind.NotHasCs;
                case SyntaxKind.HasPrefixExpression:
                    return OperatorKind.HasPrefix;
                case SyntaxKind.HasPrefixCsExpression:
                    return OperatorKind.HasPrefixCs;
                case SyntaxKind.NotHasPrefixExpression:
                    return OperatorKind.NotHasPrefix;
                case SyntaxKind.NotHasPrefixCsExpression:
                    return OperatorKind.NotHasPrefixCs;
                case SyntaxKind.HasSuffixExpression:
                    return OperatorKind.HasSuffix;
                case SyntaxKind.HasSuffixCsExpression:
                    return OperatorKind.HasSuffixCs;
                case SyntaxKind.NotHasSuffixExpression:
                    return OperatorKind.NotHasSuffix;
                case SyntaxKind.NotHasSuffixCsExpression:
                    return OperatorKind.NotHasSuffixCs;
                case SyntaxKind.LikeExpression:
                    return OperatorKind.Like;
                case SyntaxKind.LikeCsExpression:
                    return OperatorKind.LikeCs;
                case SyntaxKind.NotLikeExpression:
                    return OperatorKind.NotLike;
                case SyntaxKind.NotLikeCsExpression:
                    return OperatorKind.NotLikeCs;
                case SyntaxKind.ContainsExpression:
                    return OperatorKind.Contains;
                case SyntaxKind.ContainsCsExpression:
                    return OperatorKind.ContainsCs;
                case SyntaxKind.NotContainsExpression:
                    return OperatorKind.NotContains;
                case SyntaxKind.NotContainsCsExpression:
                    return OperatorKind.NotContainsCs;
                case SyntaxKind.StartsWithExpression:
                    return OperatorKind.StartsWith;
                case SyntaxKind.StartsWithCsExpression:
                    return OperatorKind.StartsWithCs;
                case SyntaxKind.NotStartsWithExpression:
                    return OperatorKind.NotStartsWith;
                case SyntaxKind.NotStartsWithCsExpression:
                    return OperatorKind.NotStartsWithCs;
                case SyntaxKind.EndsWithExpression:
                    return OperatorKind.EndsWith;
                case SyntaxKind.EndsWithCsExpression:
                    return OperatorKind.EndsWithCs;
                case SyntaxKind.NotEndsWithExpression:
                    return OperatorKind.NotEndsWith;
                case SyntaxKind.NotEndsWithCsExpression:
                    return OperatorKind.NotEndsWith;
                case SyntaxKind.MatchesRegexExpression:
                    return OperatorKind.MatchRegex;
                case SyntaxKind.InExpression:
                    return OperatorKind.In;
                case SyntaxKind.InCsExpression:
                    return OperatorKind.InCs;
                case SyntaxKind.NotInExpression:
                    return OperatorKind.NotIn;
                case SyntaxKind.NotInCsExpression:
                    return OperatorKind.NotInCs;
                case SyntaxKind.BetweenExpression:
                    return OperatorKind.Between;
                case SyntaxKind.NotBetweenExpression:
                    return OperatorKind.NotBetween;
                case SyntaxKind.AndExpression:
                    return OperatorKind.And;
                case SyntaxKind.OrExpression:
                    return OperatorKind.Or;
                case SyntaxKind.SearchExpression:
                    return OperatorKind.Search;
                case SyntaxKind.HasAnyExpression:
                    return OperatorKind.HasAny;
                case SyntaxKind.HasAllExpression:
                    return OperatorKind.HasAll;
                default:
                    return OperatorKind.None;
            }
        }
#endregion

#region Signature binding
        private void GetArgumentsAndTypes(
            FunctionCallExpression functionCall,
            List<Expression> arguments,
            List<TypeSymbol> argumentTypes)
        {
            var expressions = functionCall.ArgumentList.Expressions;

            for (int i = 0, n = expressions.Count; i < n; i++)
            {
                var arg = expressions[i].Element;
                arguments.Add(arg);
                argumentTypes.Add(GetResultTypeOrError(arg));
            }

            if (IsInvokeOperatorFunctionCall(functionCall))
            {
                // add fake argument to represent the implicit value
                arguments.Insert(0, functionCall.Name);
                argumentTypes.Insert(0, _implicitArgumentType);
            }
        }

        private struct SignatureResult
        {
            public TypeSymbol Type { get; }

            public Func<SyntaxNode> Expander { get; }

            public SignatureResult(TypeSymbol type, Func<SyntaxNode> expander)
            {
                this.Type = type;
                this.Expander = expander;
            }

            public static implicit operator SignatureResult(TypeSymbol type)
            {
                return new SignatureResult(type, null);
            }
        }

        /// <summary>
        /// Gets the return type of the signature when invoked with the specified arguments.
        /// </summary>
        private SignatureResult GetSignatureResult(
            Signature signature,
            IReadOnlyList<Expression> arguments,
            IReadOnlyList<TypeSymbol> argumentTypes,
            List<Diagnostic> diagnostics = null)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            if (argumentTypes == null)
                throw new ArgumentNullException(nameof(argumentTypes));

            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                signature.GetArgumentParameters(arguments, argumentParameters);
                return GetSignatureResult(signature, arguments, argumentTypes, argumentParameters, diagnostics);
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters);
            }
        }

        /// <summary>
        /// Gets the return type of the signature when invoked with the specified arguments.
        /// </summary>
        private SignatureResult GetSignatureResult(
            Signature signature,
            IReadOnlyList<Expression> arguments,
            IReadOnlyList<TypeSymbol> argumentTypes,
            IReadOnlyList<Parameter> argumentParameters,
            List<Diagnostic> diagnostics = null)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            if (argumentTypes == null)
                throw new ArgumentNullException(nameof(argumentTypes));

            if (argumentParameters == null)
                throw new ArgumentNullException(nameof(argumentParameters));

            switch (signature.ReturnKind)
            {
                case ReturnTypeKind.Declared:
                    return signature.DeclaredReturnType;

                case ReturnTypeKind.Computed:
                    return this.GetComputedSignatureResult(signature, arguments, argumentTypes);

                case ReturnTypeKind.Parameter0:
                    var iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? argumentTypes[iArg] : ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter1:
                    iArg = argumentParameters.IndexOf(signature.Parameters[1]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? argumentTypes[iArg] : ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter2:
                    iArg = argumentParameters.IndexOf(signature.Parameters[2]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? argumentTypes[iArg] : ErrorSymbol.Instance;

                case ReturnTypeKind.ParameterN:
                    iArg = argumentParameters.IndexOf(signature.Parameters[signature.Parameters.Count - 1]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? argumentTypes[iArg] : ErrorSymbol.Instance;

                case ReturnTypeKind.ParameterNLiteral:
                    iArg = argumentParameters.IndexOf(signature.Parameters[signature.Parameters.Count - 1]);
                    return iArg >= 0 && iArg < arguments.Count ? GetTypeOfType(arguments[iArg]) : ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter0Promoted:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    return iArg >= 0 && iArg < argumentTypes.Count ? Promote(argumentTypes[iArg]) : ErrorSymbol.Instance;

                case ReturnTypeKind.Common:
                    return GetCommonArgumentType(argumentParameters, argumentTypes) ?? ErrorSymbol.Instance;

                case ReturnTypeKind.Widest:
                    return GetWidestArgumentType(signature, argumentTypes) ?? ErrorSymbol.Instance;

                case ReturnTypeKind.Parameter0Cluster:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count 
                        && TryGetLiteralStringValue(arguments[iArg], out var clusterName))
                    {
                        return GetClusterFunctionResult(clusterName, arguments[iArg], diagnostics);
                    }
                    else
                    {
                        return new ClusterSymbol("", null, isOpen: true);
                    }

                case ReturnTypeKind.Parameter0Database:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count 
                        && TryGetLiteralStringValue(arguments[iArg], out var databaseName))
                    {
                        return GetDatabaseFunctionResult(databaseName, arguments[iArg], diagnostics);
                    }
                    else
                    {
                        return new DatabaseSymbol("", null, isOpen: true);
                    }

                case ReturnTypeKind.Parameter0Table:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count 
                        && TryGetLiteralStringValue(arguments[iArg], out var tableName))
                    {
                        return GetTableFunctionResult(tableName, arguments[iArg], diagnostics);
                    }
                    else
                    {
                        return TableSymbol.Empty.WithIsOpen(true);
                    }

                case ReturnTypeKind.Parameter0ExternalTable:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count 
                        && TryGetLiteralStringValue(arguments[iArg], out var externalTableName))
                    {
                        return GetExternalTableFunctionResult(externalTableName, arguments[iArg], diagnostics);
                    }
                    else
                    {
                        return TableSymbol.Empty.WithIsOpen(true);
                    }
                case ReturnTypeKind.Parameter0MaterializedView:
                    iArg = argumentParameters.IndexOf(signature.Parameters[0]);
                    if (iArg >= 0 && iArg < arguments.Count 
                        && TryGetLiteralStringValue(arguments[iArg], out var materializedViewName))
                    {
                        return GetMaterializedViewFunctionResult(materializedViewName, arguments[iArg], diagnostics);
                    }
                    else
                    {
                        return TableSymbol.Empty.WithIsOpen(true);
                    }
                case ReturnTypeKind.Custom:
                    return signature.CustomReturnType(_rowScope ?? TableSymbol.Empty, arguments, signature) ?? ErrorSymbol.Instance;

                default:
                    throw new NotImplementedException();
            }
        }

        private SignatureResult GetComputedSignatureResult(Signature signature, IReadOnlyList<Expression> arguments = null, IReadOnlyList<TypeSymbol> argumentTypes = null)
        {
            var outerScope = _localScope.Copy();

            if (signature.FunctionBodyFacts == FunctionBodyFacts.None)
            {
                // body has non-variable (fixed) return type and does not contain cluster/database/table calls.
                return new SignatureResult(
                    signature.NonVariableComputedReturnType,
                    GetDeferredCallSiteExpansion(signature, arguments, argumentTypes, outerScope));
            }
            else
            {
                var expansion = this.GetCallSiteExpansion(signature, arguments, argumentTypes, outerScope);
                var returnType = expansion?.Body?.Expression?.ResultType ?? ErrorSymbol.Instance;
                return new SignatureResult(returnType, () => expansion?.Root);
            }
        }

        private Func<SyntaxNode> GetDeferredCallSiteExpansion(Signature signature, IReadOnlyList<Expression> arguments = null, IReadOnlyList<TypeSymbol> argumentTypes = null, LocalScope outerScope = null)
        {
            Expansion expansion = null;
            var args = arguments.ToReadOnly(); // force copy
            var types = argumentTypes.ToReadOnly(); // force copy

            return () =>
            {
                if (expansion == null)
                {
                    // re-introduce binding lock since deferred function can be called outside the current binding lock
                    lock (this._globalBindingCache)
                    {
                        expansion = this.GetCallSiteExpansion(signature, args, types, outerScope);
                    }
                }

                return expansion.Root;
            };
        }

        internal static bool TryGetLiteralStringValue(Expression expression, out string value)
        {
            if (TryGetLiteralValue(expression, out var objValue))
            {
                value = objValue as string;
                return value != null;
            }
            else
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the value of the literal if the expression is a literal or refers to literal
        /// </summary>
        internal static bool TryGetLiteralValue(Expression expression, out object value)
        {
            expression = GetUnderlyingExpression(expression);

            if (expression.IsLiteral)
            {
                value = expression.LiteralValue;
                return value != null;
            }
            else if (expression is NameReference nr && nr.ReferencedSymbol is VariableSymbol vs && vs.IsConstant)
            {
                value = vs.ConstantValue;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the cluster for the specified name, or an empty open cluster.
        /// </summary>
        private ClusterSymbol GetClusterFunctionResult(string name, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            var cluster = _globals.GetCluster(name);
            if (cluster == null)
            {
                if (diagnostics != null && location != null)
                {
                    diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownCluster(name).WithLocation(location));
                }

                cluster = GetOpenCluster(name);
            }

            return cluster;
        }

        /// <summary>
        /// Gets the database addressable in the current context.
        /// </summary>
        private TypeSymbol GetDatabaseFunctionResult(string name, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            var cluster = _pathScope as ClusterSymbol ?? _currentCluster;
            var db = GetDatabase(name, cluster);

            if (db == null)
            {
                if (diagnostics != null && location != null)
                {
                    diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownDatabase(name).WithLocation(location));
                }

                db = GetOpenDatabase(name, cluster);
            }

            return db;
        }

        /// <summary>
        /// Gets the result of calling the table() function in the current context.
        /// </summary>
        private TypeSymbol GetTableFunctionResult(string name, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            var pathDb = _pathScope as DatabaseSymbol;

            // check for local table first
            if (pathDb == null)
            {
                var match = SymbolMatch.Table | SymbolMatch.Local;

                var symbols = s_symbolListPool.AllocateFromPool();
                try
                {
                    // check scope for variables, etc
                    _localScope.GetSymbols(name, match, symbols);

                    if (symbols.Count > 0)
                    {
                        var result = GetResultType(symbols[0]);
                        return result as TableSymbol ?? (TypeSymbol)ErrorSymbol.Instance;
                    }
                }
                finally
                {
                    s_symbolListPool.ReturnToPool(symbols);
                }
            }

            var db = pathDb ?? _currentDatabase;
            var table = GetTable(name, db);

            if (table == null)
            {
                if (diagnostics != null && location != null)
                {
                    diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownTable(name).WithLocation(location));
                }

                table = GetOpenTable(name, db);
            }

            return table;
        }

        /// <summary>
        /// Gets the result of calling the external_table() function in the current context.
        /// </summary>
        private TypeSymbol GetExternalTableFunctionResult(string name, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            var db = _pathScope as DatabaseSymbol ?? _currentDatabase;
            var table = db.GetExternalTable(name);

            if (table == null)
            {
                if (diagnostics != null && location != null)
                {
                    diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownExternalTable(name).WithLocation(location));
                }

                table = new TableSymbol(name).WithIsExternal(true).WithIsOpen(true);
            }

            return table;
        }

        /// <summary>
        /// Gets the result of calling the materialized_view() function in the current context.
        /// </summary>
        private TypeSymbol GetMaterializedViewFunctionResult(string name, SyntaxNode location, List<Diagnostic> diagnostics)
        {
            var db = _pathScope as DatabaseSymbol ?? _currentDatabase;
            var table = db.GetMaterializedView(name);

            if (table == null)
            {
                if (diagnostics != null && location != null)
                {
                    diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownMaterializedView(name).WithLocation(location));
                }

                table = new TableSymbol(name).WithIsMaterializedView(true).WithIsOpen(true);
            }

            return table;
        }

        /// <summary>
        /// Determines if <see cref="P:type1"/> can be promoted to <see cref="P:type2"/>
        /// </summary>
        public static bool IsPromotable(TypeSymbol type1, TypeSymbol type2)
        {
            return type1 is ScalarSymbol type1Scalar && type2 is ScalarSymbol type2Scalar && type2Scalar.IsWiderThan(type1Scalar);
        }

        /// <summary>
        /// Promotes a type to its most general form.  int -> long, decimal -> real
        /// </summary>
        public static TypeSymbol Promote(TypeSymbol symbol)
        {
            if (symbol == ScalarTypes.Int)
            {
                return ScalarTypes.Long;
            }
            else if (symbol == ScalarTypes.Decimal)
            {
                return ScalarTypes.Real;
            }
            else
            {
                return symbol;
            }
        }

        /// <summary>
        /// Gets the widest numeric type of the argument types.
        /// The widest type is the one that can contain the values of all the other types:
        /// </summary>
        public static TypeSymbol GetWidestArgumentType(Signature signature, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            ScalarSymbol widestType = null;

            for (int i = 0; i < argumentTypes.Count; i++)
            {
                var argType = argumentTypes[i];

                if (argType is ScalarSymbol s && s.IsNumeric && s != widestType)
                {
                    if (widestType == null || s.IsWiderThan(widestType))
                    {
                        widestType = s;
                    }
                }
            }

            return widestType;
        }

        /// <summary>
        /// Gets the common argument type for arguments corresponding to parameters constrained to specific <see cref="ParameterTypeKind"/>.CommonXXX values.
        /// </summary>
        public static TypeSymbol GetCommonArgumentType(IReadOnlyList<Parameter> argumentParameters, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            TypeSymbol commonType = null;
            var hadUnknown = false;

            for (int i = 0; i < argumentTypes.Count; i++)
            {
                var parameter = argumentParameters[i];
                if (parameter != null)
                {
                    var argType = argumentTypes[i];

                    if ((parameter.TypeKind == ParameterTypeKind.CommonScalar && argType.IsScalar)
                        || (parameter.TypeKind == ParameterTypeKind.CommonScalarOrDynamic && argType.IsScalar)
                        || (parameter.TypeKind == ParameterTypeKind.CommonNumber && IsNumber(argType))
                        || (parameter.TypeKind == ParameterTypeKind.CommonSummable && IsSummable(argType))
                        || (parameter.TypeKind == ParameterTypeKind.CommonOrderable && IsOrderable(argType)))
                    {
                        if (commonType == null )
                        {
                            if (argType == ScalarTypes.Unknown)
                            {
                                hadUnknown = true;
                            }
                            else
                            {
                                commonType = argType;
                            }
                        }
                        else if (IsPromotable(commonType, argType))
                        {
                            // a type that can be promoted to is better
                            commonType = argType;
                        }
                        else if (commonType == ScalarTypes.Dynamic)
                        {
                            // non-dynamic scalars are better
                            commonType = argType;
                        }
                    }
                }
            }

            if (commonType == null && hadUnknown)
                return ScalarTypes.Unknown;

            return commonType;
        }

        /// <summary>
        /// Gets the common return type across a set of signatures, or error if there is no common type.
        /// The common return type is the return type all the signatures share, or the error type if the return types differ.
        /// </summary>
        private TypeSymbol GetCommonReturnType(IReadOnlyList<Signature> signatures, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            if (signatures.Count == 0)
            {
                return ErrorSymbol.Instance;
            }
            else if (signatures.Count == 1)
            {
                return GetSignatureResult(signatures[0], arguments, argumentTypes).Type;
            }
            else
            {
                var firstType = GetSignatureResult(signatures[0], arguments, argumentTypes).Type;

                for (int i = 1; i < signatures.Count; i++)
                {
                    var type = GetSignatureResult(signatures[i], arguments, argumentTypes).Type;
                    if (!SymbolsAssignable(firstType, type))
                    {
                        if (ArgumentsHaveErrorsOrUnknown(argumentTypes))
                        {
                            return ScalarTypes.Unknown;
                        }
                        else
                        {
                            return ErrorSymbol.Instance;
                        }
                    }
                }

                return firstType;
            }
        }

        /// <summary>
        /// Gets the common scalar type amongst a set of types.
        /// This is either the one type if they are all them same type, the most promoted of the types, or the common type of the types that are not dynamic.
        /// </summary>
        private static TypeSymbol GetCommonScalarType(params TypeSymbol[] types)
        {
            TypeSymbol commonType = null;
            bool hadUnknown = false;

            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];

                if (type.IsScalar)
                {
                    // TODO: should there be a general betterness between types instead of these specific rules?
                    if (commonType == null)
                    {
                        if (type == ScalarTypes.Unknown)
                        {
                            hadUnknown = true;
                        }
                        else
                        {
                            commonType = type;
                        }
                    }
                    else if (IsPromotable(commonType, type))
                    {
                        // a type that can be promoted to is better
                        commonType = type;
                    }
                    else if (SymbolsAssignable(commonType, ScalarTypes.Dynamic))
                    {
                        // non-dynamic scalars are better
                        commonType = type;
                    }
                }
            }

            if (commonType == null && hadUnknown)
                return ScalarTypes.Unknown;

            return commonType;
        }

        /// <summary>
        /// Gets the signatures that best match the specified arguments.
        /// If there is no best match, then multiple signatures will be returned.
        /// </summary>
        private void GetBestMatchingSignatures(IReadOnlyList<Signature> signatures, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes, List<Signature> result)
        {
            var argCount = argumentTypes.Count;

            if (signatures.Count == 0)
            {
                return;
            }
            else if (signatures.Count == 1)
            {
                result.Add(signatures[0]);
                return;
            }

            // determine candidates
            if (signatures.Count > 1)
            {
                var closestCount = 0;
                var maxCount = 0;

                foreach (var s in signatures)
                {
                    if (argCount >= s.MinArgumentCount && argCount <= s.MaxArgumentCount)
                    {
                        result.Add(s);
                    }
                    else if (argCount < s.MinArgumentCount && closestCount > s.MinArgumentCount)
                    {
                        closestCount = s.MinArgumentCount;
                    }

                    if (s.MaxArgumentCount > maxCount)
                    {
                        maxCount = s.MaxArgumentCount;
                    }
                }

                // if we didn't already find candidates, pick all with closest count
                if (result.Count == 0)
                {
                    if (closestCount == 0)
                    {
                        closestCount = maxCount;
                    }

                    foreach (var s in signatures)
                    {
                        if (closestCount >= s.MinArgumentCount && closestCount <= s.MaxArgumentCount)
                        {
                            result.Add(s);
                        }
                    }
                }
            }

            // reduce results to best matching functions
            if (result.Count > 1)
            {
                int mostMatchingParameterCount = 0;

                // determine the most matching parameter count
                foreach (var s in result)
                {
                    var count = GetParameterMatchCount(s, arguments, argumentTypes);
                    if (count > mostMatchingParameterCount)
                    {
                        mostMatchingParameterCount = count;
                    }
                }

                // remove all candidates that do not have the most matching parameters
                for (int i = result.Count - 1; i >= 0; i--)
                {
                    var f = result[i];
                    if (GetParameterMatchCount(f, arguments, argumentTypes) != mostMatchingParameterCount)
                    {
                        result.RemoveAt(i);
                    }
                }

                // still more than one?  Try to find best match
                if (result.Count > 1)
                {
                    var best = result[0];
                    for (int i = 1; i < result.Count; i++)
                    {
                        if (IsBetterSignatureMatch(result[i], best, arguments, argumentTypes))
                        {
                            best = result[i];
                        }
                    }

                    for (int i = 0; i < result.Count; i++)
                    {
                        if (result[i] != best && !IsBetterSignatureMatch(best, result[i], arguments, argumentTypes))
                        {
                            // non-best is now better than best second time around??? must be ambiguous
                            return;
                        }
                    }

                    // one was clearly the best
                    result.Clear();
                    result.Add(best);
                }
            }
        }

        /// <summary>
        /// Determines if <see cref="P:signature1"/> is a better match than <see cref="P:signature2"/> for the specified arguments.
        /// </summary>
        private bool IsBetterSignatureMatch(Signature signature1, Signature signature2, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            var argCount = argumentTypes.Count;
            var matchCount1 = GetParameterMatchCount(signature1, arguments, argumentTypes);
            var matchCount2 = GetParameterMatchCount(signature2, arguments, argumentTypes);

            // if function matches all arguments but other-function does not, function is better
            if (matchCount1 == argCount && matchCount2 < argCount)
                return true;

            // function with better parameter matches wins
            Signature better = null;
            for (int i = 0; i < argumentTypes.Count; i++)
            {
                if (IsBetterParameterMatch(signature1, signature2, arguments, argumentTypes, i))
                {
                    if (better == signature2) // function1 is better here but function2 was better before, therefore it is ambiguous
                        break;

                    better = signature1;
                }
                else if (IsBetterParameterMatch(signature2, signature1, arguments, argumentTypes, i))
                {
                    if (better == signature1) // function2 is better here but function1 was better before, therefore it is ambiguous
                    {
                        better = null;
                        break;
                    }

                    better = signature2;
                }
            }

            // if function1 is clearly better on all parameter matches, function1 is better
            if (better == signature1)
                return true;

            // ambigous on parameter-to-parameter matches
            // if function1 has more matches than function2, function1 is better
            return matchCount1 > matchCount2;
        }

        /// <summary>
        /// Determines if <see cref="P:signature1"/> is a better match than <see cref="P:signature"/> for the specified argument at the corresponding parameter position.
        /// </summary>
        private bool IsBetterParameterMatch(Signature signature1, Signature signature2, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes, int argumentIndex)
        {
            var argumentParameters1 = s_parameterListPool.AllocateFromPool();
            var argumentParameters2 = s_parameterListPool.AllocateFromPool();
            try
            {
                signature1.GetArgumentParameters(arguments, argumentParameters1);
                signature2.GetArgumentParameters(arguments, argumentParameters2);

                var matches1 = GetParameterMatchKind(signature1, argumentParameters1, argumentTypes, argumentParameters1[argumentIndex], arguments[argumentIndex], argumentTypes[argumentIndex]);
                var matches2 = GetParameterMatchKind(signature2, argumentParameters2, argumentTypes, argumentParameters2[argumentIndex], arguments[argumentIndex], argumentTypes[argumentIndex]);

                return matches1 > matches2;
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters2);
                s_parameterListPool.ReturnToPool(argumentParameters1);
            }
        }

        /// <summary>
        /// Determines the number of arguments that match their corresponding signature parameter.
        /// </summary>
        private int GetParameterMatchCount(Signature signature, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            var argCount = argumentTypes.Count;
            int matches = 0;

            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                signature.GetArgumentParameters(arguments, argumentParameters);

                for (int i = 0; i < argCount; i++)
                {
                    if (GetParameterMatchKind(
                        signature, 
                        argumentParameters, argumentTypes,
                        argumentParameters[i], arguments[i], argumentTypes[i]) != ParameterMatchKind.None)
                    {
                        matches++;
                    }
                }
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters);
            }


            return matches;
        }

        private ParameterMatchKind GetParameterMatchKind(
            Signature signature,
            IReadOnlyList<Parameter> argumentParameters,
            IReadOnlyList<TypeSymbol> argumentTypes,
            Parameter parameter,
            Expression argument,
            TypeSymbol argumentType)
        {
            return GetParameterMatchKind(signature, argumentParameters, argumentTypes, parameter, argument, argumentType, AllowLooseParameterMatching(signature));
        }

        /// <summary>
        /// Determines the kind of match that the argument has with its corresponding signature parameter.
        /// </summary>
        public static ParameterMatchKind GetParameterMatchKind(
            Signature signature, 
            IReadOnlyList<Parameter> argumentParameters, 
            IReadOnlyList<TypeSymbol> argumentTypes, 
            Parameter parameter, 
            Expression argument, 
            TypeSymbol argumentType,
            bool allowLooseParameterMatching)
        {
            if (parameter == null)
                return ParameterMatchKind.None;

            if (argumentType == ScalarTypes.Unknown)
                return ParameterMatchKind.Unknown;

            if (parameter.DefaultValueIndicator != null
                && argumentType == ScalarTypes.String
                && argument is LiteralExpression lit
                && lit.LiteralValue is string value
                && value == parameter.DefaultValueIndicator)
            {
                return ParameterMatchKind.Exact;
            }

            if (argument is StarExpression)
            {
                return (parameter.ArgumentKind == ArgumentKind.Star)
                    ? ParameterMatchKind.Exact : ParameterMatchKind.None;
            }
            else if (parameter.ArgumentKind == ArgumentKind.Star)
            {
                return ParameterMatchKind.None;
            }

            switch (parameter.TypeKind)
            {
                case ParameterTypeKind.Declared:
                    if (SymbolsAssignable(parameter.DeclaredTypes, argumentType, Conversion.None))
                    {
                        if (parameter.DeclaredTypes.Count == 1)
                        {
                            return ParameterMatchKind.Exact;
                        }
                        else
                        {
                            return ParameterMatchKind.OneOfTwo;
                        }
                    }
                    else if (SymbolsAssignable(parameter.DeclaredTypes, argumentType, Conversion.Promotable))
                    {
                        return ParameterMatchKind.Promoted;
                    }
                    else if (allowLooseParameterMatching
                        && SymbolsAssignable(parameter.DeclaredTypes, argumentType, Conversion.Compatible))
                    {
                        return ParameterMatchKind.Compatible;
                    }
                    break;

                case ParameterTypeKind.Scalar:
                    if (argumentType.IsScalar)
                        return ParameterMatchKind.Scalar;
                    break;

                case ParameterTypeKind.Integer:
                    if (IsInteger(argumentType))
                        return ParameterMatchKind.OneOfTwo;
                    break;

                case ParameterTypeKind.RealOrDecimal:
                    if (IsRealOrDecimal(argumentType))
                        return ParameterMatchKind.OneOfTwo;
                    break;

                case ParameterTypeKind.StringOrDynamic:
                    if (IsStringOrDynamic(argumentType))
                        return ParameterMatchKind.OneOfTwo;
                    break;

                case ParameterTypeKind.IntegerOrDynamic:
                    if (IsIntegerOrDynamic(argumentType))
                        return ParameterMatchKind.OneOfTwo;
                    break;

                case ParameterTypeKind.Number:
                    if (IsNumber(argumentType))
                        return ParameterMatchKind.Number;
                    break;

                case ParameterTypeKind.Summable:
                    if (IsSummable(argumentType))
                        return ParameterMatchKind.Summable;
                    break;
                case ParameterTypeKind.Orderable:
                    if (IsOrderable(argumentType))
                        return ParameterMatchKind.Orderable;
                    break;
                case ParameterTypeKind.Tabular:
                    if (IsTabular(argumentType))
                        return ParameterMatchKind.Tabular;
                    break;

                case ParameterTypeKind.Database:
                    if (IsDatabase(argumentType))
                        return ParameterMatchKind.Database;
                    break;

                case ParameterTypeKind.Cluster:
                    if (IsCluster(argumentType))
                        return ParameterMatchKind.Cluster;
                    break;

                case ParameterTypeKind.NotBool:
                    if (!SymbolsAssignable(argumentType, ScalarTypes.Bool))
                        return ParameterMatchKind.NotType;
                    break;

                case ParameterTypeKind.NotRealOrBool:
                    if (!SymbolsAssignable(argumentType, ScalarTypes.Real)
                        && !SymbolsAssignable(argumentType, ScalarTypes.Bool))
                        return ParameterMatchKind.NotType;
                    break;

                case ParameterTypeKind.NotDynamic:
                    if (!SymbolsAssignable(argumentType, ScalarTypes.Dynamic))
                        return ParameterMatchKind.NotType;
                    break;

                case ParameterTypeKind.Parameter0:
                    return GetParameterMatchKind(signature, argumentParameters, argumentTypes, argumentParameters[0], argument, argumentType, allowLooseParameterMatching);

                case ParameterTypeKind.Parameter1:
                    return GetParameterMatchKind(signature, argumentParameters, argumentTypes, argumentParameters[1], argument, argumentType, allowLooseParameterMatching);

                case ParameterTypeKind.Parameter2:
                    return GetParameterMatchKind(signature, argumentParameters, argumentTypes, argumentParameters[2], argument, argumentType, allowLooseParameterMatching);

                case ParameterTypeKind.CommonScalar:
                case ParameterTypeKind.CommonNumber:
                case ParameterTypeKind.CommonSummable:
                case ParameterTypeKind.CommonOrderable:
                case ParameterTypeKind.CommonScalarOrDynamic:
                    var commonType = GetCommonArgumentType(argumentParameters, argumentTypes);
                    if (commonType != null)
                    {
                        if (SymbolsAssignable(commonType, argumentType, Conversion.None))
                        {
                            return ParameterMatchKind.Exact;
                        }
                        else if (SymbolsAssignable(commonType, argumentType, Conversion.Promotable))
                        {
                            return ParameterMatchKind.Promoted;
                        }
                        else if (allowLooseParameterMatching
                            && SymbolsAssignable(commonType, argumentType, Conversion.Compatible))
                        {
                            return ParameterMatchKind.Compatible;
                        }
                        else if (parameter.TypeKind == ParameterTypeKind.CommonScalarOrDynamic
                                 && SymbolsAssignable(argumentType, ScalarTypes.Dynamic))
                        {
                            return ParameterMatchKind.Exact;
                        }
                    }
                    break;
            }

            return ParameterMatchKind.None;
        }
#endregion

#region FunctionCall and Pattern binding
        private SemanticInfo BindFunctionCallOrPattern(FunctionCallExpression functionCall)
        {
            // the result type of the name should be bound to the function/pattern
            var symbol = GetResultTypeOrError(functionCall.Name);

            if (symbol is FunctionSymbol fn)
            {
                return BindFunctionCall(functionCall, fn);
            }
            else if (symbol is PatternSymbol ps)
            {
                return BindPattern(functionCall, ps);
            }
            else if (!symbol.IsError)
            {
                // the name was not a known function or pattern, but we decided to give it a result type, so let's use it
                return functionCall.Name.GetSemanticInfo();
            }
            else if (IsBestEffortUnionOperand(functionCall))
            {
                return new SemanticInfo(
                    new TableSymbol().WithIsOpen(true),
                    DiagnosticFacts.GetBestEffortUnionOperandNotDefined(functionCall.Name.SimpleName).WithLocation(functionCall));
            }
            else if (IsFuzzyUnionOperand(functionCall))
            {
                return new SemanticInfo(
                    new TableSymbol().WithIsOpen(true),
                    DiagnosticFacts.GetFuzzyUnionOperandNotDefined(functionCall.Name.SimpleName).WithLocation(functionCall));
            }
            else
            {
                return null;
            }
        }

        private static bool IsInvokeOperatorFunctionCall(FunctionCallExpression functionCall)
        {
            return functionCall.Parent is InvokeOperator
                || (functionCall.Parent is PathExpression p && p.Selector == functionCall && p.Parent is InvokeOperator);
        }

        private SemanticInfo BindFunctionCall(FunctionCallExpression functionCall, FunctionSymbol fn)
        {
            var diagnostics = s_diagnosticListPool.AllocateFromPool();
            var arguments = s_expressionListPool.AllocateFromPool();
            var argumentTypes = s_typeListPool.AllocateFromPool();
            var matchingSignatures = s_signatureListPool.AllocateFromPool();

            try
            {
                GetArgumentsAndTypes(functionCall, arguments, argumentTypes);

                GetBestMatchingSignatures(fn.Signatures, arguments, argumentTypes, matchingSignatures);

                if (matchingSignatures.Count == 1)
                {
                    CheckSignature(matchingSignatures[0], arguments, argumentTypes, functionCall.Name, diagnostics);
                    var sigResult = GetSignatureResult(matchingSignatures[0], arguments, argumentTypes, diagnostics);
                    return new SemanticInfo(fn, sigResult.Type, diagnostics, isConstant: fn.IsConstantFoldable && AllAreConstant(arguments), expander: sigResult.Expander);
                }
                else
                {
                    if (arguments.Count == 0 && fn.MinArgumentCount > 0)
                    {
                        diagnostics.Add(DiagnosticFacts.GetFunctionExpectsArgumentCountRange(fn.Name, fn.MinArgumentCount, fn.MaxArgumentCount).WithLocation(functionCall.Name));
                    }
                    else if (!ArgumentsHaveErrorsOrUnknown(argumentTypes))
                    {
                        var types = arguments.Select(e => GetResultTypeOrError(e)).ToList();
                        diagnostics.Add(DiagnosticFacts.GetFunctionNotDefinedWithMatchingParameters(functionCall.Name.SimpleName, types).WithLocation(functionCall.Name));
                    }

                    var returnType = GetCommonReturnType(matchingSignatures, arguments, argumentTypes);

                    return new SemanticInfo(fn, returnType, diagnostics, isConstant: fn.IsConstantFoldable && AllAreConstant(arguments));
                }
            }
            finally
            {
                s_diagnosticListPool.ReturnToPool(diagnostics);
                s_expressionListPool.ReturnToPool(arguments);
                s_typeListPool.ReturnToPool(argumentTypes);
                s_signatureListPool.ReturnToPool(matchingSignatures);
            }
        }

        private SemanticInfo BindPattern(FunctionCallExpression functionCall, PatternSymbol pattern)
        {
            var diagnostics = s_diagnosticListPool.AllocateFromPool();
            var matchingSignatures = s_patternListPool.AllocateFromPool();
            var arguments = s_expressionListPool.AllocateFromPool();
            try
            {
                // check argument count
                if (pattern.Parameters.Count != functionCall.ArgumentList.Expressions.Count)
                {
                    diagnostics.Add(DiagnosticFacts.GetArgumentCountExpected(pattern.Parameters.Count).WithLocation(functionCall.Name));
                }

                // check actual arguments
                for (int i = 0, n = functionCall.ArgumentList.Expressions.Count; i < n; i++)
                {
                    var argument = functionCall.ArgumentList.Expressions[i].Element;
                    arguments.Add(argument);

                    if (i < pattern.Parameters.Count)
                    {
                        var type = pattern.Parameters[i].DeclaredTypes[0];
                        if (CheckIsExactType(argument, type, diagnostics))
                        {
                            CheckIsLiteral(argument, diagnostics);
                        }
                    }
                }

                GetMatchingPatterns(pattern.Signatures, arguments, matchingSignatures);

                if (matchingSignatures.Count == 0)
                {
                    diagnostics.Add(DiagnosticFacts.GetNoPatternMatchesArguments().WithLocation(functionCall.Name));
                    return new SemanticInfo(pattern, ErrorSymbol.Instance, diagnostics);
                }
                else
                {
                    var result = GetReturnType(matchingSignatures);
                    return new SemanticInfo(pattern, result, diagnostics);
                }
            }
            finally
            {
                s_diagnosticListPool.ReturnToPool(diagnostics);
                s_patternListPool.ReturnToPool(matchingSignatures);
                s_expressionListPool.ReturnToPool(arguments);
            }
        }

        /// <summary>
        /// Gets the set of pattern signatures that match the arguments.
        /// </summary>
        private void GetMatchingPatterns(IReadOnlyList<PatternSignature> signatures, IReadOnlyList<Expression> arguments, List<PatternSignature> matchingSignatures)
        {
            // look for exact match
            foreach (var sig in signatures)
            {
                if (PatternMatches(sig, arguments, exact: true))
                {
                    matchingSignatures.Add(sig);
                }
            }

            // if no exact matches, look for partial matches
            if (matchingSignatures.Count == 0)
            {
                foreach (var sig in signatures)
                {
                    if (PatternMatches(sig, arguments, exact: false))
                    {
                        matchingSignatures.Add(sig);
                    }
                }
            }
        }

        /// <summary>
        /// Determines if the pattern signature matches the arguments.
        /// </summary>
        private bool PatternMatches(PatternSignature signature, IReadOnlyList<Expression> arguments, bool exact)
        {
            if (exact && signature.ArgumentValues.Count != arguments.Count)
                return false;

            for (int i = 0; i < arguments.Count; i++)
            {
                if (i < signature.ArgumentValues.Count)
                {
                    string matchValue = signature.ArgumentValues[i];
                    string argValue = arguments[i].LiteralValue?.ToString() ?? "";
                    if (matchValue != argValue)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the return type of the set of pattern signatures.
        /// The return type is either the type of the signature body if there is no path,
        /// or a database symbol containing variables named for each path.
        /// </summary>
        private TypeSymbol GetReturnType(IReadOnlyList<PatternSignature> signatures)
        {
            if (signatures.Count == 1 && signatures[0].PathValue == null)
                return signatures[0].Signature.GetReturnType(_globals);

            var paths = s_symbolListPool.AllocateFromPool();
            var types = s_typeListPool.AllocateFromPool();
            try
            {
                foreach (var sig in signatures)
                {
                    var type = sig.Signature.GetReturnType(_globals);

                    if (sig.PathValue == null)
                    {
                        if (!types.Contains(type))
                        {
                            types.Add(type);
                        }
                    }
                    else
                    {
                        paths.Add(new VariableSymbol(sig.PathValue.ToString(), type));
                    }
                }

                if (paths.Count > 0)
                {
                    if (types.Count > 0)
                    {
                        // this should not happen, but in case it does
                        return new GroupSymbol(paths.Concat(types));
                    }
                    else
                    {
                        return new GroupSymbol(paths);
                    }
                }
                else if (types.Count == 1)
                {
                    return types[0];
                }
                else
                {
                    return new GroupSymbol(types);
                }
            }
            finally
            {
                s_symbolListPool.ReturnToPool(paths);
                s_typeListPool.ReturnToPool(types);
            }
        }

        /// <summary>
        /// Gets the inline expansion of an invocation of this <see cref="Signature"/>.
        /// </summary>
        internal Expansion GetCallSiteExpansion(Signature signature, IReadOnlyList<Expression> arguments = null, IReadOnlyList<TypeSymbol> argumentTypes = null, LocalScope outerScope = null)
        {
            if (signature.ReturnKind != ReturnTypeKind.Computed)
                return null;

            // block cycles in computation
            if (_localBindingCache.SignaturesComputingExpansion.Contains(signature))
                return null;

            _localBindingCache.SignaturesComputingExpansion.Add(signature);
            try
            {
                var callSiteInfo = GetCallSiteInfo(signature, arguments, argumentTypes);

                if (!TryGetExpansionFromCache(callSiteInfo, out var expansion))
                {
                    try
                    {
                        var body = GetUnboundFunctionBody(signature);

                        if (body != null)
                        {
                            var isInDatabase = IsDatabaseSymbolSignature(signature);
                            var currentDatabase = isInDatabase ? _globals.GetDatabase(signature.Symbol) : null;
                            var currentCluster = isInDatabase ? _globals.GetCluster(currentDatabase) : null;
                            expansion = new Expansion(body);

                            if (TryBindExpansion(expansion, this, currentCluster, currentDatabase, signature.Symbol as FunctionSymbol, outerScope, callSiteInfo.Locals))
                            {
                                SetSignatureBindingInfo(signature, expansion.Body);
                            }
                            else
                            {
                                // don't return expansion that did not bind
                                expansion = null;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // don't return expansion that failed in binding
                        expansion = null;
                    }

                    AddExpansionToCache(callSiteInfo, expansion);
                }

                return expansion;
            }
            finally
            {
                _localBindingCache.SignaturesComputingExpansion.Remove(signature);
            }

            // Tries to get the expansion from global or local cache.
            bool TryGetExpansionFromCache(CallSiteInfo callsite, out Expansion expansion)
            {
                return _localBindingCache.CallSiteToExpansionMap.TryGetValue(callsite, out expansion)
                    || _globalBindingCache.CallSiteToExpansionMap.TryGetValue(callsite, out expansion);
            }

            // Adds expansion to global or local cache.
            void AddExpansionToCache(CallSiteInfo callsite, Expansion expansion)
            {
                // if there is a call to unqualified table(t) then it may require resolving using dynamic scope, so don't cache anywhere
                if ((callsite.Signature.FunctionBodyFacts & FunctionBodyFacts.Table) != 0)
                    return;

                // only add database functions that are variable in nature to global cache
                var shouldCacheGlobally = IsDatabaseSymbolSignature(callsite.Signature)
                    && callsite.Signature.FunctionBodyFacts != FunctionBodyFacts.None;

                if (shouldCacheGlobally)
                {
                    _globalBindingCache.CallSiteToExpansionMap.Add(callsite, expansion);
                }
                else
                {
                    _localBindingCache.CallSiteToExpansionMap.Add(callsite, expansion);
                }
            }
        }

        /// <summary>
        /// True if the signature is declared by a symbol that is part of database known to the current <see cref="GlobalState"/>
        /// </summary>
        private bool IsDatabaseSymbolSignature(Signature signature)
        {
            return signature != null
                && signature.Symbol != null
                && signature.Declaration == null   // they don't have syntax trees (yet)
                && signature.Body != null          // they do have a body as text
                && _globals.IsDatabaseSymbol(signature.Symbol);       // and they are known by the global state
        }

        internal void SetSignatureBindingInfo(Signature signature, FunctionBody body)
        {
            if (signature.FunctionBodyFacts == null)
            {
                signature.FunctionBodyFacts = ComputeFunctionBodyFacts(signature, body);
            }

            if (!signature.HasVariableReturnType)
            {
                var returnType = body.Expression?.ResultType ?? ErrorSymbol.Instance;
                signature.NonVariableComputedReturnType = returnType;
            }
        }

        private CallSiteInfo GetCallSiteInfo(Signature signature, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            var locals = GetArgumentsAsLocals(signature, arguments, argumentTypes);
            return new CallSiteInfo(signature, locals);
        }

        private IReadOnlyList<VariableSymbol> GetArgumentsAsLocals(Signature signature, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            var locals = new List<VariableSymbol>();

            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                if (arguments != null)
                {
                    signature.GetArgumentParameters(arguments, argumentParameters);
                }
                else if (argumentTypes != null)
                {
                    signature.GetArgumentParameters(argumentTypes, argumentParameters);
                }

                foreach (var p in signature.Parameters)
                {
                    var argIndex = argumentParameters != null ? argumentParameters.IndexOf(p) : -1;

                    if (argIndex >= 0 && arguments != null & argIndex < arguments.Count)
                    {
                        var arg = arguments[argIndex];

                        var argType = argumentTypes != null && argIndex < argumentTypes.Count 
                            ? argumentTypes[argIndex] 
                            : arg.ResultType;

                        var isLiteral = Binding.Binder.TryGetLiteralValue(arg, out var literalValue);
                        locals.Add(new VariableSymbol(p.Name, argType, isLiteral, literalValue));
                    }
                    else
                    {
                        var type = argIndex >= 0 && argumentTypes != null && argIndex < argumentTypes.Count 
                            ? argumentTypes[argIndex] 
                            : GetRepresentativeType(p);

                        var isConstant = p.IsOptional && p.DefaultValue != null;
                        object constantValue = null;
                        if (isConstant)
                        {
                            TryGetLiteralValue(p.DefaultValue, out constantValue);
                        }

                        locals.Add(new VariableSymbol(p.Name, type, isConstant, constantValue));
                    }
                }

                return locals.ToReadOnly();
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters);
            }
        }

        /// <summary>
        /// Builds an expanded declaration of the function customized given the arguments used at the call site.
        /// </summary>
        private static string GetFunctionBodyText(Signature signature)
        {
            var body = signature.Body.Trim();

            if (!body.StartsWith("{"))
                body = "{" + body;

            if (!body.EndsWith("}"))
                body += "\n}";

            return body;
        }

        private static FunctionBody GetUnboundFunctionBody(Signature signature)
        {
            if (signature.Declaration != null)
            {
                return (FunctionBody)signature.Declaration.Clone();
            }
            else
            {
                var text = GetFunctionBodyText(signature);
                return QueryParser.ParseFunctionBody(text);
            }
        }

        private static TypeSymbol GetRepresentativeType(Parameter parameter)
        {
            switch (parameter.TypeKind)
            {
                case ParameterTypeKind.Declared:
                    return parameter.DeclaredTypes[0];
                case ParameterTypeKind.Tabular:
                    return TableSymbol.Empty;
                default:
                    return ScalarTypes.Dynamic;
            }
        }

        private FunctionBodyFacts ComputeFunctionBodyFacts(Signature signature, FunctionBody body)
        {
            var result = FunctionBodyFacts.None;
            var isTabular = body.Expression?.ResultType is TableSymbol;

            // look for explicit calls to table(), database() or cluster() functions
            foreach (var fc in body.GetDescendants<FunctionCallExpression>(
                _fc => _fc.ReferencedSymbol == Functions.Table 
                    || _fc.ReferencedSymbol == Functions.ExternalTable
                    || _fc.ReferencedSymbol == Functions.MaterializedView
                    || _fc.ReferencedSymbol == Functions.Database 
                    || _fc.ReferencedSymbol == Functions.Cluster))
            {
                if (fc.ReferencedSymbol == Functions.Table)
                {
                    // distinguish between database(d).table(t) vs just table(t)
                    // since table(t) can see variables in dynamic scope
                    if (fc.Parent is PathExpression p && p.Selector == fc)
                    {
                        result |= FunctionBodyFacts.QualifiedTable;
                    }
                    else
                    {
                        result |= FunctionBodyFacts.Table;
                    }
                }
                else if(fc.ReferencedSymbol == Functions.ExternalTable)
                {
                    result |= FunctionBodyFacts.ExternalTable;
                }
                else if (fc.ReferencedSymbol == Functions.MaterializedView)
                {
                    result |= FunctionBodyFacts.MaterializedView;
                }
                else if (fc.ReferencedSymbol == Functions.Database)
                {
                    result |= FunctionBodyFacts.Database;
                }
                else if (fc.ReferencedSymbol == Functions.Cluster)
                {
                    result |= FunctionBodyFacts.Cluster;
                }

                // if the argument is not a literal, then the function likely has a variable return schema
                // note: it might not, but that would require full flow analysis of result type back to inputs.
                var isLiteral = fc.ArgumentList.Expressions.Count > 0 && fc.ArgumentList.Expressions[0].Element.IsLiteral;
                if (!isLiteral && isTabular)
                {
                    result |= FunctionBodyFacts.VariableReturn;
                }
            }

            if (isTabular && signature.Parameters.Any(p => p.IsTabular))
            {
                result |= FunctionBodyFacts.VariableReturn;
            }

            // look for any function calls that themselves that have relevant content
            foreach (var fce in body.GetDescendants<Expression>(fc => fc.ReferencedSymbol is FunctionSymbol))
            {
                var facts = GetFunctionBodyFacts(fce);
                result |= facts;
            }

            return result;
        }

        private FunctionBodyFacts GetFunctionBodyFacts(Expression expr)
        {
            if (expr.ReferencedSymbol is FunctionSymbol fs)
            {
                var signature = fs.Signatures[0];

                if (signature.FunctionBodyFacts == null)
                {
                    if (signature.ReturnKind == ReturnTypeKind.Computed)
                    {
                        if (expr is FunctionCallExpression functionCall)
                        {
                            var arguments = s_expressionListPool.AllocateFromPool();
                            var argumentTypes = s_typeListPool.AllocateFromPool();

                            try
                            {
                                GetArgumentsAndTypes(functionCall, arguments, argumentTypes);
                                GetComputedSignatureResult(signature, arguments, argumentTypes);
                            }
                            finally
                            {
                                s_expressionListPool.ReturnToPool(arguments);
                                s_typeListPool.ReturnToPool(argumentTypes);
                            }
                        }
                        else
                        {
                            GetComputedSignatureResult(signature, EmptyReadOnlyList<Expression>.Instance, EmptyReadOnlyList<TypeSymbol>.Instance);
                        }
                    }
                    else
                    {
                        signature.FunctionBodyFacts = FunctionBodyFacts.None;
                    }
                }

                return signature.FunctionBodyFacts ?? FunctionBodyFacts.None;
            }

            return FunctionBodyFacts.None;
        }
#endregion

#region Declarations
        private void AddLetDeclarationToScope(LocalScope scope, LetStatement statement, List<Diagnostic> diagnostics = null)
        {
            scope.AddSymbol(GetReferencedSymbol(statement.Name));
        }

        private void AddDeclarationsToLocalScope(SyntaxList<SeparatedElement<FunctionParameter>> declarations)
        {
            for (int i = 0, n = declarations.Count; i < n; i++)
            {
                var d = declarations[i].Element;
                AddDeclarationToLocalScope(d.NameAndType.Name);
            }
        }

        private void AddDeclarationsToLocalScope(SyntaxList<SeparatedElement<NameAndTypeDeclaration>> declarations)
        {
            for (int i = 0, n = declarations.Count; i < n; i++)
            {
                var d = declarations[i].Element;
                AddDeclarationToLocalScope(d.Name);
            }
        }

        private void AddDeclarationToLocalScope(SyntaxNode node)
        {
            if (node.ReferencedSymbol is Symbol s)
            {
                _localScope.AddSymbol(s);
            }
        }

        private void BindParameterDeclarations(SyntaxList<SeparatedElement<FunctionParameter>> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                var p = parameters[i].Element;
                BindParameterDeclaration(p);
            }
        }

        private void BindParameterDeclaration(FunctionParameter node)
        {
            BindParameterDeclaration(node.NameAndType);
        }

        private void BindParameterDeclarations(SyntaxList<SeparatedElement<NameAndTypeDeclaration>> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                var p = parameters[i].Element;
                BindParameterDeclaration(p);
            }
        }

        private void BindParameterDeclaration(NameAndTypeDeclaration node)
        {
            var name = node.Name.SimpleName;
            var type = GetTypeFromTypeExpression(node.Type);

            if (!string.IsNullOrEmpty(name))
            {
                var symbol = new ParameterSymbol(name, type);
                SetSemanticInfo(node.Name, new SemanticInfo(symbol, type));
            }
        }

        private void BindColumnDeclarations(SyntaxList<SeparatedElement<FunctionParameter>> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                var p = parameters[i].Element;
                BindColumnDeclaration(p);
            }
        }

        private void BindColumnDeclaration(FunctionParameter node)
        {
            var name = node.NameAndType.Name.SimpleName;
            var type = GetTypeFromTypeExpression(node.NameAndType.Type);

            if (!string.IsNullOrEmpty(name))
            {
                var symbol = new ColumnSymbol(name, type);
                SetSemanticInfo(node.NameAndType.Name, new SemanticInfo(symbol, type));
            }
        }

        private TupleSymbol GetScanStepTuple(ScanOperator node)
        {
            var columns = s_columnListPool.AllocateFromPool();
            try
            {
                GetDeclaredAndInferredColumns(this.RowScopeOrEmpty, columns);

                if (node.DeclareClause != null)
                {
                    foreach (var elem in node.DeclareClause.Declarations)
                    {
                        if (elem.Element.NameAndType.Name.ReferencedSymbol is ColumnSymbol c)
                        {
                            columns.Add(c);
                        }
                    }
                }

                return new TupleSymbol(columns, relatedTable: _rowScope);
            }
            finally
            {
                s_columnListPool.ReturnToPool(columns);
            }
        }

        private void BindStepDeclarations(ScanOperator node)
        {
            var stepTuple = GetScanStepTuple(node);

            foreach (var step in node.Steps)
            {
                var name = step.Name.SimpleName;
                var local = new VariableSymbol(name, stepTuple);
                SetSemanticInfo(step.Name, new SemanticInfo(local, stepTuple));
            }
        }

        private void AddStepDeclarationsToLocalScope(ScanOperator node)
        {
            var stepTuple = GetScanStepTuple(node);

            foreach (var step in node.Steps)
            {
                AddDeclarationToLocalScope(step.Name);
            }
        }

#endregion

#region Tables and Columns
        /// <summary>
        /// Adds all the columns declared by the symbol to the list of columns.
        /// </summary>
        private void AddTableColumns(Symbol symbol, List<ColumnSymbol> columns)
        {
            switch (symbol)
            {
                case TableSymbol t:
                    GetDeclaredAndInferredColumns(t, columns);
                    break;
                case GroupSymbol g:
                    foreach (var s in g.Members)
                    {
                        AddTableColumns(s, columns);
                    }
                    break;
            }
        }

        private void AddTables(Symbol symbol, List<TableSymbol> tables)
        {
            switch (symbol)
            {
                case TableSymbol t:
                    tables.Add(t);
                    break;
                case GroupSymbol g:
                    foreach (var m in g.Members)
                    {
                        AddTables(m, tables);
                    }
                    break;
            }
        }

        private TableSymbol GetFindColumnsTable(FindOperator node)
        {
            var tables = GetFindTables(node);
            return GetTableOfColumnsUnifiedByName(tables);
        }

        /// <summary>
        /// Get the set of tables applicable to the find operator.
        /// </summary>
        private IReadOnlyList<TableSymbol> GetFindTables(FindOperator node)
        {
            if (node.InClause != null)
            {
                return GetReferencedTables(node.InClause.Expressions);
            }
            else
            {
                // no in clause or row scope, so all tables in universe then!
                return GetImpliedTables();
            }
        }

        /// <summary>
        /// Gets the set of columns from the tables applicable to the search operator.
        /// </summary>
        private TableSymbol GetSearchColumnsTable(SearchOperator node)
        {
            if (_rowScope != null && node.InClause == null)
            {
                return _rowScope;
            }
            else
            {
                var tables = GetSearchTables(node);

                // access through cache
                return GetTableOfColumnsUnifiedByNameAndType(tables);
            }
        }

        /// <summary>
        /// Gets the set of tables used by the search operator
        /// </summary>
        private IReadOnlyList<TableSymbol> GetSearchTables(SearchOperator node)
        {
            if (node.InClause != null)
            {
                return GetReferencedTables(node.InClause.Expressions);
            }
            else if (_rowScope != null)
            {
                return new[] { _rowScope };
            }
            else
            {
                // no in clause or row scope, so all tables in universe then!
                return GetImpliedTables();
            }
        }

        /// <summary>
        /// Gets all the tables accessible to the current operator through osmosis,
        /// not from pipe operator or sub clause.
        /// </summary>
        private IReadOnlyList<TableSymbol> GetImpliedTables()
        {
            // include current database's tables and any views in scope
            var declaredViews = s_tableListPool.AllocateFromPool();
            try
            {
                GetViewsInScope(declaredViews);
                if (declaredViews.Count > 0)
                {
                    return _currentDatabase.Tables.Concat(declaredViews).ToList();
                }
                else
                {
                    return _currentDatabase.Tables;
                }
            }
            finally
            {
                s_tableListPool.ReturnToPool(declaredViews);
            }
        }

        /// <summary>
        /// Gets all the declared views in scope
        /// </summary>
        private void GetViewsInScope(List<TableSymbol> views)
        {
            var localSymbols = s_symbolListPool.AllocateFromPool();
            try
            {
                // get all declared tabular functions
                _localScope.GetSymbols(SymbolMatch.Tabular | SymbolMatch.Function, localSymbols);

                // pick out just view function declarations
                foreach (var sym in localSymbols)
                {
                    if (sym is FunctionSymbol fs 
                        && fs.MinArgumentCount == 0)
                    {
                        var decl = fs.Signatures[0].Declaration;
                        if (decl != null && decl.Parent is FunctionDeclaration fd && fd.ViewKeyword != null)
                        {
                            var fts = fs.GetReturnType(_globals) as TableSymbol;
                            if (fts != null)
                            {
                                views.Add(fts);
                            }
                        }
                    }
                }
            }
            finally
            {
                s_symbolListPool.ReturnToPool(localSymbols);
            }
        }

        private IReadOnlyList<TableSymbol> GetReferencedTables(SyntaxList<SeparatedElement<Expression>> list)
        {
            var tables = new List<TableSymbol>();

            foreach (var x in list)
            {
                if (x.Element.ResultType is TableSymbol ts)
                {
                    tables.Add(ts);
                }
                else if (x.Element.ResultType is GroupSymbol gs)
                {
                    tables.AddRange(gs.Members.OfType<TableSymbol>());
                }
            }

            return tables;
        }

        /// <summary>
        /// Converts a list of columns into a list of unique (unioned columns)
        /// Columns with the same name and type will be merged into one column.
        /// Columns with the same name but different type will be renamed to include the type name as a suffix.
        /// </summary>
        internal static void UnifyColumnsWithSameNameAndType(List<ColumnSymbol> columns)
        {
            var uniqueNames = s_uniqueNameTablePool.AllocateFromPool();
            var newColumns = s_columnListPool.AllocateFromPool();
            try
            {
                // TODO: pool this too?
                var map = BuildColumnNameMap(columns);

                // go through original column order and build out new column list
                for (int i = 0; i < columns.Count; i++)
                {
                    var col = columns[i];

                    if (map.TryGetValue(col.Name, out var sameNamedColumns))
                    {
                        if (sameNamedColumns.Count == 1)
                        {
                            // exactly one column with this name
                            newColumns.Add(GetUniqueColumn(col, uniqueNames));
                        }
                        else if (sameNamedColumns.Count > 1)
                        {
                            // more than one column, so lets make a unique column for each type used
                            foreach (var colType in sameNamedColumns)
                            {
                                var name = uniqueNames.GetOrAddName(col.Name + "_" + colType.Name);
                                newColumns.Add(new ColumnSymbol(name, colType));
                            }
                        }

                        // we've already handled this name, remove it so we don't try adding it again
                        map.Remove(col.Name);
                    }
                }

                // copy new list back to original
                columns.Clear();
                columns.AddRange(newColumns);
            }
            finally
            {
                s_uniqueNameTablePool.ReturnToPool(uniqueNames);
                s_columnListPool.ReturnToPool(newColumns);
            }
        }

        /// <summary>
        /// Converts list of columns to a list of columns with distinct names.
        /// If multiple columns have the same name, but differ in type, the resulting single columns has the type dynamic.
        /// </summary>
        /// <param name="columns"></param>
        internal static void UnifyColumnsWithSameName(List<ColumnSymbol> columns)
        {
            var newColumns = s_columnListPool.AllocateFromPool();
            try
            {
                var map = BuildColumnNameMap(columns);

                // go through original column order and build out new column list
                for (int i = 0; i < columns.Count; i++)
                {
                    var col = columns[i];

                    if (map.TryGetValue(col.Name, out var sameNamedColumns))
                    {
                        if (sameNamedColumns.Count == 1)
                        {
                            // exactly one column with this name (and type)
                            newColumns.Add(col);
                        }
                        else if (sameNamedColumns.Count > 1)
                        {
                            // multiple columns with same name, add a single one that uses a common type.
                            var types = sameNamedColumns.ToArray();
                            var commonType = GetCommonScalarType(types);

                            if (commonType == null)
                                commonType = ScalarTypes.Dynamic;

                            if (col.Type == commonType)
                            {
                                newColumns.Add(col);
                            }
                            else
                            {
                                newColumns.Add(new ColumnSymbol(col.Name, commonType));
                            }
                        }

                        // we've already handled this name, so remove it so we don't add it again
                        map.Remove(col.Name);
                    }
                }

                // copy new list back to original
                columns.Clear();
                columns.AddRange(newColumns);
            }
            finally
            {
                s_columnListPool.ReturnToPool(newColumns);
            }
        }

        /// <summary>
        /// Converts a list of columns into a list of unique columns by name.
        /// Columns with the same name will be renamed to include a numeric suffix.
        /// </summary>
        internal static void MakeColumnNamesUnique(List<ColumnSymbol> columns)
        {
            var names = s_uniqueNameTablePool.AllocateFromPool();
            var newColumns = s_columnListPool.AllocateFromPool();
            try
            {
                // go through original column order and build out new column list
                for (int i = 0; i < columns.Count; i++)
                {
                    var col = columns[i];
                    newColumns.Add(GetUniqueColumn(col, names));
                }

                // copy new list back to original
                columns.Clear();
                columns.AddRange(newColumns);
            }
            finally
            {
                s_uniqueNameTablePool.ReturnToPool(names);
                s_columnListPool.ReturnToPool(newColumns);
            }
        }

        /// <summary>
        /// Builds a map between names and columns with that name.
        /// </summary>
        private static Dictionary<string, List<TypeSymbol>> BuildColumnNameMap(List<ColumnSymbol> columns)
        {
            var map = new Dictionary<string, List<TypeSymbol>>();

            // build up a map between column names and types.
            for (int i = 0; i < columns.Count; i++)
            {
                var col = columns[i];
                if (!map.TryGetValue(col.Name, out var sameNameColumns))
                {
                    sameNameColumns = new List<TypeSymbol>();
                    map.Add(col.Name, sameNameColumns);
                }

                if (!sameNameColumns.Contains(col.Type))
                {
                    sameNameColumns.Add(col.Type);
                }
            }

            return map;
        }

        /// <summary>
        /// Gets the columns that appear in both list of columns (by name)
        /// </summary>
        private static void GetCommonColumns(IReadOnlyList<ColumnSymbol> columnsA, IReadOnlyList<ColumnSymbol> columnsB, List<Symbol> result)
        {
            var columns = s_columnListPool.AllocateFromPool();
            try
            {
                GetCommonColumns(columnsA, columnsB, columns);

                foreach (var c in columns)
                {
                    result.Add(c);
                }
            }
            finally
            {
                s_columnListPool.ReturnToPool(columns);
            }
        }

        /// <summary>
        /// Gets the columns that appear in both list of columns (by name)
        /// </summary>
        private static void GetCommonColumns(IReadOnlyList<ColumnSymbol> columnsA, IReadOnlyList<ColumnSymbol> columnsB, List<ColumnSymbol> result)
        {
            var names = s_stringSetPool.AllocateFromPool();
            try
            {
                foreach (var c in columnsB)
                {
                    names.Add(c.Name);
                }

                foreach (var c in columnsA)
                {
                    if (names.Contains(c.Name))
                    {
                        result.Add(c);
                    }
                }
            }
            finally
            {
                s_stringSetPool.ReturnToPool(names);
            }
        }

        /// <summary>
        /// Gets the columns that appear in all tables.
        /// </summary>
        internal static void GetCommonColumns(IReadOnlyList<TableSymbol> tables, List<ColumnSymbol> common)
        {
            common.Clear();

            if (tables.Count == 1)
            {
                common.AddRange(tables[0].Columns);
            }
            else if (tables.Count == 2)
            {
                GetCommonColumns(tables[0].Columns, tables[1].Columns, common);
            }
            else if (tables.Count > 2)
            {
                var columnsA = s_columnListPool.AllocateFromPool();
                var columnsC = s_columnListPool.AllocateFromPool();
                try
                {
                    GetCommonColumns(tables[0].Columns, tables[1].Columns, columnsA);

                    for (int i = 2; i < tables.Count; i++)
                    {
                        GetCommonColumns(columnsA, tables[i].Columns, columnsC);

                        if (i < tables.Count - 1)
                        {
                            columnsA.Clear();
                            columnsA.AddRange(columnsC);
                            columnsC.Clear();
                        }
                    }

                    common.AddRange(columnsC);
                }
                finally
                {
                    s_columnListPool.ReturnToPool(columnsA);
                    s_columnListPool.ReturnToPool(columnsC);
                }
            }
        }

        /// <summary>
        /// Gets a column with a unique name (given a set of already used names).
        /// </summary>
        private static ColumnSymbol GetUniqueColumn(ColumnSymbol column, UniqueNameTable uniqueNames)
        {
            var uniqueName = uniqueNames.GetOrAddName(column.Name);
            if (uniqueName != column.Name)
            {
                return new ColumnSymbol(uniqueName, column.Type);
            }
            else
            {
                return column;
            }
        }

        /// <summary>
        /// Creates column symbols for all the columns declared in the schema.
        /// </summary>
        private static void CreateColumnsFromSchema(SchemaTypeExpression schema, List<ColumnSymbol> columns, HashSet<string> declaredNames, List<Diagnostic> diagnostics)
        {
            for (int i = 0, n = schema.Columns.Count; i < n; i++)
            {
                var expr = schema.Columns[i].Element;
                switch (expr)
                {
                    case NameAndTypeDeclaration nat:
                        CreateColumnsFromSchema(nat, columns, declaredNames, diagnostics);
                        break;

                    case StarExpression s:
                        // not sure what this means here yet.
                        break;
                }
            }
        }

        /// <summary>
        /// Creates a column symbol for the column declared by the <see cref="NameAndTypeDeclaration"/>
        /// </summary>
        private static void CreateColumnsFromSchema(NameAndTypeDeclaration declaration, List<ColumnSymbol> columns, HashSet<string> declaredNames, List<Diagnostic> diagnostics)
        {
            var name = declaration.Name.SimpleName;

            switch (declaration.Type)
            {
                case PrimitiveTypeExpression p:
                    var type = GetType(p); // diagnostics should already have been added
                    if (DeclareColumnName(declaredNames, name, diagnostics, declaration.Name))
                    {
                        columns.Add(new ColumnSymbol(name, type));
                    }
                    break;

                case SchemaTypeExpression s:
                    var subSchemaColumns = s_columnListPool.AllocateFromPool();
                    var subSchemaNames = s_stringSetPool.AllocateFromPool();
                    try
                    {
                        CreateColumnsFromSchema(s, subSchemaColumns, subSchemaNames, diagnostics);

                        if (DeclareColumnName(declaredNames, name, diagnostics, declaration.Name))
                        {
                            columns.Add(new ColumnSymbol(name, new TableSymbol(subSchemaColumns)));
                        }
                    }
                    finally
                    {
                        s_columnListPool.ReturnToPool(subSchemaColumns);
                        s_stringSetPool.ReturnToPool(subSchemaNames);
                    }
                    break;

                default:
                    diagnostics.Add(DiagnosticFacts.GetInvalidColumnDeclaration().WithLocation(declaration));
                    break;
            }
        }

        /// <summary>
        /// Gets the columns referenced by all expressions
        /// </summary>
        private void GetColumnsInColumnList(SyntaxList<SeparatedElement<Expression>> expressions, List<ColumnSymbol> columns, List<Diagnostic> diagnostics)
        {
            foreach (var elem in expressions)
            {
                GetReferencedColumns(elem.Element, columns, diagnostics);
            }
        }

        /// <summary>
        /// Gets the columns referenced by one expression.
        /// </summary>
        private void GetReferencedColumns(Expression expression, List<ColumnSymbol> columns, List<Diagnostic> diagnostics = null)
        {
            var symbol = GetReferencedSymbol(expression);

            switch (symbol)
            {
                case ColumnSymbol c:
                    columns.Add(c);
                    break;
                case GroupSymbol g:
                    foreach (var m in g.Members)
                    {
                        if (m is ColumnSymbol c)
                        {
                            columns.Add(c);
                        }
                    }
                    break;
                default:
                    diagnostics?.Add(DiagnosticFacts.GetColumnExpected().WithLocation(expression));
                    break;
            }
        }

        /// <summary>
        /// Gets all the columns referenced in the syntax tree.
        /// </summary>
        private void GetReferencedColumnsInTree(SyntaxNode node, List<ColumnSymbol> columns)
        {
            foreach (var nr in node.GetDescendantsOrSelf<NameReference>())
            {
                GetReferencedColumns(nr, columns);
            }
        }

        private enum ProjectionStyle
        {
            Default,
            Extend,
            Print,
            Rename,
            Replace,
            Reorder,
            Summarize
        }

        /// <summary>
        /// Creates projection columns for all the expressions.
        /// </summary>
        private void CreateProjectionColumns(
            SyntaxList<SeparatedElement<Expression>> expressions, 
            ProjectionBuilder builder,
            List<Diagnostic> diagnostics,
            ProjectionStyle style = ProjectionStyle.Default,
            bool doNotRepeat = false)
        {
            foreach (var elem in expressions)
            {
                CreateProjectionColumns(
                    elem.Element,
                    builder,
                    diagnostics,
                    style: style,
                    doNotRepeat: doNotRepeat);
            }
        }

        /// <summary>
        /// Creates projection columns for the expression.
        /// </summary>
        private void CreateProjectionColumns(
            Expression expression,
            ProjectionBuilder builder,
            List<Diagnostic> diagnostics,
            ProjectionStyle style = ProjectionStyle.Default,
            bool doNotRepeat = false,
            TypeSymbol columnType = null,
            string columnName = null)
        {
            ColumnSymbol col;
            TypeSymbol type;

            // look through ordered expressions to find column references
            var oe = expression as OrderedExpression;
            if (oe != null)
            {
                expression = oe.Expression;
            }

            if (style == ProjectionStyle.Rename)
            {
                switch (expression)
                {
                    case SimpleNamedExpression n:
                        if (GetReferencedSymbol(n.Expression) is ColumnSymbol cs)
                        {
                            col = builder.Rename(cs.Name, n.Name.SimpleName, diagnostics, n.Name);
                            if (col != null)
                            {
                                SetSemanticInfo(n.Name, CreateSemanticInfo(col));
                            }
                        }
                        else
                        {
                            diagnostics.Add(DiagnosticFacts.GetColumnExpected().WithLocation(n.Expression));
                        }
                        break;

                    default:
                        diagnostics.Add(DiagnosticFacts.GetRenameAssignmentExpected().WithLocation(expression));
                        break;
                }
            }
            else
            {
                switch (expression)
                {
                    case SimpleNamedExpression n:
                        {
                            // single name assigned from multi-value tuple just assigns the first value. equivalant to (name) = tuple
                            if (n.Expression.RawResultType is TupleSymbol tu)
                            {
                                // first column has declared name so it uses declared name add/replace rule
                                col = new ColumnSymbol(n.Name.SimpleName, columnType ?? tu.Columns[0].Type);
                                builder.Declare(col, diagnostics, n.Name, replace: true);
                                SetSemanticInfo(n.Name, CreateSemanticInfo(col));

                                if (doNotRepeat)
                                {
                                    builder.DoNotAdd(tu.Columns[0]);
                                }

                                // don't add unnamed tuple columns if print style
                                if (style == ProjectionStyle.Print)
                                    break;

                                // all other columns are not declared, so they must be unique
                                for (int i = 1; i < tu.Members.Count; i++)
                                {
                                    if (GetReferencedSymbol(n.Expression) is FunctionSymbol fs1)
                                    {
                                        AddFunctionTupleResultColumn(fs1, tu.Columns[i], builder, doNotRepeat, style == ProjectionStyle.Summarize);
                                    }
                                    else
                                    {
                                        builder.Add(tu.Columns[i], doNotRepeat: doNotRepeat);
                                    }
                                }
                            }
                            else if (n.Expression.ReferencedSymbol is ColumnSymbol c)
                            {
                                col = new ColumnSymbol(n.Name.SimpleName, columnType ?? c.Type);
                                builder.Declare(col, diagnostics, n.Name, replace: true);
                                SetSemanticInfo(n.Name, CreateSemanticInfo(col));

                                if (doNotRepeat)
                                {
                                    builder.DoNotAdd(c);
                                }
                            }
                            else
                            {
                                col = new ColumnSymbol(n.Name.SimpleName, columnType ?? GetResultTypeOrError(n.Expression));
                                builder.Declare(col, diagnostics, n.Name, replace: style == ProjectionStyle.Replace || style == ProjectionStyle.Extend);
                                SetSemanticInfo(n.Name, CreateSemanticInfo(col));
                            }
                        }
                        break;

                    case CompoundNamedExpression cn:
                        {
                            if (cn.Expression.RawResultType is TupleSymbol tupleType)
                            {
                                for (int i = 0; i < tupleType.Columns.Count; i++)
                                {
                                    col = tupleType.Columns[i];
                                    type = columnType ?? col.Type;

                                    // if element has name declaration then use name declaration rule
                                    if (i < cn.Names.Names.Count)
                                    {
                                        var nameDecl = cn.Names.Names[i].Element;
                                        var name = nameDecl.SimpleName;
                                        col = new ColumnSymbol(name, type);

                                        builder.Declare(col, diagnostics, nameDecl, replace: style == ProjectionStyle.Replace || style == ProjectionStyle.Extend);
                                        SetSemanticInfo(nameDecl, CreateSemanticInfo(col));

                                        if (doNotRepeat)
                                        {
                                            builder.DoNotAdd(tupleType.Columns[i]);
                                        }
                                    }
                                    else if (style != ProjectionStyle.Print)
                                    {
                                        if (GetReferencedSymbol(cn.Expression) is FunctionSymbol fs1)
                                        {
                                            AddFunctionTupleResultColumn(fs1, col, builder, doNotRepeat, style == ProjectionStyle.Summarize);
                                        }
                                        else
                                        {
                                            // not-declared so make unique column
                                            builder.Add(col, replace: style == ProjectionStyle.Replace || style == ProjectionStyle.Extend, doNotRepeat: doNotRepeat);
                                        }
                                    }
                                }

                                // any additional names without matching tuple members gets a diagnostic
                                for (int i = tupleType.Members.Count; i < cn.Names.Names.Count; i++)
                                {
                                    var nameDecl = cn.Names.Names[i];
                                    diagnostics.Add(DiagnosticFacts.GetTheNameDoesNotHaveCorrespondingExpression().WithLocation(nameDecl));
                                }
                            }
                            else if (cn.Names.Names.Count == 1)
                            {
                                var expr = cn.Expression;
                                var name = cn.Names.Names[0].Element;
                                if (expr.ReferencedSymbol is ColumnSymbol c)
                                {
                                    col = new ColumnSymbol(name.SimpleName, columnType ?? c.Type);
                                    builder.Declare(col, diagnostics, name, replace: true);
                                    SetSemanticInfo(name, CreateSemanticInfo(col));

                                    if (doNotRepeat)
                                    {
                                        builder.DoNotAdd(c);
                                    }
                                }
                                else
                                {
                                    col = new ColumnSymbol(name.SimpleName, columnType ?? GetResultTypeOrError(cn.Expression));
                                    builder.Declare(col, diagnostics, name, replace: style == ProjectionStyle.Replace || style == ProjectionStyle.Extend);
                                    SetSemanticInfo(name, CreateSemanticInfo(col));
                                }
                            }
                            else
                            {
                                diagnostics.Add(DiagnosticFacts.GetTheExpressionDoesNotHaveMultipleValues().WithLocation(cn.Names));
                            }
                        }
                        break;

                    case FunctionCallExpression f:
                        // check for trivial case of no-op conversion operator
                        col = GetResultColumn(f);
                        if (col != null)
                        {
                            // if the expression is a column reference, then consider it a declaration
                            builder.Declare(col.WithType(columnType ?? col.Type), diagnostics, expression, replace: style == ProjectionStyle.Replace);

                            if (doNotRepeat)
                            {
                                builder.DoNotAdd(col);
                            }
                        }
                        else
                        {
                            var ftype = f.RawResultType ?? ErrorSymbol.Instance;
                            var ts = ftype as TupleSymbol;

                            if (style == ProjectionStyle.Print
                                && columnName != null
                                && (ts == null || ts.Columns.Count == 1))
                            {
                                if (ts != null && ts.Columns.Count == 1)
                                    ftype = ts.Columns[0].Type;

                                col = new ColumnSymbol(columnName, columnType ?? ftype);
                                builder.Add(col, columnName, replace: false);
                            }
                            else if (ts != null && GetReferencedSymbol(f) is FunctionSymbol fs)
                            {
                                foreach (ColumnSymbol c in ts.Members)
                                {
                                    AddFunctionTupleResultColumn(fs, c, builder, doNotRepeat, style == ProjectionStyle.Summarize);
                                }
                            }
                            else
                            {
                                var name = GetFunctionResultName(f, null, _rowScope);
                                col = new ColumnSymbol(name ?? columnName ?? "Column1", columnType ?? ftype);
                                builder.Add(col, name ?? "Column", replace: style == ProjectionStyle.Replace || style == ProjectionStyle.Extend);
                            }
                        }
                        break;

                    case StarExpression s:
                        foreach (ColumnSymbol c in GetDeclaredAndInferredColumns(RowScopeOrEmpty))
                        {
                            builder.Add(c, replace: true, doNotRepeat: doNotRepeat);
                        }
                        break;

                    default:
                        var rs = GetReferencedSymbol(expression);
                        col = GetResultColumn(expression);
                        if (col != null)
                        {
                            // if the expression is a column reference, then consider it a declaration
                            builder.Declare(col.WithType(columnType ?? col.Type), diagnostics, expression, replace: style == ProjectionStyle.Replace);

                            if (doNotRepeat)
                            {
                                builder.DoNotAdd(col);
                            }
                        }
                        else if (rs is GroupSymbol group && style == ProjectionStyle.Reorder)
                        {
                            var members = s_symbolListPool.AllocateFromPool();
                            try
                            {
                                if (oe != null && oe.Ordering != null)
                                {
                                    if (oe.Ordering.AscOrDescKeyword.Kind == SyntaxKind.DescKeyword)
                                    {
                                        members.AddRange(group.Members.OrderByDescending(m => m.Name));
                                    }
                                    else
                                    {
                                        members.AddRange(group.Members.OrderBy(m => m.Name));
                                    }
                                }
                                else
                                {
                                    members.AddRange(group.Members);
                                }

                                // add any columns referenced in group
                                foreach (var m in members)
                                {
                                    if (m is ColumnSymbol c)
                                    {
                                        builder.Add(c, doNotRepeat: true);
                                    }
                                }
                            }
                            finally
                            {
                                s_symbolListPool.ReturnToPool(members);
                            }
                        }
                        else if (GetResultType(expression) is GroupSymbol g)
                        {
                            diagnostics.Add(DiagnosticFacts.GetTheExpressionRefersToMoreThanOneColumn().WithLocation(expression));
                        }
                        else
                        {
                            type = GetResultTypeOrError(expression);
                            if (!type.IsError && !type.IsScalar)
                            {
                                diagnostics.Add(DiagnosticFacts.GetScalarTypeExpected().WithLocation(expression));
                                type = ScalarTypes.Unknown;
                            }
                            
                            if (style == ProjectionStyle.Print && columnName != null)
                            {
                                col = new ColumnSymbol(columnName, columnType ?? type);
                                builder.Add(col, columnName, replace: false);
                            }
                            else
                            {
                                var name = GetExpressionResultName(expression, null);
                                col = new ColumnSymbol(name ?? columnName ?? "Column1", columnType ?? type);
                                builder.Add(col, name ?? "Column", replace: style == ProjectionStyle.Replace || style == ProjectionStyle.Extend);
                            }
                        }
                        break;
                }
            }
        }

        public static ColumnSymbol GetResultColumn(Expression expr)
        {
            if (expr == null)
            {
                return null;
            }
            else if (expr.ReferencedSymbol is ColumnSymbol c)
            {
                return c;
            }
            else if (expr is FunctionCallExpression fc
                && IsConversionFunction(fc)
                && fc.ArgumentList.Expressions.Count == 1
                && fc.ArgumentList.Expressions[0].Element.ReferencedSymbol is ColumnSymbol ac
                && fc.ResultType == ac.Type)
            {
                // this is a no-op conversion with column argument, so use argument column as 
                // the column reference for this expression too.
                return ac;
            }
            else
            {
                return null;
            }
        }

        public static bool IsConversionFunction(Expression expr)
        {
            return expr.ReferencedSymbol is FunctionSymbol fs
                && IsConversionFunction(fs);
        }

        public static bool IsConversionFunction(FunctionSymbol fn)
        {
            return fn == Functions.ToBool
                || fn == Functions.ToBool
                || fn == Functions.ToDateTime
                || fn == Functions.ToDecimal
                || fn == Functions.ToDouble
                || fn == Functions.ToDynamic_
                || fn == Functions.ToGuid
                || fn == Functions.ToInt
                || fn == Functions.ToLong
                || fn == Functions.ToReal
                || fn == Functions.ToString
                || fn == Functions.ToTime
                || fn == Functions.ToTimespan;
        }

        private void AddFunctionTupleResultColumn(FunctionSymbol function, ColumnSymbol column, ProjectionBuilder builder, bool doNotRepeat, bool isAggregate)
        {
            //if (builder.CanAdd(column))
            {
                var prefix = function.ResultNamePrefix;

                if (prefix != null)
                {
                    var prefixedColumn = column.WithName(function.ResultNamePrefix + "_" + column.Name);
                    builder.Add(prefixedColumn, doNotRepeat: doNotRepeat);
                }
                else
                {
                    builder.Add(column, doNotRepeat: doNotRepeat);
                }
            }
        }

        private static bool DeclareColumnName(HashSet<string> declaredNames, string newName, List<Diagnostic> diagnostics, SyntaxNode location)
        {
            if (declaredNames.Contains(newName))
            {
                diagnostics.Add(DiagnosticFacts.GetDuplicateColumnDeclaration(newName).WithLocation(location));
                return false;
            }
            else
            {
                declaredNames.Add(newName);
                return true;
            }
        }

        /// <summary>
        /// Gets the name that a function call expression will use as its column name in a projection.
        /// </summary>
        private static string GetFunctionResultName(FunctionCallExpression fc, string defaultName = "", TableSymbol row = null)
        {
            var fs = fc.ReferencedSymbol as FunctionSymbol;
            var kind = fs?.ResultNameKind ?? ResultNameKind.None;
            var prefix = fs?.ResultNamePrefix;

            if (kind == ResultNameKind.NameAndFirstArgument)
            {
                prefix = fs.Name;
                kind = ResultNameKind.PrefixAndFirstArgument;
            }
            else if (kind == ResultNameKind.NameAndOnlyArgument)
            {
                prefix = fs.Name;
                kind = ResultNameKind.PrefixAndOnlyArgument;
            }

            if (kind == ResultNameKind.PrefixAndFirstArgument)
            {
                if (fc.ArgumentList.Expressions.Count > 0)
                {
                    var name = GetExpressionResultName(fc.ArgumentList.Expressions[0].Element, defaultName);
                    if (prefix != null)
                    {
                        return prefix + "_" + name;
                    }
                    else
                    {
                        return name;
                    }
                }
                else if (prefix != null)
                {
                    return prefix + "_";
                }
                else
                {
                    return null;
                }
            }
            else if (kind == ResultNameKind.PrefixAndOnlyArgument 
                && fc.ArgumentList.Expressions.Count == 1)
            {
                var name = GetExpressionResultName(fc.ArgumentList.Expressions[0].Element, defaultName);
                if (prefix != null)
                {
                    return prefix + "_" + name;
                }
                else
                {
                    return name;
                }
            }
            else if (kind == ResultNameKind.FirstArgumentValueIfColumn
                && fc.ArgumentList.Expressions.Count > 0
                && fc.ArgumentList.Expressions[0].Element.ConstantValue is string name)
            {
                if (row != null && row.TryGetColumn(name, out _))
                {
                    return name;
                }
                else
                {
                    return defaultName;
                }
            }
            else if (kind == ResultNameKind.FirstArgument)
            {
                if (fc.ArgumentList.Expressions.Count > 0)
                {
                    return GetExpressionResultName(fc.ArgumentList.Expressions[0].Element, defaultName);
                }
                else
                {
                    return null;
                }
            }
            else if (kind == ResultNameKind.PrefixOnly && prefix != null)
            {
                return prefix;
            }
            else if (kind == ResultNameKind.OnlyArgument && fc.ArgumentList.Expressions.Count == 1)
            {
                return GetExpressionResultName(fc.ArgumentList.Expressions[0].Element, defaultName);
            }
            else
            {
                return defaultName;
            }
        }

        /// <summary>
        /// Gets the name that an expression will use for its column name in a projection.
        /// </summary>
        public static string GetExpressionResultName(Expression expr, string defaultName = "", TableSymbol row = null)
        {
            switch (expr)
            {
                case NameReference n:
                    return n.SimpleName;
                case BracketedExpression be
                    when be.Expression.Kind == SyntaxKind.StringLiteralExpression
                        || be.Expression.Kind == SyntaxKind.CompoundStringLiteralExpression:
                    return (string)be.Expression.LiteralValue;
                case PathExpression p:
                    if (p.Expression.ResultType == ScalarTypes.Dynamic
                        || p.Expression.ResultType == ScalarTypes.Unknown)
                    {
                        var left = GetExpressionResultName(p.Expression, null);
                        var right = GetExpressionResultName(p.Selector, null);
                        return $"{left}_{right}";
                    }
                    else
                    {
                        return GetExpressionResultName(p.Selector, defaultName);
                    }
                case ElementExpression e:
                    if (e.Expression.ResultType == ScalarTypes.Dynamic
                        || e.Expression.ResultType == ScalarTypes.Unknown)
                    {
                        var left = GetExpressionResultName(e.Expression, null);
                        var right = GetExpressionResultName(e.Selector, null);
                        return $"{left}_{right}";
                    }
                    else
                    {
                        return GetExpressionResultName(e.Selector, defaultName);
                    }
                case OrderedExpression o:
                    return GetExpressionResultName(o.Expression, defaultName);
                case SimpleNamedExpression s:
                    return s.Name.SimpleName;
                case FunctionCallExpression f:
                    return GetFunctionResultName(f, defaultName, row);
                default:
                    return defaultName;
            }
        }

        /// <summary>
        /// Gets the declared name of a <see cref="SimpleNamedExpression"/> or null. 
        /// </summary>
        public static string GetExpressionDeclaredName(Expression expr)
        {
            switch (expr)
            {
                case SimpleNamedExpression n:
                    return n.Name.SimpleName;
                case OrderedExpression o:
                    return GetExpressionDeclaredName(o.Expression);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the expression underlying adornments such as name assignment or ordering
        /// </summary>
        public static Expression GetUnderlyingExpression(Expression expression)
        {
            switch (expression)
            {
                case SimpleNamedExpression n:
                    return GetUnderlyingExpression(n.Expression);
                case OrderedExpression o:
                    return GetUnderlyingExpression(o.Expression);
                default:
                    return expression;
            }
        }

        public static string GetNameDeclarationName(Expression expr)
        {
            switch (expr)
            {
                case NameDeclaration nd:
                    return nd.Name.SimpleName;
                case NameReference nr:
                    return nr.Name.SimpleName;
                case LiteralExpression le:
                    if (le.Kind == SyntaxKind.StringLiteralExpression
                        || le.Kind == SyntaxKind.TokenLiteralExpression)
                        return (string)le.LiteralValue;
                    break;
                case CompoundStringLiteralExpression cs:
                    return (string)cs.LiteralValue;
            }

            return null;
        }
#endregion

#region Other
        /// <summary>
        /// Gets the type referenced in the type expression.
        /// </summary>
        private TypeSymbol GetTypeFromTypeExpression(TypeExpression typeExpression, List<Diagnostic> diagnostics = null)
        {
            return GetDeclaredType(typeExpression, diagnostics, this);
        }

        internal static TypeSymbol GetDeclaredType(TypeExpression typeExpression, List<Diagnostic> diagnostics = null, Binder binder = null)
        {
            switch (typeExpression)
            {
                case PrimitiveTypeExpression p:
                    return GetType(p, diagnostics);

                case SchemaTypeExpression s:

                    if (s.Columns.Count == 1 && s.Columns[0].Element is StarExpression)
                    {
                        // (*) was the entire declaration.. no columns specified.
                        return TableSymbol.Empty;
                    }

                    var columns = s_columnListPool.AllocateFromPool();
                    try
                    {
                        for (int i = 0, n = s.Columns.Count; i < n; i++)
                        {
                            var expr = s.Columns[i].Element;
                            if (!expr.IsMissing)
                            {
                                switch (expr)
                                {
                                    case NameAndTypeDeclaration nat:
                                        var declaredType = GetDeclaredType(nat.Type, diagnostics, binder);
                                        var newColumn = new ColumnSymbol(nat.Name.SimpleName, declaredType);
                                        columns.Add(newColumn);

                                        if (binder != null)
                                        {
                                            binder.SetSemanticInfo(nat.Name, GetSemanticInfo(newColumn));
                                        }
                                        break;

                                    default:
                                        if (diagnostics != null)
                                        {
                                            diagnostics.Add(DiagnosticFacts.GetInvalidColumnDeclaration().WithLocation(expr));
                                        }
                                        break;
                                }
                            }
                        }

                        return new TableSymbol(columns);
                    }
                    finally
                    {
                        s_columnListPool.ReturnToPool(columns);
                    }

                default:
                    if (diagnostics != null)
                    {
                        diagnostics.Add(DiagnosticFacts.GetInvalidTypeExpression().WithLocation(typeExpression));
                    }

                    return ErrorSymbol.Instance;
            }
        }

        internal TypeSymbol GetTypeOfType(Expression typeofLiteral)
        {
            return GetReferencedSymbol(typeofLiteral) as TypeSymbol ?? ErrorSymbol.Instance;
        }

        internal static TypeSymbol GetType(PrimitiveTypeExpression primitiveType, List<Diagnostic> diagnostics = null)
        {
            var typeName = primitiveType.Type.Text;

            var type = ScalarTypes.GetSymbol(typeName);

            if (type != null)
                return type;

            if (diagnostics != null) // diagnostic already handled by lexer
            {
                diagnostics.Add(DiagnosticFacts.GetInvalidTypeName(typeName).WithLocation(primitiveType.Type));
            }

            return ErrorSymbol.Instance;
        }

        private static bool IsInteger(TypeSymbol type)
        {
            return type is ScalarSymbol s && s.IsInteger;
        }

        private static bool IsRealOrDecimal(TypeSymbol type)
        {
            return SymbolsAssignable(ScalarTypes.Real, type) || SymbolsAssignable(ScalarTypes.Decimal, type);
        }

        private static bool IsStringOrDynamic(TypeSymbol type)
        {
            return SymbolsAssignable(ScalarTypes.String, type) || SymbolsAssignable(ScalarTypes.Dynamic, type);
        }

        private static bool IsNumber(TypeSymbol type)
        {
            return type is ScalarSymbol s && s.IsNumeric;
        }

        private static bool IsIntegerOrDynamic(TypeSymbol type)
        {
            return IsInteger(type) || SymbolsAssignable(ScalarTypes.Dynamic, type);
        }

        private static bool IsSummable(TypeSymbol type)
        {
            return type is ScalarSymbol s && s.IsSummable;
        }

        public static bool IsOrderable(TypeSymbol type)
        {
            return type is ScalarSymbol s && s.IsOrderable;
        }

        private static bool IsTabular(TypeSymbol type)
        {
            return type != null && type.IsTabular;
        }

        private bool IsTabular(Expression expr)
        {
            return IsTabular(GetResultTypeOrError(expr));
        }

        private bool IsColumn(Expression expr)
        {
            return GetReferencedSymbol(expr) is ColumnSymbol;
        }

        private static bool IsDatabase(Symbol symbol)
        {
            return symbol is DatabaseSymbol;
        }

        private static bool IsCluster(Symbol symbol)
        {
            return symbol is ClusterSymbol;
        }

        private static SemanticInfo GetSemanticInfo(Symbol referencedSymbol, params Diagnostic[] diagnostics)
        {
            return CreateSemanticInfo(referencedSymbol, (IEnumerable<Diagnostic>)diagnostics);
        }

        private static SemanticInfo CreateSemanticInfo(Symbol referencedSymbol, IEnumerable<Diagnostic> diagnostics = null)
        {
            switch (referencedSymbol.Kind)
            {
                case SymbolKind.Operator:
                case SymbolKind.Column:
                case SymbolKind.Table:
                case SymbolKind.Database:
                case SymbolKind.Cluster:
                case SymbolKind.Parameter:
                case SymbolKind.Function:
                case SymbolKind.Pattern:
                case SymbolKind.Group:
                case SymbolKind.MaterializedView:
                    return new SemanticInfo(referencedSymbol, GetResultType(referencedSymbol), diagnostics);
                case SymbolKind.Variable:
                    var v = (VariableSymbol)referencedSymbol;
                    return new SemanticInfo(referencedSymbol, GetResultType(referencedSymbol), diagnostics, isConstant: v.IsConstant);
                case SymbolKind.Scalar:
                case SymbolKind.Tuple:
                    return new SemanticInfo((TypeSymbol)referencedSymbol, diagnostics);
                default:
                    return new SemanticInfo(null, ErrorSymbol.Instance, diagnostics);
            }
        }

        private static TypeSymbol GetResultType(Symbol symbol)
        {
            return Symbol.GetExpressionResultType(symbol);
        }
#endregion

#region Symbol assignability

        public static bool SymbolsAssignable(IReadOnlyList<TypeSymbol> targetTypes, Symbol sourceType, Conversion conversion = Conversion.None)
        {
            for (int i = 0; i < targetTypes.Count; i++)
            {
                if (SymbolsAssignable(targetTypes[i], sourceType, conversion))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// True if a value of type <see cref="P:valueType"/> can be assigned to a parameter of type <see cref="P:parameterType"/>
        /// </summary>
        public static bool SymbolsAssignable(Symbol targetType, Symbol sourceType, Conversion conversion = Conversion.None)
        {
            if (targetType == sourceType)
                return true;

            if (targetType == null || sourceType == null)
                return false;

            if (sourceType == ScalarTypes.Unknown && targetType.IsScalar)
                return true;

            if (targetType == ScalarTypes.Unknown && sourceType.IsScalar)
                return true;

            // a single column tuple is assignable to a scalar
            if (sourceType.Kind == SymbolKind.Tuple 
                && targetType.Kind == SymbolKind.Scalar
                && sourceType is TupleSymbol stt 
                && stt.Columns.Count == 1)
                return SymbolsAssignable(targetType, stt.Columns[0].Type);

            if (targetType.Kind != sourceType.Kind)
                return false;

            switch (targetType.Kind)
            {
                case SymbolKind.Column:
                    var tarCol = (ColumnSymbol)targetType;
                    var srcCol = (ColumnSymbol)sourceType;
                    return tarCol.Name == srcCol.Name && SymbolsAssignable(tarCol.Type, srcCol.Type, conversion);

                case SymbolKind.Tuple:
                case SymbolKind.Group:
                    return MembersEqual(targetType, sourceType);

                case SymbolKind.Table:
                    return TablesAssignable((TableSymbol)targetType, (TableSymbol)sourceType);

                case SymbolKind.Scalar:
                    switch (conversion)
                    {
                        case Conversion.Promotable:
                            return IsPromotable((TypeSymbol)sourceType, (TypeSymbol)targetType);
                        case Conversion.Compatible:
                            return IsPromotable((TypeSymbol)sourceType, (TypeSymbol)targetType)
                                || IsPromotable((TypeSymbol)targetType, (TypeSymbol)sourceType);
                        case Conversion.Any:
                            return true;
                        default:
                            return false;
                    }
            }

            return false;
        }

        public static bool MembersEqual(Symbol target, Symbol source)
        {
            if (target.Members.Count != source.Members.Count)
                return false;

            for (int i = 0, n = target.Members.Count; i < n; i++)
            {
                if (!SymbolsAssignable(target.Members[i], source.Members[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// True if a table value can be assigned to a parameter of a specific table type.
        /// </summary>
        private static bool TablesAssignable(TableSymbol target, TableSymbol source)
        {
            // ensure that the value table has at least the columns specified for the parameter table.

            foreach (var tarCol in target.Columns)
            {
                if (!source.TryGetColumn(tarCol.Name, out var valueColumn)
                    || !SymbolsAssignable(tarCol.Type, valueColumn.Type))
                {
                    return false;
                }
            }

            return true;
        }
#endregion

#region Check methods

        private void CheckQueryOperatorParameters(SyntaxList<NamedParameter> parameters, IReadOnlyList<QueryOperatorParameter> queryParameters, List<Diagnostic> diagnostics)
        {
            var names = s_stringSetPool.AllocateFromPool();
            try
            {
                for (int i = 0, n = parameters.Count; i < n; i++)
                {
                    CheckQueryOperatorParameter(parameters[i], queryParameters, names, diagnostics);
                }
            }
            finally
            {
                s_stringSetPool.ReturnToPool(names);
            }
        }

        private void CheckQueryOperatorParameters(SyntaxList<SeparatedElement<NamedParameter>> parameters, IReadOnlyList<QueryOperatorParameter> queryParameters, List<Diagnostic> diagnostics)
        {
            var names = s_stringSetPool.AllocateFromPool();
            try
            {
                for (int i = 0, n = parameters.Count; i < n; i++)
                {
                    CheckQueryOperatorParameter(parameters[i].Element, queryParameters, names, diagnostics);
                }
            }
            finally
            {
                s_stringSetPool.ReturnToPool(names);
            }
        }

        private void CheckQueryOperatorParameter(NamedParameter parameter, IReadOnlyList<QueryOperatorParameter> queryOperatorParameters, HashSet<string> namesAlreadySpecified, List<Diagnostic> diagnostics)
        {
            var name = parameter.Name.SimpleName;
            if (!string.IsNullOrEmpty(name))
            {
                var qop = GetQueryOperatorParameter(name, queryOperatorParameters);

                if (qop != null)
                {
                    SetSemanticInfo(parameter.Name, new SemanticInfo(qop, ScalarTypes.Unknown));

                    if (!qop.IsRepeatable)
                    {
                        if (namesAlreadySpecified.Contains(qop.Name)
                            || (qop.Aliases.Count > 0 && qop.Aliases.Any(a => namesAlreadySpecified.Contains(a))))
                        {
                            diagnostics.Add(DiagnosticFacts.GetParameterAlreadySpecified(name).WithLocation(parameter.Name));
                        }
                        else
                        {
                            namesAlreadySpecified.Add(qop.Name);
                        }
                    }

                    CheckQueryOperatorParameter(parameter, qop, diagnostics);
                }
                else
                {
                    diagnostics.Add(DiagnosticFacts.GetUnknownQueryOperatorParameterName(name).WithLocation(parameter.Name));
                }
            }
        }

        private void CheckQueryOperatorParameter(NamedParameter parameter, QueryOperatorParameter qop, List<Diagnostic> diagnostics)
        {
            if (!IsQueryOperatorParameterKind(parameter, qop))
            {
                switch (qop.ValueKind)
                {
                    case QueryOperatorParameterValueKind.IntegerLiteral:
                        CheckIsLiteral(parameter.Expression, diagnostics);
                        CheckIsInteger(parameter.Expression, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.NumericLiteral:
                        CheckIsLiteral(parameter.Expression, diagnostics);
                        CheckIsNumber(parameter.Expression, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.ScalarLiteral:
                        CheckIsLiteral(parameter.Expression, diagnostics);
                        CheckIsScalar(parameter.Expression, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.SummableLiteral:
                        CheckIsLiteral(parameter.Expression, diagnostics);
                        CheckIsSummable(parameter.Expression, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.StringLiteral:
                        CheckIsLiteral(parameter.Expression, diagnostics);
                        CheckIsExactType(parameter.Expression, ScalarTypes.String, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.BoolLiteral:
                        CheckIsLiteral(parameter.Expression, diagnostics);
                        CheckIsExactType(parameter.Expression, ScalarTypes.Bool, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.Column:
                        CheckIsColumn(parameter.Expression, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.Word:
                    case QueryOperatorParameterValueKind.WordOrNumber:
                        CheckIsTokenLiteral(parameter.Expression, qop.Values, qop.IsCaseSensitive, diagnostics);
                        break;
                }

                if (qop.ValueKind != QueryOperatorParameterValueKind.Word 
                    && qop.ValueKind != QueryOperatorParameterValueKind.WordOrNumber
                    && qop.Values != null 
                    && qop.Values.Count > 0)
                {
                    CheckIsLiteral(parameter.Expression, diagnostics);
                    CheckLiteralValue(parameter.Expression, qop.Values, qop.IsCaseSensitive, diagnostics);
                }
            }
        }

        private bool IsQueryOperatorParameterKind(NamedParameter parameter, QueryOperatorParameter qop)
        {
            var type = GetResultTypeOrError(parameter.Expression);
            switch (qop.ValueKind)
            {
                case QueryOperatorParameterValueKind.IntegerLiteral:
                    if (!(parameter.Expression.IsLiteral && IsInteger(type)))
                        return false;
                    break;
                case QueryOperatorParameterValueKind.NumericLiteral:
                    if (!(parameter.Expression.IsLiteral && IsNumber(type)))
                        return false;
                    break;
                case QueryOperatorParameterValueKind.ScalarLiteral:
                    if (!(parameter.Expression.IsLiteral && type.IsScalar))
                        return false;
                    break;
                case QueryOperatorParameterValueKind.SummableLiteral:
                    if (!(parameter.Expression.IsLiteral && IsSummable(type)))
                        return false;
                    break;
                case QueryOperatorParameterValueKind.StringLiteral:
                    if (!(parameter.Expression.IsLiteral && IsType(parameter.Expression, ScalarTypes.String)))
                        return false;
                    break;
                case QueryOperatorParameterValueKind.BoolLiteral:
                    if (!(parameter.Expression.IsLiteral && IsType(parameter.Expression, ScalarTypes.Bool)))
                        return false;
                    break;
                case QueryOperatorParameterValueKind.Column:
                    if (!(GetReferencedSymbol(parameter.Expression) is ColumnSymbol))
                        return false;
                    break;
                case QueryOperatorParameterValueKind.Word:
                    if (!IsTokenLiteral(parameter.Expression, qop.Values, qop.IsCaseSensitive))
                        return false;
                    break;
                case QueryOperatorParameterValueKind.WordOrNumber:
                    if (!IsNumber(type) && !IsTokenLiteral(parameter.Expression, qop.Values, qop.IsCaseSensitive))
                        return false;
                    break;
            }

            if (qop.ValueKind != QueryOperatorParameterValueKind.Word 
                && qop.ValueKind != QueryOperatorParameterValueKind.WordOrNumber
                && qop.Values != null
                && qop.Values.Count > 0)
            {
                if (!parameter.Expression.IsLiteral)
                    return false;

                if (!IsLiteralValue(parameter.Expression, qop.Values, qop.IsCaseSensitive))
                    return false;
            }

            return true;
        }

        private static QueryOperatorParameter GetQueryOperatorParameter(string name, IReadOnlyList<QueryOperatorParameter> parameters)
        {
            foreach (var p in parameters)
            {
                if (p.Name == name
                    || (p.Aliases.Count > 0 && p.Aliases.Contains(name)))
                {
                    return p;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks that the data value expressions have the types corresponding to the columns.
        /// </summary>
        private void CheckDataValueTypes(SyntaxList<SeparatedElement<Expression>> expressions, List<ColumnSymbol> columns, List<Diagnostic> diagnostics)
        {
            if (columns.Count > 0 && expressions.Count % columns.Count != 0)
            {
                diagnostics.Add(DiagnosticFacts.GetIncorrectNumberOfDataValues(columns.Count).WithLocation(expressions));
            }

            for (int i = 0, n = expressions.Count; i < n; i++)
            {
                var expr = expressions[i].Element;
                CheckIsScalar(expr, diagnostics);

                // note: data values are convertible at runtime so no check is given
                // consider adding checks for obvious incovertible values
                // var column = columns[i % columns.Count];
                // CheckIsType(expr, column.Type, true, diagnostics);
            }
        }

        private bool CheckIsScalar(Expression expression, List<Diagnostic> diagnostics, Symbol resultType = null)
        {
            if (resultType == null)
                resultType = GetResultType(expression);

            if (resultType != null)
            {
                if (resultType.IsScalar)
                    return true;

                if (!resultType.IsError)
                {
                    diagnostics.Add(DiagnosticFacts.GetScalarTypeExpected().WithLocation(expression));
                }
            }

            return false;
        }

        private bool CheckIsInteger(Expression expression, List<Diagnostic> diagnostics)
        {
            if (IsInteger(GetResultTypeOrError(expression)))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeInteger().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsRealOrDecimal(Expression expression, List<Diagnostic> diagnostics)
        {
            if (IsRealOrDecimal(GetResultTypeOrError(expression)))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeRealOrDecimal().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsIntegerOrDynamic(Expression expression, List<Diagnostic> diagnostics)
        {
            if (IsIntegerOrDynamic(GetResultTypeOrError(expression)))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeIntegerOrDynamic().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsStringOrDynamic(Expression expression, List<Diagnostic> diagnostics)
        {
            if (IsStringOrDynamic(GetResultTypeOrError(expression)))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustHaveType(ScalarTypes.String, ScalarTypes.Dynamic).WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsNumber(Expression expression, List<Diagnostic> diagnostics)
        {
            if (IsNumber(GetResultTypeOrError(expression)))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeNumeric().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsSummable(Expression expression, List<Diagnostic> diagnostics)
        {
            if (IsSummable(GetResultTypeOrError(expression)))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeSummable().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsOrderable(Expression expression, List<Diagnostic> diagnostics)
        {
            if (IsOrderable(GetResultTypeOrError(expression)))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeOrderable().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsExactType(Expression expression, TypeSymbol type, List<Diagnostic> diagnostics)
        {
            return CheckIsType(expression, type, Conversion.None, diagnostics);
        }

        private bool CheckIsTypeOrDynamic(Expression expression, TypeSymbol type, bool canPromote, List<Diagnostic> diagnostics)
        {
            var exprType = GetResultTypeOrError(expression);

            if (SymbolsAssignable(type, exprType, canPromote ? Conversion.Promotable : Conversion.None) 
                || SymbolsAssignable(ScalarTypes.Dynamic, exprType))
                return true;

            if (!exprType.IsError)
            {
                if (SymbolsAssignable(ScalarTypes.Dynamic, type))
                {
                    diagnostics.Add(DiagnosticFacts.GetExpressionMustHaveType(type).WithLocation(expression));
                }
                else
                {
                    diagnostics.Add(DiagnosticFacts.GetExpressionMustHaveType(type, ScalarTypes.Dynamic).WithLocation(expression));
                }
            }

            return false;
        }

        private bool IsType(Expression expression, TypeSymbol type, Conversion conversionKind = Conversion.None)
        {
            var exprType = GetResultTypeOrError(expression);
            return SymbolsAssignable(type, exprType, conversionKind);
        }

        private bool CheckIsType(Expression expression, TypeSymbol type, Conversion conversionKind, List<Diagnostic> diagnostics)
        {
            if (IsType(expression, type, conversionKind))
                return true;

            if (!GetResultTypeOrError(expression).IsError && !type.IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustHaveType(type).WithLocation(expression));
            }

            return false;
        }

        private bool IsAnyType<T>(Expression expression, IReadOnlyList<T> types, Conversion conversionKind = Conversion.None) where T : TypeSymbol
        {
            var exprType = GetResultTypeOrError(expression);

            foreach (var type in types)
            {
                if (SymbolsAssignable(type, exprType, conversionKind))
                    return true;
            }

            return false;
        }

        private bool CheckIsAnyType<T>(Expression expression, IReadOnlyList<T> types, Conversion conversionKind, List<Diagnostic> diagnostics)
            where T : TypeSymbol
        {
            if (IsAnyType(expression, types, conversionKind))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustHaveType(types).WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsNotType(Expression expression, Symbol type, List<Diagnostic> diagnostics)
        {
            var exprType = GetResultTypeOrError(expression);

            if (exprType == ScalarTypes.Unknown)
                return true;

            if (!SymbolsAssignable(type, exprType))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetTypeNotAllowed(type).WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsIntervalType(Expression expression, TypeSymbol rangeType, List<Diagnostic> diagnostics)
        {
            // check to see if add operator is defined between the expression's type and the range type
            var info = GetBinaryOperatorInfo(OperatorKind.Add, expression, rangeType, expression, GetResultTypeOrError(expression), expression);
            if (info.ReferencedSymbol != null && SymbolsAssignable(rangeType, info.ResultType))
                return true;

            if (!rangeType.IsError && !GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetTypeIsNotIntervalType(GetResultTypeOrError(expression), rangeType).WithLocation(expression));
            }

            return false;
        }

        private bool IsLiteralOrName(Expression expression)
        {
            return expression is LiteralExpression ||
                expression is CompoundStringLiteralExpression ||
                expression.Kind == SyntaxKind.DynamicExpression ||
                expression.Kind == SyntaxKind.NameReference;
        }

        private bool CheckIsIdentifierNameDeclaration(NameDeclaration name, List<Diagnostic> diagnostics)
        {
            if (name.Name is TokenName)
                return true;

            diagnostics.Add(DiagnosticFacts.GetIdentifierNameOnly().WithLocation(name));
            return false;
        }

        private bool CheckIsLiteralOrName(Expression expression, List<Diagnostic> diagnostics)
        {
            if (IsLiteralOrName(expression))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeConstantOrIdentifier().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsTabular(Expression expression, List<Diagnostic> diagnostics, Symbol resultType = null)
        {
            resultType = resultType ?? GetResultType(expression);

            if (resultType != null)
            {
                if (resultType.IsTabular)
                    return true;

                if (!resultType.IsError)
                {
                    diagnostics.Add(DiagnosticFacts.GetTableExpected().WithLocation(expression));
                }
            }

            return false;
        }

        private bool CheckIsSingleColumnTable(Expression expression, List<Diagnostic> diagnostics, Symbol resultType = null)
        {
            resultType = resultType ?? GetResultType(expression);

            if (resultType != null)
            {
                var table = resultType as TableSymbol;
                if (table != null && table.Columns.Count == 1)
                    return true;

                if (!resultType.IsError)
                {
                    diagnostics.Add(DiagnosticFacts.GetSingleColumnTableExpected().WithLocation(expression));
                }
            }

            return false;
        }


        private bool CheckIsDatabase(Expression expression, List<Diagnostic> diagnostics)
        {
            if (IsDatabase(GetResultTypeOrError(expression)))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetDatabaseExpected().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsCluster(Expression expression, List<Diagnostic> diagnostics)
        {
            if (IsCluster(GetResultTypeOrError(expression)))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetClusterExpected().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsColumn(Expression expression, List<Diagnostic> diagnostics)
        {
            if (GetReferencedSymbol(expression) is ColumnSymbol)
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetColumnExpected().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsLiteral(Expression expression, List<Diagnostic> diagnostics)
        {
            if (expression.IsLiteral)
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeLiteral().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsLiteralValue(Expression expression, List<Diagnostic> diagnostics)
        {
            if (expression.IsLiteral && expression.Kind != SyntaxKind.TokenLiteralExpression)
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeLiteralScalarValue().WithLocation(expression));
            }

            return false;
        }

        private bool CheckLiteralStringNotEmpty(Expression expression, List<Diagnostic> diagnostics)
        {
            var result = GetResultTypeOrError(expression);
            if (!result.IsError && expression.IsLiteral)
            {
                string value = expression.LiteralValue?.ToString();
                if (!string.IsNullOrEmpty(value))
                    return true;

                diagnostics.Add(DiagnosticFacts.GetExpressionMustNotBeEmpty().WithLocation(expression));
            }

            return false;
        }

        private bool IsTokenLiteral(Expression expression, IReadOnlyList<object> values, bool caseSensitive)
        {
            if (expression.Kind == SyntaxKind.TokenLiteralExpression)
            {
                if (values != null && values.Count > 0)
                {
                    object value = ConvertHelper.ChangeType(expression.LiteralValue, values[0]);
                    return Contains(values, value, caseSensitive);
                }

                return true;
            }

            return false;
        }

        private bool CheckIsTokenLiteral(Expression expression, IReadOnlyList<object> values, bool caseSensitive, List<Diagnostic> diagnostics)
        {
            var result = GetResultTypeOrError(expression);
            if (!result.IsError)
            {
                if (IsTokenLiteral(expression, values, caseSensitive))
                    return true;

                diagnostics.Add(DiagnosticFacts.GetTokenExpected(values.Select(v => v.ToString()).ToList()).WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsToken(SyntaxToken token, IReadOnlyList<object> values, bool caseSensitive, List<Diagnostic> diagnostics)
        {
            var value = ConvertHelper.ChangeType(token.Text, values[0]);
            if (Contains(values, value, caseSensitive))
                return true;

            if (!token.HasSyntaxDiagnostics)
            {
                diagnostics.Add(DiagnosticFacts.GetTokenExpected(values.Select(v => v.ToString()).ToList()).WithLocation(token));
            }

            return false;
        }

        private bool IsLiteralValue(Expression expression, IReadOnlyList<object> values, bool caseSensitive)
        {
            if (!expression.IsLiteral)
                return false;

            object value = ConvertHelper.ChangeType(expression.LiteralValue, values[0]);
            return Contains(values, value, caseSensitive);
        }

        private bool CheckLiteralValue(Expression expression, IReadOnlyList<object> values, bool caseSensitive, List<Diagnostic> diagnostics)
        {
            var result = GetResultTypeOrError(expression);
            if (!result.IsError)
            {
                if (IsLiteralValue(expression, values, caseSensitive))
                    return true;

                diagnostics.Add(DiagnosticFacts.GetExpressionMustHaveValue(values).WithLocation(expression));
            }

            return false;
        }

        private static bool Contains(IReadOnlyList<object> values, object value, bool caseSensitive)
        {
            var stringValue = value as string;
            if (stringValue != null)
            {
                for (int i = 0, n = values.Count; i < n; i++)
                {
                    var v = values[i] as string;
                    if (string.Compare(stringValue, v, ignoreCase: !caseSensitive) == 0)
                        return true;
                }
            }
            else
            {
                for (int i = 0, n = values.Count; i < n; i++)
                {
                    if (values[i] == value)
                        return true;
                }
            }

            return false;
        }

        private bool CheckIsConstant(Expression expression, List<Diagnostic> diagnostics)
        {
            if (GetIsConstant(expression) 
                || expression.ReferencedSymbol is ParameterSymbol) // parameters might be constant
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeConstant().WithLocation(expression));
            }

            return false;
        }

        /// <summary>
        /// True if any argument type is an error type or unknown.
        /// </summary>
        private static bool ArgumentsHaveErrorsOrUnknown(IReadOnlyList<TypeSymbol> argumentTypes)
        {
            for (int i = 0; i < argumentTypes.Count; i++)
            {
                var type = argumentTypes[i];
                if (type.IsError || type == ScalarTypes.Unknown)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks the invocation of a method/operator signature
        /// </summary>
        private void CheckSignature(Signature signature, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes, SyntaxElement location, List<Diagnostic> dx)
        {
            var argCount = arguments.Count;
            int initialDxCount = dx.Count;

            if (!signature.IsValidArgumentCount(argCount))
            {
                if (signature.HasRepeatableParameters)
                {
                    if (argCount < signature.MinArgumentCount || argCount > signature.MaxArgumentCount)
                    {
                        dx.Add(DiagnosticFacts.GetFunctionExpectsArgumentCountRange(signature.Symbol.Name, signature.MinArgumentCount, signature.MaxArgumentCount).WithLocation(location));
                    }
                    else
                    {
                        // not sure how else to say this.. the variable arguments are not specified correctly?
                        dx.Add(DiagnosticFacts.GetFunctionHasIncorrectNumberOfArguments().WithLocation(location));
                    }
                }
                else if (argCount != signature.Parameters.Count)
                {
                    dx.Add(DiagnosticFacts.GetFunctionExpectsArgumentCountExact(signature.Symbol.Name, signature.Parameters.Count).WithLocation(location));
                }
            }

            // check named arguments
            var namedArgumentsAllowed = NamedArgumentsAllowed(signature);
            if (namedArgumentsAllowed && dx.Count == initialDxCount)
            {
                bool hadOutOfOrderNamedArgument = false;
                bool reportedUnnamedArgument = false;

                for (int i = 0; i < argCount; i++)
                {
                    var argument = arguments[i];
                    var simpleNamed = argument as SimpleNamedExpression;
                    var isNamed = simpleNamed != null;
                    var namedParameter = isNamed ? signature.GetParameter(simpleNamed.Name.SimpleName) : null;

                    if (isNamed && namedParameter == null)
                    {
                        dx.Add(DiagnosticFacts.GetUnknownArgumentName().WithLocation(simpleNamed.Name));
                    }

                    if (isNamed && !hadOutOfOrderNamedArgument)
                    {
                        var orderedParameter = i < signature.Parameters.Count ? signature.Parameters[i] : null;
                        hadOutOfOrderNamedArgument = orderedParameter != namedParameter;
                    }
                    else if (!isNamed && hadOutOfOrderNamedArgument && !reportedUnnamedArgument)
                    {
                        dx.Add(DiagnosticFacts.GetUnnamedArgumentAfterOutofOrderNamedArgument().WithLocation(argument));
                        reportedUnnamedArgument = true;
                    }
                }
            }

            var argumentParameters = s_parameterListPool.AllocateFromPool();
            try
            {
                signature.GetArgumentParameters(arguments, argumentParameters);

                // check arguments... 
                if (dx.Count == initialDxCount)
                {
                    for (int i = 0; i < argCount; i++)
                    {
                        CheckArgument(signature, argumentParameters, arguments, argumentTypes, i, dx);
                    }
                }

                // check for missing arguments to non-optional parameters
                if (namedArgumentsAllowed && dx.Count == initialDxCount)
                {
                    foreach (var parameter in signature.Parameters)
                    {
                        if (!parameter.IsOptional)
                        {
                            var iArg = argumentParameters.IndexOf(parameter);
                            if (iArg < 0)
                            {
                                dx.Add(DiagnosticFacts.GetMissingArgumentForParameter(parameter.Name).WithLocation(location));
                            }
                        }
                    }
                }
            }
            finally
            {
                s_parameterListPool.ReturnToPool(argumentParameters);
            }
        }

        private static bool IsNamedArgument(Expression argument)
        {
            return argument is NamedExpression;
        }

        private static SyntaxNode GetNamedArgumentNameNode(Expression argument)
        {
            switch (argument)
            {
                case SimpleNamedExpression sn:
                    return sn.Name;
                case CompoundNamedExpression cn:
                    return cn.Names;
                default:
                    return null;
            }
        }

        /// <summary>
        /// True if named arguments are allowed for this signature.
        /// </summary>
        private bool NamedArgumentsAllowed(Signature signature)
        {
            var fn = signature.Symbol as FunctionSymbol;
            return fn != null && !_globals.IsBuiltInFunction(fn);
        }

        private bool AllowLooseParameterMatching(Signature signature)
        {
            return signature.Symbol is FunctionSymbol fs
                && (_globals.IsDatabaseFunction(fs)
                || fs.Signatures[0].Declaration != null); // user function have declarations
        }

        private void CheckArgument(Signature signature, IReadOnlyList<Parameter> argumentParameters, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes, int argumentIndex, List<Diagnostic> diagnostics)
        {
            var argument = arguments[argumentIndex];
            var argumentType = argumentTypes[argumentIndex];
            var parameter = argumentParameters[argumentIndex];

            if (parameter != null)
            {
                if (argument is StarExpression && signature.Symbol.Kind != SymbolKind.Operator)
                {
                    if (parameter.ArgumentKind != ArgumentKind.Star)
                    {
                        diagnostics.Add(DiagnosticFacts.GetStarExpressionNotAllowed().WithLocation(argument));
                    }
                    else if (argumentIndex < argumentTypes.Count - 1)
                    {
                        diagnostics.Add(DiagnosticFacts.GetStarExpressionMustBeLastArgument().WithLocation(argument));
                    }
                }
                else
                {
                    if (argument is CompoundNamedExpression cn)
                    {
                        diagnostics.Add(DiagnosticFacts.GetCompoundNamedArgumentsNotSupported().WithLocation(cn.Names));
                    }

                    // see through any named argument
                    argument = GetUnderlyingExpression(argument);

                    switch (parameter.TypeKind)
                    {
                        case ParameterTypeKind.Declared:
                            switch (GetParameterMatchKind(signature, argumentParameters, argumentTypes, parameter, argument, argumentType))
                            {
                                case ParameterMatchKind.Compatible:
                                case ParameterMatchKind.None:
                                    if (!AllowLooseParameterMatching(signature))
                                    {
                                        diagnostics.Add(DiagnosticFacts.GetTypeExpected(parameter.DeclaredTypes).WithLocation(argument));
                                    }
                                    break;
                            }
                            break;

                        case ParameterTypeKind.Scalar:
                            CheckIsScalar(argument, diagnostics, argumentType);
                            break;

                        case ParameterTypeKind.Integer:
                            CheckIsInteger(argument, diagnostics);
                            break;

                        case ParameterTypeKind.RealOrDecimal:
                            CheckIsRealOrDecimal(argument, diagnostics);
                            break;

                        case ParameterTypeKind.IntegerOrDynamic:
                            CheckIsIntegerOrDynamic(argument, diagnostics);
                            break;

                        case ParameterTypeKind.StringOrDynamic:
                            CheckIsStringOrDynamic(argument, diagnostics);
                            break;

                        case ParameterTypeKind.Number:
                            CheckIsNumber(argument, diagnostics);
                            break;

                        case ParameterTypeKind.Summable:
                            CheckIsSummable(argument, diagnostics);
                            break;

                        case ParameterTypeKind.Orderable:
                            CheckIsOrderable(argument, diagnostics);
                            break;

                        case ParameterTypeKind.NotBool:
                            if (CheckIsScalar(argument, diagnostics))
                            {
                                CheckIsNotType(argument, ScalarTypes.Bool, diagnostics);
                            }
                            break;

                        case ParameterTypeKind.NotRealOrBool:
                            if (CheckIsScalar(argument, diagnostics))
                            {
                                CheckIsNotType(argument, ScalarTypes.Real, diagnostics);
                                CheckIsNotType(argument, ScalarTypes.Bool, diagnostics);
                            }
                            break;

                        case ParameterTypeKind.NotDynamic:
                            if (CheckIsScalar(argument, diagnostics))
                            {
                                CheckIsNotType(argument, ScalarTypes.Dynamic, diagnostics);
                            }
                            break;

                        case ParameterTypeKind.Tabular:
                            CheckIsTabular(argument, diagnostics, argumentType);
                            break;

                        case ParameterTypeKind.Database:
                            CheckIsDatabase(argument, diagnostics);
                            break;

                        case ParameterTypeKind.Cluster:
                            CheckIsCluster(argument, diagnostics);
                            break;

                        case ParameterTypeKind.Parameter0:
                            CheckIsExactType(argument, argumentTypes[0], diagnostics);
                            break;

                        case ParameterTypeKind.Parameter1:
                            CheckIsExactType(argument, argumentTypes[1], diagnostics);
                            break;

                        case ParameterTypeKind.Parameter2:
                            CheckIsExactType(argument, argumentTypes[2], diagnostics);
                            break;

                        case ParameterTypeKind.CommonScalar:
                            if (CheckIsScalar(argument, diagnostics))
                            {
                                var commonType = GetCommonArgumentType(argumentParameters, argumentTypes);
                                if (commonType != null)
                                {
                                    CheckIsType(argument, commonType, Conversion.Promotable, diagnostics);
                                }
                            }
                            break;

                        case ParameterTypeKind.CommonScalarOrDynamic:
                            if (CheckIsScalar(argument, diagnostics))
                            {
                                var commonType = GetCommonArgumentType(argumentParameters, argumentTypes);
                                if (commonType != null)
                                {
                                    CheckIsTypeOrDynamic(argument, commonType, true, diagnostics);
                                }
                            }
                            break;

                        case ParameterTypeKind.CommonNumber:
                            if (CheckIsNumber(argument, diagnostics))
                            {
                                var commonType = GetCommonArgumentType(argumentParameters, argumentTypes);
                                if (commonType != null)
                                {
                                    CheckIsType(argument, commonType, Conversion.Promotable, diagnostics);
                                }
                            }
                            break;

                        case ParameterTypeKind.CommonSummable:
                            if (CheckIsSummable(argument, diagnostics))
                            {
                                var commonType = GetCommonArgumentType(argumentParameters, argumentTypes);
                                if (commonType != null)
                                {
                                    CheckIsType(argument, commonType, Conversion.Promotable, diagnostics);
                                }
                            }
                            break;

                        case ParameterTypeKind.CommonOrderable:
                            if (CheckIsOrderable(argument, diagnostics))
                            {
                                var commonType = GetCommonArgumentType(argumentParameters, argumentTypes);
                                if (commonType != null)
                                {
                                    CheckIsType(argument, commonType, Conversion.Promotable, diagnostics);
                                }
                            }
                            break;
                    }

                    switch (parameter.ArgumentKind)
                    {
                        case ArgumentKind.Column:
                            CheckIsColumn(argument, diagnostics);
                            break;

                        case ArgumentKind.Constant:
                            CheckIsConstant(argument, diagnostics);
                            break;

                        case ArgumentKind.Literal:
                            if (CheckIsLiteral(argument, diagnostics) && parameter.Values.Count > 0)
                            {
                                CheckLiteralValue(argument, parameter.Values, parameter.IsCaseSensitive, diagnostics);
                            }
                            break;

                        case ArgumentKind.LiteralNotEmpty:
                            if (CheckIsLiteral(argument, diagnostics))
                            {
                                CheckLiteralStringNotEmpty(argument, diagnostics);
                            }
                            break;
                    }
                }
            }
        }

        private static bool CheckArgumentCount(SyntaxList<SeparatedElement<Expression>> expressions, int expectedCount, List<Diagnostic> diagnostics)
        {
            if (expressions.Count == expectedCount)
                return true;

            diagnostics.Add(DiagnosticFacts.GetArgumentCountExpected(expectedCount).WithLocation(expressions));
            return false;
        }

// keep line for BRIDGE.NET bug
#endregion
    }
}