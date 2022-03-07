---
title: monthofyear() - Azure Data Explorer
description: This article describes monthofyear() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# monthofyear()

Returns the integer number represents the month number of the given year.

Another alias: getmonth()

```kusto
monthofyear(datetime("2015-12-14"))
```

## Syntax

`monthofyear(`*a_date*`)`

## Arguments

* `a_date`: A `datetime`.

## Returns

`month number` of the given year.