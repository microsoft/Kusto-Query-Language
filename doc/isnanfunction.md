---
title: isnan() - Azure Data Explorer
description: Learn how to use the isnan() function to check if the input is a not-a-number (NaN) value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/26/2022
---
# isnan()

Returns whether the input is a Not-a-Number (NaN) value.  

## Syntax

`isnan(`*x*`)`

## Arguments

* *x*: A real number.

## Returns

A non-zero value (true) if x is NaN; and zero (false) otherwise.

## Example

```kusto
range x from -1 to 1 step 1
| extend y = (-1*x) 
| extend div = 1.0*x/y
| extend isnan=isnan(div)
```

**Output**

|x|y|div|isnan|
|---|---|---|---|
|-1|1|-1|0|
|0|0|NaN|1|
|1|-1|-1|0|

## See also

* To check if a value is null, see [isnull()](isnullfunction.md).
* To check if a value is finite, see [isfinite()](isfinitefunction.md).
* To check if a value is infinite, see [isinf()](isinffunction.md).
