---
title: schema_merge plugin - Azure Data Explorer
description: This article describes schema_merge plugin in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 03/16/2020
---
# schema_merge plugin

Merges tabular schema definitions into unified schema. 

Schema definitions are expected to be in the format produced by the [`getschema`](./getschemaoperator.md) operator.

The `schema merge` operation joins columns in input schemas and tries to reduce
data types to common ones. If data types can't be reduced, an error is displayed on the problematic column.

```kusto
let Schema1=Table1 | getschema;
let Schema2=Table2 | getschema;
union Schema1, Schema2 | evaluate schema_merge()
```

## Syntax

`T` `|` `evaluate` `schema_merge(` *PreserveOrder* `)`

## Arguments

* *PreserveOrder*: (Optional) When set to `true`, directs the plugin to validate the column order as defined by the first tabular schema that is kept. If the same column is in several schemas, the column ordinal must be like the column ordinal of the first schema that it appeared in. Default value is `true`.

## Returns

The `schema_merge` plugin returns output similar to what [`getschema`](./getschemaoperator.md) operator returns.

## Examples

Merge with a schema that has a new column appended.

```kusto
let schema1 = datatable(Uri:string, HttpStatus:int)[] | getschema;
let schema2 = datatable(Uri:string, HttpStatus:int, Referrer:string)[] | getschema;
union schema1, schema2 | evaluate schema_merge()
```

*Result*

|ColumnName | ColumnOrdinal | DataType | ColumnType|
|---|---|---|---|
|Uri|0|System.String|string|
|HttpStatus|1|System.Int32|int|
|Referrer|2|System.String|string|

Merge with a schema that has different column ordering (`HttpStatus` ordinal changes from `1` to `2` in the new variant).

```kusto
let schema1 = datatable(Uri:string, HttpStatus:int)[] | getschema;
let schema2 = datatable(Uri:string, Referrer:string, HttpStatus:int)[] | getschema;
union schema1, schema2 | evaluate schema_merge()
```

*Result*

|ColumnName | ColumnOrdinal | DataType | ColumnType|
|---|---|---|---|
|Uri|0|System.String|string|
|Referrer|1|System.String|string|
|HttpStatus|-1|ERROR(unknown CSL type:ERROR(columns are out of order))|ERROR(columns are out of order)|

Merge with a schema that has different column ordering, but with `PreserveOrder` set to `false`.

```kusto
let schema1 = datatable(Uri:string, HttpStatus:int)[] | getschema;
let schema2 = datatable(Uri:string, Referrer:string, HttpStatus:int)[] | getschema;
union schema1, schema2 | evaluate schema_merge(PreserveOrder = false)
```

*Result*

|ColumnName | ColumnOrdinal | DataType | ColumnType|
|---|---|---|---|
|Uri|0|System.String|string
|Referrer|1|System.String|string
|HttpStatus|2|System.Int32|int|
