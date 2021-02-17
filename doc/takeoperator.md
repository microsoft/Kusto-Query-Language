---
title: take operator - Azure Data Explorer | Microsoft Docs
description: This article describes take operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
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

> [!NOTE]
> `take` is a simple, quick, and efficient way to view a small sample of records when browsing data interactively, but be aware that it doesn't guarantee any consistency in its results when executing multiple times, even if the data set hasn't changed.
> Even if the number of rows returned by the query isn't explicitly limited by the query (no `take` operator is used), Kusto limits that number by default. For more details, see [Kusto query limits](../concepts/querylimits.md).

## Syntax

`take` *NumberOfRows*
`limit` *NumberOfRows*

(`take` and `limit` are synonyms.)

## Paging of query results

Methods for implementing paging include:

* Export the result of a query to an external storage and paging through the
   generated data.
* Write a middle-tier application that provides a stateful paging API by caching
   the results of a Kusto query.
* Use pagination in [Stored query results](../management/stored-query-results.md#pagination) .


## See also

* [sort operator](sortoperator.md)
* [top operator](topoperator.md)
* [top-nested operator](topnestedoperator.md)
