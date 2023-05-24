---
title:  make_timespan()
description: Learn how to use the make_timespan() function to create a timespan scalar value from the specified time period.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/05/2023
---
# make_timespan()

Creates a [timespan](./scalar-data-types/timespan.md) scalar value from the specified time period.

## Syntax

`make_timespan(`*hour*, *minute*`)`

`make_timespan(`*hour*, *minute*, *second*`)`

`make_timespan(`*day*, *hour*, *minute*, *second*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*day*| int | &check;| The day.|
|*hour*| int | &check;| The hour. A value from 0-23.|
|*minute*| int || The minute. A value from 0-59.|
|*second*| real || The second. A value from 0 to 59.9999999.|

## Returns

If the creation is successful, the result will be a [timespan](./scalar-data-types/timespan.md) value. Otherwise, the result will be null.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUYhWL8nMTS0uSMxTj1WwVchNzE6Nh4loGOoYGukYG+iYmuoZGhlrAgBc6MUYMgAAAA==" target="_blank">Run the query</a>

```kusto
print ['timespan'] = make_timespan(1,12,30,55.123)
```

**Output**

|timespan|
|---|
|1.12:30:55.1230000|
