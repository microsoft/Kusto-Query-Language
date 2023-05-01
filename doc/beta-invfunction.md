---
title: beta_inv() - Azure Data Explorer
description: Learn how to use the beta_inv() function to return the inverse of the beta cumulative probability density function.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/09/2023
---
# beta_inv()

Returns the inverse of the beta cumulative probability density function.

If *probability* = `beta_cdf(`*x*,...`)`, then `beta_inv(`*probability*,...`)` = *x*.

The beta distribution can be used in project planning to model probable completion times given an expected completion time and variability.

## Syntax

`beta_inv(`*probability*`,`*alpha*`,`*beta*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *probability* | int, long, or real | &check;| A probability associated with the beta distribution.|
| *alpha* | int, long, or real | &check;| A parameter of the distribution.|
| *beta* | int, long, or real | &check;| A parameter of the distribution.|

## Returns

The inverse of the beta cumulative probability density function [beta_cdf()](./beta-cdffunction.md)

> [!NOTE]
>
> * If any argument is nonnumeric, the function returns `null`.
> * If `alpha ≤ 0` or `beta ≤ 0`, the function returns `null`.
> * If `probability ≤ 0` or `probability > 1`, the function returns `NaN`.
> * Given a value for *probability*, `beta_inv()` seeks that value x such that `beta_cdf(x, alpha, beta)` `=` *probability*.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA11PQQrCMBC85xVDTw3EkAheivYJPXoRkdQEDaRpsIko+HhDWiq4C8sOzOzOaBVz987UodFjyguDcuGuVtSb+APXcRiMj80UH9bfKDkR5BJcMkjBBcO2zOqonNWwPqRYscKRfPfHCWiRdW9rnJ7gk3MLtZybP9YbyQVdJcUZ7IQ9xKrsVFeRM/nAvKLxGj0OxfTF+mcdljhzDvoFBansKu8AAAA=" target="_blank">Run the query</a>

```kusto
datatable(p:double, alpha:double, beta:double, comment:string)
[
    0.1, 10.0, 20.0, "Valid input",
    1.5, 10.0, 20.0, "p > 1, yields null",
    0.1, double(-1.0), 20.0, "alpha is < 0, yields NaN"
]
| extend b = beta_inv(p, alpha, beta)
```

**Output**

|p|alpha|beta|comment|b|
|---|---|---|---|---|
|0.1|10|20|Valid input|0.226415022388749|
|1.5|10|20|p > 1, yields null||
|0.1|-1|20|alpha is < 0, yields NaN|NaN|

## See also

* For computing cumulative beta distribution function, see [beta-cdf()](./beta-cdffunction.md).
* For computing probability beta density function, see [beta-pdf()](./beta-pdffunction.md).
