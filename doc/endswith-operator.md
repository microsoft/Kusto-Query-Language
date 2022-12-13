---
title: The case-insensitive endswith string operator - Azure Data Explorer
description: Learn how to use the endswith operator to filter a record set for data with a case-insensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/11/2022
---
# endswith operator

Filters a record set for data with a case-insensitive ending string.

The following table provides a comparison of the `endswith` operators:

|Operator   |Description   |Case-Sensitive  |Example (yields `true`)  |
|-----------|--------------|----------------|-------------------------|
|[`endswith`](endswith-operator.md) |RHS is a closing subsequence of LHS |No |`"Fabrikam" endswith "Kam"`|
|[`!endswith`](not-endswith-operator.md) |RHS isn't a closing subsequence of LHS |No |`"Fabrikam" !endswith "brik"`|
|[`endswith_cs`](endswith-cs-operator.md) |RHS is a closing subsequence of LHS |Yes |`"Fabrikam" endswith_cs "kam"`|
|[`!endswith_cs`](not-endswith-cs-operator.md) |RHS isn't a closing subsequence of LHS |Yes |`"Fabrikam" !endswith_cs "brik"`|

> [!NOTE]
> The following abbreviations are used in the table above:
>
> * RHS = right hand side of the expression
> * LHS = left hand side of the expression

For more information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md).

Case-insensitive operators are currently supported only for ASCII-text. For non-ASCII comparison, use the [tolower()](tolowerfunction.md) function.

## Performance tips

> [!NOTE]
> Performance depends on the type of search and the structure of the data.

For faster results, use the case-sensitive version of an operator, for example, `endswith_cs`, not `endswith`. For best practices, see [Query best practices](best-practices.md).

## Syntax

*T* `|` `where` *col* `endswith` `(`*expression*`)`

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
    | where State endswith "sas"
    | where event_count > 10
    | project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|KANSAS|3166|
|ARKANSAS|1028|
