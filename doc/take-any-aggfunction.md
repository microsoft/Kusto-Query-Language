---
title: take_any() (aggregation function) - Azure Data Explorer
description: This article describes take_any() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 07/14/2021
---
# take_any() (aggregation function)

Arbitrarily chooses one record for each group in a [summarize operator](summarizeoperator.md),
and returns the value of one or more expressions over each such record.

> [!NOTE]
> `any()` is a legacy and obsolete version of the `take_any()` function. The legacy version adds `any_` prefix to the columns returned by the `any()` aggregation.

## Syntax

`take_any` `(` (*Expr* [`,` *Expr2* ...]) | *\** `)`

## Arguments

* *Expr*: An expression over each record selected from the input to return.
* *Expr2* .. *ExprN*: Additional expressions.

## Returns

The `take_any` aggregation function returns the values of the expressions calculated
for each of the records, selected randomly from each group of the summarize operator.

If the `*` argument is provided, the function behaves as if the expressions are all columns
of the input to the summarize operator barring the group-by columns, if any.

## Remarks

This function is useful when you want to get a sample value of one or more columns
per value of the compound group key.

When the function is provided with a single column reference, it will attempt to
return a non-null/non-empty value, if such value is present.

As a result of the random nature of this function, using it multiple times in
a single application of the `summarize` operator is not equivalent to using
it a single time with multiple expressions. The former may have each application
select a different record, while the latter guarantees that all values are calculated
over a single record (per distinct group).

## Examples

Show Random State:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| summarize take_any(State)
```

|State|
|---|
|ATLANTIC SOUTH|

Show all the details for a random record:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| project StartTime, EpisodeId, State, EventType
| summarize take_any(*)
```

|StartTime|EpisodeId|State|EventType|
|---|---|---|---|
|2007-09-29 08:11:00.0000000|11091|ATLANTIC SOUTH|Waterspout|

Show all the details of a random record for each State starting with 'A':

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| where State startswith "A"
| project StartTime, EpisodeId, State, EventType
| summarize take_any(*) by State
```

|State|StartTime|EpisodeId|EventType|
|---|---|---|---|
|ALASKA|2007-02-01 00:00:00.0000000|1733|Flood|
|ATLANTIC SOUTH|2007-09-29 08:11:00.0000000|11091|Waterspout|
|ATLANTIC NORTH|2007-11-27 00:00:00.0000000|11523|Marine Thunderstorm Wind|
|ARIZONA|2007-12-01 10:40:00.0000000|11955|Flash Flood|
|AMERICAN SAMOA|2007-12-07 14:00:00.0000000|13183|Flash Flood|
|ARKANSAS|2007-12-09 16:00:00.0000000|11319|Lightning|
|ALABAMA|2007-12-15 18:00:00.0000000|12580|Heavy Rain|
