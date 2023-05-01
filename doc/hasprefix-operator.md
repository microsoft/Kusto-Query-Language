---
title: The case-insensitive hasprefix string operator - Azure Data Explorer
description: Learn how to use the hasprefix operator to filter data with a case-insensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/25/2022
---
# hasprefix operator

Filters a record set for data with a case-insensitive starting string.

For best performance, use strings of three characters or more. `hasprefix` searches for indexed terms, where a [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

[!INCLUDE [has-prefix-operator-comparison](../../includes/has-prefix-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

When possible, use the case-sensitive [hasprefix_cs](hasprefix-cs-operator.md).

## Syntax

*T* `|` `where` *Column* `hasprefix` `(`*Expression*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular input whose records are to be filtered.|
| *Column* | string | &check; | The column used to filter.|
| *Expression* | string | &check; | The expression for which to search.|

## Returns

Rows in *T* for which the predicate is `true`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPISOxuKAoNS2zQkEpJ1EJKFlQlJ+VmlwCkdZBNgoAsFHbIG4AAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State hasprefix "la"
| project State, event_count
```

|State|event_count|
|-----|-----------|
|LAKE MICHIGAN|182|
|LAKE HURON|63|
|LAKE SUPERIOR|34|
|LAKE ST CLAIR|32|
|LAKE ERIE|27|
|LAKE ONTARIO|8|
