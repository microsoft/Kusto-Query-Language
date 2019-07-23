---
title: set_intersect() - Azure Data Explorer | Microsoft Docs
description: This article describes set_intersect() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 06/02/2019
---
# set_intersect()

Returns a `dynamic` (JSON) array of the set of all distinct values that are in all arrays - (arr1 ∩ arr2 ∩ ...).

**Syntax**

`set_intersect(`*arr1*`, `*arr2*`[`,` *arr3*, ...])`

**Arguments**

* *arr1...arrN*: Input arrays to create a intersect set (at least two arrays). All arguments must be dynamic arrays (see [pack_array](packarrayfunction.md)). 

**Returns**

Returns a dynamic array of the set of all distinct values that are in all arrays. See [`set_union()`](setunionfunction.md) and [`set_difference()`](setdifferencefunction.md).

**Example**

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

```kusto
print arr = set_intersect(dynamic([1, 2, 3]), dynamic([4,5]))
```

|arr|
|---|
|[]|