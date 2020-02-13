# Shuffle query

Shuffle query is a semantic-preserving transformation for a set of operators that supports shuffle strategy that depending on the actual data can yield considerably better performance.

Operators that supports shuffling in Kusto are [join](joinoperator.md), [summarize](summarizeoperator.md) and [make-series](make-seriesoperator.md).

Shuffle query strategy can be set by the query parameter `hint.strategy = shuffle` or `hint.shufflekey = <key>`.

**Syntax**

<!-- csl -->
```
T | where Event=="Start" | project ActivityId, Started=Timestamp
| join hint.strategy = shuffle (T | where Event=="End" | project ActivityId, Ended=Timestamp)
  on ActivityId
| extend Duration=Ended - Started
| summarize avg(Duration)
```

<!-- csl -->
```
T
| summarize hint.strategy = shuffle count(), avg(price) by supplier
```

<!-- csl -->
```
T
| make-series hint.shufflekey = Fruit PriceAvg=avg(Price) default=0  on Purchase from datetime(2016-09-10) to datetime(2016-09-13) step 1d by Supplier, Fruit
```

This strategy will share the load on all cluster nodes where each node will process one partition of the data.
It is useful to use the shuffle query strategy when the key (`join` key, `summarize` key or `make-series` key) has a high cardinality causing the regular query strategy to hit query limits.

**Difference between hint.strategy=shuffle and hint.shufflekey = key**

`hint.strategy=shuffle` means that the shuffled operator will be shuffled by all the keys.
For example, in this query :

<!-- csl -->
```
T | where Event=="Start" | project ActivityId, Started=Timestamp
| join hint.strategy = shuffle (T | where Event=="End" | project ActivityId, Ended=Timestamp)
  on ActivityId, ProcessId
| extend Duration=Ended - Started
| summarize avg(Duration)
```

The hash function that shuffles the data will use both keys ActivityId and ProcessId.

The query above is equivalent to :

<!-- csl -->
```
T | where Event=="Start" | project ActivityId, Started=Timestamp
| join hint.shufflekey = ActivityId hint.shufflekey = ProcessId (T | where Event=="End" | project ActivityId, Ended=Timestamp)
  on ActivityId, ProcessId
| extend Duration=Ended - Started
| summarize avg(Duration)
```
This hint can be used when you are interested in shuffling the data by all the keys of the shuffled operator because the compound key is too unique but each key is not unique enough.
When the shuffled operator has other shufflable operators like `summarize` or `join`, the query becomes more complex and then hint.strategy=shuffle will not be applied.

for example :

<!-- csl -->
```
T
| where Event=="Start"
| project ActivityId, Started=Timestamp, numeric_column
| summarize count(), numeric_column = any(numeric_column) by ActivityId
| join
    hint.strategy = shuffle (T
    | where Event=="End"
    | project ActivityId, Ended=Timestamp, numeric_column
)
on ActivityId, numeric_column
| extend Duration=Ended - Started
| summarize avg(Duration)
```

In this case, if we do apply the `hint.strategy=shuffle` (instead of ignoring the strategy during query-planning) and shuffle the data by the compound key [`ActivityId`, `numeric_column`] the result will not be correct.
The `summarize` which is on the left side of the `join` groubs by a subset of the `join` keys which is `ActivityId`. It means that the `summarize` will group by the key `ActivityId` while the data is partitioned by the compound key [`ActivityId`, `numeric_column`].
Shuffling by the compound key [`ActivityId`, `numeric_column`] doesn't mean that it is a valid shuffling for the key ActivityId and the results may be incorrect.

This example simplifies this assuming that the hash function used for a compound key is `binary_xor(hash(key1, 100) , hash(key2, 100))`

<!-- csl -->
```

datatable(ActivityId:string, NumericColumn:long)
[
"activity1", 2,
"activity1" ,1,
]
| extend hash_by_key = binary_xor(hash(ActivityId, 100) , hash(NumericColumn, 100))
```

|ActivityId|NumericColumn|hash_by_key|
|---|---|---|
|activity1|2|56|
|activity1|1|65|



As you see the compound key for both records was mapped to different partitions 56 and 65 but these two records has the same value of `ActivityId` which means the the `summarize` on the left side of the `join` which
expects similar values of the column `ActivityId` to be in the same partition will defintely produce wrong results.

In this case, `hint.shufflekey` solves this issue by specifying the shuffle key on the join to `hint.shufflekey = ActivityId` which is a common key for all shuffelable operators.
In this case, the shuffling is safe, both `join` and `summarize` shuffles by the same key so all similar values will defintely be in the same partition the results are correct :

