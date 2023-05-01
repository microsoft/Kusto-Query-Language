---
title: Project operator - Azure Data Explorer
description: Learn how to use the project operator to select columns to include, rename or drop, and to insert new computed columns in the output table.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/12/2023
---
# project operator

Select the columns to include, rename or drop, and insert new computed columns.

The order of the columns in the result is specified by the order of the arguments. Only the columns specified in the arguments are included in the result. Any other columns in the input are dropped.

## Syntax

*T* `| project` [*ColumnName* | `(`*ColumnName*[`,`]`)` `=`] *Expression* [`,` ...]

or

*T* `| project` *ColumnName* [`=` *Expression*] [`,` ...]

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *T* | string | &check; | The tabular input for which to project certain columns. |
| *ColumnName* | string | | A column name or comma-separated list of column names to appear in the output. |
| *Expression* | string | | The scalar expression to perform over the input. |

* Either *ColumnName* or *Expression* must be specified.
* If there's no *Expression*, then a column of *ColumnName* must appear in the input.
* If *ColumnName* is omitted, the output column name of *Expression* will be automatically generated.
* If *Expression* returns more than one column, a list of column names can be specified in parentheses. If a list of the column names isn't specified, all *Expression*'s output columns with generated names will be added to the output.

> [!NOTE]
> It's not recommended to return a new calculated column with the same name as an existing column in the input.

## Returns

A table with columns that were named as arguments. Contains same number of rows as the input table.

## Examples

### Only show specific columns

Only show the `EventId`, `State`, `EventType` of the `StormEvents` table.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKCjKz0pNLlEAC3im6CgElySWpOpA+CGVBakAzXMiTy8AAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| project EventId, State, EventType
```

The results table shows only the top 10 results.

|EventId|State|EventType|
|--|--|--|
|61032| ATLANTIC SOUTH| Waterspout|
|60904| FLORIDA| Heavy Rain|
|60913| FLORIDA| Tornado|
|64588| GEORGIA| Thunderstorm Wind|
|68796| MISSISSIPPI| Thunderstorm Wind|
|68814| MISSISSIPPI| Tornado|
|68834| MISSISSIPPI| Thunderstorm Wind|
|68846| MISSISSIPPI| Hail|
|73241| AMERICAN SAMOA| Flash Flood|
|64725| KENTUCKY| Flood|
|...|...|...|

### Potential manipulations using project

The following query renames the `BeginLocation` column and creates a new column called `TotalInjuries` from a calculation over two existing columns.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwvJL0nM8czLKi3KTC1WsFWAMV0yi1KTSxS04QKeeSlgIQBwTr1bMQAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| project StartLocation = BeginLocation, TotalInjuries = InjuriesDirect + InjuriesIndirect
| where TotalInjuries > 5
```

The following table shows only the first 10 results.

|StartLocation| TotalInjuries|
|--|--|
|LYDIA| 15|
|ROYAL| 15|
|GOTHENBURG| 9|
|PLAINS| 8|
|KNOXVILLE| 9|
|CAROL STREAM| 11|
|HOLLY| 9|
|RUFFIN| 9|
|ENTERPRISE MUNI ARPT| 50|
|COLLIERVILLE| 6|
|...|...|

## See also

* [`extend`](extendoperator.md)
* [series_stats](series-statsfunction.md)
