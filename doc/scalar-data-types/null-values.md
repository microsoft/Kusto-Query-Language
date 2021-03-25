---
title: Null Values - Azure Data Explorer | Microsoft Docs
description: This article describes Null Values in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: rkarlin
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# Null Values

All scalar data types in Kusto have a special value that represents a missing value.
This value is called the **null value**, or simply **null**.

## Null literals

The null value of a scalar type `T` is represented in the query language by the null literal `T(null)`.
Thus, the following returns a single row full of nulls:

```kusto
print bool(null), datetime(null), dynamic(null), guid(null), int(null), long(null), real(null), double(null), time(null)
```

> [!NOTE]
>Currently the `string` type doesn't support null values. For string type, use the [isempty()](../isemptyfunction.md) and the [isnotempty()](../isnotemptyfunction.md) functions.

## Comparing null to something

The null value does not compare to any other value of the data type,
including itself, with the following exceptions: equal (==) and not equal (!=) operators when comparing with non-null values.

To determine if some value is the null value, use the [isnull()](../isnullfunction.md) function, the [isnotnull()](../isnotnullfunction.md) function for numeric types, 
and the [isempty()](../isemptyfunction.md) and the [isnotempty()](../isnotemptyfunction.md) 
functions for the string type. 

For example:

```kusto
datatable(val:int)[5, int(null)]
| extend IsBiggerThan3 = val > 3
| extend IsBiggerThan3OrNull = val > 3 or isnull(val)
| extend IsEqualToNull = val == int(null)
| extend IsNotEqualToNull = val != int(null)
```

Results:

|val | IsBiggerThan3 | IsBiggerThan3OrNull | IsEqualToNull | IsNotEqualToNull|
|---|---|--------|--------|--------|
| 5 | true | true | false | true|
| &nbsp; | &nbsp; | true| &nbsp; | &nbsp;|


> [!NOTE]
> In EngineV2, a null comparison expression returns a boolean result. EngineV3 behaves as described [above](#comparing-null-to-something). However, since null coalesces to false in a boolean expression, the result in filter expressions is compatible between both engines.    

## Binary operations on null

In general, null behaves in a "sticky" way around binary operators; a binary
operation between a null value and any other value (including another null value)
produces a null value. For example:

```kusto
datatable(val:int)[5, int(null)]
| extend Add = val + 10
| extend Multiply = val * 10
```
Results:

|val|Add|Multiply|
|---|---|--------|
|5|	15|	50|
|&nbsp;|&nbsp;|&nbsp;| 		

## Null expression in filter

If an expression in the context of the filter operation such as in the [where operator](../whereoperator.md) returns null, the expression will be coalesced to `false`.  

Example:

```kusto
datatable(ival:int, sval:string)[5, "a", int(null), "b"]
| where ival != 5
```
Results:

|ival|sval|
|---|---|
|&nbsp;|b|

## Data ingestion and null values

For most data types, a missing value in the data source produces a null value
in the corresponding table cell. An exception to that are columns of type
`string` and CSV-like ingestion, where a missing value produces an empty string.
So, for example, if we have: 

```kusto
.create table T(a:string, b:int)

.ingest inline into table T
[,]
[ , ]
[a,1]
```

Then:

|a     |b     |isnull(a)|isempty(a)|strlen(a)|isnull(b)|
|------|------|---------|----------|---------|---------|
|&nbsp;|&nbsp;|false    |true      |0        |true     |
|&nbsp;|&nbsp;|false    |false     |1        |true     |
|a     |1     |false    |false     |1        |false    |

::: zone pivot="azuredataexplorer"

* If you run the query above in Kusto.Explorer, all `true`
  values will be displayed as `1`, and all `false` values
  will be displayed as `0`.

* Kusto does not offer a way to constrain a table's column from having null
  values (in other words, there's no equivalent to SQL's `NOT NULL` constraint).

::: zone-end

::: zone pivot="azuremonitor"

* Kusto does not offer a way to constrain a table's column from having null
  values (in other words, there's no equivalent to SQL's `NOT NULL` constraint).

::: zone-end
