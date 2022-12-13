---
title: avg() (aggregation function) - Azure Data Explorer
description: Learn how to use the avg() function to calculate the average value of an expression.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/02/2022
---
# avg() (aggregation function)

Calculates the average (arithmetic mean) of *expr* across the group.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`avg(`*expr*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr* | string | &check; | The expression used for aggregation calculation. Records with `null` values are ignored and not included in the calculation. |

## Returns

Returns the average value of *expr* across the group.

## Example

This example returns the average number of damaged crops per state.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVXAsS3dJzE1MTw3Jdy7KLyhWsFVILEvXgIiBRTQVkioVgksSS1IBk8Ju20QAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize AvgDamageToCrops = avg(DamageCrops) by State
```

The results table shown includes only the first 10 rows.

| State                | AvgDamageToCrops |
| -------------------- | ---------------- |
| TEXAS                | 7524.569241      |
| KANSAS               | 15366.86671      |
| IOWA                 | 4332.477535      |
| ILLINOIS             | 44568.00198      |
| MISSOURI             | 340719.2212      |
| GEORGIA              | 490702.5214      |
| MINNESOTA            | 2835.991494      |
| WISCONSIN            | 17764.37838      |
| NEBRASKA             | 21366.36467      |
| NEW YORK             | 5.714285714      |
| ...      | ...   |
