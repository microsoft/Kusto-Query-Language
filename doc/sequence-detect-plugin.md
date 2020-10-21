---
title: sequence_detect plugin - Azure Data Explorer
description: This article describes sequence_detect plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# sequence_detect plugin

Detects sequence occurrences based on provided predicates.

```kusto
T | evaluate sequence_detect(datetime_column, 10m, 1h, e1 = (Col1 == 'Val'), e2 = (Col2 == 'Val2'), Dim1, Dim2)
```

## Syntax

*T* `| evaluate` `sequence_detect` `(`*TimelineColumn*`,` *MaxSequenceStepWindow*`,` *MaxSequenceSpan*`,` *Expr1*`,` *Expr2*`,` ..., *Dim1*`,` *Dim2*`,` ...`)`

## Arguments

* *T*: The input tabular expression.
* *TimelineColumn*: column reference representing timeline, must be present in the source expression
* *MaxSequenceStepWindow*: scalar constant value of the max allowed timespan between 2 sequential steps in the sequence
* *MaxSequenceSpan*: scalar constant value of the max span for the sequence to complete all steps
* *Expr1*, *Expr2*, ...: boolean predicate expressions defining sequence steps
* *Dim1*, *Dim2*, ...: dimension expressions that are used to correlate sequences

## Returns

Returns a single table where each row in the table represents a single sequence occurence:

* *Dim1*, *Dim2*, ...: dimension columns that were used to correlate sequences.
* *Expr1*_*TimelineColumn*, *Expr2*_*TimelineColumn*, ...: Columns with time values, representing the timeline of each sequence step.
* *Duration*: the overall sequence time window

## Examples

### Exploring Storm Events 

The following query looks on the table StormEvents (weather statistics for 2007) and shows cases where sequence of 'Excessive Heat' was followed by 'Wildfire' within 5 days.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| evaluate sequence_detect(
        StartTime,
        5d,  // step max-time
        5d,  // sequence max-time
        heat=(EventType == "Excessive Heat"), 
        wildfire=(EventType == 'Wildfire'), 
        State)
```

|State|heat_StartTime|wildfire_StartTime|Duration|
|---|---|---|---|
|CALIFORNIA|2007-05-08 00:00:00.0000000|2007-05-08 16:02:00.0000000|16:02:00|
|CALIFORNIA|2007-05-08 00:00:00.0000000|2007-05-10 11:30:00.0000000|2.11:30:00|
|CALIFORNIA|2007-07-04 09:00:00.0000000|2007-07-05 23:01:00.0000000|1.14:01:00|
|SOUTH DAKOTA|2007-07-23 12:00:00.0000000|2007-07-27 09:00:00.0000000|3.21:00:00|
|TEXAS|2007-08-10 08:00:00.0000000|2007-08-11 13:56:00.0000000|1.05:56:00|
|CALIFORNIA|2007-08-31 08:00:00.0000000|2007-09-01 11:28:00.0000000|1.03:28:00|
|CALIFORNIA|2007-08-31 08:00:00.0000000|2007-09-02 13:30:00.0000000|2.05:30:00|
|CALIFORNIA|2007-09-02 12:00:00.0000000|2007-09-02 13:30:00.0000000|01:30:00|
