---
title: toint() - Azure Data Explorer
description: This article describes toint() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# toint()

Converts input to integer (signed 32-bit) number representation.

```kusto
toint("123") == int(123)
```

## Syntax

`toint(`*Expr*`)`

## Arguments

* *Expr*: Expression that will be converted to integer. 

## Returns

If the conversion is successful, the result will be an integer.
If the conversion isn't successful, the result will be `null`.
 
*Note*: Prefer using [int()](./scalar-data-types/int.md) when possible.
