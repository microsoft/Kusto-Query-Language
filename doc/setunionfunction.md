---
title: set_union() - Azure Data Explorer
description: This article describes set_union() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 06/02/2019
---
# set_union()

Returns a `dynamic` array of the set of all distinct values that are in any of arrays - (arr1 ∪ arr2 ∪ ...).

## Syntax

`set_union(`*arr1*`, `*arr2*`[`,` *arr3*, ...]``)`

## Arguments

* *arr1...arrN*: Input arrays to create a union set (at least two arrays). All arguments must be dynamic arrays (see [pack_array](packarrayfunction.md)). 

## Returns

Returns a dynamic array of the set of all distinct values that are in any of arrays. See [`set_intersect()`](setintersectfunction.md)  and [`set_difference()`](setdifferencefunction.md).

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| extend w = z * 2
| extend a1 = pack_array(x,y,x,z), a2 = pack_array(x, y), a3 = pack_array(w)
| project set_union(a1, a2, a3)
```

|Column1|
|---|
|[1,2,4,8]|
|[2,4,8,16]|
|[3,6,12,24]|
