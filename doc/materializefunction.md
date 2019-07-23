---
title: materialize() - Azure Data Explorer | Microsoft Docs
description: This article describes materialize() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 03/21/2019
---
# materialize()

Allows caching a sub-query result during the time of query execution in a way that other subqueries can reference the partial result.

 
**Syntax**

`materialize(`*expression*`)`

**Arguments**

* *expression*: Tabular expression to be evaluated and cached during query execution.

**Tips**

* Use materialize when you have join/union where their operands has mutual sub-queries that can be executed once (see the examples below).

* Useful also in scenarios when we need to join/union fork legs.

* Materialize is allowed to be used only in let statements by giving the cached result a name.

* Materialize has a cache size limit which is **5 GB**. 
  This limit is per cluster node and is mutual for all queries running concurrently.
  If a query uses `materialize()` and the cache cannot hold any additional data,
  the query aborts with an error.

**Examples**

Assuming that we want to generate a random set of values and we are interested in finding how much distinct values we have, the sum of all these values and the top 3 values.

This can be done using [batches](batches.md) and materialize :

 ```kusto
let randomSet = materialize(range x from 1 to 30000000 step 1
| project value = rand(10000000));
randomSet
| summarize dcount(value);
randomSet
| top 3 by value;
randomSet
| summarize sum(value)

```