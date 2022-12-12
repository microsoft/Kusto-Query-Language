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

[**Run the query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKkqsjM9JzUsvydBIqcxLzM1M1og21FEw0lEw1lFQSssvLVKK1dQEAI1OgS0uAAAA)

```kusto
print array_length(dynamic([1, 2, 3, "four"]))
```

|print_0|
|--|
|4|
