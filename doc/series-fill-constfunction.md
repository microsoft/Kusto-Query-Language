---
title: series_fill_const() - Azure Data Explorer | Microsoft Docs
description: This article describes series_fill_const() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# series_fill_const()

Replaces missing values in a series with a specified constant value.

Takes an expression containing dynamic numerical array as input, replaces all instances of missing_value_placeholder with specified constant_value and returns the resulting array.

**Syntax**

`series_fill_const(`*x*`[, `*constant_value*`[,` *missing_value_placeholder*`]])`
* Will return series *x* with all instances of *missing_value_placeholder* replaced with *constant_value*.

**Arguments**

* *x*: dynamic array scalar expression which is an array of numeric values.
* *constant_value*: parameter which specifies a placeholder for a missing values to be replaced. Default value is *0*. 
* *missing_value_placeholder*: optional parameter which specifies a placeholder for a missing values to be replaced. Default value is `double`(*null*).

**Notes**
* It is possible to create a series with constant fill in one call using `default = ` *DefaultValue* syntax (or just omitting which will assume 0). See [make-series](make-seriesoperator.md) for more information.

```kusto
make-series num=count() default=-1 on TimeStamp in range(ago(1d), ago(1h), 1h) by Os, Browser
```
  
* In order to apply any interpolation functions after [make-series](make-seriesoperator.md) it is recommended to specify *null* as a default value: 

```kusto
make-series num=count() default=long(null) on TimeStamp in range(ago(1d), ago(1h), 1h) by Os, Browser
```
  
* The *missing_value_placeholder* can be of any type which will be converted to actual element types. Therefore either `double`(*null*), `long`(*null*) or `int`(*null*) have the same meaning.
* The function preserves original type of the array elements. 

**Example**

```kusto
let data = datatable(arr: dynamic)
[
    dynamic([111,null,36,41,23,null,16,61,33,null,null])   
];
data 
| project arr, 
          fill_const1 = series_fill_const(arr, 0.0),
          fill_const2 = series_fill_const(arr, -1)  
```

|arr|fill_const1|fill_const2|
|---|---|---|
|[111,null,36,41,23,null,16,61,33,null,null]|[111,0.0,36,41,23,0.0,16,61,33,0.0,0.0]|[111,-1,36,41,23,-1,16,61,33,-1,-1]|