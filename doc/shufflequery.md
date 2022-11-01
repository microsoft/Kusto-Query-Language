---
title: Shuffle query - Azure Data Explorer
description: This article describes Shuffle query in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 10/07/2021
---
# shuffle query

The `shuffle` query is a semantic-preserving transformation used with a set of operators that support the `shuffle` strategy. Depending on the data involved, querying with the `shuffle` strategy can yield better performance. It is better to use the shuffle query strategy when the `shuffle` key (a `join` key, `summarize` key, `make-series` key or `partition` key) has a high cardinality and the regular operator query hits query limits.

You can use the following operators with the shuffle command:
* [join](joinoperator.md)
* [summarize](summarizeoperator.md)
* [make-series](make-seriesoperator.md) 
* [partition](partitionoperator.md)

To use the `shuffle` query strategy, add the expression `hint.strategy = shuffle` or `hint.shufflekey = <key>`. When you use `hint.strategy=shuffle`, the operator data will be shuffled by all the keys. Use this expression when the compound key is unique but each key is not unique enough, so you will shuffle the data using all the keys of the shuffled operator.

When partitioning data with the shuffle strategy, the data load is shared on all cluster nodes. Each node  processes one partition of the data. The default number of partitions is equal to the number of cluster nodes. 

The partition number can be overridden by using the syntax `hint.num_partitions = total_partitions`, which will control the number of partitions. This is useful when the cluster has a small number of cluster nodes and the default partitions number will be small, and the query fails or takes a long execution time.

> [!Note]
> Using many partitions may consume more cluster resources and degrade performance. Choose the partition number carefully by starting with the `hint.strategy = shuffle` and start increasing the partitions gradually.

In some cases, the `hint.strategy = shuffle` will be ignored, and the query will not run in `shuffle` strategy. This can happen when:

* The `join` operator has another `shuffle`-compatible operator (`join`, `summarize`, `make-series` or `partition`) on the left side or the right side.
* The `summarize` operator appears after another `shuffle`-compatible operator (`join`, `summarize`, `make-series` or `partition`) in the query.

## Syntax

### With `hint.strategy` = `shuffle`

*T* `|` *DataExpression* `|` `join`  `hint.strategy` = `shuffle` `(` *DataExpression* `)`

*T* `|` `summarize` `hint.strategy` = `shuffle` *DataExpression* 

*T* `|` *Query* `|` partition `hint.strategy` = `shuffle`  `(` *SubQuery* `)`

### With `hint.shufflekey` = *key*

*T* `|` *DataExpression* `|` `join`  `hint.shufflekey` = *key* `(` *DataExpression* `)`

*T* `|` `summarize` `hint.shufflekey` = *key* *DataExpression* 

*T* `|` `make-series` `hint.shufflekey` = *key* *DataExpression* 

*T* `|` *Query* `|` partition  `hint.shufflekey` = *key* `(` *SubQuery* `)`

## Arguments

* *T*: The tabular source whose data is to be processed by the operator.
* *DataExpression*: An implicit or explicit tabular transformation expression.
* *Query*: A transformation expression run on the records of *T*.
* *key*: Use a `join` key, `summarize` key, `make-series` key or `partition` key
* *SubQuery*: A transformation expression.

## Examples

## Use summarize with shuffle

The `shuffle` strategy query with `summarize` operator will share the load on all cluster nodes, where each node will process one partition of the data.

```kusto
StormEvents
| summarize hint.strategy = shuffle count(), avg(InjuriesIndirect) by State
| count 
```

**Output** 

|Count|
|---|
|67|

## Use join with shuffle

```kusto
StormEvents
| where State contains "West"
| where EventType contains "Flood"
| join hint.strategy=shuffle 
( StormEvents
    | where EventType contains "Hail"
    | project EpisodeId, State, DamageProperty
)   on State
| count
```

**Output** 

|Count|
|---|
|103|

## Use make-series with shuffle

```kusto
StormEvents
| where State contains "North"
| make-series hint.shufflekey = State sum(DamageProperty) default = 0 on StartTime in range(datetime(2007-01-01 00:00:00.0000000), datetime(2007-01-31 23:59:00.0000000), 15d) by State
```

**Output** 

|State|sum_DamageProperty|StartTime|
|---|---|---|---|
|NORTH DAKOTA|[60000,0,0]|["2006-12-31T00:00:00.0000000Z","2007-01-15T00:00:00.0000000Z","2007-01-30T00:00:00.0000000Z"]|
|NORTH CAROLINA|[20000,0,1000]|["2006-12-31T00:00:00.0000000Z","2007-01-15T00:00:00.0000000Z","2007-01-30T00:00:00.0000000Z"]|
|ATLANTIC NORTH|[0,0,0]|["2006-12-31T00:00:00.0000000Z","2007-01-15T00:00:00.0000000Z","2007-01-30T00:00:00.0000000Z"]|

### Use partition with shuffle

```kusto
StormEvents
| partition hint.strategy=shuffle by EpisodeId
(
    top 3 by DamageProperty
    | project EpisodeId, State, DamageProperty
)
| count
```

**Output** 

|Count|
|---|
|22345|

### Compare hint.strategy=shuffle and hint.shufflekey=key

When you use `hint.strategy=shuffle`, the shuffled operator will be shuffled by all the keys. In the following example, the query shuffles the data using both `EpisodeId` and `EventId` as keys:

