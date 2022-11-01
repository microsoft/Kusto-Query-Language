---
title: avg() (aggregation function) - Azure Data Explorer
description: This article describes avg() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 07/05/2022
---
# avg() (aggregation function)

Calculates the average (arithmetic mean) of *Expr* across the group.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`avg` `(`*Expr*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | string | &check; | Expression used for aggregation calculation. Records with `null` values are ignored and not included in the calculation. |

## Returns

Returns the average value of *Expr* across the group.

## Example

This example returns the average number of damaged crops per state.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVXAsS3dJzE1MTw3Jdy7KLyhWsFVILEvXgIiBRTQVkioVgksSS1IBk8Ju20QAAAA=)**\]**

```kusto
StormEvents
| summarize AvgDamageToCrops = avg(DamageCrops) by State
```

**Results**

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
