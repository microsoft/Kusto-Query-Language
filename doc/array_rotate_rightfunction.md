---
title: array_rotate_right() - Azure Data Explorer
description: This article describes array_rotate_right() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 08/11/2019
---
# array_rotate_right()

Rotates values inside a `dynamic` array to the right.

## Syntax

`array_rotate_right(`*arr*, *rotate_count*`)`

## Arguments

* *arr*: Input array to split, must be dynamic array.
* *rotate_count*: Integer specifying the number of positions that array elements will be rotated to the right. If the value is negative, the elements will be rotated to the left.

## Returns

Dynamic array containing the same amount of the elements as in the original array, where each element was rotated according to *rotate_count*.

## See also

* For rotating array to the left, see [array_rotate_left()](array_rotate_leftfunction.md).
* For shifting array to the left, see [array_shift_left()](array_shift_leftfunction.md).
* For shifting array to the right, see [array_shift_right()](array_shift_rightfunction.md).

## Examples

* Rotating to the right by two positions:

    <!-- csl: https://help.kusto.windows.net:443/Samples -->
    ```kusto
    print arr=dynamic([1,2,3,4,5]) 
    | extend arr_rotated=array_rotate_right(arr, 2)
    ```
    
    |arr|arr_rotated|
    |---|---|
    |[1,2,3,4,5]|[4,5,1,2,3]|

* Rotating to the left by two positions by using negative rotate_count value:

    <!-- csl: https://help.kusto.windows.net:443/Samples -->
    ```kusto
    print arr=dynamic([1,2,3,4,5]) 
    | extend arr_rotated=array_rotate_right(arr, -2)
    ```
    
    |arr|arr_rotated|
    |---|---|
    |[1,2,3,4,5]|[3,4,5,1,2]|
