---
title: The case-sensitive !startswith_cs string operator - Azure Data Explorer
description: Learn how to use the !startswith_cs string operator to filter records for data that doesn't start with a case-sensitive search string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# !startswith_cs operators

Filters a record set for data that doesn't start with a case-sensitive search string.

[!INCLUDE [startswith-operator-comparison](../../includes/startswith-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

## Syntax

*T* `|` `where` *column* `!startswith_cs` `(`*expression*`)`  

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check;| The tabular input whose records are to be filtered.|
| *column* | string | &check;| The column by which to filter.|
| *expression* | scalar | &check;| The scalar or literal expression for which to search.|

## Returns

Rows in *T* for which the predicate is `true`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPQbG4JLGopLg8syQjPrlYQclTCa4AyRQFOwUjAwMDoFRBUX5WanIJRLcOshoAsNzTI40AAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State !startswith_cs "I"
| where event_count > 2000
| project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|TEXAS|4701|
|KANSAS|3166|
|MISSOURI|2016|
