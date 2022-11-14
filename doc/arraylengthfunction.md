---
title: array_length() - Azure Data Explorer
description: Learn how to use the array_length() function to calculate the number of elements in a dynamic array.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/09/2022
---
# array_length()

Calculates the number of elements in a dynamic array.

> **Deprecated aliases:** arraylength()

## Syntax

`array_length(`*array*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *array* |dynamic | &check; | A `dynamic` value.

## Returns

Returns the number of elements in *array*, or `null` if *array* isn't an array.

## Examples

The following example shows the number of elements in the array.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKkqsjM9JzUsvydAoSCwqTo3PKs7P01CPNtRRMNJRMNZRUErLLy1SilXX1AQAe4KK2TMAAAA=)**\]**

```kusto
print array_length(parse_json('[1, 2, 3, "four"]'))
```

**Results**

|print_0|
|--|
|4|
