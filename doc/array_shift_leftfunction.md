---
title: array_shift_left() - Azure Data Explorer
description: Learn how to use the array_shift_left() function to shift the values inside a dynamic array to the left.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/21/2022
---
# array_shift_left()

Shifts the values inside a `dynamic` array to the left.

## Syntax

`array_shift_left(`*array*, *shift_count* `[,` *fill_value* ]`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
|*array* | dynamic |&check; | Input array to shift, must be dynamic array.|
|*shift_count* | integer | &check; | Number of positions that array elements will be shifted to the left. If the value is negative, the elements will be shifted to the right. |
|*fill_value* | scalar | &check; | Value used for inserting elements instead of the ones that were shifted and removed. The default is null or an empty string depending on the *array* type.|

## Returns

Returns a dynamic array containing the same number of elements as in the original array. Each element has been shifted according to *shift_count*. New elements that are added in place of removed elements will have a value of *fill_value*.

## See also

* For shifting array right, see [array_shift_right()](array_shift_rightfunction.md).
* For rotating array right, see [array_rotate_right()](array_rotate_rightfunction.md).
* For rotating array left, see [array_rotate_left()](array_rotate_leftfunction.md).

## Examples

* Shifting to the left by two positions:

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1eTlqlFIrShJzUsBKYkvzshMK7EFshIrIez4nNS0Eg2ggI6CkSYADEKYSUsAAAA=)**\]**

```kusto
print arr=dynamic([1,2,3,4,5])
| extend arr_shift=array_shift_left(arr, 2)
```

**Results**

|`arr`|`arr_shift`|
|---|---|
|[1,2,3,4,5]|[3,4,5,null,null]|

* Shifting to the left by two positions and adding default value:

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1eTlqlFIrShJzUsBKYkvzshMK7EFshIrIez4nNS0Eg2ggI6CkY6CrqEmAA+qvHJPAAAA)**\]**

```kusto
print arr=dynamic([1,2,3,4,5])
| extend arr_shift=array_shift_left(arr, 2, -1)
```

**Results**

|`arr`|`arr_shift`|
|---|---|
|[1,2,3,4,5]|[3,4,5,-1,-1]|

* Shifting to the right by two positions by using negative *shift_count* value:

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1eTlqlFIrShJzUsBKYkvzshMK7EFshIrIez4nNS0Eg2ggI6CrhEQG2oCAIeuighQAAAA)**\]**

```kusto
print arr=dynamic([1,2,3,4,5])
| extend arr_shift=array_shift_left(arr, -2, -1)
```

**Results**

|`arr`|`arr_shift`|
|---|---|
|[1,2,3,4,5]|[-1,-1,1,2,3]|
