---
title: tobool() - Azure Data Explorer
description: This article describes tobool() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# tobool()

Converts input to boolean (signed 8-bit) representation.

```kusto
tobool("true") == true
tobool("false") == false
tobool(1) == true
tobool(123) == true
```

## Syntax

`tobool(`*Expr*`)`
`toboolean(`*Expr*`)` (alias)

## Arguments

* *Expr*: Expression that will be converted to boolean. 

## Returns

If conversion is successful, result will be a boolean.
If conversion isn't successful, result will be `null`.
