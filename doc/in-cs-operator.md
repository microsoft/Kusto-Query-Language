---
title: The case-sensitive in string operator - Azure Data Explorer
description: This article describes the case-sensitive in string operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/19/2021
---
# in operator

Filters a record set for data with a case-sensitive string.

The following table provides a comparison of the `in` operators:

|Operator   |Description   |Case-Sensitive  |Example (yields `true`)  |
|-----------|--------------|----------------|-------------------------|
|[`in`](in-cs-operator.md) |Equals to any of the elements |Yes |`"abc" in ("123", "345", "abc")`|
|[`!in`](not-in-cs-operator.md) |Not equals to any of the elements |Yes | `"bca" !in ("123", "345", "abc")` |
|[`in~`](inoperator.md) |Equals to any of the elements |No | `"Abc" in~ ("123", "345", "abc")` |
|[`!in~`](not-in-operator.md) |Not equals to any of the elements |No | `"bCa" !in~ ("123", "345", "ABC")` |

> [!NOTE]
>
> * In tabular expressions, the first column of the result set is selected.
> * The expression list can produce up to `1,000,000` values.
> * Nested arrays are flattened into a single list of values. For example, `x in (dynamic([1,[2,3]]))` becomes `x in (1,2,3)`.

For further information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md). 

## Performance tips

> [!NOTE]
> Performance depends on the type of search and the structure of the data.

For faster results, use the case-sensitive version of an operator, for example, `in`, not `in~`. For best practices, see [Query best practices](best-practices.md).

## Syntax

*T* `|` `where` *col* `in` `(`*list of scalar expressions*`)`   
*T* `|` `where` *col* `in` `(`*tabular expression*`)`   

## Arguments

* *T* - The tabular input whose records are to be filtered.
* *col* - The column to filter.
* *list of expressions* - A comma-separated list of tabular, scalar, or literal expressions.
* *tabular expression* - A tabular expression that has a set of values. If the expression has multiple columns, the first column is used.

## Returns

Rows in *T* for which the predicate is `true`.

## Examples  

### Use in operator

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents 
| where State in ("FLORIDA", "GEORGIA", "NEW YORK") 
| count
```

**Output**

|Count|
|---|
|4775|  

### Use dynamic array

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let states = dynamic(['FLORIDA', 'ATLANTIC SOUTH', 'GEORGIA']);
StormEvents 
| where State in (states)
| count
```

**Output**

|Count|
|---|
|3218|

### Subquery

<!-- csl: https://help.kusto.windows.net/Samples -->
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

<!-- csl: https://help.kusto.windows.net/Samples -->
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

**Output**

|Count|
|---|
|14242|  

### Top with other example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let Lightning_By_State = materialize(StormEvents | summarize lightning_events = countif(EventType == 'Lightning') by State);
let Top_5_States = Lightning_By_State | top 5 by lightning_events | project State; 
Lightning_By_State
| extend State = iif(State in (Top_5_States), State, "Other")
| summarize sum(lightning_events) by State 
```

**Output**

| State     | sum_lightning_events |
|-----------|----------------------|
| ALABAMA   | 29                   |
| WISCONSIN | 31                   |
| TEXAS     | 55                   |
| FLORIDA   | 85                   |
| GEORGIA   | 106                  |
| Other     | 415                  |

### Use a static list returned by a function

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents | where State in (InterestingStates()) | count

```

**Output**

|Count|
|---|
|4775|  

The function definition.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
.show function InterestingStates
```

**Output**

|Name|Parameters|Body|Folder|DocString|
|---|---|---|---|---|
|InterestingStates|()|{ dynamic(["WASHINGTON", "FLORIDA", "GEORGIA", "NEW YORK"]) }
