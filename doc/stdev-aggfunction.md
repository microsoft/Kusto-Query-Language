---
title: stdev() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes stdev() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# stdev() (aggregation function)

Calculates the standard deviation of *Expr* across the group, considering the group as a [sample](https://en.wikipedia.org/wiki/Sample_%28statistics%29). 

* Used formula:

:::image type="content" source="images/stdev-aggfunction/stdev-sample.png" alt-text="Stdev sample":::

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

summarize `stdev(`*Expr*`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation. 

## Returns

The standard deviation value of *Expr* across the group.
 
## Examples

```kusto
range x from 1 to 5 step 1
| summarize make_list(x), stdev(x)

```

|list_x|stdev_x|
|---|---|
|[ 1, 2, 3, 4, 5]|1.58113883008419|