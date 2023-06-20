---
title:  inner join
description: Learn how to use the inner join flavor to merge the rows of two tables. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 06/18/2023
---

# inner join

The `inner` join flavor is like the standard inner join from the SQL world. An output record is produced whenever a record on the left side has the same join key as the record on the right side.

## Syntax

*LeftTable* `|` `join` `kind=inner` [ *Hints* ] *RightTable* `on` *Conditions*

[!INCLUDE [join-parameters-attributes-hints](../../includes/join-parameters-attributes-hints.md)]

## Returns

**Schema**: All columns from both tables, including the matching keys.  
**Rows**: Only matching rows from both tables.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVGIULBVSEksAcKknFQN79RKq+KSosy8dB2FsMSc0lRDq5z8vHRNrmguBSBQT1TXMdSBMJPUdYwQTGMoM1tdxxTKTFbXMeGKtebKAdoSid8WI1RbgOYZGiBMMUJiG8PYKUDTDZAsNQBZFaFQo5CVn5mnkJ2Zl2KbmZeXWgS0Oj9PAWgjAEho/dHtAAAA" target="_blank">Run the query</a>

```kusto
let X = datatable(Key:string, Value1:long)
[
    'a',1,
    'b',2,
    'b',3,
    'k',5,
    'c',4
];
let Y = datatable(Key:string, Value2:long)
[
    'b',10,
    'c',20,
    'c',30,
    'd',40,
    'k',50
];
X | join kind=inner Y on Key
```

**Output**

|Key|Value1|Key1|Value2|
|---|---|---|---|
|b|3|b|10|
|b|2|b|10|
|c|4|c|20|
|c|4|c|30|
|k|5|k|50|

> [!NOTE]
>
> * (b,10) from the right side, was joined twice: with both (b,2) and (b,3) on the left.
> * (c,4) on the left side, was joined twice: with both (c,20) and (c,30) on the right.
> * (k,5) from the left and (k, 50) from the right was joined once.

## See also

* Learn about other [join flavors](joinoperator.md#returns)
