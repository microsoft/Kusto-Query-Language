---
title: The case-sensitive == (equals) string operator - Azure Data Explorer
description: This article describes the case-sensitive == (equals) string operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/30/2021
---
# == (equals) operator

Filters a record set for data matching a case-sensitive string.

The following table provides a comparison of the `==` operators:

|Operator   |Description   |Case-Sensitive  |Example (yields `true`)  |
|-----------|--------------|----------------|-------------------------|
|[`==`](equals-cs-operator.md)|Equals |Yes|`"aBc" == "aBc"`|
|[`!=`](not-equals-cs-operator.md)|Not equals |Yes |`"abc" != "ABC"`|
|[`=~`](equals-operator.md) |Equals |No |`"abc" =~ "ABC"`|
|[`!~`](not-equals-operator.md) |Not equals |No |`"aBc" !~ "xyz"`|

For further information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md). 

## Performance tips

> [!NOTE]
> Performance depends on the type of search and the structure of the data.

For faster results, use the case-sensitive version of an operator, for example, `==`, not `=~`. For best practices, see [Query best practices](best-practices.md).

## Syntax

*T* `|` `where` *col* `==` `(`*expressions`)`

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
    | where State == "kansas"
    | count 
```

**Output**

|Count|
|---|
|0|  
