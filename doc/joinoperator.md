---
title: join operator - Azure Data Explorer
description: This article describes join operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 03/30/2020
ms.localizationpriority: high 
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# join operator

Merge the rows of two tables to form a new table by matching values of the specified columns from each table.

```kusto
Table1 | join (Table2) on CommonColumn, $left.Col1 == $right.Col2
```

## Syntax

*LeftTable* `|` `join` [*JoinParameters*] `(` *RightTable* `)` `on` *Attributes*

## Arguments

* *LeftTable*: The **left** table or tabular expression, sometimes called **outer** table, whose rows are to be merged. Denoted as `$left`.

* *RightTable*: The **right** table or tabular expression, sometimes called **inner** table, whose rows are to be merged. Denoted as `$right`.

* *Attributes*: One or more comma-separated **rules** that describe how rows from
  *LeftTable* are matched to rows from *RightTable*. Multiple rules are evaluated using the `and` logical operator.

  A **rule** can be one of:

  |Rule kind        |Syntax          |Predicate    |
  |-----------------|--------------|-------------------------|
  |Equality by name |*ColumnName*    |`where` *LeftTable*.*ColumnName* `==` *RightTable*.*ColumnName*|
  |Equality by value|`$left.`*LeftColumn* `==` `$right.`*RightColumn*|`where` `$left.`*LeftColumn* `==` `$right.`*RightColumn*       |

    > [!NOTE]
    > For 'equality by value', the column names *must* be qualified with the applicable owner table denoted by `$left` and `$right` notations.

