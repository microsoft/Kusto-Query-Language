---
title: funnel_sequence_completion plugin - Azure Data Explorer
description: Learn how to use the funnel_sequence_completion plugin to calculate a funnel of completed sequence steps while comparing different time periods.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/12/2022
---
# funnel_sequence_completion plugin

Calculates a funnel of completed sequence steps while comparing different time periods. The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

```kusto
T | evaluate funnel_sequence_completion(id, datetime_column, startofday(ago(30d)), startofday(now()), 1d, state_column, dynamic(['S1', 'S2', 'S3']), dynamic([10m, 30min, 1h]))
```

## Syntax

*T* `| evaluate` `funnel_sequence_completion(`*IdColumn*`,` *TimelineColumn*`,` *Start*`,` *End*`,` *BinSize*`,` *StateColumn*`,` *Sequence*`,` *MaxSequenceStepWindows*`)`

## Arguments

* *T*: The input tabular expression.
* *IdColum*: column reference, must be present in the source expression.
* *TimelineColumn*: column reference representing timeline, must be present in the source expression.
* *Start*: scalar constant value of the analysis start period.
* *End*: scalar constant value of the analysis end period.
* *BinSize*: scalar constant value of the analysis window (bin) size, each window is analyzed separately.
* *StateColumn*: column reference representing the state, must be present in the source expression.
* *Sequence*: a constant dynamic array with the sequence values (values are looked up in `StateColumn`).
* *MaxSequenceStepPeriods*: scalar constant dynamic array with the values of the max allowed timespan between the first and last sequential steps in the sequence. Each period in the array generates a funnel analysis result.

## Returns

Returns a single table useful for constructing a funnel diagram for the analyzed sequence:

* `TimelineColumn`: the analyzed time window (bin), each bin in the analysis timeframe (*Start* to *End*) generates a funnel analysis separately.
* `StateColumn`: the state of the sequence.
* `Period`: the maximal period allowed for completing steps in the funnel sequence measured from the first step in the sequence. Each value in *MaxSequenceStepPeriods* generates a funnel analysis with a separate period.
* `dcount`: distinct count of `IdColumn` in time window that transitioned from first sequence state to the value of `StateColumn`.

## See also

* [scan operator](scan-operator.md)

## Examples

### Exploring Storm Events

The following query checks the completion funnel of the sequence: `Hail` -> `Tornado` -> `Thunderstorm Wind`
in "overall" time of 1hour, 4hours, 1day.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let _start = datetime(2007-01-01);
let _end =  datetime(2008-01-01);
let _windowSize = 365d;
let _sequence = dynamic(['Hail', 'Tornado', 'Thunderstorm Wind']);
let _periods = dynamic([1h, 4h, 1d]);
StormEvents
| evaluate funnel_sequence_completion(EpisodeId, StartTime, _start, _end, _windowSize, EventType, _sequence, _periods) 
```

|`StartTime`|`EventType`|`Period`|`dcount`|
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
The outcome is three funnels (for periods: One hour, 4 hours, and one day). For each funnel step, a number of distinct counts of  are shown. You can see that the more time is given to complete the whole sequence of `Hail` -> `Tornado` -> `Thunderstorm Wind`, the higher `dcount` value is obtained. In other words, there were more occurrences of the sequence reaching the funnel step.
