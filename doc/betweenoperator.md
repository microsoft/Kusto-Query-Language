---
title: between operator - Azure Data Explorer
description: This article describes between operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
ms.localizationpriority: high
---
# between operator

Matches the input that is inside the inclusive range.

```kusto
Table1 | where Num1 between (1 .. 10)
Table1 | where Time between (datetime(2017-01-01) .. datetime(2017-01-01))
```

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

**Filtering numeric values using 'between' operator**  

<!-- csl: https://help.kusto.windows.net:443/Samples -->
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

**Filtering datetime using 'between' operator**  

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents
| where StartTime between (datetime(2007-07-27) .. datetime(2007-07-30))
| count 
```

|Count|
|---|
|476|

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
StormEvents
| where StartTime between (datetime(2007-07-27) .. 3d)
| count 
```

|Count|
|---|
|476|
