---
title: beta_pdf() - Azure Data Explorer
description: This article describes beta_pdf() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# beta_pdf()

Returns the probability density beta function.

```kusto
beta_pdf(0.2, 10.0, 50.0)
```

The beta distribution is commonly used to study variation in the percentage of something across samples, such as the fraction of the day people spend watching television.

## Syntax

`beta_pdf(`*x*`, `*alpha*`, `*beta*`)`

## Arguments

* *x*: A value at which to evaluate the function.
* *alpha*: A parameter of the distribution.
* *beta*: A parameter of the distribution.

## Returns

* The [probability beta density function](https://en.wikipedia.org/wiki/Beta_distribution#Probability_density_function).

**Notes**

If any argument is nonnumeric, beta_pdf() returns null value.

If x ≤ 0 or 1 ≤ x, beta_pdf() returns NaN value.

If alpha ≤ 0 or beta ≤ 0, beta_pdf() returns the NaN value.

## Examples

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(x:double, alpha:double, beta:double, comment:string)
[
    0.5, 10.0, 20.0, "Valid input",
    1.5, 10.0, 20.0, "x > 1, yields NaN",
    double(-10), 10.0, 20.0, "x < 0, yields NaN",
    0.1, double(-1.0), 20.0, "alpha is < 0, yields NaN"
]
| extend r = beta_pdf(x, alpha, beta)
```

|x|alpha|beta|comment|r|
|---|---|---|---|---|
|0.5|10|20|Valid input|0.746176019310951|
|1.5|10|20|x > 1, yields NaN|NaN|
|-10|10|20|x < 0, yields NaN|NaN|
|0.1|-1|20|alpha is < 0, yields NaN|NaN|

**References**

* For computing the inverse of the beta cumulative probability density function, see [beta-inv()](./beta-invfunction.md).
* For the standard cumulative beta distribution function, see [beta-cdf()](./beta-cdffunction.md).
