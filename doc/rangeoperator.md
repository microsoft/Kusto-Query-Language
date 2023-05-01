---
title: range operator - Azure Data Explorer
description: Learn how to use the range operator to generate a single-column table of values.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# range operator

Generates a single-column table of values.

> [!NOTE]
> This operator doesn't take a tabular input.

## Syntax

`range` *columnName* `from` *start* `to` *stop* `step` *step*

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*columnName*|string|&check;| The name of the single column in the output table.|
|*start*|int, long, real, datetime, or timespan|&check;| The smallest value in the output.|
|*stop*|int, long, real, datetime, or timespan|&check;| The highest value being generated in the output or a bound on the highest value if *step* steps over this value.|
|*step*|int, long, real, datetime, or timespan|&check;| The difference between two consecutive values.|

> [!NOTE]
> The values can't reference the columns of any table. If you want to compute the range based on an input table, use the [range](rangefunction.md) function potentially with the [mv-expand](mvexpandoperator.md) operator.

## Returns

A table with a single column called *columnName*,
whose values are *start*, *start* `+` *step*, ... up to and until *stop*.

## Examples

The following example creates a table with entries for the current time stamp extended over the past seven days, once a day.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVfBJLC4JT03NVkgrys9VSEzP1zBP0VQoyVfIyy/X0FQoLkktUDBMAQDbz6iXLAAAAA==" target="_blank">Run the query</a>

```kusto
range LastWeek from ago(7d) to now() step 1d
```

**Output**

|LastWeek|
|---|
|2015-12-05 09:10:04.627|
|2015-12-06 09:10:04.627|
|...|
|2015-12-12 09:10:04.627|

The following example shows how to use the `range` operator with parameters, which are then extended and consumed as a table.  

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA21QQU7DMBC8+xV7TKSGJCKAIOICHLhUSBQe4OINjUjWlrNRGyh/Zx1a4UpYlizvzNgz0yED21dqdy9tj3ALieEboxlZrimoL1AgS6aQwXGelNdXRVaUstMUcigHUN+16uSt5bS0xJsVa8/y2hBO2/RhlpDdJmlaQ57/wraBGZiFK0Z3N4mkOrs4rzYza3TOekYD4dPBaRpmKpHQLquiKAILnu0W7u1IDE573SP79hPNwU1I1bUUknlN73g0+GhHD423/aljtjC7FOPoDp7UHnDHSCbWthR19ldgElHSBTxIYU/UhVjrlmJwURrBg+SAx76y/9iqjuPswXqDHtbTiVIPbwKx/kBp6Qf2xjgc3AEAAA==" target="_blank">Run the query</a>

```kusto
let toUnixTime = (dt:datetime) 
{ 
    (dt - datetime(1970-01-01)) / 1s 
};
let MyMonthStart = startofmonth(now()); //Start of month
let StepBy = 4.534h; //Supported timespans
let nn = 64000; // Row Count parametrized
let MyTimeline = range MyMonthHour from MyMonthStart to now() step StepBy
| extend MyMonthHourinUnixTime = toUnixTime(MyMonthHour), DateOnly = bin(MyMonthHour,1d), TimeOnly = MyMonthHour - bin(MyMonthHour,1d)
; MyTimeline | order by MyMonthHour asc | take nn
```

**Output**

| MyMonthHour | MyMonthHourinUnixTime | DateOnly     | TimeOnly                    |
|--------------|------------------------|---------------|------------------------------|
| 2023-02-01  | 00:00:00.0000000      | 1675209600   | 2023-02-01 00:00:00.0000000 |
| 2023-02-01  | 04:32:02.4000000      | 1675225922.4 | 2023-02-01 00:00:00.0000000 |
| 2023-02-01  | 09:04:04.8000000      | 1675242244.8 | 2023-02-01 00:00:00.0000000 |
| 2023-02-01  | 13:36:07.2000000      | 1675258567.2 | 2023-02-01 00:00:00.0000000 |
| ...         | ...                   | ...          | ...                         |

The following example creates a table with a single column called `Steps`
whose type is `long` and whose values are `1`, `4`, and `7`.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAytKzEtPVQguSS0oVkgrys9VMFQoyVewUCgGiigYAwBtHYQiHgAAAA==" target="_blank">Run the query</a>

```kusto
range Steps from 1 to 8 step 3
```

The following example shows how the `range` operator can be used to create a small, ad-hoc, dimension table that is then used to introduce zeros where the source data has no values.

```kusto
range TIMESTAMP from ago(4h) to now() step 1m
| join kind=fullouter
  (Traces
      | where TIMESTAMP > ago(4h)
      | summarize Count=count() by bin(TIMESTAMP, 1m)
  ) on TIMESTAMP
| project Count=iff(isnull(Count), 0, Count), TIMESTAMP
| render timechart  
```
