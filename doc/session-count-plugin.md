---
title: session_count plugin - Azure Data Explorer | Microsoft Docs
description: This article describes session_count plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# session_count plugin

Calculates sessions count based on ID column over a timeline.

```kusto
T | evaluate session_count(id, datetime_column, startofday(ago(30d)), startofday(now()), 1min, 30min, dim1, dim2, dim3)
```

**Syntax**

*T* `| evaluate` `session_count(`*IdColumn*`,` *TimelineColumn*`,` *Start*`,` *End*`,` *Bin*`,` *LookBackWindow* [`,` *dim1*`,` *dim2*`,` ...]`)`

**Arguments**

* *T*: The input tabular expression.
* *IdColumn*: The name of the column with ID values that represent user activity. 
* *TimelineColumn*: The name of the column that represent timeline.
* *Start*: Scalar with value of the analysis start period.
* *End*: Scalar with value of the analysis end period.
* *Bin*:  scalar constant value of session analysis step period.
* *LookBackWindow*: scalar constant value representing session lookback period. If the id from `IdColumn` appears in a time window within `LookBackWindow` - the session is considered to be an existing, if it does not - the session is considered to be new.
* *dim1*, *dim2*, ...: (optional) list of the dimensions columns that slice the session count calculation.

**Returns**

Returns a table that has the session count values for each timeline period and for each existing dimensions combination.

Output table schema is:

|*TimelineColumn*|dim1|..|dim_n|count_sessions|
|---|---|---|---|---|--|--|--|--|--|--|
|type: as of *TimelineColumn*|..|..|..|long|


**Examples**


For the sake of the example, we will make data deterministic - a table with two columns:
- Timeline: a running number from 1 to 10,000
- Id: Id of the user from 1 to 50

`Id` appear at the specific `Timeline` slot if it is a divider of `Timeline` (Timeline % Id == 0).

This means that event with `Id==1` will appear at any `Timeline` slot, event with `Id==2` at every second `Timeline` slot, and so on.

Here are few 20 lines of the data:

```kusto
let _data = range Timeline from 1 to 10000 step 1
| extend __key = 1
| join kind=inner (range Id from 1 to 50 step 1 | extend __key=1) on __key
| where Timeline % Id == 0
| project Timeline, Id;
// Look on few lines of the data
_data
| order by Timeline asc, Id asc
| limit 20
```

|Timeline|Id|
|---|---|
|1|1|
|2|1|
|2|2|
|3|1|
|3|3|
|4|1|
|4|2|
|4|4|
|5|1|
|5|5|
|6|1|
|6|2|
|6|3|
|6|6|
|7|1|
|7|7|
|8|1|
|8|2|
|8|4|
|8|8|

Let's define a session in next terms: session considered to be active as long as user (`Id`) appears at least once at a timeframe of 100 time slots, while session look-back window is 41 time slots.

The next query shows the count of active sessions according to the definition above.

```kusto
let _data = range Timeline from 1 to 9999 step 1
| extend __key = 1
| join kind=inner (range Id from 1 to 50 step 1 | extend __key=1) on __key
| where Timeline % Id == 0
| project Timeline, Id;
// End of data definition
_data
| evaluate session_count(Id, Timeline, 1, 10000, 100, 41)
| render linechart 
```

![alt text](./images/queries/example-session-count.png "example-session-count")