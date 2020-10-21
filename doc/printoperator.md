---
title: print operator - Azure Data Explorer
description: This article describes print operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 03/16/2019
---
# print operator

Outputs single-row with one or more scalar expressions.

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print x=1, s=strcat("Hello", ", ", "World!")
```

## Syntax

`print` [*ColumnName* `=`] *ScalarExpression* [',' ...]

## Arguments

* *ColumnName*: An option name to assign to the output's singular column.
* *ScalarExpression*: A scalar expression to evaluate.

## Returns

A single-column, single-row, table whose single cell has the value of the evaluated *ScalarExpression*.

## Examples

The `print` operator is useful as a quick way to evaluate one or more
scalar expressions and make a single-row table out of the resulting values.
For example:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print 0 + 1 + 2 + 3 + 4 + 5, x = "Wow!"
```
<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print banner=strcat("Hello", ", ", "World!")
```
