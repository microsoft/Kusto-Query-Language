---
title:  sliding_window_counts plugin
description: Learn how to use the sliding_window_counts plugin to calculate counts and distinct counts of values in a sliding window over a lookback period.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# sliding_window_counts plugin

Calculates counts and distinct count of values in a sliding window over a lookback period, using the technique described [here](samples.md#perform-aggregations-over-a-sliding-window). The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Syntax

*T* `| evaluate` `sliding_window_counts(`*IdColumn*`,` *TimelineColumn*`,` *Start*`,` *End*`,` *LookbackWindow*`,` *Bin* `,` [*dim1*`,` *dim2*`,` ...]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The input tabular expression.|
| *IdColumn* | string | &check; | The name of the column with ID values that represent user activity. |
| *TimelineColumn* | string | &check; | The name of the column representing the timeline.|
| *Start* | int, long, real, datetime, or timespan | &check; | The analysis start period.|
| *End* | int, long, real, datetime, or timespan | &check; | The analysis end period.|
| *LookbackWindow* | int, long, real, datetime, or timespan | &check; | The lookback period. This value should be a multiple of the *Bin* value, otherwise the *LookbackWindow* will be rounded down to a multiple of the *Bin* value. For example, for `dcount` users in past `7d`: *LookbackWindow* = `7d`.|
| *Bin* | int, long, real, datetime, timespan, or string | &check; | The analysis step period. The possible string values are `week`, `month`, and `year` for which all periods will be [startofweek](startofweekfunction.md), [startofmonth](startofmonthfunction.md), [startofyear](startofyearfunction.md) respectively. |
| *dim1*, *dim2*, ... | string | | A list of the dimensions columns that slice the activity metrics calculation.|

## Returns

Returns a table that has the count and distinct count values of Ids in the lookback period, for each timeline period (by bin) and for each existing dimensions combination.

Output table schema is:

|*TimelineColumn*|`dim1`|..|`dim_n`|`count`|`dcount`|
|---|---|---|---|---|---|
|type: as of *TimelineColumn*|..|..|..|long|long|

## Example

Calculate counts and `dcounts` for users in past week, for each day in the analysis period. 

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA63TQWvCMBQH8Hs/xbtpIYJtNxTLDhu76LljhzEk7QsuGJPRpIrgh99rM50MWiLYQiD0R96f8o8SDqzjtYMnQO6EkzsxTqfJDCYwnbdLEueRIiU09ppZnEOHlDHbklfbd6nRHMhnmIP/VEpN+wT9aYU/i95SifGbFfUSF5SklnrDoKAJlGr3vbgMjCOg56NbRy+mHLHevAy8euV7iXd0K/OlA1hYuGfNNfJemBI8S9eoQReQLg2dm/3BobE3s8F0WdC/u1LC2WOvewg67bFVn3lURCcQe64aUmCVRCrg+tDVd12ZRjv7286rVjJ/ZVh7J9i/zrO26PEPos0Ri1cDAAA=" target="_blank">Run the query</a>

```kusto
let start = datetime(2017 - 08 - 01);
let end = datetime(2017 - 08 - 07); 
let lookbackWindow = 3d;  
let bin = 1d;
let T = datatable(UserId: string, Timestamp: datetime)
    [
    'Bob', datetime(2017 - 08 - 01), 
    'David', datetime(2017 - 08 - 01), 
    'David', datetime(2017 - 08 - 01), 
    'John', datetime(2017 - 08 - 01), 
    'Bob', datetime(2017 - 08 - 01), 
    'Ananda', datetime(2017 - 08 - 02),  
    'Atul', datetime(2017 - 08 - 02), 
    'John', datetime(2017 - 08 - 02), 
    'Ananda', datetime(2017 - 08 - 03), 
    'Atul', datetime(2017 - 08 - 03), 
    'Atul', datetime(2017 - 08 - 03), 
    'John', datetime(2017 - 08 - 03), 
    'Bob', datetime(2017 - 08 - 03), 
    'Betsy', datetime(2017 - 08 - 04), 
    'Bob', datetime(2017 - 08 - 05), 
];
T
| evaluate sliding_window_counts(UserId, Timestamp, start, end, lookbackWindow, bin)
```

**Output**

|Timestamp|Count|`dcount`|
|---|---|---|
|2017-08-01 00:00:00.0000000|5|3|
|2017-08-02 00:00:00.0000000|8|5|
|2017-08-03 00:00:00.0000000|13|5|
|2017-08-04 00:00:00.0000000|9|5|
|2017-08-05 00:00:00.0000000|7|5|
|2017-08-06 00:00:00.0000000|2|2|
|2017-08-07 00:00:00.0000000|1|1|
