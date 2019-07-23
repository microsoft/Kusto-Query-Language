---
title: datetime_part() - Azure Data Explorer | Microsoft Docs
description: This article describes datetime_part() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: mblythe
ms.service: data-explorer
ms.topic: reference
ms.date: 01/15/2019
---
# datetime_part()

Extracts the requested date part as an integer value.

```kusto
datetime_part("Day",datetime(2015-12-14))
```

**Syntax**

`datetime_part(`*part*`,`*datetime*`)`

**Arguments**

* `date`: `datetime`.
* `part`: `string`. 

Possible values of *part*: 
- Year
- Quarter
- Month
- WeekOfYear
- Day
- DayOfYear
- Hour
- Minute
- Second
- Millisecond
- Microsecond
- Nanosecond

**Returns**

An integer representing the extracted part.

**Note**

`WeekOfYear` returns according to ISO 8601 standard, which specifies that the First Week of a year, is the one that includes the first Wednesday (https://en.wikipedia.org/wiki/ISO_8601#Week_dates)

**Examples**

```kusto
let dt = datetime(2017-10-30 01:02:03.7654321); 
print 
year = datetime_part("year", dt),
quarter = datetime_part("quarter", dt),
month = datetime_part("month", dt),
weekOfYear = datetime_part("weekOfYear", dt),
day = datetime_part("day", dt),
dayOfYear = datetime_part("dayOfYear", dt),
hour = datetime_part("hour", dt),
minute = datetime_part("minute", dt),
second = datetime_part("second", dt),
millisecond = datetime_part("millisecond", dt),
microsecond = datetime_part("microsecond", dt),
nanosecond = datetime_part("nanosecond", dt)

```

|year|quarter|month|weekOfYear|day|dayOfYear|hour|minute|second|millisecond|microsecond|nanosecond|
|---|---|---|---|---|---|---|---|---|---|---|---|
|2017|4|10|44|30|303|1|2|3|765|765432|765432100|