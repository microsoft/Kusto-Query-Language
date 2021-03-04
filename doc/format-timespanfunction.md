---
title: format_timespan() - Azure Data Explorer
description: This article describes format_timespan() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# format_timespan()

Formats a timespan according to the provided format.

```kusto
format_timespan(time(14.02:03:04.12345), 'h:m:s.fffffff') == "2:3:4.1234500"
```

## Syntax

`format_timespan(`*timespan* `,` *format*`)`

## Arguments

* `timespan`: value of a type `timespan`.
* `format`: format specifier string, consisting of one or more [format elements](#supported-formats).

## Returns

The string with the format result.

## Supported formats

|Format specifier	|Description	|Examples
|---|---|---
|`d`-`dddddddd`	|The number of whole days in the time interval. Padded with zeros if needed.|	15.13:45:30: d -> 15, dd -> 15, ddd -> 015
|`f`	|The tenths of a second in the time interval. |15.13:45:30.6170000 -> 6, 15.13:45:30.05 -> 0
|`ff`	|The hundredths of a second in the time interval. |15.13:45:30.6170000 -> 61, 15.13:45:30.0050000 -> 00
|`fff`	|The milliseconds in the time interval. |6/15/2009 13:45:30.617 -> 617, 6/15/2009 13:45:30.0005 -> 000
|`ffff`	|The ten thousandths of a second in the time interval. |15.13:45:30.6175000 -> 6175, 15.13:45:30.0000500 -> 0000
|`fffff`	|The hundred thousandths of a second in the time interval. |15.13:45:30.6175400 -> 61754, 15.13:45:30.000005 -> 00000
|`ffffff`	|The millionths of a second in the time interval. |15.13:45:30.6175420 -> 617542, 15.13:45:30.0000005 -> 000000
|`fffffff`	|The ten millionths of a second in the time interval. |15.13:45:30.6175425 -> 6175425, 15.13:45:30.0001150 -> 0001150
|`F`	|If non-zero, the tenths of a second in the time interval. |15.13:45:30.6170000 -> 6, 15.13:45:30.0500000 -> (no output)
|`FF`	|If non-zero, the hundredths of a second in the time interval. |15.13:45:30.6170000 -> 61, 15.13:45:30.0050000 -> (no output)
|`FFF`	|If non-zero, the milliseconds in the time interval. |15.13:45:30.6170000 -> 617, 15.13:45:30.0005000 -> (no output)
|`FFFF`	|If non-zero, the ten thousandths of a second in the time interval. |15.13:45:30.5275000 -> 5275, 15.13:45:30.0000500 -> (no output)
|`FFFFF`	|If non-zero, the hundred thousandths of a second in the time interval. |15.13:45:30.6175400 -> 61754, 15.13:45:30.0000050 -> (no output)
|`FFFFFF`	|If non-zero, the millionths of a second in the time interval. |15.13:45:30.6175420 -> 617542, 15.13:45:30.0000005 -> (no output)
|`FFFFFFF`	|If non-zero, the ten millionths of a second in the time interval. |15.13:45:30.6175425 -> 6175425, 15.13:45:30.0001150 -> 000115
|`H`	|The hour, using a 24-hour clock from 0 to 23. |15.01:45:30 -> 1, 15.13:45:30 -> 13
|`HH`	|The hour, using a 24-hour clock from 00 to 23. |15.01:45:30 -> 01, 15.13:45:30 -> 13
|`m`	|The number of whole minutes in the time interval that are not included as part of hours or days. Single-digit minutes do not have a leading zero. |15.01:09:30 -> 9, 15.13:29:30 -> 29
|`mm`	|The number of whole minutes in the time interval that are not included as part of hours or days. Single-digit minutes have a leading zero. |15.01:09:30 -> 09, 15.01:45:30 -> 45
|`s`	|The number of whole seconds in the time interval that are not included as part of hours, days, or minutes. Single-digit seconds do not have a leading zero. |15.13:45:09 -> 9
|`ss`	|The number of whole seconds in the time interval that are not included as part of hours, days, or minutes. Single-digit seconds have a leading zero. |15.13:45:09 -> 09

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
let t = time(29.09:00:05.12345);
print 
v1=format_timespan(t, 'dd.hh:mm:ss:FF'),
v2=format_timespan(t, 'ddd.h:mm:ss [fffffff]')
```

|v1|v2|
|---|---|
|29.09:00:05:12|029.9:00:05 [1234500]|
