---
title: set_difference() - Azure Data Explorer | Microsoft Docs
description: This article describes set_difference() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 06/02/2019
---
# set_difference()

Returns a `dynamic` (JSON) array of the set of all distinct values that are in the first array but aren't in other arrays - (((arr1 \ arr2) \ arr3) \ ...).

## Syntax

`set_difference(`*arr1*`, `*arr2*`[`,` *arr3*, ...])`

## Arguments

* *arr1...arrN*: Input arrays to create a difference set (at least two arrays). All arguments must be dynamic arrays (see [pack_array](packarrayfunction.md)). 

## Returns

Returns a dynamic array of the set of all distinct values that are in arr1 but aren't in other arrays. See [`set_union()`](setunionfunction.md) and [`set_intersect()`](setintersectfunction.md).

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| extend w = z * 2
| extend a1 = pack_array(x,y,x,z), a2 = pack_array(x, y), a3 = pack_array(x,y,w)
| project set_difference(a1, a2, a3)
```

|Column1|
|---|
|[4]|
|[8]|
|[12]|

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print arr = set_difference(dynamic([1,2,3]), dynamic([1,2,3]))
```

|arr|
|---|
|[]|