```kusto
StormEvents
| where StartTime > datetime(2007-01-01 00:00:00.0000000)
| join kind = inner hint.strategy=shuffle (StormEvents | where DamageCrops > 62000000) on EpisodeId, EventId
| count
```

**Output** 

|Count|
|---|
|14|

The following query uses `hint.shufflekey = key`. The query above is equivalent to this query.

```kusto
StormEvents
| where StartTime > datetime(2007-01-01 00:00:00.0000000)
| join kind = inner hint.shufflekey = EpisodeId hint.shufflekey = EventId (StormEvents | where DamageCrops > 62000000) on EpisodeId, EventId
```

**Output** 

|Count|
|---|
|14|

### Shuffle the data with multiple keys

In some cases, the `hint.strategy=shuffle` will be ignored, and the query will not run in shuffle strategy. For example, in the following example, the join has summarize on its left side, so using `hint.strategy=shuffle` will not apply shuffle strategy to the query:

```kusto
StormEvents
| where StartTime > datetime(2007-01-01 00:00:00.0000000)
| summarize count() by EpisodeId, EventId
| join kind = inner hint.strategy=shuffle (StormEvents | where DamageCrops > 62000000) on EpisodeId, EventId

```

**Output** 

|Count|
|---|
|14|

To overcome this issue and run in shuffle strategy, choose the key which is common for the `summarize` and `join` operations. In this case, this key is `EpisodeId`. Use the hint `hint.shufflekey` to specify the shuffle key on the `join` to `hint.shufflekey = EpisodeId`:

```kusto
StormEvents
| where StartTime > datetime(2007-01-01 00:00:00.0000000)
| summarize count() by EpisodeId, EventId
| join kind = inner hint.shufflekey=EpisodeId (StormEvents | where DamageCrops > 62000000) on EpisodeId, EventId
```

**Output** 

|Count|
|---|
|14|

### Use summarize with shuffle to improve performance

In this example, using the `summarize` operator with `shuffle` strategy improves performance. The source table has 150M records and the cardinality of the group by key is 10M, which is spread over 10 cluster nodes. 

Using `summarize` operator without `shuffle` strategy, the query ends after 1:08 and the memory usage peak is ~3 GB:

```kusto
orders
| summarize arg_max(o_orderdate, o_totalprice) by o_custkey 
| where o_totalprice < 1000
| count
```

**Output** 

|Count|
|---|
|1086|

While using `shuffle` strategy with `summarize`, the query ends after ~7 seconds and the memory usage peak is 0.43 GB:

```kusto
orders
| summarize hint.strategy = shuffle arg_max(o_orderdate, o_totalprice) by o_custkey 
| where o_totalprice < 1000
| count
```

**Output** 

|Count|
|---|
|1086|

The following example demonstrates performance on a cluster that has two cluster nodes, with a table that has 60M records, where the cardinality of the group by key is 2M.

Running the query without `hint.num_partitions` will use only two partitions (as cluster nodes number) and the following query will take ~1:10 mins:

```kusto
lineitem	
| summarize hint.strategy = shuffle dcount(l_comment), dcount(l_shipdate) by l_partkey 
| consume
```

If setting the partitions number to 10, the query will end after 23 seconds: 

```kusto
lineitem	
| summarize hint.strategy = shuffle hint.num_partitions = 10 dcount(l_comment), dcount(l_shipdate) by l_partkey 
| consume
```

## Use join with shuffle to improve performance

The following example shows how using `shuffle` strategy with the `join` operator improves performance.

The examples were sampled on a cluster with 10 nodes where the data is spread over all these nodes.

The query's left-side source table has 15M records where the cardinality of the `join` key is ~14M. The query's right-side source has 150M records and the cardinality of the `join` key is 10M. The query ends after ~28 seconds and the memory usage peak is 1.43 GB:

```kusto
customer
| join
    orders
on $left.c_custkey == $right.o_custkey
| summarize sum(c_acctbal) by c_nationkey
```

When using `shuffle` strategy with a `join` operator, the query ends after ~4 seconds and the memory usage peak is 0.3 GB:

```kusto
customer
| join
    hint.strategy = shuffle orders
on $left.c_custkey == $right.o_custkey
| summarize sum(c_acctbal) by c_nationkey
```

In another example, we try the same queries on a larger dataset with the following conditions:
* Left-side source of the `join` is 150M and the cardinality of the key is 148M. 
* Right-side source of the `join` is 1.5B, and the cardinality of the key is ~100M.

The query with just the `join` operator hits Azure Data Explorer limits and times-out after 4 mins. However, when using `shuffle` strategy with the `join` operator, the query ends after ~34 seconds and the memory usage peak is 1.23 GB.

The following example shows the improvement on a cluster that has two cluster nodes, with a table of 60M records, where the cardinality of the `join` key is 2M.
Running the query without `hint.num_partitions` will use only two partitions (as cluster nodes number) and the following query will take ~1:10 mins:

```kusto
lineitem
| summarize dcount(l_comment), dcount(l_shipdate) by l_partkey
| join
    hint.shufflekey = l_partkey   part
on $left.l_partkey == $right.p_partkey
| consume
```

When setting the partitions number to 10, the query will end after 23 seconds: 

```kusto
lineitem
| summarize dcount(l_comment), dcount(l_shipdate) by l_partkey
| join
    hint.shufflekey = l_partkey  hint.num_partitions = 10    part
on $left.l_partkey == $right.p_partkey
| consume
```
