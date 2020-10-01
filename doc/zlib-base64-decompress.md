---
title: zlib_decompress_from_base64_string() - Azure Data Explorer 
description: This article describes the zlib_decompress_from_base64_string() command in Azure Data Explorer.
services: data-explorer
author: orspod
ms.author: orspodek
ms.reviewer: elgevork
ms.service: data-explorer
ms.topic: reference
ms.date: 09/29/2020
---
# zlib_decompress_from_base64_string()

Decodes the input string from base64 and performs zlib decompression.

> [!NOTE]
> The only supported windows size is 15.

## Syntax

`zlib_decompress_from_base64_string('input_string')`

## Arguments

*input_string*: Input `string` that was compressed with zlib and then base64-encoded. The function accepts one string argument.

## Returns

* Returns a `string` that represents the original string. 
* Returns an empty result if decompression or decoding failed. 
    * For example, invalid zlib-compressed and base 64-encoded strings will return an empty output.

## Examples

```kusto
print zcomp = zlib_decompress_from_base64_string("eJwLSS0uUSguKcrMS1cwNDIGACxqBQ4=")
```

**Output:**

|Test string 123|

Example of invalid input:

```kusto
print zcomp = zlib_decompress_from_base64_string("x0x0x0")
```

**Output:**
||

## Next steps

Create a compressed input string with [zlib_compress_to_base64_string()](zlib-base64-compress.md).