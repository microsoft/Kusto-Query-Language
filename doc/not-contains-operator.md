---
title: The case-insensitive !contains string operator - Azure Data Explorer
description: Learn how to use the !contains string operator to filter data that doesn't include a case sensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---

# !contains operator

Filters a record set for data that doesn't include a case-sensitive string. `!contains` searches for characters rather than [terms](datatypes-string-operators.md#what-is-a-term) of three or more characters. The query scans the values in the column, which is slower than looking up a term in a term index.

[!INCLUDE [contains-operator-comparison](../../includes/contains-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

When possible, use the case-sensitive [!contains_cs](not-contains-cs-operator.md).

Use `!has` if you're looking for a [term](datatypes-string-operators.md#what-is-a-term).

## Syntax

### Case insensitive syntax

*T* `|` `where` *Column* `!contains` `(`*Expression*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check;| The tabular input whose records are to be filtered.|
| *Column* | string | &check;| The column by which to filter.|
| *Expression* | scalar | &check;| The scalar or literal expression for which to search.|

## Returns

Rows in *T* for which the predicate is `true`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPQTE5P68kMTOvWEEpOzFPCS6LZISCnYKxgYEBUKqgKD8rNbkEolUHWQ0AK1/nHYoAAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State !contains "kan"
| where event_count > 3000
| project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|TEXAS|4701|
