---
title: partition operator - Azure Data Explorer
description: This article describes partition operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 07/03/2022
---
# Partition operator

The partition operator partitions the records of its input table into multiple subtables according to values in a key column, runs a subquery on each subtable, and produces a single output table that is the union of the results of all subqueries. This operator is useful when you need to perform a subquery only on a subset of rows that belongs to the same partition key, and not query the whole dataset. These subqueries could include aggregate functions, window functions, top N and others.

The partition operator supports several strategies of subquery operation: 

* [Native](#native-strategy) - use with an implicit data source with thousands of key partition values.
* [Shuffle](#shuffle-strategy) - use with an implicit source with millions of key partition values.
* [Legacy](#legacy-strategy) - use with an implicit or explicit source for 64 or less key partition values.

## Native strategy

This subquery is a tabular transformation that doesn't specify a tabular source. The source is implicit and is assigned according to the subtable partitions. It should be applied when the number of distinct values of the partition key isn't large, roughly in the thousand. Use `hint.strategy=native` for this strategy. There's no restriction on the number of partitions.

## Shuffle strategy

This subquery is a tabular transformation that doesn't specify a tabular source. The source is implicit and will be assigned according to the subtable partitions. The strategy applies when the number of distinct values of the partition key is large, in the millions. Use `hint.strategy=shuffle` for this strategy. There's no restriction on the number of partitions. For more information about shuffle strategy and performance, see [shuffle](shufflequery.md).

## Native and shuffle strategy operators

The difference between `hint.strategy=native` and `hint.strategy=shuffle` is mainly to allow the caller to indicate the cardinality and execution strategy of the subquery, and can affect the execution time. There's no other semantic difference
between the two.

For `native` and `shuffle` strategy, the source of the subquery is implicit, and can't be referenced by the subquery. This strategy supports a limited set of operators: `project`, `sort`, `summarize`, `take`, `top`, `order`, `mv-expand`, `mv-apply`, `make-series`, `limit`, `extend`, `distinct`, `count`, `project-away`, `project-keep`, `project-rename`, `project-reorder`, `parse`, `parse-where`, `reduce`, `sample`, `sample-distinct`, `scan`, `search`, `serialize`, `top-nested`, `top-hitters` and `where`.

Operators like `join`, `union`, `external_data`, `plugins`, or any other operator that involves table source that isn't the subtable partitions, aren't allowed.

## Legacy strategy

Legacy subqueries can use the following sources:

* Implicit - The source is a tabular transformation that doesn't specify a tabular source. The source is implicit and will be assigned according to the subtable partitions. This scenario applies when there are 64 or less key values. 
* Explicit - The subquery must include a tabular source explicitly. Only the key column of the input table is available in the subquery, and referenced by using its name in the `toscalar()` function.

For both implicit and explicit sources, the subquery type is used for legacy purposes only, and indicated by the use of `hint.strategy=legacy`, or by not including any strategy indication. 

Any other reference to the source is taken to mean the entire input table, for example, by using the [as operator](asoperator.md) and calling up the value again.

> [!NOTE]
> It is recommended to use the native or shuffle strategies rather than the legacy strategy, since the legacy strategy is limited to 64 partitions and is less efficient.
> The legacy partition operator is currently limited by the number of partitions.
> The operator will yield an error if the partition column (*Column*) has more than 64 distinct values.

## All strategies

For native, shuffle and legacy subqueries, the result must be a single tabular result. Multiple tabular results and the use of the `fork` operator aren't supported. A subquery can't include other statements, for example, it can't have a `let` statement.

## Syntax

*T* `|` `partition` [`hint.strategy=` *strategy*] [*PartitionParameters*] `by` *Column* `(` *TransformationSubQuery* `)`

*T* `|` `partition` [*PartitionParameters*] `by` *Column* `{` *ContextFreeSubQuery* `}`

## Arguments

* *T*: The tabular source whose data is to be processed by the operator.
* *strategy*: The partition strategy, `native`, `shuffle` or `legacy`. `native` strategy is used with an implicit source with thousands of key partition values. `shuffle` strategy is used with an implicit source with millions of key partition values. `legacy` strategy is used with an explicit or implicit source with 64 or less key partition values. 
* *Column*: The name of a column in *T* whose values determine how the input table   is to be partitioned. See **Notes** below.
* *TransformationSubQuery*: A tabular transformation expression, whose source is implicitly the subtables produced by partitioning the records of *T*, each subtable being homogenous on the value of *Column*.
* *ContextFreeSubQuery*: A tabular expression that includes its own tabular source, such as a table reference. The expression can reference a single column from *T*, being the key column *Column* using the syntax `toscalar(`*Column*`)`.
* *PartitionParameters*: Zero or more (space-separated) parameters in the form of: <br>
  *HintName* `=` *Value* that control the behavior of the operator. The following hints are supported:

  |HintName               |Values         |Description|Native/Shuffle/Legacy strategy|
  |-------------------|---------------|-----------|----------|
  |`hint.strategy`|`legacy`, `shuffle`, `native`|Defines the execution strategy of the partition operator.|Native, Shuffle, Legacy|
  |`hint.shufflekey`|the partition key|Runs the partition operator in shuffle strategy where the shuffle key is the specified partition key.|Shuffle|
  |`hint.materialized`|`true`,`false` |If set to `true`, will materialize the source of the `partition` operator. The default value is `false`. |Legacy|
  |`hint.concurrency`|*Number*|Hints the system how many partitions to run in parallel. The default value is 16.|Legacy|
  |`hint.spread`|*Number*|Hints the system how to distribute the partitions among cluster nodes. For example, if there are N partitions and the spread hint is set to P, then the N partitions will be processed by P different cluster nodes equally in parallel/sequentially depending on the concurrency hint. The default value is 1.|Legacy|

## Returns

The operator returns a union of the results of the individual subqueries.

## Examples

### Native strategy examples

Use `hint.strategy=native` for this strategy. See the following examples:

This query returns foreach InjuriesDirect, the count of events and total injuries in each State that starts with 'W'.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| where State startswith 'W'
| partition hint.strategy=native by InjuriesDirect (summarize Events=count(), Injuries=sum(InjuriesDirect) by State);
```

**Output** 

|State|Events|Injuries|
|---|---|---|
|WISCONSIN|4|4|
|WYOMING|5|5|
|WEST VIRGINIA|1|1|
|WASHINGTON|2|2|
|WEST VIRGINIA|756|0|
|WYOMING|390|0|
|WASHINGTON|256|0|
|WISCONSIN|1845|0|
|WYOMING|1|4|
|WASHINGTON|1|5|
|WISCONSIN|1|2|
|WASHINGTON|1|2|
|WASHINGTON|1|10|


This query returns the top 1 EventType by total injuries for each State that starts with 'W':

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| where State startswith 'W'
| partition hint.strategy = native by State
(
    summarize TotalInjueries = sum(InjuriesDirect) by EventType
    | top 2 by TotalInjueries
)
```

**Output** 

|EventType|TotalInjueries|
|---|---|
|Tornado|4|
|Hail|1|
|Thunderstorm Wind|1|
|Excessive Heat|0|
|High Wind|13|
|Lightning|5|
|High Wind|5|
|Avalanche|3|

### Shuffle strategy example

Use `hint.strategy=shuffle` for this strategy. See the following example:

This query will return the top 3 DamagedProperty foreach EpisodeId, it returns also the columns EpisodeId and State.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| partition hint.strategy=shuffle by EpisodeId
(
    top 3 by DamageProperty
    | project EpisodeId, State, DamageProperty
)
| count
```

**Output** 

|Count|
|---|
|22345|

### Legacy strategy with explicit source

This strategy is for legacy purposes only, and indicated by the use of `hint.strategy=legacy` or by not including a strategy indication at all. See the following example:

This query will run two subqueries:
* When x == 1, the query will return all rows from StormEvents that has InjuriesIndirect == 1.
* When x == 2, the query will return all rows from StormEvents that has InjuriesIndirect == 2.

the final result is the union of these 2 subqueries.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
range x from 1 to 2 step 1
| partition hint.strategy=legacy by x { StormEvents | where x == InjuriesIndirect}
| count 
```

**Output** 

|Count|
|---|
|113|

### Partition operator

In some cases, it's more performant and easier to write a query using the `partition` operator than using the [`top-nested` operator](topnestedoperator.md). The following example runs a subquery calculating `summarize` and `top` for each of States starting with `W`: (WYOMING, WASHINGTON, WEST VIRGINIA, WISCONSIN)

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| where State startswith 'W'
| partition hint.strategy=native by State 
(
    summarize Events=count(), Injuries=sum(InjuriesDirect) by EventType, State
    | top 3 by Events 
) 
```

**Output** 

|EventType|State|Events|Injuries|
|---|---|---|---|
|Hail|WYOMING|108|0|
|High Wind|WYOMING|81|5|
|Winter Storm|WYOMING|72|0|
|Heavy Snow|WASHINGTON|82|0|
|High Wind|WASHINGTON|58|13|
|Wildfire|WASHINGTON|29|0|
|Thunderstorm Wind|WEST VIRGINIA|180|1|
|Hail|WEST VIRGINIA|103|0|
|Winter Weather|WEST VIRGINIA|88|0|
|Thunderstorm Wind|WISCONSIN|416|1|
|Winter Storm|WISCONSIN|310|0|
|Hail|WISCONSIN|303|1|

### Partition reference

The following example shows how to use the [as operator](asoperator.md) to give a "name" to each data partition and then reuse that name within the subquery. This approach is only relevant to the legacy strategy.

```kusto
T
| partition by Dim
(
    as Partition
    | extend MetricPct = Metric * 100.0 / toscalar(Partition | summarize sum(Metric))
)
```
