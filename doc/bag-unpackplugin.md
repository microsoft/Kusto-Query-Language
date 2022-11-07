---
title: bag_unpack plugin - Azure Data Explorer
description: Learn how to use the bag_unpack() function to unpack a dynamic column.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/03/2022
---
# bag_unpack plugin

The `bag_unpack` plugin unpacks a single column of type `dynamic`, by treating each property bag top-level slot as a column. The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Syntax

*T* `|` `evaluate` `bag_unpack(` *Column* [`,` *OutputColumnPrefix* ] [`,` *columnsConflict* ] [`,` *ignoredProperties* ] `)` [`:` *OutputSchema*]

## Parameters

| Name | Type | Required| Description |
|---|---|---|---|
| *T* |  | &check; | The tabular input whose column *Column* is to be unpacked. |
| *Column* | dynamic | &check; | The column of *T* to unpack. |
| *OutputColumnPrefix* | string | | A common prefix to add to all columns produced by the plugin. |
| *columnsConflict* | string | | A direction for column conflict resolution. Valid values: <br />`error` - Query produces an error (default)<br />`replace_source` - Source column is replaced<br />`keep_source` - Source column is kept
| *ignoredProperties* | dynamic | Optional set of bag properties to be ignored.
| *OutputSchema* | | | The names and types for the expected columns of the `bag_unpack` plugin output.<br /><br />**Syntax**: `(` *ColumnName* `:` *ColumnType* [`,` ...] `)`<br /><br />Specifying the expected schema optimizes query execution by not having to first run the actual query to explore the schema. An error is raised if the run-time schema doesn't match the *OutputSchema* schema. |

## Returns

The `bag_unpack` plugin returns a table with as many records as its tabular input (*T*). The schema of the table is the same as the schema of its tabular input with the following modifications:

* The specified input column (*Column*) is removed.
* The schema is extended with as many columns as there are distinct slots in
  the top-level property bag values of *T*. The name of each column corresponds
  to the name of each slot, optionally prefixed by *OutputColumnPrefix*. Its
  type is either the type of the slot, if all values of the same slot have the
  same type, or `dynamic`, if the values differ in type.

> [!NOTE]
> If the OutputSchema is not specified, the plugin's output schema varies according to the input data values. Therefore, multiple executions of the plugin using different data inputs, may produce different output schema.

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

|Age|Name   |
|---|-------|
|20 |John   |
|40 |Dave   |
|30 |Jasmine|

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

|Property_Age|Property_Name|
|------------|-------------|
|20          |John         |
|40          |Dave         |
|30          |Jasmine      |

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

|Age|Name   |
|---|-------|
|20 |John   |
|40 |Dave   |
|30 |Jasmine|
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

|Age|Name     |
|---|---------|
|20 |Old_name |
|40 |Old_name |
|30 |Old_name |

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

### Expand a bag with a query-defined OutputSchema

Expand a bag and use the `OutputSchema` option to allow various optimizations to be evaluated before running the actual query.

```kusto
datatable(d:dynamic)
[
    dynamic({"Name": "John", "Age":20}),
    dynamic({"Name": "Dave", "Age":40}),
    dynamic({"Name": "Jasmine", "Age":30}),
]
| evaluate bag_unpack(d) : (Name:string, Age:long)
```

|Name  |Age  |
|---------|---------|
|John     |  20  |
|Dave     |  40  |
|Jasmine  |  30  |
