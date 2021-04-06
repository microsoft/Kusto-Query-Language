---
title: activity_counts_metrics plugin - Azure Data Explorer 
description: This article describes activity_counts_metrics plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# activity_counts_metrics plugin

Calculates useful activity metrics for each time window compared/aggregated to *all* previous time windows. Metrics include: total count values, distinct count values, distinct count of new values, and aggregated distinct count. Compare this plugin to [activity_metrics plugin](activity-metrics-plugin.md), in which every time window is compared to its previous time window only.

```kusto
T | evaluate activity_counts_metrics(id, datetime_column, startofday(ago(30d)), startofday(now()), 1d, dim1, dim2, dim3)
```

## Syntax

*T* `| evaluate` `activity_counts_metrics(`*IdColumn*`,` *TimelineColumn*`,` *Start*`,` *End*`,` *Window* [`,` *Cohort*] [`,` *dim1*`,` *dim2*`,` ...] [`,` *Lookback*] `)`

## Arguments

* *T*: The input tabular expression.
* *IdColumn*: The name of the column with ID values that represent user activity. 
* *TimelineColumn*: The name of the column that represents the timeline.
* *Start*: Scalar with value of the analysis start period.
* *End*: Scalar with value of the analysis end period.
* *Window*: Scalar with value of the analysis window period. Can be either a numeric/datetime/timestamp value, or a string that is one of `week`/`month`/`year`, in which case all periods will be [startofweek](startofweekfunction.md)/[startofmonth](startofmonthfunction.md) or [startofyear](startofyearfunction.md). 
* *dim1*, *dim2*, ...: (optional) list of the dimensions columns that slice the activity metrics calculation.

## Returns

Returns a table that has: total count values, distinct count values, distinct count of new values, and aggregated distinct count for each time window.

Output table schema is:

|`TimelineColumn`|`dim1`|...|`dim_n`|`count`|`dcount`|`new_dcount`|`aggregated_dcount`|
|---|---|---|---|---|---|---|---|
|type: as of *`TimelineColumn`*|..|..|..|long|long|long|long|


* *`TimelineColumn`*: The time window start time.
* *`count`*: The total records count in the time window and *dim(s)*
* *`dcount`*: The distinct ID values count in the time window and *dim(s)*
* *`new_dcount`*: The distinct ID values in the time window and *dim(s)* compared to all previous time windows. 
* *`aggregated_dcount`*: The total aggregated distinct ID values of *dim(s)* from first-time window to current (inclusive).

## Examples

### Daily activity counts 

The next query calculates daily activity counts for the provided input table

<!-- csl: https://help.kusto.windows.net:443/Samples -->
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

|`Timestamp`|`count`|`dcount`|`new_dcount`|`aggregated_dcount`|
|---|---|---|---|---|
|2017-08-01 00:00:00.0000000|4|4|4|4|
|2017-08-02 00:00:00.0000000|3|3|2|6|
|2017-08-03 00:00:00.0000000|6|5|2|8|
|2017-08-04 00:00:00.0000000|1|1|0|8|


