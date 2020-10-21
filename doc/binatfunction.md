---
title: bin_at() - Azure Data Explorer
description: This article describes bin_at() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# bin_at()

Rounds values down to a fixed-size "bin", with control over the bin's starting point.
(See also [`bin function`](./binfunction.md).)

## Syntax

`bin_at` `(`*Expression*`,` *BinSize*`, ` *FixedPoint*`)`

## Arguments

* *Expression*: A scalar expression of a numeric type (including `datetime` and `timespan`)
  indicating the value to round.
* *BinSize*: A scalar constant of a numeric type or `timespan` (for a `datetime` or `timespan` *Expression*) indicating
  the size of each bin.
* *FixedPoint*: A scalar constant of the same type as *Expression* indicating
  one value of *Expression* which is a "fixed point" (that is, a value `fixed_point`
  for which `bin_at(fixed_point, bin_size, fixed_point) == fixed_point`.)

## Returns

The nearest multiple of *BinSize* below *Expression*, shifted so that *FixedPoint*
will be translated into itself.

## Examples

|Expression                                                                    |Result                           |Comments                   |
|------------------------------------------------------------------------------|---------------------------------|---------------------------|
|`bin_at(6.5, 2.5, 7)`                                                         |`4.5`                            ||
|`bin_at(time(1h), 1d, 12h)`                                                   |`-12h`                           ||
|`bin_at(datetime(2017-05-15 10:20:00.0), 1d, datetime(1970-01-01 12:00:00.0))`|`datetime(2017-05-14 12:00:00.0)`|All bins will be at noon   |
|`bin_at(datetime(2017-05-17 10:20:00.0), 7d, datetime(2017-06-04 00:00:00.0))`|`datetime(2017-05-14 00:00:00.0)`|All bins will be on Sundays|


In the following example, notice that the `"fixed point"` arg is returned as one of the bins and the other bins are aligned to it based on the `bin_size`. Also note that each datetime bin represents the starting time of that bin:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto

datatable(Date:datetime, Num:int)[
datetime(2018-02-24T15:14),3,
datetime(2018-02-23T16:14),4,
datetime(2018-02-26T15:14),5]
| summarize sum(Num) by bin_at(Date, 1d, datetime(2018-02-24 15:14:00.0000000)) 
```

|Date|sum_Num|
|---|---|
|2018-02-23 15:14:00.0000000|4|
|2018-02-24 15:14:00.0000000|3|
|2018-02-26 15:14:00.0000000|5|
