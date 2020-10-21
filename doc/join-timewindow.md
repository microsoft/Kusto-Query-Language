---
title: Joining within time window - Azure Data Explorer
description: This article describes Joining within time window in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# Time window join

It's often useful to join between two large data sets on some high-cardinality key, such as an operation ID or a session ID, and further limit the right-hand-side ($right) records that need to match up with each left-hand-side ($left) record by adding a restriction on the "time-distance" between datetime columns on the left and on the right.

The function is useful in a join, like in the following scenario:
* Join between two large data sets according to some high-cardinality key, such as an operation ID or a session ID.
* Limit the right-hand-side ($right) records that need to match up with each left-hand-side ($left) record, by adding a restriction on the "time-distance" between datetime columns on the left and on the right.

The above operation differs from the usual Kusto join operation, since for the *`equi-join`* part of matching the high-cardinality key between the left and right data sets, the system can also apply a distance function and use it to considerably speed up the join.

> [!NOTE]
> A distance function doesn't behave like equality (that is, when both dist(x,y) and dist(y,z) are true it doesn't follow that dist(x,z) is also true.) Internally, we sometimes refer to this as "diagonal join".

For example, if you want to identify event sequences within a relatively small time window, assume that you have a table `T` with the following schema:

* `SessionId`: A column of type `string` with correlation IDs.
* `EventType`: A column of type `string` that identifies the event type of the record.
* `Timestamp`: A column of type `datetime` indicates when the event described by the record happened.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let T = datatable(SessionId:string, EventType:string, Timestamp:datetime)
[
    '0', 'A', datetime(2017-10-01 00:00:00),
    '0', 'B', datetime(2017-10-01 00:01:00),
    '1', 'B', datetime(2017-10-01 00:02:00),
    '1', 'A', datetime(2017-10-01 00:03:00),
    '3', 'A', datetime(2017-10-01 00:04:00),
    '3', 'B', datetime(2017-10-01 00:10:00),
];
T
```

|SessionId|EventType|Timestamp|
|---|---|---|
|0|A|2017-10-01 00:00:00.0000000|
|0|B|2017-10-01 00:01:00.0000000|
|1|B|2017-10-01 00:02:00.0000000|
|1|A|2017-10-01 00:03:00.0000000|
|3|A|2017-10-01 00:04:00.0000000|
|3|B|2017-10-01 00:10:00.0000000|


**Problem statement**

Our query should answer the following question:

   Find all the session IDs in which event type `A` was followed by an
   event type `B` within a `1min` time window.

> [!NOTE]
> In the sample data above, the only such session ID is `0`.

Semantically, the following query answers this question, albeit inefficiently.

```kusto
T 
| where EventType == 'A'
| project SessionId, Start=Timestamp
| join kind=inner
    (
    T 
    | where EventType == 'B'
    | project SessionId, End=Timestamp
    ) on SessionId
| where (End - Start) between (0min .. 1min)
| project SessionId, Start, End 

```

|SessionId|Start|End|
|---|---|---|
|0|2017-10-01 00:00:00.0000000|2017-10-01 00:01:00.0000000|

To optimize this query, we can rewrite it as described below
so that the time window is expressed as a join key.

**Rewrite the query to account for the time window**

Rewrite the query so that the `datetime` values are "discretized" into buckets whose size is half the size of the time window. Use Kusto's *`equi-join`* to compare those bucket IDs.

```kusto
let lookupWindow = 1min;
let lookupBin = lookupWindow / 2.0; // lookup bin = equal to 1/2 of the lookup window
T 
| where EventType == 'A'
| project SessionId, Start=Timestamp,
          // TimeKey on the left side of the join is mapped to a discrete time axis for the join purpose
          TimeKey = bin(Timestamp, lookupBin)
| join kind=inner
    (
    T 
    | where EventType == 'B'
    | project SessionId, End=Timestamp,
              // TimeKey on the right side of the join - emulates event 'B' appearing several times
              // as if it was 'replicated'
              TimeKey = range(bin(Timestamp-lookupWindow, lookupBin),
                              bin(Timestamp, lookupBin),
                              lookupBin)
    // 'mv-expand' translates the TimeKey array range into a column
    | mv-expand TimeKey to typeof(datetime)
    ) on SessionId, TimeKey 
| where (End - Start) between (0min .. lookupWindow)
| project SessionId, Start, End 
```

|SessionId|Start|End|
|---|---|---|
|0|2017-10-01 00:00:00.0000000|2017-10-01 00:01:00.0000000|

**Runnable query reference (with table inlined)**

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let T = datatable(SessionId:string, EventType:string, Timestamp:datetime)
[
    '0', 'A', datetime(2017-10-01 00:00:00),
    '0', 'B', datetime(2017-10-01 00:01:00),
    '1', 'B', datetime(2017-10-01 00:02:00),
    '1', 'A', datetime(2017-10-01 00:03:00),
    '3', 'A', datetime(2017-10-01 00:04:00),
    '3', 'B', datetime(2017-10-01 00:10:00),
];
let lookupWindow = 1min;
let lookupBin = lookupWindow / 2.0;
T 
| where EventType == 'A'
| project SessionId, Start=Timestamp, TimeKey = bin(Timestamp, lookupBin)
| join kind=inner
    (
    T 
    | where EventType == 'B'
    | project SessionId, End=Timestamp,
              TimeKey = range(bin(Timestamp-lookupWindow, lookupBin),
                              bin(Timestamp, lookupBin),
                              lookupBin)
    | mv-expand TimeKey to typeof(datetime)
    ) on SessionId, TimeKey 
| where (End - Start) between (0min .. lookupWindow)
| project SessionId, Start, End 
```

|SessionId|Start|End|
|---|---|---|
|0|2017-10-01 00:00:00.0000000|2017-10-01 00:01:00.0000000|


**50M data query**

The next query emulates a data set of 50M records and ~10M IDs and runs the query with the technique described above.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
let T = range x from 1 to 50000000 step 1
| extend SessionId = rand(10000000), EventType = rand(3), Time=datetime(2017-01-01)+(x * 10ms)
| extend EventType = case(EventType <= 1, "A",
                          EventType <= 2, "B",
                          "C");
let lookupWindow = 1min;
let lookupBin = lookupWindow / 2.0;
T 
| where EventType == 'A'
| project SessionId, Start=Time, TimeKey = bin(Time, lookupBin)
| join kind=inner
    (
    T 
    | where EventType == 'B'
    | project SessionId, End=Time, 
              TimeKey = range(bin(Time-lookupWindow, lookupBin), 
                              bin(Time, lookupBin),
                              lookupBin)
    | mv-expand TimeKey to typeof(datetime)
    ) on SessionId, TimeKey 
| where (End - Start) between (0min .. lookupWindow)
| project SessionId, Start, End 
| count 
```

|Count|
|---|
|1276|
