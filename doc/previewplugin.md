---
title:  preview plugin
description: Learn how to use the preview plugin to return two tables, one with the specified number of rows, and the other with the total number of records. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/12/2023
---
# preview plugin

Returns a table with up to the specified number of rows from the input record set, and the total number of records in the input record set.

## Syntax

*T* `|` `evaluate` `preview(`*NumberOfRows*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*T*|string|&check;|The table to preview.|
|*NumberOfRows*| int| &check; | The number of rows to preview from the table.|

## Returns

The `preview` plugin returns two result tables:

* A table with up to the specified number of rows.
  For example, the sample query above is equivalent to running `T | take 50`.
* A table with a single row/column, holding the number of records in the
  input record set.
  For example, the sample query above is equivalent to running `T | count`.

> [!TIP]
> If `evaluate` is preceded by a tabular source that includes a complex filter, or a filter that references most of the source table columns, prefer to use the [`materialize`](materializefunction.md) function. For example:

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSspVqhRSC1LzClNLElVKChKLctMLdcw1QQA4xlbCCEAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents | evaluate preview(5)
```

**Table1**

The following output table only includes the first 6 columns. To see the full result, run the query.

|StartTime|EndTime|EpisodeId|EventId|State|EventType|...|
|--|--|--|
|2007-12-30T16:00:00Z|2007-12-30T16:05:00Z|11749|64588|GEORGIA| Thunderstorm Wind|...|
|2007-12-20T07:50:00Z|2007-12-20T07:53:00Z|12554|68796|MISSISSIPPI| Thunderstorm Wind|...|
|2007-09-29T08:11:00Z|2007-09-29T08:11:00Z|11091|61032|ATLANTIC SOUTH| Waterspout|...|
|2007-09-20T21:57:00Z|2007-09-20T22:05:00Z|11078|60913|FLORIDA| Tornado|...|
|2007-09-18T20:00:00Z|2007-09-19T18:00:00Z|11074|60904|FLORIDA| Heavy Rain|...|

**Table2**

|Count|
|--|
|59066|
