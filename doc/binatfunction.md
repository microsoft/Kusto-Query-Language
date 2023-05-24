---
title:  bin_at()
description: Learn how to use the bin_at() function to round values down to a fixed-size bin. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
---
# bin_at()

Rounds values down to a fixed-size bin, with control over the bin's starting point.

## Syntax

`bin_at` `(`*value*`,`*bin_size*`,`*fixed_point*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | int, long, real, timespan, or datetime | &check; | The value to round. |
| *bin_size* | int, long, real, or timespan | &check; | The size of each bin. |
| *fixed_point* | int, long, real, timespan, or datetime | &check; | A constant of the same type as *value* indicating one value of *value*, which is a *fixed point* for which `bin_at(fixed_point, bin_size, fixed_point) == fixed_point`.|

> [!NOTE]
> If *value* is a timespan or datetime, then the *bin_size* must be a timespan.

## Returns

The nearest multiple of *bin_size* below *value*, shifted so that *fixed_point*
will be translated into itself.

## Examples

|Expression                                                                    |Result                           |Comments                   |
|------------------------------------------------------------------------------|---------------------------------|---------------------------|
|`bin_at(6.5, 2.5, 7)`                                                         |`4.5`                            ||
|`bin_at(time(1h), 1d, 12h)`                                                   |`-12h`                           ||
|`bin_at(datetime(2017-05-15 10:20:00.0), 1d, datetime(1970-01-01 12:00:00.0))`|`datetime(2017-05-14 12:00:00.0)`|All bins will be at noon   |
|`bin_at(datetime(2017-05-17 10:20:00.0), 7d, datetime(2017-06-04 00:00:00.0))`|`datetime(2017-05-14 00:00:00.0)`|All bins will be on Sundays|

In the following example, notice that the `"fixed point"` arg is returned as one of the bins and the other bins are aligned to it based on the `bin_size`. Also note that each datetime bin represents the starting time of that bin:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUDZfEklSrFCBRkpmbqqPgV5prlZlXohnNywUT1DAyMLTQNTDSNTIJMTS1MjTR1DHWwSZtHGJoBpY2wSptBtNtGsvLVaNQXJqbm1iUWZUKYmkA7dVUSKpUSMrMi08sAbtKR8EwRUcBiysUwOZYGRjoGUCApiYAxLxe/tAAAAA=" target="_blank">Run the query</a>

```kusto
datatable(Date:datetime, Num:int)[
datetime(2018-02-24T15:14),3,
datetime(2018-02-23T16:14),4,
datetime(2018-02-26T15:14),5]
| summarize sum(Num) by bin_at(Date, 1d, datetime(2018-02-24 15:14:00.0000000)) 
```

**Output**

|Date|sum_Num|
|---|---|
|2018-02-23 15:14:00.0000000|4|
|2018-02-24 15:14:00.0000000|3|
|2018-02-26 15:14:00.0000000|5|

## See also

* [`bin()`](./binfunction.md)
