---
title:  startofweek()
description: Learn how to use the startofweek() function to return the start of the week for the given date.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/31/2023
---
# startofweek()

Returns the start of the week containing the date, shifted by an offset, if provided.

Start of the week is considered to be a Sunday.

## Syntax

`startofweek(`*date* [`,` *offset* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *date* | datetime | &check; | The date for which to find the start of week.|
| *offset* | int | | The number of weeks to offset from the input date. The default is 0.|

## Returns

A datetime representing the start of the week for the given *date* value, with the offset, if specified.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAy2MQQqAMAwE775ijxYUGi8FwVf4gqKpqNSUGvDi421BGFiYhcn+2hgSws2KkCWiJ6iAcCsnUPMiZTl4UTzM56w+K6ZylpVQVbt6Zd0jt4Ml11sqgOxYcab72+YDGdMCYmkAAAA=" target="_blank">Run the query</a>

```kusto
range offset from -1 to 1 step 1
| project weekStart = startofweek(datetime(2017-01-01 10:10:17), offset) 
```

**Output**

|weekStart|
|---|
|2016-12-25 00:00:00.0000000|
|2017-01-01 00:00:00.0000000|
|2017-01-08 00:00:00.0000000|
