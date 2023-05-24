---
title:  array_sort_asc()
description: Learn how to use the array_sort_asc() function to sort arrays in ascending order.
ms.reviewer: slneimer
ms.topic: reference
ms.date: 09/21/2022
---
# array_sort_asc()

Receives one or more arrays. Sorts the first array in ascending order. Orders the remaining arrays to match the reordered first array.

## Syntax

`array_sort_asc(`*array1*[, ..., *argumentN*]`)`

`array_sort_asc(`*array1*[, ..., *argumentN*]`,`*nulls_last*`)`

If *nulls_last* isn't provided, a default value of `true` is used.

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*array1...arrayN*| dynamic | &check; | The array or list of arrays to sort.|
| *nulls_last* | bool |  | Determines whether `null`s should be last.|

## Returns

Returns the same number of arrays as in the input, with the first array sorted in ascending order, and the remaining arrays ordered to match the reordered first array.

`null` will be returned for every array that differs in length from the first one.

If an array contains elements of different types, it will be sorted in the following order:

* Numeric, `datetime`, and `timespan` elements
* String elements
* Guid elements
* All other elements

## Example 1 - Sorting two arrays

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFILCpKrDRUsFVIqcxLzM1M1og21DHWMdEx1TGK1bTmyoEpMUJWopSopKOUBMTJQJwCxKlKIMUFRZl5UOXxxflFJfGJxckaEAt0IIZoAgAts93scwAAAA==" target="_blank">Run the query</a>

```kusto
let array1 = dynamic([1,3,4,5,2]);
let array2 = dynamic(["a","b","c","d","e"]);
print array_sort_asc(array1,array2)
```

**Output**

|array1_sorted|array2_sorted|
|---|---|
|[1,2,3,4,5]|["a","e","b","c","d"]|

> [!NOTE]
> The output column names are generated automatically, based on the arguments to the function. To assign different names to the output columns, use the following syntax: `... | extend (out1, out2) = array_sort_asc(array1,array2)`

## Example 2 - Sorting substrings

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVHwS8xNLVawVVDyys/I0wlILM3RcU/NL0pP1QnKzEvPV7LmygGqCs4vKklNgaktLilKTiyJTywqSqzUAJPxxUAF8YnFyRrFBTmZJRpglToKSjpKmpoQypqroCgzr0ShKLW4NKcEaAiSkQD+ChdoiAAAAA==" target="_blank">Run the query</a>

```kusto
let Names = "John,Paul,George,Ringo";
let SortedNames = strcat_array(array_sort_asc(split(Names, ",")), ",");
print result = SortedNames
```

**Output**

|result|
|---|
|George,John,Paul,Ringo|

## Example 3 - Combining summarize and array_sort_asc

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA5WR0WoDIRBF3/crJC9ZYQO7tiU00C8JQSYqiY2uy4yhpPTjq23sFmIK1ZdxOPdy5WqI6e6daVXwHka9oYh2PHTs+pbRerPREE0eOnYmg9IWjDfbhqWzVEcf9LJLY0Fb0Q/Pq369Gp543i+yclh037yjL7jG96LCa4tXwQ0varw/FcVtnscKj/5uHrGu8NObvpun9t+//B9mXvzTP/O75oPROZWF9t2wjFIEPxF7YR5ORjpLsf3dJv8pt8pwtr+UmpP1hOHVqFg2s1baUaojhjG4cLAKnAyoDSZLQISLpIBRAql2jjSL+XbYfQJov9dSfAIAAA==" target="_blank">Run the query</a>

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

**Output**

|user_id|commands_in_chronological_order|
|---|---|
|user1|[<br>  "ls",<br>  "mkdir",<br>  "chmod",<br>  "dir",<br>  "pwd",<br>  "rm"<br>]|
|user2|[<br>  "rm",<br>  "pwd"<br>]|

> [!NOTE]
> If your data may contain `null` values, use [make_list_with_nulls](make-list-with-nulls-aggfunction.md) instead of [make_list](makelist-aggfunction.md).

## Example 4 - Controlling location of `null` values

By default, `null` values are put last in the sorted array. However, you can control it explicitly by adding a `bool` value as the last argument to `array_sort_asc()`.

Example with default behavior:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUgsKkqsjC/OLyqJTyxO1kipzEvMzUzWiM4rzcnRUUrKKU1V0lGqTM3JyS8HMtKLUlPzlHRAkrGamgDOvUliQgAAAA==" target="_blank">Run the query</a>

```kusto
print array_sort_asc(dynamic([null,"blue","yellow","green",null]))
```

**Output**

|print_0|
|---|
|["blue","green","yellow",null,null]|

Example with non-default behavior:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAxXJUQqAIAwA0KvIvhR2owhZtkJYMzYlvH319+DdVrUHMqOZvVnP5CXuU+mqJS46RBA2GQwIk0Xa8+E0ZgX8c00YDhLn9ALNIgvjSQAAAA==" target="_blank">Run the query</a>

```kusto
print array_sort_asc(dynamic([null,"blue","yellow","green",null]), false)
```

**Output**

|`print_0`|
|---|
|[null,null,"blue","green","yellow"]|

## See also

To sort the first array in descending order, use [array_sort_desc()](arraysortdescfunction.md).
