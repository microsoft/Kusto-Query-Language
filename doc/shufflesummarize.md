---
title: Shuffle summarize - Azure Data Explorer | Microsoft Docs
description: This article describes Shuffle summarize in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 01/20/2019
---
# Shuffle summarize

Shuffle summarize is a semantic-preserving transformation for summarize that depending on the actual data can sometimes yield considerably better performance.

**Syntax**

Shuffle summarize strategy can be set by the query parameter `hint.strategy = shuffle`:

```kusto
T
| summarize hint.strategy = shuffle count(), avg(price) by supplier
```


This strategy will share the load on all cluster nodes where each node will process one partition of the data.
Shuffle summarize strategy can provide significant performance benefit when the 'by' clause has columns with high cardinality which may be causing the regular summarize strategy to hit query limits.

In addition, it is possible to choose the shuffle keys that will be used by the query parameter `hint.shufflekey = key` :

```kusto
T
| summarize sum(price) by supplier, order
| where sum_price > 100
| summarize hint.shufflekey = supplier dcount(order) by supplier
```

It is also possible to use more than one shuffle key as in the following example:

```kusto
T
| summarize sum(price) by supplier, order, location
| where sum_price > 100
| summarize hint.shufflekey = supplier hint.shufflekey = location dcount(order) by supplier, location
```


It is useful when the key is used more than once in two different operators that can be shuffled (like the two summarize operators above) so in the query above both summarize operators will be shuffled and the query will perform better than shuffling only the top summarize one.
Note that the shuffle keys can only be join key in join and group by keys of the summarize.

**Examples**

The following example shows how shuffle summarize improves performance considerably:

The source table has 150M records and the cardinality of the group by key is 10M which is spread over 10 cluster nodes.

Running the regular summarize strategy, the query ends after 1:08 and the memory usage peak is ~3GB:


```kusto
orders
| summarize arg_max(o_orderdate, o_totalprice) by o_custkey 
| where o_totalprice < 1000
| count
```

|Count|
|---|
|1086|


While using shuffle summarize strategy, the query ends after ~7 seconds and the memory usage peak is 0.43GB:

```kusto
orders
| summarize hint.strategy = shuffle arg_max(o_orderdate, o_totalprice) by o_custkey 
| where o_totalprice < 1000
| count
```

|Count|
|---|
|1086|


In shuffle query, the default partitions number is the cluster nodes number. This number can be overriden by using the syntax `hint.num_partitions = total_partitions` which will control the number of partitions.

This hint is useful when the cluster has a small number of cluster nodes where the default partitions number will be small too and the query still fails or takes long execution time.

**Examples**

The following example shows the improvement on a cluster which has 2 cluster nodes, the table has 60M records and the cardinality of the group by key is 2M.

Running the query without the hint will use only 2 partitions (as cluster nodes number) and the following query will take ~1:10 mins :

```kusto
lineitem	
| summarize hint.strategy = shuffle dcount(l_comment), dcount(l_shipdate) by l_partkey 
| consume
```

setting partitions number to 10, the query will end after 23 seconds: 

```kusto
lineitem	
| summarize hint.strategy = shuffle hint.num_partitions = 10 dcount(l_comment), dcount(l_shipdate) by l_partkey 
| consume
```

Note that setting many partitions may degrade performance and consume more cluster resources so it is recommended to choose the partition number carefully (starting with the hint.strategy = shuffle and start increasing the partitions gradually).