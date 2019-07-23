---
title: make_list() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes make_list() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 07/16/2019
---
# make_list() (aggregation function)

Returns a `dynamic` (JSON) array of all the values of *Expr* in the group.

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

**Syntax**

`summarize` `make_list(`*Expr* [`,` *MaxListSize*]`)`

**Arguments**

* *Expr*: Expression that will be used for aggregation calculation.
* *MaxListSize* is an optional integer limit on the maximum number of elements returned (default is *1048576*). MaxListSize value cannot exceed 1048576.

**Note**

An legacy and obsolete variant of this function: `makelist()` has a default limit of *MaxListSize* = 128.

**Returns**

Returns a `dynamic` (JSON) array of all the values of *Expr* in the group.
If the input to the `summarize` operator is not sorted, the order of elements in the resulting array is undefined.
If the input to the `summarize` operator is sorted, the order of elements in the resulting array tracks that of the input.

**Tips**

Use [mv-apply](./mv-applyoperator.md) operator in order to create an ordered list by some key. see examples [here](./mv-applyoperator.md#using-mv-apply-operator-to-sort-the-output-of-makelist-aggregate-by-some-key).