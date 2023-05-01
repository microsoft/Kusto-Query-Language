---
title: The case-insensitive !hassuffix string operator - Azure Data Explorer
description: Learn how to use the !hassuffix string operator to filter records for data that doesn't have a case-insensitive suffix.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# !hassuffix operator

Filters a record set for data that doesn't have a case-insensitive ending string. `!hassuffix` returns `true` if there's no [term](datatypes-string-operators.md#what-is-a-term) inside string column ending with the specified string expression.

[!INCLUDE [hassuffix-operator-comparison](../../includes/hassuffix-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

When possible, use [!hassuffix_cs](not-hassuffix-cs-operator.md) - a case-sensitive version of the operator.

> [!NOTE]
> Text index cannot be fully utilized for this function, therefore the performance of this function is comparable to [!endswith](not-endswith-operator.md) function, though the semantics is different.

## Syntax

*T* `|` `where` *column* `!hassuffix` `(`*expression*`)`

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
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPQTEjsbi4NC0ts0JByVEJLolkgoKdgpGBgQFQqqAoPys1uQSiUwdZDQCFtu1diQAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State !hassuffix "A"
| where event_count > 2000
| project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|TEXAS|4701|
|KANSAS|3166|
|ILLINOIS|2022|
|MISSOURI|2016|
