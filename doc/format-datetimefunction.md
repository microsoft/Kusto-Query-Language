---
title: format_datetime() - Azure Data Explorer
description: This article describes format_datetime() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
ms.localizationpriority: high
---
# format_datetime()

Formats a datetime according to the provided format.

```kusto
format_datetime(datetime(2015-12-14 02:03:04.12345), 'y-M-d h:m:s.fffffff') == "15-12-14 2:3:4.1234500"
```

## Syntax

`format_datetime(`*datetime* `,` *format*`)`

## Arguments

* `datetime`: value of a type `datetime`.
* `format`: format specifier string, consisting of one or more [format elements](#supported-formats).

## Returns

The string with the format result.

## Supported formats

|Format specifier	|Description	|Examples
|---|---|---
|`d`	|The day of the month, from 1 through 31. |	2009-06-01T13:45:30 -> 1, 2009-06-15T13:45:30 -> 15
|`dd`	|The day of the month, from 01 through 31.|	2009-06-01T13:45:30 -> 01, 2009-06-15T13:45:30 -> 15
|`f`	|The tenths of a second in a date and time value. |2009-06-15T13:45:30.6170000 -> 6, 2009-06-15T13:45:30.05 -> 0
|`ff`	|The hundredths of a second in a date and time value. |2009-06-15T13:45:30.6170000 -> 61, 2009-06-15T13:45:30.0050000 -> 00
|`fff`	|The milliseconds in a date and time value. |6/15/2009 13:45:30.617 -> 617, 6/15/2009 13:45:30.0005 -> 000
|`ffff`	|The ten thousandths of a second in a date and time value. |2009-06-15T13:45:30.6175000 -> 6175, 2009-06-15T13:45:30.0000500 -> 0000
|`fffff`	|The hundred thousandths of a second in a date and time value. |2009-06-15T13:45:30.6175400 -> 61754, 2009-06-15T13:45:30.000005 -> 00000
|`ffffff`	|The millionths of a second in a date and time value. |2009-06-15T13:45:30.6175420 -> 617542, 2009-06-15T13:45:30.0000005 -> 000000
|`fffffff`	|The ten millionths of a second in a date and time value. |2009-06-15T13:45:30.6175425 -> 6175425, 2009-06-15T13:45:30.0001150 -> 0001150
|`F`	|If non-zero, the tenths of a second in a date and time value. |2009-06-15T13:45:30.6170000 -> 6, 2009-06-15T13:45:30.0500000 -> (no output)
|`FF`	|If non-zero, the hundredths of a second in a date and time value. |2009-06-15T13:45:30.6170000 -> 61, 2009-06-15T13:45:30.0050000 -> (no output)
|`FFF`	|If non-zero, the milliseconds in a date and time value. |2009-06-15T13:45:30.6170000 -> 617, 2009-06-15T13:45:30.0005000 -> (no output)
|`FFFF`	|If non-zero, the ten thousandths of a second in a date and time value. |2009-06-15T13:45:30.5275000 -> 5275, 2009-06-15T13:45:30.0000500 -> (no output)
|`FFFFF`	|If non-zero, the hundred thousandths of a second in a date and time value. |2009-06-15T13:45:30.6175400 -> 61754, 2009-06-15T13:45:30.0000050 -> (no output)
|`FFFFFF`	|If non-zero, the millionths of a second in a date and time value. |2009-06-15T13:45:30.6175420 -> 617542, 2009-06-15T13:45:30.0000005 -> (no output)
|`FFFFFFF`	|If non-zero, the ten millionths of a second in a date and time value. |2009-06-15T13:45:30.6175425 -> 6175425, 2009-06-15T13:45:30.0001150 -> 000115
|`h`	|The hour, using a 12-hour clock from 1 to 12. |2009-06-15T01:45:30 -> 1, 2009-06-15T13:45:30 -> 1
|`hh`	|The hour, using a 12-hour clock from 01 to 12. |2009-06-15T01:45:30 -> 01, 2009-06-15T13:45:30 -> 01
|`H`	|The hour, using a 24-hour clock from 0 to 23. |2009-06-15T01:45:30 -> 1, 2009-06-15T13:45:30 -> 13
|`HH`	|The hour, using a 24-hour clock from 00 to 23. |2009-06-15T01:45:30 -> 01, 2009-06-15T13:45:30 -> 13
|`m`	|The minute, from 0 through 59. |2009-06-15T01:09:30 -> 9, 2009-06-15T13:29:30 -> 29
|`mm`	|The minute, from 00 through 59. |2009-06-15T01:09:30 -> 09, 2009-06-15T01:45:30 -> 45
|`M`	|The month, from 1 through 12. |2009-06-15T13:45:30 -> 6
|`MM`	|The month, from 01 through 12.|2009-06-15T13:45:30 -> 06
|`s`	|The second, from 0 through 59. |2009-06-15T13:45:09 -> 9
|`ss`	|The second, from 00 through 59. |2009-06-15T13:45:09 -> 09
|`y`	|The year, from 0 to 99. |0001-01-01T00:00:00 -> 1, 0900-01-01T00:00:00 -> 0, 1900-01-01T00:00:00 -> 0, 2009-06-15T13:45:30 -> 9, 2019-06-15T13:45:30 -> 19
|`yy`	|The year, from 00 to 99. |	0001-01-01T00:00:00 -> 01, 0900-01-01T00:00:00 -> 00, 1900-01-01T00:00:00 -> 00, 2019-06-15T13:45:30 -> 19
|`yyyy`	|The year as a four-digit number. |	0001-01-01T00:00:00 -> 0001, 0900-01-01T00:00:00 -> 0900, 1900-01-01T00:00:00 -> 1900, 2009-06-15T13:45:30 -> 2009
|`tt`	|AM / PM hours |2009-06-15T13:45:09 -> PM

**Supported delimeters**

Format specifier can include following delimeters characters:

|Delimeter|Comment|
|---------|-------|
|`' '`| Space|
|`'/'`||
|`'-'`|Dash|
|`':'`||
|`','`||
|`'.'`||
|`'_'`||
|`'['`||
|`']'`||

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let dt = datetime(2017-01-29 09:00:05);
print 
v1=format_datetime(dt,'yy-MM-dd [HH:mm:ss]'), 
v2=format_datetime(dt, 'yyyy-M-dd [H:mm:ss]'),
v3=format_datetime(dt, 'yy-MM-dd [hh:mm:ss tt]')
```

|v1|v2|v3|
|---|---|---|
|17-01-29 [09:00:05]|2017-1-29 [9:00:05]|17-01-29 [09:00:05 AM]|
