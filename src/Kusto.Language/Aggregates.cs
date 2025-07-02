using System.Collections.Generic;
using System.Linq;

namespace Kusto.Language
{
    using Syntax;
    using Symbols;
    using static FunctionHelpers;

    /// <summary>
    /// Well known aggregates
    /// </summary>
    public static class Aggregates
    {
        public static readonly FunctionSymbol Sum =
            new FunctionSymbol("sum", ReturnTypeKind.Parameter0Promoted,
                new Parameter("expr", ParameterTypeKind.Summable))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("sum");

        public static readonly FunctionSymbol SumIf =
            new FunctionSymbol("sumif", ReturnTypeKind.Parameter0Promoted,
                new Parameter("expr", ParameterTypeKind.Summable),
                new Parameter("predicate", ScalarTypes.Bool))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("sumif");

        public static readonly FunctionSymbol Cnt =
            new FunctionSymbol("cnt", ScalarTypes.Long)
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("cnt")
            .Obsolete("count")
            .Hide(); // legacy

        public static readonly FunctionSymbol Count =
            new FunctionSymbol("count", 
                 new Signature(ScalarTypes.Long),
                 new Signature(ScalarTypes.Long, 
                    new Parameter("predicate", ScalarTypes.Bool))
                    .Hide()
                    .Obsolete("countif"))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("count");

        public static readonly FunctionSymbol CountIf =
            new FunctionSymbol("countif", ScalarTypes.Long,
                new Parameter("predicate", ScalarTypes.Bool))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("countif");

