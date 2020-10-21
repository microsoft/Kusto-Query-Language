---
title: array_slice() - Azure Data Explorer
description: This article describes array_slice() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 12/03/2018
---
# array_slice()

Extracts a slice of a dynamic array.

## Syntax

`array_slice`(*`arr`*, *`start`*, *`end`*)

## Arguments

* *`arr`*: Input array to extract the slice from must be dynamic array.
* *`start`*: zero-based (inclusive) start index of the slice, negative values are converted to array_length+start.
* *`end`*: zero-based (inclusive) end index of the slice, negative values are converted to array_length+end.

Note: out of bounds indices are ignored.

## Returns

Dynamic array of the values in the range [`start..end`] from `arr`.

## Examples

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print arr=dynamic([1,2,3]) 
| extend sliced=array_slice(arr, 1, 2)
```
|`arr`|`sliced`|
|---|---|
|[1,2,3]|[2,3]|

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print arr=dynamic([1,2,3,4,5]) 
| extend sliced=array_slice(arr, 2, -1)
```
|`arr`|sliced|
|---|---|
|[1,2,3,4,5]|[3,4,5]|

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print arr=dynamic([1,2,3,4,5]) 
| extend sliced=array_slice(arr, -3, -2)
```
|`arr`|sliced|
|---|---|
|[1,2,3,4,5]|[3,4]|
