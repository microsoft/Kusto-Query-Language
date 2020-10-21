---
title: todecimal() - Azure Data Explorer | Microsoft Docs
description: This article describes todecimal() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# todecimal()

Converts input to decimal number representation.

```kusto
todecimal("123.45678") == decimal(123.45678)
```

## Syntax

`todecimal(`*Expr*`)`

## Arguments

* *Expr*: Expression that will be converted to decimal. 

## Returns

If conversion is successful, result will be a decimal number.
If conversion is not successful, result will be `null`.
 
*Note*: Prefer using [real()](./scalar-data-types/real.md) when possible.