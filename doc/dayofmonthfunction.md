---
title: dayofmonth() - Azure Data Explorer
description: Learn how to use the dayofmonth() function to return an integer representing the day of the month.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# dayofmonth()

Returns the integer number representing the day number of the given month

```kusto
dayofmonth(datetime(2015-12-14)) == 14
```

## Syntax

`dayofmonth(`*a_date*`)`

## Arguments

* `a_date`: A `datetime`.

## Returns

`day number` of the given month.
