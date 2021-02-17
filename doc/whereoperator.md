---
title: where operator in Kusto query language - Azure Data Explorer
description: This article describes the where operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
ms.localizationpriority: high
---
# where operator

Filters a table to the subset of rows that satisfy a predicate.

```kusto
T | where fruit=="apple"
```

**Alias** `filter`

## Syntax

*T* `| where` *Predicate*

## Arguments

* *T*: The tabular input whose records are to be filtered.
* *Predicate*: A `boolean` [expression](./scalar-data-types/bool.md) over the columns of *T*. It's evaluated for each row in *T*.

## Returns

Rows in *T* for which *Predicate* is `true`.

**Notes**
Null values: all filtering functions return false when compared with null values. 
You can use special null-aware functions to write queries that handle null values.

[isnull()](./isnullfunction.md),
[isnotnull()](./isnotnullfunction.md),
[isempty()](./isemptyfunction.md),
[isnotempty()](./isnotemptyfunction.md). 

**Tips**

To get the fastest performance:

* **Use simple comparisons** between column names and constants. ('Constant' means constant over the table - so `now()` and `ago()` are OK, and so are scalar values assigned using a [`let` statement](./letstatement.md).)

    For example, prefer `where Timestamp >= ago(1d)` to `where floor(Timestamp, 1d) == ago(1d)`.

* **Simplest terms first**: If you have multiple clauses conjoined with `and`, put first the clauses that involve just one column. So `Timestamp > ago(1d) and OpId == EventId` is better than the other way around.

For more information, see the summary of [available String operators](./datatypes-string-operators.md) and the summary of [available Numerical operators](./numoperators.md).

## Example: Simple comparisons first

```kusto
Traces
| where Timestamp > ago(1h)
    and Source == "MyCluster"
    and ActivityId == SubActivityId 
```

This example retrieves records that are no older than 1 hour,
come from a source called `MyCluster`, and have two columns of the same value. 

Notice that we put the comparison between two columns last, as it can't use the index and forces a scan.

## Example: Columns contain string

```kusto
Traces | where * has "Kusto"
```

All the rows in which the word "Kusto" appears in any column.
 
