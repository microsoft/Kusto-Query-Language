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
            if (table.IsOpen && _openTableInferredColumns != null && _openTableInferredColumns.ContainsKey(table))
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
                    _globalBindingCache.CommonColumnsMap[tables] = commonColumnsTable;
                }
            }

            return commonColumnsTable;
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

        private static readonly ObjectPool<HashSet<string>> s_stringSetPool =
            new ObjectPool<HashSet<string>>(() => new HashSet<string>(), s => s.Clear());

        private static readonly ObjectPool<UniqueNameTable> s_uniqueNameTablePool =
            new ObjectPool<UniqueNameTable>(() => new UniqueNameTable(), t => t.Clear());

        private static readonly ObjectPool<ProjectionBuilder> s_projectionBuilderPool =
            new ObjectPool<ProjectionBuilder>(() => new ProjectionBuilder(), b => b.Clear());

        private static readonly ObjectPool<List<Parameter>> s_parameterListPool =
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

        private void BindGraphMatchPatternDeclarations(GraphMatchOperator graphMatch)
        {
            var graphScope = graphMatch.Parent is PipeExpression pe && pe.Expression.ResultType is GraphSymbol gs ? gs : null;
            var edgeTuple = graphScope != null ? new TupleSymbol(graphScope.EdgeShape.Columns, graphScope.EdgeShape) : TupleSymbol.Empty;
            var nodeTuple = graphScope?.NodeShape != null ? new TupleSymbol(graphScope.NodeShape.Columns, graphScope.NodeShape) : TupleSymbol.Empty;

            foreach (var notation in graphMatch.Pattern)
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

        private void AddGraphMatchPatternDeclarationsToLocalScope(GraphMatchOperator graphMatch)
        {
            foreach (var notation in graphMatch.Pattern)
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

        #endregion

        #region Other

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
                case SymbolKind.Graph:
                    return new SemanticInfo(referencedSymbol, GetResultType(referencedSymbol), diagnostics);
                case SymbolKind.Variable:
                    var v = (VariableSymbol)referencedSymbol;
                    return new SemanticInfo(referencedSymbol, GetResultType(referencedSymbol), diagnostics, isConstant: v.IsConstant);
                case SymbolKind.Scalar:
                case SymbolKind.Tuple:
                    return new SemanticInfo((TypeSymbol)referencedSymbol, diagnostics);
                default:
                    return new SemanticInfo(ErrorSymbol.Instance, diagnostics);
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
                    case QueryOperatorParameterValueKind.ForcedRealLiteral:
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
                case QueryOperatorParameterValueKind.ForcedRealLiteral:
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

        private bool CheckIsNumberOrBool(Expression expression, List<Diagnostic> diagnostics)
        {
            var type = GetResultTypeOrError(expression);

            if (IsNumber(type) || type == ScalarTypes.Bool)
                return true;

            if (!GetResultTypeOrError(expression).IsError)
            {
                diagnostics.Add(DiagnosticFacts.GetExpressionMustBeNumericOrBool().WithLocation(expression));
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