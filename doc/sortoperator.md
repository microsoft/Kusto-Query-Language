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

[**Run the query**](https://dataexplorer.azure.com/clusters/help/databases/SampleLogs?query=H4sIAAAAAAAAAwspSkxO9clPL+aqUSjPSC1KVXDOyUzNKwlKLSxNLS7xTFGwtVVQMk20MLFIMzfQtbS0NNNNTTI01zU0TU3RNTJMskhNsjRJSjNIVQKaUJxfVKKQVKkQkpkL1JyYW6CQWJwMAFAUnRtjAAAA)

```kusto
TraceLogs
| where ClientRequestId == "5a848f70-9996-eb17-15ed-21b8eb94bf0e"
| sort by Timestamp asc
```
