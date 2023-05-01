---
title: take_anyif() (aggregation function) - Azure Data Explorer
description: Learn how to use the take_anyif() function to return the value of an arbitrarily selected record for which the predicate is 'true'.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/05/2023
---
# take_anyif() (aggregation function)

Arbitrarily selects one record for each group in a [summarize operator](summarizeoperator.md) in records for which the *predicate*
is 'true'. The function returns the value of an expression over each such record.

This function is useful when you want to get a sample value of one column per value of the compound group key, subject to some predicate that is *true*. If such a value is present, the function attempts to return a non-null/non-empty value.

> **Deprecated aliases:** anyif()

> [!NOTE]
> The deprecated version adds `any_` prefix to the columns returned by the `any()` aggregation.

## Syntax

`take_anyif(` *expr*`,` *predicate* `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr* | string | &check; | The expression used for selecting a record. |
| *predicate* | string | &check; | Indicates which records may be considered for evaluation. |

## Returns

The `take_anyif` aggregation function returns the value of the expression calculated
for each of the records randomly selected from each group of the summarize operator. Only records for which *predicate* returns 'true' may be selected. If the predicate doesn't return 'true', a null value is produced.

## Examples

Pick a random EventType from Storm events, where event description has a key phrase.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvc6bc487453a064d3c9de.northeurope/databases/NewDatabase1?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5uWqUSguzc1NLMqsSlUoScxOjU/Mq8xM0wBLh1QWpOoogJl+iUVFiSWZZakKGYnFCurFJUX5eekK5Zl5KeqaABMQsZRQAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize take_anyif(EventType, EventNarrative has 'strong wind')
```

**Output**

|EventType|
|---|
|Strong Wind|
