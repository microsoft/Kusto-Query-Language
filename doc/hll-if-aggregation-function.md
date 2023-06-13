---
title:  hll_if() (aggregation function)
description: Learn how to use the hll_if() function to calculate the intermediate results of the dcount() function.
ms.reviewer: ziham
ms.topic: reference
ms.date: 04/16/2023
---
# hll_if() (aggregation function)

Calculates the intermediate results of [`dcount`](dcount-aggfunction.md) in records for which the *predicate* evaluates to `true`.

Read about the [underlying algorithm (*H*yper*L*og*L*og) and the estimation accuracy](dcount-aggfunction.md#estimation-accuracy).

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

> [!IMPORTANT]
> The results of hll(), hll_if(), and hll_merge() can be stored and later retrieved. For example, you may want to create a daily unique users summary, which can then be used to calculate weekly counts.
> However, the precise binary representation of these results may change over time. There's no guarantee that these functions will produce identical results for identical inputs, and therefore we don't advise relying on them.

## Syntax

`hll_if` `(`*expr*, *predicate* [`,` *accuracy*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr* |  string | &check; | The expression used for the aggregation calculation. |
| *predicate* |  string | &check; | The *Expr* used to filter records to add to the intermediate result of `dcount`. |
| *accuracy* | int |   | The value that controls the balance between speed and accuracy. If unspecified, the default value is `1`. For supported values, see [Estimation accuracy](#estimation-accuracy). |

## Returns

Returns the intermediate results of distinct count of *Expr* for which *Predicate* evaluates to `true`.

> [!TIP]
>
> - You can use the aggregation function [`hll_merge`](hll-merge-aggfunction.md) to merge more than one `hll` intermediate result. Only works with `hll` output only.
> - You can use [`dcount_hll`](dcount-hllfunction.md), to calculate the distinct count from `hll`,`hll_merge`, or `hll_if` aggregation functions.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAz2OQQrCMBBF955imFUCuUIXXSiIYBcpuCw1ndBI2ylJqlQ8vDEBd/Ph/fdHR/bz8UlLDIcPvEbyBDr2kcAtIPDc3GpUgJf6qmuNMjFhm+feuzfBOE2dnZgHqPLtrNC8eUMKsrHdV4KqAjz9IJRw34s7WVbPDzKxZAWlFxqb0fJPsg6GtyV2SS7+Y/IL9SJmoLMAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| where State in ("IOWA", "KANSAS")
| summarize hll_flood = hll_if(Source, EventType == "Flood") by State
| project State, SourcesOfFloodEvents = dcount_hll(hll_flood)
```

|State|SourcesOfFloodEvents|
|---|---|
|KANSAS|11|
|IOWA|7|

## Estimation accuracy

| Accuracy | Speed | Error (%) |
|---|---|---|---|
| 0 | Fastest | 1.6 |
| 1 | Balanced | 0.8 |
| 2 | Slow | 0.4 |
| 3 | Slow | 0.28 |
| 4 | Slowest | 0.2 |
