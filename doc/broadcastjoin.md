---
title: Broadcast Join - Azure Data Explorer
description: This article describes Broadcast Join in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# Broadcast join

Today, regular joins are executed on a single cluster node.
Broadcast join is an execution strategy of join that distributes the join over cluster nodes. This strategy is useful when left side of the join is small (up to few tens of MBs). In this case, a broadcast join will be more performant than a regular join.
Run the following query to get the estimated size of the left side in bytes:

```kusto
lookupSubQuery
| summarize sum(estimate_data_size(*))
``` 

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
