---
title:  tolower()
description: Learn how to use the tolower() function to convert the input string to lower case.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/22/2023
---
# tolower()

Converts the input string to lower case.

## Syntax

`tolower(`*value*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | string | &check; | The value to convert to a lowercase string.|

## Returns

If conversion is successful, result is a lowercase string.
If conversion isn't successful, result is `null`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSjJz8kvTy3SUPJIzcnJV9JUsLVVUMoAswH4X1SGIQAAAA==" target="_blank">Run the query</a>

```kusto
tolower("Hello") == "hello"
```
