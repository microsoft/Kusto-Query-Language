---
title: Broadcast Join - Azure Data Explorer | Microsoft Docs
description: This article describes Broadcast Join in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# Broadcast Join

Today, the regular joins are executed on a single cluster node.
Broadcast join is an execution strategy of join which will distribute it over cluster nodes, and it is useful when left side of the join is small (up to 100K records), In this case, join will be more performant than regular join.

If left side of the join is a small dataset, then you may run join in broadcast mode using the following syntax (hint.strategy = broadcast):

```kusto
lookupTable 
| join hint.strategy = broadcast (factTable) on key
```

Performance improvement will be more noticeable in scenarios where the join is followed by other operators such as `summarize`. for example in this query:

```kusto
lookupTable 
| join hint.strategy = broadcast (factTable) on Key
| summarize dcount(Messages) by Timestamp, Key
```