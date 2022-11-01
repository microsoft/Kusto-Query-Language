---
title: array_concat() - Azure Data Explorer
description: Learn how to use the array_concat() function to concatenate many dynamic arrays to a single array.
ms.reviewer: alexans
ms.topic: reference
ms.date: 10/23/2018
---
# array_concat()

Concatenates many dynamic arrays to a single array.

## Syntax

`array_concat(`*arr1*`[`, `*arr2*, ...]`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *arr1...arrN* | dynamic | &check; |Arrays to be concatenated into a dynamic array. All arguments must be dynamic arrays (see [pack_array](packarrayfunction.md)).|

## Returns

Returns a dynamic array of arrays with arr1, arr2, ... , arrN.

## Example

The following example shows concatenated arrays.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA13LMQ6DMAxG4b1S7/CPBGUJnXsWZKUuEogkcj3EiMMTmCrWT+8JpYlR8ZW8IkAzXvgpF4TnYwdX5fSB4d2SHsOfbc3sZhQaForLSCJkXfXmN+dBw91h7vyK5Jmj4uIx5hRJOwrn4Q5bQXxcmgAAAA==)**\]**

```kusto
range x from 1 to 3 step 1
| extend y = x * 2
| extend z = y * 2
| extend a1 = pack_array(x,y,z), a2 = pack_array(x, y)
| project array_concat(a1, a2)
```

**Results**

|Column1|
|---|
|[1,2,4,1,2]|
|[2,4,8,2,4]|
|[3,6,12,3,6]|
