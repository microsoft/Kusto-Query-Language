---
title:  binary_all_and() (aggregation function)
description: Learn how to use the binary_all_and() function to aggregate values using the binary AND operation.
ms.topic: reference
ms.date: 11/20/2022
---
# binary_all_and() (aggregation function)

Accumulates values using the binary `AND` operation for each summarization group, or in total if a group is not specified.

[!INCLUDE [data-explorer-agg-function-summarize-note](../../includes/data-explorer-agg-function-summarize-note.md)]

## Syntax

`binary_all_and` `(`*expr*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *expr* | long | &check; | The value used for the binary `AND`  calculation. |

## Returns

Returns an aggregated value using the binary `AND` operation over records for each summarization group, or in total if a group is not specified.

## Example

The following example produces `CAFEF00D` using binary `AND` operations:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA0tJLAHCpJxUjbzSXKuc/Lx0Ta5oLgUFgwo3KNBRQHANDNx0wDxniJwLhOfm6OYKVsoVy1WjUFyam5tYlFmVqlCUWlyaU6Jgq1CSX1pQkFqkUZKfkVqhkZSZl1hUGZ+YkxOfmJcCslhTUxMAwZHTS4kAAAA=" target="_blank">Run the query</a>

```kusto
datatable(num:long)
[
  0xFFFFFFFF, 
  0xFFFFF00F,
  0xCFFFFFFD,
  0xFAFEFFFF,
]
| summarize result = toupper(tohex(binary_all_and(num)))
```

**Output**

|result|
|---|
|CAFEF00D|
