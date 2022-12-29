---
title: base64_encode_tostring() - Azure Data Explorer
description: This article describes base64_encode_tostring() in Azure Data Explorer.
ms.reviewer: alexans
ms.topic: reference
ms.date: 06/22/2019
---
# base64_encode_tostring()

Encodes a string as base64 string.

> **Deprecated aliases:** base64_encodestring()

## Syntax

`base64_encode_tostring(`*string*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *string* | string | &check; |  The value to encode as a base64 string. |

## Returns

Returns *string* encoded as a base64 string.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUQgszcxLtU1KLE41M4lPzUvOT0mNL8kvLgFKpmsoeZcWl+QraQIAKEgGNSsAAAA=" target="_blank">Run the query</a>

```kusto
print Quine=base64_encode_tostring("Kusto")
```

**Output**

|Quine   |
|--------|
|S3VzdG8=|

## See also

* To decode base64 strings to UTF-8 strings, see [base64_decode_tostring()](base64_decode_tostringfunction.md).
* To decode base64 strings to an array of long values, see [base64_decode_toarray()](base64_decode_toarrayfunction.md).
