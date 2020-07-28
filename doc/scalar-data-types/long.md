---
title: The long data type - Azure Data Explorer | Microsoft Docs
description: This article describes The long data type in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# The long data type

The `long` data type represents a signed, 64-bit wide, integer.

## long literals

Literals of the `long` data type can be specified in the following syntax:

`long` `(` *Value* `)`

Where *Value* can take the following forms:
* One more or digits, in which case the literal value is the decimal representation
  of these digits. For example, `long(12)` is the number twelve of type `long`.
* The prefix `0x` followed by one or more Hex digits. For example,
  `long(0xf)` is equivalent to `long(15)`.
* A minus (`-`) sign followed by one or more digits. For example, `long(-1)`
  is the number minus one of type `long`.
* `null`, in which case this is the [null value](null-values.md)
  of the `long` data type. Thus, the null value of type `long` is `long(null)`.

Kusto also supports literals of type `long` without the `long(`/`)` prefix/suffi
for the first two forms only. Thus, `123` is a literal of type `long`, as is
`0x123`, but `-2` is **not** a literal (it is currently interpreted as the unary
function `-` applied to the literal `2` of type long).
 
For converting long into hex string - see [tohex() function](../tohexfunction.md).