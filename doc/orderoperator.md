---
title: order operator  - Azure Data Explorer | Microsoft Docs
description: This article describes order operator  in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# order operator 

Sort the rows of the input table into order by one or more columns.

```kusto
T | order by country asc, price desc
```

> [!NOTE]
> The order operator is an alias to the sort operator. For more information, see [sort operator](sortoperator.md)

## Syntax

*T* `| order by` *column* [`asc` | `desc`] [`nulls first` | `nulls last`] [`,` ...]

## Arguments

* *T*: The table input to sort.
* *column*: Column of *T* by which to sort. The type of the values must be numeric, date, time or string.
* `asc` Sort by into ascending order, low to high. The default is `desc`, descending high to low.
* `nulls first` (the default for `asc` order) will place the null values at the beginning and `nulls last` (the default for `desc` order) will place the null values at the end.

