---
title: totimespan() - Azure Data Explorer | Microsoft Docs
description: This article describes totimespan() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# totimespan()

Converts input  to [timespan](./scalar-data-types/timespan.md) scalar.

```kusto
totimespan("0.00:01:00") == time(1min)
```

**Syntax**

`totimespan(`*Expr*`)`

**Arguments**

* *Expr*: Expression that will be converted to [timespan](./scalar-data-types/timespan.md). 

**Returns**

If conversion is successful, result will be a [timespan](./scalar-data-types/timespan.md) value.
If conversion is not successful, result will be null.
 