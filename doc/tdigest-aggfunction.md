---
title:  tdigest() (aggregation function)
description: Learn how to use the tdigest() (aggregation function) function to calculate the intermediate results of the weighted percentiles of expressions across the group.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/20/2023
---
# tdigest() (aggregation function)

Calculates the intermediate results of [`percentiles()`](percentiles-aggfunction.md) across the group.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

For more information, see the [underlying algorithm (T-Digest) and the estimated error](percentiles-aggfunction.md#estimation-error-in-percentiles).

> [!IMPORTANT]
>The results of tdigest() and tdigest_merge() can be stored and later retrieved. For example, you may want to create daily percentiles summary, which can then be used to calculate weekly percentiles.
> However, the precise binary representation of these results may change over time. There's no guarantee that these functions will produce identical results for identical inputs, and therefore we don't advise relying on them.

## Syntax

`tdigest(`*expr* [`,` *weight*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr* | string | &check; | The expression used for the aggregation calculation. |
| *weight* | string | | The weights of the values for the aggregation calculation. |

## Returns

The Intermediate results of weighted percentiles of `*expr*` across the group.

> [!TIP]
>
>- Use the aggregation function [tdigest_merge()](tdigest-merge-aggfunction.md) to merge the output of `tdigest` again across another group.
>- Use the function [percentile_tdigest()](percentile-tdigestfunction.md) to calculate the percentile/percentilew of the `tdigest` results.

## Examples

This example shows the results of the tdigest percentiles sorted by state.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc6bc487453a064d3c9de.northeurope/databases/NewDatabase1?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSguzc1NLMqsSlUoSclMTy0u0XBJzE1MTw0oyi9ILSqp1FRIqlQILkksSQUAy2eq1DkAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize tdigest(DamageProperty) by State
```

The results table shown includes only the first 10 rows.

| State | tdigest_DamageProperty |
|--|--|
| NEBRASKA | [[7],[800,250,300000,5000,240000,1500000,20000,550000,0,75000,100000,1000,10000,30000,13000,2000000,1000000,650000,125000,35000,7000,2500000,4000000,450000,85000,460000,500000,6000,150000,350000,4000,72500,1200000,180000,400000,25000,50000,2000,45000,8000,120000,200000,40000,1200,15000,55000,3000,250000],[5,1,3,72,1,1,44,1,1351,12,24,17,46,13,6,1,2,1,2,6,8,1,1,1,2,1,4,2,6,1,2,2,1,1,2,26,18,12,2,2,1,7,6,4,28,4,6,6]] |
| MINNESOTA | [[7],[700,500,2000000,2500,1200000,12000000,16000,7000000,0,300000,425000,750,6000,30000,10000,22000000,10000000,9600000,600000,50000,4000,27000000,35000000,4000000,400000,5000000,6000000,3000,750000,2500000,2000,250000,11000000,38000000,3000000,20000,120000,1000,100000,5000,500000,1000000,60000,800,15000,200000,1500,1500000,900000],[1,3,1,3,1,2,1,1,1793,1,1,2,2,2,3,1,1,1,2,2,1,1,1,1,2,1,2,1,1,1,6,1,1,1,3,5,1,5,2,5,2,2,1,2,2,2,2,1,1]] |
| KANSAS | [[7],[667,200,6000000,3400,80000,300000,18875,210000,0,45857,750000,37500000,10000,81150000,15000000,6400000,2570000,225000,59400,25000,5000,400000,7000000,4500000,2500000,6500000,200000,4500,70000,122500,2785,12000000,1900000,18200000,150000,1150000,27000000,2000,30000,2000000,250000000,75000,26000,1500,1500000,1000000,2500,100000,21600000,50000,335000,600000,175000,500000,160000,51000,40000,20000,15000,252500,7520,350000,250000,3400000,1000,338000,16000000,106000,4840000,305000,540000,337500,9800000,45000,12500,700000,4000000,71000,30000000,35000,3700000,22000,56000],[12,2,2,5,2,3,8,1,2751,7,2,1,37,1,1,1,1,2,5,12,33,8,1,1,1,2,10,1,5,2,7,1,4,1,5,1,1,9,11,4,1,5,2,6,4,8,2,23,1,44,2,3,2,3,1,1,1,18,5,2,5,1,7,1,25,1,1,3,1,1,1,2,6,1,1,2,1,1,1,3,1,1,1]] |
| NEW MEXICO | [[7],[600,500,2500000,7000,1500,28000,40000,10000,0,500000,20000,1000,21000,70000,25000,3500000,200000,16500000,50000,100000,15000,4000,5000,2000],[1,3,1,1,1,1,1,7,466,1,7,4,1,1,2,1,1,1,1,2,1,4,10,8]] |
| KENTUCKY | [[7],[600,200,700000,5000,400000,12000,15000,100000,0,60000,80000,1000,9000,20000,10000,50000,30000,300000,120000,25000,7000,3000,500000,11500000,75000,35000,8000,6000,150000,1500000,4000,56000,1911,250000,2500000,18000,45000,2000],[6,2,1,42,1,3,9,8,999,2,1,52,1,21,37,25,7,2,3,14,11,35,1,1,6,10,9,10,4,1,13,1,9,3,1,2,1,37]] |
| VIRGINIA | [[7],[536,500,125000,3000,100000,7250,8000,60000,0,40000,50000,956,6000,11500,7000,25000,15000,98000,70000,12000,4000,2000,120000,1000000,45000,16000,5000,3500,75000,175000,2500,30000,1000,80000,300000,10000,20000,1500],[7,11,1,48,2,2,2,1,1025,2,6,9,2,2,1,5,16,1,3,5,12,122,1,1,1,1,64,2,2,1,1,7,209,3,2,42,19,6]] |
| OREGON | [[7],[5000,1000,60000,434000,20000,50000,100000,500000,0,1500000,20400000,6000,62600000],[8,2,1,1,1,1,3,1,401,1,1,1,1]] |
| ALASKA | [[7],[5000,1000,25000,700000,12060,15000,100000,1600000,0,10000],[5,1,1,1,1,2,1,2,242,1]] |
| CONNECTICUT | [[7],[5000,1000,2000000,0,50000,750000,6000],[1,1,1,142,1,1,1]] |
| NEVADA | [[7],[5000,1000,200000,1000000,30000,40000,297000,5000000,0,10000],[4,2,1,1,1,1,1,1,148,3]] |
