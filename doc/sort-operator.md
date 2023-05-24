---
title:  sort operator
description: Learn how to use the sort operator to sort the rows of the input table by one or more columns.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/09/2023
---
# sort operator

Sorts the rows of the input table into order by one or more columns.

> The `sort` and `order` operators are equivalent

## Syntax

*T* `| sort by` *column* [`asc` | `desc`] [`nulls first` | `nulls last`] [`,` ...]

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular input to sort. |
| *column* | scalar | &check; | The column of *T* by which to sort. The type of the column values must be numeric, date, time or string.|
| `asc` or `desc` | string | | `asc` sorts into ascending order, low to high. Default is `desc`, high to low. |
| `nulls first` or `nulls last`  | string | | `nulls first` will place the null values at the beginning and `nulls last` will place the null values at the end. Default for `asc` is `nulls first`. Default for `desc` is `nulls last`.|

## Returns

A copy of the input table sorted in either ascending or descending order based on the provided column.

## Example

The following example shows storm events by state in alphabetical order with the most recent storms in each state appearing first.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRyC9KSS1SSKpUCC5JLElVSCxO1gExi0pCMnNTFVJSi5MBfa8LRzAAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| sort by State asc, StartTime desc
```

This table only shows the top 10 query results.

|StartTime|State|EventType|...|
|--|--|--|--|
|2007-12-28T12:10:00Z|ALABAMA|Hail|...|
|2007-12-28T04:30:00Z|ALABAMA|Hail|...|
|2007-12-28T04:16:00Z|ALABAMA|Hail|...|
|2007-12-28T04:15:00Z|ALABAMA|Hail|...|
|2007-12-28T04:13:00Z|ALABAMA|Hail|...|
|2007-12-21T14:30:00Z|ALABAMA|Strong Wind|...|
|2007-12-20T18:15:00Z|ALABAMA|Strong Wind|...|
|2007-12-20T18:00:00Z|ALABAMA|Strong Wind|...|
|2007-12-20T18:00:00Z|ALABAMA|Strong Wind|...|
|2007-12-20T17:45:00Z|ALABAMA|Strong Wind|...|
|2007-12-20T17:45:00Z|ALABAMA|Strong Wind|...|
