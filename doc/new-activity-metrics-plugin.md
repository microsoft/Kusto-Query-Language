---
title: new_activity_metrics plugin - Azure Data Explorer | Microsoft Docs
description: This article describes new_activity_metrics plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 02/20/2019
---
# new_activity_metrics plugin

Calculates useful activity metrics (distinct count values, distinct count of new values, retention rate, and churn rate) for the cohort of `New Users`.

Concept of this plugin is similar to [activity_metrics plugin](./activity-metrics-plugin.md), but focuses on `New Users`.

```kusto
T | evaluate new_activity_metrics(id, datetime_column, startofday(ago(30d)), startofday(now()), 1d, dim1, dim2, dim3)
```

**Syntax**

*T* `| evaluate` `new_activity_metrics(`*IdColumn*`,` *TimelineColumn*`,` *Start*`,` *End*`,` *Window* [`,` *Cohort*] [`,` *dim1*`,` *dim2*`,` ...] [`,` *Lookback*] `)`

**Arguments**

* *T*: The input tabular expression.
* *IdColumn*: The name of the column with ID values that represent user activity. 
* *TimelineColumn*: The name of the column that represent timeline.
* *Start*: Scalar with value of the analysis start period.
* *End*: Scalar with value of the analysis end period.
* *Window*: Scalar with value of the analysis window period. Can be either a numeric/datetime/timestamp value, or a string which is one of `week`/`month`/`year`, in which case all periods will be [startofweek](startofweekfunction.md)/[startofmonth](startofmonthfunction.md)/[startofyear](startofyearfunction.md) accordingly. 
* *Cohort*: (optional) a scalar constant indicating specific cohort. If not provided, all cohorts corresponding to the analysis time window are calculated and returned.
* *dim1*, *dim2*, ...: (optional) list of the dimensions columns that slice the activity metrics calculation.
* *Lookback*: (optional) a tabular expression with a set of IDs that belong to the 'look back' period

**Returns**

Returns a table that has the distinct count values, distinct count of new values, retention rate, and churn rate for each 
combination of 'from' and 'to' timeline periods and for each existing dimensions combination.

Output table schema is:

|from_TimelineColumn|to_TimelineColumn|dcount_new_values|dcount_retained_values|dcount_churn_values|retention_rate|churn_rate|dim1|..|dim_n|
|---|---|---|---|---|---|---|---|---|---|
|type: as of *TimelineColumn*|same|long|long|double|double|double|..|..|..|


**Notes**

For definitions of `Retention Rate` and `Churn Rate` - refer to **Notes** section in 
[activity_metrics plugin](./activity-metrics-plugin.md) documentation.


**Examples**

### Weekly retention rate, and churn rate (single week)

The next query calculates a retention and churn rate for week-over-week window for `New Users` cohort
(users that arrived on the first week).

```kusto
// Generate random data of user activities
let _start = datetime(2017-05-01);
let _end = datetime(2017-05-31);
range Day from _start to _end  step 1d
| extend d = tolong((Day - _start)/1d)
| extend r = rand()+1
| extend _users=range(tolong(d*50*r), tolong(d*50*r+200*r-1), 1) 
| mv-expand id=_users to typeof(long) limit 1000000
// Take only the first week cohort (last parameter)
| evaluate new_activity_metrics(['id'], Day, _start, _end, 7d, _start)
| project from_Day, to_Day, retention_rate, churn_rate
```

|from_Day|to_Day|retention_rate|churn_rate|
|---|---|---|---|
|2017-05-01 00:00:00.0000000|2017-05-01 00:00:00.0000000|1|0|
|2017-05-01 00:00:00.0000000|2017-05-08 00:00:00.0000000|0.544632768361582|0.455367231638418|
|2017-05-01 00:00:00.0000000|2017-05-15 00:00:00.0000000|0.031638418079096|0.968361581920904|
|2017-05-01 00:00:00.0000000|2017-05-22 00:00:00.0000000|0|1|
|2017-05-01 00:00:00.0000000|2017-05-29 00:00:00.0000000|0|1|


### Weekly retention rate, and churn rate (complete matrix)

