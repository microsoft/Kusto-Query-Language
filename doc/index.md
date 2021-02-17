---
title: Overview - Azure Data Explorer
description: This article describes Overview in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 03/07/2019
ms.localizationpriority: high 
adobe-target: true
---
# Overview

A Kusto query is a read-only request to process data and return results.
The request is stated in plain text, using a data-flow model designed to
make the syntax easy to read, author, and automate. The query uses schema
entities that are organized in a hierarchy similar to SQL's: databases, tables,
and columns.

The query consists of a sequence of query statements, delimited by a semicolon
(`;`), with at least one statement being a [tabular expression statement](tabularexpressionstatements.md)
which is a statement that produces data arranged in a table-like mesh of
columns and rows. The query's tabular expression statements produce the results of the
query.

The syntax of the tabular expression statement has tabular data flow from one
tabular query operator to another, starting with data source (e.g. a table
in a database, or an operator that produces data) and then flowing through
a set of data transformation operators that are bound together through the
use of the pipe (`|`) delimiter.

For example, the following Kusto query has a single statement, which is a
tabular expression statement. The statement starts with a reference to a table
called `StormEvents` (the database that host this table is implicit here, and part
of the connection information). The data (rows) for that table are then filtered
by the value of the `StartTime` column, and then filtered by the value of the
`State` column. The query then returns the count of "surviving" rows.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents 
| where StartTime >= datetime(2007-11-01) and StartTime < datetime(2007-12-01)
| where State == "FLORIDA"  
| count 
```

To run this query, [click here](https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVuDlqlEoz0gtSlUILkksKgnJzE1VsLNVSEksSS0BsjWMDAzMdQ0NdQ0MNRUS81KQVNmgKzICKUIxryRVwdZWQcnNxz/I08VRSQFsW3J+aV6JAgAwMx4+hAAAAA==).
In this case, the result will be:

|Count|
|-----|
|   23|
