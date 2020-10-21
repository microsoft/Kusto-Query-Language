---
title: array_index_of() - Azure Data Explorer
description: This article describes array_index_of() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 01/22/2020
---
# array_index_of()

Searches the array for the specified item, and returns its position.

## Syntax

`array_index_of(`*array*,*value*`)`

## Arguments

* *array*: Input array to search.
* *value*: Value to search for. The value should be of type long, integer, double, datetime, timespan, decimal, string, or guid.

## Returns

Zero-based index position of lookup.
Returns -1 if the value isn't found in the array.

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print arr=dynamic(["this", "is", "an", "example"]) 
| project Result=array_index_of(arr, "example")
```

|Result|
|---|
|3|

## See also

If you only want to check whether a value exists in an array,
but you are not interested in its position, you can use
[set_has_element(`arr`, `value`)](sethaselementfunction.md). This function will improve the readability of your query. Both functions have the same performance.
