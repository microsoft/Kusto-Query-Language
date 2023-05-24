---
title:  isutf8()
description: Learn how to use the isutf8() function to check if the argument is a valid utf8 string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 12/21/2022
---
# isutf8()

Returns `true` if the argument is a valid UTF8 string.

## Syntax

`isutf8(`*value*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
|*value*|string|&check;| The value to check if a valid UTF8 string.|

## Returns

A boolean value indicating whether *value* is a valid UTF8 string.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNLC4tSbPQUCrOz01VKC4BSqUraQIA1zBdDCIAAAA=" target="_blank">Run the query</a>

```kusto
print result=isutf8("some string")
```
