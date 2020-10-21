---
title: series_divide() - Azure Data Explorer
description: This article describes series_divide() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# series_divide()

Calculates the element-wise division of two numeric series inputs.

## Syntax

`series_divide(`*series1*`,` *series2*`)`

## Arguments

* *series1, series2*: Input numeric arrays, the first to be element-wise divided by the second into a dynamic array result. All arguments must be dynamic arrays. 

## Returns

Dynamic array of calculated element-wise divide operation between the two inputs. Any non-numeric element or non-existing element (arrays of different sizes) yields a `null` element value.

Note: the result series is of double type, even if the inputs are integers. Division by zero follows the double division by zero (e.g. 2/0 yields double(+inf)).

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| project s1 = pack_array(x,y,z), s2 = pack_array(z, y, x)
| extend s1_divide_s2 = series_divide(s1, s2)
```

|s1	        |s2|	    s1_divide_s2|
|---|---|---|
|[1,2,4]	|[4,2,1]|	[0.25,1.0,4.0]|
|[2,4,8]	|[8,4,2]|	[0.25,1.0,4.0]|
|[3,6,12]	|[12,6,3]|	[0.25,1.0,4.0]|
