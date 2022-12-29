---
title: isfinite() - Azure Data Explorer
description: Learn how to use the isfinite() function to check if the input is a finite value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/26/2022
---
# isfinite()

Returns whether the input is a finite value (is neither infinite nor NaN).

## Syntax

`isfinite(`*x*`)`

## Arguments

* *x*: A real number.

## Returns

A non-zero value (true) if x is finite; and zero (false) otherwise.

## Example

```kusto
range x from -1 to 1 step 1
| extend y = 0.0
| extend div = 1.0*x/y
| extend isfinite=isfinite(div)
```

**Output**

|x|y|div|isfinite|
|---|---|---|---|
|-1|0|-∞|0|
|0|0|NaN|0|
|1|0|∞|0|

## See also

* To check if a value is null, see [isnull()](isnullfunction.md).
* To check if a value is infinite, see [isinf()](isinffunction.md).
* To check if a value is NaN (Not-a-Number), see [isnan()](isnanfunction.md).
