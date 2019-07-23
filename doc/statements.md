---
title: Query statements - Azure Data Explorer | Microsoft Docs
description: This article describes Query statements in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2019
---
# Query statements

A query consists of one or more **query statements**, delimited by a semicolon (`;`),
of which at least one is a [tabular expression statement](./tabularexpressionstatements.md),
which is the statement that actually returns results back.

A query may have more than one tabular expression statements. This condition is referred-to
as having a [batch](./batches.md) of tabular expression statements, in which case the results
of each tabular expression statement appears as a single tabular data stream in the query
results.

Query statements fall into one of two groups: statements that are meant primarily to
be used by users, and statements that have been designed to support scenarios in which
applications that are built on top of Kusto want to expose its query language to the
application's users, but provide some "logical model" that is different than the
"physical model" (the schema that actually exists in Kusto). Some statements are useful
in both scenarios.

[!NOTE]
The effect of the statements in a query begins at the appearance of the statement
and ends with the end of the query body. In runtime, when a query terminates its
impact on the system ends (there are no durable changes to the system's data,
other than the trail records that are left indicating that the query executed, etc.)

## User query statements

* A [let statement](./letstatement.md), which defines a binding between a name to an expression.
  Let statements can be used to break a long query into small, named, parts that are easier to
  understand.
* A [set statement](./setstatement.md), which sets a query option that affects how the query
  is processed and its results returned.
* A [tabular expression statement](./tabularexpressionstatements.md), which is the most important
  query statement of all, being the one that returns the "interesting" data back as results.

## Application query statements

* An [alias statement](./aliasstatement.md), which defines an alias to another database
  (in the same Kusto cluster or on a remote cluster).
* A [pattern statement](./patternstatement.md), which can be used by applications that are
  built on top of Kusto and which expose its query language to their users to inject themselves
  into the query name resolution process.
* A [query parameters statement](./queryparametersstatement.md), which is used by applications
  that are built on top of Kusto to protect themselves against injection attacks (similar to
  how command parameters protect SQL against SQL injection attacks.)
* A [restrict statement](./restrictstatement.md), which is used by applications that are built
  on top of Kusto to restrict queries to a specific subset of data in Kusto (including restricting
  access to specific columns and records.)