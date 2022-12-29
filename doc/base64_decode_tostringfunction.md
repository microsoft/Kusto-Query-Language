---
title: base64_decode_tostring() - Azure Data Explorer
description: Learn how to use a base64_decode_tostring() function to decode a base64 string into a UTF-8 string. 
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
---
# base64_decode_tostring()

Decodes a base64 string to a UTF-8 string.

> **Deprecated aliases:** base64_decodestring()

## Syntax

`base64_decode_tostring(`*base64_string*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *base64_string* | string | &check; | The value to decode from base64 to UTF-8 string. |

## Returns

Returns UTF-8 string decoded from base64 string.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUQgszcxLtU1KLE41M4lPSU3OT0mNL8kvLgFKpmsoBRuHVaW4W9gqaQIAN0l1sy4AAAA=" target="_blank">Run the query</a>

```kusto
print Quine=base64_decode_tostring("S3VzdG8=")
```

**Output**

|Quine|
|-----|
|Kusto|

Trying to decode a base64 string that was generated from invalid UTF-8 encoding will return null:

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUXDNLSiptE1KLE41M4lPSU3OT0mNL8kvLgFKpmsohRoHVSaGm+YZeHsEGZS4Oxr4mAdlWDqbGfg42ippAgCBCpEtQgAAAA==" target="_blank">Run the query</a>

```kusto
print Empty=base64_decode_tostring("U3RyaW5n0KHR0tGA0L7Rh9C60LA=")
```

**Output**

|Empty|
|-----|
||

## See also

* To decode base64 strings to an array of long values, see [base64_decode_toarray()](base64_decode_toarrayfunction.md)
* To encode strings to base64 string, see [base64_encode_tostring()](base64_encode_tostringfunction.md)
