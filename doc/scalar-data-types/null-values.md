---
title: Null Values - Azure Data Explorer | Microsoft Docs
description: This article describes Null Values in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
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

> [!WARNING]
> Please note that currently the `string` type doesn't support null values.

## Comparing null to something

The null value does not compare equal to any other value of the data type,
including itself. (That is, `null == null` is false.) To determine if some
value is the null value, use the [isnull()](../isnullfunction.md) function
and the [isnotnull()](../isnotnullfunction.md) function.

## Binary operations on null

In general, null behaves in a "sticky" way around binary operators; a binary
operation between a null value and any other value (including another null value)
produces a null value.

## Data ingestion and null values

For most data types, a missing value in the data source produces a null value
in the corresponding table cell. An exception to that are columns of type
`string` and CSV-like ingestion, where a missing value produces an empty string.
So, for example, if we have: 

```kusto
.create table T [a:string, b:int]

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

> If you run the query above in Kusto.Explorer, all `true`
  values will be displated as `1`, and all `false` values
  will be displayed as `0`.

> Kusto does not offer a way to constrain a table's column from having null
  values (in other words, there's no equivalent to SQL's `NOT NULL` constraint).