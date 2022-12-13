---
title: hll_merge() (aggregation function) - Azure Data Explorer
description: Learn how to use the hll_merge() aggregation function to merge HLL results into a single HLL value in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/20/2022
---
# hll_merge() (aggregation function)

Merges `HLL` results across the group into a single `HLL` value.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

For more information, see the [underlying algorithm (*H*yper*L*og*L*og) and estimation accuracy](#estimation-accuracy).

## Syntax

`hll_merge` `(`*Expr*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
|*Expr*|string|&check;|Expression that will be used for aggregation calculation.|

## Returns

The function returns the merged `hll` values of `*Expr*` across the group.

> [!TIP]
> Use the function [dcount_hll](dcount-hllfunction.md) to calculate the `dcount` from `hll` / `hll-merge` aggregation functions.

## Example

The following example shows `HLL` results across a group merged into a single `HLL` value.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVcjIyQlKLVawBTE0XBJzE9NTA4ryC1KLSio1FZIqFZIy8zSCSxKLSkIyc1N1DA1yNdG1+6YWpaemQEyIzwVxNCCGagIAlijQ1HQAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize hllRes = hll(DamageProperty) by bin(StartTime,10m)
| summarize hllMerged = hll_merge(hllRes)
```

**Results**

The results show only the first 5 results in the array.

|hllMerged|
|--|
| [[1024,14],["-6903255281122589438","-7413697181929588220","-2396604341988936699","5824198135224880646","-6257421034880415225", ...]|

## Estimation accuracy

[!INCLUDE [data-explorer-estimation-accuracy](../../includes/data-explorer-estimation-accuracy.md)]
