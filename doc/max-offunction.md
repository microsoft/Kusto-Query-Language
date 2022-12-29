---
title: max_of() - Azure Data Explorer
description: This article describes max_of() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# max_of()

Returns the maximum value of several evaluated numeric expressions.

```kusto
max_of(10, 1, -3, 17) == 17
```

## Syntax

`max_of` `(`*expr_1*`,` *expr_2* ...`)`

## Arguments

* *expr_i*: A scalar expression, to be evaluated.

- All arguments must be of the same type.
- Maximum of 64 arguments is supported.
- Non-null values take precedence to null values.

## Returns

The maximum value of all argument expressions.

## Examples

Find the maximum value in an array: 

<!-- csl: https://help.kusto.windows.net/Samples  -->
```kusto
print result = max_of(10, 1, -3, 17) 
```

**Output**

|result|
|---|
|17|

Find the maximum value in a data-table. Non-null values take precedence over null values:

<!-- csl: https://help.kusto.windows.net/Samples  -->
```kusto
datatable (A:int, B:int)
[1, 6,
8, 1,
int(null), 2,
1, int(null),
int(null), int(null)]
| project max_of(A, B)
```

**Output**

|result|
|---|
|6|
|8| 
|2| 
|1|
|(null)|