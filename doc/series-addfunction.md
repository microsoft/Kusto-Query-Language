---
title: series_add() - Azure Data Explorer
description: Learn how to use the series_add() function to calculate the element-wise addition of two numeric series inputs.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/22/2023
---
# series_add()

Calculates the element-wise addition of two numeric series inputs.

## Syntax

`series_add(`*series1*`,` *series2*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *series1, series2* | dynamic | &check; | The numeric arrays to be element-wise added into a dynamic array result. |

## Returns

Dynamic array of calculated element-wise add operation between the two inputs. Any non-numeric element or non-existing element (arrays of different sizes) yields a `null` element value.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA1XMwQrCMBAE0LtfMcdGctl69lvC0qyiYhN2c0iCH99UhOJxHjOjvN4FFTdNbxBKwgVWJINOH0gtskY0XEfjjPmgPqj9KGt6ylJgNDDz8gqsym2qvvnuPGz+9+7RPKo73owCxxi+RRN9iO15MtrHbgM/wNkBowAAAA==" target="_blank">Run the query</a>

```kusto
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| project s1 = pack_array(x,y,z), s2 = pack_array(z, y, x)
| extend s1_add_s2 = series_add(s1, s2)
```

**Output**

|s1|s2|s1_add_s2|
|---|---|---|
|[1,2,4]|[4,2,1]|[5,4,5]|
|[2,4,8]|[8,4,2]|[10,8,10]|
|[3,6,12]|[12,6,3]|[15,12,15]|
