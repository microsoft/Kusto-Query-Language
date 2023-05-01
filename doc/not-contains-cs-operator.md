---
title: The case-sensitive !contains_cs string operator - Azure Data Explorer
description: Learn how to use the !contains_cs string operator to filter data that doesn't include a case-sensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---

# !contains_cs operator

Filters a record set for data that doesn't include a case-sensitive string. `!contains_cs` searches for characters rather than [terms](datatypes-string-operators.md#what-is-a-term) of three or more characters. The query scans the values in the column, which is slower than looking up a term in a term index.

[!INCLUDE [contains-operator-comparison](../../includes/contains-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

If you're looking for a [term](datatypes-string-operators.md#what-is-a-term), use `!has_cs` for faster results.

## Syntax

### Case-sensitive syntax

*T* `|` `where` *Column* `!contains_cs` `(`*Expression*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check;| The tabular input whose records are to be filtered.|
| *Column* | string | &check;| The column by which to filter.|
| *Expression* | scalar | &check;| The scalar or literal expression for which to search.|

## Returns

Rows in *T* for which the predicate is `true`.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPQTE5P68kMTOvOD65WEHJMVgJKA/WAgDbLF9iXAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State !contains_cs "AS"
| count
```

**Output**

|Count|
|-----|
|59|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPQTE5P68kMTOvOD65WEEpxDVCCa4AyRQFOwVjAwMDoFRBUX5WanIJRLcOshoAY3LN3I0AAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State !contains_cs "TEX"
| where event_count > 3000
| project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|KANSAS|3,166|
