---
title: The case-insensitive endswith string operator - Azure Data Explorer
description: Learn how to use the endswith operator to filter a record set for data with a case-insensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/10/2023
---
# endswith operator

Filters a record set for data with a case-insensitive ending string.

[!INCLUDE [endswith-operator-comparison](../../includes/endswith-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

For faster results, use the case-sensitive version of an operator. For example, use `endswith_cs` instead of `endswith`.

## Syntax

*T* `|` `where` *col* `endswith` `(`*expression*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check;| The tabular input whose records are to be filtered. |
| *col* | string | &check; | The column to filter. |
| *expression* | string | &check; | The expression used to filter. |

## Returns

Rows in *T* for which the predicate is `true`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVYAI2Sbnl+aVaGgqJFUqBJcklqQC1ZRnpBalQngKqXkpxeWZJRkKSsWJxUpwSYhmBTsFQwOgWEFRflZqcglEiw5UEgBi2Q64eAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize Events=count() by State
| where State endswith "sas"
| where Events > 10
| project State, Events
```

**Output**

|State|Events|
|--|--|
|KANSAS|3166|
|ARKANSAS|1028|
