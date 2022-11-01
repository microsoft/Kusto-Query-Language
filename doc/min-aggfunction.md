---
title: min() (aggregation function) - Azure Data Explorer
description: This article describes min() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 07/05/2022
---
# min() (aggregation function)

Finds the minimum value across the group.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`min` `(`*Expr*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr*  string | &check; | Expression used for aggregation calculation. |

## Returns

Returns the minimum value of *Expr* across the group.

> [!TIP]
> This gives you the min on its own. But if you want other columns in the row [arg_min](arg-min-aggfunction.md).

## Example

This example returns the first record in a table.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVXDLLCouAYvb5mbmaQSXJBaVhGTmpmoCAMaAOl8xAAAA)**\]**

```kusto
StormEvents
| summarize FirstEvent=min(StartTime)
```

**Results**

| FirstEvent |
|--|
| 2007-01-01T00:00:00Z |
