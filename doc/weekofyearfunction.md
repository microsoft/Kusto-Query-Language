---
title: weekofyear() - Azure Data Explorer | Microsoft Docs
description: This article describes weekofyear() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# weekofyear()

Returns the integer number represents the week number.

Aligned with ISO 8601 standards, where first day of the week is Sunday.

```kusto
weekofyear(datetime("2015-12-14"))
```

**Syntax**

`weekofyear(`*a_date*`)`

**Arguments**

* `a_date`: A `datetime`.

**Returns**

`week number` - The week number that contains the given date.