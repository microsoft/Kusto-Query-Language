---
title:  series_pearson_correlation()
description: Learn how to use the series_pearson_correlation() function to calculate the pearson correlation coefficient of two numeric series inputs.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/05/2023
---
# series_pearson_correlation()

Calculates the pearson correlation coefficient of two numeric series inputs.

See: [Pearson correlation coefficient](https://en.wikipedia.org/wiki/Pearson_correlation_coefficient).

## Syntax

`series_pearson_correlation(`*series1*`,` *series2*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series1*, *series2* | dynamic | &check; | The arrays of numeric values for calculating the correlation coefficient.|

## Returns

The calculated Pearson correlation coefficient between the two inputs. Any non-numeric element or non-existing element (arrays of different sizes) yields a `null` result.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA03MQQrCMBCF4b2neEsrQknBZe/gDUqILxJskjIzghQPb1tBu5353ye+3Al1iFIzHKziAjVOcIc3+DKWG7RDjw6ntWtbXCmRwRCqCEdvqZal1WfOXtK8aT2yf3AYk9pRXXP+Ertb1/z5nTOEyhhTSCy2DJSSqMNEL7o9f+GirmjzAZfhTjnAAAAA" target="_blank">Run the query</a>

```kusto
range s1 from 1 to 5 step 1
| extend s2 = 2 * s1 // Perfect correlation
| summarize s1 = make_list(s1), s2 = make_list(s2)
| extend correlation_coefficient = series_pearson_correlation(s1, s2)
```

**Output**

|s1|s2|correlation_coefficient|
|---|---|---|
|[1,2,3,4,5]|[2,4,6,8,10]|1|
