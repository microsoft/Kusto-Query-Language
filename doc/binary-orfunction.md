---
title: binary_or() - Azure Data Explorer | Microsoft Docs
description: This article describes binary_or() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# binary_or()

Returns a result of the bitwise `or` operation of the two values. 

```kusto
binary_or(x,y)
```

## Syntax

`binary_or(`*num1*`,` *num2* `)`

## Arguments

* *num1*, *num2*: long numbers.

## Returns

Returns logical OR operation on a pair of numbers: num1 | num2.