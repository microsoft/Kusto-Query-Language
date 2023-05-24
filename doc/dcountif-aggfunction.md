---
title:  dcountif() (aggregation function)
description: Learn how to use the dcountif() function to return an estimate of the number of distinct values of an expression for rows where the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/16/2022
---
# dcountif() (aggregation function)

Estimates the number of distinct values of *expr* for rows in which *predicate* evaluates to `true`.
[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`dcountif` `(`*expr*, *predicate*, [`,` *accuracy*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr* | string | &check; | The expression used for the aggregation calculation. |
| *predicate* | string | &check; | The expression used to filter rows. |
| *accuracy* | int |  | The control between speed and accuracy. If unspecified, the default value is `1`. See [Estimation accuracy](#estimation-accuracy) for supported values. |

## Returns

Returns an estimate of the number of distinct values of *expr* for rows in which *predicate* evaluates to `true`.

> [!TIP]
> `dcountif()` may return an error in cases where all, or none of the rows pass the `Predicate` expression.

## Example

This example shows how many types of fatal storm events happened in each state.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22MMQ6DMBAE+7ziShAUfAAqEyk1+cCBz8ISttH5AIHy+BhoKXc0O50Edu1KXuLrB3FxDtkeBMoaQ5zwGwWnW6j1EBYv1mTX/u4zlZkilDEqyzQIFHDPj9cXyJsqh36HTlAo9bcxNR/b0ECVhMCa+Hw8OX+LHx0UrAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize DifferentFatalEvents=dcountif(EventType,(DeathsDirect + DeathsIndirect)>0) by State
| where DifferentFatalEvents > 0
| order by DifferentFatalEvents 
```

The results table shown includes only the first 10 rows.

| State          | DifferentFatalEvents |
| -------------- | -------------------- |
| CALIFORNIA     | 12                   |
| TEXAS          | 12                   |
| OKLAHOMA       | 10                   |
| ILLINOIS       | 9                    |
| KANSAS         | 9                    |
| NEW YORK       | 9                    |
| NEW JERSEY     | 7                    |
| WASHINGTON     | 7                    |
| MICHIGAN       | 7                    |
| MISSOURI       | 7                    |
| ... | ... |

## Estimation accuracy

[!INCLUDE [data-explorer-estimation-accuracy](../../includes/data-explorer-estimation-accuracy.md)]
