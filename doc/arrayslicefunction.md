---
title:  array_slice()
description: Learn how to use the array_slice() function to extract a slice of a dynamic array.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/03/2022
---
# array_slice()

Extracts a slice of a dynamic array.

## Syntax

`array_slice`(*array*, *start*, *end*)

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *array* | dynamic | &check; | The array from which to extract the slice.|
| *start*| int | &check; | The start index of the slice (inclusive). Negative values are converted to `array_length`+`start`.|
| *end*| int | &check; | The last index of the slice. (inclusive). Negative values are converted to `array_length`+`end`.|

> [!NOTE]
> Out of bounds indices are ignored.

## Returns

Returns a dynamic array of the values in the range [`start..end`] from `array`.

## Examples

The following examples return a slice of the array.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMY7VVOCqUUitKEnNS1EozslMTk2xBapKrIwHczSAbB0FQx0FI00AeoUyQ0IAAAA=" target="_blank">Run the query</a>

```kusto
print arr=dynamic([1,2,3]) 
| extend sliced=array_slice(arr, 1, 2)
```

**Output**

|arr|sliced|
|---|---|
|[1,2,3]|[2,3]|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1VTgqlFIrShJzUtRKM7JTE5NsQWqTKyMB3M0gGwdBSMdBV1DTQAv2T4vRwAAAA==" target="_blank">Run the query</a>

```kusto
print arr=dynamic([1,2,3,4,5]) 
| extend sliced=array_slice(arr, 2, -1)
```

**Output**

|arr|sliced|
|---|---|
|[1,2,3,4,5]|[3,4,5]|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1VTgqlFIrShJzUtRKM7JTE5NsQWqTKyMB3M0gGwdBV1jIDbSBABajMjTSAAAAA==" target="_blank">Run the query</a>

```kusto
print arr=dynamic([1,2,3,4,5]) 
| extend sliced=array_slice(arr, -3, -2)
```

**Output**

|arr|sliced|
|---|---|
|[1,2,3,4,5]|[3,4]|
