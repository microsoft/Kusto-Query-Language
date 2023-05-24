---
title:  toint()
description: Learn how to use the toint() function to convert the input value to an integer number representation.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/22/2023
---
# toint()

Converts the input to an integer value (signed 32-bit) number representation.

> [!NOTE]
> Prefer using [int()](./scalar-data-types/int.md) when possible.

## Syntax

`toint(`*value*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | scalar | &check; | The value to convert to an [integer](scalar-data-types/int.md).|

## Returns

If the conversion is successful, the result will be an integer. Otherwise, the result will be `null`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUSjJB5IaSoZGxkqaCra2CkAGAO190RQZAAAA" target="_blank">Run the query</a>

```kusto
print toint("123") == 123
```

**Output**

|print_0|
|--|
|true|
