---
title: Cross-database & cross-cluster queries - Azure Data Explorer
description: This article describes cross-database and cross-cluster queries in Azure Data Explorer.
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
# Cross-database and cross-cluster queries

::: zone pivot="azuredataexplorer"

Every Kusto query operates in the context of the current cluster, and the default database.
* In [Kusto Explorer](../tools/kusto-explorer.md), the default database is the one selected in the [Connections panel](../tools/kusto-explorer.md#connections-panel), and the current cluster is the connection containing that database.
* When using [client library](../api/netfx/about-kusto-data.md), the current cluster and the default database are specified by the `Data Source` and `Initial Catalog` properties of the [connection strings](../api/connection-strings/kusto.md), respectively.

## Queries
To access tables from any database other than the default, the *qualified name* syntax must be used.

To access database in the current cluster.

```kusto
database("<database name>").<table name>
```

Database in remote cluster.
```kusto
cluster("<cluster name>").database("<database name>").<table name>
```

*database name* is case-sensitive

*cluster name* is case-insensitive and can be of one of the following forms:
   * Well-formed URL, such as `http://contoso.kusto.windows.net:1234/`. Only HTTP and HTTPS schemes are supported.
   * Fully qualified domain name (FQDN), such as `contoso.kusto.windows.net`. This string is equivalent to `https://`**`contoso.kusto.windows.net`**`:443/`.
   * Short name (host name [and region] without the domain part), such as `contoso` or `contoso.westus`. These strings are interpreted as `https://`**`contoso`**`.kusto.windows.net:443/` and `https://`**`contoso.westus`**`.kusto.windows.net:443/`.

> [!NOTE]
> Cross-database access is subject to the usual permission checks.
> To execute a query, you must have read permission to the default database and
> to every other database referenced in the query (in the current and remote clusters).

*Qualified name* can be used in any context in which a table name can be used.

All of the following are valid.

```kusto
database("OtherDb").Table | where ...

union Table1, cluster("OtherCluster").database("OtherDb").Table2 | project ...

database("OtherDb1").Table1 | join cluster("OtherCluster").database("OtherDb2").Table2 on Key | join Table3 on Key | extend ...
```

> [!IMPORTANT]
> If the clusters are in different tenants, you may need to edit the `trustedExternalTenants` property. Non-trusted external tenants may get an **Unauthorized error (401)** failure. For more information, see [How to allow principals from another tenant to access your cluster](../../cross-tenant-query-and-commands.md).

When *qualified name* appears as an operand of the [union operator](./unionoperator.md), then wildcards can be used to specify multiple tables and multiple databases. Wildcards aren't permitted in cluster names.

```kusto
union withsource=TableName *, database("OtherDb*").*Table, cluster("OtherCluster").database("*").*
```

> [!NOTE]
> * The name of the default database is also a potential match, so database("&#42;")specifies all tables of all databases including the default.
> * For more ionformation on how schema changes affect cross-cluster queries, see [Cross-cluster queries and schema changes](../concepts/crossclusterandschemachanges.md)

## Access restriction

Qualified names or patterns can also be included in [restrict access](./restrictstatement.md) statement,
Wildcards in cluster names aren't permitted.

```kusto
restrict access to (my*, database("MyOther*").*, cluster("OtherCluster").database("my2*").*);
```

The above will restrict the query access to the following entities:

* Any entity name starting with *my...* in the default database. 
* Any table in all the databases named *MyOther...* of the current cluster.
* Any table in all the databases named *my2...* in the cluster *OtherCluster.kusto.windows.net*.

## Functions and Views

Functions and views (persistent and created inline) can reference tables across database and cluster boundaries. The following code is valid.

```kusto
let MyView = Table1 join database("OtherDb").Table2 on Key | join cluster("OtherCluster").database("SomeDb").Table3 on Key;
MyView | where ...
```

Persistent functions and views can be accessed from another database in the same cluster.

Tabular function (view) in `OtherDb`.

```kusto
.create function MyView(v:string) { Table1 | where Column1 has v ...  }  
```

Scalar function in `OtherDb`.

```kusto
.create function MyCalc(a:double, b:double, c:double) { (a + b) / c }  
```

In default database.

```kusto
database("OtherDb").MyView("exception") | extend CalCol=database("OtherDb").MyCalc(Col1, Col2, Col3) | limit 10
```

## Limitations of cross-cluster function calls

Tabular functions or views can be referenced across clusters. The following limitations apply:

* Remote function must return tabular schema. Scalar functions can only be accessed in the same cluster.
* Remote function can accept only scalar parameters. Functions that get one or more table arguments can only be accessed in the same cluster.
* For performance reasons, the schema of remote entities is cached by the calling cluster after the initial call. Therefore, changes made to the remote entity may result in a mismatch with the cached schema information, potentially leading to query failures. For more information, see [Cross-cluster queries and schema changes](../concepts/crossclusterandschemachanges.md).

The following cross-cluster call is valid.

```kusto
cluster("OtherCluster").database("SomeDb").MyView("exception") | count
```

The following query calls a remote scalar function `MyCalc`.
This call violates rule #1, so it's not valid.

```kusto
MyTable | extend CalCol=cluster("OtherCluster").database("OtherDb").MyCalc(Col1, Col2, Col3) | limit 10
```

The following query calls remote function `MyCalc` and provides a tabular parameter.
This call violates rule #2, so it's not valid.

```kusto
cluster("OtherCluster").database("OtherDb").MyCalc(datatable(x:string, y:string)["x","y"] )
```

The following query calls remote function `SomeTable` that has a variable schema output based on the parameter `tablename`.
This call violates rule #3, so it's not valid.

Tabular function in `OtherDb`.

```kusto
.create function SomeTable(tablename:string) { table(tablename)  }  
```

In default database.

```kusto
cluster("OtherCluster").database("OtherDb").SomeTable("MyTable")
```

The following query calls remote function `GetDataPivot` that has a variable schema output based on the data ([pivot() plugin](pivotplugin.md) has dynamic output).
This call violates rule #3, so it's not valid.

Tabular function in `OtherDb`.

```kusto
.create function GetDataPivot() { T | evaluate pivot(PivotColumn) }  
```

Tabular function in the default database.

```kusto
cluster("OtherCluster").database("OtherDb").GetDataPivot()
```

## Displaying data

Statements that return data to the client are implicitly limited by the number of records returned, even if there's no specific use of the `take` operator. To lift this limit, use the `notruncation` client request option.

To display data in graphical form, use the [render operator](renderoperator.md).

::: zone-end

::: zone pivot="azuremonitor"

Cross-database and cross-cluster queries aren't supported in Azure Monitor. See [Cross workspace queries in Azure Monitor](/azure/azure-monitor/log-query/cross-workspace-query) for queries across multiple workspaces and apps.

::: zone-end
