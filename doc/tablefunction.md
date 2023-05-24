---
title:  table() (scope function)
description: Learn how to use the table() (scope function) function to reference a table.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/16/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors-all
---
# table() (scope function)

The table() function references a table by providing its name as an expression of type `string`.

## Syntax

`table(` *TableName* [`,` *DataScope*] `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*TableName* | string | &check; | The name of the table being referenced. The value of this expression must be constant at the point of call to the function, meaning it cannot vary by the data context.|
| *DataScope* | string | | Used to restrict the table reference to data according to how this data falls under the table's effective [cache policy](../management/cachepolicy.md). If used, the actual argument must be one of the [Valid data scope values](#valid-data-scope-values).

### Valid data scope values

|Value|Description|
|--|--|
| `hotcache`| Only data that is categorized as hot cache will be referenced.|
| `all`| All the data in the table will be referenced.|
| `default`| The default is `all`, except if it has been set to `hotcache` by the cluster admin.|

## Returns

`table(T)` returns:

* Data from table *T* if a table named *T* exists.
* Data returned by function *T* if a table named *T* doesn't exist but a function named *T* exists. Function *T* must take no arguments and must return a tabular result.
* A semantic error is raised if there's no table named *T* and no function named *T*.

## Examples

### Use table() to access table of the current database

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytJTMpJ1VAPLskvynUtS80rKVbXVKhRSM4vzSsBAIdoofIcAAAA" target="_blank">Run the query</a>

```kusto
table('StormEvents') | count
```

**Output**

|Count|
|---|
|59066|

### Use table() inside let statements

The query above can be rewritten as a query-defined function (let statement) that receives a parameter `tableName` - which is passed into the table() function.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFIy89XsFXQKElMykn1S8xNtSouKcrMS9fkquZSAAKwOEJWU6FGITm/NK+Eq9aaC6hVQz24JL8o17UsNa+kWF0TAD3GJXVRAAAA" target="_blank">Run the query</a>

```kusto
let foo = (tableName:string)
{
    table(tableName) | count
};
foo('StormEvents')
```

**Output**

|Count|
|---|
|59066|

### Use table() inside Functions

The same query as above can be rewritten to be used in a function that 
receives a parameter `tableName` - which is passed into the table() function.

```kusto
.create function foo(tableName:string)
{
    table(tableName) | count
};
```

::: zone pivot="azuredataexplorer, fabric"

> [!NOTE]
> Such functions can be used only locally and not in the cross-cluster query.

::: zone-end

::: zone pivot="azuremonitor"

::: zone-end

### Use table() with non-constant parameter

A parameter, which isn't a scalar constant string, can't be passed as a parameter to the `table()` function.

Below, given an example of workaround for such case.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEIMVSwVSgoyswrUaiwNbTmygGJGSGJGUHE4pMz8vOLU4ESGvHFqTmpySX5RVbFJUBF6Zpc1VwKQFCal5mfB2ZpAE2tUSjPSC1KVYCrVrC1VVAPMVTX1IGqMcKlxkhdk6vWmgtqpQZYAADWO8bZrAAAAA==" target="_blank">Run the query</a>

```kusto
let T1 = print x=1;
let T2 = print x=2;
let _choose = (_selector:string)
{
    union
    (T1 | where _selector == 'T1'),
    (T2 | where _selector == 'T2')
};
_choose('T2')

```

**Output**

|x|
|---|
|2|
