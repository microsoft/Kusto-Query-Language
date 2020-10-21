---
title: isfinite() - Azure Data Explorer | Microsoft Docs
description: This article describes isfinite() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# isfinite()

Returns whether input is a finite value (is neither infinite nor NaN).

## Syntax

`isfinite(`*x*`)`

## Arguments

* *x*: A real number.

## Returns

A non-zero value (true) if x is finite; and zero (false) otherwise.

## See also

* For checking if value is null, see [isnull()](isnullfunction.md).
* For checking if value is infinite, see [isinf()](isinffunction.md).
* For checking if value is NaN (Not-a-Number), see [isnan()](isnanfunction.md).

## Example

```kusto
range x from -1 to 1 step 1
| extend y = 0.0
| extend div = 1.0*x/y
| extend isfinite=isfinite(div)
```

|x|y|div|isfinite|
|---|---|---|---|
|-1|0|-∞|0|
|0|0|NaN|0|
|1|0|∞|0|