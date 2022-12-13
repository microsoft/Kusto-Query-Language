---
title: dayofweek() - Azure Data Explorer
description: Learn how to use the dayofweek() function to return the 'timespan' since the preceding Sunday.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# dayofweek()

Returns the integer number of days since the preceding Sunday, as a `timespan`.

## Syntax

`dayofweek(`*date*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *date* | datetime | &check; | The datetime for which to determine the day of week.|

## Returns

The `timespan` since midnight at the beginning of the preceding Sunday, rounded down to an integer number of days.

## Examples

```kusto
dayofweek(datetime(1947-11-30 10:00:05))  // time(0.00:00:00), indicating Sunday
dayofweek(datetime(1970-05-11))           // time(1.00:00:00), indicating Monday
```
