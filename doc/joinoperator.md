# join operator

Merge the rows of two tables to form a new table by matching values of the specified column(s) from each table.

<!-- csl -->
```
Table1 | join (Table2) on CommonColumn, $left.Col1 == $right.Col2
```

**Syntax**

*LeftTable* `|` `join` [*JoinParameters*] `(` *RightTable* `)` `on` *Attributes*

**Arguments**

* *LeftTable*: The **left** table or tabular expression (sometimes called **outer** table) whose rows are to be merged. Denoted as `$left`.

* *RightTable*: The **right** table or tabular expression (sometimes called **inner* table) whose rows are to be merged. Denoted as `$right`.

* *Attributes*: One or more (comma-separated) rules that describe how rows from
  *LeftTable* are matched to rows from *RightTable*. Multiple rules are evaluated using the `and` logical operator.
  A rule can be one of:

  |Rule kind        |Syntax                                          |Predicate                                                      |
  |-----------------|------------------------------------------------|---------------------------------------------------------------|
  |Equality by name |*ColumnName*                                    |`where` *LeftTable*.*ColumnName* `==` *RightTable*.*ColumnName*|
  |Equality by value|`$left.`*LeftColumn* `==` `$right.`*RightColumn*|`where` `$left.`*LeftColumn* `==` `$right.`*RightColumn*       |

> [!NOTE]
> In case of 'equality by value', the column names *must* be qualified with the applicable owner table denoted by `$left` and `$right` notations.

* *JoinParameters*: Zero or more (space-separated) parameters in the form of
  *Name* `=` *Value* that control the behavior
  of the row-match operation and execution plan. The following parameters are supported: 

