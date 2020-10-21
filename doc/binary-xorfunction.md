---
title: binary_xor() - Azure Data Explorer | Microsoft Docs
description: This article describes binary_xor() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# binary_xor()

Returns a result of the bitwise `xor` operation of the two values.

```kusto
binary_xor(x,y)
```

## Syntax

`binary_xor(`*num1*`,` *num2* `)`

## Arguments

* *num1*, *num2*: long numbers.

## Returns

Returns logical XOR operation on a pair of numbers: num1 ^ num2.