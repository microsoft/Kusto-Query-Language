---
title: The case-insensitive !endswith string operator - Azure Data Explorer
description: This article describes the case-insensitive !endswith string operator in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 10/03/2021
---
# !endswith operator

Filters a record set for data that excludes a case-insensitive ending string.

[!INCLUDE [endswith-operator-comparison](../../includes/endswith-operator-comparison.md)]

> [!NOTE]
> The following abbreviations are used in the table above:
>
> * RHS = right hand side of the expression
> * LHS = left hand side of the expression

For more information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md). 

Case-insensitive operators are currently supported only for ASCII-text. For non-ASCII comparison, use the [tolower()](tolowerfunction.md) function.

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

For faster results, use the case-sensitive version of an operator. For example, use `endswith_cs` instead of `endswith`.

## Syntax

*T* `|` `where` *col* `!endswith` `(`*expression*`)`   

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
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVYAI2Sbnl+aVaGgqJFUqBJcklqQC1ZRnpBalQngKiql5KcXlmSUZCkqZxUpwSYhmBTsFIwMDA6BoQVF+VmpyCUSTDlQaADTefzN6AAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize Events=count() by State
| where State !endswith "is"
| where Events > 2000
| project State, Events
```

**Output**

|State|Events|
|--|--|
|TEXAS|4701|
|KANSAS|3166|
|IOWA|2337|
|MISSOURI|2016|
