---
title: zlib_decompress_from_base64_string() - Azure Data Explorer 
description: This article describes the zlib_decompress_from_base64_string() command in Azure Data Explorer.
ms.reviewer: elgevork
ms.topic: reference
ms.date: 02/15/2023
---
# zlib_decompress_from_base64_string()

Decodes the input string from base64 and performs zlib decompression.

> [!NOTE]
> The only supported windows size is 15.

## Syntax

`zlib_decompress_from_base64_string(`*string*`)`

## Parameters

| Name | Type | Required | Description |
|--|--|--|--|
| *string* | string | &check; | The string to decode. The string should have been compressed with zlib and then base64-encoded.|

## Returns

* Returns a `string` that represents the original string. 
* Returns an empty result if decompression or decoding failed. 
    * For example, invalid zlib-compressed and base 64-encoded strings will return an empty output.

## Examples

### Valid input

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUahKzs8tULBVqMrJTIpPSQXxilKLi+PTivJz45MSi1PNTOKLS4BK0zWUUr3KfYKDDUpDg9NLvZOLfIMNk8v9XDzdHZ0rCp0CTWyVNAGxo0NZVAAAAA==" target="_blank">Run the query</a>

```kusto
print zcomp = zlib_decompress_from_base64_string("eJwLSS0uUSguKcrMS1cwNDIGACxqBQ4=")
```

**Output**

|zcomp|
|--|
|Test string 123|

### Invalid input

> [!div class="nextstepaction"]
> <a href="https://dataexplorer.azure.com/clusters/help/databases/Samples?query=H4sIAAAAAAAAAysoyswrUahKzs8tULBVqMrJTIpPSQXxilKLi+PTivJz45MSi1PNTOKLS4BK0zWUKgxAUEkTAB50Ccs6AAAA" target="_blank">Run the query</a>

```kusto
print zcomp = zlib_decompress_from_base64_string("x0x0x0")
```

**Output**

|zcomp|
|--|
||

## Next steps

Create a compressed input string with [zlib_compress_to_base64_string()](zlib-base64-compress.md).