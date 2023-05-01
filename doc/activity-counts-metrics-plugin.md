---
title: activity_counts_metrics plugin - Azure Data Explorer
description: Learn how to use the activity_counts_metrics plugin to compare activity metrics in different time windows.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/15/2023
---
# activity_counts_metrics plugin

Calculates useful activity metrics for each time window compared/aggregated to *all* previous time windows. Metrics include: total count values, distinct count values, distinct count of new values, and aggregated distinct count. Compare this plugin to [activity_metrics plugin](activity-metrics-plugin.md), in which every time window is compared to its previous time window only.

## Syntax

*T* `| evaluate` `activity_counts_metrics(`*IdColumn*`,` *TimelineColumn*`,` *Start*`,` *End*`,` *Step* [`,` *Dimensions*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular input used to count activities. |
| *IdColumn* | string | &check; | The name of the column with ID values that represent user activity. |
| *TimelineColumn* | string | &check; | The name of the column that represents the timeline. |
| *Start* | datetime | &check; | The analysis start period. |
| *End* | datetime | &check; | The analysis end period. |
| *Step* | decimal, datetime, or timespan | &check; | The analysis window period. The value may also be a string of `week`, `month`, or `year`, in which case all periods would be [startofweek](startofweekfunction.md), [startofmonth](startofmonthfunction.md), or [startofyear](startofyearfunction.md). |
|  *Dimensions* | string |   | Zero or more comma-separated dimensions columns that slice the activity metrics calculation. |

## Returns

Returns a table that has the total count values, distinct count values, distinct count of new values, and aggregated distinct count for each time window. If *Dimensions* are provided, then there's another column for each dimension in the output table.

The following table describes the output table schema.

| Column name | Type | Description |
|---|---|---|
| `Timestamp` | Same as the provided *TimelineColumn* argument| The time window start time. |
| `count` | long | The total records count in the time window and *dim(s)* |
| `dcount` | long | The distinct ID values count in the time window and *dim(s)* |
| `new_dcount` | long | The distinct ID values in the time window and *dim(s)* compared to all previous time windows. |
| `aggregated_dcount` | long | The total aggregated distinct ID values of *dim(s)* from first-time window to current (inclusive). |

## Examples

### Daily activity counts

The next query calculates daily activity counts for the provided input table.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA5XSzwuCMBQH8Lvg/7CbCQs0g6Lw0I9DdW2dImS5EYOp4V5J0B/fKxU6jEUMD+PzeG/yfVoCMcBrSAUHCaqQg1EUT4bRdBjF4dz3NBbIUlh43HOjSlE1aSy6OyMpwXI8Zy0HByPrrZgZqFV5oYRhDxxYXGd9x9D3jr4XLAJKbG+gaGuH7Ry2dNjKaqOPMYfZ57W2sVri6Pnb7PMSx/+1tv/bxm87YYiE4fck8s71DasIz0HdFTyyvLqVYLJCYpi56aL9ypS2u0TfG0O7vQhfyOSsfmMCAAA=" target="_blank">Run the query</a>

```kusto
let start=datetime(2017-08-01);
let end=datetime(2017-08-04);
let window=1d;
let T = datatable(UserId:string, Timestamp:datetime)
[
'A', datetime(2017-08-01),
'D', datetime(2017-08-01),
'J', datetime(2017-08-01),
'B', datetime(2017-08-01),
'C', datetime(2017-08-02),
'T', datetime(2017-08-02),
'J', datetime(2017-08-02),
'H', datetime(2017-08-03),
'T', datetime(2017-08-03),
'T', datetime(2017-08-03),
'J', datetime(2017-08-03),
'B', datetime(2017-08-03),
'S', datetime(2017-08-03),
'S', datetime(2017-08-04),
];
 T
 | evaluate activity_counts_metrics(UserId, Timestamp, start, end, window)
```

**Output**

|`Timestamp`|`count`|`dcount`|`new_dcount`|`aggregated_dcount`|
|---|---|---|---|---|
|2017-08-01 00:00:00.0000000|4|4|4|4|
|2017-08-02 00:00:00.0000000|3|3|2|6|
|2017-08-03 00:00:00.0000000|6|5|2|8|
|2017-08-04 00:00:00.0000000|1|1|0|8|
