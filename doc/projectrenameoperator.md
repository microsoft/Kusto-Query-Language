---
title:  project-rename operator
description: Learn how to use the project-rename operator to rename columns in the output table.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/16/2023
---
# project-rename operator

Renames columns in the output table.

## Syntax

*T* `| project-rename` *NewColumnName* = *ExistingColumnName* [`,` ...]

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *T* | string | &check; | The input tabular data.|
| *NewColumnName* | string | &check; | The new column name.|
| *ExistingColumnName* | string | &check; | The name of the existing column to rename.|

## Returns

A table that has the columns in the same order as in an existing table, with columns renamed.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUUi0VU9U11FIslVPAlLJturJ6rxcNQoFRflZqcklukWpeYm5qQp5qeXxSbZJOmBGom0iALIWMhk8AAAA" target="_blank">Run the query</a>

```kusto
print a='a', b='b', c='c'
|  project-rename new_b=b, new_a=a
```

**Output**

|new_a|new_b|c|
|---|---|---|
|a|b|c|
