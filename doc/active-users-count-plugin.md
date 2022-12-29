---
title: active_users_count plugin - Azure Data Explorer
description: Learn how to use the active_users_count plugin to calculate the distinct count of values that appeared in a minimum number of periods in a lookback period.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/20/2022
---
# active_users_count plugin

Calculates distinct count of values, where each value has appeared in at least a minimum number of periods in a lookback period.

Useful for calculating distinct counts of "fans" only, while not including appearances of "non-fans". A user is counted as a "fan" only if it was active during the lookback period. The lookback period is only used to determine whether a user is considered `active` ("fan") or not. The aggregation itself doesn't include users from the lookback window. In comparison, the [sliding_window_counts](sliding-window-counts-plugin.md) aggregation is performed over a sliding window of the lookback period.

## Syntax

*T* `| evaluate` `active_users_count(`*IdColumn*`,` *TimelineColumn*`,` *Start*`,` *End*`,` *LookbackWindow*`,` *Period*`,` *ActivePeriodsCount*`,` *Bin* `,` [*dim1*`,` *dim2*`,` ...]`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *T* | string | &check; | The tabular input used to count active users.|
| *IdColumn* | string | &check; | The name of the column with ID values that represent user activity. |
| *TimelineColumn* | string | &check; | The name of the column that represents timeline. |
| *Start* | datetime |  &check;  | The analysis start period. |
| *End* | datetime | &check; | The analysis end period. |
| *LookbackWindow* | timespan | &check; | The time window defining a period where user appearance is checked. The lookback period starts at ([current appearance] - [lookback window]) and ends on ([current appearance]). |
| *Period* | timespan | &check; | A constant to count as single appearance (a user will be counted as active if it appears in at least distinct ActivePeriodsCount of this timespan. |
| *ActivePeriodsCount* | decimal | &check; | The minimal number of distinct active periods to decide if user is active. Active users are those users who appeared in at least (equal or greater than) active periods count. |
| *Bin* | decimal, datetime, or timespan | &check; | A constant value of the analysis step period. May also be a string of `week`, `month`, or `year`. All periods will be the corresponding [startofweek](startofweekfunction.md), [startofmonth](startofmonthfunction.md), or[startofyear](startofyearfunction.md) functions. |
| *dim1*, *dim2*, ... | dynamic |   | An array of the dimensions columns that slice the activity metrics calculation. |

## Returns

Returns a table that has distinct count values for IDs that have appeared in ActivePeriodCounts in the following periods: the lookback period, each timeline period, and each existing dimensions combination.

Output table schema is:

|*TimelineColumn*|dim1|..|dim_n|dcount_values|
|---|---|---|---|---|
|type: as of *TimelineColumn*|..|..|..|long|

## Examples

Calculate weekly number of distinct users that appeared in at least three different days over a period of prior eight days. Period of analysis: July 2018.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA42SwWvCMBTG74X+Dw9PFlJI0rJWhweF3XYYWNlhDIltGMGaSJO6y/74vdZWNlFMcnrv/ZJ85Ptq6WDtRONgAZVw0qmDnHLK8phmMWXRcxjUiLzo6haQXIBXY/Y7Ue7fla7MN7J5NUzeZKNMd5qNnWXp1Eme+xYHydBfKY1VNmIFFt2TuHe1nG6sbObWNUp/EShQhXXicJyPmqIw+AgDwDVZTQj060rvU8xnEXkIJfQCLe9A+DX84U0IpT5Q7vEc89HEUh8o84A49RDOU/zzTzSrgB+QJ1G3SIDozd226JbdlqbVrnfuj2XkHDjShYpcBYcMcSH/Q0K6bES/nOHnRKwCAAA=" target="_blank">Run the query</a>

```kusto
let Start = datetime(2018-07-01);
let End = datetime(2018-07-31);
let LookbackWindow = 8d;
let Period = 1d;
let ActivePeriods = 3;
let Bin = 7d;
let T =  datatable(User:string, Timestamp:datetime)
[
    "B",      datetime(2018-06-29),
    "B",      datetime(2018-06-30),
    "A",      datetime(2018-07-02),
    "B",      datetime(2018-07-04),
    "B",      datetime(2018-07-08),
    "A",      datetime(2018-07-10),
    "A",      datetime(2018-07-14),
    "A",      datetime(2018-07-17),
    "A",      datetime(2018-07-20),
    "B",      datetime(2018-07-24)
];
T | evaluate active_users_count(User, Timestamp, Start, End, LookbackWindow, Period, ActivePeriods, Bin)
```

**Output**

|Timestamp|dcount|
|---|---|
|2018-07-01 00:00:00.0000000|1|
|2018-07-15 00:00:00.0000000|1|

A user is considered active if it fulfills both of the following criteria:

* The user was seen in at least three distinct days (Period = 1d, ActivePeriods=3).
* The user was seen in a lookback window of 8d before and including their current appearance.

In the illustration below, the only appearances that are active by this criteria are the following instances: User A on 7/20 and User B on 7/4 (see plugin results above).
The appearances of User B are included for the lookback window on 7/4, but not for the Start-End time range of 6/29-30.

:::image type="content" source="images/queries/active-users-count.png" alt-text="Graph showing active users based on the loopback window and active period specified in the query.":::
