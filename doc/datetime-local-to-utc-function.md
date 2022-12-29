---
title: datetime_local_to_utc() - Azure Data Explorer
description: Learn how to use the datetime_local_to_utc() function to convert local datetime to UTC datetime.
ms.reviewer: elgevork
ms.topic: reference
ms.date: 11/28/2022
---
# datetime_local_to_utc()

Converts local datetime to UTC datetime using [a time-zone specification](timezone.md).

## Syntax

`datetime_local_to_utc(`*from*`,`*timezone*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *from* | datetime | &check; | The local datetime to convert.|
| *timezone* | string | &check; | The timezone of the desired datetime. The value must be one of the supported [timezones](timezone.md).|

## Returns

A UTC [datetime](./scalar-data-types/datetime.md) that corresponds the local [datetime](./scalar-data-types/datetime.md) in the specified `timezone`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA42PTQsCIRCG7/6KubmCseJR6BDRPYhOEWI6bYK7hjsLEf34jMBrMS9zmI935gmOalwSdil7l2wgA8ERUhxRAj0NzFTiNAh2avVOK61WSleBVkZpo5WQwI+Hfu98vEbPJTD4Ob8ZsUTv+u2t5iH/t7RbSr5jPVTizM/sBfggnAIs5OvzsG4G9gtE2dZWo/swiTePz2vZ9QAAAA==" target="_blank">Run the query</a>

```kusto
datatable(local_dt: datetime, tz: string)
[ datetime(2020-02-02 20:02:20), 'US/Pacific', 
  datetime(2020-02-02 20:02:20), 'America/Chicago', 
  datetime(2020-02-02 20:02:20), 'Europe/Paris']
| extend utc_dt = datetime_local_to_utc(local_dt, tz)
```

**Output**

|local_dt|tz|utc_dt|
|---|---|---|
|2020-02-02 20:02:20.0000000|Europe/Paris|2020-02-02 19:02:20.0000000|
|2020-02-02 20:02:20.0000000|America/Chicago|2020-02-03 02:02:20.0000000|
|2020-02-02 20:02:20.0000000|US/Pacific|2020-02-03 04:02:20.0000000|


> [!NOTE] 
> Normally there is a 1:1 mapping between UTC and local time, however there is a time ambiguity near the DST transition.
> Translating from local to UTC and then back to local may produce an hour offset between two local datetime values if the clocks were advanced due to DST.


```kusto
range Local from datetime(2022-03-27 01:00:00.0000000) to datetime(2022-03-27 04:00:00.0000000) step 1h
| extend UTC=datetime_local_to_utc(Local, 'Europe/Brussels')
| extend BackToLocal=datetime_utc_to_local(UTC, 'Europe/Brussels')
| extend diff=Local-BackToLocal
```
 
|Local|UTC|BackToLocal|diff|
|---|---|---|---|
|2022-03-27 02:00:00.0000000|2022-03-27 00:00:00.0000000|2022-03-27 01:00:00.0000000|01:00:00|
|2022-03-27 01:00:00.0000000|2022-03-27 00:00:00.0000000|2022-03-27 01:00:00.0000000|00:00:00|
|2022-03-27 03:00:00.0000000|2022-03-27 01:00:00.0000000|2022-03-27 03:00:00.0000000|00:00:00|
|2022-03-27 04:00:00.0000000|2022-03-27 02:00:00.0000000|2022-03-27 04:00:00.0000000|00:00:00|

## See also

* To convert from UTC to local, see [datetime_utc_to_local()](datetime-utc-to-local-function.md)
* List of supported [timezones](timezone.md)
