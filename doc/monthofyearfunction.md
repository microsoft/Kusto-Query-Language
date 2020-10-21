---
title: monthofyear() - Azure Data Explorer | Microsoft Docs
description: This article describes monthofyear() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
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