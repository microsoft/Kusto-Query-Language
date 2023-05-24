---
title:  bag_unpack plugin
description: Learn how to use the bag_unpack plugin to unpack a dynamic column.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
---
# bag_unpack plugin

The `bag_unpack` plugin unpacks a single column of type `dynamic`, by treating each property bag top-level slot as a column. The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Syntax

*T* `|` `evaluate` `bag_unpack(` *Column* [`,` *OutputColumnPrefix* ] [`,` *columnsConflict* ] [`,` *ignoredProperties* ] `)` [`:` *OutputSchema*]

## Parameters

| Name | Type | Required| Description |
|---|---|---|---|
| *T* | string | &check; | The tabular input whose column *Column* is to be unpacked. |
| *Column* | dynamic | &check; | The column of *T* to unpack. |
| *OutputColumnPrefix* | string | | A common prefix to add to all columns produced by the plugin. |
| *columnsConflict* | string | | The direction for column conflict resolution. Valid values: <br />`error` - Query produces an error (default)<br />`replace_source` - Source column is replaced<br />`keep_source` - Source column is kept
| *ignoredProperties* | dynamic | An optional set of bag properties to be ignored.
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
> If the *OutputSchema* is not specified, the plugin's output schema varies according to the input data values. Therefore, multiple executions of the plugin using different data inputs, may produce different output schema.

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

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjRSrlMq8xNzMZE2uaC4FIIByNaqV/BJzU5WsFJS88jPylHQUlBzTgVwjg1pNHRwKXRLLUuEKTfAo9Eoszs3MQ6g1BquN5apRSC1LzClNLElVSEpMjy/NK0hMztZI0QQABlsx468AAAA=" target="_blank">Run the query</a>

```kusto
datatable(d:dynamic)
[
    dynamic({"Name": "John", "Age":20}),
    dynamic({"Name": "Dave", "Age":40}),
    dynamic({"Name": "Jasmine", "Age":30}),
]
| evaluate bag_unpack(d)
```

**Output**

|Age|Name   |
|---|-------|
|20 |John   |
|40 |Dave   |
|30 |Jasmine|

### Expand a bag with OutputColumnPrefix

Expand a bag and use the `OutputColumnPrefix` option to produce column names that begin with the prefix 'Property_'.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjRSrlMq8xNzMZE2uaC4FIIByNaqV/BJzU5WsFJS88jPylHQUlBzTgVwjg1pNHRwKXRLLUuEKTfAo9Eoszs3MQ6g1BquN5apRSC1LzClNLElVSEpMjy/NK0hMztZI0VFQDyjKL0gtKqmMV9cEAG0gI1O8AAAA" target="_blank">Run the query</a>

```kusto
datatable(d:dynamic)
[
    dynamic({"Name": "John", "Age":20}),
    dynamic({"Name": "Dave", "Age":40}),
    dynamic({"Name": "Jasmine", "Age":30}),
]
| evaluate bag_unpack(d, 'Property_')
```

**Output**

|Property_Age|Property_Name|
|------------|-------------|
|20          |John         |
|40          |Dave         |
|30          |Jasmine      |

### Expand a bag with columnsConflict

Expand a bag and use the `columnsConflict` option to resolve conflicts between existing columns and columns produced by the `bag_unpack()` operator.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA43NsQrCMBAG4L1PcWRJC4WKOhUcRKcOOjmJlGty1mKalCapiPrupgq69m66/+fjJLqwlaJ4hy3l1vWNrlOQubxrbBuRRMcIwvC9kmVIiIfyW8UPNhqWAyvMRbMU2LoO53z2StIJaIsD/dByIirQto3+u8XHnaIn0IDKoyOosC697lBcY5mCMMq32m6MPqtGuBXvqVMoqLTG94J4AlkGB0ug6Qbj2zdNIgveEgEAAA==" target="_blank">Run the query</a>

```kusto
datatable(Name:string, d:dynamic)
[
    'Old_name', dynamic({"Name": "John", "Age":20}),
    'Old_name', dynamic({"Name": "Dave", "Age":40}),
    'Old_name', dynamic({"Name": "Jasmine", "Age":30}),
]
| evaluate bag_unpack(d, columnsConflict='replace_source') // Use new name
```

**Output**

|Age|Name   |
|---|-------|
|20 |John   |
|40 |Dave   |
|30 |Jasmine|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA43NzwqCQBAG8LtPMexlFQSjOgkdok4G9QARMu5OJu4fcV0hqndvLairM6f5Pn6MxCFspSg+oqbcDX1j6hRkLu8GdSOS6BxBGH5SsgwJ8VB+q/jBJsNyYIW9GZYC29bhXC5eSToD7XGkH1rPRAU63Zi/W33cJXoCjag8DgQV1qU3HYo2likIq7w2bmfNVTVi2PCWqCud9b0gnkCWwSEEYJWE6ekb9wh0nRABAAA=" target="_blank">Run the query</a>

```kusto
datatable(Name:string, d:dynamic)
[
    'Old_name', dynamic({"Name": "John", "Age":20}),
    'Old_name', dynamic({"Name": "Dave", "Age":40}),
    'Old_name', dynamic({"Name": "Jasmine", "Age":30}),
]
| evaluate bag_unpack(d, columnsConflict='keep_source') // Keep old name
```

**Output**

|Age|Name     |
|---|---------|
|20 |Old_name |
|40 |Old_name |
|30 |Old_name |

### Expand a bag with ignoredProperties

Expand a bag and use the `ignoredProperties` option to ignore certain properties in the property bag.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3XOPQ+CMBAG4J1fcekCJDUoOJE4mLjIYNwJMQe9IBEKaYHEqP/d8iFOtMt76dsnJ7A1Ny3JEaF4SqyKzLViC8yZR+fFLlgRC4FF9V0yDuyYm9HfDkkIRVoPj3Pc7Bh8XL4inLCnRdivCD5bByLUVSH/RrBiBKORWJ4H51zWisA2fRtQCpOmkg2NqhtSbUHaegP1WHbYEqSY3zrZYPZwBIdi/C6uS/Xw2ypeID7piet+AWW2HHFQAQAA" target="_blank">Run the query</a>

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

**Output**

|Name|
|---|
|John|
|Dave|
|Jasmine|

### Expand a bag with a query-defined OutputSchema

Expand a bag and use the `OutputSchema` option to allow various optimizations to be evaluated before running the actual query.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjRSrlMq8xNzMZE2uaC4FIIByNaqV/BJzU5WsFJS88jPylHQUlBzTgVwjg1pNHRwKXRLLUuEKTfAo9Eoszs3MQ6g1BquN5apRSC1LzClNLElVSEpMjy/NK0hMztZI0VSwUtAA6bUqLinKzEvXUQDqssrJz0vXBADtklvGyQAAAA==" target="_blank">Run the query</a>

```kusto
datatable(d:dynamic)
[
    dynamic({"Name": "John", "Age":20}),
    dynamic({"Name": "Dave", "Age":40}),
    dynamic({"Name": "Jasmine", "Age":30}),
]
| evaluate bag_unpack(d) : (Name:string, Age:long)
```

**Output**

|Name  |Age  |
|---------|---------|
|John     |  20  |
|Dave     |  40  |
|Jasmine  |  30  |
