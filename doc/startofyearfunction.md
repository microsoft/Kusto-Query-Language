---
title:  startofyear()
description: Learn how to use the startofyear() function to return the start of the year for the given date.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/31/2023
---
# startofyear()

Returns the start of the year containing the date, shifted by an offset, if provided.

## Syntax

`startofyear(`*date* [`,` *offset* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *date* | datetime | &check; | The date for which to find the start of the year.|
| *offset* | int | | The number of years to offset from the input date. The default is 0.|

## Returns

A datetime representing the start of the year for the given *date* value, with the offset, if specified.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAy2MQQqAMAwE775ijxYsNF4Kgq/wBUVTUdBImovg460gDCzMwmg6V4bkXNiQVQ54ggkIxfgCNQ8ulZ1nw81JJ0tqGOtZV/Kn2iUZ23Zw2weKPlAFFIaP6Lq/7V7sDV7RaQAAAA==" target="_blank">Run the query</a>

```kusto
range offset from -1 to 1 step 1
| project yearStart = startofyear(datetime(2017-01-01 10:10:17), offset) 
```

**Output**

|yearStart|
|---|
|2016-01-01 00:00:00.0000000|
|2017-01-01 00:00:00.0000000|
|2018-01-01 00:00:00.0000000|
