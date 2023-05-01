---
title: datatable operator - Azure Data Explorer
description: Learn how to use the datatable operator to define a table with given schema and data.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
zone_pivot_group_filename: data-explorer/zone-pivot-groups.json
zone_pivot_groups: kql-flavors
---
# datatable operator

Returns a table whose schema and values are defined in the query itself.

> [!NOTE]
> This operator doesn't have a pipeline input.

## Syntax

`datatable(` *ColumnName* `:` *ColumnType* [`,` ...] `[` *ScalarValue* [`,` *ScalarValue* ...] `])`

## Parameters

::: zone pivot="azuredataexplorer"

| Name | Type | Required | Description |
|--|--|--|--|
| *ColumnName*:*ColumnType* | string | &check; | The name of column and type of data in that column that define the schema of the table.|
| *ScalarValue* | scalar | &check; | The value to insert into the table. The number of values must be an integer multiple of the columns in the table. The *n*'th value must have a type that corresponds to column *n* % *NumColumns*. |

::: zone-end

::: zone pivot="azuremonitor"

| Name | Type | Required | Description |
|--|--|--|--|
| *ColumnName*: *ColumnType* | string | &check; | The name of column and type of data in that column that define the schema of the table.|
| *ScalarValue* | scalar | &check; | The value to insert into the table. The number of values must be an integer multiple of the columns in the table. The *n*'th value must have a type that corresponds to column *n* % *NumColumns*. |

::: zone-end

## Returns

This operator returns a data table of the given schema and data.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3XRS4vCMBAA4Lu/YsiphbiY1upa0IPYo8velz2kZtRgTCCNL1z/uxNZd6HYJAQyj++QUTLQrg0mCxmwVHQFvUcO1RFtKJvgtd1wWDqPVCBLdbFyr1cpfPWA1rM+ERMx6A9GfSFSDmzuvGUcfouTK9vhRbCSHaU5oKBMDGTPQMZuKW9zOXGCTuQqG9A3UK2cQfiQ1ISdet7Wh6/0Iv/XPw+10c0WFay1bwLUzu06+aLNj17xk3H8i6yI/EKj6uTGbe79wX33fuC0RY9AAzBok8c0UpjBkDJ4DmgVxDaY/o3mLb7vp72pd88BAAA=" target="_blank">Run the query</a>

```kusto
datatable(Date:datetime, Event:string, MoreData:dynamic) [
    datetime(1910-06-11), "Born", dynamic({"key1":"value1", "key2":"value2"}),
    datetime(1930-01-01), "Enters Ecole Navale", dynamic({"key1":"value3", "key2":"value4"}),
    datetime(1953-01-01), "Published first book", dynamic({"key1":"value5", "key2":"value6"}),
    datetime(1997-06-25), "Died", dynamic({"key1":"value7", "key2":"value8"}),
]
| where strlen(Event) > 4
| extend key2 = MoreData.key2
```

**Output**

|Date|Event|MoreData|key2|
|---|---|---|---|
|1930-01-01 00:00:00.0000000|Enters Ecole Navale|{<br>  "key1": "value3",<br>  "key2": "value4"<br>}|value4|
|1953-01-01 00:00:00.0000000|Published first book|{<br>  "key1": "value5",<br>  "key2": "value6"<br>}|value6|
