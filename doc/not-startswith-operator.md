---
title: The case-insensitive !startswith string operators - Azure Data Explorer
description: Learn how to use the !startswith string operator to filter records for data that doesn't start with a case-insensitive search string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# !startswith operator

Filters a record set for data that doesn't start with a case-insensitive search string.

[!INCLUDE [startswith-operator-comparison](../../includes/startswith-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

When possible, use the case-sensitive [!startswith_cs](not-startswith-cs-operator.md).

## Syntax

*T* `|` `where` *column* `!startswith` `(`*expression*`)`

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
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPQbG4JLGopLg8syRDQSlTCS6LZISCnYKRgYEBUKqgKD8rNbkEolUHWQ0ASJ6KLIoAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State !startswith "i"
| where event_count > 2000
| project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|TEXAS|4701|
|KANSAS|3166|
|MISSOURI|2016|
