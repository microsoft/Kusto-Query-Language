---
title: project-away operator - Azure Data Explorer | Microsoft Docs
description: This article describes project-away operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 06/22/2019
---
# project-away operator

Select what columns in the input to exclude from the output

```kusto
T | project-away price, quantity, zz*
```

The order of the columns in the result is determined by their original order in the table. Only the columns that were specified as arguments are dropped. The other columns are included in the result.  (See also `project`.)

**Syntax**

*T* `| project-away` *ColumnNameOrPattern* [`,` ...]

**Arguments**

* *T*: The input table
* *ColumnNameOrPattern:* The name of the column or column wildcard-pattern to be removed from the output.

**Returns**

A table with columns that were not named as arguments. Contains same number of rows as the input table.

**Tips**

* Use [`project-rename`](projectrenameoperator.md) if your intention is to rename columns.
* Use [`project-reorder`](projectreorderoperator.md) if your intention is to reorder columns.

* You can `project-away` any columns that are present in the original table or that were computed as part of the query.


**Examples**

The input table `T` has three columns of type `long`: `A`, `B`, and `C`.

```kusto
datatable(A:long, B:long, C:long) [1, 2, 3]
| project-away C    // Removes column C from the output
```

|A|B|
|---|---|
|1|2|

Removing columns starting with 'a'.

```kusto
print  a2='a2', b = 'b', a3='a3', a1='a1'
|  project-away a* 
```

|b|
|---|
|b|

