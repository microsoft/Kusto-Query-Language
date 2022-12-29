---
title: series_exp() - Azure Data Explorer
description: This article describes series_exp() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 10/11/2021
---
# series_exp()

Calculates the element-wise base-e exponential function (e^x) of the numeric series input.

## Syntax

`series_exp(`*series*`)`

## Arguments

* *series*: Input numeric array whose elements are applied as the exponent in the exponential function. The argument must be a dynamic array. 

## Returns

Dynamic array of calculated exponential function. Any non-numeric element yields a `null` element value.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print s = dynamic([1,2,3])
| extend s_exp = series_exp(s)
```

**Output**

|s|s_exp|
|---|---|
|[1,2,3]|[2.7182818284590451,7.38905609893065,20.085536923187668]|
