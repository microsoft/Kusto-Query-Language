---
title: hll_merge() - Azure Data Explorer
description: This article describes hll_merge() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/18/2022
---
# hll_merge()

Merges `hll` results (scalar version of the aggregate version [`hll_merge()`](hll-merge-aggfunction.md)).

Read about the [underlying algorithm (*H*yper*L*og*L*og) and estimation accuracy](#estimation-accuracy).

## Syntax

`hll_merge(` *Expr1*`,` *Expr2*`, ...)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
|*Expr*|string|&check;|Columns that have `hll` values to be merged.|

## Returns

Returns one `hll` value which is the result of merging the columns `*Exrp1*`, `*Expr2*`, ... `*ExprN*`.

## Examples

This example shows the value of the merged columns.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA1WMQQ6DMAwE75X4wx6J6KE8gLdEiLi0iCTIGCmp+vg6gUtP1uzsmscwExKeHD16SET/wC60KTS3LygJBYeMQUudyhLuh/cjvz+E17rapE5vm8y9cr44m9LdOC40CTzxTO5UtkJbx9fmr+umeASx5ck5Mz9Hg5B+pwAAAA==" target="_blank">Run the query</a>

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

## Estimation accuracy

[!INCLUDE [data-explorer-estimation-accuracy](../../includes/data-explorer-estimation-accuracy.md)]
