---
title: dcount() (aggregation function) - Azure Data Explorer
description: Learn how to use the dcount() function to return an estimate of the number of distinct values of an expression within a group.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# dcount() (aggregation function)

Calculates an estimate of the number of distinct values that are taken by a scalar expression in the summary group.

> [!NOTE]
> The `dcount()` aggregation function is primarily useful for estimating the cardinality of huge sets. It trades accuracy for performance, and may return a result that varies between executions. The order of inputs may have an effect on its output.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`dcount` `(`*expr*[`,` *accuracy*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr*| string | &check; | The input whose distinct values are to be counted. |
| *accuracy* | int |   | The value that defines the requested estimation accuracy. The default value is `1`. See [Estimation accuracy](#estimation-accuracy) for supported values. |

## Returns

Returns an estimate of the number of distinct values of *expr* in the group.

## Example

This example shows how many types of storm events happened in each state.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVXDJTEtLLQIKQ+RsU5LzS/NKNMC8kMqCVE2FpEqF4JLEklSgtvyilNQikACaLgBDbD8AXQAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize DifferentEvents=dcount(EventType) by State
| order by DifferentEvents
```

The results table shown includes only the first 10 rows.

| State                | DifferentEvents |
| -------------------- | --------------- |
| TEXAS                | 27              |
| CALIFORNIA           | 26              |
| PENNSYLVANIA         | 25              |
| GEORGIA              | 24              |
| ILLINOIS             | 23              |
| MARYLAND             | 23              |
| NORTH CAROLINA       | 23              |
| MICHIGAN             | 22              |
| FLORIDA              | 22              |
| OREGON               | 21              |
| KANSAS               | 21              |
| ... | ... |

## Estimation accuracy

[!INCLUDE [data-explorer-estimation-accuracy](../../includes/data-explorer-estimation-accuracy.md)]
