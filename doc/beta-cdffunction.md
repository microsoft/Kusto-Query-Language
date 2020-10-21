---
title: beta_cdf() - Azure Data Explorer
description: This article describes beta_cdf() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# beta_cdf()

Returns the standard cumulative beta distribution function.

```kusto
beta_cdf(0.2, 10.0, 50.0)
```

If *probability* = `beta_cdf(`*x*,...`)`, then `beta_inv(`*probability*,...`)` = *x*.

The beta distribution is commonly used to study variation in the percentage of something across samples, such as the fraction of the day people spend watching television.

## Syntax

`beta_cdf(`*x*`, `*alpha*`, `*beta*`)`

## Arguments

* *x*: A value at which to evaluate the function.
* *alpha*: A parameter of the distribution.
* *beta*: A parameter of the distribution.

## Returns

* The [cumulative beta distribution function](https://en.wikipedia.org/wiki/Beta_distribution#Cumulative_distribution_function).

**Notes**

If any argument is nonnumeric, beta_cdf() returns null value.

If x < 0 or x > 1, beta_cdf() returns NaN value.

If alpha ≤ 0 or alpha > 10000, beta_cdf() returns the NaN value.

If beta ≤ 0 or beta > 10000, beta_cdf() returns the NaN value.

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(x:double, alpha:double, beta:double, comment:string)
[
    0.9, 10.0, 20.0, "Valid input",
    1.5, 10.0, 20.0, "x > 1, yields NaN",
    double(-10), 10.0, 20.0, "x < 0, yields NaN",
    0.1, double(-1.0), 20.0, "alpha is < 0, yields NaN"
]
| extend b = beta_cdf(x, alpha, beta)
```

|x|alpha|beta|comment|b|
|---|---|---|---|---|
|0.9|10|20|Valid input|0.999999999999959|
|1.5|10|20|x > 1, yields NaN|NaN|
|-10|10|20|x < 0, yields NaN|NaN|
|0.1|-1|20|alpha is < 0, yields NaN|NaN|


## See also


* For computing the inverse of the beta cumulative probability density function, see [beta-inv()](./beta-invfunction.md).
* For computing probability density function, see [beta-pdf()](./beta-pdffunction.md).
