---
title: make_timespan() - Azure Data Explorer | Microsoft Docs
description: This article describes make_timespan() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# make_timespan()

Creates a [timespan](./scalar-data-types/timespan.md) scalar value from the specified time period.

```kusto
make_timespan(1,12,30,55.123) == time(1.12:30:55.123)
```

## Syntax

`make_timespan(`*hour*,*minute*`)`

`make_timespan(`*hour*,*minute*,*second*`)`

`make_timespan(`*day*,*hour*,*minute*,*second*`)`

## Arguments

* *day*: day (a positive integer value)
* *hour*: hour (an integer value, from 0 to 23)
* *minute*: minute (an integer value, from 0 to 59)
* *second*: second (a real value, from 0 to 59.9999999)

## Returns

If creation is successful, result will be a [timespan](./scalar-data-types/timespan.md) value, otherwise, result will be null.
 
## Example

```kusto
print ['timespan'] = make_timespan(1,12,30,55.123)

```

|timespan|
|---|
|1.12:30:55.1230000|


