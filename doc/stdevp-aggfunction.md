---
title: stdevp() (aggregation function) - Azure Data Explorer
description: This article describes stdevp() (aggregation function) in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# stdevp() (aggregation function)

Calculates the standard deviation of *Expr* across the group, considering the group as a [population](https://en.wikipedia.org/wiki/Statistical_population) for a large data set that is representative of the population. 

For a small data set that is a [sample](https://en.wikipedia.org/wiki/Sample_%28statistics%29), use [stdev() (aggregation function)](stdev-aggfunction.md). 


* Used formula:

:::image type="content" source="images/stdevp-aggfunction/stdev-population.png" alt-text="Stdev population.":::

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

## Syntax

`stdevp` `(`*Expr*`)`

## Arguments

* *Expr*: Expression that will be used for aggregation calculation. 

## Returns

The standard deviation value of *Expr* across the group.
 
## Examples

```kusto
range x from 1 to 5 step 1
| summarize make_list(x), stdevp(x)

```

|list_x|stdevp_x|
|---|---|
|[ 1, 2, 3, 4, 5]|1.4142135623731|
