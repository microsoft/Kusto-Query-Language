---
title: "The case-insensitive !hasprefix string operator - Azure Data Explorer"
description: "This article describes the case-insensitive !hasprefix operator in Azure Data Explorer."
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/25/2022
---
# !hasprefix operators

Filters a record set for data that does not include a case-insensitive starting string.

For best performance, use strings of three characters or more. `has` searches for indexed terms, where a [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

[!INCLUDE [has-prefix-operator-comparison](../../includes/has-prefix-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

For faster results, use the case-sensitive version of an operator. For example, use `hasprefix_cs` instead of `hasprefix`.

If you're testing for the presence of a symbol or alphanumeric word that is bound by non-alphanumeric characters at the start or end of a field, for faster results use `has` or `in`.

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
