---
title:  leftouter join
description: Learn how to use the leftouter join flavor to merge the rows of two tables. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 06/18/2023
---

# leftouter join

The `leftouter` join flavor returns all the records from the left side table and only matching records from the right side table.

## Syntax

*LeftTable* `|` `join` `kind=leftouter` [ *Hints* ] *RightTable* `on` *Conditions*

[!INCLUDE [join-parameters-attributes-hints](../../includes/join-parameters-attributes-hints.md)]

## Returns

**Schema**: All columns from both tables, including the matching keys.  
**Rows**: All records from the left table and only matching rows from the right table.

## Example

The result of a left outer join for tables X and Y always contains all records of the left table (X), even if the join condition doesn't find any matching record in the right table (Y).

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
X | join kind=leftouter Y on Key
```

**Output**

|Key|Value1|Key1|Value2|
|---|---|---|---|
|a|1|||
|b|2|b|10|
|b|3|b|10|
|c|4|c|20|
|c|4|c|30|

## See also

* Learn about other [join flavors](joinoperator.md#returns)
