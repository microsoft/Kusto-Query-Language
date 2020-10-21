---
title: bitset_count_ones() - Azure Data Explorer
description: This article describes bitset_count_ones() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/22/2020
---
# bitset_count_ones()

Returns the number of set bits in the binary representation of a number.

```kusto
bitset_count_ones(42)
```

## Syntax

`bitset_count_ones(`*num1*``)`

## Arguments

* *num1*: long or integer number.

## Returns

Returns the number of set bits in the binary representation of a number.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
// 42 = 32+8+2 : b'00101010' == 3 bits set
print ones = bitset_count_ones(42) 
```

|ones|
|---|
|3|
