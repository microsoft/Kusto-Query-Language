---
title:  The case-sensitive startswith string operator
description: Learn how to use the startswith string operator to filter a record set with a case-sensitive string starting sequence.
ms.reviewer: alexans
ms.topic: reference
ms.date: 09/19/2021
---
# startswith_cs operator

Filters a record set for data with a case-sensitive string starting sequence.

[!INCLUDE [startswith-operator-comparison](../../includes/startswith-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

## Syntax

*T* `|` `where` *col* `startswith_cs` `(`*expression*`)`  

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular input to filter.|
| *col* | string | &check; | The column used to filter.|
| *expression* | string | &check; | The expression by which to filter.|

## Returns

Rows in *T* for which the predicate is `true`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPobgksaikuDyzJCM+uVhByVMJLo9kiIKdgpGBgQFQqqAoPys1uQSiWQdZDQCj4hmWjAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State startswith_cs "I"
| where event_count > 2000
| project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|IOWA|2337|
|ILLINOIS|2022|
