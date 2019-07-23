---
title: todouble()/toreal() - Azure Data Explorer | Microsoft Docs
description: This article describes todouble()/toreal() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# todouble()/toreal()

Converts the input to a value of type `real`. (`todouble()` and `toreal()` are synonyms.)

```kusto
toreal("123.4") == 123.4
```

**Syntax**

`toreal(`*Expr*`)`
`todouble(`*Expr*`)`

**Arguments**

* *Expr*: An expression whose value will be converted to a value of type `real`.

**Returns**

If conversion is successful, the result is a value of type `real`.
If conversion is not successful, the result is the value `real(null)`.

*Note*: Prefer using [double() or real()](./scalar-data-types/real.md) when possible.