---
title: The case-sensitive !hasprefix_cs string operator - Azure Data Explorer
description: Learn how to use the !hasprefix_cs string operator to filter records for data that doesn't have a case-sensitive prefix.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# !hasprefix_cs operator

Filters a record set for data that doesn't have a case-sensitive starting string. `!hasprefix_cs` searches for indexed terms, where an indexed [term](datatypes-string-operators.md#what-is-a-term) is three or more characters. If your term is fewer than three characters, the query scans the values in the column, which is slower than looking up the term in the term index.

|Operator   |Description   |Case-Sensitive  |Example (yields `true`)  |
|-----------|--------------|----------------|-------------------------|
|[`hasprefix`](hasprefix-operator.md) |RHS is a term prefix in LHS |No |`"North America" hasprefix "ame"`|
|[`!hasprefix`](not-hasprefix-operator.md) |RHS isn't a term prefix in LHS |No |`"North America" !hasprefix "mer"`|
|[`hasprefix_cs`](hasprefix-cs-operator.md) |RHS is a term prefix in LHS |Yes |`"North America" hasprefix_cs "Ame"`|
|[`!hasprefix_cs`](not-hasprefix-cs-operator.md) |RHS isn't a term prefix in LHS |Yes |`"North America" !hasprefix_cs "CA"`|

> [!NOTE]
> The following abbreviations are used in the above table:
>
> * RHS = right hand side of the expression
> * LHS = left hand side of the expression

For more information about other operators and to determine which operator is most appropriate for your query, see [datatype string operators](datatypes-string-operators.md).

## Performance tips

[!INCLUDE [performance-tip-note](../../includes/performance-tip-note.md)]

## Syntax

*T* `|` `where` *column* `!hasprefix_cs` `(`*expression*`)`  

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
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAwsuyS/KdS1LzSsp5qpRKC7NzU0syqxKVUgFCcUn55fmldiCSQ1NhaRKheCSxJJUoMLyjNSiVAhPQTEjsbigKDUtsyI+uVhBKUAJKA/WAgDAMC2uXAAAAA==" target="_blank">Run the query</a>

```kusto
StormEvents
| summarize event_count=count() by State
| where State !hasprefix_cs "P"
| count
```

**Output**

|Count|
|-----|
|64|
