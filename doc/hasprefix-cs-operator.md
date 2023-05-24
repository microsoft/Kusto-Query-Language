---
title:  The case-sensitive hasprefix_cs string operator
description: Learn how to use the hasprefix_cs operator to filter data with a case-sensitive prefix string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/25/2022
---
# hasprefix_cs operator

Filters a record set for data with a case-sensitive starting string.

For best performance, use strings of three characters or more. `hasprefix_cs` searches for indexed terms, where a [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

[!INCLUDE [has-prefix-operator-comparison](../../includes/has-prefix-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

## Syntax

*T* `|` `where` *Column* `hasprefix_cs` `(`*Expression*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular input whose records are to be filtered.|
| *Column* | string | &check; | The column used to filter.|
| *Expression* | string | &check; | The expression for which to search.|

## Returns

Rows in *T* for which the predicate is `true`.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPISOxuKAoNS2zIj65WEEpQAkoDdYBACZmycFbAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State hasprefix_cs "P"
| count 
```

|Count|
|-----|
|3|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPISOxuKAoNS2zIj65WEEpQAkoXVCUn5WaXAJRoINsGAAawoaacAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State hasprefix_cs "P"
| project State, event_count
```

|State|event_count|
|-----|-----------|
|PENNSYLVANIA|1687|
|PUERTO RICO|192|
|E PACIFIC|10|
