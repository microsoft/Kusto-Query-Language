---
title: binary_shift_right() - Azure Data Explorer | Microsoft Docs
description: This article describes binary_shift_right() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# binary_shift_right()

Returns binary shift right operation on a pair of numbers.

```kusto
binary_shift_right(x,y)	
```

**Syntax**

`binary_shift_right(`*num1*`,` *num2* `)`

**Arguments**

* *num1*, *num2*: long numbers.

**Returns**

Returns binary shift right operation on a pair of numbers: num1 >> (num2%64).
If n is negative a NULL value is returned.