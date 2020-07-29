---
title: materialize() - Azure Data Explorer
description: This article describes materialize() function in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 06/06/2020
---
# materialize()

Allows caching a subquery result during the time of query execution in a way that other subqueries can reference the partial result.
 
## Syntax

`materialize(`*expression*`)`

## Arguments

* *expression*: Tabular expression to be evaluated and cached during query execution.

> [!NOTE]
> Materialize has a cache size limit of **5 GB**. This limit is per cluster node and is mutual for all queries running concurrently. If a query uses `materialize()` and the cache can't hold any more data, the query will abort with an error.

>[!TIP]
>
>* Push all possible operators that reduce the materialized data set and keep the semantics of the query. For example, use filters, or project only required columns.
>* Use materialize with join or union when their operands have mutual subqueries that can be executed once. For example, join/union fork legs. See [example of using join operator](#examples-of-query-performance-improvement).
>* Materialize can only be used in let statements if you give the cached result a name. See [example of using let statements](#examples-of-using-materialize)).

## Examples of query performance improvement

The following example shows how `materialize()` can be used to improve performance of the query.
The expression `_detailed_data` is defined using `materialize()` function and therefore is calculated only once.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let _detailed_data = materialize(StormEvents | summarize Events=count() by State, EventType);
_detailed_data
| summarize TotalStateEvents=sum(Events) by State
| join (_detailed_data) on State
| extend EventPercentage = Events*100.0 / TotalStateEvents
| project State, EventType, EventPercentage, Events
| top 10 by EventPercentage
```

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
* how many distinct values in the set (`Dcount`)
* the top three values in the set 
* the sum of all these values in the set 
 
This operation can be done using [batches](batches.md) and materialize:

<!-- csl: https://help.kusto.windows.net/Samples -->
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

```kusto
    let materializedData = materialize(Table
    | where Timestamp > ago(1d));
    union (materializedData
    | where Text has "String1"
    | summarize dcount(Resource1)), (materializedData
    | where Text has "String2"
    | summarize dcount(Resource2))
 ```

When the combined filter reduces the materialized result drastically, combine both filters on the materialized result by a logical `or` expression as in the query below. However, keep the filters in each union leg to preserve the semantics of the query.
     
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
    
