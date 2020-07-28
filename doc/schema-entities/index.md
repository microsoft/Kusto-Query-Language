---
title: Entities - Azure Data Explorer | Microsoft Docs
description: This article describes Entities in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 11/19/2019
---
# Entity types

Kusto queries execute in the context of some Kusto database that is attached
to a Kusto cluster. Data in the database is arranged in tables, which the query
may reference, and within the table it is organized as a rectangular grid of
columns and rows. Additionally, queries may reference stored functions in the
database, which are query fragments made available for reuse.

* Clusters are entities that hold databases.
  Clusters have no name, but they can be referenced by using the
  `cluster()` special function with the cluster's URI.
  For example, `cluster("https://help.kusto.windows.net")` is a reference
  to a cluster that holds the `Samples` database.

* [Databases](./databases.md) are named entities that hold tables
  and stored functions. All Kusto queries run in the context of some database,
  and the entities of that database may be referenced by the query with no
  qualifications. Additionally, other databases of the cluster, or databases
  or other clusters, may be referenced using the
  [database() special function](../databasefunction.md). For example,
  `cluster("https://help.kusto.windows.net").database("Samples")`
  is a universal reference to a specific database.

* [Tables](./tables.md) are named entities that hold data. A table has an ordered set
  of columns, and zero or more rows of data, each row holding one data value
  for each of the columns of the table. Tables may be referenced by name only
  if they are in the database in context of the query, or by qualifying them
  with a database reference otherwise. For example,
  `cluster("https://help.kusto.windows.net").database("Samples").StormEvents` is
  a universal reference to a particular table in the `Samples` database.
  Tables may also be referenced by using the [table() special function](../tablefunction.md).

* [Columns](./columns.md) are named entities that have a [scalar data type](../scalar-data-types/index.md).
  Columns are referenced in the query relative to the tabular data stream
  that is in context of the specific operator referencing them.

* [Stored functions](./stored-functions.md) are named entities that
  allow reuse of Kusto queries or query parts.

* [External tables](./externaltables.md) are entities that reference data stored outside Kusto database.
  External tables are used for exporting data from Kusto to external storage as well as for querying
  external data without ingesting it into Kusto.