---
title: tohex() - Azure Data Explorer
description: Learn how to use the tohex() function to convert the input value to a hexadecimal string.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/22/2023
---
# tohex()

Converts input to a hexadecimal string.

## Syntax

`tohex(`*value*`,` [`,` *minLength* ]`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *value* | int or long | &check; | The value that will be converted to a hex string.|
| *minLength* | int | | The value representing the number of leading characters to include in the output.  Values between 1 and 16 are supported. Values greater than 16 will be truncated to 16. If the string is longer than *minLength* without leading characters, then *minLength* is effectively ignored. Negative numbers may only be represented at minimum by their underlying data size, so for an integer (32-bit) the *minLength* will be at minimum 8, for a long (64-bit) it will be at minimum 16.|

## Returns

If conversion is successful, result will be a string value.
If conversion is not successful, result will be `null`.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA42PsQ7CMAxEdyT+4baAlNIQoGLpyI/Qum2kJqkaD/18EhAliIXb7Ds/29NsHG83iGI/0LLTl2qPuoY4KiVk7hSr1X0p5VCWqM7F3TC0CGi8nUay5Bi+QxrMQezjzhdO4poj36iT/hMVyxWhkn6ufib056WEvy0NURtgjcNIruchsbVE8HnPBJje+ZnawwOAcjs/JwEAAA==" target="_blank">Run the query</a>

```kusto
print
    tohex(256) == '100',
    tohex(-256) == 'ffffffffffffff00', // 64-bit 2's complement of -256
    tohex(toint(-256), 8) == 'ffffff00', // 32-bit 2's complement of -256
    tohex(256, 8) == '00000100',
    tohex(256, 2) == '100' // Exceeds min length of 2, so min length is ignored.
```

**Output**

|print_0|print_1|print_2|print_3|print_04|
|--|--|--|--|--|
|true|true|true|true|true|
