---
title: The case-sensitive == (equals) string operator - Azure Data Explorer
description: Learn how to use the == (equals) operator to filter a record set for data matching a case-sensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/29/2023
---
# == (equals) operator

Filters a record set for data matching a case-sensitive string.

The following table provides a comparison of the `==` operators:

|Operator   |Description   |Case-Sensitive  |Example (yields `true`)  |
|-----------|--------------|----------------|-------------------------|
|[`==`](equals-cs-operator.md)|Equals |Yes|`"aBc" == "aBc"`|
|[`!=`](not-equals-cs-operator.md)|Not equals |Yes |`"abc" != "ABC"`|
|[`=~`](equals-operator.md) |Equals |No |`"abc" =~ "ABC"`|
|[`!~`](not-equals-operator.md) |Not equals |No |`"aBc" !~ "xyz"`|

For more information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md).

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

## Syntax

*T* `|` `where` *col* `==` `(`*expression*`,` ... `)`

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
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5lIAghqF8ozUolSF4JLEklQFW1sFpezEvOLEYiWobHJ+aV4JACj9bS01AAAA" target="_blank">Run the query</a>

```kusto
StormEvents
| where State == "kansas"
| count 
```

|Count|
|---|
|0|  

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKM9ILUpVCC5JLElVsLVVUPJ29At2DFYCyiTnl+aVAABkHSoPLQAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| where State == "KANSAS"
| count 
```

|Count|
|---|
|3,166|
