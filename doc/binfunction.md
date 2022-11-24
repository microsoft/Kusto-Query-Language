---
title: bin() - Azure Data Explorer
description: Learn how to use the bin() function to round values down to an integer multiple of a given bin size. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
adobe-target: true
---
# bin()

Rounds values down to an integer multiple of a given bin size.

Used frequently in combination with [`summarize by ...`](./summarizeoperator.md).
If you have a scattered set of values, they'll be grouped into a smaller set of specific values.

Alias to `floor()` function.

## Syntax

`bin(`*value*`,`*roundTo*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* |  int, long, real, timespan, or datetime | &check; | The value to round down. |
| *roundTo* |  int, long, real, or timespan | &check; | The "bin size" that divides *value*. |

## Returns

The nearest multiple of *roundTo* below *value*. Null values, a null bin size, or a negative bin size will result in null.

## Examples

Expression | Result
---|---
`bin(4.5, 1)` | `4.0`
`bin(time(16d), 7d)` | `14d`
`bin(datetime(1970-05-11 13:45:07), 1d)`|  `datetime(1970-05-11)`

The following expression calculates a histogram of durations,
with a bucket size of 1 second:

```kusto
T | summarize Hits=count() by bin(Duration, 1s)
```
