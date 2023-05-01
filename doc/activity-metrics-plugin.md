---
title: activity_metrics plugin - Azure Data Explorer
description: Learn how to use the activity_metrics plugin to calculate activity metrics using the current time window compared to the previous window.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/20/2022
---
# activity_metrics plugin

Calculates useful metrics that include distinct count values, distinct count of new values, retention rate, and churn rate. This plugin is different from [activity_counts_metrics plugin](activity-counts-metrics-plugin.md) in which every time window is compared to *all* previous time windows.

## Syntax

*T* `| evaluate` `activity_metrics(`*IdColumn*`,` *TimelineColumn*`,` [*Start*`,` *End*`,`] *Window* [`,` *dim1*`,` *dim2*`,` ...]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The input used to calculate activity metrics. |
| *IdCoumn* | string | &check; | The name of the column with ID values that represent user activity. |
| *TimelineColumn* | string | &check; | The name of the column that represents timeline. |
| *Start* | datetime | &check; | The analysis start period. |
| *End* | datetime | &check; | The analysis end period. |
| *Step* | decimal, datetime, or timespan | &check; | The analysis window period. This value may also be a string of `week`, `month`, or `year`, in which case all periods will be [startofweek](startofweekfunction.md), [startofmonth](startofmonthfunction.md), or [startofyear](startofyearfunction.md) respectively. |
| *dim1*, *dim2*, ... | dynamic |  | An array of the dimensions columns that slice the activity metrics calculation. |

## Returns

The plugin returns a table with the distinct count values, distinct count of new values, retention rate, and churn rate for each timeline period for each existing dimensions combination.

Output table schema is:

|*TimelineColumn*|dcount_values|dcount_newvalues|retention_rate|churn_rate|dim1|..|dim_n|
|---|---|---|---|---|--|--|--|--|--|--|
|type: as of *TimelineColumn*|long|long|double|double|..|..|..|

### Notes

***Retention Rate Definition***

`Retention Rate` over a period is calculated as:

> *number of customers returned during the period*
> / (divided by)
> *number customers at the beginning of the period*

where the `# of customers returned during the period` is defined as:

> *number of customers at end of period*
> \- (minus)
> *number of new customers acquired during the period*

`Retention Rate` can vary from 0.0 to 1.0
A higher score means a larger number of returning users.

***Churn Rate Definition***

`Churn Rate` over a period is calculated as:

> *number of customers lost in the period*
> / (divided by)
> *number of customers at the beginning of the period*

where the `# of customer lost in the period` is defined as:

> *number of customers at the beginning of the period*
> \- (minus)
> *number of customers at the end of the period*

`Churn Rate` can vary from 0.0 to 1.0
The higher score means the larger number of users are NOT returning to the service.

***Churn vs. Retention Rate***
The churn vs. retention Rate is derived from the definition of `Churn Rate` and `Retention Rate`. The following calculation is always true:

> [`Retention Rate`] = 100.0% - [`Churn Rate`]

## Examples

### Weekly retention rate and churn rate

The next query calculates retention and churn rate for week-over-week window.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2VRy26EIBTdm/gPdzc4g1FsJrNoXPcjmsYQuc7QqBi4mjHpxxeQxTRlQeDmPDiHqoIPnNFKQrByVmYCJUmCGWB1aEH2pDdNGl2ejUjQOZKWoA0oJD0ha2pxK2tR1k3xnjA4q/+Ia/kmAsK73BE6JXcYrLdLimQOHjjCBYTKsx/AJ4VRECMzmvnOWOSViVRUQhUvQOuBIQQrLuJl3IUkro3GLAmp87U+24LDn/ulqf1eCj8XUXjaSnwuXhK0ag+d8FLaFzQDC8wCRj1pAlHHlWdVFa03Oa6h1FTg3k1IVveOfZ60On3x2ABPQXjMzuF2xFms+caeEsSiT0HazF34JQ79Y7XHOWCtJ/p/CkX3Dy/1C38j6lLRAQAA" target="_blank">Run the query</a>

```kusto
// Generate random data of user activities
let _start = datetime(2017-01-02);
let _end = datetime(2017-05-31);
range _day from _start to _end  step 1d
| extend d = tolong((_day - _start)/1d)
| extend r = rand()+1
| extend _users=range(tolong(d*50*r), tolong(d*50*r+200*r-1), 1)
| mv-expand id=_users to typeof(long) take 1000000
//
| evaluate activity_metrics(['id'], _day, _start, _end, 7d)
| project _day, retention_rate, churn_rate
| render timechart
```

**Output**

