---
title: tohex() - Azure Data Explorer | Microsoft Docs
description: This article describes tohex() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# tohex()

Converts input to a hexadecimal string.

```kusto
tohex(256) == '100'
tohex(-256) == 'ffffffffffffff00' // 64-bit 2's complement of -256
tohex(toint(-256), 8) == 'ffffff00' // 32-bit 2's complement of -256
tohex(256, 8) == '00000100'
tohex(256, 2) == '100' // Exceeds min length of 2, so min length is ignored.
```

## Syntax

`tohex(`*Expr*`, [`,` *MinLength*]`)`

## Arguments

* *Expr*: int or long value that will be converted to a hex string.  Other types are not supported.
* *MinLength*: numeric value representing the number of leading characters to include in the output.  Values between 1 and 16 are supported, values greater than 16 will be truncated to 16.  If the string is longer than minLength without leading characters, then minLength is effectively ignored.  Negative numbers may only be represented at minimum by their underlying data size, so for an int (32-bit) the minLength will be at minimum 8, for a long (64-bit) it will be at minimum 16.

## Returns

If conversion is successful, result will be a string value.
If conversion is not successful, result will be null.