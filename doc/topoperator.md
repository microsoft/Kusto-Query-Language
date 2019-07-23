---
title: top operator - Azure Data Explorer | Microsoft Docs
description: This article describes top operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# top operator

Returns the first *N* records sorted by the specified columns.

```kusto
T | top 5 by Name desc nulls last
```

**Syntax**

*T* `| top` *NumberOfRows* `by` *Expression* [`asc` | `desc`] [`nulls first` | `nulls last`]

**Arguments**

* *NumberOfRows*: The number of rows of *T* to return. You can specify any numeric expression.
* *Expression*: A scalar expression by which to sort. The type of the values must be numeric, date, time or string.
* `asc` or `desc` (the default) may appear to control whether selection is actually from the "bottom" or "top" of the range.
* `nulls first` (the default for `asc` order) or `nulls last` (the default for `desc` order) may appear to control whether null values will be at the beginning or the end of the range.


**Tips**

`top 5 by name` is superficially equivalent to `sort by name | take 5`. However, it runs faster and always returns sorted results, whereas `take` makes no such guarantee.
[top-nested](topnestedoperator.md) allows to produce hierarchical top results.