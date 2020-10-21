---
title: pivot plugin - Azure Data Explorer
description: This article describes pivot plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# pivot plugin

Rotates a table by turning the unique values from one column in the input table into multiple columns
in the output table, and performs aggregations where they are required on any remaining column values 
that are wanted in the final output.

```kusto
T | evaluate pivot(PivotColumn)
```

> [!NOTE]
> The output schema of the `pivot` plugin is based on the data and therefore query may produce different schema for any two runs. This also means that query that is referencing unpacked columns may become 'broken' at any time. Due to this reason - it is not advised to use this plugin for automation jobs.

## Syntax

`T | evaluate pivot(`*pivotColumn*`[, `*aggregationFunction*`] [,`*column1* `[,`*column2* ... `]])`

## Arguments

* *pivotColumn*: The column to rotate. each unique value from this column will be a column in the output table.
* *aggregation function*: (optional) aggregates multiple rows in the input table to a single row in the output table. Currently supported functions: `min()`, `max()`, `any()`, `sum()`, `dcount()`, `avg()`, `stdev()`, `variance()`, `make_list()`, `make_bag()`, `make_set()`, `count()` (default is `count()`).
* *column1*, *column2*, ...: (optional) column names. The output table will contain an additional column per each specified column. default: all columns other than the pivoted column and the aggregation column.

## Returns

Pivot returns the rotated table with specified columns (*column1*, *column2*, ...) plus all unique values of the pivot columns. Each cell for the pivoted columns will contain the aggregate function computation.

## Examples

### Pivot by a column

For each EventType and States starting with 'AL', count the number of events of this type in this state.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents
| project State, EventType 
| where State startswith "AL" 
| where EventType has "Wind" 
| evaluate pivot(State)
```

|EventType|ALABAMA|ALASKA|
|---|---|---|
|Thunderstorm Wind|352|1|
|High Wind|0|95|
|Extreme Cold/Wind Chill|0|10|
|Strong Wind|22|0|


### Pivot by a column with aggregation function

For each EventType and States starting with 'AR', display the total number of direct deaths.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents 
| where State startswith "AR" 
| project State, EventType, DeathsDirect 
| where DeathsDirect > 0
| evaluate pivot(State, sum(DeathsDirect))
```

|EventType|ARKANSAS|ARIZONA|
|---|---|---|
|Heavy Rain|1|0|
|Thunderstorm Wind|1|0|
|Lightning|0|1|
|Flash Flood|0|6|
|Strong Wind|1|0|
|Heat|3|0|


### Pivot by a column with aggregation function and a single additional column

Result is identical to previous example.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents 
| where State startswith "AR" 
| project State, EventType, DeathsDirect 
| where DeathsDirect > 0
| evaluate pivot(State, sum(DeathsDirect), EventType)
```

|EventType|ARKANSAS|ARIZONA|
|---|---|---|
|Heavy Rain|1|0|
|Thunderstorm Wind|1|0|
|Lightning|0|1|
|Flash Flood|0|6|
|Strong Wind|1|0|
|Heat|3|0|


### Specify the pivoted column, aggregation function and multiple additional columns

For each event type, source and state, sum the number of direct deaths.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents 
| where State startswith "AR" 
| where DeathsDirect > 0
| evaluate pivot(State, sum(DeathsDirect), EventType, Source)
```

|EventType|Source|ARKANSAS|ARIZONA|
|---|---|---|---|
|Heavy Rain|Emergency Manager|1|0|
|Thunderstorm Wind|Emergency Manager|1|0|
|Lightning|Newspaper|0|1|
|Flash Flood|Trained Spotter|0|2|
|Flash Flood|Broadcast Media|0|3|
|Flash Flood|Newspaper|0|1|
|Strong Wind|Law Enforcement|1|0|
|Heat|Newspaper|3|0|
