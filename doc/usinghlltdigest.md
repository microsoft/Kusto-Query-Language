---
title: Partitioning and composing intermediate results of aggregations - Azure Data Explorer | Microsoft Docs
description: This article describes Partitioning and composing intermediate results of aggregations in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 02/20/2019
---
# Partitioning and composing intermediate results of aggregations

Suppose that you want to calculate the count of distinct users over the last seven days every day. One way to do it would be to run "summarize dcount(user)" once a day with a span filtered to the last seven days. This is inefficient, because each time the calculation is run there's a six-days overlap with the previous calculation. Another option is to calculate some aggregate for each day, and then combine these aggregates in an efficient way. This option requires you to "remember" the last six results, but is much more efficient.

Partitioning queries like that would be easy for simple aggregates, such as count() and sum(). It is possible, however, to also perform them for more complex aggregates such as dcount() and percentiles(). This topic explains how Kusto supports such calculations.

Here are few examples which shows how to use hll/tdigest and that using these in some scenarios may be much more performant than other ways:

**Example**


Assuming that we have the table PageViewsHllTDigest which has the hll values over Pages viewed in each hour.
now we are interested in getting these values but binned to `12h` so we may merge the hll values using hll_merge() aggregate function by timestamp binned to `12h` and then call the function dcount_hll to get the final dcount value:

```
PageViewsHllTDigest
| summarize merged_hll = hll_merge(hllPage) by bin(Timestamp, 12h)
| project Timestamp , dcount_hll(merged_hll)


```

|Timestamp|dcount_hll_merged_hll|
|---|---|
|2016-05-01 12:00:00.0000000|20056275|
|2016-05-02 00:00:00.0000000|38797623|
|2016-05-02 12:00:00.0000000|39316056|
|2016-05-03 00:00:00.0000000|13685621|


Or even for binned timestamp for `1d` :

```
PageViewsHllTDigest
| summarize merged_hll = hll_merge(hllPage) by bin(Timestamp, 1d)
| project Timestamp , dcount_hll(merged_hll)


```

|Timestamp|dcount_hll_merged_hll|
|---|---|
|2016-05-01 00:00:00.0000000|20056275|
|2016-05-02 00:00:00.0000000|64135183|
|2016-05-03 00:00:00.0000000|13685621|


 The samething may be done over the values of tdigest which represents the BytesDelivered in each hour:

```
PageViewsHllTDigest
| summarize merged_tdigests = merge_tdigests(tdigestBytesDel) by bin(Timestamp, 12h)
| project Timestamp , percentile_tdigest(merged_tdigests, 95, typeof(long))


```

|Timestamp|percentile_tdigest_merged_tdigests|
|---|---|
|2016-05-01 12:00:00.0000000|170200|
|2016-05-02 00:00:00.0000000|152975|
|2016-05-02 12:00:00.0000000|181315|
|2016-05-03 00:00:00.0000000|146817|
 
**Example**


When having a too large datasets where we need to run a periodic queries over this dataset but running the regular queries to calculate [`percentile()`](percentiles-aggfunction.md) or [`dcount()`](dcount-aggfunction.md) over these big dataset hits kusto limits.
To solve this problem, Newly added data may be added to a temp table as hll or tdigest values using [`hll()`](hll-aggfunction.md) when the required operation is dcount or [`tdigest()`](tdigest-aggfunction.md) when the required operation is percentile using [`set/append`](../management/data-ingestion/index.md) or [`update policy`](../concepts/updatepolicy.md), In this case, the intermediate results of dcount or tdigest are saved into another dataset which should be smaller than the target big one.
Then when we need to get the final results of these values, the queries may use hll/tdigest mergers: [`hll-merge()`](hll-merge-aggfunction.md)/[`merge_tdigests()`](merge-tdigests-aggfunction.md), Then, After getting the merged values, [`percentile_tdigest()`](percentile-tdigestfunction.md) / [`dcount_hll()`](dcount-hllfunction.md) may be invoked on these merged values to get the final result of dcount or percentiles.

Assuming that we have a table PageViews where each day we ingest data, each day we want to calculate the distinct count of pages viewed per minuite later than date = datetime(2016-05-01 18:00:00.0000000).

Running the following query :


```
PageViews	
| where Timestamp > datetime(2016-05-01 18:00:00.0000000)
| summarize percentile(BytesDelivered, 90), dcount(Page,2) by bin(Timestamp, 1d)



```

|Timestamp|percentile_BytesDelivered_90|dcount_Page|
|---|---|---|
|2016-05-01 00:00:00.0000000|83634|20056275|
|2016-05-02 00:00:00.0000000|82770|64135183|
|2016-05-03 00:00:00.0000000|72920|13685621|



This query will aggregate all the values each time we run this query (e.g, if we are interested in running it many times a day).

