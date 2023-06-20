---
title:  now()
description: Learn how to use the now() function to return the current UTC time.
ms.reviewer: alexans
ms.topic: reference
ms.date: 06/19/2023
---
# now()

Returns the current UTC time, optionally offset by a given [timespan](scalar-data-types/timespan.md).

The current UTC time will stay the same across all uses of `now()` in a single query statement, even if there's technically a small time difference between when each `now()` runs.

## Syntax

`now(`[ *offset* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *offset* | timespan | | A timespan to add to the current UTC clock time. The default value is 0.|

## Returns

The current UTC clock time, plus the *offset* time if provided, as a `datetime`.

## Examples

### Find time elapsed from a given event

The following example shows the time elapsed since the start of the storm events.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRSK0oSc1LUXDNSSwoTk2xzcsv19BU0FUILkksKgnJzE0FKilJzE5VMDQAAK5wFN84AAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| extend Elapsed=now() - StartTime
| take 10
```

### Get the date relative to a specific time interval

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVEIUbBVSEksAcKknFSNnMSk1BwrheKSosy8dB2Fkszc1OKCxLywxJzSVCs4V1MhmpdLAQiUcjPzSktSlXQUzAyKdRSgghn5pUVAIcMMuEhKYiVIIAUuUJmaCFJibGaawssVa83LFQKUqVFIrShJzUsB2+OYng90WV5+uYamgi6qQwBVkuK6twAAAA==" target="_blank">Run the query</a>

```kusto
let T = datatable(label: string, timespanValue: timespan) [
    "minute", 60s, 
    "hour", 1h, 
    "day", 1d, 
    "year", 365d
];
T 
| extend timeAgo = now() - timespanValue
```

**Output**

| label | timespanValue | timeAgo |
|--|--|--|
| year | 365.00:00:00 | 2022-06-19T08:22:54.6623324Z |
| day | 1.00:00:00 | 2023-06-18T08:22:54.6623324Z |
| hour | 01:00:00 | 2023-06-19T07:22:54.6623324Z |
| minute | 00:01:00 | 2023-06-19T08:21:54.6623324Z |

> [!NOTE]
> This operation can be accomplished with the [ago() function](agofunction.md).