* *JoinParameters*: Zero or more space-separated parameters in the form of
  *Name* `=` *Value* that control the behavior of the row-match operation and execution plan. The following parameters are supported:

    ::: zone pivot="azuredataexplorer"

    |Parameters name           |Values                                        |Description                                  |
    |---------------|----------------------------------------------|---------------------------------------------|
    |`kind`         |Join flavors|See [Join Flavors](#join-flavors)|
    |`hint.remote`  |`auto`, `left`, `local`, `right`              |See [Cross-Cluster Join](joincrosscluster.md)|
    |`hint.strategy`|Execution hints                               |See [Join hints](#join-hints)                |

    ::: zone-end

    ::: zone pivot="azuremonitor"

    |Name           |Values                                        |Description                                  |
    |---------------|----------------------------------------------|---------------------------------------------|
    |`kind`         |Join flavors|See [Join Flavors](#join-flavors)|
    |`hint.remote`  |`auto`, `left`, `local`, `right`              |                                             |
    |`hint.strategy`|Execution hints                               |See [Join hints](#join-hints)                |

    ::: zone-end

> [!WARNING]
> If `kind` isn't specified, the default join flavor is `innerunique`. This is different than some other analytics products that have `inner` as the default flavor.  See [join-flavors](#join-flavors) to understand the differences and make sure  the query yields the intended results.

## Returns

**The output schema depends on the join flavor:**

| Join flavor | Output schema |
|---|---|
|`kind=leftanti`, `kind=leftsemi`| The result table contains columns from the left side only.|
| `kind=rightanti`, `kind=rightsemi` | The result table contains columns from the right side only.|
|  `kind=innerunique`, `kind=inner`, `kind=leftouter`, `kind=rightouter`, `kind=fullouter` |  A column for every column in each of the two tables, including the matching keys. The columns of the right side will be automatically renamed if there are name clashes. |
   
**Output records depend on the join flavor:**

   > [!NOTE]
   > If there are several rows with the same values for those fields, you'll get rows for all the combinations.
   > A match is a row selected from one table that has the same value for all the `on` fields as a row in the other table.

| Join flavor | Output records |
|---|---|
|`kind=leftanti`, `kind=leftantisemi`| Returns all the records from the left side that don't have matches from the right|
| `kind=rightanti`, `kind=rightantisemi`| Returns all the records from the right side that don't have matches from the left.|
| `kind` unspecified, `kind=innerunique`| Only one row from the left side is matched for each value of the `on` key. The output contains a row for each match of this row with rows from the right.|
| `kind=leftsemi`| Returns all the records from the left side that have matches from the right. |
| `kind=rightsemi`| Returns all the records from the right side that have matches from the left. |
|`kind=inner`| Contains a row in the output for every combination of matching rows from left and right. |
| `kind=leftouter` (or `kind=rightouter` or `kind=fullouter`)| Contains a row for every row on the left and right, even if it has no match. The unmatched output cells contain nulls. |

> [!TIP]
> For best performance, if one table is always smaller than the other, use it as the left (piped) side of the join.

## Example

Get extended activities from a `login` that some entries mark as the start and end of an activity.

```kusto
let Events = MyLogTable | where type=="Event" ;
Events
| where Name == "Start"
| project Name, City, ActivityId, StartTime=timestamp
| join (Events
    | where Name == "Stop"
        | project StopTime=timestamp, ActivityId)
    on ActivityId
| project City, ActivityId, StartTime, StopTime, Duration = StopTime - StartTime
```

```kusto
let Events = MyLogTable | where type=="Event" ;
Events
| where Name == "Start"
| project Name, City, ActivityIdLeft = ActivityId, StartTime=timestamp
| join (Events
        | where Name == "Stop"
        | project StopTime=timestamp, ActivityIdRight = ActivityId)
    on $left.ActivityIdLeft == $right.ActivityIdRight
| project City, ActivityId, StartTime, StopTime, Duration = StopTime - StartTime
```

## Join flavors

The exact flavor of the join operator is specified with the *kind* keyword. The following flavors of the join operator are supported:

|Join kind/flavor|Description|
|--|--|
|[`innerunique`](#default-join-flavor) (or empty as default)|Inner join with left side deduplication|
|[`inner`](#inner-join-flavor)|Standard inner join|
|[`leftouter`](#left-outer-join-flavor)|Left outer join|
|[`rightouter`](#right-outer-join-flavor)|Right outer join|
|[`fullouter`](#full-outer-join-flavor)|Full outer join|
|[`leftanti`](#left-anti-join-flavor), [`anti`](#left-anti-join-flavor), or [`leftantisemi`](#left-anti-join-flavor)|Left anti join|
|[`rightanti`](#right-anti-join-flavor) or [`rightantisemi`](#right-anti-join-flavor)|Right anti join|
|[`leftsemi`](#left-semi-join-flavor)|Left semi join|
|[`rightsemi`](#right-semi-join-flavor)|Right semi join|

### Default join flavor

The default join flavor is an inner join with left side deduplication. Default join implementation is useful in typical log/trace analysis scenarios where you want to correlate two events, each matching some filtering criterion, under the same correlation ID. You want to get back all appearances of the phenomenon, and ignore multiple appearances of the contributing trace records.

``` 
X | join Y on Key
 
X | join kind=innerunique Y on Key
```

The following two sample tables are used to explain the operation of the join.

**Table X**

|Key |Value1
|---|---
|a |1
|b |2
|b |3
|c |4

**Table Y**

|Key |Value2
|---|---
|b |10
|c |20
|c |30
|d |40

The default join does an inner join after deduplicating the left side on the join key (deduplication keeps the first record).

Given this statement: `X | join Y on Key`

the effective left side of the join, table X after deduplication, would be:

|Key |Value1
|---|---
|a |1
|b |2
|c |4

and the result of the join would be:

```kusto
let X = datatable(Key:string, Value1:long)
[
    'a',1,
    'b',2,
    'b',3,
    'c',4
];
let Y = datatable(Key:string, Value2:long)
[
    'b',10,
    'c',20,
    'c',30,
    'd',40
];
X | join Y on Key
```

|Key|Value1|Key1|Value2|
|---|---|---|---|
|b|2|b|10|
|c|4|c|20|
|c|4|c|30|

> [!NOTE]
> The keys 'a' and 'd' don't appear in the output, since there were no matching keys on both left and right sides.

### Inner-join flavor

The inner-join function is like the standard inner-join from the SQL world. An output record is produced whenever a record on the left side has the same join key as the record on the right side.

```kusto
let X = datatable(Key:string, Value1:long)
[
    'a',1,
    'b',2,
    'b',3,
    'c',4
];
let Y = datatable(Key:string, Value2:long)
[
    'b',10,
    'c',20,
    'c',30,
    'd',40
];
X | join kind=inner Y on Key
```

|Key|Value1|Key1|Value2|
|---|---|---|---|
|b|3|b|10|
|b|2|b|10|
|c|4|c|20|
|c|4|c|30|

> [!NOTE]
> * (b,10) from the right side, was joined twice: with both (b,2) and (b,3) on the left.
> * (c,4) on the left side, was joined twice: with both (c,20) and (c,30) on the right.

### Innerunique-join flavor
 
Use **innerunique-join flavor** to deduplicate keys from the left side. The result will be a row in the output from every combination of deduplicated left keys and right keys.

> [!NOTE]
> **innerunique flavor** may yield two possible outputs and both are correct.
    In the first output, the join operator randomly selected the first key that appears in t1, with the value "val1.1" and matched it with t2 keys.
    In the second output, the join operator randomly selected the second key that appears in t1, with the value "val1.2" and matched it with t2 keys.

```kusto
let t1 = datatable(key:long, value:string)  
[
1, "val1.1",  
1, "val1.2"  
];
let t2 = datatable(key:long, value:string)  
[  
1, "val1.3",
1, "val1.4"  
];
t1
| join kind = innerunique
    t2
on key
```

|key|value|key1|value1|
|---|---|---|---|
|1|val1.1|1|val1.3|
|1|val1.1|1|val1.4|

```kusto
let t1 = datatable(key:long, value:string)  
[
1, "val1.1",  
1, "val1.2"  
];
let t2 = datatable(key:long, value:string)  
[  
1, "val1.3", 
1, "val1.4"  
];
t1
| join kind = innerunique
    t2
on key
```

|key|value|key1|value1|
|---|---|---|---|
|1|val1.2|1|val1.3|
|1|val1.2|1|val1.4|

* Kusto is optimized to push filters that come after the `join`, towards the appropriate join side, left or right, when possible.

* Sometimes, the flavor used is **innerunique** and the filter is propagated to the left side of the join. The flavor will be automatically propagated and the keys that apply to that filter will always appear in the output.
    
* Use the example above and add a filter `where value == "val1.2" `. It will always give the second result and will never give the first result for the datasets:

```kusto
let t1 = datatable(key:long, value:string)  
[
1, "val1.1",  
1, "val1.2"  
];
let t2 = datatable(key:long, value:string)  
[  
1, "val1.3", 
1, "val1.4"  
];
t1
| join kind = innerunique
    t2
on key
| where value == "val1.2"
```

|key|value|key1|value1|
|---|---|---|---|
|1|val1.2|1|val1.3|
|1|val1.2|1|val1.4|

### Left outer-join flavor

The result of a left outer-join for tables X and Y always contains all records of the left table (X), even if the join condition doesn't find any matching record in the right table (Y).

```kusto
let X = datatable(Key:string, Value1:long)
[
    'a',1,
    'b',2,
    'b',3,
    'c',4
];
let Y = datatable(Key:string, Value2:long)
[
    'b',10,
    'c',20,
    'c',30,
    'd',40
];
X | join kind=leftouter Y on Key
```

|Key|Value1|Key1|Value2|
|---|---|---|---|
|b|3|b|10|
|b|2|b|10|
|c|4|c|20|
|c|4|c|30|
|a|1|||

### Right outer-join flavor

The right outer-join flavor resembles the left outer-join, but the treatment of the tables is reversed.

```kusto
let X = datatable(Key:string, Value1:long)
[
    'a',1,
    'b',2,
    'b',3,
    'c',4
];
let Y = datatable(Key:string, Value2:long)
[
    'b',10,
    'c',20,
    'c',30,
    'd',40
];
X | join kind=rightouter Y on Key
```

|Key|Value1|Key1|Value2|
|---|---|---|---|
|b|3|b|10|
|b|2|b|10|
|c|4|c|20|
|c|4|c|30|
|||d|40|

### Full outer-join flavor

A full outer-join combines the effect of applying both left and right outer-joins. Whenever records in the joined tables don't match, the result set will have `null` values for every column of the table that lacks a matching row. For those records that do match, a single row will be produced in the result set, containing fields populated from both tables.

```kusto
let X = datatable(Key:string, Value1:long)
[
    'a',1,
    'b',2,
    'b',3,
    'c',4
];
let Y = datatable(Key:string, Value2:long)
[
    'b',10,
    'c',20,
    'c',30,
    'd',40
];
X | join kind=fullouter Y on Key
```

|Key|Value1|Key1|Value2|
|---|---|---|---|
|b|3|b|10|
|b|2|b|10|
|c|4|c|20|
|c|4|c|30|
|||d|40|
|a|1|||

### Left anti-join flavor

Left anti-join returns all records from the left side that don't match any record from the right side.

```kusto
let X = datatable(Key:string, Value1:long)
[
    'a',1,
    'b',2,
    'b',3,
    'c',4
];
let Y = datatable(Key:string, Value2:long)
[
    'b',10,
    'c',20,
    'c',30,
    'd',40
];
X | join kind=leftanti Y on Key
```

|Key|Value1|
|---|---|
|a|1|

> [!NOTE]
> Anti-join models the "NOT IN" query.

### Right anti-join flavor

Right anti-join returns all records from the right side that don't match any record from the left side.

```kusto
let X = datatable(Key:string, Value1:long)
[
    'a',1,
    'b',2,
    'b',3,
    'c',4
];
let Y = datatable(Key:string, Value2:long)
[
    'b',10,
    'c',20,
    'c',30,
    'd',40
];
X | join kind=rightanti Y on Key
```

|Key|Value2|
|---|---|
|d|40|

> [!NOTE]
> Anti-join models the "NOT IN" query.

### Left semi-join flavor

Left semi-join returns all records from the left side that match a record from the right side. Only columns from the left side are returned.

```kusto
let X = datatable(Key:string, Value1:long)
[
    'a',1,
    'b',2,
    'b',3,
    'c',4
];
let Y = datatable(Key:string, Value2:long)
[
    'b',10,
    'c',20,
    'c',30,
    'd',40
];
X | join kind=leftsemi Y on Key
```

|Key|Value1|
|---|---|
|b|3|
|b|2|
|c|4|

### Right semi-join flavor

Right semi-join returns all records from the right side that match a record from the left side. Only columns from the right side are returned.

```kusto
let X = datatable(Key:string, Value1:long)
[
    'a',1,
    'b',2,
    'b',3,
    'c',4
];
let Y = datatable(Key:string, Value2:long)
[
    'b',10,
    'c',20,
    'c',30,
    'd',40
];
X | join kind=rightsemi Y on Key
```

|Key|Value2|
|---|---|
|b|10|
|c|20|
|c|30|

### Cross-join

Kusto doesn't natively provide a cross-join flavor. You can't mark the operator with the `kind=cross`.
To simulate, use a dummy key.

`X | extend dummy=1 | join kind=inner (Y | extend dummy=1) on dummy`

## Join hints

The `join` operator supports a number of hints that control the way a query runs.
These hints don't change the semantic of `join`, but may affect its performance.

Join hints are explained in the following articles:

* `hint.shufflekey=<key>` and `hint.strategy=shuffle` - [shuffle query](shufflequery.md)
* `hint.strategy=broadcast` - [broadcast join](broadcastjoin.md)
* `hint.remote=<strategy>` - [cross-cluster join](joincrosscluster.md)
