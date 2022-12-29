---
title: array_length() - Azure Data Explorer
description: Learn how to use the array_length() function to calculate the number of elements in a dynamic array.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/20/2022
---
# array_length()

Calculates the number of elements in a dynamic array.

> **Deprecated aliases:** arraylength()

## Syntax

`array_length(`*array*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *array* | dynamic | &check; | The array for which to calculate length.

## Returns

Returns the number of elements in *array*, or `null` if *array* isn't an array.

## Examples

The following example shows the number of elements in the array.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKkqsjM9JzUsvydAoSCwqTo3PKs7P01CPNtRRMNJRMNZRUErLLy1SilXX1AQAe4KK2TMAAAA=" target="_blank">Run the query</a>

```kusto
print array_length(dynamic([1, 2, 3, "four"]))
```

**Output**

|print_0|
|--|
|4|
