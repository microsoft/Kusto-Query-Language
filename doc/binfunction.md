---
title:  bin()
description: Learn how to use the bin() function to round values down to an integer multiple of a given bin size. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/28/2023
adobe-target: true
---
# bin()

Rounds values down to an integer multiple of a given bin size.

Used frequently in combination with [`summarize by ...`](./summarizeoperator.md).
If you have a scattered set of values, they'll be grouped into a smaller set of specific values.

> The `bin()` and `floor()` functions are equivalent

## Syntax

`bin(`*value*`,`*roundTo*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* |  int, long, real, [timespan](scalar-data-types/timespan.md), or datetime | &check; | The value to round down. |
| *roundTo* |  int, long, real, or [timespan](scalar-data-types/timespan.md) | &check; | The "bin size" that divides *value*. |

## Returns

The nearest multiple of *roundTo* below *value*. Null values, a null bin size, or a negative bin size will result in null.

## Examples

### Numeric bin

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjKzNMw0TPVUTDUBACk1J4hEQAAAA==" target="_blank">Run the query</a>

```kusto
print bin(4.5, 1)
```

**Output**

|print_0|
|--|
|4|

### Timespan bin

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjKzNMoycxN1TA0S9HUUTBP0QQAELu46BgAAAA=" target="_blank">Run the query</a>

```kusto
print bin(time(16d), 7d)
```

**Output**

|print_0|
|--|
|14:00:00:00|

### Datetime bin

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUjKzNNISSxJLcnMTdUwtDQ30DUw1TU0VDA0tjIxtTIw19RRMEzRBADBlT+OLAAAAA==" target="_blank">Run the query</a>

```kusto
print bin(datetime(1970-05-11 13:45:07), 1d)
```

**Output**

|print_0|
|--|
|1970-05-11T00:00:00Z|

### Pad a table with null bins

When there are rows for bins with no corresponding row in the table, we recommend to pad the table with those bins. The following query looks at strong wind storm events in California for a week in April. However, there are no events on some of the days.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02OQQuCQBCF7/6KwYtKKRaBB9mDlIEQFRl0XtnBBHeVdUqMfnxbS9Ft4Pvee9MiQUlcEzAQnJAaib63jOMkjFdhnHhB6rTGyZUwhjVnkIjUKanTMr+josF5wnhFjZafTQVUSCOiAt9GoujdEPyLhMAYuOtsV2wPp32RucDNyKfxPPWWlqQ7VcOlUcI14eEmJdfNA+Goux41TRsueY3MAN+eXxBANUHVKP/30xwWIngBzQQjwPAAAAA=" target="_blank">Run the query</a>

```kusto
let Start = datetime('2007-04-07');
let End = Start + 7d;
StormEvents
| where StartTime between (Start .. End)
| where State == "CALIFORNIA" and EventType == "Strong Wind"
| summarize PropertyDamage=sum(DamageProperty) by bin(StartTime, 1d)
```

**Output**

|StartTime|PropertyDamage|
|--|--|
|2007-04-08T00:00:00Z|3000|
|2007-04-11T00:00:00Z|1000|
|2007-04-12T00:00:00Z|105000|

In order to represent the full week, the following query pads the result table with null values for the missing days. Here's a step-by-step explanation of the process:

1. Use the `union` operator to add more rows to the table.
1. The `range` operator produces a table that has a single row and column.
1. The `mv-expand` operator over the `range` function creates as many rows as there are bins between `StartTime` and `EndTime`.
1. Use a `PropertyDamage` of `0`.
1. The `summarize` operator groups together bins from the original table to the table produced by the `union` expression. This process ensures that the output has one row per bin whose value is either zero or the original count.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA12QQWuDQBCF7/6Kh5cojUFLIYfgIbQpBEpbaqDnFSdWyO7KOkm05Md3dJtQuodl4b33zZs9EKNg5Rg5KsXEjaZodp+myyR9SNLlLF4FB/FsTCUO77zDsloFBVunNycy3AUXnL/Ikdd3gkBJfCYyiHxksRgJ8V8jE/Ic4eP6Zfv89vG6XYdQMmQi7obWqwU7a2p8NqYKJXw0jRVmADlOmZrQY++sRga2cnVMLbJJvkCfEurbkXmrlU8h32k+Npojq+IxyzLR7qPrF8S/DOqZBPDubEuOhyelVU15OqnjNt1Ra+Wab/pvESHyz6sQoxxQNia6tZmG/wDAjjX+gQEAAA==" target="_blank">Run the query</a>

```kusto
let Start = datetime('2007-04-07');
let End = Start + 7d;
StormEvents
| where StartTime between (Start .. End)
| where State == "CALIFORNIA" and EventType == "Strong Wind"
| union (
    range x from 1 to 1 step 1
    | mv-expand StartTime=range(Start, End, 1d) to typeof(datetime)
    | extend PropertyDamage=0
    )
| summarize PropertyDamage=sum(DamageProperty) by bin(StartTime, 1d)
```

**Output**

|StartTime|PropertyDamage|
|--|--|
|2007-04-07T00:00:00Z|0|
|2007-04-08T00:00:00Z|3000|
|2007-04-09T00:00:00Z|0|
|2007-04-10T00:00:00Z|0|
|2007-04-11T00:00:00Z|1000|
|2007-04-12T00:00:00Z|105000|
|2007-04-13T00:00:00Z|0|
|2007-04-14T00:00:00Z|0|
