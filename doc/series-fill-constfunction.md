---
title:  series_fill_const()
description: Learn how to use the series_fill_const() function to replace missing values in a series with a specified constant value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/26/2023
---
# series_fill_const()

Replaces missing values in a series with a specified constant value.

Takes an expression containing dynamic numerical array as input, replaces all instances of missing_value_placeholder with the specified constant_value and returns the resulting array.

## Syntax

`series_fill_const(`*series*`,` *constant_value*`,` [ *missing_value_placeholder* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series* | dynamic | &check; | An array of numeric values.|
| *constant_value* | scalar | &check; | The value used to replace the missing values.|
| *missing_value_placeholder* | scalar | | Specifies a placeholder for missing values. The default value is `double(`*null*`)`. The value can be of any type that will be converted to actual element types. `double`(*null*), `long`(*null*) and `int`(*null*) have the same meaning.|

## Returns

*series* with all instances of *missing_value_placeholder* replaced with *constant_value*.

> [!NOTE]
>
> * If you create *series* using the [make-series](make-seriesoperator.md) operator, specify *null* as the default value to use interpolation functions like `series_fill_const()` afterwards. See [explanation](make-seriesoperator.md#list-of-series-interpolation-functions).
> * If *missing_value_placeholder* is `double`(*null*), or omitted, then a result may contain *null* values. To fill these *null* values, use other interpolation functions. Only [series_outliers()](series-outliersfunction.md) supports *null* values in input arrays.
> * `series_fill_const()` preserves the original type of the array elements.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc9rf7q4d68qcw5sk2d6f.northeurope/databases/MyDatabase?query=H4sIAAAAAAAAA3WOywrCMBBF94H8wywTiNJppAvFLymlxDRCJLaSxIXgxzsmPkBwFvM4XO6d4DJMJhvYl5HNIThhYtzCdJvN2VvJGVD1dbyg6BFRwXwNQYHuFGzoavWbIJGOiP6QZx8kGXA27DgriZzd4RKXk7O5mlOsgroefQijXeaUkT5LLnqXxi8URdqsG6l+9e1//QrlA3dBmsPvAAAA" target="_blank">Run the query</a>

```kusto
let data = datatable(arr: dynamic)
    [
    dynamic([111, null, 36, 41, 23, null, 16, 61, 33, null, null])   
];
data 
| project
    arr, 
    fill_const1 = series_fill_const(arr, 0.0),
    fill_const2 = series_fill_const(arr, -1)  
```

**Output**

|`arr`|`fill_const1`|`fill_const2`|
|---|---|---|
|[111,null,36,41,23,null,16,61,33,null,null]|[111,0.0,36,41,23,0.0,16,61,33,0.0,0.0]|[111,-1,36,41,23,-1,16,61,33,-1,-1]|
