---
title: summarize operator - Azure Data Explorer
description: This article describes summarize operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/27/2022
ms.localizationpriority: high 
---
# summarize operator

Produces a table that aggregates the content of the input table.

```kusto
Sales | summarize NumTransactions=count(), Total=sum(UnitPrice * NumUnits) by Fruit, StartOfMonth=startofmonth(SellDateTime)
```

Returns a table with how many sell transactions and the total amount per fruit and sell month.
The output columns show the count of transactions, transaction worth, fruit, and the datetime of the beginning of the month
in which the transaction was recorded.

```kusto
T | summarize count() by price_range=bin(price, 10.0)
```

A table that shows how many items have prices in each interval  [0,10.0], [10.0,20.0], and so on. This example has a column for the count and one for the price range. All other input columns are ignored.

## Syntax

*T* `| summarize` [*SummarizeParameters*]
      [[*Column* `=`] *Aggregation* [`,` ...]]
    [`by`
      [*Column* `=`] *GroupExpression* [`,` ...]]

## Arguments

* *Column:* Optional name for a result column. Defaults to a name derived from the expression.
* *Aggregation:* A call to an [aggregation function](aggregation-functions.md) such as `count()` or `avg()`, with column names as arguments.
* *GroupExpression:* A scalar expression that can reference the input data.
  The output will have as many records as there are distinct values of all the
  group expressions.
* *SummarizeParameters*: Zero or more (space-separated) parameters in the form of *Name* `=` *Value* that control the behavior. The following parameters are supported:
  
  |Name  |Description  |
  |---|---|
  |`hint.num_partitions` |Specifies the number of partitions used to share the query load on cluster nodes. See [shuffle query](shufflequery.md)  |
  |`hint.shufflekey=<key>` |The `shufflekey` query shares the query load on cluster nodes, using a key to partition data. See [shuffle query](shufflequery.md) |
  |`hint.strategy=shuffle` |The `shuffle` strategy query shares the query load on cluster nodes, where each node will process one partition of the data. See [shuffle query](shufflequery.md)  |

> [!NOTE]
> When the input table is empty, the output depends on whether *GroupExpression*
> is used:
>
> * If *GroupExpression* is not provided, the output will be a single (empty) row.
> * If *GroupExpression* is provided, the output will have no rows.

## Returns

The input rows are arranged into groups having the same values of the `by` expressions. Then the specified aggregation functions are computed over each group, producing a row for each group. The result contains the `by` columns and also at least one column for each computed aggregate. (Some aggregation functions return multiple columns.)

The result has as many rows as there are distinct combinations of `by` values
(which may be zero). If there are no group keys provided, the result has a single
record.

To summarize over ranges of numeric values, use `bin()` to reduce ranges to discrete values.

> [!NOTE]
> * Although you can provide arbitrary expressions for both the aggregation and grouping expressions, it's more efficient to use simple column names, or apply `bin()` to a numeric column.
> * The automatic hourly bins for datetime columns is no longer supported. Use explicit binning instead. For example, `summarize by bin(timestamp, 1h)`.

## Aggregates default values

The following table summarizes the default values of aggregations:

| Operator | Default value |
|--|--|
| `count()`, `countif()`, `dcount()`, `dcountif()` | 0 |
| `make_bag()`, `make_bag_if()`, `make_list()`, `make_list_if()`, `make_set()`, `make_set_if()` | empty dynamic array              ([]) |
| All others | null |

 When using these aggregates over entities which includes null values, the null values will be ignored and won't participate in the calculation (see examples below).

## Examples

:::image type="content" source="images/summarizeoperator/summarize-price-by-supplier.png" alt-text="Summarize price by fruit and supplier.":::

### Unique combination

Determine what unique combinations of
`ActivityType` and `CompletionStatus` there are in a table. There are no aggregation functions, just group-by keys. The output will just show the columns for those results:

```kusto
Activities | summarize by ActivityType, completionStatus
```

**Output**

|`ActivityType`|`completionStatus`
|---|---
|`dancing`|`started`
|`singing`|`started`
|`dancing`|`abandoned`
|`singing`|`completed`

### Minimum and maximum timestamp

Finds the minimum and maximum timestamp of all records in the Activities table. There's no group-by clause, so there's just one row in the output:

```kusto
Activities | summarize Min = min(Timestamp), Max = max(Timestamp)
```

**Output**

|`Min`|`Max`
|---|---
|`1975-06-09 09:21:45` | `2015-12-24 23:45:00`

### Distinct count

Create a row for each continent, showing a count of the cities in which activities occur. Because there are few values for "continent", no grouping function is needed in the 'by' clause:

```kusto
Activities | summarize cities=dcount(city) by continent
```

**Output**

|`cities`|`continent`
|---|---
|`4290`|`Asia`|
|`3267`|`Europe`|
|`2673`|`North America`|

### Histogram

The following example calculates a histogram for each activity
type. Because `Duration` has many values, use `bin` to group its values into 10-minute intervals:

```kusto
Activities | summarize count() by ActivityType, length=bin(Duration, 10m)
```

**Output**

|`count_`|`ActivityType`|`length`
|---|---|---
|`354`| `dancing` | `0:00:00.000`
|`23`|`singing` | `0:00:00.000`
|`2717`|`dancing`|`0:10:00.000`
|`341`|`singing`|`0:10:00.000`
|`725`|`dancing`|`0:20:00.000`
|`2876`|`singing`|`0:20:00.000`
|...

### Aggregates default values

When the input of `summarize` operator has at least one empty group-by key, its result is empty, too.

When the input of `summarize` operator doesn't have an empty group-by key, the result is the default values of the aggregates used in the `summarize`:

```kusto
datatable(x:long)[]
| summarize any_x=take_any(x), arg_max_x=arg_max(x, *), arg_min_x=arg_min(x, *), avg(x), buildschema(todynamic(tostring(x))), max(x), min(x), percentile(x, 55), hll(x) ,stdev(x), sum(x), sumif(x, x > 0), tdigest(x), variance(x)
```

**Output**

|any_x|arg_max_x|arg_min_x|avg_x|schema_x|max_x|min_x|percentile_x_55|hll_x|stdev_x|sum_x|sumif_x|tdigest_x|variance_x|
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
||||NaN||||||0|0|0||0|

The result of `avg_x(x)` is `NaN` due to dividing by 0.

```kusto
datatable(x:long)[]
| summarize  count(x), countif(x > 0) , dcount(x), dcountif(x, x > 0)
```

**Output**

|count_x|countif_|dcount_x|dcountif_x|
|---|---|---|---|
|0|0|0|0|

```kusto
datatable(x:long)[]
| summarize  make_set(x), make_list(x)
```

**Output**

|set_x|list_x|
|---|---|
|[]|[]|

The aggregate avg sums all the non-nulls and counts only those which participated in the calculation (won't take nulls into account).

```kusto
range x from 1 to 2 step 1
| extend y = iff(x == 1, real(null), real(5))
| summarize sum(y), avg(y)
```

**Output**

|sum_y|avg_y|
|---|---|
|5|5|

The regular count will count nulls:

```kusto
range x from 1 to 2 step 1
| extend y = iff(x == 1, real(null), real(5))
| summarize count(y)
```

**Output**

|count_y|
|---|
|2|

```kusto
range x from 1 to 2 step 1
| extend y = iff(x == 1, real(null), real(5))
| summarize make_set(y), make_set(y)
```

**Output**

|set_y|set_y1|
|---|---|
|[5.0]|[5.0]|
