---
title: The case-sensitive contains_cs string operator - Azure Data Explorer
description: Learn how to use the contains_cs operator to filter a record set for data containing a case-sensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
---
# contains_cs operator

Filters a record set for data containing a case-sensitive string. `contains` searches for characters rather than [terms](datatypes-string-operators.md#what-is-a-term) of three or more characters. The query scans the values in the column, which is slower than looking up a term in a term index.

The following table provides a comparison of the `contains` operators:

|Operator   |Description   |Case-Sensitive  |Example (yields `true`)  |
|-----------|--------------|----------------|-------------------------|
|[`contains`](contains-operator.md) |RHS occurs as a subsequence of LHS |No |`"FabriKam" contains "BRik"`|
|[`!contains`](not-contains-operator.md) |RHS doesn't occur in LHS |No |`"Fabrikam" !contains "xyz"`|
|[`contains_cs`](contains-cs-operator.md) |RHS occurs as a subsequence of LHS |Yes |`"FabriKam" contains_cs "Kam"`|
|[`!contains_cs`](not-contains-cs-operator.md)   |RHS doesn't occur in LHS |Yes |`"Fabrikam" !contains_cs "Kam"`|

> [!NOTE]
> The following abbreviations are used in the table above:
>
> * RHS = right hand side of the expression
> * LHS = left hand side of the expression

For further information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md).

## Performance tips

> [!NOTE]
> Performance depends on the type of search and the structure of the data.
For faster results, use the case-sensitive version of an operator, for example, `contains_cs`, not `contains`.

If you're testing for the presence of a symbol or alphanumeric word that is bound by non-alphanumeric characters at the start or end of a field, for faster results use `has` or `in`. Also, `has` works faster than `contains`, `startswith`, or `endswith`, however it isn't as precise and could provide unwanted records.

For best practices, see [Query best practices](best-practices.md).

## Syntax

*T* `|` `where` *col* `contains_cs` `(`*expression*`)`

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
    | where State contains_cs "AS"
    | count
```

**Output**

|Count|
|-----|
|8|
