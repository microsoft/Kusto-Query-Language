---
title: dayofweek() - Azure Data Explorer | Microsoft Docs
description: This article describes dayofweek() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# dayofweek()

Returns the integer number of days since the preceding Sunday, as a `timespan`.

```kusto
dayofweek(datetime(2015-12-14)) == 1d  // Monday
```

## Syntax

`dayofweek(`*a_date*`)`

## Arguments

* `a_date`: A `datetime`.

## Returns

The `timespan` since midnight at the beginning of the preceding Sunday, rounded down to an integer number of days.

## Examples

```kusto
dayofweek(datetime(1947-11-30 10:00:05))  // time(0.00:00:00), indicating Sunday
dayofweek(datetime(1970-05-11))           // time(1.00:00:00), indicating Monday
```
