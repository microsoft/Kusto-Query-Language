---
title:  innerunique join
description: Learn how to use the innerunique join flavor to merge the rows of two tables. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 06/19/2023
---

# innerunique join

The `innerunique` join flavor removes duplicate keys from the left side. This behavior ensures that the output contains a row for every combination of unique left and right keys.

By default, the `innerunique` join flavor is used if the `kind` parameter isn't specified. This default implementation is useful in log/trace analysis scenarios, where you aim to correlate two events based on a shared correlation ID. It allows you to retrieve all instances of the phenomenon while disregarding duplicate trace records that contribute to the correlation.

## Syntax

*LeftTable* `|` `join` `kind=innerunique` [ *Hints* ] *RightTable* `on` *Conditions*

[!INCLUDE [join-parameters-attributes-hints](../../includes/join-parameters-attributes-hints.md)]

## Returns

**Schema**: All columns from both tables, including the matching keys.  
**Rows**: All deduplicated rows from the left table that match rows from the right table.

## Examples

### Use the default innerunique join

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVGIULBVSEksAcKknFQN79RKq+KSosy8dB2FsMSc0lRDq5z8vHRNrmguBSBQT1TXMdSBMJPUdYwQTGMoM1ldx4Qr1porB2h0JH6jjVCNBhpiaIAwxQiJbQxjpwBNNwAZH6FQo5CVn5kHtCM/TwFoNADeA/cxywAAAA==" target="_blank">Run the query</a>

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

**Output**

|Key|Value1|Key1|Value2|
|---|---|---|---|
|b|2|b|10|
|c|4|c|20|
|c|4|c|30|

> [!NOTE]
> The keys 'a' and 'd' don't appear in the output, since there were no matching keys on both left and right sides.

The query executed the default join, which is an inner join after deduplicating the left side based on the join key. The deduplication keeps only the first record. The resulting left side of the join after deduplication is:

|Key |Value1
|---|---
|a |1
|b |2
|c |4

### Two possible outputs from innerunique join

> [!NOTE]
> The `innerunique` join flavor may yield two possible outputs and both are correct.
> In the first output, the join operator randomly selected the first key that appears in t1, with the value "val1.1" and matched it with t2 keys.
> In the second output, the join operator randomly selected the second key that appears in t1, with the value "val1.2" and matched it with t2 keys.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA5WNQQvCMAyF7/0Vj54mFKHV08RfIh4mC6OupDjTwcAfb1aGoDcTSOC9vC+JBOJxRt+J9i1RM9LSImUeHOYuFWrxlCnysAMMtC51egertt976zbjIwWryvVk0goP/8F/WQfrvoXjBhdvXrjnyBgj9/okMtNUOD4K1YSEurIe0PIGa/tJfOgAAAA=" target="_blank">Run the query</a>

```kusto
let t1 = datatable(key: long, value: string)  
    [
    1, "val1.1",  
    1, "val1.2"  
];
let t2 = datatable(key: long, value: string)  
    [  
    1, "val1.3",
    1, "val1.4"  
];
t1
| join kind = innerunique
    t2
    on key
```

**Output**

|key|value|key1|value1|
|---|---|---|---|
|1|val1.1|1|val1.3|
|1|val1.1|1|val1.4|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA5WNQQvCMAyF7/0Vj54mFKHV08RfIh4mC6OupDjTwcAfb1aGoDcTSOC9vC+JBOJxRt+J9i1RM9LSImUeHOYuFWrxlCnysAMMtC51egertt976zbjIwWryvVk0goP/8F/WQfFfyvHjS7evHDPkTFG7vVLZKapcHwUqgkJdWU9oOUNScAQaekAAAA=" target="_blank">Run the query</a>

```kusto
let t1 = datatable(key: long, value: string)  
    [
    1, "val1.1",  
    1, "val1.2"  
];
let t2 = datatable(key: long, value: string)  
    [  
    1, "val1.3", 
    1, "val1.4"  
];
t1
| join kind = innerunique
    t2
    on key
```

**Output**

|key|value|key1|value1|
|---|---|---|---|
|1|val1.2|1|val1.3|
|1|val1.2|1|val1.4|

* Kusto is optimized to push filters that come after the `join`, towards the appropriate join side, left or right, when possible.
* Sometimes, the flavor used is **innerunique** and the filter is propagated to the left side of the join. The flavor is automatically propagated and the keys that apply to that filter appear in the output.
* Use the previous example and add a filter `where value == "val1.2" `. It gives the second result and will never give the first result for the datasets:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA5WOwQrCMAyG73uKn54UitDqadInEQ+ThVlXUqytMvDhjXUM9GYCCfxJvj+BMrKBQ99lyVOg1UhTixB50Lh3oVCLW06ehzXQQOJQq9FQMjYbo/Q8WCSrRDnum/CG2//gv6yt4L+V3UzPpnniEj1j9NyLi2emVNhfC9WLbGuLskCT7D7OlOhjC+eWX18MNThdAwEAAA==" target="_blank">Run the query</a>

```kusto
let t1 = datatable(key: long, value: string)  
    [
    1, "val1.1",  
    1, "val1.2"  
];
let t2 = datatable(key: long, value: string)  
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

**Output**

|key|value|key1|value1|
|---|---|---|---|
|1|val1.2|1|val1.3|
|1|val1.2|1|val1.4|

### Get extended sign-in activities

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

## See also

* Learn about other [join flavors](joinoperator.md#returns)
