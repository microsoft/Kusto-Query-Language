---
title:  The case-sensitive != (not equals) string operator
description: Learn how to use the != (not equals) string operator to filter records for data that doesn't match a case-sensitive string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# != (not equals) operator

Filters a record set for data that doesn't match a case-sensitive string.

The following table provides a comparison of the `==` (equals) operators:

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

*T* `|` `where` *column* `!=` `(`*expression*`)`

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
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAUNMFdB0VZByc3HP8jTxVFJUyExL0VBA0m7gp2CiYGBgSZQV0FRflZqcgnEFB1kOwDGtl9AhwAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where (State != "FLORIDA") and (event_count > 4000)
| project State, event_count
```

**Output**

|State|event_count|
|-----|-----------|
|TEXAS|4,701|
