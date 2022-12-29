---
title: datetime_part() - Azure Data Explorer
description: This article describes datetime_part() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/03/2022
---
# datetime_part()

Extracts the requested date part as an integer value.

> **Deprecated aliases:** datepart()

## Syntax

`datetime_part(`*part*`,`*datetime*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *part* | string | &check; | Measurement of time to extract from *date*. See [possible values](#possible-values-of-part).|
| *date* | datetime | &check; | The full date from which to extract *part*. |

### Possible values of part

* Year
* Quarter
* Month
* week_of_year
* Day
* DayOfYear
* Hour
* Minute
* Second
* Millisecond
* Microsecond
* Nanosecond

## Returns

An integer representing the extracted part.

> [!NOTE]
> `week_of_year` returns an integer which represents the week number. The week number is calculated from the first week of a year, which is the one that includes the first Thursday.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA3XPTQqDMBAF4L2nCK4UtCTaVrD0DF13JcGMGKqJTSPF29efRgPG7XvfzDANaMQ0uiNGNWjeQpBgksUExylGmOQ4yXF6yq6Xc5qQ8Ia8TnGhkTcAVdZU0VGlA39K/WhcGEbeux8jcKB/YVwrha73ao6N+QK8HtXTeXPqClkV9m1Ghz0cQ6s/WrdWxtayd7ApXT/gotfgeGHOjfpAKQXbqyXfdjUNP6JWuflSyWO/lsYLKg751i36B5qEtWAcAgAA" target="_blank">Run the query</a>

```kusto
let dt = datetime(2017-10-30 01:02:03.7654321); 
print 
year = datetime_part("year", dt),
quarter = datetime_part("quarter", dt),
month = datetime_part("month", dt),
weekOfYear = datetime_part("week_of_year", dt),
day = datetime_part("day", dt),
dayOfYear = datetime_part("dayOfYear", dt),
hour = datetime_part("hour", dt),
minute = datetime_part("minute", dt),
second = datetime_part("second", dt),
millisecond = datetime_part("millisecond", dt),
microsecond = datetime_part("microsecond", dt),
nanosecond = datetime_part("nanosecond", dt)
```

**Output**

|year|quarter|month|weekOfYear|day|dayOfYear|hour|minute|second|millisecond|microsecond|nanosecond|
|---|---|---|---|---|---|---|---|---|---|---|---|
|2017|4|10|44|30|303|1|2|3|765|765432|765432100|

> [!NOTE]
> `weekofyear` is an obsolete variant of `week_of_year` part. `weekofyear` was not ISO 8601 compliant; the first week of a year was defined as the week with the year's first Wednesday in it.
> `week_of_year` is ISO 8601 compliant; the first week of a year is defined as the week with the year's first Thursday in it. [For more information], see [ISO 8601 week dates](https://en.wikipedia.org/wiki/ISO_8601#Week_dates).
