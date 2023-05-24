---
title:  The case-sensitive hassuffix_cs string operator
description: Learn how to use the hassuffix_cs operator to filter data with a case-sensitive suffix string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# hassuffix_cs operator

Filters a record set for data with a case-insensitive ending string. `hassuffix_cs` returns `true` if there is a [term](datatypes-string-operators.md#what-is-a-term) inside the filtered string column ending with the specified string expression.

[!INCLUDE [hassuffix-operator-comparison](../../includes/hassuffix-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

> [!NOTE]
> Text index cannot be fully utilized for this function, therefore the performance of this function is comparable to [endswith_cs](endswith-cs-operator.md) function, though the semantics is different.

## Syntax

*T* `|` `where` *column* `hassuffix_cs` `(` *expression* `)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*T* | string | &check; | The tabular input whose records are to be filtered.|
|*column* | string | &check; | The column by which to filter.|
|*expression* | scalar | &check; | The scalar or literal expression for which to search.|

## Returns

Rows in *T* for which the predicate is `true`.

## Examples  

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPISOxuLg0LS2zIj65WEHJMVgJLo9kiIKdgpGBgQFQqqAoPys1uQSiWQdZDQCa6WLOjAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State hassuffix_cs "AS"
| where event_count > 2000
| project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|TEXAS|4701|
|KANSAS|3166|
