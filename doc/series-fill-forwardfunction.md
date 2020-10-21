---
title: series_fill_forward() - Azure Data Explorer
description: This article describes series_fill_forward() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# series_fill_forward()

Performs a forward fill interpolation of missing values in a series.

An expression containing dynamic numerical array is the input. The function replaces all instances of missing_value_placeholder with the nearest value from its left side other than missing_value_placeholder, and returns the resulting array. The leftmost instances of missing_value_placeholder are preserved.

## Syntax

`series_fill_forward(`*x*`[, `*missing_value_placeholder*`])`
* Will return series *x* with all instances of *missing_value_placeholder* filled forwards.

## Arguments

* *x*: dynamic array scalar expression, which is an array of numeric values. 
* *missing_value_placeholder*: optional parameter, which specifies a placeholder for a missing value to be replaced. Default value is `double`(*null*).

**Notes**

* Specify *null* as the default value to apply interpolation functions after [make-series](make-seriesoperator.md): 

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
make-series num=count() default=long(null) on TimeStamp from ago(1d) to ago(1h) step 1h by Os, Browser
```

* The *missing_value_placeholder* can be of any type that will be converted to actual element types. Both `double`(*null*) `long`(*null*) and `int`(*null*) have the same meaning.
* If missing_value_placeholder is (null) (or omitted - which have the same meaning), then a result may contain *null* values. To fill these *null* values, use other interpolation functions. Currently only [series_outliers()](series-outliersfunction.md) support *null* values in input arrays.
* The functions preserve the original type of array elements.

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let data = datatable(arr: dynamic)
[
    dynamic([null,null,36,41,null,null,16,61,33,null,null])   
];
data 
| project arr, 
          fill_forward = series_fill_forward(arr)  

```

|`arr`|`fill_forward`|
|---|---|
|[null,null,36,41,null,null,16,61,33,null,null]|[null,null,36,41,41,41,16,61,33,33,33]|
   
Use [series_fill_backward](series-fill-backwardfunction.md) or [series-fill-const](series-fill-constfunction.md) to complete interpolation of the above array.
