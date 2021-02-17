---
title: bag_unpack plugin - Azure Data Explorer
description: This article describes bag_unpack plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 06/15/2020
---
# bag_unpack plugin

The `bag_unpack` plugin unpacks a single column of type `dynamic`, by treating each property bag top-level slot as a column.

```kusto
T | evaluate bag_unpack(col1)
```

## Syntax

*T* `|` `evaluate` `bag_unpack(` *Column* [`,` *OutputColumnPrefix* ] [`,` *columnsConflict* ] [`,` *ignoredProperties* ] `)`

## Arguments

* *T*: The tabular input whose column *Column* is to be unpacked.
* *Column*: The column of *T* to unpack. Must be of type `dynamic`.
* *OutputColumnPrefix*: A common prefix to add to all columns produced by the plugin. This argument is optional.
* *columnsConflict*: A direction for column conflict resolution. This argument is optional. When argument is provided, it's expected to be a string literal matching one of the following values:
    - `error` - Query produces an error (default)
    - `replace_source` - Source column is replaced
    - `keep_source` - Source column is kept
* *ignoredProperties*: Optional set of bag properties to be ignored. When argument is provided, it's expected to be a constant of `dynamic` array with one or more string literals.

## Returns

The `bag_unpack` plugin returns a table with as many records as its tabular input (*T*). The schema of the table is the same as the schema of its tabular input with the following modifications:

* The specified input column (*Column*) is removed.
* The schema is extended with as many columns as there are distinct slots in
  the top-level property bag values of *T*. The name of each column corresponds
  to the name of each slot, optionally prefixed by *OutputColumnPrefix*. Its
  type is either the type of the slot, if all values of the same slot have the
  same type, or `dynamic`, if the values differ in type.

> [!NOTE]
> The plugin's output schema depends on the data values, making it as "unpredictable"
> as the data itself. Multiple executions of the plugin, using different
> data inputs, may produce different output schema.

> [!NOTE]
> The input data to the plugin must be such that the output schema follows all the rules for a tabular schema. In particular:
>
> * An output column name can't be the same as an existing column in the tabular
    input *T*, unless it's the column to be unpacked (*Column*), since that will produce two columns with the same name.
>
> * All slot names, when prefixed by *OutputColumnPrefix*, must be valid
    entity names and follow the [identifier naming rules](./schema-entities/entity-names.md#identifier-naming-rules).

## Examples

### Expand a bag


<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(d:dynamic)
[
    dynamic({"Name": "John", "Age":20}),
    dynamic({"Name": "Dave", "Age":40}),
    dynamic({"Name": "Jasmine", "Age":30}),
]
| evaluate bag_unpack(d)
```

|Name  |Age|
|------|---|
|John  |20 |
|Dave  |40 |
|Jasmine|30 |


### Expand a bag with OutputColumnPrefix

Expand a bag and use the `OutputColumnPrefix` option to produce column names that begin with the prefix 'Property_'.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(d:dynamic)
[
    dynamic({"Name": "John", "Age":20}),
    dynamic({"Name": "Dave", "Age":40}),
    dynamic({"Name": "Jasmine", "Age":30}),
]
| evaluate bag_unpack(d, 'Property_')
```

|Property_Name|Property_Age|
|---|---|
|John|20|
|Dave|40|
|Jasmine|30|

### Expand a bag with columnsConflict

Expand a bag and use the `columnsConflict` option to resolve conflicts between existing columns and columns produced by the `bag_unpack()` operator.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(Name:string, d:dynamic)
[
    'Old_name', dynamic({"Name": "John", "Age":20}),
    'Old_name', dynamic({"Name": "Dave", "Age":40}),
    'Old_name', dynamic({"Name": "Jasmine", "Age":30}),
]
| evaluate bag_unpack(d, columnsConflict='replace_source') // Use new name
```

|Name|Age|
|---|---|
|John|20|
|Dave|40|
|Jasmine|30|

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(Name:string, d:dynamic)
[
    'Old_name', dynamic({"Name": "John", "Age":20}),
    'Old_name', dynamic({"Name": "Dave", "Age":40}),
    'Old_name', dynamic({"Name": "Jasmine", "Age":30}),
]
| evaluate bag_unpack(d, columnsConflict='keep_source') // Keep old name
```

|Name|Age|
|---|---|
|Old_name|20|
|Old_name|40|
|Old_name|30|

### Expand a bag with ignoredProperties

Expand a bag and use the `ignoredProperties` option to ignore certain properties in the property bag.

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
datatable(d:dynamic)
[
    dynamic({"Name": "John", "Age":20, "Address": "Address-1" }),
    dynamic({"Name": "Dave", "Age":40, "Address": "Address-2"}),
    dynamic({"Name": "Jasmine", "Age":30, "Address": "Address-3"}),
]
// Ignore 'Age' and 'Address' properties
| evaluate bag_unpack(d, ignoredProperties=dynamic(['Address', 'Age']))
```

|Name|
|---|
|John|
|Dave|
|Jasmine|
