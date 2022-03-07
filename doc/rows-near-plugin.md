---
title: rows_near plugin - Azure Data Explorer
description: This article describes rows_near plugin in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/24/2022
---
# rows_near() plugin

Finds rows near a specified condition.

The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Syntax

*T* `| evaluate` `rows_near(`*Condition*`,` *NumRows*`,` [`,` *RowsAfter* ]`)`

## Arguments

* *T*: A serialized input tabular expression.
* *Condition*: Boolean expression representing the condition to find rows around. 
* *NumRows*: The number of rows to find before and after the condition.
* *RowsAfter*: (optional) when specified, overrides the number of rows to find after the condition.

## Returns

Every row from the input that is within *NumRows* from a `true` *Condition*,
When *RowsAfter* is specified, returns every row from the input that is *NumRows* before or *RowsAfter* after a `true` *Condition*.


## Example

Find rows with an `"Error"` *State*, and returns `2` rows before and after the `"Error"` record.


```kusto
datatable (Timestamp:datetime, Value:long, State:string )
[
    datetime(2021-06-01), 1, "Success",
    datetime(2021-06-02), 4, "Success",
    datetime(2021-06-03), 3, "Success",
    datetime(2021-06-04), 11, "Success",
    datetime(2021-06-05), 15, "Success",
    datetime(2021-06-06), 2, "Success",
    datetime(2021-06-07), 19, "Error",
    datetime(2021-06-08), 12, "Success",
    datetime(2021-06-09), 7, "Success",
    datetime(2021-06-10), 9, "Success",
    datetime(2021-06-11), 4, "Success",
    datetime(2021-06-12), 1, "Success",
]
| sort by Timestamp asc 
| evaluate rows_near(State == "Error", 2)
```

|Timestamp|Value|State|
|---|---|---|
|2021-06-05 00:00:00.0000000|15|Success|
|2021-06-06 00:00:00.0000000|2|Success|
|2021-06-07 00:00:00.0000000|19|Error|
|2021-06-08 00:00:00.0000000|12|Success|
|2021-06-09 00:00:00.0000000|7|Success|
