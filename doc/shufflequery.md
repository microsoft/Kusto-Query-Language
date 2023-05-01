---
title: Shuffle query - Azure Data Explorer
description: This article describes Shuffle query in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/30/2023
---
# shuffle query

The `shuffle` query is a semantic-preserving transformation used with a set of operators that support the `shuffle` strategy. Depending on the data involved, querying with the `shuffle` strategy can yield better performance. It is better to use the shuffle query strategy when the `shuffle` key (a `join` key, `summarize` key, `make-series` key or `partition` key) has a high cardinality and the regular operator query hits query limits.

You can use the following operators with the shuffle command:

* [join](joinoperator.md)
* [summarize](summarizeoperator.md)
* [make-series](make-seriesoperator.md)
* [partition](partitionoperator.md)

To use the `shuffle` query strategy, add the expression `hint.strategy = shuffle` or `hint.shufflekey = <key>`. When you use `hint.strategy=shuffle`, the operator data will be shuffled by all the keys. Use this expression when the compound key is unique but each key is not unique enough, so you will shuffle the data using all the keys of the shuffled operator.

When partitioning data with the shuffle strategy, the data load is shared on all cluster nodes. Each node processes one partition of the data. The default number of partitions is equal to the number of cluster nodes.

The partition number can be overridden by using the syntax `hint.num_partitions = total_partitions`, which will control the number of partitions. This is useful when the cluster has a small number of cluster nodes and the default partitions number will be small, and the query fails or takes a long execution time.

> [!NOTE]
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

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular source whose data is to be processed by the operator.|
| *DataExpression*| string | | An implicit or explicit tabular transformation expression.|
| *Query* | string | | A transformation expression run on the records of *T*.|
| *key*| string | | Use a `join` key, `summarize` key, `make-series` key or `partition` key.|
| *SubQuery*| string | | A transformation expression.|

> [!NOTE]
> Either *DataExpression* or *Query* must be specified depending on the chosen syntax.

## Examples

## Use summarize with shuffle

The `shuffle` strategy query with `summarize` operator will share the load on all cluster nodes, where each node will process one partition of the data.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAx3MMQ6AIAxA0d1TdJTEeANHB2ZOgFoBIyVpiwnGw2vc//tOC+f5QlLpHpCas+d0I8REOoqyVwwNJpBY9/1EWEsl7c0A/gq9paNyQrG0JcZVDSwNnH7me/3lC79aGLFfAAAA" target="_blank">Run the query</a>

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3WOwQrCMBBE7/2KJSeF4h94s6I3oYLn0GyblDQbkq0S8ONN0yJe3MPAMm93pmUKU/NEx7F6w0tjQGhZMoKWEcQDI4uvUbh78pt5tkRqcUcyDrRxfIgc8u2QjlHPfW8RKsizK9r+RC37n68XaazYAB9oxI6h8SaSwquq13I1nOQkB7wF8hg4FXxflNyK5FodzY4/vA+5oeEAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| where State has "West"
| where EventType has "Flood"
| join hint.strategy=shuffle 
    (
    StormEvents
    | where EventType has "Hail"
    | project EpisodeId, State, DamageProperty
    )
    on State
| count
```

**Output**

|Count|
|---|
|103|

## Use make-series with shuffle

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA2WOQQuCQBSE7/2KhyeFlFWRSPBW1wjsD2z4dBfd3Xj7LIR+fFt2iYY5zXwD07Ijc7yjZb95wkMhIbQsGUFJD9HJEasoNEaOmHokjR6Utpx5Nff9hCMu0HwXfjbxQRo54JncDYmXBDrs5TxxYAQ4+waJL9ogaAsk7YBxF6YckrgQYpeKPBiEqD/OxKpkC39YmUNR1tX+F8urLoHrsj56AR9yv8vdAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| where State has "North"
| make-series hint.shufflekey = State sum(DamageProperty) default = 0 on StartTime in range(datetime(2007-01-01 00:00:00.0000000), datetime(2007-01-31 23:59:00.0000000), 15d) by State
```

**Output**

|State|sum_DamageProperty|StartTime|
|---|---|---|---|
|NORTH DAKOTA|[60000,0,0]|["2006-12-31T00:00:00.0000000Z","2007-01-15T00:00:00.0000000Z","2007-01-30T00:00:00.0000000Z"]|
|NORTH CAROLINA|[20000,0,1000]|["2006-12-31T00:00:00.0000000Z","2007-01-15T00:00:00.0000000Z","2007-01-30T00:00:00.0000000Z"]|
|ATLANTIC NORTH|[0,0,0]|["2006-12-31T00:00:00.0000000Z","2007-01-15T00:00:00.0000000Z","2007-01-30T00:00:00.0000000Z"]|

