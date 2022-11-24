---
title: make_list_if() (aggregation function) - Azure Data Explorer
description: Learn how to use the make_list_if() aggregation function to create a dynamic JSON object of expression values where the predicate evaluates to true.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/16/2022
---
# make_list_if() (aggregation function)

Creates a `dynamic` JSON object (array) of *Expr* values in the group for which *Predicate* evaluates to `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`make_list_if` `(`*Expr*`,` *Predicate* [`,` *MaxSize*]`)`

## Arguments

| Name | Type | Required | Description |
|--|--|--|--|
| *Expr* | string | &check; | Expression that will be used for aggregation calculation. |
| *Predicate* | string | &check; | Predicate that has to evaluate to `true`, in order for *Expr* to be added to the result. |
| *MaxSize* | integer |  | The limit on the maximum number of elements returned. The default is *1048576* and can't exceed *1048576*. |

## Returns

Returns a `dynamic` JSON object (array) of *Expr* vlaues in the group for which *Predicate* evaluates to `true`.
If the input to the `summarize` operator is not sorted, the order of elements in the resulting array is undefined.
If the input to the `summarize` operator is sorted, the order of elements in the resulting array tracks that of the input.

## Example

The following example shows a list of names with more than 4 letters.

**\[**[**Click to run query**](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAyXNMQvCMBCG4T2/4uhkIIuiqJW6Ck4i3URCitc0eEkgSQfFH++RctPzDvcRFuihg5cpfAPhKhiPbS7JBas4f3Qc9eBSmVqKwUrxEADQXOMUGgVHVXUzM7HWh4UXjMkih81uCXd+Ftl78TyJXvwgz96b5L4I3rxRk8tFu7FuK+BxwlAh4Qxb+Qd6P2/WpQAAAA==)**\]**

```kusto
let T = datatable(name:string, day_of_birth:long)
[
   "John", 9,
   "Paul", 18,
   "George", 25,
   "Ringo", 7
];
T
| summarize make_list_if(name, strlen(name) > 4)
```

**Results**

|list_name|
|----|
|["George", "Ringo"]|

## See also

[`make_list`](./makelist-aggfunction.md) function, which does the same, without predicate expression.
