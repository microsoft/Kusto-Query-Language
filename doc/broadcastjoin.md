---
title:  Broadcast join
description: Learn how to use the broadcast join execution strategy to distribute the join over cluster nodes.
ms.reviewer: alexans
ms.topic: reference
ms.date: 04/11/2023
---
# Broadcast join

Today, regular joins are executed on a single cluster node.
Broadcast join is an execution strategy of join that distributes the join over cluster nodes. This strategy is useful when the left side of the join is small (up to several tens of MBs). In this case, a broadcast join is more performant than a regular join.

> [!NOTE]
> If the left side of the join is larger than several tens of MBs, the query will fail.
>
> You can run the following query to estimate the size of the left side, in bytes:
>
> ```kusto
> leftSide
> | summarize sum(estimate_data_size(*))
> ```

If left side of the join is a small dataset, then you may run join in broadcast mode using the following syntax (hint.strategy = broadcast):

```kusto
leftSide 
| join hint.strategy = broadcast (factTable) on key
```

The performance improvement is more noticeable in scenarios where the join is followed by other operators such as `summarize`.  See the following query for example:

```kusto
leftSide 
| join hint.strategy = broadcast (factTable) on Key
| summarize dcount(Messages) by Timestamp, Key
```
