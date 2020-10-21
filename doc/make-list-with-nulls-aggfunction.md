---
title: make_list_with_nulls() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes make_list_with_nulls() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 03/09/2020
---
# make_list_with_nulls() (aggregation function)

Returns a `dynamic` (JSON) array of all the values of *Expr* in the group, including null values.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

`summarize` `make_list_with_nulls(` *Expr* `)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation.

## Returns

Returns a `dynamic` (JSON) array of all the values of *Expr* in the group, including null values.
If the input to the `summarize` operator is not sorted, the order of elements in the resulting array is undefined.
If the input to the `summarize` operator is sorted, the order of elements in the resulting array tracks that of the input.

> [!TIP]
> Use the [`array_sort_asc()`](./arraysortascfunction.md) or [`array_sort_desc()`](./arraysortdescfunction.md) function to create an ordered list by some key.
