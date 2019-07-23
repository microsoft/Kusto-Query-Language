---
title: in and notin operators - Azure Data Explorer | Microsoft Docs
description: This article describes in and notin operators in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 03/18/2019
---
# in and !in operators

Filters a recordset based on the provided set of values.

```kusto
Table1 | where col in ('value1', 'value2')
```

**Syntax**

*Case sensitive syntax:*

*T* `|` `where` *col* `in` `(`*list of scalar expressions*`)`   
*T* `|` `where` *col* `in` `(`*tabular expression*`)`   
 
*T* `|` `where` *col* `!in` `(`*list of scalar expressions*`)`  
*T* `|` `where` *col* `!in` `(`*tabular expression*`)`   

*Case insensitive syntax:*

*T* `|` `where` *col* `in~` `(`*list of scalar expressions*`)`   
*T* `|` `where` *col* `in~` `(`*tabular expression*`)`   
 
*T* `|` `where` *col* `!in~` `(`*list of scalar expressions*`)`  
*T* `|` `where` *col* `!in~` `(`*tabular expression*`)`   

**Arguments**

* *T* - The tabular input whose records are to be filtered.
* *col* - the column to filter.
* *list of expressions* - a comma separated list of tabular, scalar or literal expressions  
* *tabular expression* - a tabular expression that has a set of values (in a case expression has multiple columns, the first column is used)

**Returns**

Rows in *T* for which the predicate is `true`

**Notes**

* The expression list can produce up to `1,000,000` values    
* Nested arrays are flattened into a single list of values, for example `x in (dynamic([1,[2,3]]))` turns into `x in (1,2,3)` 
* In case of tabular expressions, the first column of the result set is selected   
* Adding '~' to operator makes values' search case insensitive: `x in~ (expression)` or `x !in~ (expression)`.

**Examples:**  

**A simple usage of 'in' operator:**  

```kusto
StormEvents 
| where State in ("FLORIDA", "GEORGIA", "NEW YORK") 
| count
```

|Count|
|---|
|4775|  


**A simple usage of 'in~' operator:**  

```kusto
StormEvents 
| where State in~ ("Florida", "Georgia", "New York") 
| count
```

|Count|
|---|
|4775|  

**A simple usage of '!in' operator:**  

```kusto
StormEvents 
| where State !in ("FLORIDA", "GEORGIA", "NEW YORK") 
| count
```

|Count|
|---|
|54291|  


**Using dynamic array:**
```kusto
let states = dynamic(['FLORIDA', 'ATLANTIC SOUTH', 'GEORGIA']);
StormEvents 
| where State in (states)
| count
```

|Count|
|---|
|3218|


**A subquery example:**  

```kusto
// Using subquery
let Top_5_States = 
StormEvents
| summarize count() by State
| top 5 by count_; 
StormEvents 
| where State in (Top_5_States) 
| count
```

The same query can be written as:

```kusto
// Inline subquery 
StormEvents 
| where State in (
    ( StormEvents
    | summarize count() by State
    | top 5 by count_ )
) 
| count
```

|Count|
|---|
|14242|  

**Top with other example:**  

```kusto
let Lightning_By_State = materialize(StormEvents | summarize lightning_events = countif(EventType == 'Lightning') by State);
let Top_5_States = Lightning_By_State | top 5 by lightning_events | project State; 
Lightning_By_State
| extend State = iif(State in (Top_5_States), State, "Other")
| summarize sum(lightning_events) by State 
```

| State     | sum_lightning_events |
|-----------|----------------------|
| ALABAMA   | 29                   |
| WISCONSIN | 31                   |
| TEXAS     | 55                   |
| FLORIDA   | 85                   |
| GEORGIA   | 106                  |
| Other     | 415                  |

**Using a static list returned by a function:**  

```kusto
StormEvents | where State in (InterestingStates()) | count

```

|Count|
|---|
|4775|  


Here is the function definition:  

```kusto
.show function InterestingStates
```

|Name|Parameters|Body|Folder|DocString|
|---|---|---|---|---|
|InterestingStates|()|{ dynamic(["WASHINGTON", "FLORIDA", "GEORGIA", "NEW YORK"]) }
