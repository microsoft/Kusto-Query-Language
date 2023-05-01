---
title: mv-apply operator - Azure Data Explorer
description: Learn how to use the mv-apply operator to apply a subquery to each record and union the results of each subquery.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# mv-apply operator

Applies a subquery to each record, and returns the union of the results of
all subqueries.

For example, assume a table `T` has a column `Metric` of type `dynamic`
whose values are arrays of `real` numbers. The following query will locate the
two biggest values in each `Metric` value, and return the records corresponding
to these values.

```kusto
T | mv-apply Metric to typeof(real) on 
(
   top 2 by Metric desc
)
```

The `mv-apply` operator has the following
processing steps:

1. Uses the [`mv-expand`](./mvexpandoperator.md) operator to expand each record in the input into subtables (order is preserved).
1. Applies the subquery for each of the subtables.
1. Adds zero or more columns to the resulting subtable. These columns contain the values of the source columns that aren't expanded, and are repeated where needed.
1. Returns the union of the results.

The `mv-apply` operator gets the following inputs:

1. One or more expressions that evaluate into dynamic arrays to expand.
   The number of records in each expanded subtable is the maximum length of
   each of those dynamic arrays. Null values are added where multiple expressions are specified and the corresponding arrays have different lengths.

1. Optionally, the names to assign the values of the expressions after expansion.
   These names become the columns names in the subtables.
   If not specified, the original name of the column is used when the expression is a column reference. A random name is used otherwise.

   > [!NOTE]
   > It is recommended to use the default column names.

1. The data types of the elements of those dynamic arrays, after expansion.
   These become the column types of the columns in the subtables.
   If not specified, `dynamic` is used.

1. Optionally, the name of a column to add to the subtables that specifies the
   0-based index of the element in the array that resulted in the subtable record.

1. Optionally, the maximum number of array elements to expand.

The `mv-apply` operator can be thought of as a generalization of the
[`mv-expand`](./mvexpandoperator.md) operator (in fact, the latter can be implemented
by the former, if the subquery includes only projections.)

## Syntax

*T* `|` `mv-apply` [*ItemIndex*] *ColumnsToExpand* [*RowLimit*] `on` `(` *SubQuery* `)`

Where *ItemIndex* has the syntax:

`with_itemindex` `=` *IndexColumnName*

*ColumnsToExpand* is a comma-separated list of one or more elements of the form:

[*Name* `=`] *ArrayExpression* [`to` `typeof` `(`*Typename*`)`]

*RowLimit* is simply:

`limit` *RowLimit*

and *SubQuery* has the same syntax of any query statement.

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*ItemIndex*|string||Indicates the name of a column of type `long` that's appended to the input as part of the array-expansion phase and indicates the 0-based array index of the expanded value.|
|*Name*|string||The name to assign the array-expanded values of each array-expanded expression. If not specified, the name of the column will be used if available. A random name is generated if *ArrayExpression* isn't a simple column name.|
|*ArrayExpression*|dynamic|&check;|The array whose values will be array-expanded. If the expression is the name of a column in the input, the input column is removed from the input and a new column of the same name, or *ColumnName* if specified, appears in the output.|
|*Typename*|string||The name of the type that the individual elements of the `dynamic` array *ArrayExpression* take. Elements that don't conform to this type will be replaced by a null value. If unspecified, `dynamic` is used by default.|
|*RowLimit*|int||A limit on the number of records to generate from each record of the input. If unspecified, 2147483647 is used.|
|*SubQuery*|string||A tabular query expression with an implicit tabular source that gets applied to each array-expanded subtable.|

>[!NOTE]
> Unlike the [`mv-expand`](./mvexpandoperator.md) operator, the `mv-apply` operator doesn't support `bagexpand=array` expansion. If the expression to be expanded is a property bag and not an array, you can use an inner `mv-expand` operator (see example below).

## Examples

### Getting the largest element from the array

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAy2NsQrDMBBD93yFlkIydEimQvEn9BuCSy4h9M5nkmuxSz6+jqkGDRJ6YjKMkzcP16Bo82EhJMybCnqY4obdKKKv9YH9LeK39UtgJ/5FI6+7tanDMyM9dBrgyvyC4d5UbnNAPlcfI2cQk1AwxyfXciSdW9awdNCAym+rm5a/E/gf1LD7ASAFbUCrAAAA" target="_blank">Run the query</a>

```kusto
let _data =
    range x from 1 to 8 step 1
    | summarize l=make_list(x) by xMod2 = x % 2;
_data
| mv-apply element=l to typeof(long) on 
    (
    top 1 by element
    )
```

**Output**

|`xMod2`|l           |element|
|-----|------------|-------|
|1    |[1, 3, 5, 7]|7      |
|0    |[2, 4, 6, 8]|8      |

### Calculating the sum of the largest two elements in an array

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2WNsQ7CMBBD936FF6RkYGgmJJRPQAywV0FNq4pLLmqvKEH9eNqsePBw9j2TF3S9EwfbYNfs4uiRMcwc0EIYFyziE9oab1jWENw8fT3IBvf2HU2LqKzxKsg37g3s/n6CuTaV22wIn7NLiQro4ElJngdFHEcNjhWrqgsnmINDf1uPNdyHJydj95siXQv6B6sP8Fq9AAAA" target="_blank">Run the query</a>

