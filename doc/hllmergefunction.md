---
title: hll_merge() - Azure Data Explorer
description: This article describes hll_merge() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 04/15/2019
---
# hll_merge()

Merges `hll` results (scalar version of the aggregate version [`hll_merge()`](hll-merge-aggfunction.md)).

Read about the [underlying algorithm (*H*yper*L*og*L*og) and estimation accuracy](dcount-aggfunction.md#estimation-accuracy).

## Syntax

`hll_merge(` *Expr1*`,` *Expr2*`, ...)`

## Arguments

* Columns that have `hll` values to be merged.

## Returns

The result for merging the columns `*Exrp1*`, `*Expr2*`, ... `*ExprN*` to one `hll` value.

## Examples

<!-- csl: https://help.kusto.windows.net:443/KustoMonitoringPersistentDatabase -->
```kusto
range x from 1 to 10 step 1 
| extend y = x + 10
| summarize hll_x = hll(x), hll_y = hll(y)
| project merged = hll_merge(hll_x, hll_y)
| project dcount_hll(merged)
```

|`dcount_hll_merged`|
|---|
|20|
