---
title: cluster() (scope function) - Azure Data Explorer
description: Learn how to use the cluster() function to change the reference of the query to a remote cluster.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/27/2022
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# cluster() (scope function)

::: zone pivot="azuredataexplorer"

Changes the reference of the query to a remote cluster. To access a database within the same cluster, use the [database()](databasefunction.md) function. For more information, see [cross-database and cross-cluster queries](cross-cluster-or-database-queries.md).

## Syntax

`cluster(`*name*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *name* | string | &check; | The name of the cluster to reference. The value can be specified as a fully qualified domain name, or the name of the cluster without the `.kusto.windows.net` suffix. The value can't be the result of subquery evaluation. |

## Examples

### Use cluster() to access remote cluster

The following query can be run on any cluster.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/SampleLogs?query=H4sIAAAAAAAAA0vOKS0uSS3SUM9IzSlQ19RLSSxJTEosTtVQD07MLchJLQaKBZfkF+W6lqXmlRQr1Cgk55fmlQAAayjLjjcAAAA=" target="_blank">Run the query</a>

```kusto
cluster('help').database('Samples').StormEvents | count

cluster('help.kusto.windows.net').database('Samples').StormEvents | count
```

**Output**

|Count|
|---|
|59066|

### Use cluster() inside let statements

The previous query can be rewritten to use a query-defined function (`let` statement) that takes a parameter called `clusterName` and passes it to the `cluster()` function.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/SampleLogs?query=H4sIAAAAAAAAA8tJLVFIy89XsFXQSM4pLS5JLfJLzE21Ki4pysxL1+Sq5lIAAqgMsgpNvZTEksSkxOJUDfXgxNyCnNRidU294JL8olzXstS8kmKFGoXk/NK8Eq5aay6gBRrqGak5BeqaADuaG9BwAAAA" target="_blank">Run the query</a>

```kusto
let foo = (clusterName:string)
{
    cluster(clusterName).database('Samples').StormEvents | count
};
foo('help')
```

**Output**

|Count|
|---|
|59066|

### Use cluster() inside Functions

The same query as above can be rewritten to be used in a function that
receives a parameter `clusterName` - which is passed into the cluster() function.

```kusto
.create function foo(clusterName:string)
{
    cluster(clusterName).database('Samples').StormEvents | count
};
```

> [!NOTE]
> Stored functions using the `cluster()` function can't be used in cross-cluster queries.

::: zone-end

::: zone pivot="azuremonitor"

This capability isn't supported in Azure Monitor

::: zone-end