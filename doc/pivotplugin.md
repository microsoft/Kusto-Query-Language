---
title:  pivot plugin
description: Learn how to use the pivot plugin to rotate a table with specified columns and aggregates the remaining columns.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/12/2023
---
# pivot plugin

Rotates a table by turning the unique values from one column in the input table into multiple columns in the output table and performs aggregations as required on any remaining column values that will appear in the final output.

> [!NOTE]
> If the OutputSchema is not specified, the output schema of the `pivot` plugin is based on the input data. Therefore, multiple executions of the plugin using different data inputs, may produce different output schema. This also means that the query that is referencing unpacked columns may become 'broken' at any time. For this reason, we do not recommend using this plugin for automation jobs without specifying the OutputSchema function.

## Syntax

`T | evaluate pivot(`*pivotColumn*`[, `*aggregationFunction*`] [,`*column1* `[,`*column2* ... `]])` [`:` *OutputSchema*]

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *pivotColumn* | string | &check; | The column to rotate. Each unique value from this column will be a column in the output table.|
| *aggregationFunction* | string |  | An aggregation function used to aggregate multiple rows in the input table to a single row in the output table. Currently supported functions: `min()`, `max()`, `take_any()`, `sum()`, `dcount()`, `avg()`, `stdev()`, `variance()`, `make_list()`, `make_bag()`, `make_set()`, `count()`. The default is `count()`. |
| *column1*, *column2*, ... | string | | A column name or comma-separated list of column names. The output table will contain an additional column per each specified column. The default is all columns other than the pivoted column and the aggregation column. |
| *OutputSchema* | | | The names and types for the expected columns of the `pivot` plugin output.<br /><br />**Syntax**: `(` *ColumnName* `:` *ColumnType* [`,` ...] `)`<br /><br />Specifying the expected schema optimizes query execution by not having to first run the actual query to explore the schema. An error is raised if the run-time schema doesn't match the *OutputSchema* schema. |

## Returns

Pivot returns the rotated table with specified columns (*column1*, *column2*, ...) plus all unique values of the pivot columns. Each cell for the pivoted columns will contain the aggregate function computation.

## Examples

### Pivot by a column

For each EventType and State starting with 'AL', count the number of events of this type in this state.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKCjKz0pNLlEILkksSdVRAIuHVBakAqXKM1KLUiESCsUliUUlxeWZJRkKSo4+SnBZuHqFjMRiBaXwzLwUkGRqWWJOKUhfQWZZfokG2AxNABd5uxJ5AAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| project State, EventType
| where State startswith "AL"
| where EventType has "Wind"
| evaluate pivot(State)
```

**Output**

|EventType|ALABAMA|ALASKA|
|---|---|---|
|Thunderstorm Wind|352|1|
|High Wind|0|95|
|Extreme Cold/Wind Chill|0|10|
|Strong Wind|22|0|

### Pivot by a column with aggregation function

For each EventType and State starting with 'AR', display the total number of direct deaths.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKM9ILUpVCC5JLElVKC5JLCopLs8syVBQcgxSAsoWFOVnpSaXQOR1FMC6QioLgEyX1MSSjGKXzCKgNNwYZEEFOwUDoERqWWJOKcjwgsyy/BINqEHFpbkayIo1NQH4o/xAlgAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where State startswith "AR"
| project State, EventType, DeathsDirect
| where DeathsDirect > 0
| evaluate pivot(State, sum(DeathsDirect))
```

**Output**

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKM9ILUpVCC5JLElVKC5JLCopLs8syVBQcgxSAsoWFOVnpSaXQOR1FMC6QioLgEyX1MSSjGKXzCKgNNwYZEEFOwUDoERqWWJOKcjwgsyy/BINqEHFpbkayIo1kczWBAB7xf6goQAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where State startswith "AR"
| project State, EventType, DeathsDirect
| where DeathsDirect > 0
| evaluate pivot(State, sum(DeathsDirect), EventType)
```

**Output**

|EventType|ARKANSAS|ARIZONA|
|---|---|---|
|Heavy Rain|1|0|
|Thunderstorm Wind|1|0|
|Lightning|0|1|
|Flash Flood|0|6|
|Strong Wind|1|0|
|Heat|3|0|

### Specify the pivoted column, aggregation function, and multiple additional columns

For each event type, source, and state, sum the number of direct deaths.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA03NOwqAMBCE4d5TLFYKFp5AEPQCxgsEWUhAjexOIoKH91GI9Tc/YxBk6ROv0Oyk3bEwGVgwKaxAdw9HeTvkn3Zs4bTzwhOoofoGTnaOT7P5FFC8fUUal+I/Lit6j8Zju9WEKBOXFzKrwnaAAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| where State startswith "AR"
| where DeathsDirect > 0
| evaluate pivot(State, sum(DeathsDirect), EventType, Source)
```

**Output**

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

### Pivot with a query-defined output schema

The following example selects specific columns in the StormEvents table.
It uses an explicit schema definition that allows various optimizations to be evaluated before running the actual query.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKCjKz0pNLlEILkksSdVRAIuHVBakAqXKM1KLUhEiChmJxQpK4Zl5KUpAydSyxJxSoBaFgsyy/BINsHZNKwUNuHKr4pKizLx0HQVHH0cnR19Hq5x8KC/YG8LRBADkMGe5igAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| project State, EventType
| where EventType has "Wind"
| evaluate pivot(State): (EventType:string, ALABAMA:long, ALASKA:long)
```

**Output**

|EventType|ALABAMA|ALASKA|
|---|---|---|
|Thunderstorm Wind|352|1|
|High Wind|0|95|
|Marine Thunderstorm Wind|0|0|
|Strong Wind|22|0|
|Extreme Cold/Wind Chill|0|10|
|Cold/Wind Chill|0|0|
|Marine Strong Wind|0|0|
|Marine High Wind|0|0|
