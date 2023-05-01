---
title: sum() (aggregation function) - Azure Data Explorer
description: Learn how to use the sum() (aggregation function) function to calculate the sum of an expression across the group.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/05/2023
---
# sum() (aggregation function)

Calculates the sum of *expr* across the group.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`sum(`*expr*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr*  string | &check; | The expression used for the aggregation calculation. |

## Returns

Returns the sum value of *expr* across the group.

## Example

This example returns the total number of deaths by state.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVuCqUSguzc1NLMqsSlUAiznnl+aV2CaDSA1NHYWQ/JLEHJfUxJIM58Ti1GIFW5B6DbBAsUtmUWpyiaZCUqVCcEliSSrYtPyiEpAAmj4A7Xtp83QAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents 
| summarize EventCount=count(), TotalDeathCases = sum(DeathsDirect) by State 
| sort by TotalDeathCases
```

**Output**

The results table shown includes only the first 10 rows.

| State                | event_count | TotalDeathCases |
| -------------------- | ----------- | --------------- |
| TEXAS                | 4701        | 71              |
| FLORIDA              | 1042        | 57              |
| CALIFORNIA           | 898         | 48              |
| ILLINOIS             | 2022        | 29              |
| ALABAMA              | 1315        | 29              |
| MISSOURI             | 2016        | 20              |
| NEW YORK             | 1750        | 19              |
| KANSAS               | 3166        | 17              |
| GEORGIA              | 1983        | 17              |
| TENNESSEE            | 1125        | 17              |
| ...   | ... | ... |
