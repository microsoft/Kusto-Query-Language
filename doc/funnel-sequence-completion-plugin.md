---
title: funnel_sequence_completion plugin - Azure Data Explorer | Microsoft Docs
description: This article describes funnel_sequence_completion plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 01/28/2019
---
# funnel_sequence_completion plugin

Calculates funnel of completed sequence steps within comparing different time periods.

```kusto
T | evaluate funnel_sequence_completion(id, datetime_column, startofday(ago(30d)), startofday(now()), 1d, state_column, dynamic(['S1', 'S2', 'S3']), dynamic([10m, 30min, 1h]))
```

**Syntax**

*T* `| evaluate` `funnel_sequence_completion(`*IdColumn*`,` *TimelineColumn*`,` *Start*`,` *End*`,` *Step*`,` *StateColumn*`,` *Sequence*`,` *MaxSequenceStepWindow*`)`

**Arguments**

* *T*: The input tabular expression.
* *IdColum*: column reference, must be present in the source expression
* *TimelineColumn*: column reference representing timeline, must be present in the source expression
* *Start*: scalar constant value of the analysis start period
* *End*: scalar constant value of the analysis end period
* *Step*: scalar constant value of the analysis step period (bin) 
* *StateColumn*: column reference representing the state, must be present in the source expression
* *Sequence*: a constant dynamic array with the sequence values (values are looked up in `StateColumn`)
* *MaxSequenceStepWindows*: scalar constant value of the max allowed timespan between the first and last sequential steps in the sequence

**Returns**

Returns a single table useful for constructing a funnel diagram for the analyzed sequence:

* TimelineColumn: the analyzed time window
* `StateColumn`: the state of the sequence.
* Period: the next state (may be empty if there were any users which only had events for the searched sequence, but not any events that followed it). 
* dcount: distinct count of `IdColumn` in time window that transitioned from first sequence state to the value of `StateColumn`.

**Examples**

### Exploring Storm Events 

The following query checks the completion funnel of the sequence: `Hail` -> `Tornado` -> `Thunderstorm Wind`
in "overall" time of 1hour, 4hours, 1day. 

```kusto
let _start = datetime(2007-01-01);
let _end =  datetime(2008-01-01);
let _windowSize = 365d;
let _sequence = dynamic(['Hail', 'Tornado', 'Thunderstorm Wind']);
let _periods = dynamic([1h, 4h, 1d]);
StormEvents
| evaluate funnel_sequence_completion(EpisodeId, StartTime, _start, _end, _windowSize, EventType, _sequence, _periods) 
```

|StartTime|EventType|Period|dcount|
|---|---|---|---|
|2007-01-01 00:00:00.0000000|Hail|01:00:00|2877|
|2007-01-01 00:00:00.0000000|Tornado|01:00:00|208|
|2007-01-01 00:00:00.0000000|Thunderstorm Wind|01:00:00|87|
|2007-01-01 00:00:00.0000000|Hail|04:00:00|2877|
|2007-01-01 00:00:00.0000000|Tornado|04:00:00|231|
|2007-01-01 00:00:00.0000000|Thunderstorm Wind|04:00:00|141|
|2007-01-01 00:00:00.0000000|Hail|1.00:00:00|2877|
|2007-01-01 00:00:00.0000000|Tornado|1.00:00:00|244|
|2007-01-01 00:00:00.0000000|Thunderstorm Wind|1.00:00:00|155|

Understanding the results:  
The outcome it 3 funnels (for periods: 1 hour, 4 hours, and 1 day), while for each funnel step a number 
of distinct count of EpisodeId is shown. You can see that the more time is given to complete the whole sequence of `Hail` -> `Tornado` -> `Thunderstorm Wind` the higher `dcount` value (meaning more occurrences of the sequence reaching the step of the funnel).
