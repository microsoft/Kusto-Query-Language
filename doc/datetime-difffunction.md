---
title: datetime_diff() - Azure Data Explorer | Microsoft Docs
description: This article describes datetime_diff() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# datetime_diff()

Calculates calendarian difference between two [datetime](./scalar-data-types/datetime.md) values.

## Syntax

`datetime_diff(`*period*`,`*datetime_1*`,`*datetime_2*`)`

## Arguments

* `period`: `string`. 
* `datetime_1`: [datetime](./scalar-data-types/datetime.md) value.
* `datetime_2`: [datetime](./scalar-data-types/datetime.md) value.

Possible values of *period*: 
- Year
- Quarter
- Month
- Week
- Day
- Hour
- Minute
- Second
- Millisecond
- Microsecond
- Nanosecond

## Returns

An integer, which represents amount of `periods` in the result of subtraction (`datetime_1` - `datetime_2`).

## Examples

```kusto
print
year = datetime_diff('year',datetime(2017-01-01),datetime(2000-12-31)),
quarter = datetime_diff('quarter',datetime(2017-07-01),datetime(2017-03-30)),
month = datetime_diff('month',datetime(2017-01-01),datetime(2015-12-30)),
week = datetime_diff('week',datetime(2017-10-29 00:00),datetime(2017-09-30 23:59)),
day = datetime_diff('day',datetime(2017-10-29 00:00),datetime(2017-09-30 23:59)),
hour = datetime_diff('hour',datetime(2017-10-31 01:00),datetime(2017-10-30 23:59)),
minute = datetime_diff('minute',datetime(2017-10-30 23:05:01),datetime(2017-10-30 23:00:59)),
second = datetime_diff('second',datetime(2017-10-30 23:00:10.100),datetime(2017-10-30 23:00:00.900)),
millisecond = datetime_diff('millisecond',datetime(2017-10-30 23:00:00.200100),datetime(2017-10-30 23:00:00.100900)),
microsecond = datetime_diff('microsecond',datetime(2017-10-30 23:00:00.1009001),datetime(2017-10-30 23:00:00.1008009)),
nanosecond = datetime_diff('nanosecond',datetime(2017-10-30 23:00:00.0000000),datetime(2017-10-30 23:00:00.0000007))
```

|year|quarter|month|week|day|hour|minute|second|millisecond|microsecond|nanosecond|
|---|---|---|---|---|---|---|---|---|---|---|
|17|2|13|5|29|2|5|10|100|100|-700|



