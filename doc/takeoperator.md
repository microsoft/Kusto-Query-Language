---
title:  take operator
description: Learn how to use the take operator to return a specified number of rows.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/16/2023
---
# take operator

Return up to the specified number of rows.

There is no guarantee which records are returned, unless
the source data is sorted. If the data is sorted, then the top values will be returned.

> The `take` and `limit` operators are equivalent

> [!NOTE]
> `take` is a simple, quick, and efficient way to view a small sample of records when browsing data interactively, but be aware that it doesn't guarantee any consistency in its results when executing multiple times, even if the data set hasn't changed.
> Even if the number of rows returned by the query isn't explicitly limited by the query (no `take` operator is used), Kusto limits that number by default. For more details, see [Kusto query limits](../concepts/querylimits.md).

## Syntax

`take` *NumberOfRows*

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*NumberOfRows*|int|&check;|The number of rows to return.|

## Paging of query results

Methods for implementing paging include:

* Export the result of a query to an external storage and paging through the
   generated data.
* Write a middle-tier application that provides a stateful paging API by caching
   the results of a Kusto query.
* Use pagination in [Stored query results](../management/stored-query-results.md#pagination).

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVqhRKEnMTlUwBQDEz2b8FAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents | take 5
```

## See also

* [sort operator](sort-operator.md)
* [top operator](topoperator.md)
* [top-nested operator](topnestedoperator.md)
