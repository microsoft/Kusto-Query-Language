---
title: arg_min() (aggregation function) - Azure Data Explorer
description: Learn how to use the arg_min() aggregation function to find a row in a group that minimizes the input expression.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/20/2022
---
# arg_min() (aggregation function)

Finds a row in the group that minimizes *ExprToMinimize*.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

> **Deprecated aliases:** argmin()

## Syntax

`arg_min` `(`*ExprToMinimize*`,` *\** | *ExprToReturn*  [`,` ...]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *ExprToMinimize*| string | &check; | The expression used for aggregation calculation. |
| *ExprToReturn* | string | &check; | The expression used for returning the value when *ExprToMinimize* is minimum. Use a wildcard (*) to return all columns of the input table. |
  
## Null handling

When *ExprToMinimize* is null for all rows in a group, one row in the group is picked. Otherwise, rows where *ExprToMinimize* is null are ignored.

## Returns

Returns a row in the group that minimizes *ExprToMinimize*, and the value of *ExprToReturn*. Use or `*` to return the entire row.

## Examples

Find the minimum latitude of a storm event in each state.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVuDlqlEoLs3NTSzKrEpVSCxKj8/NzNNwSk3PzPNJLNFRgLDykxNLMvPzNBWSKhWCSxJLUgG8tM4mQwAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents 
| summarize arg_min(BeginLat, BeginLocation) by State
```

The results table shown includes only the first 10 rows.

| State          | BeginLat | BeginLocation |
| -------------- | -------- | ------------- |
| AMERICAN SAMOA | -14.3    | PAGO PAGO     |
| CALIFORNIA     | 32.5709  | NESTOR        |
| MINNESOTA      | 43.5     | BIGELOW       |
| WASHINGTON     | 45.58    | WASHOUGAL     |
| GEORGIA        | 30.67    | FARGO         |
| ILLINOIS       | 37       | CAIRO         |
| FLORIDA        | 24.6611  | SUGARLOAF KEY |
| KENTUCKY       | 36.5     | HAZEL         |
| TEXAS          | 25.92    | BROWNSVILLE   |
| OHIO           | 38.42    | SOUTH PT      |
| ... | ... | ... |

Find the first time an event with a direct death happened in each state showing all of the columns.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSjPSC1KVXBJTSzJKHbJLEpNLlGwUzAAyRSX5uYmFmVWpSokFqXH52bmaQSXJBaVhGTmpuooaGkqJFUqAAVKUgHnoTY6UQAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where DeathsDirect > 0
| summarize arg_min(StartTime, *) by State
```

The results table shown includes only the first 10 rows and first 3 columns.

| State      | StartTime            | EndTime              | ... |
| ---------- | -------------------- | -------------------- | --- |
| INDIANA    | 2007-01-01T00:00:00Z | 2007-01-22T18:49:00Z | ... |
| FLORIDA    | 2007-01-03T10:55:00Z | 2007-01-03T10:55:00Z | ... |
| NEVADA     | 2007-01-04T09:00:00Z | 2007-01-05T14:00:00Z | ... |
| LOUISIANA  | 2007-01-04T15:45:00Z | 2007-01-04T15:52:00Z | ... |
| WASHINGTON | 2007-01-09T17:00:00Z | 2007-01-09T18:00:00Z | ... |
| CALIFORNIA | 2007-01-11T22:00:00Z | 2007-01-24T10:00:00Z | ... |
| OKLAHOMA   | 2007-01-12T00:00:00Z | 2007-01-18T23:59:00Z | ... |
| MISSOURI   | 2007-01-13T03:00:00Z | 2007-01-13T08:30:00Z | ... |
| TEXAS      | 2007-01-13T10:30:00Z | 2007-01-13T14:30:00Z | ... |
| ARKANSAS   | 2007-01-14T03:00:00Z | 2007-01-14T03:00:00Z | ... |
| ... | ... | ... | ... |

The following example demonstrates null handling.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA31QPQvCQAzdC/0PoVMrt+jYzQp2FQdBRORKQzlIcyW9UhR/vKlYFEHzhny9F5LUNigqwnQrgws59EEcNwY2nry80wNK7zzn4DhkcIojUEvWXUeYGEj2WKtbmu96KYisgapSHoiymVFYVkyUIxL58T/n55gdWpkIhfiRPzeYG7NypY1zHN2hH9rWirshWGkureP0dZqBRQbVFZ5/eADFBqWOFwEAAA==" target="_blank">Run the query</a>

```kusto
datatable(Fruit: string, Color: string, Version: int) [
    "Apple", "Red", 1,
    "Apple", "Green", int(null),
    "Banana", "Yellow", int(null),
    "Banana", "Green", int(null),
    "Pear", "Brown", 1,
    "Pear", "Green", 2,
]
| summarize arg_min(Version, *) by Fruit
```

**Output**

| Fruit | Version | Color |
|--|--|--|
| Apple | 1 | Red |
| Banana |  | Yellow |
| Pear | 1 | Brown |
