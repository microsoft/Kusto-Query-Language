---
title: datatable operator - Azure Data Explorer
description: This article describes datatable operator in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 02/13/2020
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# datatable operator

Returns a table whose schema and values are defined in the query itself.

> [!NOTE]
> This operator doesn't have a pipeline input.

## Syntax

`datatable` `(` *ColumnName* `:` *ColumnType* [`,` ...] `)` `[` *ScalarValue* [`,` *ScalarValue* ...] `]`

## Arguments

::: zone pivot="azuredataexplorer"

* *ColumnName*, *ColumnType*: These arguments define the schema of the table. The arguments use the same syntax as used when defining a table.
  For more information, see [.create table](../management/create-table-command.md)).
* *ScalarValue*: A constant scalar value to insert into the table. The number of values
  must be an integer multiple of the columns in the table. The *n*'th value
  must have a type that corresponds to column *n* % *NumColumns*.

::: zone-end

::: zone pivot="azuremonitor"

* *ColumnName*, *ColumnType*: These arguments define the schema of the table.
* *ScalarValue*: A constant scalar value to insert into the table. The number of values
  must be an integer multiple of the columns in the table. The *n*'th value
  must have a type that corresponds to column *n* % *NumColumns*.

::: zone-end

## Returns

This operator returns a data table of the given schema and data.

## Example

```kusto
datatable (Date:datetime, Event:string)
    [datetime(1910-06-11), "Born",
     datetime(1930-01-01), "Enters Ecole Navale",
     datetime(1953-01-01), "Published first book",
     datetime(1997-06-25), "Died"]
| where strlen(Event) > 4
```
