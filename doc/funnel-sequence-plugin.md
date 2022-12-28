---
title: funnel_sequence plugin - Azure Data Explorer
description: Learn how to use the funnel_sequence plugin to learn how to calculate the distinct count of users who have taken a sequence of states, and the distribution of previous/next states that have led to/were followed by the sequence.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/18/2022
---
# funnel_sequence plugin

Calculates distinct count of users who have taken a sequence of states, and the distribution of previous/next states that have led to/were followed by the sequence. The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Syntax

*T* `| evaluate` `funnel_sequence(`*IdColumn*`,` *TimelineColumn*`,` *Start*`,` *End*`,` *MaxSequenceStepWindow*, *Step*, *StateColumn*, *Sequence*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The input tabular expression. |
| *IdColum* | string | &check; | The column reference representing the ID. This column must be present in *T*.|
| *TimelineColumn* | string | &check; | The column reference representing the timeline. This column must be present in *T*.|
| *Start* | datetime, timespan, or long | &check; | The analysis start period.|
| *End* | datetime, timespan, or long | &check; | The analysis end period.|
| *MaxSequenceStepWindow* | datetime, timespan, or long | &check; | The value of the max allowed timespan between two sequential steps in the sequence.|
| *Step* | datetime, timespan, or long | &check; | The analysis step period, or bin. |
| *StateColumn* | string | &check; | The column reference representing the state. This column must be present in *T*.|
| *Sequence* | dynamic | &check; | An array with the sequence values that are looked up in `StateColumn`.|

## Returns

Returns three output tables, which are useful for constructing a sankey diagram for the analyzed sequence:

* Table #1 - prev-sequence-next `dcount`
  * TimelineColumn: the analyzed time window
  * prev: the prev state (may be empty if there were any users that only had events for the searched sequence, but not any events prior to it).
  * next: the next state (may be empty if there were any users that only had events for the searched sequence, but not any events that followed it).
  * `dcount`: distinct count of `IdColumn` in time window that transitioned `prev` --> `Sequence` --> `next`.
  * samples: an array of IDs (from `IdColumn`) corresponding to the row's sequence (a maximum of 128 IDs are returned).

* Table #2 - prev-sequence `dcount`
  * TimelineColumn: the analyzed time window
  * prev: the prev state (may be empty if there were any users that only had events for the searched sequence, but not any events prior to it).
  * `dcount`: distinct count of `IdColumn` in time window that transitioned `prev` --> `Sequence` --> `next`.
  * samples: an array of IDs (from `IdColumn`) corresponding to the row's sequence (a maximum of 128 IDs are returned).

* Table #3 - sequence-next `dcount`
  * TimelineColumn: the analyzed time window
  * next: the next state (may be empty if there were any users that only had events for the searched sequence, but not any events that followed it).
  * `dcount`: distinct count of `IdColumn` in time window that transitioned `prev` --> `Sequence` --> `next`.
  * samples: an array of IDs (from `IdColumn`) corresponding to the row's sequence (a maximum of 128 IDs are returned).

## Examples

### Exploring storm events

The following query looks at the table StormEvents (weather statistics for 2007) and shows which events happened before/after all Tornado events occurred in 2007.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA12OuwrCQBBFe79iOhOI5CE+SGNlIdiIAQsRGbMTXUxm4+5EEPx4VxFRYYrhcC73xjEsjTlrPoJhWIuxzfxKLA6coGgnunQ59OIYVmkOmxMKnLBtiR0cqDKWoDCWURmgZ2z2MrM/Eysh+y9+dfXuHmLdoRBUHTPVe0eXjrikYN5qZxQtVOTXoZVCNxSB8qr4L8iSZDJIUn/hL51+aKqi4Xjk86+y4tY+8zfGRpfBtv9e1d+F4QP9Gyd+DAEAAA==" target="_blank">Run the query</a>

