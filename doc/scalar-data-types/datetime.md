---
title:  The datetime data type
description: This article describes The datetime data type in Azure Data Explorer.
ms.reviewer: orspodek
ms.topic: reference
ms.date: 10/23/2018
---
# The datetime data type

The `datetime` (`date`) data type represents an instant in time, typically expressed as a date and time of day.
Values range from 00:00:00 (midnight), January 1, 0001 Anno Domini (Common Era) through 11:59:59 P.M., December 31, 9999 A.D. (C.E.) in the Gregorian calendar. 

Time values are measured in 100-nanosecond units called ticks, and a particular date is the number of ticks since 12:00 midnight,
January 1, 0001 A.D. (C.E.) in the GregorianCalendar calendar (excluding ticks that would be added by leap seconds).
For example, a ticks value of 31241376000000000 represents the date, Friday, January 01, 0100 12:00:00 midnight.
This is sometimes called "a moment in linear time".

> [!WARNING]
> A `datetime` value in Kusto is always in the UTC time zone. If displaying `datetime` values 
> in other time zones is required, please use [datetime_utc_to_local()](../datetime-utc-to-local-function.md) 
> or its counterpart, [datetime_local_to_utc()](../datetime-local-to-utc-function.md), to convert to a different time zone.

## datetime literals

Literals of type `datetime` have the syntax `datetime(`*value*`)`, where a number of formats 
are supported for *value*, as indicated by the following table:

|Example                                                     |Value                                                         |
|------------------------------------------------------------|--------------------------------------------------------------|
|`datetime(2015-12-31 23:59:59.9)`<br/>`datetime(2015-12-31)`|Times are always in UTC. Omitting the date gives a time today.|
|`datetime(null)`                                            |See [null values](null-values.md).                            |

## The now() and ago() special functions

Kusto provides two special functions, [now() and ago()](../nowfunction.md),
to allow queries to reference the time at which the query starts execution.

## Supported formats

There are several formats for `datetime` that are supported as [datetime() literals](#datetime-literals)
and the [todatetime()](../todatetimefunction.md) function.

> [!WARNING]
> It is **strongly recommended** to use only the ISO 8601 formats.

### [ISO 8601](https://www.iso.org/iso/home/standards/iso8601.htm)

|Format|Example|
|------|-------|
|%Y-%m-%dT%H:%M:%s%z|2014-05-25T08:20:03.123456Z|
|%Y-%m-%dT%H:%M:%s|2014-05-25T08:20:03.123456|
|%Y-%m-%dT%H:%M|2014-05-25T08:20|
|%Y-%m-%d %H:%M:%s%z|2014-11-08 15:55:55.123456Z|
|%Y-%m-%d %H:%M:%s|2014-11-08 15:55:55|
|%Y-%m-%d %H:%M|2014-11-08 15:55|
|%Y-%m-%d|2014-11-08|

### [RFC 822](https://www.ietf.org/rfc/rfc0822.txt)

|Format|Example|
|------|-------|
|%w, %e %b %r %H:%M:%s %Z|Sat, 8 Nov 14 15:05:02 GMT|
|%w, %e %b %r %H:%M:%s|Sat, 8 Nov 14 15:05:02|
|%w, %e %b %r %H:%M|Sat, 8 Nov 14 15:05|
|%w, %e %b %r %H:%M %Z|Sat, 8 Nov 14 15:05 GMT|
|%e %b %r %H:%M:%s %Z|8 Nov 14 15:05:02 GMT|
|%e %b %r %H:%M:%s|8 Nov 14 15:05:02|
|%e %b %r %H:%M|8 Nov 14 15:05|
|%e %b %r %H:%M %Z|8 Nov 14 15:05 GMT|

### [RFC 850](https://tools.ietf.org/html/rfc850)

|Format|Example|
|------|-------|
|%w, %e-%b-%r %H:%M:%s %Z|Saturday, 08-Nov-14 15:05:02 GMT|
|%w, %e-%b-%r %H:%M:%s|Saturday, 08-Nov-14 15:05:02|
|%w, %e-%b-%r %H:%M %Z|Saturday, 08-Nov-14 15:05 GMT|
|%w, %e-%b-%r %H:%M|Saturday, 08-Nov-14 15:05|
|%e-%b-%r %H:%M:%s %Z|08-Nov-14 15:05:02 GMT|
|%e-%b-%r %H:%M:%s|08-Nov-14 15:05:02|
|%e-%b-%r %H:%M %Z|08-Nov-14 15:05 GMT|
|%e-%b-%r %H:%M|08-Nov-14 15:05|


### Sortable 

|Format|Example|
|------|-------|        
|%Y-%n-%e %H:%M:%s|2014-11-08 15:05:25|
|%Y-%n-%e %H:%M:%s %Z|2014-11-08 15:05:25 GMT|
|%Y-%n-%e %H:%M|2014-11-08 15:05|
|%Y-%n-%e %H:%M %Z|2014-11-08 15:05 GMT|
|%Y-%n-%eT%H:%M:%s|2014-11-08T15:05:25|
|%Y-%n-%eT%H:%M:%s %Z|2014-11-08T15:05:25 GMT|
|%Y-%n-%eT%H:%M|2014-11-08T15:05|
|%Y-%n-%eT%H:%M %Z|2014-11-08T15:05 GMT|
