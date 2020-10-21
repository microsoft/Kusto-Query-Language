---
title: Queries - Azure Data Explorer
description: This article describes Queries in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# Query operators

A query is a read-only operation against a Kusto Engine cluster's ingested data. Queries always run in the context of a particular database in the cluster. They may also refer to data in another database, or even in another cluster.

As ad-hoc query of data is the top-priority scenario for Kusto, the Kusto Query Language syntax is optimized for non-expert users authoring and running queries over their data and being able to understand unambiguously what each query does (logically).

The language syntax is that of a data flow, where "data" means "tabular data" (data in one or more rows/columns rectangular shape). At a minimum, a query consists of source data references (references to Kusto tables) and one or more **query operators** applied in sequence, indicated visually by the use of a pipe character (`|`) to delimit operators.

For example:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents 
| where State == 'FLORIDA' and StartTime > datetime(2000-01-01)
| count
```

Each filter prefixed by the pipe character `|` is an instance of an *operator*, with some parameters. The input to the operator is the table that is the result of the preceding pipeline. In most cases, any parameters are [scalar expressions](./scalar-data-types/index.md) over the columns of the input.
In a few cases, the parameters are the names of input columns, and in a few cases, the parameter is a second table. The result of a query is always a table, even if it only has one column and one row.

`T` is used in query to denote the preceding pipeline or source table.