```kusto
// Looking on StormEvents statistics: 
// Q1: What happens before Tornado event?
// Q2: What happens after Tornado event?
StormEvents
| evaluate funnel_sequence(EpisodeId, StartTime, datetime(2007-01-01), datetime(2008-01-01), 1d,365d, EventType, dynamic(['Tornado']))
```

Result includes three tables:

* Table #1: All possible variants of what happened before and after the sequence. For example, the second line means that there were 87 different events that had following sequence: `Hail` -> `Tornado` -> `Hail`

|`StartTime`|`prev`|`next`|`dcount`|
|---|---|---|---|
|2007-01-01 00:00:00.0000000|||293|
|2007-01-01 00:00:00.0000000|Hail|Hail|87|
|2007-01-01 00:00:00.0000000|Thunderstorm Wind|Thunderstorm Wind|77|
|2007-01-01 00:00:00.0000000|Hail|Thunderstorm Wind|28|
|2007-01-01 00:00:00.0000000|Hail||28|
|2007-01-01 00:00:00.0000000||Hail|27|
|2007-01-01 00:00:00.0000000||Thunderstorm Wind|25|
|2007-01-01 00:00:00.0000000|Thunderstorm Wind|Hail|24|
|2007-01-01 00:00:00.0000000|Thunderstorm Wind||24|
|2007-01-01 00:00:00.0000000|Flash Flood|Flash Flood|12|
|2007-01-01 00:00:00.0000000|Thunderstorm Wind|Flash Flood|8|
|2007-01-01 00:00:00.0000000|Flash Flood||8|
|2007-01-01 00:00:00.0000000|Funnel Cloud|Thunderstorm Wind|6|
|2007-01-01 00:00:00.0000000||Funnel Cloud|6|
|2007-01-01 00:00:00.0000000||Flash Flood|6|
|2007-01-01 00:00:00.0000000|Funnel Cloud|Funnel Cloud|6|
|2007-01-01 00:00:00.0000000|Hail|Flash Flood|4|
|2007-01-01 00:00:00.0000000|Flash Flood|Thunderstorm Wind|4|
|2007-01-01 00:00:00.0000000|Hail|Funnel Cloud|4|
|2007-01-01 00:00:00.0000000|Funnel Cloud|Hail|4|
|2007-01-01 00:00:00.0000000|Funnel Cloud||4|
|2007-01-01 00:00:00.0000000|Thunderstorm Wind|Funnel Cloud|3|
|2007-01-01 00:00:00.0000000|Heavy Rain|Thunderstorm Wind|2|
|2007-01-01 00:00:00.0000000|Flash Flood|Funnel Cloud|2|
|2007-01-01 00:00:00.0000000|Flash Flood|Hail|2|
|2007-01-01 00:00:00.0000000|Strong Wind|Thunderstorm Wind|1|
|2007-01-01 00:00:00.0000000|Heavy Rain|Flash Flood|1|
|2007-01-01 00:00:00.0000000|Heavy Rain|Hail|1|
|2007-01-01 00:00:00.0000000|Hail|Flood|1|
|2007-01-01 00:00:00.0000000|Lightning|Hail|1|
|2007-01-01 00:00:00.0000000|Heavy Rain|Lightning|1|
|2007-01-01 00:00:00.0000000|Funnel Cloud|Heavy Rain|1|
|2007-01-01 00:00:00.0000000|Flash Flood|Flood|1|
|2007-01-01 00:00:00.0000000|Flood|Flash Flood|1|
|2007-01-01 00:00:00.0000000||Heavy Rain|1|
|2007-01-01 00:00:00.0000000|Funnel Cloud|Lightning|1|
|2007-01-01 00:00:00.0000000|Lightning|Thunderstorm Wind|1|
|2007-01-01 00:00:00.0000000|Flood|Thunderstorm Wind|1|
|2007-01-01 00:00:00.0000000|Hail|Lightning|1|
|2007-01-01 00:00:00.0000000||Lightning|1|
|2007-01-01 00:00:00.0000000|Tropical Storm|Hurricane (Typhoon)|1|
|2007-01-01 00:00:00.0000000|Coastal Flood||1|
|2007-01-01 00:00:00.0000000|Rip Current||1|
|2007-01-01 00:00:00.0000000|Heavy Snow||1|
|2007-01-01 00:00:00.0000000|Strong Wind||1|

