---
title: The between operator - Azure Data Explorer
description: Learn how to use the between operator to return a record set of values in an inclusive range for which the predicate evaluates to true. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/20/2022
---
# between operator

Filters a record set for data matching the values in an inclusive range.

`between` can operate on any numeric, datetime, or timespan expression.

## Syntax

*T* `|` `where` *expr* `between` `(`*leftRange*` .. `*rightRange*`)`

If *expr* expression is datetime - another syntactic sugar syntax is provided:

*T* `|` `where` *expr* `between` `(`*leftRangeDateTime*` .. `*rightRangeTimespan*`)`

## Arguments

* *T* - The tabular input whose records are to be matched.
* *expr* - the expression to filter.
* *leftRange* - expression of the left range (inclusive).
* *rightRange* - expression of the right range (inclusive).

## Returns

Rows in *T* for which the predicate of (*expr* >= *leftRange* and *expr* <= *rightRange*) evaluates to `true`.

## Examples

### Filter numeric values

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
range x from 1 to 100 step 1
| where x between (50 .. 55)
```

|x|
|---|
|50|
|51|
|52|
|53|
|54|
|55|

### Filter datetime

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| where StartTime between (datetime(2007-07-27) .. datetime(2007-07-30))
| count
```

|Count|
|---|
|476|

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
| where StartTime between (datetime(2007-07-27) .. 3d)
| count
```

|Count|
|---|
|476|
