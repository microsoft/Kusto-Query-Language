---
title: make_set_if() (aggregation function) - Azure Data Explorer
description: Learn how to use the make_set_if() function to create a dynamic JSON object of a set of distinct values that an expression takes where the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/16/2022
---
# make_set_if() (aggregation function)

Creates a `dynamic` JSON object (array) of the set of distinct values that *Expr* takes in records for which *Predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`make_set_if` `(`*Expr*`,` *Predicate* [`,` *MaxSize*]`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | string | &check; | Expression that will be used for aggregation calculation. |
| *Predicate* | string | &check; | Predicate that has to evaluate to `true`, in order for *Expr* to be added to the result. |
| *MaxSize* | integer |  | The limit on the maximum number of elements returned. The default is *1048576* and can't exceed *1048576*. |

## Returns

Returns a `dynamic` JSON object (array) of the set of distinct values that *Expr* takes in records for which *Predicate* evaluates to `true`.
The array's sort order is undefined.

> [!TIP]
> To only count the distinct values, use [dcountif()](dcountif-aggfunction.md)

## See also

[`make_set`](./makeset-aggfunction.md) function, which does the same, without predicate expression.

## Example

The following example shows a list of names with more than 4 letters.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAyXNPQvCMBSF4T2/4tDJQBZFUSt1FZxEuomEFG/bYD4gSQfFH++l5U7PO9zjqKBFg5cpfJ2jVTCe6lySDYPi/NGx151NZaxdDIMUDwGgusYxVApHNetmJsdaHxZeKKaBOGx2S7jzs8jei+dJtOKHPHlvkv0SvHmTzlS07edpBd52FGZInLGVf+88QMOkAAAA)**\]**

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

**Results**

|set_name|
|----|
|["George", "Ringo"]|
