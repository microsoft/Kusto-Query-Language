---
title:  series_divide()
description: Learn how to use the series_divide() function to calculate the element-wise division of two numeric series inputs.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/22/2023
---
# series_divide()

Calculates the element-wise division of two numeric series inputs.

## Syntax

`series_divide(`*series1*`,` *series2*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series1, series2* | dynamic | &check; | The numeric arrays over which to calculate the element-wise division. The first array is to be divided by the second. |

## Returns

Dynamic array of calculated element-wise divide operation between the two inputs. Any non-numeric element or non-existing element (arrays of different sizes) yields a `null` element value.

Note: the result series is of double type, even if the inputs are integers. Division by zero follows the double division by zero (e.g. 2/0 yields double(+inf)).

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA1XMQQrCMBBG4b2n+JeNzCZ17VlCaEaJYhNmgiShhzdKobj9eDzx651RcZP0gkVJuEALZ9jTBq6F14CG6yjOmA/qg9pOWdKDlwK1A7Nfns6L+DZVatQNQed/74RGqOa4qXUhvmNg92uVJbLuNKn9LswHwOb0BqkAAAA=" target="_blank">Run the query</a>

```kusto
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| project s1 = pack_array(x,y,z), s2 = pack_array(z, y, x)
| extend s1_divide_s2 = series_divide(s1, s2)
```

**Output**

|s1	        |s2|	    s1_divide_s2|
|---|---|---|
|[1,2,4]	|[4,2,1]|	[0.25,1.0,4.0]|
|[2,4,8]	|[8,4,2]|	[0.25,1.0,4.0]|
|[3,6,12]	|[12,6,3]|	[0.25,1.0,4.0]|
