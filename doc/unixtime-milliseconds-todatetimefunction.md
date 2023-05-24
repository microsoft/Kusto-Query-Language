---
title:  unixtime_milliseconds_todatetime()
description: Learn how to use the unixtime_milliseconds_todatetime() function to convert unix-epoch milliseconds to UTC datetime.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/28/2023
---
# unixtime_milliseconds_todatetime()

Converts unix-epoch milliseconds to UTC datetime.

## Syntax

`unixtime_milliseconds_todatetime(`*milliseconds*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *milliseconds* | real | &check; | The epoch timestamp in microseconds. A `datetime` value that occurs before the epoch time (1970-01-01 00:00:00) has a negative timestamp value.|

## Returns

If the conversion is successful, the result is a [datetime](./scalar-data-types/datetime.md) value. Otherwise, the result is null.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUhJLEmNL8nMTVWwVSjNy6wAMeNzM3NyMotTk/PzUorjS/JBakDiGoamJmbGBgYWBiCgCQDATIC6QQAAAA==" target="_blank">Run the query</a>

```kusto
print date_time = unixtime_milliseconds_todatetime(1546300800000)
```

**Output**

|date_time|
|---|
|2019-01-01 00:00:00.0000000|

## See also

* Convert unix-epoch seconds to UTC datetime using [unixtime_seconds_todatetime()](unixtime-seconds-todatetimefunction.md).
* Convert unix-epoch microseconds to UTC datetime using [unixtime_microseconds_todatetime()](unixtime-microseconds-todatetimefunction.md).
* Convert unix-epoch nanoseconds to UTC datetime using [unixtime_nanoseconds_todatetime()](unixtime-nanoseconds-todatetimefunction.md).
