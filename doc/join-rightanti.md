---
title:  rightanti join
description: Learn how to use the rightanti join flavor to merge the rows of two tables. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 06/18/2023
---

# rightanti join

The `rightanti` join flavor returns all records from the left side that don't match any record from the right side. The anti join models the "NOT IN" query.

> **Alias**: `rightantisemi`

## Syntax

*LeftTable* `|` `join` `kind=rightanti` [ *Hints* ] *RightTable* `on` *Conditions*

[!INCLUDE [join-parameters-attributes-hints](../../includes/join-parameters-attributes-hints.md)]

## Returns

**Schema**: All columns from the right table.  
**Rows**: All records from the right table that don't match records from the left table.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVGIULBVSEksAcKknFQN79RKq+KSosy8dB2FsMSc0lRDq5z8vHRNrmguBSBQT1TXMdSBMJPUdYwQTGMoM1ldx4Qr1porB2h0JH6jjVCNBhpiaIAwxQiJbQxjpwBNNwAZH6FQo5CVn5mnkJ2Zl2JblJmeUZKYV5IJtDI/TwFoEwBGWDfq2gAAAA==" target="_blank">Run the query</a>

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

**Output**

|Key|Value1|
|---|---|
|a|1|
