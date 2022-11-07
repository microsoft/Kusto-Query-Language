---
title: base64_decode_tostring() - Azure Data Explorer
description: Learn how to use a base64_decode_tostring() function to decode a base64 string into a UTF-8 string. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/03/2022
---
# base64_decode_tostring()

Decodes a base64 string to a UTF-8 string.

> **Deprecated aliases:** base64_decodestring()

## Syntax

`base64_decode_tostring(`*String*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *String* | string | &check; | Input string to be decoded from base64 to UTF-8 string. |

## Returns

Returns UTF-8 string decoded from base64 string.

* To decode base64 strings to an array of long values, see [base64_decode_toarray()](base64_decode_toarrayfunction.md)
* To encode strings to base64 string, see [base64_encode_tostring()](base64_encode_tostringfunction.md)

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print Quine=base64_decode_tostring("S3VzdG8=")
```

|Quine|
|-----|
|Kusto|

Trying to decode a base64 string that was generated from invalid UTF-8 encoding will return null:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
print Empty=base64_decode_tostring("U3RyaW5n0KHR0tGA0L7Rh9C60LA=")
```

|Empty|
|-----|
||
