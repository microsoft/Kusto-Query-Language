---
title:  The case-insensitive has string operator
description: Learn how to use the has operator to filter data with a case-insensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/28/2022
---
# has operator

Filters a record set for data with a case-insensitive string. `has` searches for indexed terms, where an indexed [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

[!INCLUDE [has-operator-comparison](../../includes/has-operator-comparison.md)]

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

When possible, use the case-sensitive [has_cs](has-cs-operator.md).

## Syntax

*T* `|` `where` *Column* `has` `(`*Expression*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The tabular input whose records are to be filtered.|
| *Column* | string | &check; | The column used to filter the records.|
| *Expression* | scalar or tabular | &check; | An expression for which to search. If the value is a tabular expression and has multiple columns, the first column is used.|

## Returns

Rows in *T* for which the predicate is `true`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPISOxWEHJL7VcCS6OpFnBTsHQAChRUJSflZpcAtGig6wCANR4w8uCAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State has "New"
| where event_count > 10
| project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|NEW YORK|1,750|
|NEW JERSEY|1,044|
|NEW MEXICO|527|
|NEW HAMPSHIRE|394|  
