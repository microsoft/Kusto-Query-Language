---
title: binary_and() - Azure Data Explorer
description: Learn how to use the binary_and() function to compare bits in corresponding operands. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/21/2022
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
