---
title: The timespan data type - Azure Data Explorer | Microsoft Docs
description: This article describes The timespan data type in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# The timespan data type

The `timespan` (`time`) data type represents a  time interval.

## timespan literals

Literals of type `timespan` have the syntax `timespan(`*value*`)`, where a number of formats 
are supported for *value*, as indicated by the following table:

|Value|Length of time|
---|---
`2d`|2 days
`1.5h`|1.5 hour
`30m`|30 minutes
`10s`|10 seconds
`0.1s`|0.1 second
`100ms`| 100 millisecond
`10microsecond`|10 microseconds
`1tick`|100ns
`time(15 seconds)`|15 seconds
`time(2)`| 2 days
`time(0.12:34:56.7)`|`0d+12h+34m+56.7s`

The special form `time(null)` is the [null value](null-values.md).

## timespan operators

Two values of type `timespan` may be added, subtracted, and divided.
The last operation returns a value of type `real` representing the
fractional number of times one value can fit the other.

## Examples

The following example calculates how many seconds are in a day in several ways:

```kusto
print
    result1 = 1d / 1s,
    result2 = time(1d) / time(1s),
    result3 = 24 * 60 * time(00:01:00) / time(1s)
```