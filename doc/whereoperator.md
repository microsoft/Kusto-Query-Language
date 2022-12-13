---
title: where operator in Kusto query language - Azure Data Explorer
description: This article describes the where operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# where operator

Filters a table to the subset of rows that satisfy a predicate.

**Alias** `filter`

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

    For example, prefer `where Timestamp >= ago(1d)` to `where floor(Timestamp, 1d) == ago(1d)`.

* **Simplest terms first**: If you have multiple clauses conjoined with `and`, put first the clauses that involve just one column. So `Timestamp > ago(1d) and OpId == EventId` is better than the other way around.

For more information, see the summary of [available String operators](./datatypes-string-operators.md) and the summary of [available Numerical operators](./numoperators.md).

## Examples

### Order comparisons by complexity

Retrieve storm records that report damaged property, are floods, and start and end in different places.

Notice that we put the comparison between two columns last, as the where operator can't use the index and forces a scan.

[**Run the query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjPSC1KVXBJzE1MTw0oyi9ILSqpVLBTMODlUgCCxLwUBbDakMqCVAVbWwUlt5z8/BQlhKxTanpmnk9+cmJJZn6egqKtgmteCowLAAhN4ulrAAAA)

```kusto
StormEvents
| where DamageProperty > 0
    and EventType == "Flood"
    and BeginLocation != EndLocation 
```

### Check if column contains string

The following query returns the rows in which the word "cow" appears in any column.

[**Run the query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjPSC1KVdBSyEgsVlBKzi9XAgC3DyzDIAAAAA==)

```kusto
StormEvents
| where * has "cow"
```
