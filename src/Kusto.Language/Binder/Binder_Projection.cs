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
    /// The binder performs general semantic analysis of the syntax tree, 
    /// identifying the symbols corresponding to named references, 
    /// the return types of operations and generating error diagnostics.
    /// </summary>
    internal sealed partial class Binder
    {
        private enum ProjectionStyle
        {
            Default,
            Extend,
            Print,
            Rename,
            Replace,
            Reorder,
            Summarize,
            GraphMatch,
            ByNames,
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

            // this is poorly formed syntax?
            if (expression == null)
                return;

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
            else if (style == ProjectionStyle.ByNames
                && _rowScope != null)
            {
                var namesOrPatterns = s_stringListPool.AllocateFromPool();
                var matchingColumns = s_columnListPool.AllocateFromPool();
                try
                {
                    GetProjectByNamesNames(expression, namesOrPatterns);
                    foreach (var nameOrPattern in namesOrPatterns)
                    {
                        matchingColumns.Clear();
                        _rowScope.GetMatchingColumns(nameOrPattern, matchingColumns);
                        builder.AddRange(matchingColumns, doNotRepeat: true);
                    }
                }
                finally
                {
                    s_stringListPool.ReturnToPool(namesOrPatterns);
                    s_columnListPool.ReturnToPool(matchingColumns);
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
                                if (tu.Columns.Count > 0)
                                {
                                    // first column has declared name so it uses declared name add/replace rule
                                    var firstCol = tu.Columns[0];
                                    col = new ColumnSymbol(n.Name.SimpleName, columnType ?? firstCol.Type, originalColumns: new[] { firstCol }, source: firstCol.Source);
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
                            }
                            else if (n.Expression.ReferencedSymbol is ColumnSymbol c)
                            {
                                col = new ColumnSymbol(n.Name.SimpleName, columnType ?? c.Type, source: n.Expression);
                                builder.Declare(col, diagnostics, n.Name, replace: true);
                                SetSemanticInfo(n.Name, CreateSemanticInfo(col));

                                if (doNotRepeat)
                                {
                                    builder.DoNotAdd(c);
                                }
                            }
                            else
                            {
                                col = new ColumnSymbol(n.Name.SimpleName, columnType ?? GetResultTypeOrError(n.Expression), source: n.Expression);
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
                                        col = new ColumnSymbol(name, type, originalColumns: col.OriginalColumns, source: col.Source);

                                        builder.Declare(col, diagnostics, nameDecl, replace: style == ProjectionStyle.Replace || style == ProjectionStyle.Extend);
                                        SetSemanticInfo(nameDecl, CreateSemanticInfo(col));

                                        if (doNotRepeat)
                                        {
                                            builder.DoNotAdd(tupleType.Columns[i]);
                                        }
                                    }
                                    else if (style != ProjectionStyle.Print)
                                    {
                                        if (cn.Expression.ReferencedSymbol is FunctionSymbol fs1)
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
                                    col = new ColumnSymbol(name.SimpleName, columnType ?? c.Type, originalColumns: new[] { c });
                                    builder.Declare(col, diagnostics, name, replace: true);
                                    SetSemanticInfo(name, CreateSemanticInfo(col));

                                    if (doNotRepeat)
                                    {
                                        builder.DoNotAdd(c);
                                    }
                                }
                                else
                                {
                                    col = new ColumnSymbol(name.SimpleName, columnType ?? GetResultTypeOrError(cn.Expression), source: expr);
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

                                col = new ColumnSymbol(columnName, columnType ?? ftype, source: f);
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
                                var name = GetFunctionResultName(f, null, _rowScope) ?? columnName ?? GetDefaultColumnName(expression, style == ProjectionStyle.Extend);
                                col = new ColumnSymbol(name, columnType ?? ftype, source: f);
                                builder.Add(col, replace: style == ProjectionStyle.Replace || style == ProjectionStyle.Extend);
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
                        if (col != null && style != ProjectionStyle.GraphMatch)
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
                            AddProjectionColumns(builder, OrderColumns(group.Members.OfType<ColumnSymbol>(), oe?.Ordering), doNotRepeat: true);
                        }
                        else if (GetResultType(expression) is GroupSymbol g)
                        {
                            diagnostics.Add(DiagnosticFacts.GetTheExpressionRefersToMoreThanOneColumn().WithLocation(expression));
                        }
                        else if (style == ProjectionStyle.GraphMatch)
                        {
                            var colName = GetGraphExpressionResultName(expression) ?? col?.Name;
                            if (col == null)
                            {
                                type = GetResultTypeOrError(expression);
                                if (!type.IsError && !type.IsScalar)
                                {
                                    diagnostics.Add(DiagnosticFacts.GetScalarTypeExpected().WithLocation(expression));
                                    type = ScalarTypes.Unknown;
                                }

                                col = GetOrDeclareColumnForExpression(expression, colName, columnType ?? type);
                            }

                            if (!string.IsNullOrWhiteSpace(colName))
                            {
                                col = col.WithName(colName);
                            }

                            builder.Add(col.WithType(columnType ?? col.Type));
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
                                col = GetOrDeclareColumnForExpression(expression, columnName, columnType ?? type);
                                builder.Add(col, columnName, replace: false);
                            }
                            else
                            {
                                var name = GetExpressionResultName(expression, null) ?? columnName ?? GetDefaultColumnName(expression, style == ProjectionStyle.Extend);
                                col = GetOrDeclareColumnForExpression(expression, name, columnType ?? type);
                                builder.Add(col, replace: style == ProjectionStyle.Replace || style == ProjectionStyle.Extend);
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Gets all the column names specified as literals, dynamic arrays or from column_names_of 
        /// </summary>
        private static void GetProjectByNamesNames(Expression expression, List<string> names)
        {
            while (expression.ReferencedSymbol is VariableSymbol vs
                && vs.Source != null)
            {
                // see through variable to initializer expression..
                // this will work with parameters too when functions are rebound
                expression = vs.Source;
            }

            if (expression is DynamicExpression dex)
            {
                if (dex.Expression.ConstantValue is string dstring)
                {
                    names.Add(dstring);
                }
                else if (dex.Expression is JsonArrayExpression jex)
                {
                    for (int i = 0; i < jex.Values.Count; i++)
                    {
                        if (jex.Values[i].Element.ConstantValue is string astring)
                        {
                            names.Add(astring);
                        }
                    }
                }
            }
            else if (expression is FunctionCallExpression fc
                && expression.ReferencedSymbol == Functions.ColumnNamesOf
                && fc.ArgumentList.Expressions.Count > 0
                && fc.ArgumentList.Expressions[0].Element.ResultType is TableSymbol rt)
            {
                // get names of columns in the argument to column_names_of() function
                foreach (var rtcol in rt.Columns)
                {
                    names.Add(rtcol.Name);
                }
            }
            else if (expression.ConstantValue is string name)
            {
                // a string literal or constant
                names.Add(name);
            }
        }

        private enum ColumnOrdering
        {
            None,
            Ascending,
            Descending,
            GrannyAscending,
            GrannyDescending
        }

        private static ColumnOrdering GetColumnOrdering(OrderingClause ordering)
        {
            if (ordering == null)
                return ColumnOrdering.None;

            switch (ordering.AscOrDescKeyword.Kind)
            {
                case SyntaxKind.AscKeyword:
                    return ColumnOrdering.Ascending;
                case SyntaxKind.DescKeyword:
                    return ColumnOrdering.Descending;
                case SyntaxKind.GrannyAscKeyword:
                    return ColumnOrdering.GrannyAscending;
                case SyntaxKind.GrannyDescKeyword:
                    return ColumnOrdering.GrannyDescending;
                default:
                    return ColumnOrdering.None;
            }
        }

        private static IEnumerable<ColumnSymbol> OrderColumns(IEnumerable<ColumnSymbol> columns, OrderingClause ordering)
        {
            switch (GetColumnOrdering(ordering))
            {
                case ColumnOrdering.Ascending:
                    return columns.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase);
                case ColumnOrdering.Descending:
                    return columns.OrderByDescending(c => c.Name, StringComparer.OrdinalIgnoreCase);
                case ColumnOrdering.GrannyAscending:
                    return columns.OrderBy(c => c.Name, StringAndNumberComparer.OrdinalIgnoreCase);
                case ColumnOrdering.GrannyDescending:
                    return columns.OrderByDescending(c => c.Name, StringAndNumberComparer.OrdinalIgnoreCase);
                case ColumnOrdering.None:
                default:
                    return columns;
            }
        }

        private static void AddProjectionColumns(ProjectionBuilder builder, IEnumerable<ColumnSymbol> columns, bool doNotRepeat)
        {
            // add any columns referenced in group
            foreach (var c in columns)
            {
                builder.Add(c, doNotRepeat: doNotRepeat);
            }
        }

        /// <summary>
        /// Returns a column symbol representing the result of the expression.
        /// If the expression just references a column, then it returns that column.
        /// Otherwise it creates new column symbol.
        /// </summary>
        private static ColumnSymbol GetOrDeclareColumnForExpression(Expression expression, string name = null, TypeSymbol type = null, string defaultName = null)
        {
            name = name ?? GetExpressionResultName(expression, defaultName);
            if (GetResultColumn(expression) is ColumnSymbol col)
            {
                if (name != null && col.Name != name)
                {
                    return new ColumnSymbol(name, type ?? col.Type, originalColumns: new[] { col }, source: expression);
                }
                else
                {
                    return col;
                }
            }
            else
            {
                type = type ?? GetResultTypeOrError(expression);
                return new ColumnSymbol(name, type, source: expression); 
            }
        }


        private int _defaultColumnNameSuffix = 1;
        private string GetDefaultColumnName(SyntaxNode location, bool includeRowScope)
        {
            var name = "Column" + _defaultColumnNameSuffix++;

            while (this.CanBindName(name, SymbolMatch.Any, location, includeRowScope: includeRowScope, inferColumns: false))
            {
                name = "Column" + _defaultColumnNameSuffix++;
            }

            return name;
        }

        public static ColumnSymbol GetResultColumn(Expression expr)
        {
            if (expr == null)
            {
                return null;
            }
            else if (expr is ParenthesizedExpression p)
            {
                return GetResultColumn(p.Expression);
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
                diagnostics?.Add(DiagnosticFacts.GetDuplicateColumnDeclaration(newName).WithLocation(location));
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
                case ParenthesizedExpression p:
                    return GetExpressionResultName(p.Expression, defaultName, row);
                case NameReference n:
                    return n.SimpleName;
                case BracketedExpression be:
                    if (be.Expression.IsLiteral
                        && be.Expression.ResultType is TypeSymbol bet
                        && (bet == ScalarTypes.String || bet == ScalarTypes.Long || bet == ScalarTypes.Int))
                    {
                        return be.Expression.LiteralValue.ToString();
                    }
                    return defaultName;
                case PathExpression p:
                    if (p.Expression.ResultType.IsDynamicArrayOrBag()
                        || p.Expression.ResultType == ScalarTypes.Unknown)
                    {
                        var left = GetExpressionResultName(p.Expression, null);
                        var right = GetExpressionResultName(p.Selector, null);
                        if (!string.IsNullOrWhiteSpace(left))
                        {
                            return $"{left}_{right}";
                        }
                        else
                        {
                            return right;
                        }
                    }
                    else
                    {
                        return GetExpressionResultName(p.Selector, defaultName);
                    }
                case ElementExpression e:
                    if (e.Expression.ResultType.IsDynamicArrayOrBag()
                        || e.Expression.ResultType == ScalarTypes.Unknown)
                    {
                        var left = GetExpressionResultName(e.Expression, null);
                        var right = GetExpressionResultName(e.Selector, null);
                        if (!string.IsNullOrWhiteSpace(left))
                        {
                            return $"{left}_{right}";
                        }
                        else
                        {
                            return right;
                        }
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

        /// <summary>
        /// Gets the name that an expression will use for its column name in a graph projection.
        /// </summary>
        public static string GetGraphExpressionResultName(Expression expression)
        {
            if (expression == null)
            {
                return string.Empty;
            }

            Expression patternElementExpression;
            Expression selector;
            if (expression is PathExpression pathExpression)
            {
                patternElementExpression = pathExpression.Expression;
                selector = pathExpression.Selector;
            }
            else if (expression is ElementExpression elementExpression)
            {
                patternElementExpression = elementExpression.Expression;
                selector = elementExpression.Selector;
            }
            else
            {
                return GetExpressionResultName(expression);
            }

            var left = patternElementExpression.Kind == SyntaxKind.ElementExpression || patternElementExpression.Kind == SyntaxKind.PathExpression
                ? GetGraphExpressionResultName(patternElementExpression)
                : GetExpressionResultName(patternElementExpression);

            var right = GetExpressionResultName(selector);    
            
            return !string.IsNullOrWhiteSpace(left) ? $"{left}_{right}" : right;
        }
    }
}