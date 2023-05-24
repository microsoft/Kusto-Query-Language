---
title:  The case-insensitive !hasprefix string operator
description: Learn how to use the !hasprefix operator to filter records for data that doesn't include a case-insensitive prefix.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/09/2023
---
# !hasprefix operators

Filters a record set for data that doesn't include a case-insensitive starting string.

For best performance, use strings of three characters or more. `!hasprefix` searches for indexed terms, where an indexed [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

[!INCLUDE [has-prefix-operator-comparison](../../includes/has-prefix-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

When possible, use the case-sensitive [!hasprefix_cs](not-hasprefix-cs-operator.md).

## Syntax

*T* `|` `where` *Column* `!hasprefix` `(`*Expression*`)`

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
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPQTEjsbigKDUts0JByU8JLolkgoKdgpGBgQFQqqAoPys1uQSiUwdZDQCJ3wPtiQAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State !hasprefix "N"
| where event_count > 2000
| project State, event_count
```

|State|event_count|
|-----|-----------|
|TEXAS|4701|
|KANSAS|3166|
|IOWA|2337|
|ILLINOIS|2022|
|MISSOURI|2016|
