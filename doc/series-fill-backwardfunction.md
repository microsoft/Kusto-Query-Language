---
title: series_fill_backward() - Azure Data Explorer | Microsoft Docs
description: This article describes series_fill_backward() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# series_fill_backward()

Performs backward fill interpolation of missing values in a series.

Takes an expression containing dynamic numerical array as input, replaces all instances of missing_value_placeholder with the nearest value from its right side other than missing_value_placeholder and returns the resulting array. The rightmost instances of missing_value_placeholder are preserved.

**Syntax**

`series_fill_backward(`*x*`[, `*missing_value_placeholder*`])`
* Will return series *x* with all instances of *missing_value_placeholder* filled backwards.

**Arguments**

* *x*: dynamic array scalar expression which is an array of numeric values.
* *missing_value_placeholder*: optional parameter which specifies a placeholder for a missing values to be replaced. Default value is `double`(*null*).

**Notes**

* In order to apply any interpolation functions after [make-series](make-seriesoperator.md) it is recommended to specify *null* as a default value: 

```kusto
make-series num=count() default=long(null) on TimeStamp in range(ago(1d), ago(1h), 1h) by Os, Browser
```

* The *missing_value_placeholder* can be of any type which will be converted to actual element types. Therefore either `double`(*null*), `long`(*null*) or `int`(*null*) have the same meaning.
* If *missing_value_placeholder* is `double`(*null*) (or just omitted which have the same meaning) then a result may contains *null* values. Use other interpolation functions in order to fill them. Currently only [series_outliers()](series-outliersfunction.md) support *null* values in input arrays.
* The functions preserves original type of array elements.

**Example**

```kusto
let data = datatable(arr: dynamic)
[
    dynamic([111,null,36,41,null,null,16,61,33,null,null])   
];
data 
| project arr, 
          fill_forward = series_fill_backward(arr)

```

|arr|fill_forward|
|---|---|
|[111,null,36,41,null,null,16,61,33,null,null]|[111,36,36,41,16, 16,16,61,33,null,null]|

  
One can use [series_fill_forward](series-fill-forwardfunction.md) or [series-fill-const](series-fill-constfunction.md) in order to complete interpolation of the above array.