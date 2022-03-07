---
title: The case-sensitive hasprefix_cs string operator - Azure Data Explorer
description: This article describes the case-sensitive hasprefix_cs string operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/21/2021
---
# hasprefix_cs operator

Filters a record set for data with a case-sensitive starting string. `has` searches for indexed terms, where a [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

The following table provides a comparison of the `has` operators:

|Operator   |Description   |Case-Sensitive  |Example (yields `true`)  |
|-----------|--------------|----------------|-------------------------|
|[`hasprefix`](hasprefix-operator.md) |RHS is a term prefix in LHS |No |`"North America" hasprefix "ame"`|
|[`!hasprefix`](not-hasprefix-operator.md) |RHS isn't a term prefix in LHS |No |`"North America" !hasprefix "mer"`|
|[`hasprefix_cs`](hasprefix-cs-operator.md) |RHS is a term prefix in LHS |Yes |`"North America" hasprefix_cs "Ame"`|
|[`!hasprefix_cs`](not-hasprefix-cs-operator.md) |RHS isn't a term prefix in LHS |Yes |`"North America" !hasprefix_cs "CA"`|

> [!NOTE]
> The following abbreviations are used in the table above:
>
> * RHS = right hand side of the expression
> * LHS = left hand side of the expression

For further information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md). 

## Performance tips

> [!NOTE]
> Performance depends on the type of search and the structure of the data.

For faster results, use the case-sensitive version of an operator, for example, `hasprefix_cs`, not `hasprefix`. For best practices, see [Query best practices](best-practices.md).

## Syntax

*T* `|` `where` *col* `hasprefix_cs` `(`*expression*`)`

## Arguments

* *T* - The tabular input whose records are to be filtered.
* *col* - The column to filter.
* *expression* - Scalar or literal expression.

## Returns

Rows in *T* for which the predicate is `true`.

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
    | summarize event_count=count() by State
    | where State hasprefix_cs "P"
    | count 
```

**Output**

|Count|
|-----|
|3|

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents
    | summarize event_count=count() by State
    | where State hasprefix_cs "P"
    | project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|PENNSYLVANIA|1687|
|PUERTO RICO|192|
|E PACIFIC|10|
