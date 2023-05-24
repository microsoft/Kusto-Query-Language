---
title:  strcat_array()
description: Learn how to use the strcat_array() function to create a concatenated string of array values using a specified delimiter. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/31/2023
---
# strcat_array()

Creates a concatenated string of array values using a specified delimiter.

## Syntax

`strcat_array(`*array*, *delimiter*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *array* | dynamic | &check; | An array of values to be concatenated.|
| *delimeter* | string | &check; | The value used to concatenate the values in *array*.|

## Returns

The input *array* values concatenated to a single string with the specified *delimiter*.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSguKVKwBZHJiSXxiUVFiZUaKZV5ibmZyRrRhjoKRjoKxrGaOgpKunZKmgBWe4fjMgAAAA==" target="_blank">Run the query</a>

```kusto
print str = strcat_array(dynamic([1, 2, 3]), "->")
```

**Output**

|str|
|---|
|1->2->3|
