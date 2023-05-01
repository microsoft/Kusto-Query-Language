---
title: The case-sensitive matches regex string operator - Azure Data Explorer
description: Learn how to use the matches regex string operator to filter a record set based on a case-sensitive regex value.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/17/2023
---
# matches regex operator

Filters a record set based on a case-sensitive regex value.

For more information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md).

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

## Syntax

*T* `|` `where` *col* `matches` `regex` `(`*expression*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular input whose records are to be filtered.|
| *col* | string | &check; | The column by which to filter.|
| *expression* | scalar | &check; | The expression used to filter.|

## Returns

Rows in *T* for which the predicate is `true`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPITexJDkjtVihKDU9tUJByVtPK1gJrgTJHAU7BUMDoERBUX5WanIJRLcOsgoA+5LANo0AAAA=" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State matches regex "K.*S"
| where event_count > 10
| project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|KANSAS|3166|
|ARKANSAS|1028|
|LAKE SUPERIOR|34|
|LAKE ST CLAIR|32|  
