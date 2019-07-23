---
title: Scalar data types - Azure Data Explorer | Microsoft Docs
description: This article describes Scalar data types in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 10/23/2018
---
# Scalar data types

Every data value (such as the value of an expression, or the parameter to a function,
or the value of an expression) has a **data type**. A data type is broadly
categorized as either being a **scalar data type**
(one of the built-in predefined types listed below), or be a **user-defined record**
(an ordered sequence of name/scalar-data-type pairs, such as the data type of a
row of a table).

Kusto supplies a set of system data types that define all the types of data
that can be used with Kusto.

> User-defined data types are not supported in Kusto.

The following table lists the data types supported by in Kusto, alongside
additional aliases one can use to refer to them and a roughly equivalent
.NET Framework type.

| Type       | Additional name(s)   | Equivalent .NET type              | gettype()   |Storage Type (internal name)|
| ---------- | -------------------- | --------------------------------- | ----------- |----------------------------|
| `bool`     | `boolean`            | `System.Boolean`                  | `int8`      |`I8`                        |
| `datetime` | `date`               | `System.DateTime`                 | `datetime`  |`DateTime`                  |
| `dynamic`  |                      | `System.Object`                   | `array` or `dictionary` or any of the other values |`Dynamic`|
| `guid`     | `uuid`, `uniqueid`   | `System.Guid`                     | `guid`      |`UniqueId`                  |
| `int`      |                      | `System.Int32`                    | `int`       |`I32`                       |
| `long`     |                      | `System.Int64`                    | `long`      |`I64`                       |
| `real`     | `double`             | `System.Double`                   | `real`      |`R64`                       |
| `string`   |                      | `System.String`                   | `string`    |`StringBuffer`              |
| `timespan` | `time`               | `System.TimeSpan`                 | `timespan`  |`TimeSpan`                  |
| `decimal`  |                      | `System.Data.SqlTypes.SqlDecimal` | `decimal`   | `Decimal`                  |

All data types include a special "null" value, which represents the lack of data
or a mismatch of data. (For example, attempting to ingest the string `"abc"`
into an `int` column results in this value.)
It is not possible to materialize this value explicitly, but one can detect
whether an expression evaluates to this value by using the `isnull()` function.

> [!WARNING]
> As of this writing, support for the `guid` type is
> incomplete. We strongly recommend that teams use values of type `string`
> instead.