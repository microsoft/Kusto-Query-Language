---
title: binary_shift_right() - Azure Data Explorer
description: This article describes binary_shift_right() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# binary_shift_right()

Returns binary shift right operation on a pair of numbers.

```kusto
binary_shift_right(x,y)	
```

## Syntax

`binary_shift_right(`*num1*`,` *num2* `)`

## Arguments

* *num1*, *num2*: long numbers.

## Returns

Returns binary shift right operation on a pair of numbers: num1 >> (num2%64).
If n is negative a NULL value is returned.