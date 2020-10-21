---
title: binary_and() - Azure Data Explorer | Microsoft Docs
description: This article describes binary_and() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# binary_and()

Returns a result of the bitwise `and` operation between two values.

```kusto
binary_and(x,y)	
```

## Syntax

`binary_and(`*num1*`,` *num2* `)`

## Arguments

* *num1*, *num2*: long numbers.

## Returns

Returns logical AND operation on a pair of numbers: num1 & num2.