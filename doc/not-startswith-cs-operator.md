---
title: The case-sensitive !startswith_cs string operator - Azure Data Explorer
description: This article describes the case-sensitive !startswith_cs string operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/19/2021
---
# !startswith_cs operators

Filters a record set for data that does not start with a case-sensitive search string.

The following table provides a comparison of the `startswith` operators:

|Operator   |Description   |Case-Sensitive  |Example (yields `true`)  |
|-----------|--------------|----------------|-------------------------|
|[`startswith`](startswith-operator.md) |RHS is an initial subsequence of LHS |No |`"Fabrikam" startswith "fab"`|
|[`!startswith`](not-startswith-operator.md) |RHS isn't an initial subsequence of LHS |No |`"Fabrikam" !startswith "kam"`|
|[`startswith_cs`](startswith-cs-operator.md)  |RHS is an initial subsequence of LHS |Yes |`"Fabrikam" startswith_cs "Fab"`|
|[`!startswith_cs`](not-startswith-cs-operator.md) |RHS isn't an initial subsequence of LHS |Yes |`"Fabrikam" !startswith_cs "fab"`|

> [!NOTE]
> The following abbreviations are used in the table above:
>
> * RHS = right hand side of the expression
> * LHS = left hand side of the expression

For further information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md). 

## Performance tips

> [!NOTE]
> Performance depends on the type of search and the structure of the data.

For faster results, use the case-sensitive version of an operator, for example, `hassuffix_cs`, not `hassuffix`. 

If you're testing for the presence of a symbol or alphanumeric word that is bound by non-alphanumeric characters at the start or end of a field, for faster results use `has` or `in`. 

For best practices, see [Query best practices](best-practices.md).

## Syntax

*T* `|` `where` *col* `!startswith_cs` `(`*expression*`)`  

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
    | where State !startswith_cs "I"
    | where event_count > 2000
    | project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|TEXAS|4701|
|KANSAS|3166|
|MISSOURI|2016|