<!-- csl -->
```
T
| where Event=="Start"
| project ActivityId, Started=Timestamp, numeric_column
| summarize count(), numeric_column = any(numeric_column) by ActivityId
| join
    hint.shufflekey = ActivityId (T
    | where Event=="End"
    | project ActivityId, Ended=Timestamp, numeric_column
)
on ActivityId, numeric_column
| extend Duration=Ended - Started
| summarize avg(Duration)
```

|ActivityId|NumericColumn|hash_by_key|
|---|---|---|
|activity1|2|56|
|activity1|1|65|

In shuffle query, the default partitions number is the cluster nodes number. This number can be overriden by using the syntax `hint.num_partitions = total_partitions` which will control the number of partitions.

This hint is useful when the cluster has a small number of cluster nodes where the default partitions number will be small too and the query still fails or takes long execution time.

Note that setting many partitions may degrade performance and consume more cluster resources so it is recommended to choose the partition number carefully (starting with the hint.strategy = shuffle and start increasing the partitions gradually).

**Examples**

The following example shows how shuffle `summarize` improves performance considerably.

The source table has 150M records and the cardinality of the group by key is 10M which is spread over 10 cluster nodes.

Running the regular `summarize` strategy, the query ends after 1:08 and the memory usage peak is ~3GB:

<!-- csl -->
```
orders
| summarize arg_max(o_orderdate, o_totalprice) by o_custkey 
| where o_totalprice < 1000
| count
```
|Count|
|---|
|1086|

While using shuffle `summarize` strategy, the query ends after ~7 seconds and the memory usage peak is 0.43GB:

<!-- csl -->
```
orders
| summarize hint.strategy = shuffle arg_max(o_orderdate, o_totalprice) by o_custkey 
| where o_totalprice < 1000
| count
```
|Count|
|---|
|1086|

The following example shows the improvement on a cluster which has 2 cluster nodes, the table has 60M records and the cardinality of the group by key is 2M.

Running the query without `hint.num_partitions` will use only 2 partitions (as cluster nodes number) and the following query will take ~1:10 mins :

<!-- csl -->
```
lineitem	
| summarize hint.strategy = shuffle dcount(l_comment), dcount(l_shipdate) by l_partkey 
| consume
```
setting partitions number to 10, the query will end after 23 seconds: 

<!-- csl -->
```
lineitem	
| summarize hint.strategy = shuffle hint.num_partitions = 10 dcount(l_comment), dcount(l_shipdate) by l_partkey 
| consume
```

The following example shows how shuffle `join` improves performance considerably.

The examples were sampled on a cluster with 10 nodes where the data is spread over all these nodes.

The left table has 15M records where the cardinality of the `join` key is ~14M, The right side of the `join` is with 150M records and the cardinality of the `join` key is 10M.
Running the regular strategy of the `join`, the query ends after ~28 seconds and the memory usage peak is 1.43GB :

<!-- csl-->
```
customer
| join
    orders
on $left.c_custkey == $right.o_custkey
| summarize sum(c_acctbal) by c_nationkey
```

While using shuffle `join` strategy, the query ends after ~4 seconds and the memory usage peak is 0.3GB :

<!-- csl-->
```
customer
| join
    hint.strategy = shuffle orders
on $left.c_custkey == $right.o_custkey
| summarize sum(c_acctbal) by c_nationkey
```

Trying the same queries on a larger dataset where left side of the `join` is 150M and the cardinality of the key is 148M, Right side of the `join` is 1.5B and the cardinality of the key is ~100M.

The query with the default `join` strategy hits kusto limits and times-out after 4 mins.
While using shuffle `join` strategy, the query ends after ~34 seconds and the memory usage peak is 1.23GB.


The following example shows the improvement on a cluster which has 2 cluster nodes, the table has 60M records and the cardinality of the `join` key is 2M.
Running the query without `hint.num_partitions` will use only 2 partitions (as cluster nodes number) and the following query will take ~1:10 mins :

<!-- csl -->
```
lineitem
| summarize dcount(l_comment), dcount(l_shipdate) by l_partkey
| join
    hint.shufflekey = l_partkey   part
on $left.l_partkey == $right.p_partkey
| consume
```
setting partitions number to 10, the query will end after 23 seconds: 

<!-- csl -->
```
lineitem
| summarize dcount(l_comment), dcount(l_shipdate) by l_partkey
| join
    hint.shufflekey = l_partkey  hint.num_partitions = 10    part
on $left.l_partkey == $right.p_partkey
| consume
```
