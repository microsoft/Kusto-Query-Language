using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kusto.Language.Binding
{
    using Kusto.Language;
    using Parsing;
    using Symbols;
    using Syntax;
    using Utils;

    /// <summary>
    /// The binder performs general semantic analysis of the syntax tree, 
    /// identifying the symbols corresponding to named references, 
    /// the return types of operations and generating error diagnostics.
    /// </summary>
    internal sealed partial class Binder
    {
        /// <summary>
        /// Global state including symbols declared in ambient database.
        /// </summary>
        private readonly GlobalState _globals;

        /// <summary>
        /// Keeps track of the number of dynamic nodes in the current traversal.
        /// Used to check if the current node is a child of a dynamic node.
        /// </summary>
        private int _dynamicDepth = 0;

        /// <summary>
        /// The cluster assumed when resolveing unqualified calls to database() 
        /// </summary>
        private readonly ClusterSymbol _currentCluster;

        /// <summary>
        /// The database assumed when resolving unqualified references table/function names or calls to table()
        /// </summary>
        private readonly DatabaseSymbol _currentDatabase;

        /// <summary>
        /// The function being declared.
        /// </summary>
        private readonly FunctionSymbol _currentFunction;

        /// <summary>
        /// All symbol declared locally within the query appear in the local scope.
        /// These are symbols declared by let statements or the as query operator.
        /// Local scopes may be nested within other local scopes.
        /// </summary>
        private LocalScope _localScope;

        /// <summary>
        /// Columns accessible in piped query operators, or from the $left variable in a join on clause.
        /// </summary>
        private TableSymbol _rowScope;

        /// <summary>
        /// Columns accessible from right side of join operator via the $right variable
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
        /// Binding state that is private to one binding (including analysis of called function bodies)
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
        /// Do semantic analysis over the body of a called function.
        /// </summary>
        public static bool TryBindCalledFunctionBody(
            SyntaxTree bodyTree,
            Binder outer,
            ClusterSymbol currentCluster,
            DatabaseSymbol currentDatabase,
            FunctionSymbol currentFunction,
            LocalScope outerScope,
            IEnumerable<Symbol> locals)
        {
            if (!bodyTree.IsSafeToRecurse)
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
            bodyTree.Root.Accept(treeBinder);

            return true;
        }

        /// <summary>
        /// Entry point for <see cref="FunctionBodyFacts"/> to access the cache.
        /// This is used for testing.
        /// </summary>
        public static bool TryGetDatabaseFunctionBodyFacts(FunctionSymbol symbol, GlobalState globals, out FunctionBodyFacts facts)
        {
            if (globals.Cache != null)
            {
                var bindingCache = globals.Cache.GetOrCreate<GlobalBindingCache>();
                lock (bindingCache)
                {
                    return bindingCache.DatabaseFunctionBodyFacts.TryGetValue(symbol.Signatures[0], out facts);
                }
            }

            facts = null;
            return false;
        }

        /// <summary>
        /// Adds the symbols to the current local scope.
        /// </summary>
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

                return binder.GetComputedFunctionCallResult(signature, null, argumentTypes).Type;
            }
        }

        /// <summary>
        /// Gets the symbol that would be referenced at the specified location.
        /// </summary>
        public static Symbol GetReferencedSymbol(SyntaxTree tree, int position, string name, GlobalState globals, SymbolMatch match, CancellationToken cancellationToken)
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
                    if (startNode != null)
                    {
                        binder.SetContext(startNode, position);
                        var info = binder.BindName(name, match, startNode);
                        return info?.ReferencedSymbol;
                    }
                }
            }

            return null;
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
                    if (startNode != null)
                    {
                        binder.SetContext(startNode, position);
                        return binder._rowScope;
                    }
                }
            }

            return TableSymbol.Empty;
        }

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
                    if (startNode != null)
                    {
                        binder.SetContext(startNode, position);
                        binder.GetSymbolsInContext(startNode, match, include, list);
                    }
                }
            }
        }

        private static SyntaxNode GetStartNode(SyntaxNode root, int position)
        {
            var token = root.GetTokenAt(position);

            if (token != null)
            {
                if (position <= token.TextStart)
                {
                    var prev = token.GetPreviousToken();
                    if (prev != null && prev.Depth >= token.Depth)
                    {
                        return prev.Parent;
                    }
                }

                return token.Parent;
            }

            return null;
        }

        private void GetSymbolsInContext(SyntaxNode contextNode, SymbolMatch match, IncludeFunctionKind include, List<Symbol> list)
        {
            if (_pathScope is GroupSymbol g
                && IsPassThrough(g))
            {
                var savePathScope = _pathScope;
                foreach (var s in g.Members)
                {
                    _pathScope = s;
                    GetSymbolsInContext(contextNode, match, include, list);
                }
                _pathScope = savePathScope;
            }
            else if (_pathScope != null)
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
                        GetPathMembers(_pathScope, memberMatch, list);
                    }
                }
                else if (memberMatch != 0)
                {
                    GetPathMembers(_pathScope, memberMatch, list);
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

        private static void GetPathMembers(Symbol target, SymbolMatch memberMatch, List<Symbol> result)
        {
            if (target is GroupSymbol g && IsPassThrough(g))
            {
                foreach (var s in g.Members)
                {
                    GetPathMembers(s, memberMatch, result);
                }
            }
            else
            {
                target.GetMembers(memberMatch, result);
            }
        }


        private static void GetPathMembers(Symbol target, string name, SymbolMatch memberMatch, List<Symbol> result)
        {
            if (target is GroupSymbol g && IsPassThrough(g))
            {
                foreach (var s in g.Members)
                {
                    GetPathMembers(s, name, memberMatch, result);
                }
            }
            else
            {
                target.GetMembers(name, memberMatch, result);
            }
        }

        private void GetSpecialFunctions(string name, List<Symbol> functions)
        {
            if (_pathScope != null)
            {
                if (_pathScope is GroupSymbol g
                    && g.Members.Count > 0
                    && IsPassThrough(g))
                {
                    // use info for first symbol in group
                    var savePathScope = _pathScope;
                    _pathScope = g.Members[0];
                    GetSpecialFunctions(name, functions);
                    _pathScope = savePathScope;
                }
                else
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
    }
}