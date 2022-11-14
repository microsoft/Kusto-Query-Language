---
title: column_ifexists() - Azure Data Explorer
description: This article describes column_ifexists() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/09/2022
---
# column_ifexists()

Takes a column name as a string and a default value. Returns a reference to the column if it exists,
otherwise - returns the default value.

> **Deprecated aliases:** columnifexists()

## Syntax

`column_ifexists(`*columnName*`, `*defaultValue*)

## Arguments

* *columnName*: The name of the column
* *defaultValue*: The value to use if the column doesn't exist in the context that the function was used in.
                  This value can be any scalar expression (for example, a reference to another column).

## Returns

If *columnName* exists, then the column it refers to. Otherwise - *defaultValue*.

## Examples

```kusto
.create function with (docstring = "Wraps a table query that allows querying the table even if columnName doesn't exist ", folder="My Functions")
ColumnOrDefault(tableName:string, columnName:string)
{
    // There's no column "Capital" in "StormEvents", therefore, the State column will be used instead
    table(tableName) | project column_ifexists(columnName, State)
}


ColumnOrDefault("StormEvents", "Capital");
```
