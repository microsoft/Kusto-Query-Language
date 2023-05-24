---
title:  datetime_add()
description: Learn how to use the datetime_add() function to calculate a new datetime.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# datetime_add()

Calculates a new [datetime](./scalar-data-types/datetime.md) from a specified period multiplied by a specified amount, added to, or subtracted from a specified [datetime](./scalar-data-types/datetime.md).

## Syntax

`datetime_add(`*period*`,`*amount*`,`*datetime*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *period* | string | &check; | The length of time by which to increment.|
| *amount* | int | &check; | The number of *periods* to add to or subtract from *datetime*. |
| *datetime* | datetime | &check; | The date to increment by the result of the *period* x *amount* calculation. |

Possible values of *period*:

* Year
* Quarter
* Month
* Week
* Day
* Hour
* Minute
* Second
* Millisecond
* Microsecond
* Nanosecond

## Returns

A [datetime](./scalar-data-types/datetime.md) after a certain time/date interval has been added.

## Examples

### Period
> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA4XPuw6DMAyF4b1PkQ2QMjRdmPosyMKWiFASmjqqeHsIl8mSu/5H33CW7CMbsxJk8zYITOwDDYDYNjU21tkAMw331L6ert+j6zr7+BTITFJeXcchRZ4EPaoOf0SzcDXqDGEVam86mlKR92r8883HwiTPHVmnXxpTREHPrNENq0hHU8sBAAA=" target="_blank">Run the query</a>

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

**Output**

|year|quarter|month|week|day|hour|minute|second|
|---|---|---|---|---|---|---|---|
|2018-01-01 00:00:00.0000000|2017-04-01 00:00:00.0000000|2017-02-01 00:00:00.0000000|2017-01-08 00:00:00.0000000|2017-01-02 00:00:00.0000000|2017-01-01 01:00:00.0000000|2017-01-01 00:01:00.0000000|2017-01-01 00:00:01.0000000|

### Amount

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUVCoTE0sUrBVSEksSS3JzE2NT0xJ0VAHCarr6Jrq5CZmp8bD5DSMDAzNdQx1DDU1dbgKSxOLSlIxtULF1XUMjfDozs3PK8nA0AsWBdpriM/i8tTUbAydIEGglQYGODUCAFYx9CDvAAAA" target="_blank">Run the query</a>

```kusto
print  year = datetime_add('year',-5,make_datetime(2017,1,1)),
quarter = datetime_add('quarter',12,make_datetime(2017,1,1)),
month = datetime_add('month',-15,make_datetime(2017,1,1)),
week = datetime_add('week',100,make_datetime(2017,1,1))
```

**Output**

|year|quarter|month|week|
|---|---|---|---|
|2012-01-01T00:00:00Z|2020-01-01T00:00:00Z|2015-10-01T00:00:00Z|2018-12-02T00:00:00Z|
