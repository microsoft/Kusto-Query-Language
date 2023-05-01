---
title: schema_merge plugin - Azure Data Explorer
description: Learn how to use the schema_merge plugin to merge tabular schema definitions into a unified schema.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/22/2023
---
# schema_merge plugin

Merges tabular schema definitions into a unified schema.

Schema definitions are expected to be in the format produced by the [`getschema`](./getschemaoperator.md) operator.

The `schema merge` operation joins columns in input schemas and tries to reduce
data types to common ones. If data types can't be reduced, an error is displayed on the problematic column.

The plugin is invoked with the [`evaluate`](evaluateoperator.md) operator.

## Syntax

`T` `|` `evaluate` `schema_merge(`*PreserveOrder*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *PreserveOrder* | bool | | When set to `true`, directs the plugin to validate the column order as defined by the first tabular schema that is kept. If the same column is in several schemas, the column ordinal must be like the column ordinal of the first schema that it appeared in. Default value is `true`.|

## Returns

The `schema_merge` plugin returns output similar to what [`getschema`](./getschemaoperator.md) operator returns.

## Examples

Merge with a schema that has a new column appended.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEoTs5IzU00VLBVSEksAcKknFSN0KJMq+KSosy8dB0Fj5KSgmCgRGmxVWZeiWZ0rEKNQnpqCUSbNVcO3AgjIo3QUQhKTUstKkotgirAMLM0LzM/D+YwHbjxNQqpZYk5pYklqVCh+NzUovRUDU0ALOh/occAAAA=" target="_blank">Run the query</a>

```kusto
let schema1 = datatable(Uri:string, HttpStatus:int)[] | getschema;
let schema2 = datatable(Uri:string, HttpStatus:int, Referrer:string)[] | getschema;
union schema1, schema2 | evaluate schema_merge()
```

**Output**

|ColumnName | ColumnOrdinal | DataType | ColumnType|
|---|---|---|---|
|Uri|0|System.String|string|
|HttpStatus|1|System.Int32|int|
|Referrer|2|System.String|string|

Merge with a schema that has different column ordering (`HttpStatus` ordinal changes from `1` to `2` in the new variant).

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEoTs5IzU00VLBVSEksAcKknFSN0KJMq+KSosy8dB0Fj5KSgmCgRGmxVWZeiWZ0rEKNQnpqCUSbNVcO3AgjnEYEpaalFhWlFhFrZmleZn4ezGE6cONrFFLLEnNKE0tSoULxualF6akamgBdra59xwAAAA==" target="_blank">Run the query</a>

```kusto
let schema1 = datatable(Uri:string, HttpStatus:int)[] | getschema;
let schema2 = datatable(Uri:string, Referrer:string, HttpStatus:int)[] | getschema;
union schema1, schema2 | evaluate schema_merge()
```

**Output**

|ColumnName | ColumnOrdinal | DataType | ColumnType|
|---|---|---|---|
|Uri|0|System.String|string|
|Referrer|1|System.String|string|
|HttpStatus|-1|ERROR(unknown CSL type:ERROR(columns are out of order))|ERROR(columns are out of order)|

Merge with a schema that has different column ordering, but with `PreserveOrder` set to `false`.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA42OsQrCQBBEe79iywTSaBmxt1MUKxHZmMl5cLnI7l6qfHwO1HSCTDfDPF6AkT6e6HlNO2rZcpqA4iK+VhMfXUV7s9c5D0lrH6283mgiB3vftquwIDY/ESd0EIH8y0zRD/ErVi34iTBySGz4VPce4lAcBQoZcZAWkiU6DopyBn2W6PrcAAAA" target="_blank">Run the query</a>

```kusto
let schema1 = datatable(Uri:string, HttpStatus:int)[] | getschema;
let schema2 = datatable(Uri:string, Referrer:string, HttpStatus:int)[] | getschema;
union schema1, schema2 | evaluate schema_merge(PreserveOrder = false)
```

**Output**

|ColumnName | ColumnOrdinal | DataType | ColumnType|
|---|---|---|---|
|Uri|0|System.String|string
|Referrer|1|System.String|string
|HttpStatus|2|System.Int32|int|
