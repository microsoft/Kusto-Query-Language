---
title: around() function - Azure Data Explorer
description: This article describes the around() function in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 05/05/2021
---
# around()

Returns a `bool` value indicating if the first argument is within a range around the center value.

## Syntax

`around(`*value*`,`*center*`,`*delta*`)`

## Arguments

* *value*: A number, date, or [timespan](scalar-data-types/timespan.md) that is evaluated.
* *center*: A number, date, or [timespan](scalar-data-types/timespan.md) representing the center of the range defined as [(`center`-`delta`) .. (`center` + `delta`)].
* *delta*: A number, or [timespan](scalar-data-types/timespan.md) representing the delta value of the range defined as [(`center`-`delta`) .. (`center` + `delta`)].

## Returns

Returns `true` if the value is within the range, `false` if the value is outside the range.
Returns `null` if any of the arguments is `null`.

## Example: Filtering values around a specific timestamp

The following example filters rows around specific timestamp.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
range dt 
    from datetime(2021-01-01 01:00) 
    to datetime(2021-01-01 02:00) 
    step 1min
| where around(dt, datetime(2021-01-01 01:30), 1min)
```

|dt|
|---|
|2021-01-01 01:29:00.0000000|
|2021-01-01 01:30:00.0000000|
|2021-01-01 01:31:00.0000000|
