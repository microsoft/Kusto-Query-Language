---
title:  new_activity_metrics plugin
description: Learn how to use the new_activity_metrics plugin to calculate activity metrics. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/08/2023
---
# new_activity_metrics plugin

Calculates useful activity metrics (distinct count values, distinct count of new values, retention rate, and churn rate) for the cohort of `New Users`. Each cohort of `New Users` (all users, which were first seen in time window) is compared to all prior cohorts.
Comparison takes into account *all* previous time windows. For example, for records from T2 to T3, the distinct count of users will be all users in T3 who weren't seen in both T1 and T2.
The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Syntax

*TabularExpression* `| evaluate` `new_activity_metrics(`*IdColumn*`,` *TimelineColumn*`,` *Start*`,` *End*`,` *Window* [`,` *Cohort*] [`,` *dim1*`,` *dim2*`,` ...] [`,` *Lookback*] `)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *TabularExpression*| string | &check; | The tabular expression for which to calculate activity metrics.|
| *IdColumn*| string | &check; | The name of the column with ID values that represent user activity.|
| *TimelineColumn*| string | &check; | The name of the column that represents the timeline.|
| *Start*| scalar | &check; | The value of the analysis start period.|
| *End*| scalar | &check; | The value of the analysis end period.|
| *Window*| scalar | &check; | The value of the analysis window period. Can be a numeric, datetime, or timespan value, or a string that is one of `week`, `month` or `year`, in which case all periods will be [startofweek](startofweekfunction.md)/[startofmonth](startofmonthfunction.md)/[startofyear](startofyearfunction.md) accordingly. When using `startofweek`, make sure start time is a Sunday, otherwise first cohort will be empty (since [startofweek](startofweekfunction.md) is considered to be a Sunday).|
| *Cohort*| scalar | | Indicates a specific cohort. If not provided, all cohorts corresponding to the analysis time window are calculated and returned.|
| *dim1*, *dim2*, ...| dynamic | | An array of the dimensions columns that slice the activity metrics calculation.|
| *Lookback*| string | | A tabular expression with a set of IDs that belong to the 'look back' period.|

## Returns

Returns a table that contains the following for each combination of 'from' and 'to' timeline periods and for each existing column (dimensions) combination:

* distinct count values
* distinct count of new values
* retention rate
* churn rate

Output table schema is:

|from_TimelineColumn|to_TimelineColumn|dcount_new_values|dcount_retained_values|dcount_churn_values|retention_rate|churn_rate|dim1|..|dim_n|
|---|---|---|---|---|---|---|---|---|---|
|type: as of *TimelineColumn*|same|long|long|double|double|double|..|..|..|

* `from_TimelineColumn` - the cohort of new users. Metrics in this record refer to all users who were first seen in this period. The decision on *first seen* takes into account all previous periods in the analysis period.
* `to_TimelineColumn` - the period being compared to.
* `dcount_new_values` - the number of distinct users in `to_TimelineColumn` that weren't seen in *all* periods prior to and including `from_TimelineColumn`.
* `dcount_retained_values` - out of all new users, first seen in `from_TimelineColumn`, the number of distinct users that were seen in `to_TimelineCoumn`.
* `dcount_churn_values` - out of all new users, first seen in `from_TimelineColumn`, the number of distinct users that weren't seen in `to_TimelineCoumn`.
* `retention_rate` - the percent of `dcount_retained_values` out of the cohort (users first seen in `from_TimelineColumn`).
* `churn_rate` - the percent of `dcount_churn_values` out of the cohort (users first seen in `from_TimelineColumn`).

>[!NOTE]
>For definitions of `Retention Rate` and `Churn Rate` - refer to **Notes** section in [activity_metrics plugin](./activity-metrics-plugin.md) documentation.

## Examples

The following sample data set shows which users seen on which days. The table was generated based on a source `Users` table, as follows:

```kusto
Users | summarize tostring(make_set(user)) by bin(Timestamp, 1d) | order by Timestamp asc;
```

**Output**

|Timestamp|set_user|
|---|---|
|2019-11-01 00:00:00.0000000|[0,2,3,4]|
|2019-11-02 00:00:00.0000000|[0,1,3,4,5]|
|2019-11-03 00:00:00.0000000|[0,2,4,5]|
|2019-11-04 00:00:00.0000000|[0,1,2,3]|
|2019-11-05 00:00:00.0000000|[0,1,2,3,4]|

The output of the plugin for the original table is the following:

```kusto
let StartDate = datetime(2019-11-01 00:00:00);
let EndDate = datetime(2019-11-07 00:00:00);
Users 
| evaluate new_activity_metrics(user, Timestamp, StartDate, EndDate-1tick, 1d) 
| where from_Timestamp < datetime(2019-11-03 00:00:00.0000000)
```

**Output**

|R|from_Timestamp|to_Timestamp|dcount_new_values|dcount_retained_values|dcount_churn_values|retention_rate|churn_rate|
|---|---|---|---|---|---|---|---|
|1|2019-11-01 00:00:00.0000000|2019-11-01 00:00:00.0000000|4|4|0|1|0|
|2|2019-11-01 00:00:00.0000000|2019-11-02 00:00:00.0000000|2|3|1|0.75|0.25|
|3|2019-11-01 00:00:00.0000000|2019-11-03 00:00:00.0000000|1|3|1|0.75|0.25|
|4|2019-11-01 00:00:00.0000000|2019-11-04 00:00:00.0000000|1|3|1|0.75|0.25|
|5|2019-11-01 00:00:00.0000000|2019-11-05 00:00:00.0000000|1|4|0|1|0|
|6|2019-11-01 00:00:00.0000000|2019-11-06 00:00:00.0000000|0|0|4|0|1|
|7|2019-11-02 00:00:00.0000000|2019-11-02 00:00:00.0000000|2|2|0|1|0|
|8|2019-11-02 00:00:00.0000000|2019-11-03 00:00:00.0000000|0|1|1|0.5|0.5|
|9|2019-11-02 00:00:00.0000000|2019-11-04 00:00:00.0000000|0|1|1|0.5|0.5|
|10|2019-11-02 00:00:00.0000000|2019-11-05 00:00:00.0000000|0|1|1|0.5|0.5|
|11|2019-11-02 00:00:00.0000000|2019-11-06 00:00:00.0000000|0|0|2|0|1|

Following is an analysis of a few records from the output:

* Record `R=3`, `from_TimelineColumn` = `2019-11-01`,  `to_TimelineColumn` = `2019-11-03`:
  * The users considered for this record are all new users seen on 11/1. Since this is the first period, these are all users in that bin – [0,2,3,4]
  * `dcount_new_values` – the number of users on 11/3 who weren't seen on 11/1. This includes a single user – `5`.
  * `dcount_retained_values` – out of all new users on 11/1, how many were retained until 11/3? There are three values (`[0,2,4]`), while `count_churn_values` is one (user=`3`).
  * `retention_rate` = 0.75 – the three retained users out of the four new users who were first seen in 11/1.

* Record `R=9`, `from_TimelineColumn` = `2019-11-02`,  `to_TimelineColumn` = `2019-11-04`:
  * This record focuses on the new users who were first seen on 11/2 – users `1` and `5`.
  * `dcount_new_values` – the number of users on 11/4 who weren't seen through all periods `T0 .. from_Timestamp`. Meaning, users who are seen on 11/4 but who weren't seen on either 11/1 or 11/2 – there are no such users.
  * `dcount_retained_values` – out of all new users on 11/2 (`[1,5]`), how many were retained until 11/4? There's one such user (`[1]`), `while count_churn_values` is one (user `5`).
  * `retention_rate` is 0.5 – the single user that was retained on 11/4 out of the two new ones on 11/2.

### Weekly retention rate, and churn rate (single week)

The next query calculates a retention and churn rate for week-over-week window for `New Users` cohort (users that arrived on the first week).

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA21Ry27DIBC85yvmFmht2U4V5VDlVqk/0FtVWcisExIbLNg8LPXjC7arHFoOsBpmZ9ihKPBOlrxigldWux5asYJrcQnkoRo2V8OGwqojRh1YecY+kYhNT2JTVru83OZlJV9nCln9H+ElEaLFgfCmRrQ+Wi1y7OauwDSg0qtv0J0TkITYdc4ehEhN+dIhUUSefBB9JKbnC4lnVA+8TkOE/WQrFiWNJ2zLuHmZ4Q8W+zflXOWoIqOSiHr9Naf7EB1g9H5WTc/mcSDXiqQh0ZneMKpyWquiwIc6E5ztRvCR0BofGDeiMxp3dHFu0amIDMqrnpj8NM9VdZf0GZZu9RL+WMdrb5ogPtdGr7+yFGC2RJFN0WXY6V8kyQzenajhKeV6YrObTx+dLBtn6/TnGZrjxc/1D5PKiK4LAgAA" target="_blank">Run the query</a>

```kusto
// Generate random data of user activities
let _start = datetime(2017-05-01);
let _end = datetime(2017-05-31);
range Day from _start to _end step 1d
| extend d = tolong((Day - _start) / 1d)
| extend r = rand() + 1
| extend _users=range(tolong(d * 50 * r), tolong(d * 50 * r + 200 * r - 1), 1) 
| mv-expand id=_users to typeof(long) limit 1000000
// Take only the first week cohort (last parameter)
| evaluate new_activity_metrics(['id'], Day, _start, _end, 7d, _start)
| project from_Day, to_Day, retention_rate, churn_rate
```

**Output**

|from_Day|to_Day|retention_rate|churn_rate|
|---|---|---|---|
|2017-05-01 00:00:00.0000000|2017-05-01 00:00:00.0000000|1|0|
|2017-05-01 00:00:00.0000000|2017-05-08 00:00:00.0000000|0.544632768361582|0.455367231638418|
|2017-05-01 00:00:00.0000000|2017-05-15 00:00:00.0000000|0.031638418079096|0.968361581920904|
|2017-05-01 00:00:00.0000000|2017-05-22 00:00:00.0000000|0|1|
|2017-05-01 00:00:00.0000000|2017-05-29 00:00:00.0000000|0|1|

### Weekly retention rate, and churn rate (complete matrix)

The next query calculates retention and churn rate for week-over-week window for `New Users` cohort. If the previous example calculated the statistics for a single week - the following query produces an NxN table for each from/to combination.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22RwWrDMBBE7/mKuUVqbWynhByKb4Ve+gelGGFtUhVbMtImTaAf35VtyKH1QRbL25ndUVXhlTxFw4RovA0jrGGDcMQ5UYTp2V0cO0qbgRhdYhMZbYaI3UhqVzeHst6XdaOfF4S8/Q94yoBYnAgv5oZjFKtVjsPSlZgmNHbzA7pyLmQhDkPwJ6VyU7l2aFTC6TsYBczjK41HNPd6l5dI7WyrViWLB+xrOaIu8Kcm/bt6uZVohGg0RG+8lHSdxAHOtotqHptvE4Wjyhoagxsdo6nnb1NVeDOJMZloRmLJ0iUEIZisSOcZL2Y45+A9fXdr0LdO2Oj6pN63zm4/ihxWsa5dzDEVOMyrTzF8Uc9zkt1McVj+Uew8u+C7/K4F+s9zXO6/Mr0rj+8BAAA=" target="_blank">Run the query</a>

```kusto
// Generate random data of user activities
let _start = datetime(2017-05-01);
let _end = datetime(2017-05-31);
range Day from _start to _end step 1d
| extend d = tolong((Day - _start) / 1d)
| extend r = rand() + 1
| extend _users=range(tolong(d * 50 * r), tolong(d * 50 * r + 200 * r - 1), 1) 
| mv-expand id=_users to typeof(long) limit 1000000
// Last parameter is omitted - 
| evaluate new_activity_metrics(['id'], Day, _start, _end, 7d)
| project from_Day, to_Day, retention_rate, churn_rate
```

**Output**

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
the `New Users` cohort (all IDs that don't appear in this set are `New Users`). The
query examines the retention behavior of the `New Users` during the analysis period.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22SzW6DMBCE7zzFHO0WBKSKcki4VepjIBdvUjeAkdn8SXn42jgpjVIfMFq+nR2PyXN8UE9OMcGpXtsOWrGC3eIwkoNq2BwNGxqTlhh1a+3+UzV7VIEjNh2JRVGusmKRFaVcR2pk5fgZWf5BqNf/AW+/wGSjCqZ2hHd1wdZ5c7MBtlFkZBpQ6gR+XUFnDsWgzba1/U6I0JvNjRK5x+Uj7+IkLSReUT5+q0MQYzUZETdRjRcsC/9wMsVTzWssiviWofREKXHT7I4ZnQc/CUZXUTmchC8D2a0IOhKt6QyjLKa1TvJ8CuTu/x5M3K84fZGLAW3uwV8xOPtNDYdy6ietk4lO/JmOqj2Ey+7pVN8u91J3xM40ozA6jS1RKJ0STrHSc+XBhkzmUeF66qmZbdwd+fzY2L4Ov9cPqFoyT2oCAAA=" target="_blank">Run the query</a>

```kusto
// Generate random data of user activities
let _lookback = datetime(2017-02-01);
let _start = datetime(2017-05-01);
let _end = datetime(2017-05-31);
let _data = range Day from _lookback to _end step 1d
    | extend d = tolong((Day - _lookback) / 1d)
    | extend r = rand() + 1
    | extend _users=range(tolong(d * 50 * r), tolong(d * 50 * r + 200 * r - 1), 1) 
    | mv-expand id=_users to typeof(long) limit 1000000;
//
let lookback_data = _data | where Day < _start | project Day, id;
_data
| evaluate new_activity_metrics(id, Day, _start, _end, 7d, _start, lookback_data)
| project from_Day, to_Day, retention_rate
```

**Output**

|from_Day|to_Day|retention_rate|
|---|---|---|
|2017-05-01 00:00:00.0000000|2017-05-01 00:00:00.0000000|1|
|2017-05-01 00:00:00.0000000|2017-05-08 00:00:00.0000000|0.404081632653061|
|2017-05-01 00:00:00.0000000|2017-05-15 00:00:00.0000000|0.257142857142857|
|2017-05-01 00:00:00.0000000|2017-05-22 00:00:00.0000000|0.296326530612245|
|2017-05-01 00:00:00.0000000|2017-05-29 00:00:00.0000000|0.0587755102040816|
