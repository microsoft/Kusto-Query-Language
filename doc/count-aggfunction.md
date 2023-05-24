---
title:  count() (aggregation function)
description: Learn how to use the count() function to count the number of records in a group.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/27/2022
---
# count() (aggregation function)

Counts the number of records per summarization group, or total if summarization is done without grouping.

Use the [countif](countif-aggfunction.md) aggregation function to count only records for which a predicate returns `true`.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`count()`

## Returns

Returns a count of the records per summarization group (or in total, if summarization is done without grouping).

## Example

This example returns a count of events in states starting with letter `W`:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKM9ILUpVCC5JLElVKC5JLCopLs8syVBQClcCShaX5uYmFmVWpSo455fmldgmg0gNTYWkSogOAJStyvpLAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| where State startswith "W"
| summarize Count=count() by State
```

**Output**

|State|Count|
|---|---|
|WEST VIRGINIA|757|
|WYOMING|396|
|WASHINGTON|261|
|WISCONSIN|1850|
