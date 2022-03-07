---
title: The case-insensitive in~ string operator - Azure Data Explorer
description: This article describes the case-insensitive in~ string operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/19/2021
---
# in~ operator

Filters a record set for data with a case-insensitive string.

The following table provides a comparison of the `in` operators:

|Operator   |Description   |Case-Sensitive  |Example (yields `true`)  |
|-----------|--------------|----------------|-------------------------|
|[`in`](in-cs-operator.md) |Equals to one of the elements |Yes |`"abc" in ("123", "345", "abc")`|
|[`!in`](not-in-cs-operator.md) |Not equals to any of the elements |Yes | `"bca" !in ("123", "345", "abc")` |
|[`in~`](inoperator.md) |Equals to any of the elements |No | `"Abc" in~ ("123", "345", "abc")` |
|[`!in~`](not-in-operator.md) |Not equals to any of the elements |No | `"bCa" !in~ ("123", "345", "ABC")` |

> [!NOTE]
>
> * In tabular expressions, the first column of the result set is selected.
> * The expression list can produce up to `1,000,000` values.
> * Nested arrays are flattened into a single list of values. For example, `x in (dynamic([1,[2,3]]))` becomes `x in (1,2,3)`.

For further information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md). 

Case-insensitive operators are currently supported only for ASCII-text. For non-ASCII comparison, use the [tolower()](tolowerfunction.md) function.

## Performance tips

> [!NOTE]
> Performance depends on the type of search and the structure of the data.

For faster results, use the case-sensitive version of an operator, for example, `in`, not `in~`. 

If you're testing for the presence of a symbol or alphanumeric word that is bound by non-alphanumeric characters at the start or end of a field, for faster results use `has` or `in`. 

For best practices, see [Query best practices](best-practices.md).

## Syntax

*T* `|` `where` *col* `in~` `(` *list of scalar expressions* `)`
*T* `|` `where` *col* `in~` `(` *tabular expression* `)`

## Arguments

* *T* - The tabular input whose records are to be filtered.
* *col* - The column to filter.
* *list of expressions* - A comma-separated list of tabular, scalar, or literal expressions.
* *tabular expression* - A tabular expression that has a set of values. If the expression has multiple columns, the first column is used.

## Returns

Rows in *T* for which the predicate is `true`.

## Examples  

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
StormEvents 
| where State in~ ("FLORIDA", "georgia", "NEW YORK") 
| count
```

**Output**

|Count|
|---|
|4775|  
