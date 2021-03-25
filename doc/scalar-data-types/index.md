---
title: Scalar data types - Azure Data Explorer | Microsoft Docs
description: This article describes Scalar data types in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 01/27/2020
---
# Scalar data types

Every data value (such as the value of an expression, or the parameter to a function,
or the value of an expression) has a **data type**. A data type is either a **scalar data type**
(one of the built-in predefined types listed below), or a **user-defined record**
(an ordered sequence of name/scalar-data-type pairs, such as the data type of a
row of a table).

Kusto supplies a set of system data types that define all the types of data
that can be used with Kusto.

> [!NOTE]
> User-defined data types are not supported in Kusto.

The following table lists the data types supported by Kusto, alongside
additional aliases you can use to refer to them and a roughly equivalent
.NET Framework type.

| Type       | Additional name(s)   | Equivalent .NET type              | gettype()   |
| ---------- | -------------------- | --------------------------------- | ----------- |
| `bool`     | `boolean`            | `System.Boolean`                  | `int8`      |
| `datetime` | `date`               | `System.DateTime`                 | `datetime`  |
| `dynamic`  |                      | `System.Object`                   | `array` or `dictionary` or any of the other values |
| `guid`     |                      | `System.Guid`                     | `guid`      |
| `int`      |                      | `System.Int32`                    | `int`       |
| `long`     |                      | `System.Int64`                    | `long`      |
| `real`     | `double`             | `System.Double`                   | `real`      |
| `string`   |                      | `System.String`                   | `string`    |
| `timespan` | `time`               | `System.TimeSpan`                 | `timespan`  |
| `decimal`  |                      | `System.Data.SqlTypes.SqlDecimal` | `decimal`   |

All non-string data types include a special "null" value, which represents the lack of data
or a mismatch of data. For example, attempting to ingest the string `"abc"`
into an `int` column results in this value.
It isn't possible to materialize this value explicitly, but you can detect
whether an expression evaluates to this value by using the `isnull()` function.

> [!WARNING]
> Support for the `guid` type is incomplete.
> We strongly recommend that teams use values of type `string` instead.
