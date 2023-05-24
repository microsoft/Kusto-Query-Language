---
title:  series_exp()
description: Learn how to use the series_exp() function to calculate the element-wise base-e exponential function (e^x) of the numeric series input.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/22/2023
---
# series_exp()

Calculates the element-wise base-e exponential function (e^x) of the numeric series input.

## Syntax

`series_exp(`*series*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values whose elements are applied as the exponent in the exponential function. |

## Returns

Dynamic array of calculated exponential function. Any non-numeric element yields a `null` element value.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShWsFVIqcxLzM1M1og21DHSMY7V5KpRSK0oSc1LUSiOT60oAKooTi3KTAVzNIo1ASk3NNg5AAAA" target="_blank">Run the query</a>

```kusto
print s = dynamic([1,2,3])
| extend s_exp = series_exp(s)
```

**Output**

|s|s_exp|
|---|---|
|[1,2,3]|[2.7182818284590451,7.38905609893065,20.085536923187668]|
