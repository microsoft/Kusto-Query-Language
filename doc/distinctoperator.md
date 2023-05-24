---
title:  distinct operator
description: Learn how to use the distinct operator to create a table with the distinct combination of the columns of the input table.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# distinct operator

Produces a table with the distinct combination of the provided columns of the input table.

## Syntax

*T* `| distinct` *ColumnName*`[,`*ColumnName2*`, ...]`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *ColumnName*| string | &check;| The column name to search for distinct values. |

> [!NOTE]
> The `distinct` operator supports providing an asterisk `*` as the group key to denote all columns, which is helpful for wide tables.

## Example

Shows distinct combination of states and type of events that led to over 45 direct injuries.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjPSC1KVfDMyyotykwtdsksSk0uUbBTMDEFSaZkFpdk5gEFgksSS1J1FMDaQioLUgH0ldkdRQAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where InjuriesDirect > 45
| distinct State, EventType
```

**Output**

|State|EventType|
|--|--|
|TEXAS|Winter Weather|
|KANSAS|Tornado|
|MISSOURI|Excessive Heat|
|OKLAHOMA|Thunderstorm Wind|
|OKLAHOMA|Excessive Heat|
|ALABAMA|Tornado|
|ALABAMA|Heat|
|TENNESSEE|Heat|
|CALIFORNIA|Wildfire|

## See also

If the group by keys are of high cardinalities, try `summarize by ...` with the [shuffle strategy](shufflequery.md).