|_day|retention_rate|churn_rate|
|---|---|---|
|2017-01-02 00:00:00.0000000|NaN|NaN|
|2017-01-09 00:00:00.0000000|0.179910044977511|0.820089955022489|
|2017-01-16 00:00:00.0000000|0.744374437443744|0.255625562556256|
|2017-01-23 00:00:00.0000000|0.612096774193548|0.387903225806452|
|2017-01-30 00:00:00.0000000|0.681141439205955|0.318858560794045|
|2017-02-06 00:00:00.0000000|0.278145695364238|0.721854304635762|
|2017-02-13 00:00:00.0000000|0.223172628304821|0.776827371695179|
|2017-02-20 00:00:00.0000000|0.38|0.62|
|2017-02-27 00:00:00.0000000|0.295519001701645|0.704480998298355|
|2017-03-06 00:00:00.0000000|0.280387770320656|0.719612229679344|
|2017-03-13 00:00:00.0000000|0.360628154795289|0.639371845204711|
|2017-03-20 00:00:00.0000000|0.288008028098344|0.711991971901656|
|2017-03-27 00:00:00.0000000|0.306134969325153|0.693865030674847|
|2017-04-03 00:00:00.0000000|0.356866537717602|0.643133462282398|
|2017-04-10 00:00:00.0000000|0.495098039215686|0.504901960784314|
|2017-04-17 00:00:00.0000000|0.198296836982968|0.801703163017032|
|2017-04-24 00:00:00.0000000|0.0618811881188119|0.938118811881188|
|2017-05-01 00:00:00.0000000|0.204657727593507|0.795342272406493|
|2017-05-08 00:00:00.0000000|0.517391304347826|0.482608695652174|
|2017-05-15 00:00:00.0000000|0.143667296786389|0.856332703213611|
|2017-05-22 00:00:00.0000000|0.199122325836533|0.800877674163467|
|2017-05-29 00:00:00.0000000|0.063468992248062|0.936531007751938|

:::image type="content" source="images/activity-metrics-plugin/activity-metrics-churn-and-retention.png" border="false" alt-text="Table showing the calculated retention and churn rates per seven days as specified in the query.":::

### Distinct values and distinct 'new' values

The next query calculates distinct values and 'new' values (IDs that didn't appear in previous time window) for week-over-week window.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2VRTWuEMBC9C/6HuW3cVTSWZQ/Fc39EKRLM7DaLJpKMdoX++CYxlC3NITCP95E3qWt4Q41WEIIVWpoJpCAB5gqLQwtiILUqUujybESC3pGwBF1gIakJWdvwS9XwqmmL18RBLf8zztULDwyfckPopdjgan1cciSz68ARzsBlnn0DPihAwYzMaPSNsairkqiouSyeiNYTQwlWnPgT3IcmrovBLBnJ47k52qKEP/OpbfxdcY/zaDytFT5mbwlKdrtPeCltM5orC8oCRjUpAt7Ek2d1HaNXMS5hqWmBWz8hWTU49n5Q8vBRxg2UqUgZu5dw2evM1txxoESRg1k09cEQ3e+o8WtHgsB6tf+ssO3h0/v9ADoJzJ7WAQAA" target="_blank">Run the query</a>

```kusto
// Generate random data of user activities
let _start = datetime(2017-01-02);
let _end = datetime(2017-05-31);
range _day from _start to _end  step 1d
| extend d = tolong((_day - _start)/1d)
| extend r = rand()+1
| extend _users=range(tolong(d*50*r), tolong(d*50*r+200*r-1), 1)
| mv-expand id=_users to typeof(long) take 1000000
//
| evaluate activity_metrics(['id'], _day, _start, _end, 7d)
| project _day, dcount_values, dcount_newvalues
| render timechart
```

**Output**

| _day | dcount_values | dcount_newvalues |
|--|--|--|
| 2017-01-02 00:00:00.0000000 | 630 | 630 |
| 2017-01-09 00:00:00.0000000 | 738 | 575 |
| 2017-01-16 00:00:00.0000000 | 1187 | 841 |
| 2017-01-23 00:00:00.0000000 | 1092 | 465 |
| 2017-01-30 00:00:00.0000000 | 1261 | 647 |
| 2017-02-06 00:00:00.0000000 | 1744 | 1043 |
| 2017-02-13 00:00:00.0000000 | 1563 | 432 |
| 2017-02-20 00:00:00.0000000 | 1406 | 818 |
| 2017-02-27 00:00:00.0000000 | 1956 | 1429 |
| 2017-03-06 00:00:00.0000000 | 1593 | 848 |
| 2017-03-13 00:00:00.0000000 | 1801 | 1423 |
| 2017-03-20 00:00:00.0000000 | 1710 | 1017 |
| 2017-03-27 00:00:00.0000000 | 1796 | 1516 |
| 2017-04-03 00:00:00.0000000 | 1381 | 1008 |
| 2017-04-10 00:00:00.0000000 | 1756 | 1162 |
| 2017-04-17 00:00:00.0000000 | 1831 | 1409 |
| 2017-04-24 00:00:00.0000000 | 1823 | 1164 |
| 2017-05-01 00:00:00.0000000 | 1811 | 1353 |
| 2017-05-08 00:00:00.0000000 | 1691 | 1246 |
| 2017-05-15 00:00:00.0000000 | 1812 | 1608 |
| 2017-05-22 00:00:00.0000000 | 1740 | 1017 |
| 2017-05-29 00:00:00.0000000 | 960 | 756 |

:::image type="content" source="images/activity-metrics-plugin/activity-metrics-dcount-and-dcount-newvalues.png" border="false" alt-text="Table showing the count of distinct values (dcount_values) and of new distinct values (dcount_newvalues) that didn't appear in previous time window as specified in the query.":::
