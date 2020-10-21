---
title: database() (scope function) - Azure Data Explorer | Microsoft Docs
description: This article describes database() (scope function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# database() (scope function)

::: zone pivot="azuredataexplorer"

Changes the reference of the query to a specific database within the cluster scope. 

```kusto
database('Sample').StormEvents
cluster('help').database('Sample').StormEvents
```

> [!NOTE]
> * For more information, see [cross-database and cross-cluster queries](cross-cluster-or-database-queries.md).
> * For accessing remote cluster and remote database, see [cluster()](clusterfunction.md) scope function.

## Syntax

`database(`*stringConstant*`)`

## Arguments

* *stringConstant*: Name of the database that is referenced. Database identified can be either `DatabaseName` or `PrettyName`. Argument has to be _constant_ prior of query execution, i.e. cannot come from sub-query evaluation.

## Examples

### Use database() to access table of other database

```kusto
database('Samples').StormEvents | count
```

|Count|
|---|
|59066|

### Use database() inside let statements 

The same query as above can be rewritten to use inline function (let statement) that 
receives a parameter `dbName` - which is passed into the database() function.

```kusto
let foo = (dbName:string)
{
    database(dbName).StormEvents | count
};
foo('help')
```

|Count|
|---|
|59066|

### Use database() inside functions 

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
