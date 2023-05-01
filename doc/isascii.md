---
title: isascii() - Azure Data Explorer
description: Learn how to use the isascii() to check if the argument is a valid ascii string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 01/03/2023
---
# isascii()

Returns `true` if the argument is a valid ASCII string.

## Syntax

`isascii(`*value*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
|*value*|string|&check;| The value to check if a valid ASCII string.|

## Returns

A boolean value indicating whether *value* is a valid ASCII string.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUShKLS7NKbHNLE4sTs7M1FAqzs9NVSguAcqlK2kCAIfayAkjAAAA" target="_blank">Run the query</a>

```kusto
print result=isascii("some string")
```

**Output**

|result|
|--|
|true|
