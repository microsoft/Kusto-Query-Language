---
title: tostring() - Azure Data Explorer
description: This article describes tostring() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# tostring()

Converts input to a string representation.

```kusto
tostring(123) == "123"
```

## Syntax

`tostring(`*`Expr`*`)`

## Arguments

* *`Expr`*: Expression that will be converted to string. 

## Returns

If the *`Expr`* value is non-null, the result will be a string representation of *`Expr`*.
If the *`Expr`* value is null, the result will be an empty string.
 