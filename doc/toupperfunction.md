---
title: toupper() - Azure Data Explorer
description: Learn how to use the toupper() function to convert a string to upper case.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/23/2023
---
# toupper()

Converts a string to upper case.

## Syntax

`toupper(`*value*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | string | &check; | The value to convert to an uppercase string.|

## Returns

If conversion is successful, result is an uppercase string.
If conversion isn't successful, result is `null`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSjJLy0oSC3SUMpIzcnJV9JUsLVVUPJw9fHxVwIAC8jUKyEAAAA=" target="_blank">Run the query</a>

```kusto
toupper("hello") == "HELLO"
```
