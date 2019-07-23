---
title: take operator - Azure Data Explorer | Microsoft Docs
description: This article describes take operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
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

**Remarks**

`take` is a simple, quick, and efficient way to view a small sample of records
when browsing data interactively, but be aware that it doesn't guarantee any consistency
in its results when executing multiple times, even if the data set hasn't changed.

Even is the number of rows returned by the query is not explicitly limited
by the query (no `take` operator is used), Kusto limits that number by default.
Please see [Kusto query limits](../concepts/querylimits.md) for details.

See:
[sort operator](sortoperator.md)
[top operator](topoperator.md)
[top-nested operator](topnestedoperator.md)

## A note on paging through a large resultset (or: the lack of a `skip` operator)

Kusto does not support the complementary `skip` operator. This is intentional, as
`take` and `skip` together are mainly used for thin client paging, and have a major
performance impact on the service. Application builders that want to support result
paging are advised to query for several pages of data (say, 10,000 records at a time)
and then display a page of data at a time to the user.