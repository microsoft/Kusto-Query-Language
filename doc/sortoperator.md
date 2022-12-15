---
title: sort operator  - Azure Data Explorer
description: This article describes sort operator  in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/24/2022
---
# sort operator

Sort the rows of the input table into order by one or more columns.

**Alias**: [order](orderoperator.md)

## Syntax

*T* `| sort by` *column* [`asc` | `desc`] [`nulls first` | `nulls last`] [`,` ...]

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *T* | string | &check; | Tabular input to sort. |
| *column* | string | &check; | Column of *T* by which to sort. The type of the column values must be numeric, date, time or string.|
| `asc` or `desc` | string | | `asc` sorts into ascending order, low to high. Default is `desc`, high to low. |
| `nulls first` or `nulls last`  | string | | `nulls first` will place the null values at the beginning and `nulls last` will place the null values at the end. Default for `asc` is `nulls first`. Default for `desc` is `nulls last`.|

## Example

All rows in table Traces that have a specific `ClientRequestId`, sorted by their timestamp.

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/SampleLogs?query=H4sIAAAAAAAAAwspSkxO9clPL+aqUSgoys9KTS5RcM7JTM0rCUotLE0tLvFM0VEIycwFshJzC4CKyjNSi1LRlSjY2ioomSZamFikmRvoWlpamummJhma6xqapqboGhkmWaQmWZokpRmkKgFNKM4vKlFIqkSYqpBYnAwAl8Rv8YgAAAA=" target="_blank">Run the query</a>

```kusto
TraceLogs
| project ClientRequestId, Timestamp
| where ClientRequestId == "5a848f70-9996-eb17-15ed-21b8eb94bf0e"
| sort by Timestamp asc
```

The following table only shows the top 10 results. To see the full output, run the query.

|ClientRequestId|Timestamp|
|--|--|
|5a848f70-9996-eb17-15ed-21b8eb94bf0e| 2014-03-08T12:24:55.5464757Z|
|5a848f70-9996-eb17-15ed-21b8eb94bf0e| 2014-03-08T12:24:56.0929514Z|
|5a848f70-9996-eb17-15ed-21b8eb94bf0e| 2014-03-08T12:25:40.3574831Z|
|5a848f70-9996-eb17-15ed-21b8eb94bf0e| 2014-03-08T12:25:40.9039588Z|
|5a848f70-9996-eb17-15ed-21b8eb94bf0e| 2014-03-08T12:26:25.1684905Z|
|5a848f70-9996-eb17-15ed-21b8eb94bf0e| 2014-03-08T12:26:25.7149662Z|
|5a848f70-9996-eb17-15ed-21b8eb94bf0e| 2014-03-08T12:27:09.9794979Z|
|5a848f70-9996-eb17-15ed-21b8eb94bf0e| 2014-03-08T12:27:10.5259736Z|
|5a848f70-9996-eb17-15ed-21b8eb94bf0e| 2014-03-08T12:27:54.7905053Z|
|5a848f70-9996-eb17-15ed-21b8eb94bf0e| 2014-03-08T12:27:55.336981Z|
