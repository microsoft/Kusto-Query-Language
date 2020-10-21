---
title: has_any operator - Azure Data Explorer
description: This article describes has_any operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 08/11/2019
---
# has_any operator

`has_any` operator filters based on the provided set of values.

```kusto
Table1 | where col has_any ('value1', 'value2')
```

## Syntax

*T* `|` `where` *col* `has_any` `(`*list of scalar expressions*`)`   
*T* `|` `where` *col* `has_any` `(`*tabular expression*`)`   
 
## Arguments

* *T* - Tabular input whose records are to be filtered.
* *col* - Column to filter.
* *list of expressions* - Comma separated list of tabular, scalar, or literal expressions  
* *tabular expression* - Tabular expression that has a set of values (if expression has multiple columns, the first column is used)

## Returns

Rows in *T* for which the predicate is `true`

**Notes**

* The expression list can produce up to `10,000` values.    
* For tabular expressions, the first column of the result set is selected.   

**Examples:**  

**A simple usage of `has_any` operator:**  

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents 
| where State has_any ("CAROLINA", "DAKOTA", "NEW") 
| summarize count() by State
```

|State|count_|
|---|---|
|NEW YORK|1750|
|NORTH CAROLINA|1721|
|SOUTH DAKOTA|1567|
|NEW JERSEY|1044|
|SOUTH CAROLINA|915|
|NORTH DAKOTA|905|
|NEW MEXICO|527|
|NEW HAMPSHIRE|394|


**Using dynamic array:**

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let states = dynamic(['south', 'north']);
StormEvents 
| where State has_any (states)
| summarize count() by State
```

|State|count_|
|---|---|
|NORTH CAROLINA|1721|
|SOUTH DAKOTA|1567|
|SOUTH CAROLINA|915|
|NORTH DAKOTA|905|
|ATLANTIC SOUTH|193|
|ATLANTIC NORTH|188|
