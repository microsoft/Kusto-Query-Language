---
title: facet operator - Azure Data Explorer
description: This article describes facet operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# facet operator

Returns a set of tables, one for each specified column.
Each table specifies the list of values taken by its column.
An additional table can be created by using the `with` clause.

## Syntax

*T* `| facet by` *ColumnName* [`, ` ...] [`with (` *filterPipe* `)`

## Arguments

* *ColumnName:* The name of column in the input, to be summarized as an output table.
* *filterPipe:* A query expression applied to the input table to produce one of the outputs.

## Returns

Multiple tables: one for the `with` clause, and one for each column.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| where State startswith "A" and EventType has "Heavy"
| facet by State, EventType
  with 
  (
  where StartTime between(datetime(2007-01-04) .. 7d) 
  | project StartTime, Source, EpisodeId, EventType
  | take 5
  )
```

|StartTime|Source|EpisodeId|EventType|
|---|---|---|---|
|2007-01-04 12:00:00.0000000|COOP Observer|2192|Heavy Snow|
|2007-01-04 15:00:00.0000000|Trained Spotter|2192|Heavy Snow|
|2007-01-04 15:00:00.0000000|Trained Spotter|2192|Heavy Snow|
|2007-01-04 15:00:00.0000000|Trained Spotter|2192|Heavy Snow|
|2007-01-06 18:00:00.0000000|COOP Observer|2193|Heavy Snow|

|State|count_State|
|---|---|
|ALABAMA|19|
|ARIZONA|33|
|ARKANSAS|1|
|AMERICAN SAMOA|1|
|ALASKA|58|

|EventType|count_EventType|
|---|---|
|Heavy Rain|34|
|Heavy Snow|78|
