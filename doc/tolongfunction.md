---
title: tolong() - Azure Data Explorer | Microsoft Docs
description: This article describes tolong() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# tolong()

Converts input to long (signed 64-bit) number representation.

```kusto
tolong("123") == 123
```

**Syntax**

`tolong(`*Expr*`)`

**Arguments**

* *Expr*: Expression that will be converted to long. 

**Returns**

If conversion is successful, result will be a long number.
If conversion is not successful, result will be `null`.
 
*Note*: Prefer using [long()](./scalar-data-types/long.md) when possible.