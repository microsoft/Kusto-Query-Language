---
title: isinf() - Azure Data Explorer
description: Learn how to use the isinf() function to check if the input is an infinite value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/26/2022
---
# isinf()

Returns whether the input is an infinite (positive or negative) value.  

## Syntax

`isinf(`*x*`)`

## Arguments

* *x*: A real number.

## Returns

A non-zero value (true) if x is a positive or negative infinite; and zero (false) otherwise.

## Example

```kusto
range x from -1 to 1 step 1
| extend y = 0.0
| extend div = 1.0*x/y
| extend isinf=isinf(div)
```

|x|y|div|isinf|
|---|---|---|---|
|-1|0|-∞|1|
|0|0|NaN|0|
|1|0|∞|1|

## See also

* To check if a value is null, see [isnull()](isnullfunction.md).
* To check if a value is finite, see [isfinite()](isfinitefunction.md).
* To check if a value is NaN (Not-a-Number), see [isnan()](isnanfunction.md).
