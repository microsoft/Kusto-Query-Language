---
title: array_sum() - Azure Data Explorer
description: Learn how to use the array_sum() function to calculate the sum of elements in a dynamic array.

ms.reviewer: alexans
ms.topic: reference
ms.date: 09/21/2022
---
# array_sum()

Calculates the sum of elements in a dynamic array.

## Syntax

`array_sum`(*array*)

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *array*| | &check;| Array used for input.|

## Returns

Returns a double type value with the sum of the elements of the array.

> [!NOTE]
> If the array contains elements of non-numeric types, the result is `null`.

## Example

This following example shows the sum of an array.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYxidVU4OWqUUitKEnNSwGpiC8uzbUF0omVIJYGkKUJANbCqMA+AAAA)**\]**

```kusto
print arr=dynamic([1,2,3,4]) 
| extend arr_sum=array_sum(arr)
```

**Results**

|`arr`|`arr_sum`|
|---|---|
|[1,2,3,4]|10|
