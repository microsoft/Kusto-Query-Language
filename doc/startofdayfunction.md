---
title: startofday() - Azure Data Explorer
description: Learn how to use the startofday() function to return the start of the day for the given date.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/31/2023
---
# startofday()

Returns the start of the day containing the date, shifted by an offset, if provided.

## Syntax

`startofday(`*date* [`,` *offset* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *date* | datetime | &check; | The date for which to find the start.|
| *offset* | int | | The number of days to offset from the input date. The default is 0.|

## Returns

A datetime representing the start of the day for the given *date* value, with the offset, if specified.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAy2MQQqAMAwE775ijwoWGi+C4Ct8QdFEFGpKm4vg460gDCzMwuRw7QwVKWyQrBGOYApCMU6g5kHKevJq2MK9WMiGuX51Vappt2BsR+R28DQ6TxWQnz7Grv/L3Qu9LKDlZwAAAA==" target="_blank">Run the query</a>

```kusto
range offset from -1 to 1 step 1
| project dayStart = startofday(datetime(2017-01-01 10:10:17), offset) 
```

**Output**

|dayStart|
|---|
|2016-12-31 00:00:00.0000000|
|2017-01-01 00:00:00.0000000|
|2017-01-02 00:00:00.0000000|