* Table #2: shows all distinct events grouped by the previous event. For example, the second line shows that there were a total of 150 events of `Hail` that happened just before `Tornado`.

|`StartTime`|`prev`|`dcount`|
|---------|-----|------|
|2007-01-01 00:00:00.0000000||331|
|2007-01-01 00:00:00.0000000|Hail|150|
|2007-01-01 00:00:00.0000000|Thunderstorm Wind|135|
|2007-01-01 00:00:00.0000000|Flash Flood|28|
|2007-01-01 00:00:00.0000000|Funnel Cloud|22|
|2007-01-01 00:00:00.0000000|Heavy Rain|5|
|2007-01-01 00:00:00.0000000|Flood|2|
|2007-01-01 00:00:00.0000000|Lightning|2|
|2007-01-01 00:00:00.0000000|Strong Wind|2|
|2007-01-01 00:00:00.0000000|Heavy Snow|1|
|2007-01-01 00:00:00.0000000|Rip Current|1|
|2007-01-01 00:00:00.0000000|Coastal Flood|1|
|2007-01-01 00:00:00.0000000|Tropical Storm|1|

* Table #3: shows all distinct events grouped by next event. For example, the second line shows that there were a total of 143 events of `Hail` that happened after `Tornado`.

|`StartTime`|`next`|`dcount`|
|---------|-----|------|
|2007-01-01 00:00:00.0000000||332|
|2007-01-01 00:00:00.0000000|Hail|145|
|2007-01-01 00:00:00.0000000|Thunderstorm Wind|143|
|2007-01-01 00:00:00.0000000|Flash Flood|32|
|2007-01-01 00:00:00.0000000|Funnel Cloud|21|
|2007-01-01 00:00:00.0000000|Lightning|4|
|2007-01-01 00:00:00.0000000|Heavy Rain|2|
|2007-01-01 00:00:00.0000000|Flood|2|
|2007-01-01 00:00:00.0000000|Hurricane (Typhoon)|1|

Now, let's try to find out how the following sequence continues:  
`Hail` -> `Tornado` -> `Thunderstorm Wind`

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRSC1LzClNLElVSCvNy0vNiS9OLSxNzUtO1eBSQAWuBZnF+Smpnik66DLBJYlFJSGZuakYMilAg0uAEhpGBgbmugaGQKSJV5EFLkWGmNYam5liCoL9FVJZkKqjgGFPZV5ibmayRrS6R2JmjrqOgnpIflFeYko+mJlRmpeSWlQMChuF8My8FPVYTWQDNAHldk1eNgEAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| evaluate funnel_sequence(
               EpisodeId,
               StartTime,
               datetime(2007-01-01),
               datetime(2008-01-01),
               1d,
               365d,
               EventType, 
               dynamic(['Hail', 'Tornado', 'Thunderstorm Wind'])
           )
```

Skipping `Table #1` and `Table #2`, and looking at `Table #3`, we can conclude that sequence `Hail` -> `Tornado` -> `Thunderstorm Wind` in 92 events ended with this sequence, continued as `Hail` in 41 events, and turned back to `Tornado` in 14.

|`StartTime`|`next`|`dcount`|
|---------|-----|------|
|2007-01-01 00:00:00.0000000||92|
|2007-01-01 00:00:00.0000000|Hail|41|
|2007-01-01 00:00:00.0000000|Tornado|14|
|2007-01-01 00:00:00.0000000|Flash Flood|11|
|2007-01-01 00:00:00.0000000|Lightning|2|
|2007-01-01 00:00:00.0000000|Heavy Rain|1|
|2007-01-01 00:00:00.0000000|Flood|1|
