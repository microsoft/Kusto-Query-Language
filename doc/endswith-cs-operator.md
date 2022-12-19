---
title: The case-sensitive endswith_cs string operator - Azure Data Explorer
description: Learn how to use the endswith_cs operator to filter a record set for data with a case-sensitive ending string. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/11/2022
---
# endswith_cs operator

Filters a record set for data with a case-sensitive ending string.

[!INCLUDE [endswith-operator-comparison](../../includes/endswith-operator-comparison.md)]

> [!NOTE]
> The following abbreviations are used in the table above:
>
> * RHS = right hand side of the expression
> * LHS = left hand side of the expression

For more information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md).

## Performance tips

> [!NOTE]
> Performance depends on the type of search and the structure of the data.

For faster results, use the case-sensitive version of an operator, for example, `endswith_cs`, not `endswith`. For best practices, see [Query best practices](best-practices.md).

## Syntax

*T* `|` `where` *col* `endswith_cs` `(`*expression*`)`

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
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVYAIKdgqJOeX5pVoaCokVSoElySWpAJVlWekFqVCeAqpeSnF5ZklGfHJxQpKfo5KAE4M2OtQAAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize Events = count() by State
| where State endswith_cs "NA"
```

|State|Events|
|--|--|
|NORTH CAROLINA |1721|
|MONTANA |1230|
|INDIANA |1164|
|SOUTH CAROLINA| 915|
|LOUISIANA| 463|
|ARIZONA| 340|
