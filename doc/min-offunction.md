---
title: min_of() - Azure Data Explorer
description: This article describes min_of() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/13/2020
---
# min_of()

Returns the minimum value of several evaluated numeric expressions.

```kusto
min_of(10, 1, -3, 17) == -3
```

## Syntax

`min_of` `(`*expr_1*`,` *expr_2* ...`)`

## Arguments

* *expr_i*: A scalar expression, to be evaluated.

- All arguments must be of the same type.
- Maximum of 64 arguments is supported.
- Non-null values take precedence to null values.

## Returns

The minimum value of all argument expressions.

## Examples

Find the maximum value in an array: 

<!-- csl: https://help.kusto.windows.net/Samples  -->
```kusto
print result=min_of(10, 1, -3, 17) 
```

**Output**

|result|
|---|
|-3|

Find the minimum value in a data-table. Non-null values take precedence over null values:

<!-- csl: https://help.kusto.windows.net/Samples  -->
```kusto
datatable (A:int, B:int)
[5, 2,
10, 1,
int(null), 3,
1, int(null),
int(null), int(null)]
| project min_of(A, B)
```

**Output**

|result|
|---|
|2|
|1|
|3| 
|1| 
|(null) |