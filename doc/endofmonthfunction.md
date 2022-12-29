---
title: endofmonth() - Azure Data Explorer
description: Learn how to use the endofmonth() function to return a datetime representing the end of the month for the given date value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/11/2022
---
# endofmonth()

Returns the end of the month containing the date, shifted by an offset, if provided.

## Syntax

`endofmonth(`*date* [, *offset*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *date* | datetime | &check;| The date used to find the end of the month. |
| *offset* | int | | The number of offset months from *date*. Default is 0. |

## Returns

A datetime representing the end of the month for the given *date* value, with the *offset*, if specified.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAy3MOwqAMBBF0d5VvFJBIWMjCJYuJJgZP5CMJFO6eD8Ip7nNzT6tDBUpbJCsER3BFIRifIIqXDizHrwYoibb5hQwgVNQ+boO3tj2yHXvaOgcPUBufA1N+6+bGwOSY4VoAAAA" target="_blank">Run the query</a>

```kusto
  range offset from -1 to 1 step 1
 | project monthEnd = endofmonth(datetime(2017-01-01 10:10:17), offset) 
```

**Output**

|monthEnd|
|---|
|2016-12-31 23:59:59.9999999|
|2017-01-31 23:59:59.9999999|
|2017-02-28 23:59:59.9999999|
