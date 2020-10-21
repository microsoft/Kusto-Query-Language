---
title: range operator - Azure Data Explorer
description: This article describes range operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# range operator

Generates a single-column table of values.

Notice that it doesn't have a pipeline input. 

## Syntax

`range` *columnName* `from` *start* `to` *stop* `step` *step*

## Arguments

* *columnName*: The name of the single column in the output table.
* *start*: The smallest value in the output.
* *stop*: The highest value being generated in the output (or a bound
on the highest value, if *step* steps over this value).
* *step*: The difference between two consecutive values. 

The arguments must be numeric, date or timespan values. They can't reference the columns of any table. (If you want to compute the range based on an input table, use the range function, maybe with the mv-expand operator.) 

## Returns

A table with a single column called *columnName*,
whose values are *start*, *start* `+` *step*, ... up to and until *stop*.

## Example  

A table of midnight at the past seven days. The bin (floor) function reduces each time to the start of the day.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
range LastWeek from ago(7d) to now() step 1d
```

|LastWeek|
|---|
|2015-12-05 09:10:04.627|
|2015-12-06 09:10:04.627|
|...|
|2015-12-12 09:10:04.627|


A table with a single column called `Steps`
whose type is `long` and whose values are `1`, `4`, and `7`.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
range Steps from 1 to 8 step 3
```

The next example shows how the `range` operator can be used to create
a small, ad-hoc, dimension table which is then used to introduce zeros where the source data has no values.

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
