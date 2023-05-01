---
title: set_union() - Azure Data Explorer
description: Learn how to use the set_union() function to create a union set of all the  distinct values in all of the array inputs.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/05/2023
---
# set_union()

Returns a `dynamic` array of the set of all distinct values that are in any of the arrays - (arr1 ∪ arr2 ∪ ...).

## Syntax

`set_union(`*set1*`,` *set2* [`,` *set3*, ...]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *set1...setN* | dynamic | &check; | Arrays used to create a union set. A minimum of 2 arrays are required. See [pack_array](packarrayfunction.md).|

## Returns

Returns a dynamic array of the set of all distinct values that are in any of arrays.

## Example

### Set from multiple dynamic array

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA13MMQ7CMAyF4Z1TvLFBXprMPQNHqKwSECCSKHXVJOLwuBMK62e/P3O4exTccnxjhEQ4rOITxtMHvogPV1RM+nGG/VFTqj3tSq0nHtUSL6+Zc+Y6FKpUqBkC2/8L6sGu591oLOX49ItojdgSO7psMq1e5i08YhiUNXdszRcYE8jtzQAAAA==" target="_blank">Run the query</a>

```kusto
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| extend w = z * 2
| extend a1 = pack_array(x,y,x,z), a2 = pack_array(x, y), a3 = pack_array(w)
| project a1,a2,a3,Out=set_union(a1, a2, a3)
```

**Output**

|a1|a2|a3|`Out`|
|---|---|---|---|
|[1,2,1,4]|[1,2]|[8]|[1,2,4,8]|
|[2,4,2,8]|[2,4]|[16]|[2,4,8,16]|
|[3,6,3,12]|[3,6]|[24]|[3,6,12,24]|

### Set from one dynamic array

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUBQ3HoiJDK4WUyrzE3MxkTa5oLgUggHI1otUdTdR1FNQdjcCkOZQdq6mjgKbOGazOGazC2RBMmgDVccUqcNUopFaUpOalKPiXltgWp5bEl+Zl5ueBLdZRAJGaAMqBZpGPAAAA" target="_blank">Run the query</a>

```kusto
datatable (Arr1: dynamic)
[
    dynamic(['A4', 'A2', 'A7', 'A2']), 
    dynamic(['C4', 'C7', 'C1', 'C4'])
] 
| extend Out=set_union(Arr1, Arr1)
```

**Output**

|Arr1|`Out`|
|---|---|
|["A4","A2","A7","A2"]|["A4","A2","A7"]|
|["C4","C7","C1","C4"]|["C4","C7","C1"]|

## See also

* [`set_intersect()`](setintersectfunction.md)
* [`set_difference()`](setdifferencefunction.md)
