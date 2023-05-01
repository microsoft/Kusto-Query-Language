---
title: not() - Azure Data Explorer
description: Learn how to use the not() function to reverse the value of its boolean argument.
ms.reviewer: alexans
ms.topic: reference
ms.date: 03/12/2023
---
# not()

Reverses the value of its `bool` argument.

## Syntax

`not(`*expr*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
|*expr*|scalar|&check;|An expression that evaluates to a boolean value. The result of this expression will be reversed.|

## Returns

Returns the reversed logical value of its `bool` argument.

## Example

> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUcjLL9FIS8wpTtVUsLVVKCkqTQUAozQnchgAAAA=" target="_blank">Run the query</a>

```kusto
print not(false) == true
```

**Output**

|print_0|
|--|
|true|
