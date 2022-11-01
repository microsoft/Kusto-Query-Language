---
title: dcountif() (aggregation function) - Azure Data Explorer
description: This article describes dcountif() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 07/10/2022
---
# dcountif() (aggregation function)

Calculates an estimate of the number of distinct values of *Expr* of rows for which *Predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`dcountif` `(`*Expr*, *Predicate*, [`,` *Accuracy*]`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | string | &check; | Expression that will be used for aggregation calculation. |
| *Predicate* | string | &check; | Expression that will be used to filter rows. |
| *Accuracy* | int |  | Controls the balance between speed and accuracy. If unspecified, the default value is `1`. See [Estimation accuracy](#estimation-accuracy) for supported values. |

## Returns

Returns an estimate of the number of distinct values of *Expr* of rows for which *Predicate* evaluates to `true` in the group.

> [!TIP]
> `dcountif()` may return an error in cases where all, or none of the rows pass the `Predicate` expression.

## Example

This example shows how many types of fatal storm events happened in each state.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22MMQ6DMBAE+7ziShAUfAAqEyk1+cCBz8ISttH5AIHy+BhoKXc0O50Edu1KXuLrB3FxDtkeBMoaQ5zwGwWnW6j1EBYv1mTX/u4zlZkilDEqyzQIFHDPj9cXyJsqh36HTlAo9bcxNR/b0ECVhMCa+Hw8OX+LHx0UrAAAAA==)**\]**

```kusto
StormEvents
| summarize DifferentFatalEvents=dcountif(EventType,(DeathsDirect + DeathsIndirect)>0) by State
| where DifferentFatalEvents > 0
| order by DifferentFatalEvents 
```

**Results**

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
