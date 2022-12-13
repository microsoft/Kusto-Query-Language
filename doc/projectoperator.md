---
title: Project operator - Azure Data Explorer
description: This article describes Project operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
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
| *T* | string | &check; | Tabular input for which to project certain columns. |
| *ColumnName* | string | | Name of column to appear in the output. |
| *Expression* | string | | Scalar expression to perform over the input. |

* Either *ColumnName* or *Expression* must be specified.
* If there is no *Expression*, then a column of *ColumnName* must appear in the input.
* If *ColumnName* is omitted, the output column name of *Expression* will be automatically generated.
* If *Expression* returns more than one column, a list of column names can be specified in parentheses. If a list of the column names is not specified, all *Expression*'s output columns with generated names will be added to the output.

> [!NOTE]
> It is possible but not recommended to return a new calculated column with the same name as an existing column in the input.

## Returns

A table with columns that were named as arguments. Contains same number of rows as the input table.

## Examples

### Only show specific columns

Only show the `EventId`, `State`, `EventType`, and `EpisodeNarrative` of the `StormEvents` table.

[**Run the query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSgoys9KTS5RAIt4pugoBJcklqTqQPghlQUgZkFmcX5Kql9iUVFiSWZZKgC3v2vmQgAAAA==)

```kusto
StormEvents
| project EventId, State, EventType, EpisodeNarrative
```

### Potential manipulations using project

```kusto
StormEvents
| project
    StartLocation = BeginLocation,                    // Rename column
    TotalDeaths = DeathsDirect + DeathsIndirect,      // Calculate a new column from two existing columns
```

## See also

* [`extend`](extendoperator.md)
* [series_stats](series-statsfunction.md)
