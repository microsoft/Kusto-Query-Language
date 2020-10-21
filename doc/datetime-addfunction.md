---
title: datetime_add() - Azure Data Explorer | Microsoft Docs
description: This article describes datetime_add() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# datetime_add()

Calculates a new [datetime](./scalar-data-types/datetime.md) from a specified datepart multiplied by a specified amount, added to a specified [datetime](./scalar-data-types/datetime.md).

## Syntax

`datetime_add(`*period*`,`*amount*`,`*datetime*`)`

## Arguments

* `period`: [string](./scalar-data-types/string.md). 
* `amount`: [integer](./scalar-data-types/int.md).
* `datetime`: [datetime](./scalar-data-types/datetime.md) value.

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

A date after a certain time/date interval has been added.

## Examples

```kusto
print  year = datetime_add('year',1,make_datetime(2017,1,1)),
quarter = datetime_add('quarter',1,make_datetime(2017,1,1)),
month = datetime_add('month',1,make_datetime(2017,1,1)),
week = datetime_add('week',1,make_datetime(2017,1,1)),
day = datetime_add('day',1,make_datetime(2017,1,1)),
hour = datetime_add('hour',1,make_datetime(2017,1,1)),
minute = datetime_add('minute',1,make_datetime(2017,1,1)),
second = datetime_add('second',1,make_datetime(2017,1,1))

```

|year|quarter|month|week|day|hour|minute|second|
|---|---|---|---|---|---|---|---|
|2018-01-01 00:00:00.0000000|2017-04-01 00:00:00.0000000|2017-02-01 00:00:00.0000000|2017-01-08 00:00:00.0000000|2017-01-02 00:00:00.0000000|2017-01-01 01:00:00.0000000|2017-01-01 00:01:00.0000000|2017-01-01 00:00:01.0000000|






