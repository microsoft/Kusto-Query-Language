---
title: materialize() - Azure Data Explorer
description: Learn how to use the materialize() function to capture the value of a tabular expression for reuse.
ms.reviewer: zivc
ms.topic: reference
ms.date: 01/05/2023
---
# materialize()

Captures the value of a tabular expression for the duration of the query execution so that it can be referenced multiple times by the query without recalculation.

## Syntax

`materialize(`*expression*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expression* | string | &check; | The tabular expression to be evaluated and cached during query execution.|

## Remarks

The `materialize()` function is useful in the following scenarios:

* To speed up queries that perform *heavy* calculations whose results are used multiple times in the query.
* To evaluate a tabular expression only once and use it many times in a query. This is commonly required if the tabular expression is non-deterministic. For example, if the expression uses the `rand()` or the `dcount()` functions.

> [!NOTE]
> Materialize has a cache size limit of **5 GB**. This limit is per cluster node and is mutual for all queries running concurrently. If a query uses `materialize()` and the cache can't hold any more data, the query will abort with an error.

>[!TIP]
> Another way to perform materialization of tabular expression is by using the `hint.materialized` flag
> of the [as operator](asoperator.md) and [partition operator](partitionoperator.md). They all share a
> single materialization cache.

>[!TIP]
>
>* Push all possible operators that reduce the materialized data set and keep the semantics of the query. For example, use common filters on top of the same materialized expression.
>* Use materialize with join or union when their operands have mutual subqueries that can be executed once. For example, join/union fork legs. See [example of using join operator](#examples-of-query-performance-improvement).
>* Materialize can only be used in let statements if you give the cached result a name. See [example of using let statements](#examples-of-using-materialize)).

## Examples of query performance improvement

The following example shows how `materialize()` can be used to improve performance of the query.
The expression `_detailed_data` is defined using `materialize()` function and therefore is calculated only once.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2WPwQqDMAyG74LvkGMdstXz8Lj7QO/S2TAqtZUaxxx7+FUrOPXU8iffl0QjQSWRhNIoKylIQA6tIHRKaPVBVpB17e2Fhnr4Qj+0rXA+hxDltR0MsQQeIxTksTQUyrHD5BpHW3Uc/RtKS0LP0OLyJRa+q29CGqsMsK0qAWvWDnwTGhlG39HV/hFP9JcE3Snj/Mzhchg5sZ2zDdZ0WD/d65Zghsh2kPFpy13TDxDFARZQAQAA" target="_blank">Run the query</a>

```kusto
let _detailed_data = materialize(StormEvents | summarize Events=count() by State, EventType);
_detailed_data
| summarize TotalStateEvents=sum(Events) by State
| join (_detailed_data) on State
| extend EventPercentage = Events*100.0 / TotalStateEvents
| project State, EventType, EventPercentage, Events
| top 10 by EventPercentage
```

**Output**

|State|EventType|EventPercentage|Events|
|---|---|---|---|
|HAWAII WATERS|Waterspout|100|2|
|LAKE ONTARIO|Marine Thunderstorm Wind|100|8|
|GULF OF ALASKA|Waterspout|100|4|
|ATLANTIC NORTH|Marine Thunderstorm Wind|95.2127659574468|179|
|LAKE ERIE|Marine Thunderstorm Wind|92.5925925925926|25|
|E PACIFIC|Waterspout|90|9|
|LAKE MICHIGAN|Marine Thunderstorm Wind|85.1648351648352|155|
|LAKE HURON|Marine Thunderstorm Wind|79.3650793650794|50|
|GULF OF MEXICO|Marine Thunderstorm Wind|71.7504332755633|414|
|HAWAII|High Surf|70.0218818380744|320|

The following example generates a set of random numbers and calculates:

* How many distinct values in the set (`Dcount`)
* The top three values in the set
* The sum of all these values in the set

This operation can be done using [batches](batches.md) and materialize:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3WNQQqDMBBF94J3+Mu4M7gs2fUGniDVabEkToiT0hYP32gKQqF/k5nw5n1HgmjnkX2fJ4O6Qo63QnGybnqTKj9bMncjPHGN7KEhjK7dg0UoQB/kihD5ToPgYV2i7N06lC542zSnujpaVyzJextzG84Dp1nMuD9qv/6FhQM6XF7F/dfUJ2/y9nV8ADNWwIPoAAAA" target="_blank">Run the query</a>

```kusto
let randomSet = 
    materialize(
        range x from 1 to 3000000 step 1
        | project value = rand(10000000));
randomSet | summarize Dcount=dcount(value);
randomSet | top 3 by value;
randomSet | summarize Sum=sum(value)
```

Result set 1:  

|Dcount|
|---|
|2578351|

Result set 2:

|value|
|---|
|9999998|
|9999998|
|9999997|

Result set 3:

|Sum|
|---|
|15002960543563|

## Examples of using materialize()

> [!TIP]
> Materialize your column at ingestion time if most of your queries extract fields from dynamic objects across millions of rows.

To use the `let` statement with a value that you use more than once, use the [materialize() function](./materializefunction.md). Try to push all possible operators that will reduce the materialized data set and still keep the semantics of the query. For example, use filters, or project only required columns.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
    let materializedData = materialize(Table
    | where Timestamp > ago(1d));
    union (materializedData
    | where Text !has "somestring"
    | summarize dcount(Resource1)), (materializedData
    | where Text !has "somestring"
    | summarize dcount(Resource2))
```

The filter on `Text` is mutual and can be pushed to the materialize expression.
The query only needs columns `Timestamp`, `Text`, `Resource1`, and `Resource2`. Project these columns inside the materialized expression.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
    let materializedData = materialize(Table
    | where Timestamp > ago(1d)
    | where Text !has "somestring"
    | project Timestamp, Resource1, Resource2, Text);
    union (materializedData
    | summarize dcount(Resource1)), (materializedData
    | summarize dcount(Resource2))
```

If the filters aren't identical, as in the following query:  

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
    let materializedData = materialize(Table
    | where Timestamp > ago(1d));
    union (materializedData
    | where Text has "String1"
    | summarize dcount(Resource1)), (materializedData
    | where Text has "String2"
    | summarize dcount(Resource2))
 ```

When the combined filter reduces the materialized result drastically, combine both filters on the materialized result by a logical `or` expression as in the following query. However, keep the filters in each union leg to preserve the semantics of the query.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
    let materializedData = materialize(Table
    | where Timestamp > ago(1d)
    | where Text has "String1" or Text has "String2"
    | project Timestamp, Resource1, Resource2, Text);
    union (materializedData
    | where Text has "String1"
    | summarize dcount(Resource1)), (materializedData
    | where Text has "String2"
    | summarize dcount(Resource2))
```
