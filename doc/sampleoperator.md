---
title: sample operator - Azure Data Explorer | Microsoft Docs
description: This article describes sample operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 12/07/2018
---
# sample operator

Returns up to the specified number of random rows from the input table.

```kusto
T | sample 5
```

**Syntax**

*T* `| sample`  *NumberOfRows*

**Arguments**
* *NumberOfRows*: The number of rows of *T* to return. You can specify any numeric expression.

**Remarks**

`sample` is a non-deterministic operator, and will return different result set each time it is evaluated during the query. For example, the following query yields two different rows (even if one would expect to return the same row twice).

```kusto
let _data = range x from 1 to 100 step 1;
let _sample = _data | sample 1;
union (_sample), (_sample)
```

|x|
|---|
|83|
|3|

In order to ensure that in example above `_sample` is calculated once, one can use [materialize()](./materializefunction.md) function:

```kusto
let _data = range x from 1 to 100 step 1;
let _sample = materialize(_data | sample 1);
union (_sample), (_sample)
```

|x|
|---|
|34|
|34|

**Tips**

* if you want to sample a certain percentage of your data (rather than a specified number of rows), you can use 

```kusto
StormEvents | where rand() < 0.1
```

* If you want to sample keys rather than rows (for example - sample 10 Ids and get all rows for these Ids) you can use [`sample-distinct`](./sampledistinctoperator.md) in combination with the `in` operator.

**Examples**  

```kusto
let sampleEpisodes = StormEvents | sample-distinct 10 of EpisodeId;
StormEvents
| where EpisodeId in (sampleEpisodes)
```