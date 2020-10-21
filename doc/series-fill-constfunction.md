---
title: series_fill_const() - Azure Data Explorer
description: This article describes series_fill_const() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# series_fill_const()

Replaces missing values in a series with a specified constant value.

Takes an expression containing dynamic numerical array as input, replaces all instances of missing_value_placeholder with the specified constant_value and returns the resulting array.

## Syntax

`series_fill_const(`*x*`, `*constant_value*`[,` *missing_value_placeholder*`])`
* Will return series *x* with all instances of *missing_value_placeholder* replaced with *constant_value*.

## Arguments

* *x*: dynamic array scalar expression that is an array of numeric values.
* *constant_value*: the value replacing missing values. 
* *missing_value_placeholder*: optional parameter that specifies a placeholder for a missing value to be replaced. Default value is `double`(*null*).

**Notes**
* If you create the series using the [make-series](make-seriesoperator.md) operator, it fills in the missing values using default 0. Alternatively, you can specify a constant value to fill in by specifying `default = ` *DefaultValue* in the make-series statement.

```kusto
make-series num=count() default=-1 on TimeStamp from ago(1d) to ago(1h) step 1h by Os, Browser
```
  
* To apply any interpolation functions after [make-series](make-seriesoperator.md), specify *null* as a default value: 

```kusto
make-series num=count() default=long(null) on TimeStamp from ago(1d) to ago(1h) step 1h by Os, Browser
```
  
* The *missing_value_placeholder* can be of any type, which will be converted to actual element types. As such, either `double`(*null*), `long`(*null*) or `int`(*null*) have the same meaning.
* The function preserves original type of the array elements. 

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let data = datatable(`arr`: dynamic)
[
    dynamic([111,null,36,41,23,null,16,61,33,null,null])   
];
data 
| project arr, 
          fill_const1 = series_fill_const(arr, 0.0),
          fill_const2 = series_fill_const(arr, -1)  
```

|`arr`|`fill_const1`|`fill_const2`|
|---|---|---|
|[111,null,36,41,23,null,16,61,33,null,null]|[111,0.0,36,41,23,0.0,16,61,33,0.0,0.0]|[111,-1,36,41,23,-1,16,61,33,-1,-1]|
