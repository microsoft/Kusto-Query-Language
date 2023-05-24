---
title:  array_index_of()
description: Learn how to use the array_index_of() function to search an array for a specified item, and return its position.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/03/2022
---
# array_index_of()

Searches an array for the specified item, and returns its position.

## Syntax

`array_index_of(`*array*,*value*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *array*| dynamic | &check; | The array to search.|
| *value* | long, integer, double, datetime, timespan, decimal, string, guid, or boolean | &check; | The value to lookup. |
| *start* | number |  | The search start position. A negative value will offset the starting search value from the end of the array by `abs(start_index)` steps.
| *length* | int |  | The number of values to examine. A value of -1 means unlimited length.
| *occurrence* | int | The number of the occurrence. The default is 1.

## Returns

Returns a zero-based index position of lookup.
Returns -1 if the value isn't found in the array.
Returns *null* for irrelevant inputs (*occurrence* < 0 or  *length* < -1).

## Example

The following example shows the position number of specific words within the array.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA52T7WrCMBiF/3sVB//MQoq0dV8Mr2QMielbDUuTkqRO735pmsk2dQ5LG0jhPOfkvK0iD27tsj5o3koxe536rXRThum4cj2stOdtp+j0xVv2Mums1H4CWe8LLAcYP6ykrmm/Ms0sbNmgyRCu+RzKmPe+Q2N6XUPqcHe9h/OBsZmADZTyAuUYomBVdo5lyRG3Yks1LNcbQgJW14FlBI7ykIZb79BY0yIqUDCsY0zTOfDGk0WJHVc9OQZnvpIIru881jRGSu6LC+5DvyVbnDEeDYwQvbWkBcE0ySEh768dqGR5kZ30ndQP59VptOGoeXjKbJyXI2FCu9/C/BpdbDqRH/9DrhJZG4RPzf5gH6mRkqhPf1LzI4823MsdjS2mwX1IpWIH4B6KO48KpKgl7V3CP1/Cxx8gX9yK/wT41ILVWgMAAA==" target="_blank">Run the query</a>

```kusto
let arr=dynamic(["this", "is", "an", "example", "an", "example"]);
print
 idx1 = array_index_of(arr,"an")    // lookup found in input string
 , idx2 = array_index_of(arr,"example",1,3) // lookup found in researched range 
 , idx3 = array_index_of(arr,"example",1,2) // search starts from index 1, but stops after 2 values, so lookup can't be found
 , idx4 = array_index_of(arr,"is",2,4) // search starts after occurrence of lookup
 , idx5 = array_index_of(arr,"example",2,-1)  // lookup found
 , idx6 = array_index_of(arr, "an", 1, -1, 2)   // second occurrence found in input range
 , idx7 = array_index_of(arr, "an", 1, -1, 3)   // no third occurrence in input array
 , idx8 = array_index_of(arr, "an", -3)   // negative start index will look at last 3 elements
 , idx9 = array_index_of(arr, "is", -4)   // negative start index will look at last 3 elements
```

**Output**

|idx1|idx2|idx3|idx4|idx5|idx6|idx7|idx8|idx9|
|----|----|----|----|----|----|----|----|----|
|2   |3   |-1  |-1   |3   |4   |-1  |4  |-1  |

## See also

Use [set_has_element(`arr`, `value`)](sethaselementfunction.md) to check whether a value exists in an array. This function will improve the readability of your query. Both functions have the same performance.
