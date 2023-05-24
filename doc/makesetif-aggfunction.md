---
title:  make_set_if() (aggregation function)
description: Learn how to use the make_set_if() function to create a dynamic JSON object of a set of distinct values that an expression takes where the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/05/2023
---
# make_set_if() (aggregation function)

Creates a `dynamic` array of the set of distinct values that *expr* takes in records for which *predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`make_set_if(`*expr*`,` *predicate* [`,` *maxSize*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr* | string | &check; | The expression used for the aggregation calculation. |
| *predicate* | string | &check; | A predicate that has to evaluate to `true` in order for *expr* to be added to the result. |
| *maxSize* | int |  | The maximum number of elements returned. The default and max value is 1048576. |

## Returns

Returns a `dynamic` array of the set of distinct values that *expr* takes in records for which *predicate* evaluates to `true`. The array's sort order is undefined.

> [!TIP]
> To only count the distinct values, use [dcountif()](dcountif-aggfunction.md).

## See also

[`make_set`](./makeset-aggfunction.md) function, which does the same, without predicate expression.

## Example

The following example shows a list of names with more than 4 letters.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAyXNPQvCMBSF4T2/4tDJQBZFUSt1FZxEuomEFG/bYD4gSQfFH++l5U7PO9zjqKBFg5cpfJ2jVTCe6lySDYPi/NGx151NZaxdDIMUDwGgusYxVApHNetmJsdaHxZeKKaBOGx2S7jzs8jei+dJtOKHPHlvkv0SvHmTzlS07edpBd52FGZInLGVf+88QMOkAAAA" target="_blank">Run the query</a>

```kusto
let T = datatable(name:string, day_of_birth:long)
[
   "John", 9,
   "Paul", 18,
   "George", 25,
   "Ringo", 7
];
T
| summarize make_set_if(name, strlen(name) > 4)
```

**Output**

|set_name|
|----|
|["George", "Ringo"]|
