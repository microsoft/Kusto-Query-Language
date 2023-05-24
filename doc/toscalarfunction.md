---
title:  toscalar()
description: Learn how to use the toscalar() function to return a scalar constant value of the evaluated expression.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/16/2023
---
# toscalar()

Returns a scalar constant value of the evaluated expression.

This function is useful for queries that require staged calculations. For example,
calculate a total count of events, and then use the result to filter groups
that exceed a certain percent of all events.

Any two statements must be separated by a semicolon.

## Syntax

`toscalar(`*expression*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *expression* | string | &check; | The value to convert to a scalar value.|

## Returns

A scalar constant value of the evaluated expression.
If the result is a tabular, then the first column and first row will be taken for conversion.

> [!TIP]
> You can use a [let statement](letstatement.md) for readability of the query when using `toscalar()`.

## Limitations

`toscalar()` can't be applied on a scenario that applies the function on each row. This is because the function can only be calculated a constant number of times during the query execution.
Usually, when this limitation is hit, the following error will be returned: `can't use '<column name>' as it is defined outside its row-context scope.`

In the following example, the query fails with the error:

> `'toscalar': can't use 'x' as it is defined outside its row-context scope.` 

```kusto
let _dataset1 = datatable(x:long)[1,2,3,4,5];
let _dataset2 = datatable(x:long, y:long) [ 1, 2, 3, 4, 5, 6];
let tg = (x_: long)
{
    toscalar(_dataset2| where x == x_ | project y);
};
_dataset1
| extend y = tg(x)
```

This failure can be mitigated by using the `join` operator, as in the following example:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVGIT0ksSSxOLTFUsFUAMUsSk3JSNSqsFHLy89I1ow11FIx0FIx1FEx0FExjrblykLQYYdOio1AJ1auAqllHwQyoH24dV41CVn5mnoIG3DRNhfw8hQoFoERBUX5WanKJQgXQMAB+d0ZipAAAAA==" target="_blank">Run the query</a>

```kusto
let _dataset1 = datatable(x: long)[1, 2, 3, 4, 5];
let _dataset2 = datatable(x: long, y: long) [1, 2, 3, 4, 5, 6];
_dataset1
| join (_dataset2) on x 
| project x, y
```

**Output**

|x|y|
|---|---|
|1|2|
|3|4|
|5|6|

## Examples

Evaluate `Start`, `End`, and `Step` as scalar constants, and use the result for `range` evaluation.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA02NQQrCQAxF94J3+EuFbupOJEtP0BMMNS3COFPSCIP08M1kELoL/7/3E1kxaBAFQfM6hhjkssg7KQr118f5FI14ptexl5BmRsEk+YPectyxKi92bxjzN+lfHGp6MG+1aPqv6e25TdQfPuLOBi7KHllPTnWwgIzrHKQK7lv8zY7AAAAA" target="_blank">Run the query</a>

```kusto
let Start = toscalar(print x=1);
let End = toscalar(range x from 1 to 9 step 1 | count);
let Step = toscalar(2);
range z from Start to End step Step | extend start=Start, end=End, step=Step
```

**Output**

|z|start|end|step|
|---|---|---|---|
|1|1|9|2|
|3|1|9|2|
|5|1|9|2|
|7|1|9|2|
|9|1|9|2|

The following example shows how `toscalar` can be used to "fix" an expression
so that it will be calculated precisely once. In this case, the expression being
calculated returns a different value per evaluation.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0XLsQqAIBRG4T3wHf5RoUXX8FlC8nYJTEONDHr4bGo9HydQBWtY1FQWF1yWka6Zz81LpSYxhM9N9z/3ml1kQsOa0w7dXxiUSge0GB5QqxQ9mmU94rZsXqEZQWVlAAAA" target="_blank">Run the query</a>

```kusto
let g1 = toscalar(new_guid());
let g2 = new_guid();
range x from 1 to 2 step 1
| extend x=g1, y=g2
```

**Output**

|x|y|
|---|---|
|e6a15e72-756d-4c93-93d3-fe85c18d19a3|c2937642-0d30-4b98-a157-a6706e217620|
|e6a15e72-756d-4c93-93d3-fe85c18d19a3|c6a48cb3-9f98-4670-bf5b-589d0e0dcaf5|
