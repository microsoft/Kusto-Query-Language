---
title: datetime_utc_to_local() - Azure Data Explorer
description: This article describes the datetime_utc_to_local function in Azure Data Explorer.
ms.reviewer: elgevork
ms.topic: reference
ms.date: 07/12/2022
---
# datetime_utc_to_local()

Converts UTC datetime to local datetime using a [time-zone specification](timezone.md). 

## Syntax

`datetime_utc_to_local(`*from*`,`*timezone*`)`

## Arguments

* *from*: UTC [datetime](./scalar-data-types/datetime.md).
* *timezone*: [string](./scalar-data-types/string.md). The timezone string must be one of the supported [timezones](timezone.md).

## Returns

A local [datetime](./scalar-data-types/datetime.md) in the `timezone` that corresponds the UTC [datetime](./scalar-data-types/datetime.md).

## Example

```kusto
print dt=now()
| extend pacific_dt = datetime_utc_to_local(dt, 'US/Pacific'), canberra_dt = datetime_utc_to_local(dt, 'Australia/Canberra')
| extend diff = pacific_dt - canberra_dt
```

**Output**

|dt|pacific_dt|canberra_dt|diff|
|---|---|---|---|
|2022-07-11 22:18:48.4678620|2022-07-11 15:18:48.4678620|2022-07-12 08:18:48.4678620|-17:00:00|

## See also

* To convert a datetime from local to UTC, see [datetime_local_to_utc()](datetime-local-to-utc-function.md).
* [Timezones](timezone.md)