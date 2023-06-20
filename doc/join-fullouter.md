---
title:  fullouter join
description: Learn how to use the fullouter join flavor to merge the rows of two tables. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 06/18/2023
---

# fullouter join

A `fullouter` join combines the effect of applying both left and right outer-joins. For columns of the table that lack a matching row, the result set contains `null` values. For those records that do match, a single row is produced in the result set containing fields populated from both tables.

## Syntax

*LeftTable* `|` `join` `kind=fullouter` [ *Hints* ] *RightTable* `on` *Conditions*

[!INCLUDE [join-parameters-attributes-hints](../../includes/join-parameters-attributes-hints.md)]

## Returns

**Schema**: All columns from both tables, including the matching keys.  
**Rows**: All records from both tables with unmatched cells populated with null.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVGIULBVSEksAcKknFQN79RKq+KSosy8dB2FsMSc0lRDq5z8vHRNrmguBSBQT1TXMdSBMJPUdYwQTGMoM1ldx4Qr1porB2h0JH6jjVCNBhpiaIAwxQiJbQxjpwBNNwAZH6FQo5CVn5mnkJ2Zl2Kbk5pWkl9akloEtDI/TwFoEwD4dnPs2gAAAA==" target="_blank">Run the query</a>

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

**Output**

|Key|Value1|Key1|Value2|
|---|---|---|---|
|a|1|||
|b|2|b|10|
|b|3|b|10|
|c|4|c|20|
|c|4|c|30|
|||d|40|

## See also

* Learn about other [join flavors](joinoperator.md#returns)
