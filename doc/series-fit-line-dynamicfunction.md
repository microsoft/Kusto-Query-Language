---
title: series_fit_line_dynamic() - Azure Data Explorer
description: Learn how to use the series_fit_line_dynamic() function to apply a linear regression on a series to return a dynamic object.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/26/2023
---
# series_fit_line_dynamic()

Applies linear regression on a series, returning dynamic object.  

Takes an expression containing dynamic numerical array as input, and does [linear regression](https://en.wikipedia.org/wiki/Line_fitting) to find the line that best fits it. This function should be used on time series arrays, fitting the output of make-series operator. It generates a dynamic value with the following content:

* `rsquare`: [r-square](https://en.wikipedia.org/wiki/Coefficient_of_determination) is a standard measure of the fit quality. It's a number in the range [0-1], where 1 is the best possible fit, and 0 means the data is unordered and doesn't fit any line
* `slope`: Slope of the approximated line (the *a*-value from *y=ax+b*)
* `variance`: Variance of the input data
* `rvariance`: Residual variance that is the variance between the input data values and the approximated ones.
* `interception`: Interception of the approximated line (the *b*-value from *y=ax+b*)
* `line_fit`: Numerical array containing a series of values of the best fit line. The series length is equal to the length of the input array. It's used mainly for charting.

This operator is similar to [series_fit_line](series-fit-linefunction.md), but unlike `series-fit-line` it returns a dynamic bag.

## Syntax

`series_fit_line_dynamic(`*series*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values.|

> [!TIP]
> The most convenient way of using this function is by applying it to the results of [make-series](make-seriesoperator.md) operator.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc9rf7q4d68qcw5sk2d6f.northeurope/databases/MyDatabase?query=H4sIAAAAAAAAA11OTWvDMAy9F/ofdGsCXqkztu7ia2HQUwu7jBG8RG0ErZIp3tbAfvyU2IN2Bpn3Jft1QhzmM9BDtVvAwkRyceL5iNk7ccbtd5YbsE0Od2BtY+BWjVfcG1w9sD9Tlb0WBh4MPBp40oDVUWbXOsoLxYVa96tx3vL57AfwEpBrOFBwPQphXyosT8RY/r05XAXjf7v9x6cXdBpdSj/h1GR/aruo9yNK6osX8lxF4yuR5O1uTPnnPnNAqbAL1PIUoCshZbZadqP9R3sqrmBsLNoXBQKdsWq8hF+uDo0sdgEAAA==" target="_blank">Run the query</a>

```kusto
print
    id=' ',
    x=range(bin(now(), 1h) - 11h, bin(now(), 1h), 1h),
    y=dynamic([2, 5, 6, 8, 11, 15, 17, 18, 25, 26, 30, 30])
| extend fit=series_fit_line_dynamic(y)
| extend
    RSquare=fit.rsquare,
    Slope=fit.slope,
    Variance=fit.variance,
    RVariance=fit.rvariance,
    Interception=fit.interception,
    LineFit=fit.line_fit
| render timechart
```

:::image type="content" source="images/series-fit-line/series-fit-line.png" alt-text="Series fit line.":::

| RSquare | Slope | Variance | RVariance | Interception | LineFit                                                                                     |
|---------|-------|----------|-----------|--------------|---------------------------------------------------------------------------------------------|
| 0.982   | 2.730 | 98.628   | 1.686     | -1.666       | 1.064, 3.7945, 6.526, 9.256, 11.987, 14.718, 17.449, 20.180, 22.910, 25.641, 28.371, 31.102 |
