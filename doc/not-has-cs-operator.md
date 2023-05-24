---
title:  The case-sensitive !has_cs string operator
description: Learn how to use the !has_cs string operator to filter records for data that doesn't have a matching case-sensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/09/2023
---
# !has_cs operator

Filters a record set for data that doesn't have a matching case-sensitive string. `!has_cs` searches for indexed terms, where an indexed [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

[!INCLUDE [has-operator-comparison](../../includes/has-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

## Syntax

*T* `|` `where` *column* `!has_cs` `(`*expression*`)`  

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
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPQTEjsTg+uVhBKS+1XAkoB1YOAGM3qTFYAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State !has_cs "new"
| count
```

**Output**

|Count|
|-----|
|67|
