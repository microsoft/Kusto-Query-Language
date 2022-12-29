---
title: array_shift_right() - Azure Data Explorer
description: Learn how to use the array_shift_right() function to shift values inside a dynamic array to the right.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/20/2022
---
# array_shift_right()

Shifts the values inside a dynamic array to the right.

## Syntax

`array_shift_right(`*array*, *shift_count* [, *default_value* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*array* | dynamic |&check; | The array to shift.|
|*shift_count* | integer | &check; | The number of positions that array elements will be shifted to the right. If the value is negative, the elements will be shifted to the left. |
|*default_value* | scalar | | The value used for an element that was shifted and removed. The default is null or an empty string depending on the type of elements in the *array*.|

## Returns

Returns a dynamic array containing the same amount of the elements as in the original array. Each element has been shifted according to *shift_count*. New elements that are added instead of the removed elements will have a value of *default_value*.

## Examples

Shifting to the right by two positions:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1eTlqlFIrShJzUsBKYkvzshMK7EFshIrIez4osz0jBINoIiOgpEmAKRlW6FMAAAA" target="_blank">Run the query</a>

```kusto
print arr=dynamic([1,2,3,4,5])
| extend arr_shift=array_shift_right(arr, 2)
```

**Output**

|arr|arr_shift|
|---|---|
|[1,2,3,4,5]|[null,null,1,2,3]|

Shifting to the right by two positions and adding a default value:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1eTlqlFIrShJzUsBKYkvzshMK7EFshIrIez4osz0jBINoIiOgpGOgq6hJgBHJWeJUAAAAA==" target="_blank">Run the query</a>

```kusto
print arr=dynamic([1,2,3,4,5])
| extend arr_shift=array_shift_right(arr, 2, -1)
```

**Output**

|arr|arr_shift|
|---|---|
|[1,2,3,4,5]|[-1,-1,1,2,3]|

Shifting to the left by two positions by using a negative shift_count value:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKrJNqcxLzM1M1og21DHSMdYx0TGN1eTlqlFIrShJzUsBKYkvzshMK7EFshIrIez4osz0jBINoIiOgq4REBtqAgCqvHZwUQAAAA==" target="_blank">Run the query</a>

```kusto
print arr=dynamic([1,2,3,4,5])
| extend arr_shift=array_shift_right(arr, -2, -1)
```

**Output**

|arr|arr_shift|
|---|---|
|[1,2,3,4,5]|[3,4,5,-1,-1]|

## See also

* To shift an array to the left, use [array_shift_left()](array_shift_leftfunction.md).
* To rotate an array to the right, use [array_rotate_right()](array_rotate_rightfunction.md).
* To rotate an array to the left, use [array_rotate_left()](array_rotate_leftfunction.md).
