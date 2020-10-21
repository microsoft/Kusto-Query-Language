---
title: variancep() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes variancep() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# variancep() (aggregation function)

Calculates the variance of *Expr* across the group, considering the group as a [population](https://en.wikipedia.org/wiki/Statistical_population). 

* Used formula:

:::image type="content" source="images/variancep-aggfunction/variance-population.png" alt-text="Variance population":::

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

summarize `variancep(`*Expr*`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation. 

## Returns

The variance value of *Expr* across the group.
 
## Examples

```kusto
range x from 1 to 5 step 1
| summarize make_list(x), variancep(x) 
```

|list_x|variance_x|
|---|---|
|[ 1, 2, 3, 4, 5]|2|