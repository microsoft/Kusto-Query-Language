---
title: table() (scope function) - Azure Data Explorer
description: This article describes table() (scope function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/19/2020
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# table() (scope function)

The table() function references a table by providing its name as an expression
of type `string`.

```kusto
table('StormEvent')
```

## Syntax

`table` `(` *TableName* [`,` *DataScope*] `)`

## Arguments

::: zone pivot="azuredataexplorer"

* *TableName*: An expression of type `string` that provides the name of the table
  being referenced. The value of this expression must be constant at the point
  of call to the function (i.e. it cannot vary by the data context).

* *DataScope*: An optional parameter of type `string` that can be used to restrict
  the table reference to data according to how this data falls under the table's
  effective [cache policy](../management/cachepolicy.md). If used, the actual argument
  must be a constant `string` expression having one of the following possible values:

    - `"hotcache"`: Only data that is categorized as hot cache will be referenced.
    - `"all"`: All the data in the table will be referenced.
    - `"default"`: This is the same as `"all"`, except if the cluster has been
      set to use `"hotcache"` as the default by the cluster admin.

::: zone-end

::: zone pivot="azuremonitor"

* *TableName*: An expression of type `string` that provides the name of the table
  being referenced. The value of this expression must be constant at the point
  of call to the function (i.e. it cannot vary by the data context).

* *DataScope*: An optional parameter of type `string` that can be used to restrict
  the table reference to data according to how this data falls under the table's
  effective cache policy. If used, the actual argument
  must be a constant `string` expression having one of the following possible values:

    - `"hotcache"`: Only data that is categorized as hot cache will be referenced.
    - `"all"`: All the data in the table will be referenced.
    - `"default"`: This is the same as `"all"`.

::: zone-end

## Examples

### Use table() to access table of the current database

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
table('StormEvent') | count
```

|Count|
|---|
|59066|

### Use table() inside let statements

The same query as above can be rewritten to use inline function (let statement) that 
receives a parameter `tableName` - which is passed into the table() function.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let foo = (tableName:string)
{
    table(tableName) | count
};
foo('help')
```

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

::: zone pivot="azuredataexplorer"

**Note:** such functions can be used only locally and not in the cross-cluster query.

::: zone-end

### Use table() with non-constant parameter

A parameter, which is not scalar constant string can't be passed as parameter to `table()` function.

Below, given an example of workaround for such case.

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

|x|
|---|
|2|