if we save the hll and tdigest values (which are the intermediate results of dcount and percentile) into a temp table PageViewsHllTDigest using update policy or set/append commands so we may only merge the values and then use dcount_hll/percentile_tdigest using the following query :


```
PageViewsHllTDigest
| summarize  percentile_tdigest(merge_tdigests(tdigestBytesDel), 90), dcount_hll(hll_merge(hllPage)) by bin(Timestamp, 1d)

```

|Timestamp|percentile_tdigest_merge_tdigests_tdigestBytesDel|dcount_hll_hll_merge_hllPage|
|---|---|---|
|2016-05-01 00:00:00.0000000|84224|20056275|
|2016-05-02 00:00:00.0000000|83486|64135183|
|2016-05-03 00:00:00.0000000|72247|13685621|

This should be more performant and the query runs over a smaller table (in this example, The first query runs over ~215M records while the second one runs over 32 records only).

**Example**

The Retention Query.
Assuming that we have a table which summarizes when each wikipedia page was viewed (Sample size is 10M), And we are interested in finding foreach date1 date2
what percent of pages reviewed in both date1 and date2 relatively to the pages viewed on date1 (date1 < date2).
  
The trivial way which uses join and summarize operators :


```
// get the total pages viewed in each day
let totalPagesPerDay = PageViewsSample
| summarize by Page, Day = startofday(Timestamp)
| summarize count() by Day;
// joining the table to itself to get a grid where 
// each row shows foreach page1 , in which two dates
// it was viewed.
// then counting the pages between each two dates to
// get how many pages viewed between date1 and date2.
PageViewsSample
| summarize by Page, Day1 = startofday(Timestamp)
| join kind = inner
(
    PageViewsSample
    | summarize by Page, Day2 = startofday(Timestamp)
)
on Page
| where Day2 > Day1
| summarize count() by Day1, Day2
| join kind = inner
    totalPagesPerDay
on $left.Day1 == $right.Day
| project Day1, Day2, Percentage = count_*100.0/count_1


```

|Day1|Day2|Percentage|
|---|---|---|
|2016-05-01 00:00:00.0000000|2016-05-02 00:00:00.0000000|34.0645725975255|
|2016-05-01 00:00:00.0000000|2016-05-03 00:00:00.0000000|16.618368960101|
|2016-05-02 00:00:00.0000000|2016-05-03 00:00:00.0000000|14.6291376489636|

 
The query above took ~18 seconds.

When using the functions of [`hll()`](hll-aggfunction.md), [`hll_merge()`](hll-merge-aggfunction.md) and [`dcount_hll()`](dcount-hllfunction.md), the equivalent query for above will end after ~1.3 seconds and shows that hll functions speeds up 
the query above by ~14 times:

```
let Stats=PageViewsSample | summarize pagehll=hll(Page, 2) by day=startofday(Timestamp); // saving the hll values (intermediate results of the dcount values)
let day0=toscalar(Stats | summarize min(day)); // finding the min date over all dates.
let dayn=toscalar(Stats | summarize max(day)); // finding the max date over all dates.
let daycount=tolong((dayn-day0)/1d); // finding the range between max and min
Stats
| project idx=tolong((day-day0)/1d), day, pagehll
| mv-expand pidx=range(0, daycount) to typeof(long)
// extending column to get the dcount value from hll'ed values for each date (same as totalPagesPerDay from above query)
| extend key1=iff(idx < pidx, idx, pidx), key2=iff(idx < pidx, pidx, idx), pages=dcount_hll(pagehll)
// foreach two dates , merge the hll'ed values to get the total dcount over each two dates, 
// this will help us to get the pages viewed in both date1 and date2 (see describtion below about the intersection_size)
| summarize (day1, pages1)=arg_min(day, pages), (day2, pages2)=arg_max(day, pages), union_size=dcount_hll(hll_merge(pagehll)) by key1, key2
| where day2 > day1
// to get pages viewed in date1 and also date2, we look at the merged dcount of date1 and date2, subtract it from pages of date1 + pages on date2.
| project pages1, day1,day2, intersection_size=(pages1 + pages2 - union_size)
| project day1, day2, Percentage = intersection_size*100.0 / pages1



```

|day1|day2|Percentage|
|---|---|---|
|2016-05-01 00:00:00.0000000|2016-05-02 00:00:00.0000000|33.2298494510578|
|2016-05-01 00:00:00.0000000|2016-05-03 00:00:00.0000000|16.9773830213667|
|2016-05-02 00:00:00.0000000|2016-05-03 00:00:00.0000000|14.5160020350006|

Note: the results of the queries are not 100% accurate due to the error of the hll functions.(see [`dcount()`](dcount-aggfunction.md) for further information about the errors).