---
title: Batches - Azure Data Explorer
description: This article describes Batches in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
---
# Batches

A query can include multiple tabular expression statements, as long as they're delimited by a semicolon (`;`) character. The query then returns multiple
tabular results. Results are produced by the tabular expression statements and ordered according to the order of the statements in the query text.

For example, the following query produces two tabular results. User agent tools
can then display those results with the appropriate name associated with each
(`Count of events in Florida` and `Count of events in Guam`, respectively).

```kusto
StormEvents | where State == "FLORIDA" | count | as ['Count of events in Florida'];
StormEvents | where State == "GUAM" | count | as ['Count of events in Guam']
```

Batch is useful for scenarios where a common calculation is shared by multiple subqueries, such as for dashboards. If the common
calculation is complex, use the [materialize() function](./materializefunction.md) and construct the query so that
it will be executed only once:

```kusto
let m = materialize(StormEvents | summarize n=count() by State);
m | where n > 2000;
m | where n < 10
```

Notes:
* Prefer batching and [`materialize`](materializefunction.md) over using the [fork operator](forkoperator.md).
