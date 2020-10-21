---
title: tdigest_merge() - Azure Data Explorer
description: This article describes tdigest_merge() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 12/09/2019
---
# tdigest_merge()

Merges `tdigest` results (scalar version of the aggregate version [`tdigest_merge()`](tdigest-merge-aggfunction.md)).

Read more about the underlying algorithm (T-Digest) and the estimated error [here](percentiles-aggfunction.md#estimation-error-in-percentiles).

## Syntax

`merge_tdigests(` *Expr1*`,` *Expr2*`, ...)`

`tdigest_merge(` *Expr1*`,` *Expr2*`, ...)` - An alias.

## Arguments

* Columns that have the `tdigest` values to be merged.

## Returns

The result for merging the columns `*Expr1*`, `*Expr2*`, ... `*ExprN*` to one `tdigest`.

## Examples

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range x from 1 to 10 step 1 
| extend y = x + 10
| summarize tdigestX = tdigest(x), tdigestY = tdigest(y)
| project merged = tdigest_merge(tdigestX, tdigestY)
| project percentile_tdigest(merged, 100, typeof(long))
```

|percentile_tdigest_merged|
|---|
|20|
