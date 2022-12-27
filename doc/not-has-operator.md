---
title: The case-insensitive !has string operators - Azure Data Explorer
description: This article describes the case-insensitive !has string operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/21/2021
---
# !has operator

Filters a record set for data that does not have a matching case-insensitive string. `has` searches for indexed terms, where a [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

[!INCLUDE [has-operator-comparison](../../includes/has-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

For faster results, use the case-sensitive version of an operator. For example, use `has_cs` instead of `has`.

If you're testing for the presence of a symbol or alphanumeric word that is bound by non-alphanumeric characters at the start or end of a field, for faster results use `has` or `in`.

## Syntax

*T* `|` `where` *Column* `!has` `(`*Expression*`)`   

## Arguments

* *T* - The tabular input whose records are to be filtered.
* *Column* - The column to filter.
* *Expression* - Scalar or literal expression.

## Returns

Rows in *T* for which the predicate is `true`.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
    | summarize event_count=count() by State
    | where State !has "NEW"
    | where event_count > 3000
    | project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|TEXAS|4,701|
|KANSAS|3,166| 
