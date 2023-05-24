---
title:  endofweek()
description: Learn how to use the endofweek() function to return a datetime representing the end of the week for the given date value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/11/2022
---
# endofweek()

Returns the end of the week containing the date, shifted by an offset, if provided.

Last day of the week is considered to be a Saturday.

## Syntax

`endofweek(`*date* [, *offset*]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *date* | datetime | &check;| The date used to find the end of the week. |
| *offset* | int | | The number of offset weeks from *date*. Default is 0. |

## Returns

A datetime representing the end of the week for the given *date* value, with the *offset*, if specified.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAy3MMQqAMBBE0d5TTKmgkLURBEsPImYiKmYlLth4eA0Ir/nNT1NcCA3hoiEkPdAITCG4jCekwIMz6cbZcJP7GD0GMHoNOUs/GW09WLZOusbJB+L6rKvqf1y9enVBc2YAAAA=" target="_blank">Run the query</a>

```kusto
  range offset from -1 to 1 step 1
 | project weekEnd = endofweek(datetime(2017-01-01 10:10:17), offset)  
```

**Output**

|weekEnd|
|---|
|2016-12-31 23:59:59.9999999|
|2017-01-07 23:59:59.9999999|
|2017-01-14 23:59:59.9999999|
