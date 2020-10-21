---
title: base64_encode_fromarray() - Azure Data Explorer
description: This article describes base64_encode_fromarray() in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: alexans
ms.service: data-explorer
ms.topic: reference
ms.date: 03/11/2020
---
# base64_encode_fromarray()

Encodes a base64 string from a bytes array.

## Syntax

`base64_encode_fromarray(`*BytesArray*`)`

## Arguments

* *BytesArray*: Input bytes array to be encoded into base64 string.

## Returns

Returns the base64 string encoded from the bytes array.

* For decoding base64 strings to a UTF-8 string see [base64_decode_tostring()](base64_decode_tostringfunction.md)
* For encoding strings to base64 string see [base64_encode_tostring()](base64_encode_tostringfunction.md)
* This function is the inverse of [base64_decode_toarray()](base64_decode_toarrayfunction.md)

## Example

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let bytes_array = toscalar(print base64_decode_toarray("S3VzdG8="));
print decoded_base64_string = base64_encode_fromarray(bytes_array)
```

|decoded_base64_string|
|---|
|S3VzdG8=|


Trying to encode a base64 string from an invalid bytes array which was generated from invalid UTF-8 encoded string will return null:

<!-- csl: https://help.kusto.windows.net/Samples -->
```kusto
let empty_bytes_array = toscalar(print base64_decode_toarray("U3RyaW5n0KHR0tGA0L7Rh9C60LA"));
print empty_string = base64_encode_fromarray(empty_bytes_array)
```

|empty_string|
|---|
||