### Use partition with shuffle

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22OsQ6DMBBDd77iRiohFuZuZeiGxBcEOCAV5KKLQYrUjyewsODBi+0ntxBd650dQvYnbxQWVhzN1qEMUAOe4jvM2zguTF2k2tsgA3+HjJLyyyGeqjP8mNVM3Kh4VsQrS1CVH/e4lwW1SNziqf5KL3rZHA7GAN74mQAAAA==" target="_blank">Run the query</a>

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0WNQQrCMBRE955ilhW0xC4UhLrRLrquFwjmt402PyX5VQQPb6iKwyxm8+Y14oOr7sQSFy88egqERnSQs3WEA4wWkjSzQqndWm1SodR+bq4+WSby6i3jZtmghGWmgN6y5FFCOuieZeynth0IWfMX4ic8aac7OgY/xqTcFt9beEY12ugN1WaFGapNkl38xPIGNG76e7oAAAA=" target="_blank">Run the query</a>

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22OzQrCQAyE7z5FjhW0xB4UBL1oD57XF1jY1K51s2U3KoIPb/wpvRjmEDJk5jMSU6hvxJInT7i3lAiM2CRHHwi24KyQ6FpUiKs5LlSAuP6oxO9M9fMcPUPn2cEGPDMlaD1Lmdtr01yoo4fe697n6Ojg/nlvBHUKMwLBALS3wZ5ol2KfFWlZ/Woh8pg5GyJe+VHdN9IAAAA=" target="_blank">Run the query</a>

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22OsQ6CMBRFd7/ijpAoqQyamOCiDMz4A9U+oGpb0j40GD/eBjUu3tzhDu/lnJqdN+WNLIfZE/eOPKFm6fmgDWELJZk4ziQXYr0Qy1gIsZmaiXfS+BkGY6TXD8LJDZaTFMcRZa+DU1SpOSZEpeLl2WmLi7YKBbS15NFpy1lgH1HtWIRuaJorIal/aviq7aWRLe2860OUW+UfATj7B/YCGrh7PdwAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| where StartTime > datetime(2007-01-01 00:00:00.0000000)
| summarize count() by EpisodeId, EventId
| join kind = inner hint.strategy=shuffle (StormEvents | where DamageCrops > 62000000) on EpisodeId, EventId
```

**Output**

|EpisodeId|EventId|...|EpisodeId1|EventId1|...|
|--|--|--|--|--|--|
|1030 |4407 |...| 1030 |4407|...|
|1030 |13721 |...| 1030 |13721|...|
|2477 |12530 |...| 2477 |12530|...|
|2103 |10237 |...| 2103 |10237|...|
|2103| 10239| ...| 2103 |10239|...|
|...|...|...|...|...|...|

To overcome this issue and run in shuffle strategy, choose the key which is common for the `summarize` and `join` operations. In this case, this key is `EpisodeId`. Use the hint `hint.shufflekey` to specify the shuffle key on the `join` to `hint.shufflekey = EpisodeId`:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA22OPQ7CMBSDd07hsZUAhQ4gIZUFOnQuFwjklYaSpEpeQUUcnqj8LVgePNjyV7HzpriS5TB54NaQJ1QsPe+1IWygJBPHmGRCrGZiEQ0h1qPn4qU0LkNvjPT6Tji63nKS4jCg6HRwiko1xXhRqtg8O23RaquQQ1tLHo22PA9NX9cXamnIvzMk1Q8PH7ydNPJEW++6EAGX2RsCzv45fALuk5Ra4AAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where StartTime > datetime(2007-01-01 00:00:00.0000000)
| summarize count() by EpisodeId, EventId
| join kind = inner hint.shufflekey=EpisodeId (StormEvents | where DamageCrops > 62000000) on EpisodeId, EventId
```

**Output**

|EpisodeId|EventId|...|EpisodeId1|EventId1|...|
|--|--|--|--|--|--|
|1030 |4407| ...| 1030 |4407| ...|
|1030 |13721| ...| 1030 |13721| ...|
|2477 |12530| ...| 2477 |12530| ...|
|2103 |10237| ...| 2103 |10237| ...|
|2103 |10239| ...| 2103 |10239| ...|
|...|...|...|...|...|...|

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
