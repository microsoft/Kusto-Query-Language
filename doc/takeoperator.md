---
title: take operator - Azure Data Explorer | Microsoft Docs
description: This article describes take operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# take operator

Return up to the specified number of rows.

```kusto
T | take 5
```

There is no guarantee which records are returned, unless
the source data is sorted.

**Syntax**

`take` *NumberOfRows*
`limit` *NumberOfRows*

(`take` and `limit` are synonyms.)

**Notes**

`take` is a simple, quick, and efficient way to view a small sample of records
when browsing data interactively, but be aware that it doesn't guarantee any consistency
in its results when executing multiple times, even if the data set hasn't changed.

Even if the number of rows returned by the query isn't explicitly limited
by the query (no `take` operator is used), Kusto limits that number by default.
Please see [Kusto query limits](../concepts/querylimits.md) for details.

See:
[sort operator](sortoperator.md)
[top operator](topoperator.md)
[top-nested operator](topnestedoperator.md)

## Does Kusto support paging of query results?

Kusto doesn't provide a built-in paging mechanism.

Kusto is a complex service that continuously optimizes the data it stores to provide excellent query performance over huge data sets. While paging is a useful mechanism for stateless clients with limited
resources, it shifts the burden to the backend service which
has to track client state information. Subsequently, the performance
and scalability of the backend service is severely limited.

For paging support implement one of the following features:

* Exporting the result of a query to an external storage and paging through the
   generated data.

* Writing a middle-tier application that provides a stateful paging API by caching
   the results of a Kusto query.
