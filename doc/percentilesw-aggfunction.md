---
title: percentilew(), percentilesw() - Azure Data Explorer
description: Learn how to use the percentilew(), percentilesw() functions to calculate weighted percentiles in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/15/2023
---
# percentilew(), percentilesw() (aggregation function)

The `percentilew()` function calculates a weighted estimate for the specified [nearest-rank percentile](percentiles-aggfunction.md#nearest-rank-percentile) of the population defined by *expr*. `percentilesw()` works similarly to `percentilew()`. However, `percentilesw()` can calculate multiple weighted percentile values at once, which is more efficient than calculating each weighted percentile value separately.

Weighted percentiles calculate percentiles in a dataset by giving each value in the input dataset a weight. In this method, each value is considered to be repeated a number of times equal to its weight, which is then used to calculate the percentile. By giving more importance to certain values, weighted percentiles provide a way to calculate percentiles in a "weighted" manner.

To calculate unweighted percentiles, see [percentiles()](percentiles-aggfunction.md).

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`percentilew(`*expr*`,` *weightExpr*`,` *percentile*`)`

`percentilesw(`*expr*`,` *weightExpr*`,` *percentiles*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*expr* | string | &check; | The expression to use for aggregation calculation.|
|*percentile*| int or long | &check;| A constant that specifies the percentile.|
|*percentiles* | int or long | &check; | One or more comma-separated percentiles.|
|*weightExpr*|long|&check;|The weight to give each value.|

## Returns

Returns a table with the estimates for *expr* of the specified percentiles in the group, each in a separate column.

> [!NOTE]
> To return the percentiles in a single column, see [Return percentiles as an array](#return-percentiles-as-an-array).

## Examples

### Calculate weighted percentiles

Assume you repetitively measure the time (Duration) it takes an action to complete. Instead of recording every value of the measurement, you record each value of Duration, rounded to 100 msec, and how many times the rounded value appeared (BucketSize).

Use `summarize percentilesw(Duration, BucketSize, ...)` to calculate the given
percentiles in a "weighted" way. Treat each value of Duration as if it was repeated BucketSize times in the input, without actually needing to materialize those records.

The following example shows weighted percentiles.
Using the following set of latency values in milliseconds:
`{ 1, 1, 2, 2, 2, 5, 7, 7, 12, 12, 15, 15, 15, 18, 21, 22, 26, 35 }`.

To reduce bandwidth and storage, do pre-aggregation to the
following buckets: `{ 10, 20, 30, 40, 50, 100 }`. Count the number of events in each bucket to produce the following table:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVHISSxJzUuuDElMyklVsFVISSwBQhBbIyi10Dm/NK/EKic/L11HwQei0Kk0OTsVIqapwBWtwKUABBY6CoYGOhC2mY6CEYxtrKNgDGMb6iiYGChwxVpzIVsJAGDD8KqDAAAA" target="_blank">Run the query</a>

```kusto
let latencyTable = datatable (ReqCount:long, LatencyBucket:long) 
[ 
    8, 10, 
    6, 20, 
    3, 30, 
    1, 40 
];
latencyTable
```

The table displays:

* Eight events in the 10-ms bucket (corresponding to subset `{ 1, 1, 2, 2, 2, 5, 7, 7 }`)
* Six events in the 20-ms bucket (corresponding to subset `{ 12, 12, 15, 15, 15, 18 }`)
* Three events in the 30-ms bucket (corresponding to subset `{ 21, 22, 26 }`)
* One event  in the 40-ms bucket (corresponding to subset `{ 35 }`)

At this point, the original data is no longer available. Only the number of events in each bucket. To compute percentiles from this data, use the `percentilesw()` function.
For the 50, 75, and 99.9 percentiles, use the following query:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA1WOMQvCMBCF9/yKN7ZwSGutGsVFVydxE4cYDymmqbYJovjjjYSA3i0fj8d9Z9jBKMdWP/fqZBgrnJUL++Vsx/dN561bmM5eCNtYXHt95ZjlEAcIhJkTyoIiTwnjxBWhSlwSJgXEcSl+leKNwbet6psX48a9Zusaw8Mj+/MR0jeEOlyc1QQpRzL/AMC/VMrDAAAA" target="_blank">Run the query</a>

```kusto
let latencyTable = datatable (ReqCount:long, LatencyBucket:long) 
[ 
    8, 10, 
    6, 20, 
    3, 30, 
    1, 40 
];
latencyTable
| summarize percentilesw(LatencyBucket, ReqCount, 50, 75, 99.9)
```

**Output**

| percentile_LatencyBucket_50 | percentile_LatencyBucket_75 | percentile_LatencyBucket_99_9 |
|--|--|--|
| 20 | 20 | 40 |

## Return percentiles as an array

Instead of returning the values in individual columns, use the `percentilesw_array()` function to return the percentiles in a single column of dynamic array type.

### Syntax

`percentilesw_array(`*expr*`,` *weightExpr*`,` *percentiles*`)`

### Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*expr* | string | &check; | The expression to use for aggregation calculation.|
|*percentiles*| int, long, or dynamic | &check;| One or more comma-separated percentiles or a dynamic array of percentiles. Each percentile can be an integer or long value.|
|*weightExpr*|long|&check;|The weight to give each value.|

### Returns

Returns an estimate for *expr* of the specified percentiles in the group as a single column of dynamic array type.

### Examples

#### Comma-separated percentiles

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/SampleIoTData?query=H4sIAAAAAAAAA1WOQQuCQBCF7/sr3lFhCM2sLLrUtVN0i4hpG0Ja11pXwujHZ8hCzVw+Ho/5xoiHYS9Wd3s+G8EKF/b9fjnayWNTt9YvTG2vhO1QXLf6JkMWQx2g0M+ckCY08JQwDpwRssApYZJAHZfqV6neaNqqYle+BHdxWqwvjTTPEzvHXfRnJYSfCHl/d5YTimJUxB+W4nlIyQAAAA==" target="_blank">Run the query</a>

```kusto
let latencyTable = datatable (ReqCount:long, LatencyBucket:long) 
[ 
    8, 10, 
    6, 20, 
    3, 30, 
    1, 40 
];
latencyTable
| summarize percentilesw_array(LatencyBucket, ReqCount, 50, 75, 99.9)
```

**Output**

| percentile_LatencyBucket |
|---|---|---|
| [20, 20, 40] |

#### Dynamic array of percentiles

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/SampleIoTData?query=H4sIAAAAAAAAA1WOQQuCQBCF7/sr3lFhiMysLLrUtVN0E4lpHUJa11pXwujHZ4hQ8y4fjwffGPEw7MXq7sQXI9iiYN/ny8FRHvu6tX5tanslHIbhrtU3GboQKoNCfytCNKWBF4TZyDEhHjkizKdQ+Ub9KtUbTVtV7MqX4C5Oi/WlkeZ5Zue4C/6shPEnQtFZrkodZEkvWCaENJ2keRh+AIIR/2/UAAAA" target="_blank">Run the query</a>

```kusto
let latencyTable = datatable (ReqCount:long, LatencyBucket:long) 
[ 
    8, 10, 
    6, 20, 
    3, 30, 
    1, 40 
];
latencyTable
| summarize percentilesw_array(LatencyBucket, ReqCount, dynamic([50, 75, 99.9]))
```

**Output**

| percentile_LatencyBucket |
|---|---|---|
| [20, 20, 40] |
