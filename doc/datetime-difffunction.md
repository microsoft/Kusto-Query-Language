---
title:  datetime_diff()
description: Learn how to use the datetime_diff() function to calculate the period between two datetime values. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/28/2022
---
# datetime_diff()

Calculates the number of the specified periods between two [datetime](./scalar-data-types/datetime.md) values.

## Syntax

`datetime_diff(`*period*`,`*datetime1*`,`*datetime2*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *period* | string | &check; | The measurement of time used to calculate the return value. See [possible values](#possible-values-of-period).|
| *datetime1* | datetime | &check; | The left-hand side of the subtraction equation.|
| *datetime2* | datetime | &check; | The right-hand side of the subtraction equation. |

### Possible values of *period*

* Year
* Quarter
* Month
* Week
* Day
* Hour
* Minute
* Second
* Millisecond
* Microsecond
* Nanosecond

## Returns

An integer that represents the amount of *periods* in the result of subtraction (*datetime1* - *datetime2*).

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA52S3YrDIBCF7/sU3rVCDTORkCawz7JIY6lsY7pZw9K3X3+6bagmQsWrc8bvDONcR6XN5ibFSD5IJ4w0qpefnTqddlunbvf/4q4ErBmgvXQuAjAsGUdK95vvSYxGJlB3I6LVrzQncsbB0fpBm3PM8nK+L6x8X570K+VXDHLqKweBlQ0BaAGivhpLIyVvq8YxO3GLkVZ8n3gepsTonJpgciSACaazZsxe6cnIxBC9nuL6x1C18cc8XbjTf+Rx0F1MD/oyHVqEAlea9+MqGgh7oC4XtRQ1M9fyLM2uaj7SVjxSj+OwnPowM6kBuDrLUHWwhS5XC70Y+/QyqRBOJjUU1ZT+AYwetRIGBAAA" target="_blank">Run the query</a>

```kusto
print
year = datetime_diff('year',datetime(2017-01-01),datetime(2000-12-31)),
quarter = datetime_diff('quarter',datetime(2017-07-01),datetime(2017-03-30)),
month = datetime_diff('month',datetime(2017-01-01),datetime(2015-12-30)),
week = datetime_diff('week',datetime(2017-10-29 00:00),datetime(2017-09-30 23:59)),
day = datetime_diff('day',datetime(2017-10-29 00:00),datetime(2017-09-30 23:59)),
hour = datetime_diff('hour',datetime(2017-10-31 01:00),datetime(2017-10-30 23:59)),
minute = datetime_diff('minute',datetime(2017-10-30 23:05:01),datetime(2017-10-30 23:00:59)),
second = datetime_diff('second',datetime(2017-10-30 23:00:10.100),datetime(2017-10-30 23:00:00.900)),
millisecond = datetime_diff('millisecond',datetime(2017-10-30 23:00:00.200100),datetime(2017-10-30 23:00:00.100900)),
microsecond = datetime_diff('microsecond',datetime(2017-10-30 23:00:00.1009001),datetime(2017-10-30 23:00:00.1008009)),
nanosecond = datetime_diff('nanosecond',datetime(2017-10-30 23:00:00.0000000),datetime(2017-10-30 23:00:00.0000007))
```

**Output**

|year|quarter|month|week|day|hour|minute|second|millisecond|microsecond|nanosecond|
|---|---|---|---|---|---|---|---|---|---|---|
|17|2|13|5|29|2|5|10|100|100|-700|
