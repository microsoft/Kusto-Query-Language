---
title: The case-insensitive =~ (equals) string operator - Azure Data Explorer
description: Learn how to use the =~ (equals) operator to filter a record set for data with a case-insensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/11/2022
---
# =~ (equals) operator

Filters a record set for data with a case-insensitive string.

The following table provides a comparison of the `==` (equals) operators:

|Operator   |Description   |Case-Sensitive  |Example (yields `true`)  |
|-----------|--------------|----------------|-------------------------|
|[`==`](equals-cs-operator.md)|Equals |Yes|`"aBc" == "aBc"`|
|[`!=`](not-equals-cs-operator.md)|Not equals |Yes |`"abc" != "ABC"`|
|[`=~`](equals-operator.md) |Equals |No |`"abc" =~ "ABC"`|
|[`!~`](not-equals-operator.md) |Not equals |No |`"aBc" !~ "xyz"`|

For more information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md).

Case-insensitive operators are currently supported only for ASCII-text. For non-ASCII comparison, use the [tolower()](tolowerfunction.md) function.

## Performance tips

> [!NOTE]
> Performance depends on the type of search and the structure of the data.

For faster results, use the case-sensitive version of an operator, for example, `==`, not `=~`. For best practices, see [Query best practices](best-practices.md).

## Syntax

*T* `|` `where` *col* `=~` `(`*expression*`)`

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
    | where State =~ "kansas"
    | count 
```

**Output**

|Count|
|---|
|3,166|  
