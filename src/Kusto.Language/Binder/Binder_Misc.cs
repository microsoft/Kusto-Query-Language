using System;
using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language.Binding
{
    using Symbols;
    using Syntax;
    using Utils;

    internal sealed partial class Binder
    {
        #region Semantic Info accessors
        private void SetSemanticInfo(SyntaxNode node, SemanticInfo info)
        {
            if (node != null)
            {
                _semanticInfoSetter?.Invoke(node, info);
            }
        }

        private static TypeSymbol GetResultTypeOrError(Expression expression) =>
            expression?.ResultType ?? ErrorSymbol.Instance;

        private static TypeSymbol GetResultType(Expression expression) =>
            expression?.ResultType;

        private static Symbol GetReferencedSymbol(Expression expression) =>
            expression?.ReferencedSymbol;

        private static bool GetIsConstant(Expression expression) =>
            expression?.IsConstant ?? false;
        #endregion

        #region Symbol access/caching

        /// <summary>
        /// The set of open cluster symbols so far.
        /// This set is accumulated as the binder processes the query in lexical order.
        /// </summary>
        private Dictionary<string, ClusterSymbol> _openClusters;

        /// <summary>
        /// Gets or creates an open cluster symbol of the given name.
        /// This is used when a cluster('...') call does not map to a known cluster.
        /// </summary>
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
        /// A map between an open cluster symbol and the set of inferred open databases so far.
        /// This set is accumulated as the binder processes the query in lexical order.
        /// </summary>
        private Dictionary<ClusterSymbol, Dictionary<string, DatabaseSymbol>> _openDatabases;

        /// <summary>
        /// Gets or creates an open database symbol of the given name.
        /// This is primarily used for database('...') of an unknown database within an open cluster.
        /// </summary>
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
        /// A map between an open database symbol and the set of inferred open tables identified so far.
        /// This set is accumulated as the binder processes the query in lexical order.
        /// </summary>
        private Dictionary<DatabaseSymbol, Dictionary<string, TableSymbol>> _openTables;

        /// <summary>
        /// Gets or creates an open table symbol of the given name.
        /// This is primarily used for db.Table or db.table('...') of an unknown table within an open database.
        /// </summary>
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

        /// <summary>
        /// A map between an open table and the set of inferred columns (so far)
        /// This set is accumulated as the binder processes the query in lexical order.
        /// </summary>
        private Dictionary<TableSymbol, Dictionary<string, ColumnSymbol>> _openTableInferredColumns;

        /// <summary>
        /// Gets or creates an inferred column symbol and associates it with the specified open table.
        /// </summary>
        private ColumnSymbol GetOpenTableInferredColumn(string name, TableSymbol table)
        {
            if (_openTableInferredColumns == null)
            {
                _openTableInferredColumns = new Dictionary<TableSymbol, Dictionary<string, ColumnSymbol>>();
            }

            if (!_openTableInferredColumns.TryGetValue(table, out var columnMap))
            {
                columnMap = new Dictionary<string, ColumnSymbol>();
                _openTableInferredColumns.Add(table, columnMap);
            }

            if (!columnMap.TryGetValue(name, out var column))
            {
                column = new ColumnSymbol(name, ScalarTypes.Unknown);
                columnMap.Add(name, column);
            }

            return column;
        }

        /// <summary>
        /// Gets all the declared or inferred columns for the specified table.
        /// </summary>
        private void GetDeclaredAndInferredColumns(TableSymbol table, List<ColumnSymbol> columns)
        {
            columns.AddRange(table.Columns);

            if (table.IsOpen && _openTableInferredColumns != null && _openTableInferredColumns.TryGetValue(table, out var columnMap))
            {
                columns.AddRange(columnMap.Values);
            }
        }

        /// <summary>
        /// Gets all the declared or inferred columns for the specified table.
        /// </summary>
        public IReadOnlyList<ColumnSymbol> GetDeclaredAndInferredColumns(TableSymbol table)
        {
            if (table == null)
            {
                return EmptyReadOnlyList<ColumnSymbol>.Instance;
            }
            else if (table.IsOpen && _openTableInferredColumns != null && _openTableInferredColumns.ContainsKey(table))
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

        /// <summary>
        /// Gets the declared or inferred column of the given name for the specified table.
        /// If the column is not declared and the table is open, a new column is inferred with the given name.
        /// </summary>
        public bool TryGetDeclaredOrInferredColumn(TableSymbol table, string name, out ColumnSymbol column)
        {           
            if (table.TryGetColumn(name, out column))
            {
                return true;
            }
            else if (table.IsOpen)
            {
                column = GetOpenTableInferredColumn(name, table);
                return true;
            }
            else
            {
                column = null;
                return false;
            }
        }

        private Dictionary<TableSymbol, TupleSymbol> _tupleMap;

        /// <summary>
        /// Gets a tuple with the same columns (declared and inferred) as the table.
        /// </summary>
        private TupleSymbol GetTuple(TableSymbol table)
        {
            if (_tupleMap == null)
            {
                _tupleMap = new Dictionary<TableSymbol, TupleSymbol>();
            }

            if (!_tupleMap.TryGetValue(table, out var tuple))
            {
                tuple = new TupleSymbol(table.Columns, table);
                _tupleMap.Add(table, tuple);
            }

            return tuple;
        }

        /// <summary>
        /// Returns true if the list of tables can be cached globally (tied by global state cache)
        /// </summary>
        private bool CanGlobalCache(IReadOnlyList<TableSymbol> tables)
        {
            // if this is the list of tables from the current database definition, then okay.
            // otherwise if its a list of stricly database tables then allow
            //     (note: this still may cause excessive caching if queries have lots of queries with joins,unions of strictly database tables)
            return tables == _currentDatabase.Tables 
                || tables.All(t => _globals.IsDatabaseTable(t));
        }

        /// <summary>
        /// A table that contains all the columns in the specified list of tables, unified on name.
        /// </summary>
        private TableSymbol GetTableOfColumnsUnifiedByName(IReadOnlyList<TableSymbol> tables)
        {
            // consider making this cache thread safe
            if (!_globalBindingCache.UnifiedNameColumnsMap.TryGetValue(tables, out var unifiedColumnsTable))
            {
                var canCache = CanGlobalCache(tables);

                tables = tables.ToReadOnly();
                var columns = new List<ColumnSymbol>();

                foreach (var table in tables)
                {
                    columns.AddRange(table.Columns);
                }

                Binder.UnifyColumnsWithSameName(columns);

                unifiedColumnsTable = new TableSymbol(columns).WithIsOpen(tables.Any(t => t.IsOpen));

                if (canCache)
                {
                    _globalBindingCache.UnifiedNameColumnsMap.AddOrUpdate(tables, unifiedColumnsTable);
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
                var canCache = CanGlobalCache(tables);

                tables = tables.ToReadOnly();
                var columns = new List<ColumnSymbol>();

                foreach (var table in tables)
                {
                    columns.AddRange(table.Columns);
                }

                Binder.UnifyColumnsWithSameNameAndType(columns);

                unifiedColumnsTable = new TableSymbol(columns).WithIsOpen(tables.Any(t => t.IsOpen));

                if (canCache)
                {
                    _globalBindingCache.UnifiedNameAndTypeColumnsMap.AddOrUpdate(tables, unifiedColumnsTable);
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
                var canCache = CanGlobalCache(tables);

                tables = tables.ToReadOnly();
                var columns = new List<ColumnSymbol>();

                Binder.GetCommonColumns(tables, columns);

                commonColumnsTable = new TableSymbol(columns);

                // since these are the common columns, open columns can only exist if all tables are open
                if (tables.Count > 0 && tables.All(t => t.IsOpen))
                {
                    commonColumnsTable = commonColumnsTable.WithIsOpen(true);
                }

                if (canCache)
                {
                    _globalBindingCache.CommonColumnsMap.AddOrUpdate(tables, commonColumnsTable);
                }
            }

            return commonColumnsTable;
        }

        /// <summary>
        /// Gets the <see cref="GraphSymbol"/> from the given <paramref name="node"/>.
        /// </summary>
        private static GraphSymbol GetGraphSymbol(SyntaxNode node)
        {
            // There are two valid configurations:
            //
            // 1. In a regular expression, the graph will be the result of the expression (left-side)
            //    of a pipe expression.
            //
            // 2. In a partitioned make-graph subquery, the node will have a MakeGraphPartitionedByClause
            //    node as one of its parents, which contains the graph symbol.

            var pe = node.Parent as PipeExpression;
            if (pe?.Expression.ResultType is GraphSymbol pegs)
            {
                return pegs;
            }

            var current = node;
            while (current != null && !(current is MakeGraphPartitionedByClause)) {
                current = current.Parent is MakeGraphPartitionedByClause mgpb ? mgpb : current.Parent;
            }

            if (current?.Parent is MakeGraphOperator mg && mg.ResultType is GraphSymbol mggs)
            {
                return mggs;
            }

            return null;
        }
        #endregion

        #region Common definitions
        private static readonly ObjectPool<List<Symbol>> s_symbolListPool =
            new ObjectPool<List<Symbol>>(() => new List<Symbol>(), list => list.Clear());

        private static readonly ObjectPool<HashSet<Symbol>> s_symbolHashSetPool =
            new ObjectPool<HashSet<Symbol>>(() => new HashSet<Symbol>(), list => list.Clear());

        private static readonly ObjectPool<List<Diagnostic>> s_diagnosticListPool =
            new ObjectPool<List<Diagnostic>>(() => new List<Diagnostic>(), list => list.Clear());

        private static readonly ObjectPool<List<ColumnSymbol>> s_columnListPool =
            new ObjectPool<List<ColumnSymbol>>(() => new List<ColumnSymbol>(), list => list.Clear());

        private static readonly ObjectPool<List<TableSymbol>> s_tableListPool =
            new ObjectPool<List<TableSymbol>>(() => new List<TableSymbol>(), list => list.Clear());

        private static readonly ObjectPool<List<FunctionSymbol>> s_functionListPool =
            new ObjectPool<List<FunctionSymbol>>(() => new List<FunctionSymbol>(), list => list.Clear());

        private static readonly ObjectPool<List<Signature>> s_signatureListPool =
            new ObjectPool<List<Signature>>(() => new List<Signature>(), list => list.Clear());

        private static readonly ObjectPool<List<PatternSignature>> s_patternListPool =
            new ObjectPool<List<PatternSignature>>(() => new List<PatternSignature>(), list => list.Clear());

        private static readonly ObjectPool<List<Expression>> s_expressionListPool =
            new ObjectPool<List<Expression>>(() => new List<Expression>(), list => list.Clear());

        private static readonly ObjectPool<List<TypeSymbol>> s_typeListPool =
            new ObjectPool<List<TypeSymbol>>(() => new List<TypeSymbol>(), list => list.Clear());

        private static readonly ObjectPool<List<string>> s_stringListPool =
            new ObjectPool<List<string>>(() => new List<string>(), list => list.Clear());

        private static readonly ObjectPool<HashSet<string>> s_stringSetPool =
            new ObjectPool<HashSet<string>>(() => new HashSet<string>(), s => s.Clear());

        private static readonly ObjectPool<UniqueNameTable> s_uniqueNameTablePool =
            new ObjectPool<UniqueNameTable>(() => new UniqueNameTable(), t => t.Clear());

        private static readonly ObjectPool<ProjectionBuilder> s_projectionBuilderPool =
            new ObjectPool<ProjectionBuilder>(() => new ProjectionBuilder(), b => b.Clear());

        private static readonly ObjectPool<List<Parameter>> s_parameterListPool =
            new ObjectPool<List<Parameter>>(() => new List<Parameter>(), list => list.Clear());

        private static readonly ObjectPool<List<object>> s_objectListPool =
            new ObjectPool<List<object>>(() => new List<object>(), list => list.Clear());

        private static readonly ObjectPool<Dictionary<string, int>> s_stringToIntMapPool =
            new ObjectPool<Dictionary<string, int>>(() => new Dictionary<string, int>(), m => m.Clear());

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
        private static readonly SemanticInfo LiteralNullInfo = new SemanticInfo(ScalarTypes.Null, isConstant: true);
        private static readonly SemanticInfo UnknownInfo = new SemanticInfo(ScalarTypes.Unknown, isConstant: true);
        private static readonly SemanticInfo ErrorInfo = new SemanticInfo(ErrorSymbol.Instance);
        private static readonly SemanticInfo VoidInfo = new SemanticInfo(VoidSymbol.Instance);
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

        private void BindParameterDeclarationsAsVariables(SyntaxList<SeparatedElement<FunctionParameter>> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                var p = parameters[i].Element;
                BindParameterDeclarationAsVariable(p);
            }
        }

        private void BindParameterDeclarationAsVariable(FunctionParameter node)
        {
            var name = node.NameAndType.Name.SimpleName;
            var type = GetTypeFromTypeExpression(node.NameAndType.Type);

            if (!string.IsNullOrEmpty(name))
            {
                var variable = new VariableSymbol(name, type, node.DefaultValue?.Value?.IsConstant ?? false, node.DefaultValue?.Value?.ConstantValueInfo, node.DefaultValue?.Value);
                SetSemanticInfo(node.NameAndType.Name, new SemanticInfo(variable, type));
            }
        }

        private void BindColumnDeclarations(SyntaxList<SeparatedElement<FunctionParameter>> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                var p = parameters[i].Element;
                BindColumnDeclaration(p.NameAndType);
            }
        }

        private void BindColumnDeclarations(SyntaxList<SeparatedElement<NameAndTypeDeclaration>> parameters)
        {
            for (int i = 0; i < parameters.Count; i++)
            {
                var p = parameters[i].Element;
                BindColumnDeclaration(p);
            }
        }

        private void BindColumnDeclaration(NameAndTypeDeclaration node)
        {
            var name = node.Name.SimpleName;
            var type = GetTypeFromTypeExpression(node.Type);

            if (!string.IsNullOrEmpty(name))
            {
                var symbol = new ColumnSymbol(name, type);
                SetSemanticInfo(node.Name, new SemanticInfo(symbol, type));
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

        private void BindGraphMatchPatternDeclarations(SyntaxNode operatorNode, SyntaxList<SeparatedElement<GraphMatchPattern>> patterns)
        {
            var graphScope = GetGraphSymbol(operatorNode);
            var edgeTuple = graphScope != null ? new TupleSymbol(graphScope.EdgeShape.Columns, graphScope.EdgeShape) : TupleSymbol.Empty;
            var nodeTuple = graphScope?.NodeShape != null ? new TupleSymbol(graphScope.NodeShape.Columns, graphScope.NodeShape) : TupleSymbol.Empty;

            foreach (var pattern in patterns)
            {
                foreach (var notation in pattern.Element.PatternElements)
                {
                    if (notation is GraphMatchPatternNode node && node.Name != null)
                    {
                        var local = new VariableSymbol(node.Name.SimpleName, nodeTuple);
                        SetSemanticInfo(node.Name, new SemanticInfo(local, nodeTuple));
                    }
                    else if (notation is GraphMatchPatternEdge edge && edge.Name != null)
                    {
                        var local = new VariableSymbol(edge.Name.SimpleName, edgeTuple);
                        SetSemanticInfo(edge.Name, new SemanticInfo(local, edgeTuple));
                    }
                }
            }
        }

        private void AddGraphMatchPatternDeclarationsToLocalScope(SyntaxList<SeparatedElement<GraphMatchPattern>> patterns)
        {
            foreach (var pattern in patterns)
            {
                foreach (var notation in pattern.Element.PatternElements)
                {
                    if (notation is GraphMatchPatternNode node && node.Name != null)
                    {
                        AddDeclarationToLocalScope(node.Name);
                    }
                    else if (notation is GraphMatchPatternEdge edge && edge.Name != null)
                    {
                        AddDeclarationToLocalScope(edge.Name);
                    }
                }
            }
        }

        #endregion

        #region Directives

        public void ApplyDirective(Directive directive, List<Diagnostic> diagnostics = null)
        {
            switch (directive.Name)
            {
                case "connect":
                    ApplyConnectDirective(directive, directive.Token, diagnostics);
                    break;
                case "database":
                    ApplyDatabaseDirective(directive, directive.Token, diagnostics);
                    break;

                default:
                    if (!KustoFacts.Directives.Contains(directive.Name)
                        && diagnostics != null)
                    {
                        diagnostics.Add(DiagnosticFacts.GetUnknownDirective(directive.Name).WithLocation(directive.Token.TextStart + 1, directive.Name.Length));
                    }
                    break;
            }
        }

        private static char[] databaseSplitChars = new[] { '/' };

        private void ApplyDatabaseDirective(Directive directive, SyntaxElement location, List<Diagnostic> diagnostics)
        {
            if (TryGetDirectiveClusterAndDatabase(directive, out var clusterName, out var databaseName))
            {
                if (clusterName != null)
                {
                    _currentCluster = _globals.GetCluster(clusterName) ?? ClusterSymbol.Unknown;

                    if (_currentCluster == ClusterSymbol.Unknown && diagnostics != null && location != null)
                    {
                        diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownCluster(clusterName).WithSeverity(DiagnosticSeverity.Error).WithLocation(location));
                    }
                }

                _currentDatabase = _currentCluster.GetDatabase(databaseName) ?? DatabaseSymbol.Unknown;

                if (_currentDatabase == DatabaseSymbol.Unknown && diagnostics != null && location != null)
                {
                    diagnostics.Add(DiagnosticFacts.GetNameDoesNotReferToAnyKnownDatabase(databaseName).WithLocation(location));
                }
            }
        }

        internal static bool TryGetDirectiveClusterAndDatabase(
            Directive directive, out string clusterName, out string databaseName)
        {
            clusterName = null;
            databaseName = null;

            if (directive.Name == "database")
            {
                if (directive.Arguments.Count > 1)
                {
                    clusterName = GetDirectiveArgumentStringValue(directive.Arguments[0]);
                    databaseName = GetDirectiveArgumentStringValue(directive.Arguments[1]);
                    return true;
                }
                else if (directive.Arguments.Count == 1)
                {
                    var arg = GetDirectiveArgumentStringValue(directive.Arguments[0]);
                    KustoFacts.GetHostAndPath(arg, out var hostname, out var path);
                    if (hostname != null && path != null)
                    {
                        clusterName = hostname;
                        databaseName = path;
                        return true;
                    }
                    else if (hostname != null)
                    {
                        clusterName = null;
                        databaseName = hostname;
                        return true;
                    }
                }
            }
            else if (directive.Name == "connect" && directive.Arguments.Count > 0)
            {
                var connection = GetDirectiveArgumentStringValue(directive.Arguments[0]);
                var info = ConnectionInfo.Parse(connection);
                var dataSource = info.DataSource;
                KustoFacts.GetHostAndPath(info.DataSource, out var host, out var path);
                clusterName = host;
                databaseName = path;
                return true;
            }

            return false;
        }

        internal static string GetDirectiveArgumentStringValue(Editor.ClientDirectiveArgument argument)
        {
            return argument.Value as string ?? "";
        }

        private void ApplyConnectDirective(Directive directive, SyntaxElement location, List<Diagnostic> diagnostics)
        {
            ApplyDatabaseDirective(directive, location, diagnostics);
        }

        #endregion

        #region Other

        public static bool HasDynamicPrimitives(IReadOnlyList<TypeSymbol> types)
        {
            foreach (var type in types)
            {
                if (type is DynamicPrimitiveSymbol)
                    return true;
            }

            return false;
        }

        public static void GetUnwrappedDynamicPrimitives(IReadOnlyList<TypeSymbol> types, List<TypeSymbol> unwrapped)
        {
            foreach (var type in types)
            {
                unwrapped.Add(
                    type is DynamicPrimitiveSymbol dp
                        ? dp.UnderlyingType
                        : type);
            }
        }

        /// <summary>
        /// Finds the <see cref="EntityGroupElementSymbol"/> associated with the location.
        /// </summary>
        public static EntityGroupElementSymbol GetMacroExpandScope(SyntaxNode location)
        {
            PathExpression path = location as PathExpression;

            if (location is NameReference nr)
            {
                if (nr.Parent is FunctionCallExpression fc
                    && fc.Parent is PathExpression fcpe)
                {
                    path = fcpe;
                }
                else if (nr.Parent is PathExpression pe
                        && pe.Selector == nr)
                {
                    path = pe;
                }
            }

            Symbol symbol = null;

            if (path != null)
            {
                symbol = path.Expression.ReferencedSymbol;
            }

            if (symbol is VariableSymbol vs)
            {
                symbol = vs.Type;
            }

            return symbol as EntityGroupElementSymbol;
        }

        /// <summary>
        /// Gets the <see cref="ScopeKind"/> in effect for all of a function's arguments.
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

        /// <summary>
        /// Gets the <see cref="ScopeKind"/> in effect for a function's specific argument.
        /// </summary>
        private ScopeKind GetArgumentScope(FunctionCallExpression fc, int position, ScopeKind outerScope)
        {
            var fs = fc.Name.ReferencedSymbol as FunctionSymbol;
            var sig = fc.Name.ReferencedSignature;

            if (fs != null)
            {
                if (_globals.IsAggregateFunction(fs))
                {
                    // aggregate function arguments are always normal
                    return ScopeKind.Normal;
                }
                else if ((sig != null && sig.HasAggregateParameters) || (sig == null && HasAggregateParameters(fs)))
                {
                    // if the specific argument may be an aggregate then use aggregate scoping
                    var possibleParameters = s_parameterListPool.AllocateFromPool();
                    GetPossibleArgumentParameters(fc, position, possibleParameters);
                    var anyAggregate = possibleParameters.Any(pp => pp.ArgumentKind == ArgumentKind.Aggregate);
                    s_parameterListPool.ReturnToPool(possibleParameters);
                    if (anyAggregate)
                        return ScopeKind.Aggregate;
                }
            }
            
            if (outerScope == ScopeKind.Aggregate)
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

        private static TableSymbol GetArgumentRowScope(FunctionCallExpression fc, int position, TableSymbol defaultRowScope)
        {
            var fs = fc.Name.ReferencedSymbol as FunctionSymbol;
            var sig = fc.Name.ReferencedSignature;

            if (fs != null)
            {
                var possibleParameters = s_parameterListPool.AllocateFromPool();
                GetPossibleArgumentParameters(fc, position, possibleParameters);

                var anyP0 = possibleParameters.Any(p => p.ArgumentKind == ArgumentKind.Column_Parameter0);
                var anyP0_Common = possibleParameters.Any(p => p.ArgumentKind == ArgumentKind.Column_Parameter0_Common);
                var anyP0_Expression = possibleParameters.Any(p => p.ArgumentKind == ArgumentKind.Expression_Parameter0_Element);

                if (anyP0_Expression
                    && fc.ArgumentList.Expressions.Count > 0
                    && fc.ArgumentList.Expressions[0].Element.ResultType is TypeSymbol argType
                    && argType is TupleSymbol tuple)
                {
                    return new TableSymbol(tuple.Columns);
                }

                if ((anyP0 || anyP0_Common)
                    && fc.ArgumentList.Expressions.Count > 0 
                    && fc.ArgumentList.Expressions[0].Element.ResultType is TableSymbol p0Table)
                {
                    // if both, show all p0Table columns
                    if (anyP0)
                        return p0Table;

                    var rowScope = defaultRowScope ?? TableSymbol.Empty;
                    var commonColumns = new List<ColumnSymbol>();
                    GetCommonColumns(rowScope.Columns, p0Table.Columns, commonColumns);
                    return new TableSymbol(commonColumns);
                }
            }

            return defaultRowScope;
        }

        private static bool HasAggregateParameters(FunctionSymbol fs)
        {
            if (fs.Signatures.Count == 1)
            {
                return fs.Signatures[0].HasAggregateParameters;
            }
            else if (fs.Signatures.Count > 1)
            {
                foreach (var sig in fs.Signatures)
                {
                    if (sig.HasAggregateParameters)
                        return true;
                }
            }

            return false;
        }

        private static void GetPossibleArgumentParameters(
            FunctionCallExpression fc, 
            int position, 
            List<Parameter> possibleParameters)
        {
            var childIndex = GetChildIndex(fc.ArgumentList.Expressions, position);
            var fs = fc.ReferencedSymbol as FunctionSymbol;
            var sig = fc.ReferencedSignature;

            // check for easy lookup named parameter case
            if (sig != null
                && sig.AllowsNamedArguments
                && childIndex >= 0 
                && childIndex <= fc.ArgumentList.Expressions.Count)
            {
                var child = fc.ArgumentList.Expressions[childIndex].Element;
                if (child is SimpleNamedExpression nex)
                {
                    var parameter = sig.GetParameter(nex.Name.SimpleName);
                    possibleParameters.Add(parameter);
                    return;
                }
            }

            // otherwise we need to do a parameter layout to know which to choose
            var arguments = s_expressionListPool.AllocateFromPool();
            try
            {
                // get all arguments
                foreach (var arg in fc.ArgumentList.Expressions)
                {
                    if (arg.Element != null)
                    {
                        arguments.Add(arg.Element);
                    }
                }

                if (sig != null)
                {
                    GetPossibleArgumentParameters(sig, position, arguments, childIndex, possibleParameters);
                }
                else if (fs != null)
                {
                    foreach (var fsig in fs.Signatures)
                    {
                        GetPossibleArgumentParameters(fsig, position, arguments, childIndex, possibleParameters);
                    }
                }
            }
            finally
            {
                s_expressionListPool.ReturnToPool(arguments);
            }
        }

        private static void GetPossibleArgumentParameters(
            Signature sig, 
            int position, 
            List<Expression> arguments, 
            int argumentIndex,
            List<Parameter> possibleParameters)
        {
            // the position is right of the existing arguments
            if (arguments.Count == 0 || position > arguments[arguments.Count - 1].End)
            {
                // drop any trailing missing arguments
                while (arguments.Count > 0 && arguments[arguments.Count - 1].IsMissing)
                {
                    arguments.RemoveAt(arguments.Count - 1);
                }

                sig.GetNextPossibleParameters(arguments, possibleParameters);
            }
            else
            {
                var parameters = s_parameterListPool.AllocateFromPool();
                sig.GetArgumentParameters(arguments, parameters);
                
                if (argumentIndex >= 0 && argumentIndex <= parameters.Count)
                {
                    // we know the signature and are at a specific argument index
                    // so use the argument here
                    possibleParameters.Add(parameters[argumentIndex]);
                }

                s_parameterListPool.ReturnToPool(parameters);
            }
        }

        /// <summary>
        /// Gets the index of the child in the parent or -1 if none found
        /// </summary>
        private static int GetChildIndex(SyntaxElement parent, int position)
        {
            var firstMissingChildIndex = -1;

            // look for existing child that matches position
            for (int i = 0; i < parent.ChildCount; i++)
            {
                var child = parent.GetChild(i);
                if (child != null)
                {
                    if (position >= child.TextStart && position < child.End)
                    {
                        return i;
                    }
                    else if (child.IsMissing && position == child.TextStart)
                    {
                        firstMissingChildIndex = i;
                    }
                }
            }

            return firstMissingChildIndex;
        }

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
                    var cannotBeEmpty = typeExpression.Parent is NameAndTypeDeclaration;
                    if (s.Columns.Count == 0 && cannotBeEmpty && diagnostics != null)
                    {
                        diagnostics.Add(DiagnosticFacts.GetColumnDeclarationExpected().WithLocation(s));
                    }
                    else if (s.Columns.Count == 1 && s.Columns[0].Element is StarExpression)
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

            if (diagnostics != null && !primitiveType.ContainsSyntaxDiagnostics) // diagnostic already handled by lexer
            {
                diagnostics.Add(DiagnosticFacts.GetInvalidTypeName(typeName).WithLocation(primitiveType.Type));
            }

            return ErrorSymbol.Instance;
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
                case SymbolKind.Function:
                case SymbolKind.Pattern:
                case SymbolKind.Group:
                case SymbolKind.MaterializedView:
                case SymbolKind.Graph:
                case SymbolKind.GraphModel:
                case SymbolKind.GraphSnapshot:
                case SymbolKind.EntityGroup:
                case SymbolKind.EntityGroupElement:
                case SymbolKind.StoredQueryResult:
                    return new SemanticInfo(referencedSymbol, GetResultType(referencedSymbol), diagnostics);
                case SymbolKind.Parameter:
                    // parameter is treated as probably constant so we don't raise an error diagnostic
                    return new SemanticInfo(referencedSymbol, GetResultType(referencedSymbol), diagnostics, isConstant: ((ParameterSymbol)referencedSymbol).IsScalar);
                case SymbolKind.Variable:
                    var v = (VariableSymbol)referencedSymbol;
                    return new SemanticInfo(referencedSymbol, GetResultType(referencedSymbol), diagnostics, isConstant: v.IsConstant);
                case SymbolKind.Primitive:
                case SymbolKind.Array:
                case SymbolKind.Bag:
                case SymbolKind.Tuple:
                    return new SemanticInfo((TypeSymbol)referencedSymbol, diagnostics);
                default:
                    return new SemanticInfo(ErrorSymbol.Instance, diagnostics);
            }
        }

        private static TypeSymbol GetResultType(Symbol symbol)
        {
            return Symbol.GetResultType(symbol);
        }
        #endregion

        #region Symbol assignability

        private static bool SymbolsAssignable(IReadOnlyList<TypeSymbol> targetTypes, Symbol sourceType, Conversion allowedConversion = Conversion.None)
        {
            return sourceType.IsAssignableToAny(targetTypes, allowedConversion);
        }

        /// <summary>
        /// True if a value of type <see cref="P:valueType"/> can be assigned to a parameter of type <see cref="P:parameterType"/>
        /// </summary>
        private static bool SymbolsAssignable(Symbol targetType, Symbol sourceType, Conversion allowedConversion = Conversion.None)
        {
            return sourceType.IsAssignableTo(targetType, allowedConversion);
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
                var actualType = GetResultTypeOrError(parameter.Expression);

                switch (qop.ValueKind)
                {
                    case QueryOperatorParameterValueKind.IntegerLiteral:
                        CheckIsIntegerLiteral(parameter.Expression, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.NumericLiteral:
                    case QueryOperatorParameterValueKind.ForcedRealLiteral:
                        CheckIsNumericLiteral(parameter.Expression, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.ScalarLiteral:
                        CheckIsScalarLiteral(parameter.Expression, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.SummableLiteral:
                        CheckIsSummableLiteral(parameter.Expression, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.StringLiteral:
                        CheckIsStringLiteral(parameter.Expression, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.BoolLiteral:
                        CheckIsBooleanlLiteral(parameter.Expression, diagnostics);
                        break;
                    case QueryOperatorParameterValueKind.String:
                        CheckIsStringOrDynamic(parameter.Expression, diagnostics);
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
                    CheckIsLiteralValue(parameter.Expression, qop.Values, qop.IsCaseSensitive, diagnostics);
                }
            }
        }

        private bool IsQueryOperatorParameterKind(NamedParameter parameter, QueryOperatorParameter qop)
        {
            var type = GetResultTypeOrError(parameter.Expression);
            switch (qop.ValueKind)
            {
                case QueryOperatorParameterValueKind.IntegerLiteral:
                    if (!(parameter.Expression.IsLiteral && type.IsInteger()))
                        return false;
                    break;
                case QueryOperatorParameterValueKind.NumericLiteral:
                case QueryOperatorParameterValueKind.ForcedRealLiteral:
                    if (!(parameter.Expression.IsLiteral && type.IsNumeric()))
                        return false;
                    break;
                case QueryOperatorParameterValueKind.ScalarLiteral:
                    if (!(parameter.Expression.IsLiteral && type.IsScalar))
                        return false;
                    break;
                case QueryOperatorParameterValueKind.SummableLiteral:
                    if (!(parameter.Expression.IsLiteral && type.IsSummable()))
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
                case QueryOperatorParameterValueKind.String:
                    if (!IsType(parameter.Expression, ScalarTypes.String))
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
                    if (!type.IsNumeric() && !IsTokenLiteral(parameter.Expression, qop.Values, qop.IsCaseSensitive))
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

            // we don't know anything
            if (resultType == null)
                return true;

            if (resultType.IsScalar)
                return true;

            if (!resultType.IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetScalarTypeExpected().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsScalar(TypeSymbol type, SyntaxElement location, List<Diagnostic> diagnostics)
        {
            if (type.IsScalar)
                return true;

            if (!type.IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetScalarTypeExpected().WithLocation(location));
            }

            return false;
        }

        private bool CheckIsScalar(SyntaxList<SeparatedElement<Expression>> list, List<Diagnostic> diagnostics)
        {
            return CheckAll(list, diagnostics, (expr, dx) => CheckIsScalar(expr, dx));
        }

        private bool CheckIsScalasr(IReadOnlyList<ColumnSymbol> columns, SyntaxElement location, List<Diagnostic> diagnostics)
        {
            return CheckAll(columns, location, diagnostics, (col, loc, dx) => CheckIsScalar(col.Type, loc, dx));
        }

        private bool CheckIsInteger(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type.IsInteger())
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeInteger().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsIntegerLiteral(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type.IsInteger() && expression.IsLiteral)
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetIntegerLiteralExpected().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsStringLiteral(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type == ScalarTypes.String && expression.IsLiteral)
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetStringLiteralExpected().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsBooleanlLiteral(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type == ScalarTypes.Bool && expression.IsLiteral)
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetBooleanLiteralExpected().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsSummableLiteral(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type is ScalarSymbol s && s.IsSummable && expression.IsLiteral)
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetSummableLiteralExpected().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsNumericLiteral(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type is ScalarSymbol s && s.IsNumeric && expression.IsLiteral)
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetSummableLiteralExpected().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsScalarLiteral(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type is ScalarSymbol s && expression.IsLiteral)
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetScalarLiteralExpected().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsRealOrDecimal(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type.IsRealOrDecimal())
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeRealOrDecimal().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsIntegerOrArray(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type.IsIntegerOrArray())
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeIntegerOrArray().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsStringOrDynamic(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (TypeFacts.IsStringOrDynamic(type))
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustHaveType(ScalarTypes.String, ScalarTypes.Dynamic).WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsStringOrArray(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (TypeFacts.IsStringOrArray(type))
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeStringOrArray().WithLocation(expression));
            }

            return false;
        }

        /// <summary>
        /// Checks if the expression is any dynamic type.
        /// </summary>
        private bool CheckIsDynamic(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type is DynamicSymbol)
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustHaveType(ScalarTypes.Dynamic).WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsDynamicArray(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (TypeFacts.IsDynamicArray(type))
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeDynamicArray().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsDynamicBag(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (TypeFacts.IsDynamicBag(type))
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeDynamicBag().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsNumber(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type.IsNumeric())
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeNumeric().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsNumberOrBool(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type.IsNumeric() || type == ScalarTypes.Bool)
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeNumericOrBool().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsSummable(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (type.IsSummable())
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeSummable().WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsOrderable(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (TypeFacts.IsOrderable(type))
                return true;

            if (!type.IsError && type != ScalarTypes.Unknown)
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
                || SymbolsAssignable(ScalarTypes.Dynamic, exprType, Conversion.Dynamic))
                return true;

            if (!exprType.IsError && exprType != ScalarTypes.Unknown)
            {
                if (SymbolsAssignable(ScalarTypes.Dynamic, type, Conversion.Dynamic))
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

        private bool IsType(Expression expression, TypeSymbol expectedType, Conversion conversionKind = Conversion.None)
        {
            return IsType(GetResultTypeOrError(expression), expectedType, conversionKind);
        }

        private bool IsType(Symbol type, Symbol expectedType, Conversion conversionKind = Conversion.None)
        {
            if (type == ScalarTypes.Unknown)
            {
                // we don't know that it not the type
                return true;
            }
            else if (expectedType == ScalarTypes.Dynamic)
            {
                return type is DynamicSymbol;
            }
            else
            {
                return SymbolsAssignable(expectedType, type, conversionKind);
            }
        }

        private bool CheckIsType(Expression expression, TypeSymbol type, Conversion conversionKind, List<Diagnostic> diagnostics)
        {
            if (IsType(expression, type, conversionKind))
                return true;

            var exprType = GetResultTypeOrError(expression);
            if (!exprType.IsError && !type.IsError && exprType != ScalarTypes.Unknown && type != ScalarTypes.Unknown)
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

            var exprType = GetResultTypeOrError(expression);
            if (!exprType.IsError && exprType != ScalarTypes.Unknown)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustHaveType(types).WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsNotType(Expression expression, Symbol expectedType, List<Diagnostic> diagnostics)
        {
            var exprType = GetResultTypeOrError(expression);

            if (exprType == ScalarTypes.Unknown)
                return true;

            if (expectedType == ScalarTypes.Dynamic
                && !(exprType is DynamicSymbol))
                return true;

            if (!SymbolsAssignable(expectedType, exprType))
                return true;

            // avoid additional errors
            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetTypeNotAllowed(expectedType).WithLocation(expression));
            }

            return false;
        }

        private bool CheckIsNotType(Symbol type, Symbol notExpectedType, SyntaxElement location, List<Diagnostic> diagnostics)
        {
            // we don't know that its the unexpected type.
            if (type == ScalarTypes.Unknown)
                return true;

            if (!IsType(type, notExpectedType))
                return true;

            diagnostics.Add(DiagnosticFacts.GetTypeNotAllowed(type).WithLocation(location));
            return false;
        }

        private bool CheckAll(SyntaxList<SeparatedElement<Expression>> expressions, List<Diagnostic> diagnostics, Func<Expression, List<Diagnostic>, bool> fnCheck)
        {
            for (int i = 0; i < expressions.Count; i++)
            {
                var expr = expressions[i].Element;
                if (!fnCheck(expr, diagnostics))
                    return false;
            }

            return true;
        }

        private bool CheckAll(IReadOnlyList<ColumnSymbol> columns, SyntaxElement location, List<Diagnostic> diagnostics, Func<ColumnSymbol, SyntaxElement, List<Diagnostic>, bool> fnCheck)
        {
            foreach (var col in columns)
            {
                if (!fnCheck(col, location, diagnostics))
                    return false;
            }

            return true;
        }

        private bool CheckIsNotDynamic(SyntaxList<SeparatedElement<Expression>> expressions, List<Diagnostic> diagnostics)
        {
            return CheckAll(expressions, diagnostics, (expr, dx) => CheckIsNotType(expr, ScalarTypes.Dynamic, dx));
        }

        private bool CheckIsNotDynamic(IReadOnlyList<ColumnSymbol> columns, SyntaxElement location, List<Diagnostic> diagnostics)
        {
            return CheckAll(columns, location, diagnostics, (col, loc, dx) => CheckIsNotType(col.Type, ScalarTypes.Dynamic, loc, diagnostics));
        }

        private bool CheckIsIntervalType(Expression expression, TypeSymbol rangeType, List<Diagnostic> diagnostics)
        {
            // check to see if add operator is defined between the expression's type and the range type
            var info = GetBinaryOperatorInfo(OperatorKind.Add, expression, rangeType, expression, GetResultTypeOrError(expression), expression);
            if (info.ReferencedSymbol != null && SymbolsAssignable(rangeType, info.ResultType))
                return true;

            var exprType = GetResultTypeOrError(expression);
            if (!rangeType.IsError && !exprType.IsError && rangeType != ScalarTypes.Unknown && exprType != ScalarTypes.Unknown)
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

            var exprType = GetResultTypeOrError(expression);
            if (!exprType.IsError)
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
                    diagnostics.Add(DiagnosticFacts.GetTabularValueExpected().WithLocation(expression));
                }
            }

            return false;
        }

        private bool CheckIsGraph(Expression expression, List<Diagnostic> diagnostics, Symbol resultType = null)
        {
            resultType = resultType ?? GetResultType(expression);

            if (resultType != null)
            {
                if (resultType is GraphSymbol)
                    return true;

                if (!resultType.IsError)
                {
                    diagnostics.Add(DiagnosticFacts.GetGraphExpected().WithLocation(expression));
                }
            }

            return false;
        }

        private bool CheckIsTabularOrGraph(Expression expression, List<Diagnostic> diagnostics, Symbol resultType = null)
        {
            resultType = resultType ?? GetResultType(expression);
            if (resultType != null)
            {
                if (resultType.IsTabular || resultType is GraphSymbol)
                {
                    return true;
                }

                if (!resultType.IsError)
                {
                    diagnostics.Add(DiagnosticFacts.GetTableOrGraphExpected().WithLocation(expression));
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

        /// <summary>
        /// Check if the expression is a literal.
        /// </summary>
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

        /// <summary>
        /// Check if the expression is a scalar literal, but not a token literal.
        /// </summary>
        private bool CheckIsLiteralNotToken(Expression expression, List<Diagnostic> diagnostics)
        {
            if (expression.IsLiteral && expression.Kind != SyntaxKind.TokenLiteralExpression)
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeLiteralScalarValue().WithLocation(expression));
            }

            return false;
        }

        /// <summary>
        /// Check if the expression is a non-empty literal string.
        /// </summary>
        private bool CheckIsLiteralStringNotEmpty(Expression expression, List<Diagnostic> diagnostics)
        {
            var exprType = GetResultTypeOrError(expression);
            if (!exprType.IsError && exprType != ScalarTypes.Unknown && expression.IsLiteral)
            {
                string value = expression.LiteralValue?.ToString();
                if (!string.IsNullOrEmpty(value))
                    return true;

                diagnostics.Add(DiagnosticFacts.GetExpressionMustNotBeEmpty().WithLocation(expression));
            }

            return false;
        }

        /// <summary>
        /// Return true if the expression a literal that matches one of the values.
        /// </summary>
        private bool IsLiteralValue(Expression expression, IReadOnlyList<object> values, bool caseSensitive)
        {
            if (!expression.IsLiteral)
                return false;

            return Contains(values, expression.LiteralValue, caseSensitive);
        }

        /// <summary>
        /// Checks if the expression is a literal that matches one of the listed values.
        /// </summary>
        private bool CheckIsLiteralValue(Expression expression, IReadOnlyList<object> values, bool caseSensitive, List<Diagnostic> diagnostics)
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

        /// <summary>
        /// Returns true if the expression is a token literal with one of the listed values.
        /// </summary>
        private bool IsTokenLiteral(Expression expression, IReadOnlyList<object> values, bool caseSensitive)
        {
            if (expression.Kind == SyntaxKind.TokenLiteralExpression)
            {
                if (values != null && values.Count > 0)
                {
                    return Contains(values, expression.LiteralValue, caseSensitive);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the expression is a token literal that matches one of the listed values.
        /// </summary>
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

        /// <summary>
        /// Checks if the token has one of the listed text values.
        /// </summary>
        private bool CheckIsToken(SyntaxToken token, IReadOnlyList<object> values, bool caseSensitive, List<Diagnostic> diagnostics)
        {
            if (Contains(values, token.Text, caseSensitive))
                return true;

            if (!token.HasSyntaxDiagnostics)
            {
                diagnostics.Add(DiagnosticFacts.GetTokenExpected(values.Select(v => v.ToString()).ToList()).WithLocation(token));
            }

            return false;
        }

        /// <summary>
        /// Returns true if the value in the list of values.
        /// </summary>
        private static bool Contains(IReadOnlyList<object> values, object value, bool caseSensitive)
        {
            for (int i = 0, n = values.Count; i < n; i++)
            {
                if (ValueComparer.AreEquivalent(values[i], value, caseSensitive))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the expression is a constant.
        /// </summary>
        private bool CheckIsConstant(Expression expression, List<Diagnostic> diagnostics)
        {
            if (GetIsConstant(expression))
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeConstant().WithLocation(expression));
            }

            return false;
        }

        /// <summary>
        /// Returns true if the expression is a constant that matches one of the listed values.
        /// </summary>
        private static bool IsConstantValue(Expression expression, IReadOnlyList<object> values, bool caseSensitive)
        {
            if (!expression.IsConstant)
                return false;

            return Contains(values, expression.ConstantValue, caseSensitive);
        }

        /// <summary>
        /// Returns true if the expression is either not constant or does not match any of the listed values.
        /// </summary>
        private bool IsNotConstantValue(Expression expression, IReadOnlyList<object> values, bool caseSensitive)
        {
            if (!expression.IsConstant)
                return true;

            return !Contains(values, expression.ConstantValue, caseSensitive);
        }

        /// <summary>
        /// Checks if the expression is a constant that matches one of the listed values.
        /// </summary>
        private bool CheckIsConstantValue(Expression expression, IReadOnlyList<object> values, bool caseSensitive, List<Diagnostic> diagnostics)
        {
            var result = GetResultTypeOrError(expression);
            if (!result.IsError)
            {
                if (IsConstantValue(expression, values, caseSensitive))
                    return true;

                diagnostics.Add(DiagnosticFacts.GetExpressionMustHaveValue(values).WithLocation(expression));
            }

            return false;
        }

        /// <summary>
        /// Checks if the expression is a constant that matches the specified value.
        /// </summary>
        private bool CheckIsConstantValue(Expression expression, object value, bool caseSensitive, List<Diagnostic> diagnostics)
        {
            return CheckIsConstantValue(expression, new[] { value }, caseSensitive, diagnostics);
        }

        /// <summary>
        /// Checks if the expression is either not constant does not match any of the listed values.
        /// </summary>
        private bool CheckIsNotConstantValue(Expression expression, IReadOnlyList<object> values, bool caseSensitive, List<Diagnostic> diagnostics)
        {
            var result = GetResultTypeOrError(expression);
            if (!result.IsError)
            {
                if (IsNotConstantValue(expression, values, caseSensitive))
                    return true;

                diagnostics.Add(DiagnosticFacts.GetExpressionMustNotHaveValue(values).WithLocation(expression));
            }

            return false;
        }

        /// <summary>
        /// Checks if the expression is either not constant does not match the specified value.
        /// </summary>
        private bool CheckIsNotConstantValue(Expression expression, object value, bool caseSensitive, List<Diagnostic> diagnostics)
        {
            return CheckIsNotConstantValue(expression, new[] { value }, caseSensitive, diagnostics);
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

                if (dx.Count == initialDxCount)
                {
                    // check if a common parameter type is required and 
                    // if it can be determined from the arguments.
                    CheckCommonArgumentTypes(signature, argumentParameters, arguments, argumentTypes, location, dx);
                }

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

        private void CheckCommonArgumentTypes(Signature signature, IReadOnlyList<Parameter> argumentParameters, IReadOnlyList<Expression> arguments, IReadOnlyList<TypeSymbol> argumentTypes, SyntaxElement location, List<Diagnostic> dx)
        {
            for (int i = 0; i < argumentParameters.Count; i++)
            {
                var p = argumentParameters[i];

                switch (p.TypeKind)
                {
                    case ParameterTypeKind.CommonNumber:
                    case ParameterTypeKind.CommonSummable:
                    case ParameterTypeKind.CommonOrderable:
                    case ParameterTypeKind.CommonScalar:
                    case ParameterTypeKind.CommonScalarOrDynamic:
                        var commonType = TypeFacts.GetCommonArgumentType(argumentParameters, argumentTypes);
                        if (commonType == null)
                        {
                            dx.Add(DiagnosticFacts.GetNoCommonArgumentType().WithLocation(location));
                            return;
                        }
                        break;
                }
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

        /// <summary>
        /// True if the signature allows implicit argument coercion.
        /// Most built-in function require arguments to explicitly match types of parameters.
        /// User functions, however, allow implicit coercion of scalar arguments.
        /// </summary>
        private bool AllowImplicitArgumentCoercion(Signature signature)
        {
            // check to see if function is user function.
            return signature.Symbol is FunctionSymbol fs
                && (_globals.IsDatabaseFunction(fs)  // user function stored in database
                    || fs.Signatures[0].Declaration != null); // local user function have declarations (built-in functions do not).
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
                    if (parameter.ArgumentKind != ArgumentKind.StarOnly
                        && parameter.ArgumentKind != ArgumentKind.StarAllowed)
                    {
                        diagnostics.Add(DiagnosticFacts.GetStarExpressionNotAllowed().WithLocation(argument));
                    }
                }
                else if (IsDefaultValueIndicator(parameter, argument))
                {
                    // do nothing, this is a legal value for this parameter
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
                        case ParameterTypeKind.Any:
                            // do no checks
                            break;

                        case ParameterTypeKind.Declared:
                            if (parameter.DeclaredTypes.Count == 1 
                                && parameter.DeclaredTypes[0] is TableSymbol tablePattern)
                            {
                                if (argumentType is TableSymbol argumentTable)
                                {
                                    var compatible = tablePattern.Columns.All(c => argumentTable.TryGetColumn(c.Name, out var ac) && SymbolsAssignable(c.Type, ac.Type, Conversion.None));
                                    if (!compatible)
                                    {
                                        diagnostics.Add(DiagnosticFacts.GetTabularValueDoesNotHaveRequiredColumns().WithLocation(argument));
                                    }
                                }
                                else
                                {
                                    diagnostics.Add(DiagnosticFacts.GetTabularValueExpected().WithLocation(argument));
                                }
                            }
                            else
                            {
                                switch (GetParameterMatchKind(signature, argumentParameters, argumentTypes, parameter, argument, argumentType))
                                {
                                    case ParameterMatchKind.Compatible:
                                    case ParameterMatchKind.None:
                                        if (!AllowImplicitArgumentCoercion(signature))
                                        {
                                            diagnostics.Add(DiagnosticFacts.GetTypeExpected(parameter.DeclaredTypes).WithLocation(argument));
                                        }
                                        break;
                                }
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

                        case ParameterTypeKind.IntegerOrArray:
                            CheckIsIntegerOrArray(argument, diagnostics);
                            break;

                        case ParameterTypeKind.StringOrDynamic:
                            CheckIsStringOrDynamic(argument, diagnostics);
                            break;

                        case ParameterTypeKind.StringOrArray:
                            CheckIsStringOrArray(argument, diagnostics);
                            break;

                        case ParameterTypeKind.DynamicArray:
                            CheckIsDynamicArray(argument, diagnostics);
                            break;

                        case ParameterTypeKind.DynamicBag:
                            CheckIsDynamicBag(argument, diagnostics);
                            break;

                        case ParameterTypeKind.Number:
                            CheckIsNumber(argument, diagnostics);
                            break;

                        case ParameterTypeKind.NumberOrBool:
                            CheckIsNumberOrBool(argument, diagnostics);
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
                                var commonType = TypeFacts.GetCommonArgumentType(argumentParameters, argumentTypes);
                                if (commonType != null)
                                {
                                    CheckIsType(argument, commonType, Conversion.Promotable, diagnostics);
                                }
                            }
                            break;

                        case ParameterTypeKind.CommonScalarOrDynamic:
                            if (CheckIsScalar(argument, diagnostics))
                            {
                                var commonType = TypeFacts.GetCommonArgumentType(argumentParameters, argumentTypes);
                                if (commonType != null)
                                {
                                    CheckIsTypeOrDynamic(argument, commonType, true, diagnostics);
                                }
                            }
                            break;

                        case ParameterTypeKind.CommonNumber:
                            if (CheckIsNumber(argument, diagnostics))
                            {
                                var commonType = TypeFacts.GetCommonArgumentType(argumentParameters, argumentTypes);
                                if (commonType != null)
                                {
                                    CheckIsType(argument, commonType, Conversion.Promotable, diagnostics);
                                }
                            }
                            break;

                        case ParameterTypeKind.CommonSummable:
                            if (CheckIsSummable(argument, diagnostics))
                            {
                                var commonType = TypeFacts.GetCommonArgumentType(argumentParameters, argumentTypes);
                                if (commonType != null)
                                {
                                    CheckIsType(argument, commonType, Conversion.Promotable, diagnostics);
                                }
                            }
                            break;

                        case ParameterTypeKind.CommonOrderable:
                            if (CheckIsOrderable(argument, diagnostics))
                            {
                                var commonType = TypeFacts.GetCommonArgumentType(argumentParameters, argumentTypes);
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
                        case ArgumentKind.Column_Parameter0:
                        case ArgumentKind.Column_Parameter0_Common:
                            CheckIsColumn(argument, diagnostics);
                            break;

                        case ArgumentKind.Constant:
                            CheckIsConstant(argument, diagnostics);
                            break;

                        case ArgumentKind.Literal:
                            if (CheckIsLiteral(argument, diagnostics) && parameter.Values.Count > 0)
                            {
                                CheckIsLiteralValue(argument, parameter.Values, parameter.IsCaseSensitive, diagnostics);
                            }
                            break;

                        case ArgumentKind.LiteralNotEmpty:
                            if (CheckIsLiteral(argument, diagnostics))
                            {
                                CheckIsLiteralStringNotEmpty(argument, diagnostics);
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