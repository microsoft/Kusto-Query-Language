---
title: base64_encode_fromarray() - Azure Data Explorer
description: Learn how to use the base64_encode_fromarray() function to encode a base64 string from a bytes array.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
---
# base64_encode_fromarray()

Encodes a base64 string from a bytes array.

## Syntax

`base64_encode_fromarray(`*base64_string_decoded_as_array*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *base64_string_decoded_as_array* | dynamic | &check; | The bytes array to be encoded into a base64 string. |

## Returns

Returns the base64 string encoded from the bytes array.

## Examples

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFIqixJLY5PLCpKrFSwVSjJL05OzEks0igoyswDSiYWp5qZxKekJuenpMaX5IOVaSgFG4dVpbhb2CppalpzQVRClKTEQ3UUlwBF04EGQvmpeWAT0orycyFmIFmrCQCPOEFEhwAAAA==" target="_blank">Run the query</a>

```kusto
let bytes_array = toscalar(print base64_decode_toarray("S3VzdG8="));
print decoded_base64_string = base64_encode_fromarray(bytes_array)
```

**Output**

|decoded_base64_string|
|---|
|S3VzdG8=|

Trying to encode a base64 string from an invalid bytes array that was generated from invalid UTF-8 encoded string will return null:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAA8tJLVFIzS0oqYxPqixJLY5PLCpKrFSwVSjJL05OzEks0igoyswrUUhKLE41M4lPSU3OT0mNL8kHK9NQCjUOqkwMN80z8PYIMihxdzTwMQ/KsHQ2M/BxVNLUtOaCaIaYX1wC5KQDjYaalZoHNiutKD8XYhqGMzQBTXW2Jp0AAAA=" target="_blank">Run the query</a>

```kusto
let empty_bytes_array = toscalar(print base64_decode_toarray("U3RyaW5n0KHR0tGA0L7Rh9C60LA"));
print empty_string = base64_encode_fromarray(empty_bytes_array)
```

**Output**

|empty_string|
|---|
||

## See also

* For decoding base64 strings to a UTF-8 string, see [base64_decode_tostring()](base64_decode_tostringfunction.md)
* For encoding strings to a base64 string see [base64_encode_tostring()](base64_encode_tostringfunction.md)
* This function is the inverse of [base64_decode_toarray()](base64_decode_toarrayfunction.md)
