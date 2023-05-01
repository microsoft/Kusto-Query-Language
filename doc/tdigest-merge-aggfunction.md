---
title: tdigest_merge() (aggregation functions) - Azure Data Explorer
description: Learn how to use the tdigest_merge() aggregation function to merge tdigest results across the group.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/20/2023
---
# tdigest_merge() (aggregation functions)

Merges tdigest results across the group.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

For more information about the underlying algorithm (T-Digest) and the estimated error, see [estimation error in percentiles](percentiles-aggfunction.md#estimation-error-in-percentiles).

> The `tdigest_merge()` and `merge_tdigest()` functions are equivalent

## Syntax

`tdigest_merge(`*expr*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*expr* | string | &check; | The expression used for the aggregation calculation.|

## Returns

Returns the merged tdigest values of *expr* across the group.

> [!NOTE]
>
> * Use the function [percentile_tdigest()](percentile-tdigestfunction.md) to calculate the percentiles from the `tdigest_merge` results.
> * All tdigests that are included in the same group must be of the same type.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVQgoSnVMT3dJzE1MTw0oyi9ILSqptC1JyUxPLS7RQBXWVEiqVAguSSxJRTEhN7UoPTUeqqVYA5uBmgCTdTq/fgAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize PreAggDamageProperty=tdigest(DamageProperty) by State
| summarize tdigest_merge(PreAggDamageProperty)
```

**Output**

|merge_tdigests_PreAggDamageProperty|
|---|
|[[7],[91,30,73667,966,110000000,24428,2500,20000,16500000,6292,40000,123208,1000000,133091,90583,20000000,977000,20007,547000,19000000,1221,9600000,300000,70072,55940,75000,417500,1410000,20400000,331500,15000000,62000000,50222,121690000,160400,6200000,252500,450,11000000,2200000,5700000,11566,12000000,263,50000,200000,3700000,13286,171000,100000000,28200000,65000000,17709,30693,16000000,7938,5200,2875,1500000,3480000,151100000,9800000,18200000,21600000,199,2570000,30000000,38000000,72000,891250,500000000,26385,80092,27000000,35000000,754500,11500000,3262500,113945,5000,62429,175294,9071,6500000,3321,15159,21850000,300000000,22683,3000,10000000,60055,600000,52000000,496000,15000,50000000,10140000,11900000,2100000,62600000,77125,310667,70000000,101000000,2088,1608571,19182,400000,179833,775000,612000,150000000,13500000,2600000,1250000,65400,45000000,297000,2500000,40000000,24846,30000,59067,1893,15762,142571,220666,195000,2000000,355000,2275000,6000000,46000000,38264,50857,4002,97333,27750,1000,1111429,7043,272500,455200,503,37500000,10000,1489,0,1200000,110538,60000000,250000,10730,1901429,291000,698750,649000,2716667,137000000,6400000,29286,41051,6850000,102000,4602,80000000,250000000,371667,8000000,729,8120000,5000000,20830,152400,803300,349667,202000,207000,81150000,48000000,750000,26000000,8900000,239143,75000000,248000,14342,74857,5992,500000,150000,938000,10533333,45248,105000000,7000000,35030,4000000,2000,7692500,3000000,25000000,4500000,87222,12054,100000,25000,9771,4840000,28000000,1307143,32024],[19,1,3,32,1,14,45,572,1,51,126,41,101,11,12,8,2,14,4,1,27,1,58,42,20,177,6,4,1,12,10,2,9,1,5,1,2,28,3,6,1,23,4,30,610,145,1,21,4,2,1,1,24,13,1,153,5,4,26,5,1,6,1,1,28,1,5,1,11,4,1,13,44,2,4,2,1,4,9,1672,7,17,47,2,39,17,2,1,17,666,16,71,21,3,1,530,10,1,1,2,1,4,6,4,1,20,7,11,40,6,2,1,1,2,1,3,5,2,1,21,2,13,271,3,14,23,7,15,2,41,1,2,7,1,27,7,205,3,4,1403,7,69,4,10,215,1,1472,127,45756,10,13,1,198,17,7,1,12,7,6,1,1,14,7,2,2,17,1,2,3,2,48,5,21,10,5,10,21,4,5,1,2,39,2,2,7,1,1,22,7,60,175,119,3,3,40,1,8,101,15,1135,4,22,3,3,9,76,430,611,12,1,2,7,8]]|
