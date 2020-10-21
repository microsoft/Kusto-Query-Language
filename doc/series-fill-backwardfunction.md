---
title: series_fill_backward() - Azure Data Explorer
description: This article describes series_fill_backward() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# series_fill_backward()

Performs a backward fill interpolation of missing values in a series.

An expression containing dynamic numerical array is the input. The function replaces all instances of missing_value_placeholder with the nearest value from its right side (other than missing_value_placeholder), and returns the resulting array. The rightmost instances of missing_value_placeholder are preserved.

## Syntax

`series_fill_backward(`*x*`[, `*missing_value_placeholder*`])`
* Will return series *x* with all instances of *missing_value_placeholder* filled backwards.

## Arguments

* *x*: dynamic array scalar expression, which is an array of numeric values.
* *missing_value_placeholder*: this optional parameter specifies a placeholder for missing values. The default value is `double`(*null*).

**Notes**

* Specify *null* as the default value to apply any interpolation functions after [make-series](make-seriesoperator.md): 

```kusto
make-series num=count() default=long(null) on TimeStamp from ago(1d) to ago(1h) step 1h by Os, Browser
```

* The *missing_value_placeholder* can be of any type that will be converted to actual element types. Both `double`(*null*), `long`(*null*) and `int`(*null*) have the same meaning.
* If *missing_value_placeholder* is `double`(*null*), (or omitted, which have the same meaning) then a result may contain *null* values. To fill these *null* values, use other interpolation functions. Currently only [series_outliers()](series-outliersfunction.md) support *null* values in input arrays.
* The function preserves original type of array elements.

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let data = datatable(arr: dynamic)
[
    dynamic([111,null,36,41,null,null,16,61,33,null,null])   
];
data 
| project arr, 
          fill_forward = series_fill_backward(arr)

```

|`arr`|`fill_forward`|
|---|---|
|[111,null,36,41,null,null,16,61,33,null,null]|[111,36,36,41,16, 16,16,61,33,null,null]|

  
Use [series_fill_forward](series-fill-forwardfunction.md) or [series-fill-const](series-fill-constfunction.md) to complete interpolation of the above array.
