---
title: series_subtract() - Azure Data Explorer
description: This article describes series_subtract() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# series_subtract()

Calculates the element-wise subtraction of two numeric series inputs.

## Syntax

`series_subtract(`*series1*`,` *series2*`)`

## Arguments

* *series1, series2*: Input numeric arrays, the second to be element-wise subtracted from the first into a dynamic array result. All arguments must be dynamic arrays. 

## Returns

Dynamic array of calculated element-wise subtract operation between the two inputs. Any non-numeric element or non-existing element (arrays of different sizes) yields a `null` element value.

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| project s1 = pack_array(x,y,z), s2 = pack_array(z, y, x)
| extend s1_subtract_s2 = series_subtract(s1, s2)
```

|s1|s2|s1_subtract_s2|
|---|---|---|
|[1,2,4]|[4,2,1]|[-3,0,3]|
|[2,4,8]|[8,4,2]|[-6,0,6]|
|[3,6,12]|[12,6,3]|[-9,0,9]|
