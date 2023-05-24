---
title:  iff()
description: This article describes iff() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/28/2022
---
# iff()

Returns the value of *then* if *if* evaluates to `true`,
or the value of *else* otherwise.

> The `iff()` and `iif()` functions are equivalent

## Syntax

`iff(`*if*`,` *then*`,` *else*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*if*| string | &check; | An expression that evaluates to a boolean value.|
|*then*| scalar | &check; | An expression that gets evaluated and its value returned from the function if *if* evaluates to `true`.|
|*else*| scalar | &check; | An expression that gets evaluated and its value returned from the function if *if* evaluates to `false`.|

## Returns

This function returns the value of *then* if *if* evaluates to `true`,
or the value of *else* otherwise.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRSK0oSc1LUQhKzMxTsFXITEvT0ADLhVQWpCoAxTSUPFITyyrBCpR0FJTcchKLMxTccvLzUyBcEENTE8gEG5EK0guS8MsvUShCiGgCrSooys9KTS5RCC5JLEnVUQBb45kCZYDs0wHbAgAA0TJCoAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| extend Rain = iff((EventType in ("Heavy Rain", "Flash Flood", "Flood")), "Rain event", "Not rain event")
| project State, EventId, EventType, Rain
```

**Output**

The following table shows only the first 5 rows.

|State|EventId|EventType|Rain|
|--|--|--|--|
|ATLANTIC SOUTH| 61032 |Waterspout |Not rain event
|FLORIDA| 60904 |Heavy Rain |Rain event
|FLORIDA| 60913 |Tornado |Not rain event
|GEORGIA| 64588 |Thunderstorm Wind |Not rain event
|MISSISSIPPI| 68796 |Thunderstorm Wind |Not rain event
|...|...|...|...|
