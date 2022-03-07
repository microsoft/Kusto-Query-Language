---
title: dayofmonth() - Azure Data Explorer
description: This article describes dayofmonth() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
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