::: zone pivot="azuredataexplorer"

  |Name           |Values                                        |Description                                  |
  |---------------|----------------------------------------------|---------------------------------------------|
  |`kind`         |Join flavors|See [Join Flavors](#join-flavors)|                                             |
  |`hint.remote`  |`auto`, `left`, `local`, `right`              |See [Cross-Cluster Join](joincrosscluster.md)|
  |`hint.strategy`|Execution hints                               |See [Join hints](#join-hints)                |

::: zone-end

::: zone pivot="azuremonitor"

  |Name           |Values                                        |Description                                  |
  |---------------|----------------------------------------------|---------------------------------------------|
  |`kind`         |Join flavors|See [Join Flavors](#join-flavors)|                                             |
  |`hint.remote`  |`auto`, `left`, `local`, `right`              |                                             |
  |`hint.strategy`|Execution hints                               |See [Join hints](#join-hints)                |

::: zone-end


> [!WARNING]
> The default join flavor, if `kind` is not specified, is `innerunique`. This is different than some other
> analytics products, which have `inner` as the default flavor. Please read carefully [below](#join-flavors)
> to understand the differences between the different kinds and to make sure the query yields the intended results.

**Returns**

Output schema depends on the join flavor:

 * `kind=leftanti`, `kind=leftsemi`:

     The result table contains columns from the left side only.

     
 * `kind=rightanti`, `kind=rightsemi`:

     The result table contains columns from the right side only.

     
*  `kind=innerunique`, `kind=inner`, `kind=leftouter`, `kind=rightouter`, `kind=fullouter`

     A column for every column in each of the two tables, including the matching keys. The columns of the right side will be automatically renamed if there are name clashes.

     
Output records depends on the join flavor:

 * `kind=leftanti`, `kind=leftantisemi`

     Returns all the records from the left side that don't have matches from the right.     
     
 * `kind=rightanti`, `kind=rightantisemi`

     Returns all the records from the right side that don't have matches from the left.  
      
*  `kind=innerunique`, `kind=inner`, `kind=leftouter`, `kind=rightouter`, `kind=fullouter`, `kind=leftsemi`, `kind=rightsemi`

    A row for every match between the input tables. A match is a row selected from one table that has the same value for all the `on` fields as a row in the other table with these constraints:

   - `kind` unspecified, `kind=innerunique`

    Only one row from the left side is matched for each value of the `on` key. The output contains a row for each match of this row with rows from the right.
    
   - `kind=leftsemi`
   
    Returns all the records from the left side that have matches from the right.
    
   - `kind=rightsemi`
   
       Returns all the records from the right side that have matches from the left.

   - `kind=inner`
 
    There's a row in the output for every combination of matching rows from left and right.

   - `kind=leftouter` (or `kind=rightouter` or `kind=fullouter`)

    In addition to the inner matches, there's a row for every row on the left (and/or right), even if it has no match. In that case, the unmatched output cells contain nulls.
    If there are several rows with the same values for those fields, you'll get rows for all the combinations.

 

**Tips**

For best performance:

* If one table is always smaller than the other, use it as the left (piped) side of the join.

**Example**

Get extended activities from a log in which some entries mark the start and end of an activity. 

<!-- csl -->
```
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

<!-- csl -->
```
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

[More about this example](./samples.md#activities).

## Join flavors

The exact flavor of the join operator is specified with the kind keyword. As of today, Kusto
supports the following flavors of the join operator: 

|Join kind|Description|
|--|--|
|[`innerunique`](#default-join-flavor) (or empty as default)|Inner join with left side deduplication|
|[`inner`](#inner-join)|Standard inner join|
|[`leftouter`](#left-outer-join)|Left outer join|
|[`rightouter`](#right-outer-join)|Right outer join|
|[`fullouter`](#full-outer-join)|Full outer join|
|[`leftanti`](#left-anti-join), [`anti`](#left-anti-join) or [`leftantisemi`](#left-anti-join)|Left anti join|
|[`rightanti`](#right-anti-join) or [`rightantisemi`](#right-anti-join)|Right anti join|
|[`leftsemi`](#left-semi-join)|Left semi join|
|[`rightsemi`](#right-semi-join)|Right semi join|

### inner and innerunique join operator flavors

-    When using **inner join flavor**, there will be a row in the output for every combination of matching rows from left and right without left keys deduplications. The output will be a cartesian product of left and right keys.
    Example of **inner join**:

<!-- csl-->
```
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
| join kind = inner
    t2
on key
```

|key|value|key1|value1|
|---|---|---|---|
|1|val1.2|1|val1.3|
|1|val1.1|1|val1.3|
|1|val1.2|1|val1.4|
|1|val1.1|1|val1.4|

-    Using **innerunique join flavor** will deduplicate keys from left side and there will be a row in the output from every combination of deduplicated left keys and right keys.
    Example of **innerunique join** for the same datasets used above, Please note that **innerunique flavor** for this case may yield two possible outputs and both are correct.
    In the first output, the join operator randomly picked the first key which appears in t1 with the value "val1.1" and matched it with t2 keys while in the second one, the join operator randomly picked the second key appears in t1 which has the value "val1.2" and matched it with t2 keys:

<!-- csl-->
```
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

<!-- csl-->
```
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


-    Kusto is optimized to push filters that come after the `join` towards the appropriate join side left or right when possible.
    Sometimes, when the flavor used is **innerunique** and the filter can be propagated to the left side of the join, then it will be propagated automatically and the keys which applies to that filter will always appear in the output.
    for example, using the example above and adding filter ` where value == "val1.2" ` will always give the second result and will never give the first result for the used datasets :

<!-- csl-->
```
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
 
### Default join flavor
    
    X | join Y on Key
    X | join kind=innerunique Y on Key
     
Let's use two sample tables to explain the operation of the join: 
 
Table X 

|Key |Value1 
|---|---
|a |1 
|b |2 
|b |3 
|c |4 

Table Y 

|Key |Value2 
|---|---
|b |10 
|c |20 
|c |30 
|d |40 
 
The default join performs an inner join after de-duplicating the left side on the join key (deduplication retains the first record). 
Given this statement: 

    X | join Y on Key 

the effective left side of the join (table X after de-duplication) would be: 

|Key |Value1 
|---|---
|a |1 
|b |2 
|c |4 

and the result of the join would be: 

<!-- csl -->
```
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


(Note that the keys 'a' and 'd' don't appear in the output, since there were no matching keys on both left and right sides). 
 
(Historically, this was the first implementation of the join supported by the initial version of Kusto; it is useful in the typical log/trace analysis scenarios where we want to correlate two events (each matching some filtering criterion) under the same correlation ID, and get back all appearances of the phenomenon we're looking for, ignoring multiple appearances of the contributing trace records.)
 
### Inner join

This is the standard inner join as known from the SQL world. Output record is produced whenever a record on the left side has the same join key as the record on the right side. 
 
<!-- csl -->
```
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

Note that (b,10) coming from the right side was joined twice: with both (b,2) and (b,3) on the left; also (c,4) on the left was joined twice: with both (c,20) and (c,30) on the right. 

### Left outer join 

The result of a left outer join for tables X and Y always contains all records of the left table (X), even if the join condition does not find any matching record in the right table (Y). 
 
<!-- csl -->
```
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

 
### Right outer join 

Resembles the left outer join, but the treatment of the tables is reversed. 
 
<!-- csl -->
```
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

 
### Full outer join 

Conceptually, a full outer join combines the effect of applying both left and right outer joins. Where records in the joined tables don't match, the result set will have NULL values for every column of the table that lacks a matching row. For those records that do match, a single row will be produced in the result set (containing fields populated from both tables). 
 
<!-- csl -->
```
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

 
### Left anti join

Left anti join returns all records from the left side that don't match any record from the right side. 
 
<!-- csl -->
```
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

Anti-join models the "NOT IN" query. 

### Right anti join

Right anti join returns all records from the right side that don't match any record from the left side. 
 
<!-- csl -->
```
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

Anti-join models the "NOT IN" query. 

### Left semi join

Left semi join returns all records from the left side that match a record from the right side. Only columns from the left side are returned. 

<!-- csl -->
```
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

### Right semi join

Right semi join returns all records from the right side that match a record from the left side. Only columns from the right side are returned. 

<!-- csl -->
```
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


### Cross join

Kusto doesn't natively provide a cross-join flavor (i.e., you can't mark the operator with `kind=cross`).
It isn't difficult to simulate this, however, by coming up with a dummy key:

    X | extend dummy=1 | join kind=inner (Y | extend dummy=1) on dummy

## Join hints

The `join` operator supports a number of hints that control the way a query executes.
These do not change the semantic of `join`, but may affect its performance.

Join hints explained in the following articles: 
* `hint.shufflekey=<key>` and `hint.strategy=shuffle` - [shuffle query](shufflequery.md)
* `hint.strategy=broadcast` - [broadcast join](broadcastjoin.md)
* `hint.remote=<strategy>` - [cross-cluster join](joincrosscluster.md)
