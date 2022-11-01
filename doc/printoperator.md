---
title: print operator - Azure Data Explorer
description: This article describes print operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 07/03/2022
---
# print operator

Outputs a single-row with one or more scalar expression results as columns.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print x=1, s=strcat("Hello", ", ", "World!")
```

## Syntax

`print` [*ColumnName* `=`] *ScalarExpression* [',' ...]

## Arguments

* *ColumnName*: An optional name to assign to the output column.
* *ScalarExpression*: A scalar expression to evaluate.

## Returns

A table with one or more columns and a single row. Each column returns the corresponding value of the evaluated *ScalarExpression*.

## Examples

The `print` operator is useful as a quick way to evaluate one or more
scalar expressions and make a single-row table out of the resulting values.
For example:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print 0 + 1 + 2 + 3 + 4 + 5, x = "Wow!"
```
<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print banner=strcat("Hello", ", ", "World!")
```
