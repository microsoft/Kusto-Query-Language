---
title: unicode_codepoints_to_string() - Azure Data Explorer
description: Learn how to use the unicode_codepoints_to_string() function to return the string represented by the Unicode codepoints.
ms.reviewer: alexans
ms.topic: reference
ms.date: 02/27/2023
---
# unicode_codepoints_to_string()

Returns the string represented by the Unicode codepoints. This function is the inverse operation of [`unicode_codepoints_from_string()`](unicode-codepoints-from-string-function.md) function.

> **Deprecated aliases:** make_string()

## Syntax

`unicode_codepoints_to_string (`*values*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *values* | int, long, or dynamic | &check; | One or more comma-separated values to convert. The values may also be a [dynamic array](scalar-data-types/dynamic.md).|

> [!NOTE]
> This function receives up to 64 arguments.

## Returns

Returns the string made of the UTF characters whose Unicode codepoint value is provided by the arguments to this function. The input must consist of valid Unicode codepoints.
If any argument isn't a valid Unicode codepoint, the function returns `null`.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvce69202ceceed490b88d.northeurope/databases/Other?query=H4sIAAAAAAAAAysoyswrUSguKVKwVSjNy0zOT0mNBxEF+UDx4viS/HigXGZeuoa5qY6CoaE5iACzzECEoSYA+KAQ+EAAAAA=" target="_blank">Run the query</a>

```kusto
print str = unicode_codepoints_to_string(75, 117, 115, 116, 111)
```

**Output**

|str|
|---|
|Kusto|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvce69202ceceed490b88d.northeurope/databases/Other?query=H4sIAAAAAAAAAysoyswrUSguKVKwVSjNy0zOT0mNBxEF+UDx4viS/HigXGZeukZKZV5ibmayRrS5qY6CoaE5iACzzECEYaymJgBfBO+kSwAAAA==" target="_blank">Run the query</a>

```kusto
print str = unicode_codepoints_to_string(dynamic([75, 117, 115, 116, 111]))
```

**Output**

|str|
|---|
|Kusto|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvce69202ceceed490b88d.northeurope/databases/Other?query=H4sIAAAAAAAAAysoyswrUSguKVKwVSjNy0zOT0mNBxEF+UDx4viS/HigXGZeukZKZV5ibmayRrS5qY6CoaE5iDCN1QRRZiDCUBMAeB3lVUsAAAA=" target="_blank">Run the query</a>

```kusto
print str = unicode_codepoints_to_string(dynamic([75, 117, 115]), 116, 111)
```

**Output**

|str|
|---|
|Kusto|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvce69202ceceed490b88d.northeurope/databases/Other?query=H4sIAAAAAAAAAysoyswrUSguKVKwVSjNy0zOT0mNBxEF+UDx4viS/HigXGZeuoa5qY6CoQEQG5rDGHARMxjDUBMACrIR/1AAAAA=" target="_blank">Run the query</a>

```kusto
print str = unicode_codepoints_to_string(75, 10, 117, 10, 115, 10, 116, 10, 111)
```

**Output**

|str|
|---|
|K<br>u<br>s<br>t<br>o|

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/kvce69202ceceed490b88d.northeurope/databases/Other?query=H4sIAAAAAAAAAysoyswrUSguKVKwVSjNy0zOT0mNBxEF+UDx4viS/HigXGZeukZRYl56qoaJhY6puaaOAoRnZqpjaQDnWZrrGBoZaWoCAIEH/7dTAAAA" target="_blank">Run the query</a>

```kusto
print str = unicode_codepoints_to_string(range(48,57), range(65,90), range(97,122))
```

**Output**

|str|
|---|
0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz|
