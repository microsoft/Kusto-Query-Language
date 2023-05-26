---
title:  beta_pdf()
description: Learn how to use the beta_pdf() function to return the beta probability density function. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 05/25/2023
---
# beta_pdf()

Returns the probability density beta function.

The beta distribution is commonly used to study variation in the percentage of something across samples, such as the fraction of the day people spend watching television.

## Syntax

`beta_pdf(`*x*`,` *alpha*`,` *beta*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *x* | int, long, or real | &check;| A value at which to evaluate the function.|
| *alpha* | int, long, or real | &check;| A parameter of the distribution.|
| *beta* | int, long, or real | &check;| A parameter of the distribution.|

## Returns

The [probability beta density function](https://en.wikipedia.org/wiki/Beta_distribution#Probability_density_function).

> [!NOTE]
>
> * If any argument is nonnumeric, the function returns `null`.
> * If `x ≤ 0` or `1 ≤ x`, the function returns `NaN`.
> * If `alpha ≤ 0` or `beta ≤ 0`, the function returns `NaN`.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22PwQrCMBBE7/mKoacWYkgEL0X9hB69iEhqogbStLQpRPDjjWmph7oLyw7M22WU9LFrq/NQqnaMC4W03VMuqtb+J25t02jny8H3xj0KciaIxdmOQnDGKbZpZidpjYJx3egzmjxi5Qk4QlC8jLZqQCWr2Tn9yjeCFytiD/6H4CzeWTD25WYiRYEZViC5kDd08Nop9DiklNdO3fMw55+CFx9P6bKEIAEAAA==" target="_blank">Run the query</a>

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

**Output**

|x|alpha|beta|comment|r|
|---|---|---|---|---|
|0.5|10|20|Valid input|0.746176019310951|
|1.5|10|20|x > 1, yields NaN|NaN|
|-10|10|20|x < 0, yields NaN|NaN|
|0.1|-1|20|alpha is < 0, yields NaN|NaN|

## See also

* For computing the inverse of the beta cumulative probability density function, see [beta-inv()](./beta-invfunction.md).
* For the standard cumulative beta distribution function, see [beta-cdf()](./beta-cdffunction.md).
