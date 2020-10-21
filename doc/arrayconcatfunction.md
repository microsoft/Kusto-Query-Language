---
title: array_concat() - Azure Data Explorer
description: This article describes array_concat() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# array_concat()

Concatenates a number of dynamic arrays to a single array.

## Syntax

`array_concat(`*arr1*`[`,` *arr2*, ...]`)`

## Arguments

* *arr1...arrN*: Input arrays to be concatenated into a dynamic array. All arguments must be dynamic arrays (see [pack_array](packarrayfunction.md)). 

## Returns

Dynamic array of arrays with arr1, arr2, ... , arrN.

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| extend a1 = pack_array(x,y,z), a2 = pack_array(x, y)
| project array_concat(a1, a2)
```

|Column1|
|---|
|[1,2,4,1,2]|
|[2,4,8,2,4]|
|[3,6,12,3,6]|
