---
title: database() (scope function) - Azure Data Explorer
description: Learn how to use the database() function to change the reference of the query to a specific database within the cluster scope.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/09/2023
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# database() (scope function)

::: zone pivot="azuredataexplorer"

Changes the reference of the query to a specific database within the cluster scope.

> [!NOTE]
>
> * For more information, see [cross-database and cross-cluster queries](cross-cluster-or-database-queries.md).
> * For accessing remote cluster and remote database, see [`cluster()`](clusterfunction.md) scope function.

## Syntax

`database(`*databaseName*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
| *databaseName* | string | The name of the database to reference. The *databaseName* can be either the `DatabaseName` or `PrettyName`. The argument must be a constant value and can't come from a subquery evaluation.|

## Examples

### Use database() to access table of other database

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLElMSixO1VAPTswtyEktVtfUCy7JL8p1LUvNKylWqFFIzi/NKwEAS+mhvycAAAA=" target="_blank">Run the query</a>

```kusto
database('Samples').StormEvents | count
```

**Output**

|Count|
|---|
|59066|

### Use database() inside let statements

The query above can be rewritten as a query-defined function (let statement) that
receives a parameter `dbName` - which is passed into the database() function.

```kusto
let foo = (dbName:string)
{
    database(dbName).StormEvents | count
};
foo('help')
```

**Output**

|Count|
|---|
|59066|

### Use database() inside stored functions

The same query as above can be rewritten to be used in a function that
receives a parameter `dbName` - which is passed into the database() function.

```kusto
.create function foo(dbName:string)
{
    database(dbName).StormEvents | count
};
```

> [!NOTE]
> Such functions can be used only locally and not in the cross-cluster query.

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end
