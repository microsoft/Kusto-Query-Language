---
title: array_split() - Azure Data Explorer
description: Learn how to use the array_split() function to split an array into multiple arrays.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/21/2022
---
# array_split()

Splits an array to multiple arrays according to the split indices and packs the generated array in a dynamic array.

## Syntax

`array_split`(*array*, *`indices`*)

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *array*| dynamic| &check; |Array to split.|
| *indices* | integer | &check;| Split indices (zero based). This can be a single integer or a dynamic array of integers. Negative values are converted to `array_length` + `value`.

## Returns

Returns a dynamic array containing N+1 arrays with the values in the range `[0..i1), [i1..i2), ... [iN..array_length)` from `array`, where N is the number of input indices and `i1...iN` are the indices.

## Examples

This following example shows how to split and array.

**\[**[**Click to run query**](https://dataexplorer.azure.com/?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1VTgqlFIrShJzUsBKYkvLsjJLLEFshIrIWwNIFtHwUgTAB7YikBGAAAA)**\]**

```kusto
print arr=dynamic([1,2,3,4,5]) 
| extend arr_split=array_split(arr, 2)
```

**Results**

|`arr`|`arr_split`|
|---|---|
|[1,2,3,4,5]|[[1,2],[3,4,5]]|

**\[**[**Click to run query**](https://dataexplorer.azure.com/?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1VTgqlFIrShJzUsBKYkvLsjJLLEFshIrIWwNIFtHAUmncaymJgD5vl9PUwAAAA==)**\]**

```kusto
print arr=dynamic([1,2,3,4,5]) 
| extend arr_split=array_split(arr, dynamic([1,3]))
```

**Results**

|`arr`|`arr_split`|
|---|---|
|[1,2,3,4,5]|[[1],[2,3],[4,5]]|
