---
title: active_users_count plugin - Azure Data Explorer | Microsoft Docs
description: This article describes active_users_count plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 01/15/2019
---
# active_users_count plugin

Calculates distinct count of values, where each value has appeared in at least a minimum number of periods in a lookback period.

Useful for calculating distinct counts of "fans" only,  while not including appearances of "non-fans". A user is counted as a "fan" only if it was active during the lookback period. 

```kusto
T | evaluate active_users_count(id, datetime_column, startofday(ago(30d)), startofday(now()), 7d, 1d, 2, 7d, dim1, dim2, dim3)
```

**Syntax**

*T* `| evaluate` `active_users_count(`*IdColumn*`,` *TimelineColumn*`,` *Start*`,` *End*`,` *LookbackWindow*`,` *Period*`,` *ActivePeriodsCount*`,` *Bin* `,` [*dim1*`,` *dim2*`,` ...]`)`

**Arguments**

* *T*: The input tabular expression.
* *IdColumn*: The name of the column with ID values that represent user activity. 
* *TimelineColumn*: The name of the column that represent timeline.
* *Start*: (optional) Scalar with value of the analysis start period.
* *End*: (optional) Scalar with value of the analysis end period.
* *LookbackWindow*: A sliding time window defining a period where user appearance is checked. Lookback period starts at ([current appearance] - [lookback window]) and ends on ([current appearance]). 
* *Period*: Scalar constant timespan to count as single appearance (a user will be counted as active if it appears in at least distinct ActivePeriodsCount of this timespan.
* *ActivePeriodsCount*: Minimal number of distinct active periods to decide if user is active. Active users are those who appeared in at least (equal or greater than) active periods count.
* *Bin*: Scalar constant value of the analysis step period. Can be either a numeric/datetime/timestamp value, or a string which is one of `week`/`month`/`year`, in which case all periods will be [startofweek](startofweekfunction.md)/[startofmonth](startofmonthfunction.md)/[startofyear](startofyearfunction.md) accordingly.
* *dim1*, *dim2*, ...: (optional) list of the dimensions columns that slice the activity metrics calculation.

**Returns**

Returns a table that has the distinct count values for Ids that have appeared in over ActivePeriodCounts in the lookback period, for each timeline period and for each existing dimensions combination.

Output table schema is:

|*TimelineColumn*|dim1|..|dim_n|dcount_values|
|---|---|---|---|---|
|type: as of *TimelineColumn*|..|..|..|long|


**Examples**

Calculate weekly amount of distinct users that appeared in at least on 3 different days over a period of prior 8 days. Period of analysis: July 2018.

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

|Timestamp|dcount|
|---|---|
|2018-07-01 00:00:00.0000000|1|
|2018-07-15 00:00:00.0000000|1|

A user is considered active if it was seen in at least 3 distinct days (Period = 1d, ActivePeriods=3) in a lookback window of 8d previous to current appearance (including current appearance). In the illustration below, the only appearances that are active according to this criteria, are User A on the 7/20 and User B on the 7/4 (see plugin results above). Note that although the appearances of User B on 6/29-30 are not in the Start-End time range, they are included for the lookback window of User B on the 7/4. 

![alt text](images/queries/active-users-count.png "active-users-count")