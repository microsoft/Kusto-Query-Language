---
title: array_rotate_left() - Azure Data Explorer
description: Learn how to use the array_rotate_left() function to rotate values inside a dynamic array to the left.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/21/2022
---
# array_rotate_left()

Rotates values inside a `dynamic` array to the left.

## Syntax

`array_rotate_left(`*array*, *rotate_count*`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
|*array* | dynamic | &check;| Input array to rotate, must be dynamic array.|
|*rotate_count*| integer | &check;| Number of positions that array elements will be rotated to the left. If the value is negative, the elements will be rotated to the right.|

## Returns

Dynamic array containing the same amount of the elements as in original array, where each element was rotated according to *rotate_count*.

## See also

* For rotating array to the right, see [array_rotate_right()](array_rotate_rightfunction.md).
* For shifting array to the left, see [array_shift_left()](array_shift_leftfunction.md).
* For shifting array to the right, see [array_shift_right()](array_shift_rightfunction.md).

## Examples

* Rotating to the left by two positions:

 **\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1eSqUUitKEnNSwGpiC/KL0ksSU2xBbITK6G8+JzUtBINoIiOgpEmACPTVOVNAAAA)**\]**

```kusto
print arr=dynamic([1,2,3,4,5])
| extend arr_rotated=array_rotate_left(arr, 2)
```

**Results**

|arr|arr_rotated|
|---|---|
|[1,2,3,4,5]|[3,4,5,1,2]|

* Rotating to the right by two positions by using negative rotate_count value:

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1eSqUUitKEnNSwGpiC/KL0ksSU2xBbITK6G8+JzUtBINoIiOgq6RJgCXfX6MTgAAAA==)**\]**

```kusto
print arr=dynamic([1,2,3,4,5])
| extend arr_rotated=array_rotate_left(arr, -2)
```

**Results**

|arr|arr_rotated|
|---|---|
|[1,2,3,4,5]|[4,5,1,2,3]|