```kusto
let _data =
    range x from 1 to 8 step 1
    | summarize l=make_list(x) by xMod2 = x % 2;
_data
| mv-apply l to typeof(long) on
    (
    top 2 by l
    | summarize SumOfTop2=sum(l)
    )
```

**Output**

|`xMod2`|l        |SumOfTop2|
|-----|---------|---------|
|1    |[1,3,5,7]|12       |
|0    |[2,4,6,8]|14       |

### Using `with_itemindex` for working with a subset of the array

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAzVOS27CMBDd5xRvU5FIRRC6rMwNOENkkoG4tT2WMyEJ4vCEQcziLeZ9PQmazoqFKbBetvFKmHHJHFBDGPUeg1BCrfwDwxiCze5O8CbYf2q8G6ScK5wXzCfuDjCr/wuH30KDiwfCbWtT8gsmJ33jhIKLHc1GEeQpUBTjX22yJOJL6TleK3CElpaKux16yoSFR/T2Rtiof4OW/RiiaiZVvHOPBj/6rNYJKfMftfKmvj+dT34iokT+AAAA" target="_blank">Run the query</a>

```kusto
let _data =
    range x from 1 to 10 step 1
    | summarize l=make_list(x) by xMod2 = x % 2;
_data
| mv-apply with_itemindex=index element=l to typeof(long) on 
    (
    // here you have 'index' column
    where index >= 3
    )
| project index, element
```

**Output**

|index|element|
|---|---|
|3|7|
|4|9|
|3|8|
|4|10|

### Using mutiple columns to join element of 2 arrays

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA12OPQuDMBCG9/yKo0sSsIMpXYQOfuzduohIqhlCEysaSy3++KaXtog5CLwf93CtdH6uRgG7SJOA7lwE6TDECbRzJ61uUIq/5KQk4F8c/RxW0jSmEdBU4H+gFV+HGYYZhlkIkXBcl3Is5WKzW6BdfGxSAVnAPvay782MN4bT4N4BQ6J6OtW1cJ4cnGB0QyMdC71dvQtljsUFxslaOeiXQpCvW3lTtdFj2OBf9MYX3vf0tetlYPI3W/swT0sBAAA=" target="_blank">Run the query</a>

```kusto
datatable (Val: int, Arr1: dynamic, Arr2: dynamic)
[
    1, dynamic(['A1', 'A2', 'A3']), dynamic(['B1', 'B2', 'B3']), 
    5, dynamic(['C1', 'C2']), dynamic(['D1', 'D2'])
] 
| mv-apply Arr1, Arr2 on (
    extend Out = strcat(Arr1, "_", Arr2)
    | summarize Arr1 = make_list(Arr1), Arr2 = make_list(Arr2), Out= make_list(Out)
    )
```

**Output**

|Val|Arr1|Arr2|`Out`|
|---|---|---|---|
|1|["A1","A2","A3"]|["B1","B2","B3"]|["A1_B1","A2_B2","A3_B3"]|
|5|["C1","C2"]|["D1","D2"]|["C1_D1","C2_D2"]|

### Applying mv-apply to a property bag

In the following example, `mv-apply` is used in combination with an
inner `mv-expand` to remove values that don't start with "555" from a property bag:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA21PwW6DMAy98xVeL4BEpdG1O1Tqqfdp2nqrEDLUooyQICdAmfrxczMOa7VEtuSX955fTujkFoqiT9NzSW99WxBvwTqudZXAAbki94juz8h2b3rttqCMruLgGICccLPZLG+Vrl7WYfJ3Tlcyr1+TR57HQ6lau0j3SsVBFlyBLo70CQZUPVnYQYdlk6NSUSyP7bDErlMTMLVmoHc2HbGrPXFWGA1+U+S7COjSoRg2tbQdIDNOM9czrjCeiWmGjmkGT9YhOzvW7gwLybqYebZvW+T6m6CTvfZgPnwIMW2xodySi2aT5yz2mvif/xRY5b/p84YmO0uSe8+bUIAvKt0SRwl89/oDn62shLsBAAA=" target="_blank">Run the query</a>

```kusto
datatable(SourceNumber: string, TargetNumber: string, CharsCount: long)
[
    '555-555-1234', '555-555-1212', 46,
    '555-555-1212', '', int(null)
]
| extend values = pack_all()
| mv-apply removeProperties = values on 
    (
    mv-expand kind = array values
    | where values[1] !startswith "555"
    | summarize propsToRemove = make_set(values[0])
    )
| extend values = bag_remove_keys(values, propsToRemove)
| project-away propsToRemove
```

**Output**

|SourceNumber|TargetNumber|CharsCount|values
|---|---|---|---|
|555-555-1234|555-555-1212|46|{<br> "SourceNumber": "555-555-1234",<br>   "TargetNumber": "555-555-1212"<br> }|
|555-555-1212|&nbsp;|&nbsp;|{<br> "SourceNumber": "555-555-1212"<br> }|

## See also

* [mv-expand](./mvexpandoperator.md) operator
