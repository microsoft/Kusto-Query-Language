---
title: array_shift_right() - Azure Data Explorer
description: Learn how to use the array_shift_right() function to shift values inside a dynamic array to the right.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/21/2022
---
# array_shift_right()

Shifts the values inside a dynamic array to the right.

## Syntax

`array_shift_right(`*array*, *shift_count* [, *fill_value* ]`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
|*array* | dynamic |&check; | Input array to shift, must be dynamic array.|
|*shift_count* | integer | &check; | Number of positions that array elements will be shifted to the right. If the value is negative, the elements will be shifted to the left. |
|*fill_value* | scalar | &check; | Value used for inserting elements instead of the ones that were shifted and removed. The default is null or an empty string depending on the *array* type.|

## Returns

Returns a dynamic array containing the same amount of the elements as in the original array. Each element has been shifted according to *shift_count*. New elements that are added instead of the removed elements will have a value of *fill_value*.

## See also

* For shifting array left, see [array_shift_left()](array_shift_leftfunction.md).
* For rotating array right, see [array_rotate_right()](array_rotate_rightfunction.md).
* For rotating array left, see [array_rotate_left()](array_rotate_leftfunction.md).

## Examples

* Shifting to the right by two positions:

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1eTlqlFIrShJzUsBKYkvzshMK7EFshIrIez4osz0jBINoIiOgpEmAKRlW6FMAAAA)**\]**

```kusto
print arr=dynamic([1,2,3,4,5])
| extend arr_shift=array_shift_right(arr, 2)
```

**Results**

|arr|arr_shift|
|---|---|
|[1,2,3,4,5]|[null,null,1,2,3]|

* Shifting to the right by two positions and adding a default value:

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1eTlqlFIrShJzUsBKYkvzshMK7EFshIrIez4osz0jBINoIiOgpGOgq6hJgBHJWeJUAAAAA==)**\]**

```kusto
print arr=dynamic([1,2,3,4,5])
| extend arr_shift=array_shift_right(arr, 2, -1)
```

**Results**

|arr|arr_shift|
|---|---|
|[1,2,3,4,5]|[-1,-1,1,2,3]|

* Shifting to the left by two positions by using a negative shift_count value:

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1eTlqlFIrShJzUsBKYkvzshMK7EFshIrIez4osz0jBINoIiOgq4REBtqAgCqvHZwUQAAAA==)**\]**

```kusto
print arr=dynamic([1,2,3,4,5])
| extend arr_shift=array_shift_right(arr, -2, -1)
```

**Results**

|arr|arr_shift|
|---|---|
|[1,2,3,4,5]|[3,4,5,-1,-1]|
