---
title: bag_unpack plugin - Azure Data Explorer | Microsoft Docs
description: This article describes bag_unpack plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 02/07/2019
---
# bag_unpack plugin

The `bag_unpack` plugin unpacks a single column of type `dynamic`
by treating each property bag top-level slot as a column.

    T | evaluate bag_unpack(col1)

**Syntax**

*T* `|` `evaluate` `bag_unpack(` *Column* `,` [ *OutputColumnPrefix* ] `)`

**Arguments**

* *T*: The tabular input whose column *Column* is to be unpacked.
* *Column*: The column of *T* to unpack. Must be of type `dynamic`.
* *OutputColumnPrefix*: A common prefix to add to all columns produced by the plugin.
  Optional.

**Returns**

The `bag_unpack` plugin returns a table with as many records as its tabular
input (*T*). The schema of the table is the same as that of its tabular input with
the following modifications:

* The specified input column (*Column*) is removed.

* The schema is extended with as many columns as there are distinct slots in
  the top-level property bag values of *T*. The name of each column corresponds
  to the name of each slot, optionally prefixed by *OutputColumnPrefix*. Its
  type is either the type of the slot (if all values of the same slot have the
  same type), or `dynamic` (if the values differ in type).

**Remarks**

The plugin's output schema depends on the data values, making it as "unpredictable"
as the data itself. Therefore, multiple executions of the plugin with different
data input may produce different output schema.

The input data to the plugin must be such, that the output schema comply with
all the rules for a tabular schema. In particular:

1. An output column name cannot be the same as an existing column in the tabular
   input *T* unless it is the column to be unpacked (*Column*) itself,
   as that will produce two columns with the same name.

2. All slot names, when prefixed by *OutputColumnPrefix*, must be valid
   entity names and comply with the [identifier naming rules](./schema-entities/entity-names.md#identifier-naming-rules).

**Example**

```kusto
datatable(d:dynamic)
[
    dynamic({"Name": "John", "Age":20}),
    dynamic({"Name": "Dave", "Age":40}),
    dynamic({"Name": "Smitha", "Age":30}),
]
| evaluate bag_unpack(d)
```

|Name  |Age|
|------|---|
|John  |20 |
|Dave  |40 |
|Smitha|30 |