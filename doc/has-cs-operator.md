---
title: The case-sensitive has_cs string operator - Azure Data Explorer
description: Learn how to use the has_cs operator to filter data with a case-sensitive search string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/18/2022
---
# has_cs_ operator

Filters a record set for data with a case-sensitive search string. `has` searches for indexed terms, where a [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

The following table provides a comparison of the `has` operators:

|Operator   |Description   |Case-Sensitive  |Example (yields `true`)  |
|-----------|--------------|----------------|-------------------------|
|[`has`](has-operator.md) |Right-hand-side (RHS) is a whole term in left-hand-side (LHS) |No |`"North America" has "america"`|
|[`!has`](not-has-operator.md) |RHS isn't a full term in LHS |No |`"North America" !has "amer"`|
|[`has_cs`](has-cs-operator.md) |RHS is a whole term in LHS |Yes |`"North America" has_cs "America"`|
|[`!has_cs`](not-has-cs-operator.md) |RHS isn't a full term in LHS |Yes |`"North America" !has_cs "amer"`|

> [!NOTE]
> The following abbreviations are used in the previous table:
>
> * RHS = right hand side of the expression
> * LHS = left hand side of the expression

For more information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md).

Case-insensitive operators are currently supported only for ASCII-text. For non-ASCII comparison, use the [tolower()](tolowerfunction.md) function.

## Performance tips

> [!NOTE]
> Performance depends on the type of search and the structure of the data.

For faster results, use the case-sensitive version of an operator, for example, `has_cs`, not `has`. For best practices, see [Query best practices](best-practices.md).

## Syntax

*T* `|` `where` *Column* `has_cs` `(`*Expression*`)`

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
    | where State has_cs "New"
    | count
```

**Output**

|Count|
|-----|
|0|
