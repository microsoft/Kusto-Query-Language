---
title: dayofyear() - Azure Data Explorer | Microsoft Docs
description: This article describes dayofyear() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# dayofyear()

Returns the integer number represents the day number of the given year.

```kusto
dayofyear(datetime(2015-12-14))
```

## Syntax

`dayofweek(`*a_date*`)`

## Arguments

* `a_date`: A `datetime`.

## Returns

`day number` of the given year.