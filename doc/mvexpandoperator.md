---
title: mv-expand operator - Azure Data Explorer
description: This article describes mv-expand operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/24/2019
ms.localizationpriority: high
---
# mv-expand operator

Expands multi-value dynamic arrays or property bags into multiple records.

`mv-expand` can be described as the opposite of the aggregation operators
that pack multiple values into a single [dynamic](./scalar-data-types/dynamic.md)-typed
array or property bag, such as `summarize` ... `make-list()` and `make-series`.
Each element in the (scalar) array or property bag generates a new record in the
output of the operator. All columns of the input that aren't expanded are duplicated to all the records in the output.

## Syntax

*T* `| mv-expand ` [`bagexpansion=`(`bag` | `array`)] [`with_itemindex=`*IndexColumnName*] *ColumnName* [`to typeof(` *Typename*`)`] [`,` *ColumnName* ...] [`limit` *Rowlimit*]

*T* `| mv-expand ` [`bagexpansion=`(`bag` | `array`)] *Name* `=` *ArrayExpression* [`to typeof(`*Typename*`)`] [, [*Name* `=`] *ArrayExpression* [`to typeof(`*Typename*`)`] ...] [`limit` *Rowlimit*]

## Arguments

* *ColumnName*, *ArrayExpression*: A column reference, or a scalar expression, with a value
  of type `dynamic`, that holds an array or a property bag. The individual top-level elements
  of the array or property bag get expanded into multiple records.<br>
  When *ArrayExpression* is used and *Name* doesn't equal any input column name,
  the expanded value is extended into a new column in the output.
  Otherwise, the existing *ColumnName* is replaced.

* *Name:* A name for the new column.

* *Typename:* Indicates the underlying type of the array's elements, which becomes the type of the column produced by the `mv-expand` operator. The operation of applying type is cast-only and doesn't include parsing or type-conversion. Array elements that don't conform with the declared type will become `null` values.

* *RowLimit:* The maximum number of rows generated from each original row. The default is 2147483647. 

  > [!NOTE]
  > `mvexpand` is a legacy and obsolete form of the operator `mv-expand`. The legacy version has a default row limit of 128.

* *IndexColumnName:* If `with_itemindex` is specified, the output will include another column (named *IndexColumnName*), which contains the index (starting at 0) of the item in the original expanded collection. 

## Returns

For each record in the input, the operator returns zero, one, or many records in the output,
as determined in the following way:

1. Input columns that aren't expanded appear in the output with their original value.
   If a single input record is expanded into multiple output records, the value is duplicated
   to all records.

1. For each *ColumnName* or *ArrayExpression* that is expanded, the number of output records
   is determined for each value as explained [below](#modes-of-expansion). For each input record, the maximum number of output records is calculated. All arrays or property bags are expanded "in parallel"
   so that missing values (if any) are replaced by null values. Elements are expanded into rows in the order that they appear in the original array/bag.

1. If the dynamic value is null, then a single record is produced for that value (null).
   If the dynamic value is an empty array or property bag, no record is produced for that value.
   Otherwise, as many records are produced as there are elements in the dynamic value.

The expanded columns are of type `dynamic`, unless they're explicitly typed
by using the `to typeof()` clause.

### Modes of expansion

Two modes of property bag expansions are supported:

* `bagexpansion=bag` or `kind=bag`: Property bags are expanded into single-entry property bags. This mode is the default mode.
* `bagexpansion=array` or `kind=array`: Property bags are expanded into two-element `[`*key*`,`*value*`]` array structures, allowing uniform access to keys and values. This mode also allows, for example, running a distinct-count aggregation over property names. 

## Examples

### Single Column

A simple expansion of a single column:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
 ```kusto
datatable (a:int, b:dynamic)[1,dynamic({"prop1":"a", "prop2":"b"})]
| mv-expand b 
```

|a|b|
|---|---|
|1|{"prop1":"a"}|
|1|{"prop2":"b"}|

### Zipped two columns

Expanding two columns will first 'zip' the applicable columns and then expand them:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
datatable (a:int, b:dynamic, c:dynamic)[1,dynamic({"prop1":"a", "prop2":"b"}), dynamic([5, 4, 3])]
| mv-expand b, c
```

|a|b|c|
|---|---|---|
|1|{"prop1":"a"}|5|
|1|{"prop2":"b"}|4|
|1||3|

### Cartesian product of two columns

If you want to get a Cartesian product of expanding two columns, expand one after the other:

<!-- csl: https://kuskusdfv3.kusto.windows.net/Kuskus -->
```kusto
datatable (a:int, b:dynamic, c:dynamic)
  [
  1,
  dynamic({"prop1":"a", "prop2":"b"}),
  dynamic([5, 6])
  ]
| mv-expand b
| mv-expand c
```

|a|b|c|
|---|---|---|
|1|{  "prop1": "a"}|5|
|1|{  "prop1": "a"}|6|
|1|{  "prop2": "b"}|5|
|1|{  "prop2": "b"}|6|

### Convert output

To force the output of an mv-expand to a certain type (default is dynamic), use `to typeof`:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
datatable (a:string, b:dynamic, c:dynamic)["Constant", dynamic([1,2,3,4]), dynamic([6,7,8,9])]
| mv-expand b, c to typeof(int)
| getschema 
```

ColumnName|ColumnOrdinal|DateType|ColumnType
-|-|-|-
a|0|System.String|string
b|1|System.Object|dynamic
c|2|System.Int32|int

Notice column `b` is returned as `dynamic` while `c` is returned as `int`.

### Using with_itemindex

Expansion of an array with `with_itemindex`:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
range x from 1 to 4 step 1
| summarize x = make_list(x)
| mv-expand with_itemindex=Index x
```

|x|Index|
|---|---|
|1|0|
|2|1|
|3|2|
|4|3|

## See also

* See [Chart count of live activities over time](./samples.md#chart-concurrent-sessions-over-time) for more examples.
* [mv-apply](./mv-applyoperator.md) operator.
* [summarize make_list()](makelist-aggfunction.md), which is the opposite function of mv-expand.
* [bag_unpack()](bag-unpackplugin.md) plugin for expanding dynamic JSON objects into columns using property bag keys.
