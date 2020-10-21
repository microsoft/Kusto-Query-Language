---
title: base64_decode_toarray() - Azure Data Explorer
description: This article describes base64_decode_toarray() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 06/22/2019
---
# base64_decode_toarray()

Decodes a base64 string to an array of long values.

## Syntax

`base64_decode_toarray(`*String*`)`

## Arguments

* *String*: Input string to be decoded from base64 to UTF8 string.

## Returns

Returns an array of long values decoded from a base64 string.

* To decode base64 strings to a UTF-8 string, see [base64_decode_tostring()](base64_decode_tostringfunction.md)
* To encode strings to a base64 string, see [base64_encode_tostring()](base64_encode_tostringfunction.md)

## Example

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print Quine=base64_decode_toarray("S3VzdG8=")  
// 'K', 'u', 's', 't', 'o'
```

|Quine|
|-----|
|[75,117,115,116,111]|

If you try to decode a base64 string that was generated from an invalid UTF-8 encoding, "null" will be returned:

<!-- csl: https://help.kusto.windows.net:443/Samples -->
```kusto
print Empty=base64_decode_toarray("U3RyaW5n0KHR0tGA0L7Rh9C60LA=")
```

|Empty|
|-----|
||
