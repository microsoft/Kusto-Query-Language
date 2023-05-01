---
title: tolong() - Azure Data Explorer
description: Learn how to use the tolong() function to convert the input value to a long number representation.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/22/2023
---
# tolong()

Converts the input value to a long (signed 64-bit) number representation.

> [!NOTE]
> Prefer using [long()](./scalar-data-types/long.md) when possible.

## Syntax

`tolong(`*value*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | scalar | &check; | The value to convert to a [long](scalar-data-types/long.md).|

## Returns

If conversion is successful, the result is a long number.
If conversion isn't successful, the result is `null`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSjJz8nPS9dQMjQyVtJUsLVVADIAlTTbCRoAAAA=" target="_blank">Run the query</a>

```kusto
tolong("123") == 123
```
