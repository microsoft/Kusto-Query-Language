---
title: series_pow() - Azure Data Explorer
description: This article describes series_pow() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 08/15/2021
---
# series_pow()

Calculates the element-wise power of two numeric series inputs.

## Syntax

`series_pow(`*series1*`,` *series2*`)`

## Arguments

* *series1, series2*: Input numeric arrays, the first (base) is element-wise raised to the power of the second (power) into a dynamic array result. All arguments must be dynamic arrays.

## Returns

Dynamic array of calculated element-wise power operation between the two inputs. Any non-numeric element or non-existing element (arrays of different sizes) yields a `null` element value.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print x = dynamic([1,2,3,4]), y=dynamic([1,2,3,0.5])
| extend x_pow_y = series_pow(x,y) 
```

|x|y|x_pow_y|
|---|---|---|
|[1,2,3,4]|[1,2,3,0.5]|[1.0,4.0,27.0,2.0]|