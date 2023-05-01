---
title: where operator - Azure Data Explorer
description: Learn how to use the where operator to filter a table to the subset of rows that satisfy a predicate.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# where operator

Filters a table to the subset of rows that satisfy a predicate.

> The `where` and `filter` operators are equivalent

## Syntax

*T* `| where` *Predicate*

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *T* | string | &check; | Tabular input whose records are to be filtered. |
| *Predicate* | string | &check; | Expression that evaluates to a bool for each row in *T*.

## Returns

Rows in *T* for which *Predicate* is `true`.

> [!NOTE]
> All filtering functions return false when compared with null values. Use special null-aware functions to write queries that handle null values.
>
> * [isnull()](./isnullfunction.md)
> * [isnotnull()](./isnotnullfunction.md)
> * [isempty()](./isemptyfunction.md)
> * [isnotempty()](./isnotemptyfunction.md)

## Performance tips

* **Use simple comparisons** between column names and constants. ('Constant' means constant over the table - so `now()` and `ago()` are OK, and so are scalar values assigned using a [`let` statement](./letstatement.md).)

    For example, prefer `where Timestamp >= ago(1d)` to `where bin(Timestamp, 1d) == ago(1d)`.

* **Simplest terms first**: If you have multiple clauses conjoined with `and`, put first the clauses that involve just one column. So `Timestamp > ago(1d) and OpId == EventId` is better than the other way around.

For more information, see the summary of [available String operators](./datatypes-string-operators.md) and the summary of [available Numerical operators](./numoperators.md).

## Examples

### Order comparisons by complexity

The following query returns storm records that report damaged property, are floods, and start and end in different places.

Notice that we put the comparison between two columns last, as the where operator can't use the index and forces a scan.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKCjKz0pNLlFwScxNTE8NKMovSC0qqdRRACsIqSxI1VFwSk3PzPPJT04syczPA8rkpcA4QP3lGalFqWi6FewUDLgUgCAxLwVhkIKtrYKSW05+fooSXBLFaAVFW2TDAe7+E2GoAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| project DamageProperty, EventType, BeginLocation, EndLocation
| where DamageProperty > 0
    and EventType == "Flood"
    and BeginLocation != EndLocation 
```

The following table only shows the top 10 results. To see the full output, run the query.

|DamageProperty|EventType|BeginLocation|EndLocation|
|--|--|--|--|
|5000 |Flood|FAYETTE CITY LOWBER|
|5000 |Flood|MORRISVILLE WEST WAYNESBURG|
|10000|Flood|COPELAND HARRIS GROVE|
|5000 |Flood|GLENFORD MT PERRY|
|25000|Flood|EAST SENECA BUFFALO AIRPARK ARPT|
|20000|Flood|EBENEZER SLOAN|
|10000|Flood|BUEL CALHOUN|
|10000|Flood|GOODHOPE WEST MILFORD|
|5000 |Flood|DUNKIRK FOREST|
|20000|Flood|FARMINGTON MANNINGTON|

### Check if column contains string

The following query returns the rows in which the word "cow" appears in any column.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjPSC1KVdBSyEgsVlBKzi9XAgC3DyzDIAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where * has "cow"
```
