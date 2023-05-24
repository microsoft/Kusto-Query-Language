---
title:  endofyear()
description: Learn how to use the endofyear() function to return a datetime representing the end of the year for the given date value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/11/2022
---
# endofyear()

Returns the end of the year containing the date, shifted by an offset, if provided.

## Syntax

`endofyear(`*date* [, *offset*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *date* | datetime | &check;| The date used to find the end of the year. |
| *offset* | int | | The number of offset years from *date*. Default is 0. |

## Returns

A datetime representing the end of the year for the given *date* value, with the &*offset*, if specified.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAy3MMQqAMAxG4d1T/GMLFhoXQXD0IMUmomAjNYvg4VUQvuUtr6ayMFTkZINU3REIpiCcxgeowY2j6saz4eJUp5IxgktW+dLlZGzrzq6L1IdIL1AcPr1v/7F/AESu49RmAAAA" target="_blank">Run the query</a>

```kusto
  range offset from -1 to 1 step 1
 | project yearEnd = endofyear(datetime(2017-01-01 10:10:17), offset) 
```

**Output**

|yearEnd|
|---|
|2016-12-31 23:59:59.9999999|
|2017-12-31 23:59:59.9999999|
|2018-12-31 23:59:59.9999999|
