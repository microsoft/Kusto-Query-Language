---
title: set_has_element() - Azure Data Explorer
description: This article describes set_has_element() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/23/2020
---
# set_has_element()

Determines whether the specified set contains the specified element.

## Syntax

`set_has_element(`*array*,*value*`)`

## Arguments

* *array*: Input array to search.
* *value*: Value to search for. The value should be of type `long`, `integer`, `double`, `datetime`, `timespan`, `decimal`, `string`, `guid`, or `boolean`.

## Returns

True or false depending on if the value exists in the array.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print arr=dynamic(["this", "is", "an", "example"]) 
| project Result=set_has_element(arr, "example")
```

**Output**

|Result|
|---|
|1|

## See also

Use [`array_index_of(arr, value)`](arrayindexoffunction.md) to find the position at which the value exists in the array. Both functions are equally performant.
