---
title: Batches - Azure Data Explorer | Microsoft Docs
description: This article describes Batches in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/29/2018
---
# Batches

A query can include multiple tabular expression statements, as long as they
are delimited by a semicolon (`;`) character. The query then returns multiple
tabular results, as produced by the tabular expression statements, and ordered
according to the order of the statements in the query text.

For example, the following query produces two tabular results. User agent tools
can then display those results with the appropriate name associated with each
(`Count of events in Florida` and `Count of events in Guam`, respectively).

```kusto
StormEvents | where State == "FLORIDA" | count | as ['Count of events in Florida'];
StormEvents | where State == "GUAM" | count | as ['Count of events in Guam']
```

Batch is particularly useful for scenarios in which there is a common calculation
that is shared by multiple sub-queries, such as for dashboards. If the common
calculation is complex, it is recommended that one construct the query so that
it'll be executed only once, using the [materialize() function](./materializefunction.md):

```kusto
let m = materialize(StormEvents | summarize n=count() by State);
m | where n > 2000;
m | where n < 10
```

Notes:
* Prefer batching and [`materialize`](materializefunction.md) over using the [fork operator](forkoperator.md).