---
title: array_shift_left() - Azure Data Explorer
description: This article describes array_shift_left() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 08/11/2019
---
# array_shift_left()

Shifts the values inside a `dynamic` array to the left.

## Syntax

`array_shift_left(`*`arr`*, *`shift_count`* `[,` *`fill_value`* ]`)`

## Arguments

* *`arr`*: Input array to split, must be dynamic array.
* *`shift_count`*: Integer specifying the number of positions that array elements will be shifted to the left. If the value is negative, the elements will be shifted to the right.
* *`fill_value`*: Scalar value that is used for inserting elements instead of the ones that were shifted and removed. Default: null value or empty string (depending on the *`arr`* type).

## Returns

Dynamic array containing the same number of elements as in the original array. Each element has been shifted according to *shift_count*. New elements that are added in place of removed elements will have a value of *fill_value*.

## See also

* For shifting array right, see [array_shift_right()](array_shift_rightfunction.md).
* For rotating array right, see [array_rotate_right()](array_rotate_rightfunction.md).
* For rotating array left, see [array_rotate_left()](array_rotate_leftfunction.md).

## Examples

* Shifting to the left by two positions:

    <!-- csl: https://help.kusto.windows.net:443/Samples -->
    ```kusto
    print arr=dynamic([1,2,3,4,5]) 
    | extend arr_shift=array_shift_left(arr, 2)
    ```
    
    |`arr`|`arr_shift`|
    |---|---|
    |[1,2,3,4,5]|[3,4,5,null,null]|

* Shifting to the left by two positions and adding default value:

    <!-- csl: https://help.kusto.windows.net:443/Samples -->
    ```kusto
    print arr=dynamic([1,2,3,4,5]) 
    | extend arr_shift=array_shift_left(arr, 2, -1)
    ```
    
    |`arr`|`arr_shift`|
    |---|---|
    |[1,2,3,4,5]|[3,4,5,-1,-1]|


* Shifting to the right by two positions by using negative *shift_count* value:

    <!-- csl: https://help.kusto.windows.net:443/Samples -->
    ```kusto
    print arr=dynamic([1,2,3,4,5]) 
    | extend arr_shift=array_shift_left(arr, -2, -1)
    ```
    
    |`arr`|`arr_shift`|
    |---|---|
    |[1,2,3,4,5]|[-1,-1,1,2,3]|
