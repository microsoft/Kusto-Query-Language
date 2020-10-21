---
title: sample operator - Azure Data Explorer
description: This article describes sample operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/18/2020
---
# sample operator

Returns up to the specified number of random rows from the input table.

```kusto
T | sample 5
```

> [!NOTE]
> * `sample` is geared for speed rather than even distribution of values. Specifically, it means that it will not produce 'fair' results if used after operators that union 2 data sets of different sizes (such as a `union` or `join` operators). It's recommended to use `sample` right after the table reference and filters.
> * `sample` is a non-deterministic operator, and will return different result set each time it is evaluated during the query. For example, the following query yields two different rows (even if one would expect to return the same row twice).

## Syntax

*T* `| sample` *NumberOfRows*

## Arguments

* *NumberOfRows*: The number of rows of *T* to return. You can specify any numeric expression.

## Examples

```kusto
let _data = range x from 1 to 100 step 1;
let _sample = _data | sample 1;
union (_sample), (_sample)
```

| x   |
| --- |
| 83  |
| 3   |

To ensure that in example above `_sample` is calculated once, one can use [materialize()](./materializefunction.md) function:

```kusto
let _data = range x from 1 to 100 step 1;
let _sample = materialize(_data | sample 1);
union (_sample), (_sample)
```

| x   |
| --- |
| 34  |
| 34  |

To sample a certain percentage of your data (rather than a specified number of rows), you can use

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents | where rand() < 0.1
```

To sample keys rather than rows (for example - sample 10 Ids and get all rows for these Ids) you can use [`sample-distinct`](./sampledistinctoperator.md) in combination with the `in` operator.


<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let sampleEpisodes = StormEvents | sample-distinct 10 of EpisodeId;
StormEvents
| where EpisodeId in (sampleEpisodes)
```
