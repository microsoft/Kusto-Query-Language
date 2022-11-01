---
title: iff() - Azure Data Explorer
description: This article describes iff() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# iff()

Evaluates the first argument (the predicate), and returns the value of either the second or third arguments, depending on whether the predicate evaluated to `true` (second) or `false` (third).

The second and third arguments must be of the same type.

## Syntax

`iff(`*predicate*`,` *ifTrue*`,` *ifFalse*`)`

## Arguments

* *predicate*: An expression that evaluates to a `boolean` value.
* *ifTrue*: An expression that gets evaluated and its value returned from the function if *predicate* evaluates to `true`.
* *ifFalse*: An expression that gets evaluated and its value returned from the function if *predicate* evaluates to `false`.

## Returns

This function returns the value of *ifTrue* if *predicate* evaluates to `true`,
or the value of *ifFalse* otherwise.

## Example

<a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRSK0oSc1LUQhKzMxTsFXITEvT0ADLhVQWpCoAxTSUPFITyyrBCpR0FJTcchKLMxTccvLzUyBcEENTE8gEG5EK0guS8MsvUShCiGgCrSooys9KTS5RCC5JLEnVUQBb45kCZYDs0wHbAgAA0TJCoAAAAA==" target="_blank">Click to run query</a>

```kusto
StormEvents
| extend Rain = iff((EventType in ("Heavy Rain", "Flash Flood", "Flood")), "Rain event", "Not rain event")
| project State, EventId, EventType, Rain
```
