---
title: beta_inv() - Azure Data Explorer | Microsoft Docs
description: This article describes beta_inv() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# beta_inv()

Returns the inverse of the beta cumulative probability beta density function.

```kusto
beta_inv(0.1, 10.0, 50.0)
```

If *probability* = `beta_cdf(`*x*,...`)`, then `beta_inv(`*probability*,...`)` = *x*. 

The beta distribution can be used in project planning to model probable completion times given an expected completion time and variability.

## Syntax

`beta_inv(`*probability*`, `*alpha*`, `*beta*`)`

## Arguments

* *probability*: A probability associated with the beta distribution.
* *alpha*: A parameter of the distribution.
* *beta*: A parameter of the distribution.

## Returns

* The inverse of the beta cumulative probability density function [beta_cdf()](./beta-cdffunction.md)

**Notes**

If any argument is nonnumeric, beta_inv() returns null value.

If alpha ≤ 0 or beta ≤ 0, beta_inv() returns the null value.

If probability ≤ 0 or probability > 1, beta_inv() returns the NaN value.

Given a value for probability, beta_inv() seeks that value x such that beta_cdf(x, alpha, beta) = probability.

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(p:double, alpha:double, beta:double, comment:string)
[
    0.1, 10.0, 20.0, "Valid input",
    1.5, 10.0, 20.0, "p > 1, yields null",
    0.1, double(-1.0), 20.0, "alpha is < 0, yields NaN"
]
| extend b = beta_inv(p, alpha, beta)
```

|p|alpha|beta|comment|b|
|---|---|---|---|---|
|0.1|10|20|Valid input|0.226415022388749|
|1.5|10|20|p > 1, yields null||
|0.1|-1|20|alpha is < 0, yields NaN|NaN|

## See also

* For computing cumulative beta distribution function, see [beta-cdf()](./beta-cdffunction.md).
* For computing probability beta density function, see [beta-pdf()](./beta-pdffunction.md).
