---
title:  beta_cdf()
description: Learn how to use the beta_cdf() function to return a standard beta cumulative distribution function.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/09/2023
---
# beta_cdf()

Returns the standard cumulative beta distribution function.

If *probability* = `beta_cdf(`*x*,...`)`, then `beta_inv(`*probability*,...`)` = *x*.

The beta distribution is commonly used to study variation in the percentage of something across samples, such as the fraction of the day people spend watching television.

## Syntax

`beta_cdf(`*x*`,` *alpha*`,` *beta*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *x* | int, long, or real | &check;| A value at which to evaluate the function.|
| *alpha* | int, long, or real | &check;| A parameter of the distribution.|
| *beta* | int, long, or real | &check;| A parameter of the distribution.|

## Returns

The [cumulative beta distribution function](https://en.wikipedia.org/wiki/Beta_distribution#Cumulative_distribution_function).

> [!NOTE]
>
> * If any argument is nonnumeric, the function returns `null`.
> * If `x < 0` or `x > 1`, the function returns `NaN`.
> * If `alpha ≤ 0` or `alpha > 10000`, the function returns `NaN`.
> * If `beta ≤ 0` or `beta > 10000`, the function returns `NaN`.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22PwQrCMBBE7/mKoacWYkgFDxb1E3r0IiJpEzWQpsWmEMGPN6algnUXlh2Yt8tI4UJXRqW+kO0QFgphuruYVaXcV9Rt0yjrit49tL1l5EQQirMtRc4Zp1jHmRyF0RLadoNLaPTkbPPj8Tggp3hqZWSPUpSTc/yVrnKeLYgd+B+Cs3BnxtiHm4gYBbpfgORMXlDeKStRYR9TXmp5Tf2UfwyevQFA2/Y/IAEAAA==" target="_blank">Run the query</a>

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

**Output**

|x|alpha|beta|comment|b|
|---|---|---|---|---|
|0.9|10|20|Valid input|0.999999999999959|
|1.5|10|20|x > 1, yields NaN|NaN|
|-10|10|20|x < 0, yields NaN|NaN|
|0.1|-1|20|alpha is < 0, yields NaN|NaN|

## See also

* For computing the inverse of the beta cumulative probability density function, see [beta-inv()](./beta-invfunction.md).
* For computing probability density function, see [beta-pdf()](./beta-pdffunction.md).
