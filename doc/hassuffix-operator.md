---
title: The case-insensitive hassuffix string operator - Azure Data Explorer
description:  Learn how to use the hassuffix operator to filter data with a case-insensitive suffix string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# hassuffix operator

Filters a record set for data with a case-insensitive ending string. `hassuffix` returns `true` if there is a [term](datatypes-string-operators.md#what-is-a-term) inside the filtered string column ending with the specified string expression.

[!INCLUDE [hassuffix-operator-comparison](../../includes/hassuffix-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

When possible, use the case-sensitive [hassuffix_cs](hassuffix-cs-operator.md).

> [!NOTE]
> Text index cannot be fully utilized for this function, therefore the performance of this function is comparable to [endswith](endswith-operator.md) function, though the semantics is different.

## Syntax

*T* `|` `where` *Column* `hassuffix` `(`*Expression*`)`

## Parameters

|Name|Type|Required|Description|
|--|--|--|--|
|*T*|string|The tabular input whose records are to be filtered.|
|*Column*|string|The column by which to filter.|
|*Expression*|scalar|The scalar or literal expression for which to search.|

## Returns

Rows in *T* for which the predicate is `true`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPISOxuLg0LS2zQkEpXwkoV1CUn5WaXAKR1UE2CQBH0LHRbQAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State hassuffix "o"
| project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|COLORADO|1654|
|OHIO|1233|
|GULF OF MEXICO|577|
|NEW MEXICO|527|
|IDAHO|247|
|PUERTO RICO|192|
|LAKE ONTARIO|8|
