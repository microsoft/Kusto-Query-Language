---
title: max() (aggregation function) - Azure Data Explorer
description: This article describes max() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 07/05/2022
---
# max() (aggregation function)

Finds the maximum value across the group.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`max` `(`*Expr*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr*  string | &check; | Expression used for aggregation calculation. |

## Returns

Returns the maximum value of *Expr* across the group.

> [!TIP]
> This gives you the max on its own. But if you want other columns in the rowuse [arg_max](arg-max-aggfunction.md).

## Example

This example returns the last record in a table.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVfBJLEktLgFL2OYmVmgElyQWlYRk5qZqAgAAp60yMgAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize LatestEvent=max(StartTime)
```

**Results**

| LatestEvent |
|--|
| 2007-12-31T23:53:00Z |
