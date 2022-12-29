---
title: series_cos() - Azure Data Explorer
description: This article describes series_cos() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 10/11/2021
---
# series_cos()

Calculates the element-wise cosine function of the numeric series input.

## Syntax

`series_cos(`*series*`)`

## Arguments

* *series*: Input numeric array, on which the cosine function is applied. The argument must be a dynamic array. 

## Returns

Dynamic array of calculated cosine function values. Any non-numeric element yields a `null` element value.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print arr = dynamic([-1,0,1])
| extend arr_cos = series_cos(arr)
```

**Output**

|arr|arr_cos|
|---|---|
|[-6.5,0,8.2]|[0.54030230586813976,1.0,0.54030230586813976]|

