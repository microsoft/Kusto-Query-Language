---
title: array_reverse() - Azure Data Explorer
description: Learn how to use the array_reverse() function to reverse the order of the elements in a dynamic array.
ms.reviewer: slneimer
ms.topic: reference
ms.date: 11/20/2022
---
# array_reverse()

Reverses the order of the elements in a dynamic array.

## Syntax

`array_reverse(`*value*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*value*| dynamic | &check;| The array to reverse.|

## Returns

Returns an array that contains the same elements as the input array in reverse order.

## Example

This example shows an array of words reversed.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1ohWKsnILFbSUVCCkIl5IDK1IjG3ICdVKVZTgZerRqGgKD8rNblEISi1uDSnxBaoP7Eyvii1LLWoOFUDyNMEAKks9PlYAAAA" target="_blank">Run the query</a>

```kusto
print arr=dynamic(["this", "is", "an", "example"]) 
| project Result=array_reverse(arr)
```

**Output**

|Result|
|---|
|["example","an","is","this"]|
