---
title: array_sort_asc() - Azure Data Explorer
description: This article describes array_sort_asc() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: slneimer
ms.service: data-explorer
ms.topic: reference
ms.date: 09/22/2020
---
# array_sort_asc()

Receives one or more arrays. Sorts the first array in ascending order. Orders the remaining arrays to match the reordered first array.

## Syntax

`array_sort_asc(`*array1*[, ..., *argumentN*]`)`

`array_sort_asc(`*array1*[, ..., *argumentN*]`,`*nulls_last*`)`

If *nulls_last* isn't provided, a default value of `true` is used.

## Arguments

* *array1...arrayN*: Input arrays.
* *nulls_last*: A bool indicating whether `null`s should be last

## Returns

Returns the same number of arrays as in the input, with the first array sorted in ascending order, and the remaining arrays ordered to match the reordered first array.

`null` will be returned for every array that differs in length from the first one.

If an array contains elements of different types, it will be sorted in the following order:

* Numeric, `datetime`, and `timespan` elements
* String elements
* Guid elements
* All other elements

## Example 1 - Sorting two arrays

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let array1 = dynamic([1,3,4,5,2]);
let array2 = dynamic(["a","b","c","d","e"]);
print array_sort_asc(array1,array2)
```

|`array1_sorted`|`array2_sorted`|
|---|---|
|[1,2,3,4,5]|["a","e","b","c","d"]|

> [!Note]
> The output column names are generated automatically, based on the arguments to the function. To assign different names to the output columns, use the following syntax: `... | extend (out1, out2) = array_sort_asc(array1,array2)`

## Example 2 - Sorting substrings

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let Names = "John,Paul,George,Ringo";
let SortedNames = strcat_array(array_sort_asc(split(Names, ",")), ",");
print result = SortedNames
```

|`result`|
|---|
|George,John,Paul,Ringo|

## Example 3 - Combining summarize and array_sort_asc

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
datatable(command:string, command_time:datetime, user_id:string)
[
    'chmod',   datetime(2019-07-15),   "user1",
    'ls',      datetime(2019-07-02),   "user1",
    'dir',     datetime(2019-07-22),   "user1",
    'mkdir',   datetime(2019-07-14),   "user1",
    'rm',      datetime(2019-07-27),   "user1",
    'pwd',     datetime(2019-07-25),   "user1",
    'rm',      datetime(2019-07-23),   "user2",
    'pwd',     datetime(2019-07-25),   "user2",
]
| summarize timestamps = make_list(command_time), commands = make_list(command) by user_id
| project user_id, commands_in_chronological_order = array_sort_asc(timestamps, commands)[1]
```

|`user_id`|`commands_in_chronological_order`|
|---|---|
|user1|[<br>  "ls",<br>  "mkdir",<br>  "chmod",<br>  "dir",<br>  "pwd",<br>  "rm"<br>]|
|user2|[<br>  "rm",<br>  "pwd"<br>]|

> [!Note]
> If your data may contain `null` values, use [make_list_with_nulls](make-list-with-nulls-aggfunction.md) instead of [make_list](makelist-aggfunction.md).

## Example 4 - Controlling location of `null` values

By default, `null` values are put last in the sorted array. However, you can control it explicitly by adding a `bool` value as the last argument to `array_sort_asc()`.

Example with default behavior:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print array_sort_asc(dynamic([null,"blue","yellow","green",null]))
```

|`print_0`|
|---|
|["blue","green","yellow",null,null]|

Example with non-default behavior:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print array_sort_asc(dynamic([null,"blue","yellow","green",null]), false)
```

|`print_0`|
|---|
|[null,null,"blue","green","yellow"]|

## See also

To sort the first array in descending order, use [array_sort_desc()](arraysortdescfunction.md).
