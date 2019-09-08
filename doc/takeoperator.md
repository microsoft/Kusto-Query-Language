# take operator

Return up to the specified number of rows.

<!-- csl -->
```
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

Even is the number of rows returned by the query is not explicitly limited
by the query (no `take` operator is used), Kusto limits that number by default.
Please see [Kusto query limits](../concepts/querylimits.md) for details.

See:
[sort operator](sortoperator.md)
[top operator](topoperator.md)
[top-nested operator](topnestedoperator.md)

## Does Kusto support paging of query results?

Kusto does not provide a built-in paging mechanism.

Kusto is aa complex service that continuously optimizes the data it stores
in the background in order to provide excellent query performance over huge data
sets. While paging is a useful mechanism for stateless clients with limited
resources, it does so by shifting the burden to the backend service which now
has to track client state information; this, in turn, severly limits the performance
and scalability of the backend service.

Customers that require paging support can implement one by using other Kusto
features, such as:

1. Exporting the result of a query to an external storage and paging through the
   generated data.

2. Writing a middle-tier application that provide a statful paging API by caching
   the results of a Kusto query.
