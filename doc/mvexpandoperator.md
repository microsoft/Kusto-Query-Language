---
title: mv-expand operator - Azure Data Explorer | Microsoft Docs
description: This article describes mv-expand operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 02/24/2019
---
# mv-expand operator

Expands multi-value array or property bag.

`mv-expand` is applied on a [dynamic](./scalar-data-types/dynamic.md)-typed column so that each value in the collection gets a separate row. All the other columns in an expanded row are duplicated. 

**Syntax**

*T* `| mv-expand ` [`bagexpansion=`(`bag` | `array`)] [`with_itemindex=`*IndexColumnName*] *ColumnName* [`,` *ColumnName* ...] [`limit` *Rowlimit*]

*T* `| mv-expand ` [`bagexpansion=`(`bag` | `array`)] [*Name* `=`] *ArrayExpression* [`to typeof(`*Typename*`)`] [, [*Name* `=`] *ArrayExpression* [`to typeof(`*Typename*`)`] ...] [`limit` *Rowlimit*]

**Arguments**

* *ColumnName:* In the result, arrays in the named column are expanded to multiple rows. 
* *ArrayExpression:* An expression yielding an array. If this form is used, a new column is added and the existing one is preserved.
* *Name:* A name for the new column.
* *Typename:* Indicates the underlying type of the array's elements,
    which becomes the type of the column produced by the operator.
    Note that values in the array that do not conform to this type will
    not be converted; rather, they will take on a `null` value.
* *RowLimit:* The maximum number of rows generated from each original row. The default is 2147483647. 
*Note*: 
    Legacy and obsolete form of the operator `mvexpand` has default row-limit of 128.
* *IndexColumnName:* If `with_itemindex` is specified, the output will include an additional column (named *IndexColumnName*), which contains the index (starting at 0) of the item in the original expanded collection. 

**Returns**

Multiple rows for each of the values in any array in the named column or in the array expression.
If several columns or expressions are specified they are expanded in parallel so for each input row there will be as many output rows as there are elements in the longest expanded expression (shorter lists are padded with nulls). If the value in a row is an empty array, the row expands to nothing (will not show in the result set). If the value in a row is not an array, the row is kept as is in the result set. 

The expanded column always has dynamic type. Use a cast such as `todatetime()` or `tolong()` if you want to compute or aggregate values.

Two modes of property-bag expansions are supported:
* `bagexpansion=bag`: Property bags are expanded into single-entry property bags. This is the default expansion.
* `bagexpansion=array`: Property bags are expanded into two-element `[`*key*`,`*value*`]` array structures,
  allowing uniform access to keys and values (as well as, for example, running a distinct-count aggregation
  over property names). 

**Examples**

A simple expansion of a single column:
 ```kusto
datatable (a:int, b:dynamic)[1,dynamic({"prop1":"a", "prop2":"b"})]
| mv-expand b 
```

|a|b|
|---|---|
|1|{"prop1":"a"}|
|1|{"prop2":"b"}|


Expanding two columns will first 'zip' the applicable columns and then expand them:

```kusto
datatable (a:int, b:dynamic, c:dynamic)[1,dynamic({"prop1":"a", "prop2":"b"}), dynamic([5])]
| mv-expand b, c 
```

|a|b|c|
|---|---|---|
|1|{"prop1":"a"}|5|
|1|{"prop2":"b"}||

If you want to get a Cartesian product of expanding two columns, expand one after the other:
```kusto
datatable (a:int, b:dynamic, c:dynamic)[1,dynamic({"prop1":"a", "prop2":"b"}), dynamic([5])]
| mv-expand b 
| mv-expand c
```

|a|b|c|
|---|---|---|
|1|{"prop1":"a"}|5|
|1|{"prop2":"b"}|5|


Expansion of an array with `with_itemindex`:
```kusto
range x from 1 to 4 step 1 
| summarize x = make_list(x) 
| mv-expand with_itemindex=Index  x 
```

|x|Index|
|---|---|
|1|0|
|2|1|
|3|2|
|4|3|


**More examples**

See [Chart count of live activities over time](./samples.md#concurrent-activities).

**See also**

- [mv-apply](./mv-applyoperator.md) operator.
- [summarize make_list()](makelist-aggfunction.md) which performs the opposite function.
- [bag_unpack()](bag-unpackplugin.md) plugin for expanding dynamic JSON objects into columns using property bag keys.