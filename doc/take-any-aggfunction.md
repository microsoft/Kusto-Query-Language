---
title: take_any() (aggregation function) - Azure Data Explorer
description: Learn how to use the take_any() (aggregation function) to return the value of an arbitrarily selected record.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/20/2023
---
# take_any() (aggregation function)

Arbitrarily chooses one record for each group in a [summarize operator](summarizeoperator.md),
and returns the value of one or more expressions over each such record.

> **Deprecated aliases:** any()

> [!NOTE]
> The deprecated version adds `any_` prefix to the columns returned by the `any()` aggregation.

## Syntax

`take_any(`*expr_1* [`,` *expr_2* ...]`)`

`take_any(`*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr_N* | string | &check; | The expression used for selecting a record. If the wildcard value (`*`) is given in place of an expression, all records will be selected.|

## Returns

The `take_any` aggregation function returns the values of the expressions calculated
for each of the records selected Indeterministically from each group of the summarize operator.

If the `*` argument is provided, the function behaves as if the expressions are all columns
of the input to the summarize operator barring the group-by columns, if any.

## Remarks

This function is useful when you want to get a sample value of one or more columns
per value of the compound group key.

When the function is provided with a single column reference, it will attempt to
return a non-null/non-empty value, if such value is present.

As a result of the indeterministic nature of this function, using this function multiple times in
a single application of the `summarize` operator isn't equivalent to using
this function a single time with multiple expressions. The former may have each application
select a different record, while the latter guarantees that all values are calculated
over a single record (per distinct group).

## Examples

Show indeterministic State:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc6bc487453a064d3c9de.northeurope/databases/NewDatabase1?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSguzc1NLMqsSlUoScxOjU/Mq9QILkksSdUEALgBS0YoAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize take_any(State)
```

**Output**

|State|
|---|
|ATLANTIC SOUTH|

Show all the details for a random record:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc6bc487453a064d3c9de.northeurope/databases/NewDatabase1?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSgoys9KTS5RCC5JLCoJycxN1VFwLcgszk9J9UzRAYmWgERAykMqC1JBOopLc3MTizKrUhVKErNT4xPzKjW0NAGzMGIFVgAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| project StartTime, EpisodeId, State, EventType
| summarize take_any(*)
```

**Output**

|StartTime|EpisodeId|State|EventType|
|---|---|---|---|
|2007-09-29 08:11:00.0000000|11091|ATLANTIC SOUTH|Waterspout|

Show all the details of a random record for each State starting with 'A':

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc6bc487453a064d3c9de.northeurope/databases/NewDatabase1?query=H4sIAAAAAAAAAyWMMQ7CMBAEeyT+cEoFKJ+gSEGd9OggK8Ugx9bdQmTE4xMr7c7O9EwWuy9m+vHwl2WCQXoqIU41+hI4SXNtKs2WXniycuMQIlrpcvA04ja2u7UtNTaUjGr4J0a18INQ37jrXE6XszzKfl4BiZpjAH0AAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| where State startswith "A"
| project StartTime, EpisodeId, State, EventType
| summarize take_any(*) by State
```

**Output**

|State|StartTime|EpisodeId|EventType|
|---|---|---|---|
|ALASKA|2007-02-01 00:00:00.0000000|1733|Flood|
|ATLANTIC SOUTH|2007-09-29 08:11:00.0000000|11091|Waterspout|
|ATLANTIC NORTH|2007-11-27 00:00:00.0000000|11523|Marine Thunderstorm Wind|
|ARIZONA|2007-12-01 10:40:00.0000000|11955|Flash Flood|
|AMERICAN SAMOA|2007-12-07 14:00:00.0000000|13183|Flash Flood|
|ARKANSAS|2007-12-09 16:00:00.0000000|11319|Lightning|
|ALABAMA|2007-12-15 18:00:00.0000000|12580|Heavy Rain|
