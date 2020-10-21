---
title: series_pearson_correlation() - Azure Data Explorer
description: This article describes series_pearson_correlation() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/31/2019
---
# series_pearson_correlation()

Calculates the pearson correlation coefficient of two numeric series inputs.

See: [Pearson correlation coefficient](https://en.wikipedia.org/wiki/Pearson_correlation_coefficient).

## Syntax

`series_pearson_correlation(`*Series1*`,` *Series2*`)`

## Arguments

* *Series1, Series2*: Input numeric arrays for calculating the correlation coefficient. All arguments must be dynamic arrays of the same length. 

## Returns

The calculated Pearson correlation coefficient between the two inputs. Any non-numeric element or non-existing element (arrays of different sizes) yields a `null` result.

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range s1 from 1 to 5 step 1 | extend s2 = 2*s1 // Perfect correlation
| summarize s1 = make_list(s1), s2 = make_list(s2)
| extend correlation_coefficient = series_pearson_correlation(s1,s2)
```

|s1|s2|correlation_coefficient|
|---|---|---|
|[1,2,3,4,5]|[2,4,6,8,10]|1|
