---
title: series_fit_line() - Azure Data Explorer
description: Learn how to use the series_fit_line() function to apply a linear regression on a series to return multiple columns.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/26/2023
---
# series_fit_line()

Applies linear regression on a series, returning multiple columns.  

Takes an expression containing dynamic numerical array as input and does [linear regression](https://en.wikipedia.org/wiki/Line_fitting) to find the line that best fits it. This function should be used on time series arrays, fitting the output of make-series operator. The function generates the following columns:

* `rsquare`: [r-square](https://en.wikipedia.org/wiki/Coefficient_of_determination) is a standard measure of the fit quality. The value's a number in the range [0-1], where 1 - is the best possible fit, and 0 means the data is unordered and doesn't fit any line.
* `slope`: Slope of the approximated line ("a" from y=ax+b).
* `variance`: Variance of the input data.
* `rvariance`: Residual variance that is the variance between the input data values the approximated ones.
* `interception`: Interception of the approximated line ("b" from y=ax+b).
* `line_fit`: Numerical array holding a series of values of the best fitted line. The series length is equal to the length of the input array. The value's used for charting.

## Syntax

`series_fit_line(`*series*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values.|

> [!TIP]
> The most convenient way of using this function is to apply it to the results of [make-series](make-seriesoperator.md) operator.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc9rf7q4d68qcw5sk2d6f.northeurope/databases/MyDatabase?query=H4sIAAAAAAAAA1VMywrCQAy8C/2H3NzCCm7Fx6VXQfDUghcRWdtoAzWt6YoW/HijPYiBSWYmybRCHKIRaFGZjmFsB/FMxfMFzYnYcPMwsQVXxTAB5yoL/+7Qhr8+LXv2VyrMPrEwt7CwsNIDp1DllgrVifJEV7PpB4c4Gr0AnwG5BJPlt7sXtJDXTatj54U8F8qyH91wQCmwDdSwhS0xrinEaYdC2B3PFI61eqb/JovmokCgKxaVl/AGWOACk/UAAAA=" target="_blank">Run the query</a>

```kusto
print
    id=' ',
    x=range(bin(now(), 1h) - 11h, bin(now(), 1h), 1h),
    y=dynamic([2, 5, 6, 8, 11, 15, 17, 18, 25, 26, 30, 30])
| extend (RSquare, Slope, Variance, RVariance, Interception, LineFit)=series_fit_line(y)
| render timechart
```

:::image type="content" source="images/series-fit-line/series-fit-line.png" alt-text="Series fit line.":::

| RSquare | Slope | Variance | RVariance | Interception | LineFit                                                                                     |
|---------|-------|----------|-----------|--------------|---------------------------------------------------------------------------------------------|
| 0.982   | 2.730 | 98.628   | 1.686     | -1.666       | 1.064, 3.7945, 6.526, 9.256, 11.987, 14.718, 17.449, 20.180, 22.910, 25.641, 28.371, 31.102 |
