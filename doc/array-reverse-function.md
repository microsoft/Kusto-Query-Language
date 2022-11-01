---
title: array_reverse() - Azure Data Explorer
description: Learn how to use the array_reverse() function to reverse the order of the elements in a dynamic array.
ms.reviewer: slneimer
ms.topic: reference
ms.date: 09/21/2022
---
# array_reverse()

Reverses the order of the elements in a dynamic array.

## Syntax

`array_reverse(`*array*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
|*array*|  | &check;| Input array to reverse.|

## Returns

Returns an array that contains exactly the same elements as the input array, but in reverse order.

## Example

This example shows an array of words reversed.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1ohWKsnILFbSUVCCkIl5IDK1IjG3ICdVKVZTgZerRqGgKD8rNblEISi1uDSnxBaoP7Eyvii1LLWoOFUDyNMEAKks9PlYAAAA)**\]**

```kusto
print arr=dynamic(["this", "is", "an", "example"]) 
| project Result=array_reverse(arr)
```

**Results**

|Result|
|---|
|["example","an","is","this"]|