The next query calculates retention and churn rate for week-over-week window for `New Users` cohort. If the previous
example calculated the statistics for a single week - the below produces NxN table for each from/to combination.

```kusto
// Generate random data of user activities
let _start = datetime(2017-05-01);
let _end = datetime(2017-05-31);
range Day from _start to _end  step 1d
| extend d = tolong((Day - _start)/1d)
| extend r = rand()+1
| extend _users=range(tolong(d*50*r), tolong(d*50*r+200*r-1), 1) 
| mv-expand id=_users to typeof(long) limit 1000000
// Last parameter is omitted - 
| evaluate new_activity_metrics(['id'], Day, _start, _end, 7d)
| project from_Day, to_Day, retention_rate, churn_rate
```

|from_Day|to_Day|retention_rate|churn_rate|
|---|---|---|---|
|2017-05-01 00:00:00.0000000|2017-05-01 00:00:00.0000000|1|0|
|2017-05-01 00:00:00.0000000|2017-05-08 00:00:00.0000000|0.190397350993377|0.809602649006622|
|2017-05-01 00:00:00.0000000|2017-05-15 00:00:00.0000000|0|1|
|2017-05-01 00:00:00.0000000|2017-05-22 00:00:00.0000000|0|1|
|2017-05-01 00:00:00.0000000|2017-05-29 00:00:00.0000000|0|1|
|2017-05-08 00:00:00.0000000|2017-05-08 00:00:00.0000000|1|0|
|2017-05-08 00:00:00.0000000|2017-05-15 00:00:00.0000000|0.405263157894737|0.594736842105263|
|2017-05-08 00:00:00.0000000|2017-05-22 00:00:00.0000000|0.227631578947368|0.772368421052632|
|2017-05-08 00:00:00.0000000|2017-05-29 00:00:00.0000000|0|1|
|2017-05-15 00:00:00.0000000|2017-05-15 00:00:00.0000000|1|0|
|2017-05-15 00:00:00.0000000|2017-05-22 00:00:00.0000000|0.785488958990536|0.214511041009464|
|2017-05-15 00:00:00.0000000|2017-05-29 00:00:00.0000000|0.237644584647739|0.762355415352261|
|2017-05-22 00:00:00.0000000|2017-05-22 00:00:00.0000000|1|0|
|2017-05-22 00:00:00.0000000|2017-05-29 00:00:00.0000000|0.621835443037975|0.378164556962025|
|2017-05-29 00:00:00.0000000|2017-05-29 00:00:00.0000000|1|0|


### Weekly retention rate with lookback period

The following query calculates the retention rate of `New Users` cohort when taking into 
consideration `lookback` period: a tabular query with set of Ids that are used to define
the `New Users` cohort (all IDs that do not appear in this set are `New Users`). The 
query examines the retention behavior of the `New Users` during the analysis period.

```kusto
// Generate random data of user activities
let _lookback = datetime(2017-02-01);
let _start = datetime(2017-05-01);
let _end = datetime(2017-05-31);
let _data = range Day from _lookback to _end  step 1d
| extend d = tolong((Day - _lookback)/1d)
| extend r = rand()+1
| extend _users=range(tolong(d*50*r), tolong(d*50*r+200*r-1), 1) 
| mv-expand id=_users to typeof(long) limit 1000000;
//
let lookback_data = _data | where Day < _start | project Day, id;
_data
| evaluate new_activity_metrics(id, Day, _start, _end, 7d, _start, lookback_data)
| project from_Day, to_Day, retention_rate
```

|from_Day|to_Day|retention_rate|
|---|---|---|
|2017-05-01 00:00:00.0000000|2017-05-01 00:00:00.0000000|1|
|2017-05-01 00:00:00.0000000|2017-05-08 00:00:00.0000000|0.404081632653061|
|2017-05-01 00:00:00.0000000|2017-05-15 00:00:00.0000000|0.257142857142857|
|2017-05-01 00:00:00.0000000|2017-05-22 00:00:00.0000000|0.296326530612245|
|2017-05-01 00:00:00.0000000|2017-05-29 00:00:00.0000000|0.0587755102040816|
