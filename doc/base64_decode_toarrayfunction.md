---
title:  base64_decode_toarray()
description: Learn how to use the base64_decode_toarray() function to decode a base64 string into an array of long values.
ms.reviewer: alexans
ms.topic: reference
ms.date: 11/23/2022
---
# base64_decode_toarray()

Decodes a base64 string to an array of long values.

## Syntax

`base64_decode_toarray(`*base64_string*`)`

## Parameters

| Name | Type | Required | Description |
| -- | -- | -- | -- |
| *base64_string* | string | &check; |  The value to decode from base64 to an array of long values.|

## Returns

Returns an array of long values decoded from a base64 string.

## Example

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUQgszcxLtU1KLE41M4lPSU3OT0mNL8lPLCpKrNRQCjYOq0pxt7BV0lRQ4NLXV1D3VtdRUC8FEcUgogRE5KsDAAf/Q9pKAAAA" target="_blank">Run the query</a>

```kusto
print Quine=base64_decode_toarray("S3VzdG8=")  
// 'K', 'u', 's', 't', 'o'
```

**Output**

|Quine|
|-----|
|[75,117,115,116,111]|

## See also

* To decode base64 strings to a UTF-8 string, see [base64_decode_tostring()](base64_decode_tostringfunction.md)
* To encode strings to a base64 string, see [base64_encode_tostring()](base64_encode_tostringfunction.md)
