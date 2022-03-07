---
title: array_reverse() - Azure Data Explorer
description: This article describes array_reverse() in Azure Data Explorer.
ms.reviewer: slneimer
ms.topic: reference
ms.date: 11/09/2020
---
# array_reverse()

Reverses the order of the elements in a dynamic array.

## Syntax

`array_reverse(`*array*`)`

## Arguments

*array*: Input array to reverse.

## Returns

An array that contains exactly the same elements as the input array, but in reverse order.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print arr=dynamic(["this", "is", "an", "example"]) 
| project Result=array_reverse(arr)
```

|Result|
|---|
|["example","an","is","this"]|
