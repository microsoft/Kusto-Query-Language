---
title: variance() (aggregation function) - Azure Data Explorer | Microsoft Docs
description: This article describes variance() (aggregation function) in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 02/19/2019
---
# variance() (aggregation function)

Calculates the variance of *Expr* across the group, considering the group as a [sample](https://en.wikipedia.org/wiki/Sample_%28statistics%29). 

* Used formula:
![alt text](./images/aggregations/variance-sample.png "variance-sample")

* Can be used only in context of aggregation inside [summarize](summarizeoperator.md)

**Syntax**

summarize `variance(`*Expr*`)`

**Arguments**

* *Expr*: Expression that will be used for aggregation calculation. 

**Returns**

The variance value of *Expr* across the group.
 
**Examples**

```kusto
range x from 1 to 5 step 1
| summarize make_list(x), variance(x) 
```

|list_x|variance_x|
|---|---|
|[ 1, 2, 3, 4, 5]|2.5|