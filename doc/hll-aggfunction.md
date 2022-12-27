---
title: hll() (aggregation function) - Azure Data Explorer
description: Learn how to use the hll() function to calculate the results of the dcount() function.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/19/2022
---
# hll() (aggregation function)

Calculates the intermediate results of [`dcount`](dcount-aggfunction.md) across the group only in context of aggregation inside [summarize](summarizeoperator.md).

Read about the [underlying algorithm (*H*yper*L*og*L*og) and the estimation accuracy](#estimation-accuracy).

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`hll` `(`*Expr* [`,` *Accuracy*]`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* |  string | &check; | Expression used for the aggregation calculation. |
| *Accuracy* |   |   | Controls the balance between speed and accuracy. If unspecified, the default value is `1`. For supported values, see [Estimation accuracy](#estimation-accuracy). |

## Returns

Returns the intermediate results of distinct count of *`Expr`* across the group.

> [!TIP]
>
>- You may use the aggregation function [`hll_merge`](hll-merge-aggfunction.md) to merge more than one `hll` intermediate results (it works on `hll` output only).
>- You may use the function [`dcount_hll`](dcount-hllfunction.md), which will calculate the `dcount` from `hll` / `hll_merge` aggregation functions.

## Example

The following example returns the hll results of property damage based on the start time.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVcjIydFwScxNTE8NKMovSC0qqdRUSKpUSMrM0wguSSwqCcnMTdUxNMjVBACCSG7CQQAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize hll(DamageProperty) by bin(StartTime,10m)
```

The results table shown includes only the first 10 rows.

| StartTime | hll_DamageProperty |
|--|--|
| 2007-01-01T00:20:00Z | [[1024,14],["3803688792395291579"],[]] |
| 2007-01-01T01:00:00Z | [[1024,14],["7755241107725382121","-5665157283053373866","3803688792395291579","-1003235211361077779"],[]] |
| 2007-01-01T02:00:00Z | [[1024,14],["-1003235211361077779","-5665157283053373866","7755241107725382121"],[]] |
| 2007-01-01T02:20:00Z  | [[1024,14],["7755241107725382121"],[]] |
| 2007-01-01T03:30:00Z  | [[1024,14],["3803688792395291579"],[]] |
| 2007-01-01T03:40:00Z | [[1024,14],["-5665157283053373866"],[]] |
| 2007-01-01T04:30:00Z | [[1024,14],["3803688792395291579"],[]] |
| 2007-01-01T05:30:00Z | [[1024,14],["3803688792395291579"],[]] |
| 2007-01-01T06:30:00Z | [[1024,14],["1589522558235929902"],[]] |

## Estimation accuracy

[!INCLUDE [data-explorer-estimation-accuracy](../../includes/data-explorer-estimation-accuracy.md)]
