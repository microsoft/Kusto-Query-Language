---
title: tdigest_merge() - Azure Data Explorer
description: This article describes tdigest_merge() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/08/2022
---
# tdigest_merge()

Merges `tdigest` results (scalar version of the aggregate version [`tdigest_merge()`](tdigest-merge-aggfunction.md)).

Read more about the underlying algorithm (T-Digest) and the estimated error [here](percentiles-aggfunction.md#estimation-error-in-percentiles).

## Syntax

`tdigest_merge(` *Expr1*`,` *Expr2*`, ...)`

`merge_tdigest(` *Expr1*`,` *Expr2*`, ...)`

> [!NOTE]
> `merge_tdigest` is an alias of `tdigest_merge`. 

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | dynamic | &check; | Columns that have the `tdigest` values to be merged. |

## Returns

The result for merging the columns `*Expr1*`, `*Expr2*`, ... `*ExprN*` to one `tdigest`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02OSwrDMBBD9z3FLG2aRXKA3qNdhZAoxiX+YE/BDj18h7Ym2Wmk0UNp8gZUaE3B0UAcaOgpM6IclzehMPxClW7yc5VMvPxybkp2B/FiDTLfJf1LVXTX9ONkVy3FmMITM5NDMliOdPwaqtEOwLkUkWZ4thvGBv1xOlnVS6dGhFVtwRutP5Qlo27WAAAA" target="_blank">Run the query</a>

<!-- csl: https://help.kusto.windows.net/Samples -->
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