        public static readonly FunctionSymbol DCount =
            new FunctionSymbol("dcount", ScalarTypes.Long,
                new Parameter("expr", ParameterTypeKind.Scalar),
                new Parameter("accuracy", ParameterTypeKind.NotDynamic, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("dcount");

        public static readonly FunctionSymbol DCountIf =
            new FunctionSymbol("dcountif", ScalarTypes.Long,
                new Parameter("expr", ParameterTypeKind.Scalar),
                new Parameter("predicate", ScalarTypes.Bool),
                new Parameter("accuracy", ParameterTypeKind.NotDynamic, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("dcountif");

        public static readonly FunctionSymbol TDigest =
            new FunctionSymbol("tdigest", 
                ScalarTypes.DynamicArray,
                new Parameter("expr", ParameterTypeKind.Summable),
                new Parameter("weight", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("tdigest");

        public static readonly FunctionSymbol TDigestMerge =
            new FunctionSymbol("tdigest_merge", 
                ScalarTypes.DynamicArray,
                new Parameter("tdigest", ParameterTypeKind.DynamicArray))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("merge_tdigests");

        public static readonly FunctionSymbol MergeTDigest =
            new FunctionSymbol("merge_tdigest", 
                ScalarTypes.DynamicArray,
                new Parameter("tdigest", ParameterTypeKind.DynamicArray))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("merge_tdigests");

        public static readonly FunctionSymbol Hll =
            new FunctionSymbol("hll", 
                ScalarTypes.DynamicArray,
                new Parameter("expr", ParameterTypeKind.NotRealOrBool),
                new Parameter("accuracy", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("hll");

        public static readonly FunctionSymbol HllIf =
            new FunctionSymbol("hll_if", 
                ScalarTypes.DynamicArray,
                new Parameter("expr", ParameterTypeKind.NotRealOrBool),
                new Parameter("predicate", ScalarTypes.Bool),
                new Parameter("accuracy", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("hll_if");

        public static readonly FunctionSymbol HllMerge =
            new FunctionSymbol("hll_merge", 
                ScalarTypes.DynamicArray,
                new Parameter("hll", ParameterTypeKind.DynamicArray))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("hll_merge");

        public static readonly FunctionSymbol Min =
            new FunctionSymbol("min", ReturnTypeKind.Parameter0,
                new Parameter("expr", ParameterTypeKind.Orderable))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("min");

        public static readonly FunctionSymbol MinIf =
            new FunctionSymbol("minif", ReturnTypeKind.Parameter0,
                new Parameter("expr", ParameterTypeKind.Orderable),
                new Parameter("predicate", ScalarTypes.Bool))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("minif");

        public static readonly FunctionSymbol Max =
            new FunctionSymbol("max", ReturnTypeKind.Parameter0,
               new Parameter("expr", ParameterTypeKind.Orderable))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("max");

        public static readonly FunctionSymbol MaxIf =
            new FunctionSymbol("maxif", ReturnTypeKind.Parameter0,
                new Parameter("expr", ParameterTypeKind.Orderable),
                new Parameter("predicate", ScalarTypes.Bool))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("maxif");

        public static readonly FunctionSymbol Avg =
            new FunctionSymbol("avg",
                new Signature(ScalarTypes.Real,
                    new Parameter("expr", ParameterTypeKind.Integer)),
                new Signature(ScalarTypes.Real,
                    new Parameter("expr", ScalarTypes.Real)),
                new Signature(ScalarTypes.Decimal,
                    new Parameter("expr", ScalarTypes.Decimal)),
                new Signature(ScalarTypes.TimeSpan,
                    new Parameter("expr", ScalarTypes.TimeSpan)),
                new Signature(ScalarTypes.DateTime,
                    new Parameter("expr", ScalarTypes.DateTime)))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("avg");

        public static readonly FunctionSymbol AvgIf =
            new FunctionSymbol("avgif",
                new Signature(ScalarTypes.Real,
                    new Parameter("expr", ParameterTypeKind.Integer),
                    new Parameter("predicate", ScalarTypes.Bool)),
                new Signature(ScalarTypes.Real,
                    new Parameter("expr", ScalarTypes.Real),
                    new Parameter("predicate", ScalarTypes.Bool)),
                new Signature(ScalarTypes.Decimal,
                    new Parameter("expr", ScalarTypes.Decimal),
                    new Parameter("predicate", ScalarTypes.Bool)),
                new Signature(ScalarTypes.TimeSpan,
                    new Parameter("expr", ScalarTypes.TimeSpan),
                    new Parameter("predicate", ScalarTypes.Bool)),
                new Signature(ScalarTypes.DateTime,
                    new Parameter("expr", ScalarTypes.DateTime),
                    new Parameter("predicate", ScalarTypes.Bool)))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("avgif");

        public static readonly FunctionSymbol MakeList_Deprecated =
            new FunctionSymbol("makelist",
                ReturnTypeKind.Parameter0Array,
                new Parameter("expr", ParameterTypeKind.Scalar),
                new Parameter("maxSize", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("list")
            .Obsolete("make_list")
            .Hide();

        public static readonly FunctionSymbol MakeList =
            new FunctionSymbol("make_list",
                ReturnTypeKind.Parameter0Array,
                new Parameter("expr", ParameterTypeKind.Scalar),
                new Parameter("maxSize", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("list");

        public static readonly FunctionSymbol MakeListIf =
            new FunctionSymbol("make_list_if",
                ReturnTypeKind.Parameter0Array,
                new Parameter("expr", ParameterTypeKind.Scalar),
                new Parameter("predicate", ScalarTypes.Bool),
                new Parameter("maxSize", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("list");

        public static readonly FunctionSymbol MakeListWithNulls =
            new FunctionSymbol("make_list_with_nulls", 
                ReturnTypeKind.Parameter0Array,
                new Parameter("expr", ParameterTypeKind.Scalar))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("list");

        public static readonly FunctionSymbol MakeSet_Deprecated =
            new FunctionSymbol("makeset",
                ReturnTypeKind.Parameter0Array,
                new Parameter("expr", ParameterTypeKind.Scalar),
                new Parameter("maxSize", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("set")
            .Obsolete("make_set")
            .Hide();

        public static readonly FunctionSymbol MakeSet =
            new FunctionSymbol("make_set",
                ReturnTypeKind.Parameter0Array,
                new Parameter("expr", ParameterTypeKind.Scalar),
                new Parameter("maxSize", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("set");

        public static readonly FunctionSymbol MakeSetIf =
            new FunctionSymbol("make_set_if",
                ReturnTypeKind.Parameter0Array,
                new Parameter("expr", ParameterTypeKind.Scalar),
                new Parameter("predicate", ScalarTypes.Bool),
                new Parameter("maxSize", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("set");

        public static readonly FunctionSymbol Passthrough =
           new FunctionSymbol("passthrough", ReturnTypeKind.Parameter0,
               new Parameter("expr", ParameterTypeKind.Scalar))
           .WithResultNameKind(ResultNameKind.FirstArgument)
           .Hide();

        public static readonly FunctionSymbol MakeDictionary =
            new FunctionSymbol("make_dictionary", 
                ScalarTypes.DynamicBag,
                new Parameter("expr", ParameterTypeKind.DynamicBag),
                new Parameter("maxSize", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("dictionary")
            .Hide();

        public static readonly FunctionSymbol MakeBag =
            new FunctionSymbol("make_bag", 
                ScalarTypes.DynamicBag,
                new Parameter("expr", ParameterTypeKind.DynamicBag),
                new Parameter("maxSize", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("bag");

        public static readonly FunctionSymbol MakeBagIf =
            new FunctionSymbol("make_bag_if", 
                ScalarTypes.DynamicBag,
                new Parameter("expr", ParameterTypeKind.DynamicBag),
                new Parameter("predicate", ScalarTypes.Bool),
                new Parameter("maxSize", ParameterTypeKind.Integer, minOccurring: 0))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("bag");

        public static readonly FunctionSymbol BuildSchema =
            new FunctionSymbol("buildschema", 
                ScalarTypes.DynamicBag,
                new Parameter("expr", ParameterTypeKind.DynamicBag))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("schema");

        public static readonly FunctionSymbol BinaryAllOr =
           new FunctionSymbol("binary_all_or",
               new Signature(ReturnTypeKind.Parameter0,
                   new Parameter("expr", ParameterTypeKind.Summable)))
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol BinaryAllAnd =
          new FunctionSymbol("binary_all_and",
              new Signature(ReturnTypeKind.Parameter0,
                  new Parameter("expr", ParameterTypeKind.Summable)))
          .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol BinaryAllXor =
          new FunctionSymbol("binary_all_xor",
              new Signature(ReturnTypeKind.Parameter0,
                  new Parameter("expr", ParameterTypeKind.Summable)))
          .WithResultNameKind(ResultNameKind.FirstArgument);

        public static readonly FunctionSymbol CountDistinct =
            new FunctionSymbol("count_distinct", ScalarTypes.Long,
                new Parameter("expr", ParameterTypeKind.NotDynamic))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("count_distinct")
            .WithOptimizedAlternative("dcount");

        public static readonly FunctionSymbol CountDistinctIf =
            new FunctionSymbol("count_distinctif", ScalarTypes.Long,
                new Parameter("expr", ParameterTypeKind.NotDynamic),
                new Parameter("predicate", ScalarTypes.Bool))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("count_distinctif")
            .WithOptimizedAlternative("dcountif");

        private static void AddPercentileColumns(
            List<ColumnSymbol> columns, CustomReturnTypeContext context, string valueParameterName, string percentileParameterName)
        {
            if (context.GetArgument(valueParameterName) is Expression valueArg
                && context.GetResultName(valueArg) is string valueArgName)
            {
                var resultType = valueArg.ResultType;
                if (resultType == ScalarTypes.Int)
                    resultType = ScalarTypes.Long;
                else if (resultType == ScalarTypes.Decimal)
                    resultType = ScalarTypes.Real;

                foreach (var percentileArg in context.GetArguments(percentileParameterName))
                {
                    var percentileFragment = MakeValidNameFragment(GetConstantValue(percentileArg));
                    var name = percentileParameterName + "_" + valueArgName + "_" + percentileFragment;
                    columns.Add(new ColumnSymbol(name, resultType, source: valueArg));
                }
            }
        }

        private static CustomReturnType PercentileReturn = context =>
        {
            var cols = new List<ColumnSymbol>();
            AddPercentileColumns(cols, context, "expr", "percentile");
            return new TupleSymbol(cols);
        };

        private static CustomReturnType PercentileArrayReturn = context =>
        {
            var cols = new List<ColumnSymbol>();
            
            if (context.GetArgument("expr") is Expression valueArg
                && context.GetResultName(valueArg) is string valueArgName)
            {
                cols.Add(new ColumnSymbol("percentiles_" + valueArgName, ScalarTypes.DynamicArray, source: valueArg));
            }

            return new TupleSymbol(cols);
        };

        public static readonly FunctionSymbol Percentile =
            new FunctionSymbol("percentile", 
                PercentileReturn, 
                Tabularity.Scalar,
                new Parameter("expr", ParameterTypeKind.Scalar),
                new Parameter("percentile", ParameterTypeKind.Number));

        public static readonly FunctionSymbol Percentiles =
            new FunctionSymbol("percentiles", 
                PercentileReturn, 
                Tabularity.Scalar,
                new Parameter("expr", ParameterTypeKind.Scalar),
                new Parameter("percentile", ParameterTypeKind.Number, minOccurring: 1, maxOccurring: MaxRepeat));

        public static readonly FunctionSymbol PercentilesArray =
            new FunctionSymbol("percentiles_array",
                new Signature(
                    PercentileArrayReturn, 
                    Tabularity.Scalar,
                    new Parameter("expr", ParameterTypeKind.Scalar),
                    new Parameter("percentile", ParameterTypeKind.Number, minOccurring: 1, maxOccurring: MaxRepeat)),
                new Signature(
                    PercentileArrayReturn, 
                    Tabularity.Scalar,
                    new Parameter("expr", ParameterTypeKind.Scalar),
                    new Parameter("percentiles", ParameterTypeKind.DynamicArray)));

        public static readonly FunctionSymbol PercentileW =
            new FunctionSymbol("percentilew", PercentileReturn, Tabularity.Scalar,
                new Parameter("expr", ParameterTypeKind.Scalar),
                new Parameter("weight", ParameterTypeKind.Integer),
                new Parameter("percentile", ParameterTypeKind.Number));

        public static readonly FunctionSymbol PercentilesW =
            new FunctionSymbol("percentilesw", PercentileReturn, Tabularity.Scalar,
                new Parameter("expr", ParameterTypeKind.Scalar),
                new Parameter("weight", ParameterTypeKind.Integer),
                new Parameter("percentile", ParameterTypeKind.Number, minOccurring: 1, maxOccurring: MaxRepeat));

        public static readonly FunctionSymbol PercentilesWArray =
            new FunctionSymbol("percentilesw_array",
                new Signature(PercentileArrayReturn, Tabularity.Scalar,
                    new Parameter("expr", ParameterTypeKind.Scalar),
                    new Parameter("weight", ParameterTypeKind.Integer),
                    new Parameter("percentile", ParameterTypeKind.Number, minOccurring: 1, maxOccurring: MaxRepeat)),
                new Signature(PercentileArrayReturn, Tabularity.Scalar,
                    new Parameter("expr", ParameterTypeKind.Scalar),
                    new Parameter("weight", ParameterTypeKind.Integer),
                    new Parameter("percentiles", ParameterTypeKind.DynamicArray)));

        public static readonly FunctionSymbol Stdev =
            new FunctionSymbol("stdev", ScalarTypes.Real,
                new Parameter("expr", ParameterTypeKind.Summable))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("stdev");

        public static readonly FunctionSymbol StdevIf =
            new FunctionSymbol("stdevif", ScalarTypes.Real,
                new Parameter("expr", ParameterTypeKind.Summable),
                new Parameter("predicate", ScalarTypes.Bool))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("stdevif");

        public static readonly FunctionSymbol Stdevp =
            new FunctionSymbol("stdevp", ScalarTypes.Real,
                new Parameter("expr", ParameterTypeKind.Summable))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("stdevp");

        public static readonly FunctionSymbol Variance =
            new FunctionSymbol("variance", ScalarTypes.Real,
                new Parameter("expr", ParameterTypeKind.Summable))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("variance");

        public static readonly FunctionSymbol VarianceIf =
            new FunctionSymbol("varianceif", ScalarTypes.Real,
                new Parameter("expr", ParameterTypeKind.Summable),
                new Parameter("predicate", ScalarTypes.Bool))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("varianceif");

        public static readonly FunctionSymbol Variancep =
            new FunctionSymbol("variancep", ScalarTypes.Real,
                new Parameter("expr", ParameterTypeKind.Summable))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("variancep");

        public static readonly FunctionSymbol VariancepIf =
            new FunctionSymbol("variancepif", ScalarTypes.Real,
                new Parameter("expr", ParameterTypeKind.Summable),
                new Parameter("predicate", ScalarTypes.Bool))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("variancepif");

        public static readonly FunctionSymbol Covariance =
            new FunctionSymbol("covariance", ScalarTypes.Real,
                new Parameter("expr", ParameterTypeKind.Summable),
                new Parameter("expr", ParameterTypeKind.Summable))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("covariance");

        public static readonly FunctionSymbol CovarianceIf =
            new FunctionSymbol("covarianceif", ScalarTypes.Real,
                new Parameter("expr", ParameterTypeKind.Summable),
                new Parameter("expr", ParameterTypeKind.Summable),
                new Parameter("predicate", ScalarTypes.Bool))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("covarianceif");

        public static readonly FunctionSymbol Covariancep =
            new FunctionSymbol("covariancep", ScalarTypes.Real,
                new Parameter("expr", ParameterTypeKind.Summable),
                new Parameter("expr", ParameterTypeKind.Summable))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("covariancep");

        public static readonly FunctionSymbol CovariancepIf =
            new FunctionSymbol("covariancepif", ScalarTypes.Real,
                new Parameter("expr", ParameterTypeKind.Summable),
                new Parameter("expr", ParameterTypeKind.Summable),
                new Parameter("predicate", ScalarTypes.Bool))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("covariancepif");

        public static readonly FunctionSymbol Any =
            new FunctionSymbol("any",
                new Signature(
                    ReturnTypeKind.Parameter0,
                    new Parameter("expr", ParameterTypeKind.Scalar)),
                new Signature(
                    context => GetAnyResult(context, unnamedExpressionPrefix: null),
                    Tabularity.Scalar,
                    new Parameter("expr", ParameterTypeKind.Scalar, minOccurring: 2, maxOccurring: MaxRepeat)),
                new Signature(
                    context => GetAnyResult(context, unnamedExpressionPrefix: null),
                    Tabularity.Scalar,
                    new Parameter("expr", ParameterTypeKind.Scalar, ArgumentKind.StarOnly)))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("any")
            .Obsolete("take_any");

        public static readonly FunctionSymbol TakeAny =
           new FunctionSymbol("take_any",
               new Signature(
                   context => GetAnyResult(context, unnamedExpressionPrefix: "any_"),
                   Tabularity.Scalar,
                   new Parameter("expr", ParameterTypeKind.Scalar, ArgumentKind.StarAllowed, minOccurring: 1, maxOccurring: MaxRepeat)));

        public static readonly FunctionSymbol AnyIf =
            new FunctionSymbol("anyif",
                new Signature(
                    ReturnTypeKind.Parameter0,
                    new Parameter("expr", ParameterTypeKind.Scalar),
                    new Parameter("predicate", ScalarTypes.Bool)))
            .WithResultNameKind(ResultNameKind.PrefixAndFirstArgument)
            .WithResultNamePrefix("anyif")
            .Obsolete("take_anyif");

        public static readonly FunctionSymbol TakeAnyIf =
           new FunctionSymbol("take_anyif",
               new Signature(
                   ReturnTypeKind.Parameter0,
                   new Parameter("expr", ParameterTypeKind.Scalar),
                   new Parameter("predicate", ScalarTypes.Bool)))
            .WithResultNameKind(ResultNameKind.FirstArgument);

        public static TypeSymbol GetAnyResult(CustomReturnTypeContext context, string unnamedExpressionPrefix)
        {
            var columns = new List<ColumnSymbol>();
            var prefix = unnamedExpressionPrefix ?? string.Empty;

            var doNotRepeat = new HashSet<ColumnSymbol>(GetSummarizeByColumns(context.Arguments));
            var anyStar = context.Arguments.Any(a => a is StarExpression);

            for (int i = 0; i < context.Arguments.Count; i++)
            {
                var arg = context.Arguments[i];

                if (arg is StarExpression)
                {
                    foreach (var c in context.RowScope.Columns)
                    {                       
                        if (CanAddAnyResultColumn(c, doNotRepeat, anyStar))
                        {
                            doNotRepeat.Add(c);
                            columns.Add(c);
                        }
                    }
                }
                else if (arg is SimpleNamedExpression snx
                    && GetResultColumn(snx.Expression) is ColumnSymbol vc)
                {
                    if (CanAddAnyResultColumn(vc, doNotRepeat, anyStar))
                    {
                        doNotRepeat.Add(vc);
                        columns.Add(new ColumnSymbol(snx.Name.SimpleName, vc.Type, originalColumns: new[] { vc }));
                    }
                }
                else if (GetResultColumn(arg) is ColumnSymbol c)
                {
                    // this is explicitly referenced column (not assigned)
                    if (CanAddAnyResultColumn(c, doNotRepeat, anyStar))
                    {
                        if (doNotRepeat.Contains(c))
                        {
                            // change identity of explicitly referenced columns so won't match same columns already in projection list
                            // this will get renamed by project builder
                            columns.Add(new ColumnSymbol(c.Name, c.Type, originalColumns: new[] { c }));
                        }
                        else
                        {
                            doNotRepeat.Add(c);
                            columns.Add(c);
                        }
                    }
                }
                else
                {
                    var expName = Binding.Binder.GetExpressionResultName(arg, "");
                    if (string.IsNullOrEmpty(expName))
                    {
                        expName = prefix + "arg" + i;
                    }

                    var col = new ColumnSymbol(expName, arg.ResultType, source: arg);
                    columns.Add(col);
                }
            }

            return new TupleSymbol(columns);
        }

        private static bool CanAddAnyResultColumn(ColumnSymbol column, HashSet<ColumnSymbol> doNotRepeat, bool anyStar)
        {
            if (!anyStar)
                return true;

            return !doNotRepeat.Contains(column);
        }

        public static readonly FunctionSymbol ArgMin =
            new FunctionSymbol("arg_min",
                new Signature(
                    context => GetArgMinMaxResult(context, "min"),
                    Tabularity.Scalar,
                    new Parameter("minimized", ParameterTypeKind.Orderable),
                    new Parameter("returned", ParameterTypeKind.Scalar, ArgumentKind.StarAllowed, minOccurring: 0, maxOccurring: MaxRepeat)));

        public static readonly FunctionSymbol ArgMax =
            new FunctionSymbol("arg_max",
                new Signature(
                    context => GetArgMinMaxResult(context, "max"), 
                    Tabularity.Scalar,
                    new Parameter("maximized", ParameterTypeKind.Orderable),
                    new Parameter("returned", ParameterTypeKind.Scalar, ArgumentKind.StarAllowed, minOccurring: 0, maxOccurring: MaxRepeat)));

        private static TypeSymbol GetArgMinMaxResult(CustomReturnTypeContext context, string prefix)
        {
            var columns = new List<ColumnSymbol>();

            if (context.Arguments.Count > 0)
            {
                var byClauseColumns = new HashSet<ColumnSymbol>(GetSummarizeByColumns(context.Arguments));
                var doNotRepeat = new HashSet<ColumnSymbol>();

                var primaryArg = context.Arguments[0];
                var primaryColName = Binding.Binder.GetExpressionResultName(primaryArg);

                var anyStar = context.Arguments.Any(a => a is StarExpression);

                for (int i = 0; i < context.Arguments.Count; i++)
                {
                    var arg = context.Arguments[i];

                    if (arg is StarExpression)
                    {
                        foreach (var c in context.RowScope.Columns)
                        {
                            if (CanAddArgMinMaxResultColumn(i, c, byClauseColumns, doNotRepeat, anyStar))
                            {
                                doNotRepeat.Add(c);
                                columns.Add(c);
                            }
                        }
                    }
                    else if (arg is SimpleNamedExpression snx 
                        && GetResultColumn(snx.Expression) is ColumnSymbol vc)
                    {
                        if (CanAddArgMinMaxResultColumn(i, vc, byClauseColumns, doNotRepeat, anyStar))
                        {
                            doNotRepeat.Add(vc);
                            columns.Add(new ColumnSymbol(snx.Name.SimpleName, vc.Type, originalColumns: new[] { vc }));
                        }
                    }
                    else if (GetResultColumn(arg) is ColumnSymbol c)
                    {
                        // this is explicitly referenced column (not assigned)
                        if (CanAddArgMinMaxResultColumn(i, c, byClauseColumns, doNotRepeat, anyStar))
                        {
                            if (doNotRepeat.Contains(c)
                                || byClauseColumns.Contains(c))
                            {
                                // change identity of explicitly referenced columns so won't match same columns already in projection list
                                // this will get renamed by project builder
                                columns.Add(new ColumnSymbol(c.Name, c.Type, originalColumns: new[] { c }));
                            }
                            else
                            {
                                doNotRepeat.Add(c);
                                columns.Add(c);
                            }
                        }
                    }
                    else
                    {
                        var expName = Binding.Binder.GetExpressionResultName(arg, null);
                        if (expName == null)
                        {
                            if (i == 0)
                            {
                                expName = prefix + "_";
                            }
                            else
                            {
                                expName = prefix + "_" + primaryColName + "_arg" + i;
                            }
                        }

                        var col = new ColumnSymbol(expName, arg.ResultType, source: arg);
                        columns.Add(col);
                    }
                }
            }

            return new TupleSymbol(columns);
        }

        private static bool CanAddArgMinMaxResultColumn(int argIndex, ColumnSymbol column, HashSet<ColumnSymbol> byClauseColumns, HashSet<ColumnSymbol> doNotRepeat, bool anyStar)
        {
            if (argIndex == 0)
                return true;

            if (!anyStar)
                return true;

            return !doNotRepeat.Contains(column)
                && !byClauseColumns.Contains(column);
        }

        private static ColumnSymbol GetResultColumn(Expression expr) =>
            Kusto.Language.Binding.Binder.GetResultColumn(expr);

        public static readonly FunctionSymbol ArgMin_Deprecated =
            new FunctionSymbol("argmin",
                new Signature(
                    GetArgMinMaxDepResult,
                    Tabularity.Scalar,
                    new Parameter("minimized", ParameterTypeKind.Orderable),
                    new Parameter("returned", ParameterTypeKind.Scalar, ArgumentKind.StarAllowed, minOccurring: 0, maxOccurring: MaxRepeat)))
            .WithResultNamePrefix("min")
            .Obsolete("arg_min")
            .Hide();

        public static readonly FunctionSymbol ArgMax_Deprecated =
            new FunctionSymbol("argmax",
                new Signature(
                    GetArgMinMaxDepResult,
                    Tabularity.Scalar,
                    new Parameter("maximized", ParameterTypeKind.Orderable),
                    new Parameter("returned", ParameterTypeKind.Scalar, ArgumentKind.StarAllowed, minOccurring: 0, maxOccurring: MaxRepeat)))
            .WithResultNamePrefix("max")
            .Obsolete("arg_max")
            .Hide();

        private static TypeSymbol GetArgMinMaxDepResult(CustomReturnTypeContext context)
        {
            var columns = new List<ColumnSymbol>();

            if (context.Arguments.Count > 0)
            {
                // determine columns in by expression
                var byClauseColumns = new HashSet<ColumnSymbol>(GetSummarizeByColumns(context.Arguments));
                var doNotRepeat = new HashSet<ColumnSymbol>();
                var anyStar = context.Arguments.Any(a => a is StarExpression);

                var primaryArg = context.Arguments[0];
                string primaryColName;

                if (GetResultColumn(primaryArg) is ColumnSymbol pc)
                {
                    doNotRepeat.Add(pc);
                    columns.Add(pc);
                    primaryColName = pc.Name;
                }
                else
                {
                    primaryColName = Binding.Binder.GetExpressionResultName(primaryArg);
                    var primaryCol = new ColumnSymbol(primaryColName, primaryArg.ResultType, source: primaryArg);
                    columns.Add(primaryCol);
                }

                for (int i = 1; i < context.Arguments.Count; i++)
                {
                    var arg = context.Arguments[i];

                    if (arg is StarExpression)
                    {
                        foreach (var c in context.RowScope.Columns)
                        {
                            if (c != primaryArg.ReferencedSymbol 
                                && CanAddArgMinMaxResultColumn(i, c, byClauseColumns, doNotRepeat, anyStar))
                            {
                                doNotRepeat.Add(c);
                                columns.Add(c.WithName(primaryColName + "_" + c.Name).WithOriginalColumns(c));
                            }
                        }
                    }
                    else if (arg is SimpleNamedExpression snx
                        && GetResultColumn(snx.Expression) is ColumnSymbol vc)
                    {
                        if (CanAddArgMinMaxResultColumn(i, vc, byClauseColumns, doNotRepeat, anyStar))
                        {
                            doNotRepeat.Add(vc);
                            columns.Add(new ColumnSymbol(primaryColName + "_" +snx.Name.SimpleName, vc.Type, originalColumns: new[] { vc }));
                        }
                    }
                    else if (GetResultColumn(arg) is ColumnSymbol c)
                    {
                        if (CanAddArgMinMaxResultColumn(i, c, byClauseColumns, doNotRepeat, anyStar))
                        {
                            doNotRepeat.Add(c);
                            columns.Add(c.WithName(primaryColName + "_" + c.Name).WithOriginalColumns(c));
                        }
                    }
                    else
                    {
                        var expName = Binding.Binder.GetExpressionResultName(arg, null);
                        if (expName == null)
                            expName = "arg" + i;
                        var col = new ColumnSymbol(primaryColName + "_" + expName, arg.ResultType, source: arg);
                        columns.Add(col);
                    }
                }
            }

            return new TupleSymbol(columns);
        }

        public static IReadOnlyList<FunctionSymbol> All { get; } = new FunctionSymbol[]
        {
            Sum,
            SumIf,
            Cnt,
            Count,
            CountIf,
            DCount,
            DCountIf,
            TDigest,
            TDigestMerge,
            MergeTDigest,
            Hll,
            HllIf,
            HllMerge,
            Min,
            MinIf,
            Max,
            MaxIf,
            Avg,
            AvgIf,
            MakeList_Deprecated,
            MakeList,
            MakeListIf,
            MakeListWithNulls,
            MakeSet_Deprecated,
            MakeSet,
            MakeSetIf,
            MakeDictionary,
            MakeBag,
            MakeBagIf,
            BuildSchema,
            Passthrough,
            Percentile,
            Percentiles,
            PercentilesArray,
            PercentileW,
            PercentilesW,
            PercentilesWArray,
            Stdev,
            StdevIf,
            Stdevp,
            Variance,
            VarianceIf,
            Variancep,
            VariancepIf,
            Covariance,
            CovarianceIf,
            Covariancep,
            CovariancepIf,
            Any,
            TakeAny,
            AnyIf,
            TakeAnyIf,
            ArgMin,
            ArgMax,
            ArgMin_Deprecated,
            ArgMax_Deprecated,
            BinaryAllOr,
            BinaryAllAnd,
            BinaryAllXor,
            CountDistinct,
            CountDistinctIf
        };
    }
}