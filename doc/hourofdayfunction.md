---
title: hourofday() - Azure Data Explorer | Microsoft Docs
description: This article describes hourofday() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# hourofday()

Returns the integer number representing the hour number of the given date

```kusto
hourofday(datetime(2015-12-14 18:54)) == 18
```

**Syntax**

`hourofday(`*a_date*`)`

**Arguments**

* `a_date`: A `datetime`.

**Returns**

`hour number` of the day (0-23).