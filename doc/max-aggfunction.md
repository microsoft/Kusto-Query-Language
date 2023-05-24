---
title:  max() (aggregation function)
description: Learn how to use the max() function to find the maximum value of the expression in the group.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/05/2023
---
# max() (aggregation function)

Finds the maximum value the expression in the group.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`max(`*expr*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr* | string | &check; | The expression used for the aggregation calculation. |

## Returns

Returns the maximum value of *expr* across the group.

> [!TIP]
> This gives you the max on its own. If you want to see other columns in addition to the max, use [arg_max](arg-max-aggfunction.md).

## Example

This example returns the last record in a table.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSguzc1NLMqsSlXwSSxJLS4By9jmJlZoBJckFpWEZOamagIADGp6XTMAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize LatestEvent=max(StartTime)
```

**Output**

| LatestEvent |
|--|
| 2007-12-31T23:53:00Z |
