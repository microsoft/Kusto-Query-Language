---
title:  print operator
description: Learn how to use the print operator to output a single row with one or more scalar expression results as columns.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/12/2023
---
# print operator

Outputs a single row with one or more scalar expression results as columns.

## Syntax

`print` [*ColumnName* `=`] *ScalarExpression* [',' ...]

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *ColumnName* | string | | The name to assign to the output column.|
| *ScalarExpression* | string | &check; | The expression to evaluate.|

## Returns

A table with one or more columns and a single row. Each column returns the corresponding value of the evaluated *ScalarExpression*.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUTBQ0FYwBGIjIDYGYhMgNtVRqFCwVVAKzy9XVAIAppjMyScAAAA=" target="_blank">Run the query</a>

```kusto
print 0 + 1 + 2 + 3 + 4 + 5, x = "Wow!"
```

**Output**

|print_0|x|
|--|--|
|15| Wow!|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUhKzMtLLbItLilKTizRUPJIzcnJV9JRgKLw/KKcFEUlTQBf/iftLAAAAA==" target="_blank">Run the query</a>

```kusto
print banner=strcat("Hello", ", ", "World!")
```

**Output**

|banner|
|--|
|Hello, World!|
