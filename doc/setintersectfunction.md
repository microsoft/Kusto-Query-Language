---
title: set_intersect() - Azure Data Explorer
description: This article describes set_intersect() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 06/02/2019
---
# set_intersect()

Returns a `dynamic` array of the set of all distinct values that are in all arrays - (arr1 ∩ arr2 ∩ ...).

## Syntax

`set_intersect(`*arr1*`, `*arr2*`[`,` *arr3*, ...])`

## Arguments

* *arr1...arrN*: Input arrays to create an intersect set (at least two arrays). All arguments must be dynamic arrays. For more information, see [pack_array](packarrayfunction.md). 

## Returns

Returns a dynamic array of the set of all distinct values that are in all arrays. See [`set_union()`](setunionfunction.md) and [`set_difference()`](setdifferencefunction.md).

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| extend w = z * 2
| extend a1 = pack_array(x,y,x,z), a2 = pack_array(x, y), a3 = pack_array(w,x)
| project set_intersect(a1, a2, a3)
```

|Column1|
|---|
|[1]|
|[2]|
|[3]|

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print arr = set_intersect(dynamic([1, 2, 3]), dynamic([4,5]))
```

|arr|
|---|
|[]|
