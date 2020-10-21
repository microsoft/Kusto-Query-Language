---
title: unixtime_nanoseconds_todatetime() - Azure Data Explorer
description: This article describes unixtime_nanoseconds_todatetime() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 11/25/2019
---
# unixtime_nanoseconds_todatetime()

Converts unix-epoch nanoseconds to UTC datetime.

## Syntax

`unixtime_nanoseconds_todatetime(*nanoseconds*)`

## Arguments

* *nanoseconds*: A real number represents epoch timestamp in nanoseconds. `Datetime` that occurs before the epoch time (1970-01-01 00:00:00) has a negative timestamp value.

## Returns

If the conversion is successful, the result will be a [datetime](./scalar-data-types/datetime.md) value. If conversion is not successful, result will be null.

## See also

* Convert unix-epoch seconds to UTC datetime using [unixtime_seconds_todatetime()](unixtime-seconds-todatetimefunction.md).
* Convert unix-epoch milliseconds to UTC datetime using [unixtime_milliseconds_todatetime()](unixtime-milliseconds-todatetimefunction.md).
* Convert unix-epoch microseconds to UTC datetime using [unixtime_microseconds_todatetime()](unixtime-microseconds-todatetimefunction.md).

## Example

<!-- csl: https://help.kusto.windows.net/Samples  -->
```kusto
print date_time = unixtime_nanoseconds_todatetime(1546300800000000000)
```

|date_time|
|---|
|2019-01-01 00:00:00.0000000|
