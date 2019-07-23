---
title: dayofweek() - Azure Data Explorer | Microsoft Docs
description: This article describes dayofweek() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# dayofweek()

Returns the integer number of days since the preceding Sunday, as a `timespan`.

```kusto
dayofweek(datetime(2015-12-14)) == 1d  // Monday
```

**Syntax**

`dayofweek(`*a_date*`)`

**Arguments**

* `a_date`: A `datetime`.

**Returns**

The `timespan` since midnight at the beginning of the preceding Sunday, rounded down to an integer number of days.

**Examples**

```kusto
dayofweek(1947-11-29 10:00:05)  // time(6.00:00:00), indicating Saturday
dayofweek(1970-05-11)           // time(1.00:00:00), indicating Monday
```